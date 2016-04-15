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
    public partial class PortraitTab : Form, TabInterface {
        public PortraitTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            refreshBackgroundImage();
            refreshCenterImage();

            backgroundImageScale.Value = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitBackgroundScale")));
            xOffsetBox.Text = SettingsManager.getSettingInt("PortraitBackgroundXOffset").ToString();
            yOffsetBox.Text = SettingsManager.getSettingInt("PortraitBackgroundYOffset").ToString();
            centerImageScale.Value = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitCenterScale")));
            xOffsetBoxCenter.Text = SettingsManager.getSettingInt("PortraitCenterXOffset").ToString();
            yOffsetBoxCenter.Text = SettingsManager.getSettingInt("PortraitCenterYOffset").ToString();
        }

        public void InitializeTooltips() {

        }

        private Image LoadImageFromPath(string path) {
            if (path != null) {
                try {
                    return Image.FromFile(path);
                } catch {
                    return null;
                }
            }
            return null;
        }

        private void refreshBackgroundImage() {
            backgroundImageBox.Image = LoadImageFromPath(SettingsManager.getSettingString("PortraitBackgroundImage"));
        }

        private void refreshCenterImage() {
            centerImageBox.Image = LoadImageFromPath(SettingsManager.getSettingString("PortraitCenterImage"));
        }

        private void openFileDialog(string setting) {
            try {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Select Player Image";
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    // attempt to load the image
                    SettingsManager.setSetting(setting, dialog.FileName);
                }
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }

        private void changeBackgroundImageButton_Click(object sender, EventArgs e) {
            openFileDialog("PortraitBackgroundImage");
            refreshBackgroundImage();
        }

        private void changeCenterImageButton_Click(object sender, EventArgs e) {
            openFileDialog("PortraitCenterImage");
            refreshCenterImage();
        }

        private void backgroundImageScale_Scroll(object sender, EventArgs e) {
            SettingsManager.setSetting("PortraitBackgroundScale", (sender as TrackBar).Value);
        }

        private void xOffsetBox_TextChanged(object sender, EventArgs e) {
            int offset = 0;
            if (int.TryParse((sender as TextBox).Text, out offset)) {
                SettingsManager.setSetting("PortraitBackgroundXOffset", offset);
            }
        }

        private void yOffsetBox_TextChanged(object sender, EventArgs e) {
            int offset = 0;
            if (int.TryParse((sender as TextBox).Text, out offset)) {
                SettingsManager.setSetting("PortraitBackgroundYOffset", offset);
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

        private void xOffsetBoxCenter_TextChanged(object sender, EventArgs e) {
            int offset = 0;
            if (int.TryParse((sender as TextBox).Text, out offset)) {
                SettingsManager.setSetting("PortraitCenterXOffset", offset);
            }
        }

        private void yOffsetBoxCenter_TextChanged(object sender, EventArgs e) {
            int offset = 0;
            if (int.TryParse((sender as TextBox).Text, out offset)) {
                SettingsManager.setSetting("PortraitCenterYOffset", offset);
            }
        }

        private void centerImageScale_Scroll(object sender, EventArgs e) {
            SettingsManager.setSetting("PortraitCenterScale", (sender as TrackBar).Value);
        }

        private void refreshButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("hud@portrait");
        }
    }
}
