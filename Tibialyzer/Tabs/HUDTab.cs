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
        ToolTip tooltip = UIManager.CreateTooltip();

        public HUDTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            hudTypeList.ReadOnly = true;

            hudTypeList.Items.Clear();
            foreach (string hudName in Constants.HudTypes) {
                hudTypeList.Items.Add(hudName);
            }
            hudTypeList.SelectedIndex = 0;

            alwaysShowHUDCheckbox.Checked = SettingsManager.getSettingBool("AlwaysShowHUD");
        }

        public void ApplyLocalization() {
            positionOffsetHeader.Text = Tibialyzer.Translation.HUDTab.positionOffsetHeader;
            closeAllHudsButton.Text = Tibialyzer.Translation.HUDTab.closeAllHudsButton;
            sizeHeader.Text = Tibialyzer.Translation.HUDTab.sizeHeader;
            displayHUDTextCheckbox.Text = Tibialyzer.Translation.HUDTab.displayHUDTextCheckbox;
            centerHUDTextCheckbox.Text = Tibialyzer.Translation.HUDTab.centerHUDTextCheckbox;
            reverseProgressBarCheckbox.Text = Tibialyzer.Translation.HUDTab.reverseProgressBarCheckbox;
            hudTypeListHeader.Text = Tibialyzer.Translation.HUDTab.hudTypeListHeader;
            yOffsetLabel.Text = Tibialyzer.Translation.HUDTab.yOffsetLabel;
            closeHudButton.Text = Tibialyzer.Translation.HUDTab.closeHudButton;
            advancedOptionsButton.Text = Tibialyzer.Translation.HUDTab.advancedOptionsButton;
            opacityLabel.Text = Tibialyzer.Translation.HUDTab.opacityLabel;
            widthLabel.Text = Tibialyzer.Translation.HUDTab.widthLabel;
            startupHeader.Text = Tibialyzer.Translation.HUDTab.startupHeader;
            xOffsetLabel.Text = Tibialyzer.Translation.HUDTab.xOffsetLabel;
            heightLabel.Text = Tibialyzer.Translation.HUDTab.heightLabel;
            sizeLabel.Text = Tibialyzer.Translation.HUDTab.sizeLabel;
            alwaysShowHUDCheckbox.Text = Tibialyzer.Translation.HUDTab.alwaysShowHUDCheckbox;
            anchorHeader.Text = Tibialyzer.Translation.HUDTab.anchorHeader;
            fontSizeHeader.Text = Tibialyzer.Translation.HUDTab.fontSizeHeader;
            showHudButton.Text = Tibialyzer.Translation.HUDTab.showHudButton;
            displayOptionsHeader.Text = Tibialyzer.Translation.HUDTab.displayOptionsHeader;
            showHudOnStartupCheckbox.Text = Tibialyzer.Translation.HUDTab.showHudOnStartupCheckbox;
            int anchorDropDownListSelectedIndex = anchorDropDownList.SelectedIndex;
            anchorDropDownList.Items.Clear();
            anchorDropDownList.Items.Add(Tibialyzer.Translation.HUDTab.anchorDropDownList_0);
            anchorDropDownList.Items.Add(Tibialyzer.Translation.HUDTab.anchorDropDownList_1);
            anchorDropDownList.Items.Add(Tibialyzer.Translation.HUDTab.anchorDropDownList_2);
            anchorDropDownList.Items.Add(Tibialyzer.Translation.HUDTab.anchorDropDownList_3);
            anchorDropDownList.Items.Add(Tibialyzer.Translation.HUDTab.anchorDropDownList_4);
            anchorDropDownList.SelectedIndex = anchorDropDownListSelectedIndex;
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
            anchorDropDownList.SelectedIndex = SettingsManager.getSettingInt(hudName + "Anchor");
            hudWidthBox.Text = SettingsManager.getSettingString(hudName + "Width");
            hudHeightBox.Text = SettingsManager.getSettingString(hudName + "Height");
            showHudOnStartupCheckbox.Checked = SettingsManager.getSettingBool(hudName + "ShowOnStartup");
            fontSizeBox.Text = SettingsManager.getSettingString(hudName + "FontSize");
            hudOpacityBox.Text = SettingsManager.getSettingString(hudName + "Opacity");
            displayHUDTextCheckbox.Checked = SettingsManager.getSettingBool(hudName + "DisplayText");
            centerHUDTextCheckbox.Checked = SettingsManager.getSettingBool(hudName + "CenterText");
            reverseProgressBarCheckbox.Checked = SettingsManager.getSettingBool(hudName + "ReverseProgressBar");
            
            advancedOptionsButton.Visible = hudName == "HealthList" || hudName == "Portrait";
            centerHUDTextCheckbox.Visible = hudName == "HealthBar" || hudName == "ManaBar" || hudName == "ExperienceBar";
            reverseProgressBarCheckbox.Visible = hudName == "HealthBar" || hudName == "ManaBar" || hudName == "ExperienceBar";

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

            SettingsManager.setSetting(getSelectedHudName() + "Anchor", anchorDropDownList.SelectedIndex);
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

        private void advancedOptionsButton_Click(object sender, EventArgs e) {
            if (getSelectedHudName() == "HealthList") {
                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    MainForm.mainForm.switchTab(14);
                });
            } else if (getSelectedHudName() == "Portrait") {
                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    MainForm.mainForm.switchTab(15);
                });
            }
        }

        private void alwaysShowHUD_CheckedChanged(object sender, EventArgs e) {
            SettingsManager.setSetting("AlwaysShowHUD", (sender as CheckBox).Checked);
        }

        private void centerHUDTextCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting(getSelectedHudName() + "CenterText", (sender as CheckBox).Checked);
        }

        private void reverseProgressBarCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting(getSelectedHudName() + "ReverseProgressBar", (sender as CheckBox).Checked);
        }
    }
}
