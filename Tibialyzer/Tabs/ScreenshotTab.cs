using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class ScreenshotTab : Form, TabInterface {
        public ScreenshotTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            this.screenshotAdvanceCheckbox.Checked = SettingsManager.getSettingBool("AutoScreenshotAdvance");
            this.screenshotOnRareItemCheckbox.Checked = SettingsManager.getSettingBool("AutoScreenshotItemDrop");
            this.screenshotDeathCheckbox.Checked = SettingsManager.getSettingBool("AutoScreenshotDeath");
            this.screenshotValueBox.Text = SettingsManager.getSettingString("ScreenshotRareItemValue");

            this.enableScreenshotCheckbox.Checked = SettingsManager.getSettingBool("EnableScreenshots");
            if (SettingsManager.getSettingString("ScreenshotPath") == null || !Directory.Exists(SettingsManager.getSettingString("ScreenshotPath"))) {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                SettingsManager.setSetting("ScreenshotPath", path);
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            }

            screenshotDisplayList.ReadOnly = true;
            screenshotDisplayList.AttemptDeleteItem += ScreenshotDisplayList_AttemptDeleteItem;

            screenshotPathBox.Text = SettingsManager.getSettingString("ScreenshotPath");
            refreshScreenshots();
        }

        public void ApplyLocalization() {
            enableScreenshotCheckbox.Text = Tibialyzer.Translation.ScreenshotTab.enableScreenshotCheckbox;
            changeScreenshotDirectoryButton.Text = Tibialyzer.Translation.ScreenshotTab.changeScreenshotDirectoryButton;
            screenshotValueLabel.Text = Tibialyzer.Translation.ScreenshotTab.screenshotValueLabel;
            screenshotListHeader.Text = Tibialyzer.Translation.ScreenshotTab.screenshotListHeader;
            screenshotAdvanceCheckbox.Text = Tibialyzer.Translation.ScreenshotTab.screenshotAdvanceCheckbox;
            screenshotDeathCheckbox.Text = Tibialyzer.Translation.ScreenshotTab.screenshotDeathCheckbox;
            screenshotDirectoryHeader.Text = Tibialyzer.Translation.ScreenshotTab.screenshotDirectoryHeader;
            screenshotOptionsHeader.Text = Tibialyzer.Translation.ScreenshotTab.screenshotOptionsHeader;
            openScreenshotDirectoryButton.Text = Tibialyzer.Translation.ScreenshotTab.openScreenshotDirectoryButton;
            screenshotOnRareItemCheckbox.Text = Tibialyzer.Translation.ScreenshotTab.screenshotOnRareItemCheckbox;
        }

        public void refreshScreenshots() {
            string selectedValue = screenshotDisplayList.SelectedIndex >= 0 ? screenshotDisplayList.Items[screenshotDisplayList.SelectedIndex].ToString() : null;
            int index = 0;

            string path = SettingsManager.getSettingString("ScreenshotPath");
            if (path == null) return;

            if (!Directory.Exists(path)) {
                return;
            }

            string[] files = Directory.GetFiles(path);

            refreshingScreenshots = true;

            screenshotDisplayList.Items.Clear();
            foreach (string file in files) {
                if (Constants.ImageExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase)) { //check if file is an image
                    string f = Path.GetFileName(file);
                    if (f == selectedValue) {
                        index = screenshotDisplayList.Items.Count;
                    }
                    screenshotDisplayList.Items.Add(f);
                }
            }

            refreshingScreenshots = false;
            if (screenshotDisplayList.Items.Count > 0) {
                screenshotDisplayList.SelectedIndex = index;
            }
        }
        private void enableScreenshotBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("EnableScreenshots", (sender as CheckBox).Checked.ToString());
        }
        private void screenshotBrowse_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = SettingsManager.getSettingString("ScreenshotPath");
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                SettingsManager.setSetting("ScreenshotPath", folderBrowserDialog.SelectedPath);
                screenshotPathBox.Text = folderBrowserDialog.SelectedPath;
                refreshScreenshots();
            }
        }

        private void autoScreenshot_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutoScreenshotAdvance", (sender as CheckBox).Checked.ToString());
        }

        private void autoScreenshotDrop_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutoScreenshotItemDrop", (sender as CheckBox).Checked.ToString());
        }

        private void autoScreenshotDeath_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutoScreenshotDeath", (sender as CheckBox).Checked.ToString());
        }

        bool refreshingScreenshots = false;
        private void screenshotList_SelectedIndexChanged(object sender, EventArgs e) {
            if (refreshingScreenshots) return;
            if (screenshotDisplayList.SelectedIndex >= 0) {
                string selectedImage = screenshotDisplayList.Items[screenshotDisplayList.SelectedIndex].ToString();

                string path = SettingsManager.getSettingString("ScreenshotPath");
                if (path == null) return;

                string imagePath = Path.Combine(path, selectedImage);
                if (!File.Exists(imagePath)) return;
                try {
                    Image image = Image.FromFile(imagePath);
                    if (image != null) {
                        Image oldImage = screenshotBox.Image;
                        screenshotBox.Image = image;
                        if (oldImage != null) {
                            oldImage.Dispose();
                        }
                        screenshotTitleHeader.Text = selectedImage;
                    }
                } catch {

                }
            }
        }

        private void ScreenshotDisplayList_AttemptDeleteItem(object sender, EventArgs e) {
            if (screenshotDisplayList.SelectedIndex >= 0) {
                string fileName = screenshotDisplayList.Text;
                string path = SettingsManager.getSettingString("ScreenshotPath");
                if (path == null) return;

                string imagePath = Path.Combine(path, fileName);
                if (!File.Exists(imagePath)) return;

                Image oldImage = screenshotBox.Image;
                screenshotBox.Image = null;
                if (oldImage != null) {
                    oldImage.Dispose();
                }

                try {
                    File.Delete(imagePath);
                } catch {
                    return;
                }

                screenshotDisplayList.Items.RemoveAt(screenshotDisplayList.SelectedIndex);
                refreshScreenshots();
            }
        }

        private void openInExplorer_Click(object sender, EventArgs e) {
            string path = SettingsManager.getSettingString("ScreenshotPath");
            if (path == null) return;
            Process.Start(path);
        }

        private bool imageStretched = false;
        private Size initialSize;
        private Point initialLocation;
        private void screenshotBox_Click(object sender, EventArgs e) {
            if (imageStretched) {
                (sender as Control).Location = initialLocation;
                (sender as Control).Size = initialSize;
                imageStretched = false;
            } else {
                initialSize = (sender as Control).Size;
                initialLocation = (sender as Control).Location;
                imageStretched = true;
                (sender as Control).Location = new Point(screenshotListHeader.Location.X - 9, screenshotListHeader.Location.Y);
                (sender as Control).Size = new Size(650, 549);
            }
        }

        private void screenshotValueBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            int value = 0;
            if (int.TryParse((sender as TextBox).Text, out value)) {
                SettingsManager.setSetting("ScreenshotRareItemValue", value);
            }
        }
    }
}
