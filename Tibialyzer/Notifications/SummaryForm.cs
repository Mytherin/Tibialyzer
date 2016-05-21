using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {

    public partial class SummaryForm : NotificationForm {
        public struct ItemRegion {
            public TibiaObject item;
            public Rectangle region;
        }
        private int BlockWidth = 200;
        private int BlockHeight = 25;
        private object updateLock = new object();
        private List<ItemRegion>[] lootRegions = new List<ItemRegion>[20];
        private List<ItemRegion>[] wasteRegions = new List<ItemRegion>[20];
        private List<ItemRegion>[] recentDropsRegions = new List<ItemRegion>[20];
        private ToolTip tooltip;

        public SummaryForm() {
            InitializeComponent();
            tooltip = UIManager.CreateTooltip();
            this.Name = "Tibialyzer (Summary Form)";
        }

        public static void RenderText(Graphics gr, string text, int x, Color fillColor, Color textColor, Color traceColor, int maxHeight = -1, int y = 4, Font font = null, bool center = false, InterpolationMode interpolationMode = InterpolationMode.High, SmoothingMode smoothingMode = SmoothingMode.HighQuality) {
            gr.InterpolationMode = interpolationMode;
            gr.SmoothingMode = smoothingMode;
            gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            gr.CompositingQuality = CompositingQuality.HighQuality;
            FontFamily family = FontFamily.GenericMonospace;
            FontStyle fontStyle = FontStyle.Bold;
            float fontSize = 10;
            if (font != null) {
                family = font.FontFamily;
                fontStyle = font.Style;
                fontSize = font.SizeInPoints;
            }
            GraphicsPath p = new GraphicsPath();
            p.AddString(
                text,
                family,
                (int)fontStyle,
                gr.DpiY * fontSize / 72,
                new Point(x, y),
                new StringFormat());
            if (x < 0 || center) {
                if (x < 0) {
                    x = (int)(Math.Abs(x) - p.GetBounds().Width - 10);
                }
                if (center) {
                    y = (int)((maxHeight - (p.GetBounds().Height + p.GetBounds().Y)) / 2);
                }
                p = new GraphicsPath();
                p.AddString(
                    text,
                    family,
                    (int)fontStyle,
                    gr.DpiY * fontSize / 72,
                    new Point(x, y),
                    new StringFormat());
            }
            maxHeight = maxHeight < 0 ? (int)p.GetBounds().Height : maxHeight;
            if (fillColor != Color.Empty) {
                using (Brush brush = new SolidBrush(fillColor)) {
                    gr.FillRectangle(brush, new RectangleF(p.GetBounds().X - 8, 0, p.GetBounds().Width + 16, maxHeight - 1));
                }
                gr.DrawRectangle(Pens.Black, new Rectangle((int)p.GetBounds().X - 8, 0, (int)p.GetBounds().Width + 16, maxHeight - 1));
            }
            using (Pen pen = new Pen(traceColor, 2)) {
                gr.DrawPath(pen, p);
            }
            using (SolidBrush brush = new SolidBrush(textColor)) {
                gr.FillPath(brush, p);
            }
        }

        public static void RenderImageResized(Graphics gr, Image image, Rectangle targetRectangle) {
            if (image == null) return;
            lock (image) {
                int x = targetRectangle.X, y = targetRectangle.Y;
                int width = targetRectangle.Width, height = targetRectangle.Height;
                if (image.Width > image.Height) {
                    height = (int)Math.Floor(height * ((double)image.Height / image.Width));
                    y += (width - height) / 2;
                } else if (image.Height > image.Width) {
                    width = (int)Math.Floor(width * ((double)image.Width / image.Height));
                    x += (height - width) / 2;
                }
                gr.DrawImage(image, new Rectangle(x, y, width, height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
        }

        public Image CreatureBox(Creature creature, int amount = 0) {
            Bitmap bitmap = new Bitmap(BlockWidth, BlockHeight);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                Color backColor = StyleManager.GetElementColor(creature.GetStrength());
                using (Brush brush = new SolidBrush(backColor)) {
                    gr.FillRectangle(brush, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }
                gr.DrawRectangle(Pens.Black, new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1));
                RenderImageResized(gr, StyleManager.GetImage("item_background.png"), new Rectangle(1, 1, BlockHeight - 2, BlockHeight - 2));
                RenderImageResized(gr, creature.GetImage(), new Rectangle(1, 1, BlockHeight - 2, BlockHeight - 2));
                RenderText(gr, creature.displayname.ToTitle(), BlockHeight + 2, Color.Empty, StyleManager.NotificationTextColor, Color.Black, BlockHeight);
                if (amount > 0) {
                    RenderText(gr, amount.ToString(), -BlockWidth, Color.FromArgb(backColor.R / 2, backColor.G / 2, backColor.B / 2), StyleManager.NotificationTextColor, Color.Black, BlockHeight);
                }
            }
            return bitmap;
        }

        public Image SummaryBox(string header, string value, Color textColor) {
            Bitmap bitmap = new Bitmap(BlockWidth, BlockHeight);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                using (SolidBrush brush = new SolidBrush(StyleManager.MainFormButtonColor)) {
                    gr.FillRectangle(brush, new RectangleF(0, 0, BlockWidth, BlockHeight));
                }
                gr.DrawRectangle(Pens.Black, new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1));
                RenderText(gr, header, 0, Color.Empty, StyleManager.NotificationTextColor, Color.Black, BlockHeight);
                RenderText(gr, value, -BlockWidth, Color.FromArgb(StyleManager.MainFormButtonColor.R / 2, StyleManager.MainFormButtonColor.G / 2, StyleManager.MainFormButtonColor.B / 2), textColor, Color.Black, BlockHeight);
            }
            return bitmap;
        }

        public Image RecentDropsBox(Creature creature, List<Tuple<Item, int>> items, int imageHeight, List<ItemRegion> regions) {
            Bitmap bitmap = new Bitmap(BlockWidth, imageHeight);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                using (Brush brush = new SolidBrush(StyleManager.MainFormButtonColor)) {
                    gr.FillRectangle(brush, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }
                gr.DrawRectangle(Pens.Black, new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1));
                Rectangle creatureRegion = new Rectangle(1, 1, imageHeight - 1, imageHeight - 1);
                RenderImageResized(gr, StyleManager.GetImage("item_background.png"), new Rectangle(1, 1, imageHeight - 2, imageHeight - 2));
                RenderImageResized(gr, creature.GetImage(), creatureRegion);
                regions.Add(new ItemRegion { item = creature, region = creatureRegion });
                int count = 0;
                foreach (Tuple<Item, int> item in items) {
                    Rectangle region = new Rectangle(8 + (imageHeight - 1) * ++count, 1, imageHeight - 2, imageHeight - 2);
                    regions.Add(new ItemRegion { item = item.Item1, region = region });
                    RenderImageResized(gr, StyleManager.GetImage("item_background.png"), region);
                    RenderItemCount(gr, item, region);
                }
            }
            return bitmap;
        }

        private void CreateHeaderLabel(string title, int x, ref int y, List<Control> controls) {
            Label label = new Label();
            label.Text = title;
            label.Location = new Point(x, y);
            label.Size = new Size(BlockWidth, 15);
            label.BackColor = Color.Transparent;
            label.ForeColor = StyleManager.NotificationTextColor;
            label.Font = StyleManager.MainFormLabelFontSmall;
            this.Controls.Add(label);
            controls.Add(label);
            y += 15;
        }

        private PictureBox CreateSummaryLabel(string title, string value, int x, ref int y, Color color, List<Control> controls) {
            Image image = SummaryBox(title, value, color);
            PictureBox box = new PictureBox();
            box.Size = image.Size;
            box.BackColor = Color.Transparent;
            box.Location = new Point(x, y);
            box.Image = image;
            this.Controls.Add(box);
            controls.Add(box);
            y += box.Height;
            return box;
        }

        private void CreateCreatureBox(Creature creature, int count, int x, ref int y, List<Control> controls) {
            Image image = CreatureBox(creature, count);
            PictureBox box = new PictureBox();
            box.Size = image.Size;
            box.BackColor = Color.Transparent;
            box.Location = new Point(x, y);
            box.Image = image;
            box.Name = "creature" + Constants.CommandSymbol + creature.title;
            box.Click += CommandClick;
            this.Controls.Add(box);
            controls.Add(box);
            y += box.Height;
        }

        private PictureBox CreateCreatureDropsBox(Creature creature, List<Tuple<Item, int>> items, string message, int x, ref int y, List<Control> controls, int imageHeight, List<ItemRegion> region = null, int index = 0) {
            Image image = RecentDropsBox(creature, items, imageHeight, region);
            PictureBox box = new PictureBox();
            box.Size = image.Size;
            box.BackColor = Color.Transparent;
            box.Location = new Point(x, y);
            box.Image = image;
            box.Name = index.ToString();
            this.Controls.Add(box);
            controls.Add(box);
            // copy button
            PictureBox copyButton = new PictureBox();
            copyButton.Size = new Size(box.Size.Height - 4, box.Size.Height - 4);
            copyButton.BackColor = StyleManager.MainFormButtonColor;
            copyButton.Location = new Point(box.Location.X + box.Size.Width - box.Size.Height + 2, y + 2);
            copyButton.Click += CopyLootText;
            copyButton.Name = message;
            copyButton.Image = StyleManager.GetImage("copyicon.png");
            copyButton.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(copyButton);
            controls.Add(copyButton);
            copyButton.BringToFront();

            y += box.Height;
            return box;
        }

        private void CopyLootText(object sender, EventArgs e) {
            Clipboard.SetText((sender as Control).Name);
        }

        private static void RenderItemCount(Graphics gr, Tuple<Item, int> item, Rectangle region) {
            if (region.Width > 28) {
                RenderImageResized(gr, item.Item1.GetImage(), region);
                if (item.Item1.stackable || item.Item2 > 1) {
                    LootDropForm.DrawCountOnGraphics(gr, item.Item2, region.X + region.Width - 1, region.Y + region.Height - 1);
                }
            } else {
                RenderImageResized(gr, (item.Item1.stackable || item.Item2 > 1) ? LootDropForm.DrawCountOnItem(item.Item1, item.Item2) : item.Item1.GetImage(), region);
            }
        }

        private PictureBox CreateItemList(List<Tuple<Item, int>> items, int x, ref int y, List<Control> controls, int imageHeight, List<ItemRegion> newRegions = null, int boxIndex = 0) {
            Image image = new Bitmap(BlockWidth, imageHeight);
            using (Graphics gr = Graphics.FromImage(image)) {
                int counter = 0;
                foreach (Tuple<Item, int> item in items) {
                    Rectangle region = new Rectangle(x + (counter++) * (imageHeight + 1), 0, imageHeight - 1, imageHeight - 1);
                    if (newRegions != null) newRegions.Add(new ItemRegion { item = item.Item1, region = region });
                    RenderImageResized(gr, StyleManager.GetImage("item_background.png"), region);
                    RenderItemCount(gr, item, region);
                }
            }
            PictureBox box = new PictureBox();
            box.Size = image.Size;
            box.BackColor = Color.Transparent;
            box.Location = new Point(x, y);
            box.Image = image;
            box.Name = boxIndex.ToString();
            this.Controls.Add(box);
            controls.Add(box);
            y += box.Height;
            return box;
        }

        private void OpenItemWindow(object sender, MouseEventArgs e, List<ItemRegion>[] lootRegions) {
            Point mousePoint = new Point(e.X, e.Y);
            int index = 0;
            int.TryParse((sender as Control).Name, out index);
            if (index > lootRegions.Length && lootRegions[index] == null) return;
            List<ItemRegion> regions = lootRegions[index];
            foreach (ItemRegion reg in regions) {
                if (reg.region.Contains(mousePoint)) {
                    CommandManager.ExecuteCommand(reg.item.GetCommand());
                    return;
                }
            }
        }

        private void OpenLootWindow(object sender, MouseEventArgs e) {
            OpenItemWindow(sender, e, lootRegions);
        }
        private void OpenWasteWindow(object sender, MouseEventArgs e) {
            OpenItemWindow(sender, e, wasteRegions);
        }
        private void OpenRecentDropsWindow(object sender, MouseEventArgs e) {
            OpenItemWindow(sender, e, recentDropsRegions);
        }

        private int x = 5;
        private List<Control> summaryControls = new List<Control>();
        private List<Control> lootControls = new List<Control>();
        private List<Control> damageControls = new List<Control>();
        private List<Control> usedItemsControls = new List<Control>();
        public override void LoadForm() {
            this.SuspendForm();

            Label label;
            label = new Label();
            label.Text = "Summary";
            label.Location = new Point(x, 0);
            label.Size = new Size(BlockWidth, 30);
            label.BackColor = Color.Transparent;
            label.ForeColor = StyleManager.NotificationTextColor;
            label.Font = StyleManager.MainFormLabelFontSmall;
            label.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(label);

            this.NotificationInitialize();

            this.NotificationFinalize();
            this.ResumeForm();

            this.RefreshForm();
        }

        public void ClearControlList(List<Control> list, out int minheight, out int maxheight) {
            minheight = int.MaxValue;
            maxheight = int.MinValue;
            foreach (Control c in list) {
                if (c.Location.Y < minheight) {
                    minheight = c.Location.Y;
                }
                if (c.Location.Y + c.Height > maxheight) {
                    maxheight = c.Location.Y + c.Height;
                }
                this.Controls.Remove(c);
                c.Dispose();
            }
            list.Clear();
        }

        private long totalValue = 0;
        private long averageValue = 0;
        private long totalWaste = 0;
        public void UpdateSummaryForm() {
            int minheight, maxheight;
            ClearControlList(summaryControls, out minheight, out maxheight);
            int y = maxheight < 0 ? 30 : minheight;

            PictureBox loot = CreateSummaryLabel("Loot value (gp)", totalValue.ToString("N0"), x, ref y, StyleManager.ItemGoldColor, summaryControls);
            tooltip.SetToolTip(loot, String.Format("Average gold for these creature kills: {0} gold.", averageValue.ToString("N0")));
            CreateSummaryLabel("Exp gained", HuntManager.activeHunt.totalExp.ToString("N0"), x, ref y, StyleManager.NotificationTextColor, summaryControls);
            CreateSummaryLabel("Time", LootDropForm.TimeToString((long)HuntManager.activeHunt.totalTime), x, ref y, StyleManager.NotificationTextColor, summaryControls);
            CreateSummaryLabel("Supplies used (gp)", totalWaste.ToString("N0"), x, ref y, StyleManager.WasteColor, summaryControls);
            long profit = totalValue - totalWaste;
            CreateSummaryLabel(profit > 0 ? "Profit (gp)" : "Waste (gp)", profit.ToString("N0"), x, ref y, profit > 0 ? StyleManager.ItemGoldColor : StyleManager.WasteColor, summaryControls);
            if (ScanningManager.lastResults != null) {
                CreateSummaryLabel("Exp/hour", ScanningManager.lastResults.expPerHour.ToString("N0"), x, ref y, StyleManager.NotificationTextColor, summaryControls);
            }
        }

        public void UpdateLoot() {
            try {
                lock (updateLock) {
                    this.Invoke((MethodInvoker)delegate {
                        this.SuspendForm();
                        this.UpdateLootForm();
                        this.UpdateSummaryForm();
                        this.ResumeForm();
                    });
                }
            } catch {

            }
        }

        public void UpdateDamage() {
            try {
                if (this.IsDisposed) return;
                this.Invoke((MethodInvoker)delegate {
                    try {
                        lock (updateLock) {
                            this.SuspendForm();
                            this.UpdateDamageForm();
                            this.ResumeForm();
                        }
                    } catch {

                    }
                });
            } catch {

            }
        }

        public void UpdateWaste() {
            try {
                if (this.IsDisposed) return;
                this.Invoke((MethodInvoker)delegate {
                    try {
                        lock (updateLock) {
                            this.SuspendForm();
                            this.UpdateWasteForm();
                            this.UpdateSummaryForm();
                            this.ResumeForm();
                        }
                    } catch {

                    }
                });
            } catch {

            }
        }

        public void UpdateLootForm() {
            Hunt hunt = HuntManager.activeHunt;
            int minheight, maxheight;
            ClearControlList(lootControls, out minheight, out maxheight);

            int counter;
            int y = minheight;
            if (maxheight < 0) {
                y = 30;
                foreach (Control c in summaryControls) {
                    y = Math.Max(c.Location.Y + c.Height, y);
                }
            }
            var loot = LootDropForm.GenerateLootInformation(hunt, "", null);
            totalValue = 0;
            foreach (Tuple<Item, int> tpl in loot.Item2) {
                totalValue += tpl.Item1.GetMaxValue() * tpl.Item2;
            }

            averageValue = LootDropForm.GetAverageGold(loot.Item1);

            int maxDrops = SettingsManager.getSettingInt("SummaryMaxItemDrops");
            if (maxDrops < 0) maxDrops = 5;
            if (maxDrops > 0) {
                List<ItemRegion> region;
                int imageHeight = SettingsManager.getSettingInt("SummaryLootItemSize");
                imageHeight = imageHeight < 0 ? BlockHeight : imageHeight;

                CreateHeaderLabel("Item Drops", x, ref y, lootControls);
                counter = 0;
                bool display = true;
                int width = 0;
                var items = new List<Tuple<Item, int>>();
                foreach (Tuple<Item, int> tpl in loot.Item2) {
                    int amount = tpl.Item2;
                    while (amount > 0) {
                        int count = Math.Min(100, amount);
                        amount -= count;
                        items.Add(new Tuple<Item, int>(tpl.Item1, count));
                        width += imageHeight + 2;
                        if (width > BlockWidth - imageHeight) {
                            region = new List<ItemRegion>();
                            CreateItemList(items, x, ref y, lootControls, imageHeight, region, counter).MouseDown += OpenLootWindow;
                            lootRegions[counter] = region;
                            items.Clear();
                            width = 0;
                            if (++counter >= maxDrops) {
                                display = false;
                                break;
                            }
                        }
                    }
                    if (!display) break;
                }
                if (items.Count > 0) {
                    region = new List<ItemRegion>();
                    CreateItemList(items, x, ref y, lootControls, imageHeight, region, counter).MouseDown += OpenLootWindow;
                    lootRegions[counter] = region;
                    items.Clear();
                }
            }
            int maxCreatures = SettingsManager.getSettingInt("SummaryMaxCreatures");
            if (maxCreatures < 0) maxCreatures = 5;
            if (maxCreatures > 0) {
                CreateHeaderLabel("Creature Kills", x, ref y, lootControls);
                counter = 0;
                foreach (Creature cr in loot.Item1.Keys.OrderByDescending(o => loot.Item1[o] * (1 + o.experience)).ToList<Creature>()) {
                    CreateCreatureBox(cr, loot.Item1[cr], x, ref y, lootControls);
                    if (++counter >= maxCreatures) break;
                }
            }
            int maxRecentDrops = SettingsManager.getSettingInt("SummaryMaxRecentDrops");
            if (maxRecentDrops < 0) maxRecentDrops = 5;
            if (maxRecentDrops > 0) {
                CreateHeaderLabel("Recent Drops", x, ref y, lootControls);
                int imageHeight = SettingsManager.getSettingInt("SummaryRecentDropsItemSize");
                imageHeight = imageHeight < 0 ? BlockHeight : imageHeight;
                var recentDrops = ScanningManager.GetRecentDrops(maxRecentDrops);
                int index = 0;
                foreach (var drops in recentDrops) {
                    List<ItemRegion> region = new List<ItemRegion>();
                    CreateCreatureDropsBox(drops.Item1, drops.Item2, drops.Item3, x, ref y, lootControls, imageHeight, region, index).MouseDown += OpenRecentDropsWindow;
                    recentDropsRegions[index++] = region;
                }
            }
            UpdateDamageForm();
        }

        public void UpdateDamageForm() {
            Hunt hunt = HuntManager.activeHunt;

            int minheight, maxheight;
            ClearControlList(damageControls, out minheight, out maxheight);
            int y = minheight;

            y = 30;
            foreach (Control c in lootControls.Count > 0 ? lootControls : summaryControls) {
                y = Math.Max(c.Location.Y + c.Height, y);
            }

            int maxDamage = SettingsManager.getSettingInt("SummaryMaxDamagePlayers");
            if (maxDamage < 0) maxDamage = 5;
            if (maxDamage > 0 && ScanningManager.lastResults != null) {
                CreateHeaderLabel("Damage Dealt", x, ref y, damageControls);
                var dps = ScanningManager.lastResults.DamagePerSecond;
                var damageDealt = DamageChart.GenerateDamageInformation(dps, "").Item2;
                for (int i = 0; i < damageDealt.Count; i++) {
                    damageDealt[i].color = Constants.DamageChartColors[i % Constants.DamageChartColors.Count];
                }
                int counter = 0;
                foreach (DamageObject obj in damageDealt) {
                    CreateSummaryLabel(obj.name, String.Format("{0:0.0}%", obj.percentage), x, ref y, obj.color, damageControls);
                    if (++counter >= maxDamage) {
                        break;
                    }
                }
            }
            UpdateWasteForm();
        }

        public void UpdateWasteForm() {
            Hunt hunt = HuntManager.activeHunt;

            int minheight, maxheight;
            ClearControlList(usedItemsControls, out minheight, out maxheight);

            int y = 30;
            foreach (Control c in damageControls.Count > 0 ? damageControls : (lootControls.Count > 0 ? lootControls : summaryControls)) {
                y = Math.Max(c.Location.Y + c.Height, y);
            }
            totalWaste = 0;
            int maxUsedItems = SettingsManager.getSettingInt("SummaryMaxUsedItems");
            if (maxUsedItems < 0) maxUsedItems = 5;
            if (maxUsedItems > 0) {
                List<ItemRegion> region;
                int imageHeight = SettingsManager.getSettingInt("SummaryWasteItemSize");
                imageHeight = imageHeight < 0 ? BlockHeight : imageHeight;
                int counter = 0;
                CreateHeaderLabel("Used Items", x, ref y, usedItemsControls);
                int width = 0;
                var items = new List<Tuple<Item, int>>();
                bool display = true;
                foreach (Tuple<Item, int> tpl in HuntManager.GetUsedItems(hunt)) {
                    int amount = tpl.Item2;
                    totalWaste += amount * tpl.Item1.GetMaxValue();
                    while (amount > 0 && display) {
                        int count = Math.Min(100, amount);
                        amount -= count;
                        items.Add(new Tuple<Item, int>(tpl.Item1, count));
                        width += imageHeight + 2;
                        if (width > BlockWidth - imageHeight) {
                            region = new List<ItemRegion>();
                            CreateItemList(items, x, ref y, usedItemsControls, imageHeight, region, counter).MouseDown += OpenWasteWindow;
                            wasteRegions[counter] = region;
                            items.Clear();
                            width = 0;
                            if (++counter >= maxUsedItems) display = false;
                        }
                    }
                }
                if (items.Count > 0) {
                    region = new List<ItemRegion>();
                    CreateItemList(items, x, ref y, usedItemsControls, imageHeight, region, counter).MouseDown += OpenWasteWindow;
                    wasteRegions[counter] = region;
                    items.Clear();
                }
            }
            if (y != maxheight) {
                this.Size = new Size(BlockWidth + 10, y + 5);
            }
        }

        private void CommandClick(object sender, EventArgs e) {
            CommandManager.ExecuteCommand((sender as Control).Name);
        }

        public override string FormName() {
            return "SummaryForm";
        }

        public override int MinWidth() {
            return 210;
        }

        public override int MaxWidth() {
            return 810;
        }

        public override int WidthInterval() {
            return 50;
        }

        public override void RefreshForm() {
            this.SuspendForm();
            this.Size = new Size(GetWidth(), this.Size.Height);
            BlockWidth = this.Size.Width - 10;

            UpdateSummaryForm();
            UpdateLootForm();
            UpdateDamageForm();
            UpdateWasteForm();
            //update the summary form again because the loot value is computed in UpdateLootForm()
            //and UpdateLootForm() has to be called after UpdateLootForm() because it needs the controls to be added to compute its base y position
            UpdateSummaryForm();

            this.ResumeForm();
        }
    }
}
