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
    public partial class NotificationsTab : Form, TabInterface {
        ToolTip tooltip = UIManager.CreateTooltip();
        public NotificationsTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            notificationTypeList.ReadOnly = true;

            notificationTypeList.Items.Clear();
            foreach (string str in Constants.NotificationTypes) {
                notificationTypeList.Items.Add(str);
            }
            notificationTypeList.SelectedIndex = 0;

            Constants.MaximumNotificationDuration = notificationDurationBox.Maximum;

            monitorAnchorDropdown.SelectedIndex = SettingsManager.getSettingInt("MonitorAnchor");
            onlyShowWhenTibiaIsActiveCheckbox.Checked = SettingsManager.getSettingBool("NotificationShowTibiaActive");
        }

        public void ApplyLocalization() {
            tooltip.RemoveAll();
            notificationTypeListHeader.Text = Tibialyzer.Translation.NotificationsTab.notificationTypeListHeader;
            yOffsetLabel.Text = Tibialyzer.Translation.NotificationsTab.yOffsetLabel;
            positionOffsetHeader.Text = Tibialyzer.Translation.NotificationsTab.positionOffsetHeader;
            overwriteSettingsHeader.Text = Tibialyzer.Translation.NotificationsTab.overwriteSettingsHeader;
            xOffsetLabel.Text = Tibialyzer.Translation.NotificationsTab.xOffsetLabel;
            onlyShowWhenTibiaIsActiveCheckbox.Text = Tibialyzer.Translation.NotificationsTab.onlyShowWhenTibiaIsActiveCheckbox;
            monitorAnchorHeader.Text = Tibialyzer.Translation.NotificationsTab.monitorAnchorHeader;
            notificationLengthHeader.Text = Tibialyzer.Translation.NotificationsTab.notificationLengthHeader;
            applyTheseSettingsToAllButton.Text = Tibialyzer.Translation.NotificationsTab.applyTheseSettingsToAllButton;
            displayGroupHeader.Text = Tibialyzer.Translation.NotificationsTab.displayGroupHeader;
            displayNotificationButton.Text = Tibialyzer.Translation.NotificationsTab.displayNotificationButton;
            anchorHeader.Text = Tibialyzer.Translation.NotificationsTab.anchorHeader;
            clearDisplayButton.Text = Tibialyzer.Translation.NotificationsTab.clearDisplayButton;
            testingHeader.Text = Tibialyzer.Translation.NotificationsTab.testingHeader;
            tooltip.SetToolTip(applyTheseSettingsToAllButton, Tibialyzer.Translation.NotificationsTab.applyTheseSettingsToAllButtonTooltip);
            tooltip.SetToolTip(displayGroupDropDownList, Tibialyzer.Translation.NotificationsTab.displayGroupDropDownListTooltip);
            tooltip.SetToolTip(notificationAnchorDropDownList, Tibialyzer.Translation.NotificationsTab.notificationAnchorDropDownListTooltip);
            int displayGroupDropDownListSelectedIndex = displayGroupDropDownList.SelectedIndex;
            displayGroupDropDownList.Items.Clear();
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_0);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_1);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_2);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_3);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_4);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_5);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_6);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_7);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_8);
            displayGroupDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.displayGroupDropDownList_9);
            displayGroupDropDownList.SelectedIndex = displayGroupDropDownListSelectedIndex;
            int notificationAnchorDropDownListSelectedIndex = notificationAnchorDropDownList.SelectedIndex;
            notificationAnchorDropDownList.Items.Clear();
            notificationAnchorDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.notificationAnchorDropDownList_0);
            notificationAnchorDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.notificationAnchorDropDownList_1);
            notificationAnchorDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.notificationAnchorDropDownList_2);
            notificationAnchorDropDownList.Items.Add(Tibialyzer.Translation.NotificationsTab.notificationAnchorDropDownList_3);
            notificationAnchorDropDownList.SelectedIndex = notificationAnchorDropDownListSelectedIndex;
        }

        private void testNotificationDisplayButton_Click(object sender, EventArgs e) {
            string command = Constants.NotificationTestCommands[notificationTypeList.SelectedIndex];
            CommandManager.ExecuteCommand(command);
        }


        private string selectedNotificationObject() {
            return notificationTypeList.Items[notificationTypeList.SelectedIndex].ToString().Replace(" ", ""); ;
        }

        private void notificationTypeList_SelectedIndexChanged(object sender, EventArgs e) {
            string settingObject = selectedNotificationObject();

            selectedWindowHeader.Text = notificationTypeList.Items[notificationTypeList.SelectedIndex].ToString();

            int anchor = Math.Max(Math.Min(SettingsManager.getSettingInt(settingObject + "Anchor"), 3), 0);
            int xOffset = SettingsManager.getSettingInt(settingObject + "XOffset");
            int yOffset = SettingsManager.getSettingInt(settingObject + "YOffset");
            int notificationLength = SettingsManager.getSettingInt(settingObject + "Duration");
            int groupnr = Math.Max(Math.Min(SettingsManager.getSettingInt(settingObject + "Group"), 9), 0);
            int sliderValue = Math.Max(Math.Min(notificationLength, notificationDurationBox.Maximum), notificationDurationBox.Minimum);

            MainForm.prevent_settings_update = true;
            notificationLengthHeader.Text = String.Format("Duration ({0})", sliderValue == notificationDurationBox.Maximum ? "INF" : sliderValue.ToString() + "s");
            notificationDurationBox.Value = sliderValue;
            displayGroupDropDownList.SelectedIndex = groupnr;
            notificationXOffsetBox.Text = xOffset.ToString();
            notificationYOffsetBox.Text = yOffset.ToString();
            notificationAnchorDropDownList.SelectedIndex = anchor;
            MainForm.prevent_settings_update = false;
        }

        private void notificationAnchorBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            SettingsManager.setSetting(selectedNotificationObject() + "Anchor", notificationAnchorDropDownList.SelectedIndex);
        }

        private void groupSelectionList_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            SettingsManager.setSetting(selectedNotificationObject() + "Group", displayGroupDropDownList.SelectedIndex);
        }

        private void notificationXOffsetBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            int value;
            if (int.TryParse(notificationXOffsetBox.Text, out value)) {
                SettingsManager.setSetting(selectedNotificationObject() + "XOffset", value);
            }
        }

        private void notificationYOffsetBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            int value;
            if (int.TryParse(notificationYOffsetBox.Text, out value)) {
                SettingsManager.setSetting(selectedNotificationObject() + "YOffset", value);
            }
        }

        private void notificationDurationBox_Scroll(object sender, EventArgs e) {
            int sliderValue = notificationDurationBox.Value;
            notificationLengthHeader.Text = String.Format("Duration ({0})", sliderValue == notificationDurationBox.Maximum ? "INF" : sliderValue.ToString() + "s");
            SettingsManager.setSetting(selectedNotificationObject() + "Duration", sliderValue);
        }

        private void applyNotificationSettingsToAllButton_Click(object sender, EventArgs e) {
            string selectedSettingObject = selectedNotificationObject();

            int anchor = Math.Max(Math.Min(SettingsManager.getSettingInt(selectedSettingObject + "Anchor"), 3), 0);
            int xOffset = SettingsManager.getSettingInt(selectedSettingObject + "XOffset");
            int yOffset = SettingsManager.getSettingInt(selectedSettingObject + "YOffset");
            int notificationLength = SettingsManager.getSettingInt(selectedSettingObject + "Duration");
            int groupnr = Math.Max(Math.Min(SettingsManager.getSettingInt(selectedSettingObject + "Group"), 9), 0);
            int sliderValue = Math.Max(Math.Min(notificationLength, notificationDurationBox.Maximum), notificationDurationBox.Minimum);

            foreach (string str in Constants.NotificationTypes) {
                string settingObject = str.Replace(" ", "");
                SettingsManager.setSetting(settingObject + "Anchor", anchor);
                SettingsManager.setSetting(settingObject + "XOffset", xOffset);
                SettingsManager.setSetting(settingObject + "YOffset", yOffset);
                SettingsManager.setSetting(settingObject + "Duration", notificationLength);
                SettingsManager.setSetting(settingObject + "Group", groupnr);
            }
        }

        private void clearNotificationDisplayButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("close@");
        }

        private void monitorAnchorDropdown_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("MonitorAnchor", monitorAnchorDropdown.SelectedIndex);
        }

        private void hideWhenTibiaMinimized_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("NotificationShowTibiaActive", (sender as CheckBox).Checked);
        }
    }
}
