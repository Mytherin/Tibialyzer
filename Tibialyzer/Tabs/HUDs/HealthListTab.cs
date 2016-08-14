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
    public partial class HealthListTab : Form, TabInterface {
        public HealthListTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            displayPlayerNameCheckbox.Checked = SettingsManager.getSettingBool("HealthListDisplayNames");
            displayPlayerImageCheckbox.Checked = SettingsManager.getSettingBool("HealthListDisplayIcons");

            nameListBox.Items.Clear();
            foreach (string str in SettingsManager.getSetting("HealthListPlayerNames")) {
                nameListBox.Items.Add(str);
            }
            nameListBox.RefreshControl();
            nameListBox.ItemsChanged += NameListBox_ItemsChanged; ;
        }

        public void ApplyLocalization() {
            refreshButton.Text = Tibialyzer.Translation.HealthListTab.refreshButton;
            healthListHeader.Text = Tibialyzer.Translation.HealthListTab.healthListHeader;
            browseButton.Text = Tibialyzer.Translation.HealthListTab.browseButton;
            displayPlayerImageCheckbox.Text = Tibialyzer.Translation.HealthListTab.displayPlayerImageCheckbox;
            displayPlayerNameCheckbox.Text = Tibialyzer.Translation.HealthListTab.displayPlayerNameCheckbox;
            playerImageHeader.Text = Tibialyzer.Translation.HealthListTab.playerImageHeader;
            displayOptionsHeader.Text = Tibialyzer.Translation.HealthListTab.displayOptionsHeader;
        }

        private void NameListBox_ItemsChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            List<string> names = new List<string>();

            foreach (object obj in (sender as PrettyListBox).Items) {
                names.Add(obj.ToString());
            }
            SettingsManager.setSetting("HealthListPlayerNames", names);
        }

        private void nameListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            
            string imagePath = SettingsManager.getSettingString("HealthListImage" + nameListBox.SelectedIndex);

            playerImageBox.Image = null;
            if (imagePath != null) {
                try {
                    playerImageBox.Image = Image.FromFile(imagePath);
                } catch { }
            }
        }

        private void changePlayerImageButton_Click(object sender, EventArgs e) {
            int index = nameListBox.SelectedIndex;
            try {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Select Player Image";
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    // attempt to load the image
                    playerImageBox.Image = Image.FromFile(dialog.FileName);
                    SettingsManager.setSetting("HealthListImage" + index.ToString(), dialog.FileName);
                }
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }

        private void displayPlayerNameBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("HealthListDisplayNames", (sender as CheckBox).Checked);
        }

        private void displayPlayerImageBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("HealthListDisplayIcons", (sender as CheckBox).Checked);
        }

        private void refreshButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("hud@healthlist");
        }
    }
}
