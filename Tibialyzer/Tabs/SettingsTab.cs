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
    public partial class SettingsTab : Form, TabInterface {
        public SettingsTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            this.popupAnimationBox.Checked = SettingsManager.getSettingBool("EnableSimpleNotificationAnimation");
            this.eventPopupBox.Checked = SettingsManager.getSettingBool("EnableEventNotifications");
            this.unrecognizedPopupBox.Checked = SettingsManager.getSettingBool("EnableUnrecognizedNotifications");
            this.copyAdvancesCheckbox.Checked = SettingsManager.getSettingBool("CopyAdvances");
            this.popupTypeBox.SelectedIndex = SettingsManager.getSettingBool("UseRichNotificationType") ? 1 : 0;
            this.outfitGenderCheckbox.SelectedIndex = SettingsManager.getSettingBool("OutfitGenderMale") ? 0 : 1;
            this.lookModeCheckbox.Checked = SettingsManager.getSettingBool("LookMode");
            this.scanningSpeedTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("ScanSpeed"), scanningSpeedTrack.Minimum), scanningSpeedTrack.Maximum);
            this.scanSpeedDisplayLabel.Text = Constants.ScanSpeedText[scanningSpeedTrack.Value / 10] + String.Format("({0})", scanningSpeedTrack.Value);
            this.experienceComputationDropdown.SelectedIndex = SettingsManager.getSettingString("ExperiencePerHourCalculation") == "TibiaStyle" ? 0 : 1;
            this.scanEntireMemoryDropdown.SelectedIndex = SettingsManager.getSettingBool("ScanInternalTabStructure") ? 0 : 1;
            this.skipDuplicateLootCheckbox.Checked = SettingsManager.getSettingBool("SkipDuplicateLoot");
            this.skipDuplicateCommandsCheckbox.Checked = SettingsManager.getSettingBool("SkipDuplicateCommands");
        }

        public void InitializeTooltips() {
            ToolTip tooltip = UIManager.CreateTooltip();

            tooltip.SetToolTip(lookModeCheckbox, "When you look (shift+click) at an item, creature or npc in-game, Tibialyzer will automatically open a box displaying information about that object.");
            tooltip.SetToolTip(outfitGenderCheckbox, "Outfit gender displayed in outfit@ searches.");
            tooltip.SetToolTip(copyAdvancesCheckbox, "When you advance in level or skill, the advancement text will be automatically copied for you, so you can easily paste it and notify your friends.");
            tooltip.SetToolTip(eventPopupBox, "When a raid message is send, a notification will appear informing you of the raid.");
            tooltip.SetToolTip(unrecognizedPopupBox, "When you type in an unrecognized command in Tibia chat (unrecognized@), a notification will appear notifying you of this.");
            tooltip.SetToolTip(resetSettingsButton, "Clears all settings and resets them back to the default settings, except for the hunt settings. ");
            tooltip.SetToolTip(popupTypeBox, "Rich notifications are Windows Forms notifications that look pretty. Simple notifications are default Windows bubble notifications. ");
            tooltip.SetToolTip(scanningSpeedTrack, "Set the memory scanning speed of Tibialyzer. Lower settings drastically reduce CPU usage, but increase response time for Tibialyzer to respond to events in-game (such as in-game commands, look events and loot parsing).");
            tooltip.SetToolTip(popupAnimationBox, "Whether or not popups should be animated or simply appear.");
            tooltip.SetToolTip(experienceComputationDropdown, "The algorithm used to compute experience per hour. Standard Tibia Style uses the same algorithm as the Tibia client; while weighted places more emphasis on recent experience gained.");
            tooltip.SetToolTip(scanEntireMemoryDropdown, "Scanning the internal tab structure is much faster and prevents duplicate issues (only available for C client).\nOnly select scanning the entire memory if for some reason this setting does not work (e.g. because of an update).");
        }

        private void unlockResetButton_Click(object sender, MouseEventArgs e) {
            if (resetSettingsButton.Enabled) {
                resetSettingsButton.Enabled = false;
                resetSettingsButton.Text = "(Locked)";
                (sender as Control).Text = "Unlock Reset Button";
                unlockLabel.Text = "Unlock";
                unlockLabel.BackColor = StyleManager.MainFormDangerColor;
            } else {
                resetSettingsButton.Enabled = true;
                resetSettingsButton.Text = "Reset Settings To Default";
                (sender as Control).Text = "Lock Reset Button";
                unlockLabel.Text = "Lock";
                unlockLabel.BackColor = StyleManager.MainFormSafeColor;
            }
        }

        public int MinimumScanSpeed() {
            return scanningSpeedTrack.Minimum;
        }

        public int MaximumScanSpeed() {
            return scanningSpeedTrack.Maximum;
        }

        private void resetToDefaultButton_Click(object sender, EventArgs e) {
            SettingsManager.ResetSettingsToDefault();
            SettingsManager.SaveSettings();
            AutoHotkeyManager.ShutdownAutohotkey();
            MainForm.mainForm.initializeSettings();
        }

        private void scanningSpeedTrack_Scroll(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("ScanSpeed", scanningSpeedTrack.Value);
            scanSpeedDisplayLabel.Text = Constants.ScanSpeedText[scanningSpeedTrack.Value / 10] + String.Format("({0})", scanningSpeedTrack.Value);
        }

        private void lookCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("LookMode", (sender as CheckBox).Checked.ToString());
        }

        private void outfitGenderBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("OutfitGenderMale", ((sender as ComboBox).SelectedIndex == 0).ToString());
        }

        private void eventNotificationEnable_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("EnableEventNotifications", (sender as CheckBox).Checked.ToString());
        }

        private void unrecognizedCommandNotification_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("EnableUnrecognizedNotifications", (sender as CheckBox).Checked.ToString());
        }

        private void enableSimpleNotificationAnimations_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("EnableSimpleNotificationAnimation", (sender as CheckBox).Checked);
        }

        private void advanceCopyCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("CopyAdvances", (sender as CheckBox).Checked.ToString());
        }
        private void notificationTypeBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("UseRichNotificationType", ((sender as ComboBox).SelectedIndex == 1).ToString());
        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

        private void experienceComputationDropdown_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("ExperiencePerHourCalculation", (sender as ComboBox).SelectedIndex == 0 ? "TibiaStyle" : "WeightedStyle" );
        }

        private void scanEntireMemoryDropdown_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("ScanInternalTabStructure", (sender as ComboBox).SelectedIndex == 0);
        }

        private void skipDuplicateLootCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("SkipDuplicateLoot", (sender as CheckBox).Checked);
        }

        private void skipDuplicateCommandsCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("SkipDuplicateCommands", (sender as CheckBox).Checked);
        }
    }
}
