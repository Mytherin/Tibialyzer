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
    public partial class AdvancedTab : Form, TabInterface {
        ToolTip tooltip = UIManager.CreateTooltip();
        public AdvancedTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            itemSelectionBox.Text = "Plate Armor";
            itemSelectionBox_TextChanged(itemSelectionBox, null);
        }

        public void ApplyLocalization() {
            applyDiscardRatioButton.Text = Tibialyzer.Translation.AdvancedTab.applyDiscardRatioButton;
            itemPropertiesHeader.Text = Tibialyzer.Translation.AdvancedTab.itemPropertiesHeader;
            applyValueConvertButton.Text = Tibialyzer.Translation.AdvancedTab.applyValueConvertButton;
            applySQLQueryButton.Text = Tibialyzer.Translation.AdvancedTab.applySQLQueryButton;
            valueDiscardRatioHeader.Text = Tibialyzer.Translation.AdvancedTab.valueDiscardRatioHeader;
            applyConvertRatioButton.Text = Tibialyzer.Translation.AdvancedTab.applyConvertRatioButton;
            applyValueDiscardButton.Text = Tibialyzer.Translation.AdvancedTab.applyValueDiscardButton;
            customConvertRatioHeader.Text = Tibialyzer.Translation.AdvancedTab.customConvertRatioHeader;
            customDiscardRatioHeader.Text = Tibialyzer.Translation.AdvancedTab.customDiscardRatioHeader;
            valueConvertRatioHeader.Text = Tibialyzer.Translation.AdvancedTab.valueConvertRatioHeader;
            sqlQueryHeader.Text = Tibialyzer.Translation.AdvancedTab.sqlQueryHeader;
        }

        private void applyDiscardRatioButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customDiscardRatioBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateDiscardQuery(String.Format("(MAX(vendor_value, actual_value) / capacity) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
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
            }
        }

        private void applyValueConvertButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customValueConvertBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                StorageManager.UpdateConvertQuery(String.Format("MAX(vendor_value, actual_value) >= {0}", ratio.ToString(CultureInfo.InvariantCulture)));
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

        private void applySQLQueryButton_Click(object sender, EventArgs e) {
            string query = sqlQueryTextbox.Text;

        }
    }
}
