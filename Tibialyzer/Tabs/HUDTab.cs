using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class HUDTab : Form, TabInterface {
        public HUDTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            hudTypeList.ReadOnly = true;

            hudTypeList.Items.Clear();
            foreach (string hudName in Constants.HudTypes) {
                hudTypeList.Items.Add(hudName);
            }
            hudTypeList.SelectedIndex = 0;
        }

        public void InitializeTooltips() {

        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

        private string getSelectedHudName() {
            return hudTypeList.Items[hudTypeList.SelectedIndex].ToString().Replace(" ", "");
        }

        private void hudTypeList_SelectedIndexChanged(object sender, EventArgs e) {
            selectedHUDLabel.Text = hudTypeList.Items[hudTypeList.SelectedIndex].ToString();
            MainForm.prevent_settings_update = true;
            string hudName = getSelectedHudName();
            hudXOffsetBox.Text = SettingsManager.getSettingString(hudName + "XOffset");
            hudYOffsetBox.Text = SettingsManager.getSettingString(hudName + "YOffset");
            hudAnchorBox.SelectedIndex = SettingsManager.getSettingInt(hudName + "Anchor");
            hudWidthBox.Text = SettingsManager.getSettingString(hudName + "Width");
            hudHeightBox.Text = SettingsManager.getSettingString(hudName + "Height");
            showHudOnStartupBox.Checked = SettingsManager.getSettingBool(hudName + "ShowOnStartup");
            fontSizeBox.Text = SettingsManager.getSettingString(hudName + "FontSize");
            hudOpacityBox.Text = SettingsManager.getSettingString(hudName + "Opacity");
            displayHUDTextBox.Checked = SettingsManager.getSettingBool(hudName + "DisplayText");
            MainForm.prevent_settings_update = false;
        }

        private void hudXOffsetBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            int value;
            if (int.TryParse(hudXOffsetBox.Text, out value)) {
                SettingsManager.setSetting(getSelectedHudName() + "XOffset", value);
            }
        }

        private void hudYOffsetBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            int value;
            if (int.TryParse(hudYOffsetBox.Text, out value)) {
                SettingsManager.setSetting(getSelectedHudName() + "YOffset", value);
            }
        }

        private void hudAnchorBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting(getSelectedHudName() + "Anchor", hudAnchorBox.SelectedIndex);
        }

        private void showHudOnStartupBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting(getSelectedHudName() + "ShowOnStartup", (sender as CheckBox).Checked);
        }
        
        private void showHudButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand(Constants.HudTestCommands[hudTypeList.SelectedIndex]);
        }

        private void closeHudButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("closehud@" + Constants.HudTypes[hudTypeList.SelectedIndex]);
        }

        private void closeAllHudsButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("closehud@");
        }

        private void hudWidthBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            int value;
            if (int.TryParse(hudWidthBox.Text, out value)) {
                SettingsManager.setSetting(getSelectedHudName() + "Width", value);
            }
        }

        private void hudHeightBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            int value;
            if (int.TryParse(hudHeightBox.Text, out value)) {
                SettingsManager.setSetting(getSelectedHudName() + "Height", value);
            }
        }

        private void fontSizeBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            double value;
            if (double.TryParse(fontSizeBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out value)) {
                SettingsManager.setSetting(getSelectedHudName() + "FontSize", value);
            }
        }

        private void hudOpacityBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            double value;
            if (double.TryParse(hudOpacityBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out value)) {
                if (value >= 0 && value <= 1) {
                    SettingsManager.setSetting(getSelectedHudName() + "Opacity", value);
                }
            }
        }

        private void displayHUDTextBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting(getSelectedHudName() + "DisplayText", (sender as CheckBox).Checked);
        }
    }
}
