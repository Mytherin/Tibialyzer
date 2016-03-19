
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
            GlobalDataManager.ExperienceChanged += NotificationManager.UpdateExperienceDisplay;
            GlobalDataManager.DamageChanged += NotificationManager.UpdateDamageDisplay;
            GlobalDataManager.UsedItemsChanged += NotificationManager.UpdateUsedItemsDisplay;

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
            UIManager.Initialize();
            MemoryReader.Initialize();

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
                if ((c is Label && !c.Name.Contains("button", StringComparison.OrdinalIgnoreCase)) || c is Panel) {
                    c.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);
                }
                if (c is Panel || c is TabPage || c is TabControl) {
                    makeDraggable(c.Controls);
                }
            }
        }

        private string lastWarning;
        public void DisplayWarning(string message) {
            try {
                warningImageBox.Visible = true;
                if (lastWarning != message) {
                    explanationTooltip.SetToolTip(warningImageBox, message);
                    lastWarning = message;
                }
            } catch(Exception ex) {
                Console.WriteLine(String.Format("Failed to display warning \"{0}\" with error \"{1}\"", message, ex.Message));
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
            Tabs = new List<TabInterface> { new MainTab(), new SettingsTab(), new HuntsTab(), new LogsTab(), new NotificationsTab(), new PopupsTab(), new DatabaseTab(), new AutoHotkeyTab(), new ScreenshotTab(), new BrowseTab(), new HelpTab(), new SystemTab(), new SummaryTab() };
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
            summaryButton.Enabled = true;
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
                case 12:
                    summaryButton.Enabled = false; break;
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

        private void summaryButton_Click(object sender, EventArgs e) {
            switchTab(12);
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