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
        ToolTip tooltip = UIManager.CreateTooltip();
        public SettingsTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            this.enablePopupAnimationsCheckbox.Checked = SettingsManager.getSettingBool("EnableSimpleNotificationAnimation");
            this.popupOnEventCheckbox.Checked = SettingsManager.getSettingBool("EnableEventNotifications");
            this.unrecognizedPopupCheckbox.Checked = SettingsManager.getSettingBool("EnableUnrecognizedNotifications");
            this.copyAdvancesCheckbox.Checked = SettingsManager.getSettingBool("CopyAdvances");
            this.defaultOutfitGenderDropDownList.SelectedIndex = SettingsManager.getSettingBool("OutfitGenderMale") ? 0 : 1;
            this.viewLookedAtObjectsCheckbox.Checked = SettingsManager.getSettingBool("LookMode");
            this.scanningSpeedTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("ScanSpeed"), scanningSpeedTrack.Minimum), scanningSpeedTrack.Maximum);
            this.scanningSpeedDisplayHeader.Text = Constants.ScanSpeedText[scanningSpeedTrack.Value / 10] + String.Format("({0})", scanningSpeedTrack.Value);
            this.experienceComputationDropDownList.SelectedIndex = SettingsManager.getSettingString("ExperiencePerHourCalculation") == "TibiaStyle" ? 0 : 1;
            this.scanInternalTabsCheckbox.Checked = SettingsManager.getSettingBool("ScanInternalTabStructure");
            this.skipDuplicateLootCheckbox.Checked = SettingsManager.getSettingBool("SkipDuplicateLoot");
            this.skipDuplicateCommandsCheckbox.Checked = SettingsManager.getSettingBool("SkipDuplicateCommands");
            this.extraPlayerLookInformationCheckbox.Checked = SettingsManager.getSettingBool("GatherExtraPlayerInformation");
        }

        public void ApplyLocalization() {
            tooltip.RemoveAll();

            skipDuplicateLootCheckbox.Text = Tibialyzer.Translation.SettingsTab.skipDuplicateLootCheckbox;
            experienceComputationHeader.Text = Tibialyzer.Translation.SettingsTab.experienceComputationHeader;
            popupOptionsHeader.Text = Tibialyzer.Translation.SettingsTab.popupOptionsHeader;
            resetSettingsButton.Text = Tibialyzer.Translation.SettingsTab.resetSettingsButton;
            defaultOutfitGenderHeader.Text = Tibialyzer.Translation.SettingsTab.defaultOutfitGenderHeader;
            extraPlayerLookInformationCheckbox.Text = Tibialyzer.Translation.SettingsTab.extraPlayerLookInformationCheckbox;
            copyAdvancesCheckbox.Text = Tibialyzer.Translation.SettingsTab.copyAdvancesCheckbox;
            unrecognizedPopupCheckbox.Text = Tibialyzer.Translation.SettingsTab.unrecognizedPopupCheckbox;
            optionsHeader.Text = Tibialyzer.Translation.SettingsTab.optionsHeader;
            skipDuplicateCommandsCheckbox.Text = Tibialyzer.Translation.SettingsTab.skipDuplicateCommandsCheckbox;
            unlockResetButtonHeader.Text = Tibialyzer.Translation.SettingsTab.unlockResetButtonHeader;
            scanInternalTabsCheckbox.Text = Tibialyzer.Translation.SettingsTab.scanInternalTabsCheckbox;
            resetSettingsToDefaultHeader.Text = Tibialyzer.Translation.SettingsTab.resetSettingsToDefaultHeader;
            scanningSpeedHeader.Text = Tibialyzer.Translation.SettingsTab.scanningSpeedHeader;
            popupOnEventCheckbox.Text = Tibialyzer.Translation.SettingsTab.popupOnEventCheckbox;
            unlockResetButton.Text = Tibialyzer.Translation.SettingsTab.unlockResetButton;
            enablePopupAnimationsCheckbox.Text = Tibialyzer.Translation.SettingsTab.enablePopupAnimationsCheckbox;
            scanningOptionsHeader.Text = Tibialyzer.Translation.SettingsTab.scanningOptionsHeader;
            memoryScanSettingsHeader.Text = Tibialyzer.Translation.SettingsTab.memoryScanSettingsHeader;
            viewLookedAtObjectsCheckbox.Text = Tibialyzer.Translation.SettingsTab.viewLookedAtObjectsCheckbox;
            tooltip.SetToolTip(scanInternalTabsCheckbox, Tibialyzer.Translation.SettingsTab.scanInternalTabsCheckboxTooltip);
            tooltip.SetToolTip(resetSettingsToDefaultHeader, Tibialyzer.Translation.SettingsTab.resetSettingsToDefaultHeaderTooltip);
            tooltip.SetToolTip(popupOnEventCheckbox, Tibialyzer.Translation.SettingsTab.popupOnEventCheckboxTooltip);
            tooltip.SetToolTip(extraPlayerLookInformationCheckbox, Tibialyzer.Translation.SettingsTab.extraPlayerLookInformationCheckboxTooltip);
            tooltip.SetToolTip(defaultOutfitGenderHeader, Tibialyzer.Translation.SettingsTab.defaultOutfitGenderHeaderTooltip);
            tooltip.SetToolTip(scanningSpeedTrack, Tibialyzer.Translation.SettingsTab.scanningSpeedTrackTooltip);
            tooltip.SetToolTip(copyAdvancesCheckbox, Tibialyzer.Translation.SettingsTab.copyAdvancesCheckboxTooltip);
            tooltip.SetToolTip(unrecognizedPopupCheckbox, Tibialyzer.Translation.SettingsTab.unrecognizedPopupCheckboxTooltip);
            tooltip.SetToolTip(enablePopupAnimationsCheckbox, Tibialyzer.Translation.SettingsTab.enablePopupAnimationsCheckboxTooltip);
            tooltip.SetToolTip(experienceComputationDropDownList, Tibialyzer.Translation.SettingsTab.experienceComputationDropDownListTooltip);
            tooltip.SetToolTip(viewLookedAtObjectsCheckbox, Tibialyzer.Translation.SettingsTab.viewLookedAtObjectsCheckboxTooltip);
            int defaultOutfitGenderDropDownListSelectedIndex = defaultOutfitGenderDropDownList.SelectedIndex;
            defaultOutfitGenderDropDownList.Items.Clear();
            defaultOutfitGenderDropDownList.Items.Add(Tibialyzer.Translation.SettingsTab.defaultOutfitGenderDropDownList_0);
            defaultOutfitGenderDropDownList.Items.Add(Tibialyzer.Translation.SettingsTab.defaultOutfitGenderDropDownList_1);
            defaultOutfitGenderDropDownList.SelectedIndex = defaultOutfitGenderDropDownListSelectedIndex;
            int experienceComputationDropDownListSelectedIndex = experienceComputationDropDownList.SelectedIndex;
            experienceComputationDropDownList.Items.Clear();
            experienceComputationDropDownList.Items.Add(Tibialyzer.Translation.SettingsTab.experienceComputationDropDownList_0);
            experienceComputationDropDownList.Items.Add(Tibialyzer.Translation.SettingsTab.experienceComputationDropDownList_1);
            experienceComputationDropDownList.SelectedIndex = experienceComputationDropDownListSelectedIndex;
        }

        private void unlockResetButton_Click(object sender, MouseEventArgs e) {
            if (resetSettingsButton.Enabled) {
                resetSettingsButton.Enabled = false;
                resetSettingsButton.Text = "(Locked)";
                (sender as Control).Text = "Unlock Reset Button";
                unlockResetButtonHeader.Text = "Unlock";
                unlockResetButtonHeader.BackColor = StyleManager.MainFormDangerColor;
            } else {
                resetSettingsButton.Enabled = true;
                resetSettingsButton.Text = "Reset Settings To Default";
                (sender as Control).Text = "Lock Reset Button";
                unlockResetButtonHeader.Text = "Lock";
                unlockResetButtonHeader.BackColor = StyleManager.MainFormSafeColor;
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
            scanningSpeedDisplayHeader.Text = Constants.ScanSpeedText[scanningSpeedTrack.Value / 10] + String.Format("({0})", scanningSpeedTrack.Value);
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

        private void experienceComputationDropdown_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("ExperiencePerHourCalculation", (sender as ComboBox).SelectedIndex == 0 ? "TibiaStyle" : "WeightedStyle" );
        }
        
        private void skipDuplicateLootCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("SkipDuplicateLoot", (sender as CheckBox).Checked);
        }

        private void skipDuplicateCommandsCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("SkipDuplicateCommands", (sender as CheckBox).Checked);
        }

        private void gatherOnlineInformation_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("GatherExtraPlayerInformation", (sender as CheckBox).Checked);
        }

        private void scanInternalTabsCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("ScanInternalTabStructure", (sender as CheckBox).Checked);
        }
    }
}
