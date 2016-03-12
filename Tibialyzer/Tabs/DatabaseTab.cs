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
        public DatabaseTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            CreateRatioDisplay(Constants.DisplayItemList, databaseItemsHeader.Location.X + 10, databaseItemsHeader.Location.Y + databaseItemsHeader.Size.Height + 8, discardLabels, convertLabels);
            UpdateDisplay();
            int x = databaseItemsHeader.Location.X;
            const int y = 145, width = 135;
            CreateLegendLabel(StyleManager.DatabaseDiscardColor, "Discard", x, y); x += width;
            CreateLegendLabel(StyleManager.DatabaseNoDiscardColor, "Pickup", x, y); x += width;
            CreateLegendLabel(StyleManager.ItemGoldColor, "Gold Convert", x, y); x += width;
            CreateLegendLabel(StyleManager.DatabaseNoConvertColor, "No Convert", x, y); x += width;

            itemSelectionBox.Text = "Plate Armor";
            itemSelectionBox_TextChanged(itemSelectionBox, null);
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

        public void InitializeTooltips() {
            ToolTip tooltip = UIManager.CreateTooltip();

            string discardRatioText = "Pickup all items with a gold/cap ratio higher than the value entered in the box; discard all items with a lower ratio.";
            tooltip.SetToolTip(applyDiscardRatioButton, discardRatioText);
            tooltip.SetToolTip(customDiscardRatioBox, discardRatioText);
            string discardValueText = "Pickup all items with a gold value higher than the value entered in the box; discard all items with a lower value.";
            tooltip.SetToolTip(applyValueDiscardButton, discardValueText);
            tooltip.SetToolTip(customValueDiscardBox, discardValueText);
            string discardQueryText = "Pickup all items that match the SQL condition; discard all items that don't.\nExample of a query: MAX(vendor_value, actual_value) >= 2000\nPick up all items with a value above 2000 gold, discard other items.";
            tooltip.SetToolTip(applyQueryDiscardButton, discardQueryText);
            tooltip.SetToolTip(customQueryDiscardBox, discardQueryText);
            string convertRatioText = "Don't convert items with a gold/cap ratio higher than the value entered in the box to gold; convert items that with a lower ratio.";
            tooltip.SetToolTip(applyConvertRatioButton, convertRatioText);
            tooltip.SetToolTip(customConvertRatioBox, convertRatioText);
            string convertValueText = "Don't convert items with a gold value higher than the value entered in the box to gold; convert items that with a lower gold value.";
            tooltip.SetToolTip(applyValueConvertButton, convertValueText);
            tooltip.SetToolTip(customValueConvertBox, convertValueText);
            string convertQueryText = "Don't convert items that match the SQL condition to gold; convert items that don't.\nExample of a query: MAX(vendor_value, actual_value) >= 2000\nDon't convert items with a value above 2000 gold, convert other items.";
            tooltip.SetToolTip(applyQueryConvertButton, convertQueryText);
            tooltip.SetToolTip(customQueryConvertBox, convertQueryText);

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
                label.Text = String.Format(goldRatio < 100 ? "{0:0.#}" : "{0:0.}", goldRatio);
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
        
        private void applyDiscardRatioButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customDiscardRatioBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateDiscardQuery(String.Format("(MAX(vendor_value, actual_value) / capacity) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
                UpdateDisplay();
            }
        }

        private void customDiscardRatioBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                applyDiscardRatioButton_Click(null, null);
                e.Handled = true;
            }
        }

        private void applyConvertRatioButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customConvertRatioBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateConvertQuery(String.Format("(MAX(vendor_value, actual_value) / capacity) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
                UpdateDisplay();
            }
        }

        private void customConvertRatioBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                applyConvertRatioButton_Click(null, null);
                e.Handled = true;
            }
        }

        private void applyValueDiscardButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customValueDiscardBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateDiscardQuery(String.Format("MAX(vendor_value, actual_value) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
                UpdateDisplay();
            }
        }

        private void applyValueConvertButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customValueConvertBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateConvertQuery(String.Format("MAX(vendor_value, actual_value) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
                UpdateDisplay();
            }
        }

        private void applyQueryDiscardButton_Click(object sender, EventArgs e) {
            try {
                StorageManager.UpdateDiscardQuery(customQueryDiscardBox.Text);
                UpdateDisplay();
            } catch(Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }

        private void applyQueryConvertButton_Click(object sender, EventArgs e) {
            try {
                StorageManager.UpdateConvertQuery(customQueryDiscardBox.Text);
                UpdateDisplay();
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }

        private void itemSelectionBox_TextChanged(object sender, EventArgs e) {
            Item item = StorageManager.getItem(itemSelectionBox.Text);
            if (item != null) {
                itemPropertyItemBox.Image = item.image;
                stackableValueLabel.Text = item.stackable ? "True" : "False";
                vendorValueLabel.Text = StyleManager.GoldToText(item.vendor_value);
                actualValueLabel.Text = StyleManager.GoldToText(item.actual_value);
                capacityLabel.Text = String.Format("{0:0.00}", item.capacity);
                discardValueLabel.Text = item.discard ? "True" : "False";
                convertValueLabel.Text = item.convert_to_gold ? "True" : "False";
                categoryValueLabel.Text = item.category;
            }
        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

    }
}
