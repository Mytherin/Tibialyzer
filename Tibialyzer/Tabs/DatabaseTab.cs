using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class DatabaseTab : Form, TabInterface {
        ToolTip tooltip = UIManager.CreateTooltip();
        public DatabaseTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            CreateRatioDisplay(Constants.DisplayItemList, databaseItemsHeader.Location.X, databaseItemsHeader.Location.Y + databaseItemsHeader.Size.Height + 8, discardLabels, convertLabels);
            UpdateDisplay();
            int x = databaseItemsHeader.Location.X + 25;
            int y = databaseItemsHeader.Location.Y + 145, width = 154;
            CreateLegendLabel(StyleManager.DatabaseDiscardColor, "Discard", x, y); x += width;
            CreateLegendLabel(StyleManager.DatabaseNoDiscardColor, "Pickup", x, y); x += width;
            CreateLegendLabel(StyleManager.ItemGoldColor, "Gold Convert", x, y); x += width;
            CreateLegendLabel(StyleManager.DatabaseNoConvertColor, "No Convert", x, y); x += width;
        }

        public void CreateLegendLabel(Color color, string text, int x, int y) {
            Label label = new Label();
            label.BackColor = color;
            label.Size = new Size(24, 24);
            label.Location = new Point(x, y);
            this.Controls.Add(label);

            label = new Label();
            label.BackColor = Color.Transparent;
            label.ForeColor = StyleManager.MainFormButtonForeColor;
            label.Text = text;
            label.Font = StyleManager.MainFormLabelFontSmall;
            label.Location = new Point(x + 25, y);
            label.Size = new Size(110, 24);
            label.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(label);
        }

        public void ApplyLocalization() {
            tooltip.RemoveAll();
            mageCreatureProductsButton.Text = Tibialyzer.Translation.DatabaseTab.mageCreatureProductsButton;
            conversionPresetsHeader.Text = Tibialyzer.Translation.DatabaseTab.conversionPresetsHeader;
            noGoldCoinButton.Text = Tibialyzer.Translation.DatabaseTab.noGoldCoinButton;
            valuablesOnly5KButton.Text = Tibialyzer.Translation.DatabaseTab.valuablesOnly5KButton;
            mageNoCreatureProductsButton.Text = Tibialyzer.Translation.DatabaseTab.mageNoCreatureProductsButton;
            valuablesOnly1KButton.Text = Tibialyzer.Translation.DatabaseTab.valuablesOnly1KButton;
            defaultSettingsButton.Text = Tibialyzer.Translation.DatabaseTab.defaultSettingsButton;
            databaseItemsHeader.Text = Tibialyzer.Translation.DatabaseTab.databaseItemsHeader;
        }

        private void CreateRatioDisplay(List<string> itemList, int baseX, int baseY, List<Control> discardControls, List<Control> convertControls) {
            int it = 0;
            foreach (string itemName in itemList) {
                Item item = StorageManager.getItem(itemName);
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = item.image;
                pictureBox.Location = new Point(baseX + it * 52, baseY);
                pictureBox.BackgroundImage = StyleManager.GetImage("item_background.png");
                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Size = new Size(48, 48);
                pictureBox.Name = itemName;

                double goldRatio = item.GetMaxValue() / item.capacity;
                Label label = new Label();
                label.Text = goldRatio > 1000 ? StyleManager.GoldToText(goldRatio).ToString() : String.Format(goldRatio < 100 ? "{0:0.#}" : "{0:0.}", goldRatio);
                label.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + pictureBox.Size.Height);
                label.Font = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                label.Size = new Size(48, 24);
                label.ForeColor = StyleManager.MainFormButtonColor;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Name = itemName + ":" + goldRatio.ToString(CultureInfo.InvariantCulture);
                label.Click += SetDiscardRatio;
                discardControls.Add(label);

                Label convertLabel = new Label();
                convertLabel.Text = StyleManager.GoldToText(item.GetMaxValue()).ToString();
                convertLabel.Location = new Point(pictureBox.Location.X, label.Location.Y + label.Size.Height);
                convertLabel.Font = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                convertLabel.Size = new Size(48, 24);
                convertLabel.ForeColor = StyleManager.MainFormButtonColor;
                convertLabel.TextAlign = ContentAlignment.MiddleCenter;
                convertLabel.Name = itemName + ":" + goldRatio.ToString(CultureInfo.InvariantCulture);
                convertLabel.Click += SetConvertValue;
                convertControls.Add(convertLabel);

                this.Controls.Add(pictureBox);
                this.Controls.Add(label);
                this.Controls.Add(convertLabel);
                it++;
            }
        }

        private void SetConvertValue(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse((sender as Control).Name.Split(':')[1], NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateConvertQuery(String.Format("(MAX(vendor_value, actual_value) / capacity) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
                UpdateDisplay();
            }
        }

        private void SetDiscardRatio(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse((sender as Control).Name.Split(':')[1], NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateDiscardQuery(String.Format("(MAX(vendor_value, actual_value) / capacity) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
                UpdateDisplay();
            }
        }

        private List<Control> discardLabels = new List<Control>();
        private List<Control> convertLabels = new List<Control>();
        private void UpdateDisplay() {
            foreach (Control c in discardLabels) {
                string itemName = c.Name.Split(':')[0];
                Item item = StorageManager.getItem(itemName);
                if (item.discard) {
                    c.BackColor = StyleManager.DatabaseDiscardColor;
                } else {
                    c.BackColor = StyleManager.DatabaseNoDiscardColor;
                }
            }
            foreach (Control c in convertLabels) {
                string itemName = c.Name.Split(':')[0];
                Item item = StorageManager.getItem(itemName);
                if (item.convert_to_gold) {
                    c.BackColor = StyleManager.ItemGoldColor;
                } else {
                    c.BackColor = StyleManager.DatabaseNoConvertColor;
                }
            }
        }

        private void defaultSettingsButton_Click(object sender, EventArgs e) {
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=0, discard=0;");
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=1 WHERE stackable = 0 AND actual_value/capacity < 20");
            StorageManager.ExecuteCommand("UPDATE Items SET discard=1 WHERE actual_value/capacity < 10 OR actual_value<50");
            StorageManager.ReloadItems();
            UpdateDisplay();
        }

        private void noGoldCoinButton_Click(object sender, EventArgs e) {
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=0, discard=0;");
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=1 WHERE stackable = 0 AND actual_value/capacity < 20");
            StorageManager.ExecuteCommand("UPDATE Items SET discard=1 WHERE actual_value/capacity < 20 OR actual_value<50");
            StorageManager.ReloadItems();
            UpdateDisplay();
        }
        
        private void mageCreatureProductsButton_Click(object sender, EventArgs e) {
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=0, discard=0;");
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=1 WHERE stackable = 0 AND actual_value/capacity < 50");
            StorageManager.ExecuteCommand("UPDATE Items SET discard=1 WHERE actual_value/capacity < 40 OR actual_value<50");
            StorageManager.ReloadItems();
            UpdateDisplay();
        }

        private void mageNoCreatureProductsButton_Click(object sender, EventArgs e) {
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=0, discard=0;");
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=1 WHERE stackable = 0 AND actual_value/capacity < 50");
            StorageManager.ExecuteCommand("UPDATE Items SET discard=1 WHERE actual_value/capacity < 40 OR (category='Creature Products' AND actual_value < 2000) OR actual_value<50");
            StorageManager.ReloadItems();
            UpdateDisplay();
        }

        private void valuablesOnly1KButton_Click(object sender, EventArgs e) {
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=0, discard=0;");
            StorageManager.ExecuteCommand("UPDATE Items SET discard=1 WHERE actual_value < 1000");
            StorageManager.ReloadItems();
            UpdateDisplay();
        }

        private void valuablesOnly5KButton_Click(object sender, EventArgs e) {
            StorageManager.ExecuteCommand("UPDATE Items SET convert_to_gold=0, discard=0;");
            StorageManager.ExecuteCommand("UPDATE Items SET discard=1 WHERE actual_value < 5000");
            StorageManager.ReloadItems();
            UpdateDisplay();
        }
    }
}
