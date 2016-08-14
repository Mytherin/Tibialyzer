using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class AutoHotkeyTab : Form, TabInterface {
        ToolTip tooltip = UIManager.CreateTooltip();
        public AutoHotkeyTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            this.startScriptOnStartupBox.Checked = SettingsManager.getSettingBool("StartAutohotkeyAutomatically");
            this.exitScriptOnExitCheckbox.Checked = SettingsManager.getSettingBool("ShutdownAutohotkeyOnExit");
            this.suspendedAnchorDropDownList.SelectedIndex = Math.Min(Math.Max(SettingsManager.getSettingInt("SuspendedNotificationAnchor"), 0), 3);
            this.suspendedXOffsetBox.Text = SettingsManager.getSettingInt("SuspendedNotificationXOffset").ToString();
            this.suspendedYOffsetBox.Text = SettingsManager.getSettingInt("SuspendedNotificationYOffset").ToString();

            string massiveString = "";
            foreach (string str in SettingsManager.getSetting("AutoHotkeySettings")) {
                massiveString += str + "\n";
            }
            this.autoHotkeyGridSettings.Text = massiveString;
            (this.autoHotkeyGridSettings as RichTextBoxAutoHotkey).RefreshSyntax();
        }

        public void ApplyLocalization() {
            tooltip.RemoveAll();
            yOffsetLabel.Text = Tibialyzer.Translation.AutoHotkeyTab.yOffsetLabel;
            positionOffsetHeader.Text = Tibialyzer.Translation.AutoHotkeyTab.positionOffsetHeader;
            xOffsetLabel.Text = Tibialyzer.Translation.AutoHotkeyTab.xOffsetLabel;
            anchorHeader.Text = Tibialyzer.Translation.AutoHotkeyTab.anchorHeader;
            closeSuspendedWindowButton.Text = Tibialyzer.Translation.AutoHotkeyTab.closeSuspendedWindowButton;
            autoHotkeyOptionsHeader.Text = Tibialyzer.Translation.AutoHotkeyTab.autoHotkeyOptionsHeader;
            shutdownAutoHotkeyButton.Text = Tibialyzer.Translation.AutoHotkeyTab.shutdownAutoHotkeyButton;
            autoHotkeyDownloadHeader.Text = Tibialyzer.Translation.AutoHotkeyTab.autoHotkeyDownloadHeader;
            testingHeader.Text = Tibialyzer.Translation.AutoHotkeyTab.testingHeader;
            suspendedTestButton.Text = Tibialyzer.Translation.AutoHotkeyTab.suspendedTestButton;
            startScriptOnStartupBox.Text = Tibialyzer.Translation.AutoHotkeyTab.startScriptOnStartupBox;
            startAutoHotkeyHeader.Text = Tibialyzer.Translation.AutoHotkeyTab.startAutoHotkeyHeader;
            startAutoHotkeyButton.Text = Tibialyzer.Translation.AutoHotkeyTab.startAutoHotkeyButton;
            exitScriptOnExitCheckbox.Text = Tibialyzer.Translation.AutoHotkeyTab.exitScriptOnExitCheckbox;
            downloadAutoHotkeyButton.Text = Tibialyzer.Translation.AutoHotkeyTab.downloadAutoHotkeyButton;
            autoHotkeyScriptHeader.Text = Tibialyzer.Translation.AutoHotkeyTab.autoHotkeyScriptHeader;
            tooltip.SetToolTip(suspendedAnchorDropDownList, Tibialyzer.Translation.AutoHotkeyTab.suspendedAnchorDropDownListTooltip);
            tooltip.SetToolTip(downloadAutoHotkeyButton, Tibialyzer.Translation.AutoHotkeyTab.downloadAutoHotkeyButtonTooltip);
            int suspendedAnchorDropDownListSelectedIndex = suspendedAnchorDropDownList.SelectedIndex;
            suspendedAnchorDropDownList.Items.Clear();
            suspendedAnchorDropDownList.Items.Add(Tibialyzer.Translation.AutoHotkeyTab.suspendedAnchorDropDownList_0);
            suspendedAnchorDropDownList.Items.Add(Tibialyzer.Translation.AutoHotkeyTab.suspendedAnchorDropDownList_1);
            suspendedAnchorDropDownList.Items.Add(Tibialyzer.Translation.AutoHotkeyTab.suspendedAnchorDropDownList_2);
            suspendedAnchorDropDownList.Items.Add(Tibialyzer.Translation.AutoHotkeyTab.suspendedAnchorDropDownList_3);
            suspendedAnchorDropDownList.SelectedIndex = suspendedAnchorDropDownListSelectedIndex;
        }

        private void startAutohotkeyScript_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("StartAutohotkeyAutomatically", (sender as CheckBox).Checked.ToString());
        }
        private void shutdownOnExit_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("ShutdownAutohotkeyOnExit", (sender as CheckBox).Checked.ToString());
        }

        private void downloadAutoHotkey_Click(object sender, EventArgs e) {
            WebClient client = new WebClient();

            client.DownloadDataCompleted += Client_DownloadDataCompleted;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;

            downloadBar.Visible = true;

            client.DownloadDataAsync(new Uri(Constants.AutoHotkeyURL));
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            this.downloadBar.Value = e.ProgressPercentage;
            this.downloadBar.Maximum = 100;
        }

        private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
            try {
                string filepath = System.IO.Path.GetTempPath() + "autohotkeyinstaller.exe";
                Console.WriteLine(filepath);
                File.WriteAllBytes(filepath, e.Result);
                System.Diagnostics.Process.Start(filepath);
            } catch {
            }
            downloadBar.Visible = false;
        }

        private void autoHotkeyGridSettings_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            AutoHotkeyManager.UpdateSettings(autoHotkeyGridSettings.Text.Split('\n').ToList());
        }

        private void startAutoHotkey_Click(object sender, EventArgs e) {
            AutoHotkeyManager.StartAutohotkey();
        }

        private void shutdownAutoHotkey_Click(object sender, EventArgs e) {
            AutoHotkeyManager.ShutdownAutohotkey();
        }

        private void suspendedTest_Click(object sender, EventArgs e) {
            AutoHotkeyManager.ShowSuspendedWindow(true);
        }

        private void closeSuspendedWindow_Click(object sender, EventArgs e) {
            AutoHotkeyManager.CloseSuspendedWindow();
        }

        private void suspendedAnchor_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("SuspendedNotificationAnchor", (sender as ComboBox).SelectedIndex);
        }

        private void suspendedXOffset_TextChanged(object sender, EventArgs e) {
            int xOffset;
            if (int.TryParse((sender as TextBox).Text, out xOffset)) {
                SettingsManager.setSetting("SuspendedNotificationXOffset", xOffset);
            }
        }

        private void suspendedYOffset_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            int yOffset;
            if (int.TryParse((sender as TextBox).Text, out yOffset)) {
                SettingsManager.setSetting("SuspendedNotificationYOffset", yOffset);
            }
        }
    }
}
