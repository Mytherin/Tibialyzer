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
        public AutoHotkeyTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }
        
        public void InitializeSettings() {
            this.startScriptOnStartupBox.Checked = SettingsManager.getSettingBool("StartAutohotkeyAutomatically");
            this.exitScriptOnShutdownBox.Checked = SettingsManager.getSettingBool("ShutdownAutohotkeyOnExit");
            this.suspendedAnchorBox.SelectedIndex = Math.Min(Math.Max(SettingsManager.getSettingInt("SuspendedNotificationAnchor"), 0), 3);
            this.suspendedXOffsetBox.Text = SettingsManager.getSettingInt("SuspendedNotificationXOffset").ToString();
            this.suspendedYOffsetBox.Text = SettingsManager.getSettingInt("SuspendedNotificationYOffset").ToString();
            
            string massiveString = "";
            foreach (string str in SettingsManager.getSetting("AutoHotkeySettings")) {
                massiveString += str + "\n";
            }
            this.autoHotkeyGridSettings.Text = massiveString;
            (this.autoHotkeyGridSettings as RichTextBoxAutoHotkey).RefreshSyntax();
        }

        public void InitializeTooltips() {
            ToolTip tooltip = MainForm.CreateTooltip();

            tooltip.SetToolTip(downloadAutoHotkeyButton, "Download AutoHotkey to the temporary directory and launches an installer. Complete the installer to install AutoHotkey.");
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
