
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.Globalization;
using System.Xml;

namespace Tibialyzer {
    public partial class MainForm : Form {
        public static MainForm mainForm;

        public static bool prevent_settings_update = false;
        private bool minimize_notification = true;
        private ToolTip scan_tooltip = new ToolTip();
        public static StreamWriter fileWriter = null;

        private static List<TabInterface> Tabs = new List<TabInterface>();

        public static void ExitWithError(string title, string text, bool exit = true) {
            MessageBox.Show(mainForm, text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (exit) {
                System.Environment.Exit(1);
            }
        }

        public MainForm() {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            mainForm = this;
            InitializeComponent();

            SettingsManager.LoadSettings(Constants.SettingsFile);

            LootDatabaseManager.LootChanged += NotificationManager.UpdateLootDisplay;
            LootDatabaseManager.LootChanged += UpdateLogDisplay;

            if (!File.Exists(Constants.DatabaseFile)) {
                ExitWithError("Fatal Error", String.Format("Could not find database file {0}.", Constants.DatabaseFile));
            }

            if (!File.Exists(Constants.NodeDatabase)) {
                ExitWithError("Fatal Error", String.Format("Could not find database file {0}.", Constants.NodeDatabase));
            }

            LootDatabaseManager.Initialize();
            StyleManager.InitializeStyle();
            NotificationForm.Initialize();
            Parser.Initialize();
            PopupManager.Initialize(this.notifyIcon1);

            prevent_settings_update = true;
            try {
                StorageManager.InitializeStorage();
            } catch (Exception e) {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.\nMessage: {1}", Constants.DatabaseFile, e.Message));
            }
            ProcessManager.Initialize();
            this.initializeSettings();
            this.initializeTooltips();
            try {
                Pathfinder.LoadFromDatabase(Constants.NodeDatabase);
            } catch (Exception e) {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.\nMessage: {1}", Constants.NodeDatabase, e.Message));
            }
            prevent_settings_update = false;

            this.InitializeTabs();
            switchTab(0);
            makeDraggable(this.Controls);

            if (SettingsManager.getSettingBool("StartAutohotkeyAutomatically")) {
                AutoHotkeyManager.StartAutohotkey();
            }
            ReadMemoryManager.Initialize();
            HuntManager.Initialize();

            this.Load += MainForm_Load;

            fileWriter = new StreamWriter(Constants.BigLootFile, true);

            tibialyzerLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);

            ScanningManager.StartScanning();

            scan_tooltip.AutoPopDelay = 60000;
            scan_tooltip.InitialDelay = 500;
            scan_tooltip.ReshowDelay = 0;
            scan_tooltip.ShowAlways = true;
            scan_tooltip.UseFading = true;

            SetScanningImage("scanningbar-red.gif", "No Tibia Client Found...", true);
        }

        private void MainForm_Load(object sender, EventArgs e) {
            (Tabs[10] as HelpTab).LoadHelpTab();
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public void SetScanningImage(string image, string text, bool enabled) {
            this.loadTimerImage.Image = StyleManager.GetImage(image);
            scan_tooltip.SetToolTip(this.loadTimerImage, text);
            this.loadTimerImage.Enabled = enabled;
        }

        public static ToolTip CreateTooltip() {
            ToolTip tooltip = new ToolTip();
            tooltip.AutoPopDelay = 60000;
            tooltip.InitialDelay = 500;
            tooltip.ReshowDelay = 0;
            tooltip.ShowAlways = true;
            tooltip.UseFading = true;
            return tooltip;
        }

        private void initializeTooltips() {

            /*explanationTooltip.SetToolTip(alwaysShowLoot, "When this box is checked, a rich notification is shown every time a creature is killed with the loot of the creature, regardless of what that loot is.");
            explanationTooltip.SetToolTip(rareDropNotificationValueCheckbox, "When an item that is worth at least this amount of gold drops, a notification is displayed.");
            explanationTooltip.SetToolTip(goldCapRatioCheckbox, "When an item that has at least this gold/cap ratio drops, a notification is displayed.");*/
            //explanationTooltip.SetToolTip(specificNotificationCheckbox, "When any item that is specified in the box below drops, a notification is displayed informing you of the dropped item.");
            //explanationTooltip.SetToolTip(notificationLengthSlider, "The amount of time that rich notifications (loot@, creature@) remain on the screen before fading.");




        }

        public void UpdateLogDisplay() {
            if (logButton.Enabled == false) {
                refreshHuntLog(getSelectedHunt());
            }
        }

        public void InitializeHuntDisplay(int activeHuntIndex) {
            (Tabs[2] as HuntsTab).InitializeHuntDisplay(activeHuntIndex);
        }

        public void initializeSettings() {
            SettingsManager.ApplyDefaultSettings();
            // convert legacy settings
            bool legacy = false;
            if (SettingsManager.settingExists("NotificationGoldRatio") || SettingsManager.settingExists("NotificationValue")) {
                // convert old notification conditions to new SQL conditions
                List<string> conditions = new List<string>();
                if (SettingsManager.settingExists("NotificationValue") && SettingsManager.getSettingBool("ShowNotificationsValue")) {
                    double value = SettingsManager.getSettingDouble("NotificationValue");
                    conditions.Add(String.Format("item.value >= {0}", value.ToString(CultureInfo.InvariantCulture)));
                }
                if (SettingsManager.settingExists("NotificationGoldRatio") && SettingsManager.getSettingBool("ShowNotificationsGoldRatio")) {
                    double value = SettingsManager.getSettingDouble("NotificationGoldRatio");
                    conditions.Add(String.Format("item.value / item.capacity >= {0}", value.ToString(CultureInfo.InvariantCulture)));
                }
                if (SettingsManager.getSettingBool("AlwaysShowLoot")) {
                    conditions.Add("1");
                }
                SettingsManager.removeSetting("NotificationGoldRatio");
                SettingsManager.removeSetting("NotificationValue");
                SettingsManager.removeSetting("ShowNotificationsGoldRatio");
                SettingsManager.removeSetting("ShowNotificationsValue");
                SettingsManager.removeSetting("AlwaysShowLoot");
                SettingsManager.setSetting("NotificationConditions", conditions);
                legacy = true;
            }
            if (SettingsManager.settingExists("NotificationDuration")) {
                int notificationLength = SettingsManager.getSettingInt("NotificationDuration") < 0 ? 30 : SettingsManager.getSettingInt("NotificationDuration");
                int anchor = Math.Min(Math.Max(SettingsManager.getSettingInt("RichNotificationAnchor"), 0), 3);
                int xOffset = SettingsManager.getSettingInt("RichNotificationXOffset") == -1 ? 30 : SettingsManager.getSettingInt("RichNotificationXOffset");
                int yOffset = SettingsManager.getSettingInt("RichNotificationYOffset") == -1 ? 30 : SettingsManager.getSettingInt("RichNotificationYOffset");
                foreach (string obj in Constants.NotificationTypes) {
                    string settingObject = obj.Replace(" ", "");
                    SettingsManager.setSetting(settingObject + "Anchor", anchor);
                    SettingsManager.setSetting(settingObject + "XOffset", xOffset);
                    SettingsManager.setSetting(settingObject + "YOffset", yOffset);
                    SettingsManager.setSetting(settingObject + "Duration", notificationLength);
                    SettingsManager.setSetting(settingObject + "Group", 0);
                }
                SettingsManager.removeSetting("NotificationDuration");
                SettingsManager.removeSetting("RichNotificationAnchor");
                SettingsManager.removeSetting("RichNotificationXOffset");
                SettingsManager.removeSetting("RichNotificationYOffset");
                legacy = true;
            }
            if (legacy) {
                // legacy settings had "#" as comment symbol in AutoHotkey text, replace that with the new comment symbol ";"
                List<string> newAutoHotkeySettings = new List<string>();
                foreach (string str in SettingsManager.getSetting("AutoHotkeySettings")) {
                    newAutoHotkeySettings.Add(str.Replace('#', ';'));
                }
                SettingsManager.setSetting("AutoHotkeySettings", newAutoHotkeySettings);

                SettingsManager.setSetting("ScanSpeed", Math.Min(Math.Max(SettingsManager.getSettingInt("ScanSpeed") + 5, (Tabs[1] as SettingsTab).MinimumScanSpeed()), (Tabs[1] as SettingsTab).MaximumScanSpeed()));
            }

            foreach (TabInterface tab in Tabs) {
                tab.InitializeSettings();
            }
        }

        void makeDraggable(Control.ControlCollection controls) {
            foreach (Control c in controls) {
                if ((c is Label && !c.Name.ToLower().Contains("button")) || c is Panel) {
                    c.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);
                }
                if (c is Panel || c is TabPage || c is TabControl) {
                    makeDraggable(c.Controls);
                }
            }
        }

        private string lastWarning;
        public void DisplayWarning(string message) {
            warningImageBox.Visible = true;
            if (lastWarning != message) {
                explanationTooltip.SetToolTip(warningImageBox, message);
                lastWarning = message;
            }
        }

        public void ClearWarning(string message) {
            if (lastWarning == message) {
                warningImageBox.Visible = false;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static Pen pathPen = new Pen(StyleManager.PathFinderPathColor, 3);
        public static MapPictureBox DrawRoute(Coordinate begin, Coordinate end, Size pictureBoxSize, Size minSize, Size maxSize, List<Color> additionalWalkableColors, List<Target> targetList = null) {
            if (end.x >= 0 && begin.z != end.z) {
                throw new Exception("Can't draw route with different z-coordinates");
            }
            Rectangle sourceRectangle;
            MapPictureBox pictureBox = new MapPictureBox();
            if (pictureBoxSize.Width != 0) {
                pictureBox.Size = pictureBoxSize;
            }
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            if (targetList != null) {
                foreach (Target target in targetList) {
                    pictureBox.targets.Add(target);
                }
                if (end.x < 0) {
                    if (pictureBoxSize.Width == 0) {
                        pictureBoxSize = new Size(Math.Min(Math.Max(end.z, minSize.Width), maxSize.Width),
                            Math.Min(Math.Max(end.z, minSize.Height), maxSize.Height));
                        pictureBox.Size = pictureBoxSize;
                    }
                    Map map = StorageManager.getMap(begin.z);
                    pictureBox.map = map;
                    pictureBox.sourceWidth = end.z;
                    pictureBox.mapCoordinate = new Coordinate(begin.x, begin.y, begin.z);
                    pictureBox.zCoordinate = begin.z;
                    pictureBox.UpdateMap();
                    return pictureBox;
                }

            }

            // First find the route at a high level
            Node beginNode = Pathfinder.GetNode(begin.x, begin.y, begin.z);
            Node endNode = Pathfinder.GetNode(end.x, end.y, end.z);

            List<Rectangle> collisionBounds = null;
            DijkstraNode highresult = Dijkstra.FindRoute(beginNode, endNode);
            if (highresult != null) {
                collisionBounds = new List<Rectangle>();
                while (highresult != null) {
                    highresult.rect.Inflate(5, 5);
                    collisionBounds.Add(highresult.rect);
                    highresult = highresult.previous;
                }
                if (collisionBounds.Count == 0) collisionBounds = null;
            }

            Map m = StorageManager.getMap(begin.z);
            DijkstraPoint result = Dijkstra.FindRoute(m.image, new Point(begin.x, begin.y), new Point(end.x, end.y), collisionBounds, additionalWalkableColors);
            if (result == null) {
                throw new Exception("Couldn't find route.");
            }

            // create a rectangle from the result
            double minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            DijkstraPoint node = result;
            while (node != null) {
                if (node.point.X < minX) minX = node.point.X;
                if (node.point.Y < minY) minY = node.point.Y;
                if (node.point.X > maxX) maxX = node.point.X;
                if (node.point.Y > maxY) maxY = node.point.Y;
                node = node.previous;
            }

            minX -= 10;
            minY -= 10;
            maxX += 10;
            maxY += 10;

            int size = (int)Math.Max(maxX - minX, maxY - minY);
            sourceRectangle = new Rectangle((int)minX, (int)minY, size, size);
            if (pictureBoxSize.Width == 0) {
                pictureBoxSize = new Size(Math.Min(Math.Max(sourceRectangle.Width, minSize.Width), maxSize.Width),
                    Math.Min(Math.Max(sourceRectangle.Height, minSize.Height), maxSize.Height));
                pictureBox.Size = pictureBoxSize;
            }
            TibiaPath path = new TibiaPath();
            path.begin = new Coordinate(begin);
            path.end = new Coordinate(end);
            path.path = result;
            pictureBox.paths.Add(path);

            pictureBox.map = m;
            pictureBox.sourceWidth = size;
            pictureBox.mapCoordinate = new Coordinate(sourceRectangle.X + sourceRectangle.Width / 2, sourceRectangle.Y + sourceRectangle.Height / 2, begin.z);
            pictureBox.zCoordinate = begin.z;
            pictureBox.UpdateMap();

            return pictureBox;
        }

        public class PageInfo {
            public bool prevPage = false;
            public bool nextPage = false;
            public int startDisplay = 0;
            public int endDisplay = 0;
            public int currentPage = 0;
            public PageInfo(bool prevPage, bool nextPage) {
                this.prevPage = prevPage;
                this.nextPage = nextPage;
            }
        }

        enum HeaderType { Numeric = 0, String = 1 };
        private static IComparable CoerceTypes(IComparable value, HeaderType type) {
            if (type == HeaderType.Numeric) {
                string valueString = value.ToString();
                double dblVal;
                if (double.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out dblVal)) {
                    return dblVal;
                }
                return (double)-127;
            } else if (type == HeaderType.String) {
                return value.ToString();
            }
            return value;
        }

        public static int DisplayCreatureAttributeList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, out int maxwidth, Func<TibiaObject, string> tooltip_function = null, List<Control> createdControls = null, int page = 0, int pageitems = 20, PageInfo pageInfo = null, string extraAttribute = null, Func<TibiaObject, Attribute> attributeFunction = null, EventHandler headerSortFunction = null, string sortedHeader = null, bool desc = false, Func<TibiaObject, IComparable> extraSort = null, List<string> removedAttributes = null, bool conditional = false) {
            const int size = 24;
            const int imageSize = size - 4;
            // add a tooltip that displays the creature names
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            int currentPage = 0;
            if (pageInfo != null) {
                pageInfo.prevPage = page > 0;
            }
            int offset = 0;
            if (sortedHeader != "" && sortedHeader != null) {
                int hash = sortedHeader.GetHashCode();
                HeaderType type = HeaderType.String;
                foreach (TibiaObject obj in l) {
                    List<string> headers = conditional ? obj.GetConditionalHeaders() : obj.GetAttributeHeaders();
                    if (headers.Contains(sortedHeader)) {
                        IComparable value = conditional ? obj.GetConditionalHeaderValue(sortedHeader) : obj.GetHeaderValue(hash);
                        if (value is string) {
                            type = HeaderType.String;
                        } else {
                            type = HeaderType.Numeric;
                        }
                        break;
                    }
                }

                if (desc) {
                    if (sortedHeader == extraAttribute && extraSort != null) {
                        l = l.OrderByDescending(o => extraSort(o)).ToList();
                    } else {
                        l = l.OrderByDescending(o => CoerceTypes(conditional ? o.GetConditionalHeaderValue(sortedHeader) : o.GetHeaderValue(hash), type)).ToList();
                    }
                } else {
                    if (sortedHeader == extraAttribute && extraSort != null) {
                        l = l.OrderBy(o => extraSort(o)).ToList();
                    } else {
                        l = l.OrderBy(o => CoerceTypes(conditional ? o.GetConditionalHeaderValue(sortedHeader) : o.GetHeaderValue(hash), type)).ToList();
                    }
                }
            }
            int start = 0;
            List<TibiaObject> pageItems = new List<TibiaObject>();
            Dictionary<string, int> totalAttributes = new Dictionary<string, int>();
            foreach (TibiaObject cr in l) {
                if (offset > pageitems) {
                    if (page > currentPage) {
                        offset = 0;
                        currentPage += 1;
                    } else {
                        if (pageInfo != null) {
                            pageInfo.nextPage = true;
                        }
                        break;
                    }
                }
                if (currentPage == page) {
                    pageItems.Add(cr);
                } else {
                    start++;
                }
                offset++;
            }
            if (pageInfo != null) {
                pageInfo.startDisplay = start;
                pageInfo.endDisplay = start + pageItems.Count;
            }
            Dictionary<string, double> sortValues = new Dictionary<string, double>();
            foreach (TibiaObject obj in conditional ? l : pageItems) {
                List<string> headers = conditional ? obj.GetConditionalHeaders() : new List<string>(obj.GetAttributeHeaders());
                List<Attribute> attributes = conditional ? obj.GetConditionalAttributes() : obj.GetAttributes();
                if (extraAttribute != null) {
                    headers.Add(extraAttribute);
                    attributes.Add(attributeFunction(obj));
                }
                for (int i = 0; i < headers.Count; i++) {
                    string header = headers[i];
                    Attribute attribute = attributes[i];
                    if (!sortValues.ContainsKey(header)) {
                        sortValues.Add(header, i);
                    } else {
                        sortValues[header] = Math.Max(sortValues[header], i);
                    }
                    if (removedAttributes != null && removedAttributes.Contains(header)) continue;
                    int width = TextRenderer.MeasureText(header, StyleManager.TextFont).Width + 10;
                    if (attribute is StringAttribute || attribute is CommandAttribute) {
                        string text = attribute is StringAttribute ? (attribute as StringAttribute).value : (attribute as CommandAttribute).value;
                        width = Math.Max(TextRenderer.MeasureText(text, StyleManager.TextFont).Width, width);
                    } else if (attribute is ImageAttribute) {
                        width = Math.Max((attribute as ImageAttribute).value == null ? 0 : (attribute as ImageAttribute).value.Width, width);
                    } else if (attribute is BooleanAttribute) {
                        width = Math.Max(20, width);
                    } else {
                        throw new Exception("Unrecognized attribute.");
                    }
                    width = Math.Min(width, attribute.MaxWidth);
                    if (!totalAttributes.ContainsKey(header)) {
                        int headerWidth = TextRenderer.MeasureText(header, StyleManager.TextFont).Width;
                        totalAttributes.Add(header, Math.Max(headerWidth, width));
                    } else if (totalAttributes[header] < width) {
                        totalAttributes[header] = width;
                    }
                }
            }
            base_x += 24;
            maxwidth = base_x;
            List<string> keys = totalAttributes.Keys.ToList();
            if (conditional) {
                keys = keys.OrderBy(o => sortValues[o]).ToList();
            }
            // create header information
            int x = base_x;
            foreach (string k in keys) {
                int val = totalAttributes[k];
                Label label = new Label();
                label.Name = k;
                label.Text = k;
                label.Location = new Point(x, base_y);
                label.ForeColor = StyleManager.NotificationTextColor;
                label.Size = new Size(val, size);
                label.Font = StyleManager.TextFont;
                label.BackColor = Color.Transparent;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.BorderStyle = BorderStyle.FixedSingle;
                if (headerSortFunction != null)
                    label.Click += headerSortFunction;
                controls.Add(label);
                if (createdControls != null) {
                    createdControls.Add(label);
                }
                x += val;
                maxwidth += val;
            }
            maxwidth += 10;
            offset = 0;

            // create object information
            foreach (TibiaObject obj in pageItems) {
                List<string> headers = conditional ? obj.GetConditionalHeaders() : new List<string>(obj.GetAttributeHeaders());
                List<Attribute> attributes = conditional ? obj.GetConditionalAttributes() : obj.GetAttributes();
                if (extraAttribute != null) {
                    headers.Add(extraAttribute);
                    attributes.Add(attributeFunction(obj));
                }
                string command = obj.GetCommand();

                // Every row is rendered on a single picture box for performance reasons
                PictureBox picture;
                picture = new PictureBox();
                picture.Image = obj.GetImage();
                picture.Size = new Size(imageSize, imageSize);
                picture.SizeMode = PictureBoxSizeMode.Zoom;
                picture.Location = new Point(base_x - 24, size * (offset + 1) + base_y);
                picture.BackColor = Color.Transparent;
                if (obj.AsItem() != null) {
                    picture.BackgroundImage = StyleManager.GetImage("item_background.png");
                }
                if (createdControls != null) {
                    createdControls.Add(picture);
                }
                controls.Add(picture);
                if (tooltip_function == null) {
                    if (obj.AsItem() != null) {
                        value_tooltip.SetToolTip(picture, obj.AsItem().look_text);
                    } else {
                        value_tooltip.SetToolTip(picture, obj.GetName());
                    }
                } else {
                    value_tooltip.SetToolTip(picture, tooltip_function(obj));
                }
                x = base_x;
                foreach (string k in keys) {
                    int val = totalAttributes[k];
                    int index = headers.IndexOf(k);
                    if (index < 0) {
                        x += val;
                        continue;
                    }
                    Attribute attribute = attributes[index];
                    Control c;
                    if (attribute is StringAttribute || attribute is CommandAttribute) {
                        string text = attribute is StringAttribute ? (attribute as StringAttribute).value : (attribute as CommandAttribute).value;
                        Color color = attribute is StringAttribute ? (attribute as StringAttribute).color : (attribute as CommandAttribute).color;
                        // create label
                        Label label = new Label();
                        label.Text = text;
                        label.ForeColor = color;
                        label.Size = new Size(val, size);
                        label.Font = StyleManager.TextFont;
                        label.Location = new Point(x, size * (offset + 1) + base_y);
                        label.BackColor = Color.Transparent;
                        if (createdControls != null) {
                            createdControls.Add(label);
                        }
                        controls.Add(label);
                        c = label;
                    } else if (attribute is ImageAttribute || attribute is BooleanAttribute) {
                        // create picturebox
                        picture = new PictureBox();
                        picture.Image = (attribute is ImageAttribute) ? (attribute as ImageAttribute).value : ((attribute as BooleanAttribute).value ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png"));
                        picture.Size = new Size(imageSize, imageSize);
                        picture.SizeMode = PictureBoxSizeMode.Zoom;
                        picture.Location = new Point(x + (val - imageSize) / 2, size * (offset + 1) + base_y);
                        picture.BackColor = Color.Transparent;
                        if (createdControls != null) {
                            createdControls.Add(picture);
                        }
                        controls.Add(picture);
                        c = picture;
                    } else {
                        throw new Exception("Unrecognized attribute.");
                    }
                    if (attribute is CommandAttribute) {
                        c.Name = (attribute as CommandAttribute).command;
                    } else {
                        c.Name = obj.GetCommand();
                    }
                    c.Click += executeNameCommand;
                    if (tooltip_function == null) {
                        if (attribute is StringAttribute || attribute is CommandAttribute) {
                            string text = attribute is StringAttribute ? (attribute as StringAttribute).value : (attribute as CommandAttribute).value;
                            value_tooltip.SetToolTip(c, text);
                        } else {
                            value_tooltip.SetToolTip(c, obj.GetName());
                        }
                    } else {
                        value_tooltip.SetToolTip(c, tooltip_function(obj));
                    }
                    x += val;
                }

                offset++;
            }
            return (offset + 1) * size;
        }

        public static int DisplayCreatureList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, int max_x, int spacing, Func<TibiaObject, string> tooltip_function = null, float magnification = 1.0f, List<Control> createdControls = null, int page = 0, int pageheight = 10000, PageInfo pageInfo = null, int currentDisplay = -1) {
            int x = 0, y = 0;
            int height = 0;
            // add a tooltip that displays the creature names
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            int currentPage = 0;
            if (pageInfo != null) {
                pageInfo.prevPage = page > 0;
            }
            int start = 0, end = 0;
            int pageStart = 0;
            if (currentDisplay >= 0) {
                page = int.MaxValue;
            }
            for (int i = 0; i < l.Count; i++) {
                TibiaObject cr = l[i];
                int imageWidth;
                int imageHeight;
                Image image = cr.GetImage();
                string name = cr.GetName();

                if (cr.AsItem() != null || cr.AsSpell() != null) {
                    imageWidth = 32;
                    imageHeight = 32;
                } else {
                    imageWidth = image.Width;
                    imageHeight = image.Height;
                }

                if (currentDisplay >= 0 && i == currentDisplay) {
                    currentDisplay = -1;
                    i = pageStart;
                    start = i;
                    page = currentPage;
                    pageInfo.prevPage = page > 0;
                    pageInfo.currentPage = page;
                    x = 0;
                    y = 0;
                    continue;
                }

                if (max_x < (x + base_x + (int)(imageWidth * magnification) + spacing)) {
                    x = 0;
                    y = y + spacing + height;
                    height = 0;
                    if (y > pageheight) {
                        if (page > currentPage) {
                            y = 0;
                            currentPage += 1;
                            pageStart = start;
                        } else {
                            if (pageInfo != null) {
                                pageInfo.nextPage = true;
                            }
                            break;
                        }
                    }
                }
                if ((int)(imageHeight * magnification) > height) {
                    height = (int)(imageHeight * magnification);
                }
                if (currentPage == page) {
                    PictureBox image_box;
                    image_box = new PictureBox();
                    image_box.Image = image;
                    image_box.BackColor = Color.Transparent;
                    image_box.Size = new Size((int)(imageWidth * magnification), height);
                    image_box.Location = new Point(base_x + x, base_y + y);
                    image_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    image_box.Name = cr.GetCommand();
                    image_box.Click += executeNameCommand;
                    if (cr.AsItem() != null) {
                        image_box.BackgroundImage = StyleManager.GetImage("item_background.png");
                    }
                    controls.Add(image_box);
                    if (createdControls != null) createdControls.Add(image_box);
                    image_box.Image = image;
                    if (tooltip_function == null) {
                        value_tooltip.SetToolTip(image_box, name.ToTitle());
                    } else {
                        string prefix = "";
                        if (cr.AsNPC() != null) {
                            NPC npc = cr is NPC ? cr as NPC : (cr as LazyTibiaObject).getTibiaObject() as NPC;
                            prefix = name.ToTitle() + " (" + npc.city.ToTitle() + ")\n";
                        }
                        value_tooltip.SetToolTip(image_box, prefix + tooltip_function(cr));
                    }
                    end++;
                } else {
                    start++;
                }

                x = x + (int)(imageWidth * magnification) + spacing;
            }
            if (pageInfo != null) {
                pageInfo.startDisplay = start;
                pageInfo.endDisplay = start + end;
            }
            x = 0;
            y = y + height;
            return y;
        }

        public static void OpenUrl(string str) {
            // Weird command prompt escape characters
            str = str.Trim().Replace(" ", "%20").Replace("&", "^&").Replace("|", "^|").Replace("(", "^(").Replace(")", "^)");
            // Always start with http:// or https://
            if (!str.StartsWith("http://") && !str.StartsWith("https://")) {
                str = "http://" + str;
            }
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C start " + str);

            procStartInfo.UseShellExecute = true;

            // Do not show the cmd window to the user.
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(procStartInfo);
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == 0xC) {
                // This messages is send by AutoHotkey to execute a command
                string command = Marshal.PtrToStringUni(m.LParam);
                if (command != null) {
                    if (CommandManager.ExecuteCommand(command)) {
                        return; //if the passed along string is a command, we have executed it successfully
                    }
                }
            }
            if (m.Msg == 0x317) {
                // We intercept this message because this message signifies the AutoHotkey state (suspended or not)

                int wParam = m.WParam.ToInt32();
                if (wParam == 32) {
                    // 32 signifies we have entered suspended mode, so we warn the user with a popup
                    AutoHotkeyManager.ShowSuspendedWindow();
                } else if (wParam == 33) {
                    // 33 signifies we are not suspended, destroy the suspended window (if it exists)
                    AutoHotkeyManager.CloseSuspendedWindow();
                }
            }
            base.WndProc(ref m);
        }

        #region Tab Menu
        private List<Control> activeControls = new List<Control>();
        private List<List<Control>> tabControls = new List<List<Control>>();
        private void InitializeTabs() {
            Tabs = new List<TabInterface> { new MainTab(), new SettingsTab(), new HuntsTab(), new LogsTab(), new NotificationsTab(), new PopupsTab(), new DatabaseTab(), new AutoHotkeyTab(), new ScreenshotTab(), new BrowseTab(), new HelpTab(), new SystemTab() };
            foreach(TabInterface tab in Tabs) {
                List<Control> controlList = new List<Control>();
                foreach (Control c in (tab as Form).Controls) {
                    controlList.Add(c);
                    c.Location = new Point(c.Location.X + 101, c.Location.Y + 24);
                }
                tabControls.Add(controlList);
            }

            // Manually add controls that appear on multiple pages
            tabControls[3].Add((Tabs[2] as HuntsTab).GetHuntList());
            tabControls[3].Add((Tabs[2] as HuntsTab).GetHuntLabel());
        }

        private void switchTab(int tab) {
            foreach (Control c in activeControls) {
                this.Controls.Remove(c);
            }
            activeControls.Clear();
            foreach (Control c in tabControls[tab]) {
                activeControls.Add(c);
                this.Controls.Add(c);
            }

            mainButton.Enabled = true;
            generalButton.Enabled = true;
            huntButton.Enabled = true;
            logButton.Enabled = true;
            notificationButton.Enabled = true;
            popupButton.Enabled = true;
            databaseButton.Enabled = true;
            autoHotkeyButton.Enabled = true;
            screenshotButton.Enabled = true;
            browseButton.Enabled = true;
            helpButton.Enabled = true;
            upgradeButton.Enabled = true;
            switch (tab) {
                case 0:
                    mainButton.Enabled = false; break;
                case 1:
                    generalButton.Enabled = false; break;
                case 2:
                    huntButton.Enabled = false; break;
                case 3:
                    logButton.Enabled = false; break;
                case 4:
                    notificationButton.Enabled = false; break;
                case 5:
                    popupButton.Enabled = false; break;
                case 6:
                    databaseButton.Enabled = false; break;
                case 7:
                    autoHotkeyButton.Enabled = false; break;
                case 8:
                    screenshotButton.Enabled = false; break;
                case 9:
                    browseButton.Enabled = false; break;
                case 10:
                    helpButton.Enabled = false; break;
                case 11:
                    upgradeButton.Enabled = false; break;
            }
        }

        private void mainButton_Click(object sender, MouseEventArgs e) {
            switchTab(0);
        }

        private void generalButton_Click(object sender, MouseEventArgs e) {
            switchTab(1);
        }

        private void huntButton_Click(object sender, MouseEventArgs e) {
            switchTab(2);
        }

        private void logButton_Click(object sender, MouseEventArgs e) {
            switchTab(3);
            refreshHuntLog(getSelectedHunt());
        }

        private void notificationButton_Click(object sender, MouseEventArgs e) {
            switchTab(4);
        }

        private void popupButton_Click(object sender, MouseEventArgs e) {
            switchTab(5);
        }

        private void databaseButton_Click(object sender, MouseEventArgs e) {
            switchTab(6);
        }

        private void autoHotkeyButton_Click(object sender, MouseEventArgs e) {
            switchTab(7);
        }

        private void screenshotButton_Click(object sender, MouseEventArgs e) {
            switchTab(8);
        }

        private void browseButton_Click(object sender, MouseEventArgs e) {
            switchTab(9);
        }

        private void helpButton_Click(object sender, MouseEventArgs e) {
            switchTab(10);
        }

        private void upgradeButton_Click(object sender, EventArgs e) {
            switchTab(11);
        }
        #endregion

        #region Main

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (SettingsManager.getSettingBool("ShutdownAutohotkeyOnExit")) {
                AutoHotkeyManager.ShutdownAutohotkey();
            }
            if (fileWriter != null) {
                fileWriter.Close();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            notifyIcon1.Visible = false;
        }

        public void draggable_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private static void executeNameCommand(object sender, EventArgs e) {
            CommandManager.ExecuteCommand((sender as Control).Name);
        }

        private void closeButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void minimizeButton_Click(object sender, EventArgs e) {
            this.Hide();
            this.minimizeIcon.Visible = true;
            if (minimize_notification) {
                this.minimize_notification = false;
                this.minimizeIcon.ShowBalloonTip(3000);
            }
        }

        private void closeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.CloseButtonHoverColor;
        }

        private void closeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.CloseButtonNormalColor;
        }

        private void minimizeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MinimizeButtonHoverColor;
        }

        private void minimizeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MinimizeButtonNormalColor;
        }

        private void minimizeIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.minimizeIcon.Visible = false;
            this.Show();
        }

        private void mainButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void mainButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

        private void warningImageBox_MouseDown(object sender, MouseEventArgs e) {
            (sender as Control).Visible = false;
        }
        #endregion

        public void refreshHunts() {
            (Tabs[2] as HuntsTab).refreshHunts();
        }

        public Hunt getSelectedHunt() {
            PrettyListBox huntList = (Tabs[2] as HuntsTab).GetHuntList();
            if (huntList.SelectedIndex < 0) return null;
            return HuntManager.GetHunt(huntList.SelectedIndex);
        }

        public void refreshHuntLog(Hunt h) {
            (Tabs[3] as LogsTab).refreshHuntLog(h);
        }

        public bool skip_hunt_refresh = false;
        public bool switch_hunt = false;

        public void refreshScreenshots() {
            (Tabs[8] as ScreenshotTab).refreshScreenshots();
        }

        public IEnumerable<SystemCommand> GetCustomCommands() {
            return (Tabs[11] as SystemTab).GetCustomCommands();
        }
    }
}