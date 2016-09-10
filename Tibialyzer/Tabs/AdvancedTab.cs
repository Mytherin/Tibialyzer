using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
            sqlQueryResultCollection.ReadOnly = true;
            sqlQueryResultCollection.TextAlign = HorizontalAlignment.Left;
            sqlQueryResultCollection.DrawMode = DrawMode.OwnerDrawVariable;

            oldAddressesDropDownList.Items.Clear();
            foreach(string version in Constants.OldMemoryAddresses) {
                oldAddressesDropDownList.Items.Add("Tibia " + version);
            }
            oldAddressesDropDownList.SelectedIndex = oldAddressesDropDownList.Items.Count - 1;
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
            sqlQueryResultCollection.Items.Clear();
            SQLiteDataReader reader;
            try {
                reader = StorageManager.ExecuteQuery(query);
            } catch(Exception ex) {
                sqlQueryResultCollection.Items.Add(ex.Message);
                return;
            }
            if (reader == null) return;
            string result = "[";
            for (int i = 0; i < reader.FieldCount; i++) {
                result += reader.GetOriginalName(i);
                result += ", ";
            }
            sqlQueryResultCollection.Items.Add(result.Substring(0, result.Length - 2) + "]");

            while (reader.Read()) {
                result = "[";
                for(int i = 0; i < reader.FieldCount; i++) {
                    string res = reader[i].ToString();
                    result += res == null ? "NULL" : res;
                    result += ", ";
                }
                sqlQueryResultCollection.Items.Add(result.Substring(0, result.Length - 2) + "]");
            }
        }

        private void oldAddressesApplyButton_Click(object sender, EventArgs e) {
            int selection = oldAddressesDropDownList.SelectedIndex;
            if (selection >= 0 && selection < Constants.OldMemoryAddresses.Length) {
                MainTab.DownloadNewAddresses(String.Format("https://raw.githubusercontent.com/Mytherin/Tibialyzer/master/Addresses/Addresses-{0}", Constants.OldMemoryAddresses[selection].Replace(".", "")));
                // automatic update always downloads the new addresses; if the user is manually downloading old addresses
                // it would be confusing to replace the old addresses with new addresses on startup
                SettingsManager.setSetting("AutomaticallyDownloadAddresses", false);
            }
        }
    }
}
