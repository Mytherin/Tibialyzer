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
        public NotificationsTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
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
        }

        public void InitializeTooltips() {
            ToolTip tooltip = UIManager.CreateTooltip();

            tooltip.SetToolTip(notificationAnchorBox, "The screen anchor to which the offsets should be applied.");
            tooltip.SetToolTip(notificationGroupBox, "The display group to which this notification type belongs. Only one notification can be active per group.");
            tooltip.SetToolTip(notificationDurationBox, "How long the notification should be alive before fading. If it is set to INF it will never fade away.");
            tooltip.SetToolTip(applyNotificationSettingsToAllButton, "Apply the settings of this notification type to all notifications.");
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

            selectedWindowLabel.Text = notificationTypeList.Items[notificationTypeList.SelectedIndex].ToString();

            int anchor = Math.Max(Math.Min(SettingsManager.getSettingInt(settingObject + "Anchor"), 3), 0);
            int xOffset = SettingsManager.getSettingInt(settingObject + "XOffset");
            int yOffset = SettingsManager.getSettingInt(settingObject + "YOffset");
            int notificationLength = SettingsManager.getSettingInt(settingObject + "Duration");
            int groupnr = Math.Max(Math.Min(SettingsManager.getSettingInt(settingObject + "Group"), 9), 0);
            int sliderValue = Math.Max(Math.Min(notificationLength, notificationDurationBox.Maximum), notificationDurationBox.Minimum);

            MainForm.prevent_settings_update = true;
            notificationDurationLabel.Text = String.Format("Duration ({0})", sliderValue == notificationDurationBox.Maximum ? "INF" : sliderValue.ToString() + "s");
            notificationDurationBox.Value = sliderValue;
            notificationGroupBox.SelectedIndex = groupnr;
            notificationXOffsetBox.Text = xOffset.ToString();
            notificationYOffsetBox.Text = yOffset.ToString();
            notificationAnchorBox.SelectedIndex = anchor;
            MainForm.prevent_settings_update = false;
        }

        private void notificationAnchorBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            SettingsManager.setSetting(selectedNotificationObject() + "Anchor", notificationAnchorBox.SelectedIndex);
        }

        private void groupSelectionList_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            SettingsManager.setSetting(selectedNotificationObject() + "Group", notificationGroupBox.SelectedIndex);
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
            notificationDurationLabel.Text = String.Format("Duration ({0})", sliderValue == notificationDurationBox.Maximum ? "INF" : sliderValue.ToString() + "s");
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

        private void ControlMouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

        private void monitorAnchorDropdown_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("MonitorAnchor", monitorAnchorDropdown.SelectedIndex);
        }
    }
}
