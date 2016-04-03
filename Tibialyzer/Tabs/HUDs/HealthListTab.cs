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
            InitializeTooltips();
        }

        public void InitializeSettings() {
            displayPlayerNameBox.Checked = SettingsManager.getSettingBool("HealthListDisplayNames");
            displayPlayerImageBox.Checked = SettingsManager.getSettingBool("HealthListDisplayIcons");

            nameListBox.Items.Clear();
            foreach (string str in SettingsManager.getSetting("HealthListPlayerNames")) {
                nameListBox.Items.Add(str);
            }
            nameListBox.RefreshControl();
            nameListBox.ItemsChanged += NameListBox_ItemsChanged; ;
        }

        public void InitializeTooltips() {

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
