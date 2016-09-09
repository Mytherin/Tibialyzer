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
            ApplyLocalization();
        }

        public void InitializeSettings() {
            refreshCenterImage();

            backgroundImageScale.Value = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitBackgroundScale")));
            xOffsetBox.Text = SettingsManager.getSettingInt("PortraitBackgroundXOffset").ToString();
            yOffsetBox.Text = SettingsManager.getSettingInt("PortraitBackgroundYOffset").ToString();
            centerImageScale.Value = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitCenterScale")));
            xOffsetBoxCenter.Text = SettingsManager.getSettingInt("PortraitCenterXOffset").ToString();
            yOffsetBoxCenter.Text = SettingsManager.getSettingInt("PortraitCenterYOffset").ToString();
        }

        public void ApplyLocalization() {
            backgroundScaleHeader.Text = Tibialyzer.Translation.PortraitTab.backgroundScaleHeader;
            scaleHeader.Text = Tibialyzer.Translation.PortraitTab.scaleHeader;
            centerImageHeader.Text = Tibialyzer.Translation.PortraitTab.centerImageHeader;
            yOffsetLabel.Text = Tibialyzer.Translation.PortraitTab.yOffsetLabel;
            xOffsetLabel.Text = Tibialyzer.Translation.PortraitTab.xOffsetLabel;
            refreshButton.Text = Tibialyzer.Translation.PortraitTab.refreshButton;
            yOffsetCenterLabel.Text = Tibialyzer.Translation.PortraitTab.yOffsetCenterLabel;
            changeCenterImageButton.Text = Tibialyzer.Translation.PortraitTab.changeCenterImageButton;
            xOffsetCenterLabel.Text = Tibialyzer.Translation.PortraitTab.xOffsetCenterLabel;
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
        
        private void refreshCenterImage() {
            string centerLocation = SettingsManager.getSettingString("PortraitCenterImage");
            if (centerLocation == null) return;
            centerImageBox.Image = LoadImageFromPath(centerLocation);
            if (centerImageBox.Image == null) {
                OutfiterOutfit outfit = new OutfiterOutfit();
                outfit.FromString(centerLocation);
                centerImageBox.Image = outfit.GetImage();
                outfiterCode.Text = centerLocation;
            }
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

        private void outfiterButton_Click(object sender, EventArgs e) {
            string code = outfiterCode.Text;
            SettingsManager.setSetting("PortraitCenterImage", code);
            refreshCenterImage();
        }
    }
}
