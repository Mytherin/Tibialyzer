using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class MainTab : Form, TabInterface {
        public MainTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            this.stackAllItemsCheckbox.Checked = SettingsManager.getSettingBool("StackAllItems");
            this.ignoreLowExperienceButton.Checked = SettingsManager.getSettingBool("IgnoreLowExperience");
            this.ignoreLowExperienceBox.Enabled = this.ignoreLowExperienceButton.Checked;
            this.ignoreLowExperienceBox.Text = SettingsManager.getSettingInt("IgnoreLowExperienceValue").ToString();
            this.saveAllLootCheckbox.Checked = SettingsManager.getSettingBool("AutomaticallyWriteLootToFile");
            this.maxDamagePlayersBox.SelectedIndex = Math.Min(Math.Max(SettingsManager.getSettingInt("MaxDamageChartPlayers"), 0), maxDamagePlayersBox.Items.Count - 1);
            
            nameListBox.Items.Clear();
            foreach (string str in SettingsManager.getSetting("Names")) {
                nameListBox.Items.Add(str);
            }
            nameListBox.RefreshControl();
            nameListBox.ItemsChanged += NameListBox_ItemsChanged;
        }

        public void InitializeTooltips() {
            ToolTip tooltip = UIManager.CreateTooltip();

            tooltip.SetToolTip(saveDamageImageButton, "Saves an image of the damage chart (damage@) to a file.");
            tooltip.SetToolTip(saveLootImageButton, "Saves an image of the loot command (loot@) to a file.");
            tooltip.SetToolTip(stackAllItemsCheckbox, "In the loot@ view, display all items as if they were stackable.");
            tooltip.SetToolTip(ignoreLowExperienceButton, "In the loot@ view, do not display creatures that give less than {Exp Value} experience.");
            tooltip.SetToolTip(saveAllLootCheckbox, String.Format("Whenever you find loot, save the loot message to the file {0}.", Constants.BigLootFile));
            tooltip.SetToolTip(selectClientProgramButton, "Select the Tibia client to scan from. This should be either the C++ Client or the Flash Client, although you can select any program.");
            tooltip.SetToolTip(executeButton, "Execute a Tibialyzer command directly.");
        }

        private void detectFlashClientButton_Click(object sender, EventArgs e) {
            ProcessManager.DetectFlashClient();
        }

        private void stackAllItemsCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("StackAllItems", (sender as CheckBox).Checked);
        }

        private void selectClientButton_Click(object sender, EventArgs e) {
            SelectProcessForm form = new SelectProcessForm();
            form.StartPosition = FormStartPosition.Manual;

            form.SetDesktopLocation(this.DesktopLocation.X + (this.Width - form.Width) / 2, this.DesktopLocation.Y + (this.Height - form.Height) / 2);
            form.Show();
        }

        private void saveLootImage_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "png";
            dialog.Title = "Save Loot Image";
            if (File.Exists("loot_screenshot.png")) {
                int i = 1;
                while (File.Exists("loot_screenshot (" + i.ToString() + ").png")) i++;
                dialog.FileName = "loot_screenshot (" + i.ToString() + ").png";
            } else {
                dialog.FileName = "loot_screenshot.png";
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                CommandManager.ExecuteCommand("loot" + Constants.CommandSymbol + "screenshot" + Constants.CommandSymbol + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/"));
            }

        }

        private void saveAllLootCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutomaticallyWriteLootToFile", (sender as CheckBox).Checked);
        }

        private void NameListBox_ItemsChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            List<string> names = new List<string>();

            foreach (object obj in (sender as PrettyListBox).Items) {
                names.Add(obj.ToString());
            }
            SettingsManager.setSetting("Names", names);
        }

        private void damageButton_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "png";
            dialog.Title = "Save Damage Image";
            if (File.Exists("damage_screenshot.png")) {
                int i = 1;
                while (File.Exists("damage_screenshot (" + i.ToString() + ").png")) i++;
                dialog.FileName = "damage_screenshot (" + i.ToString() + ").png";
            } else {
                dialog.FileName = "damage_screenshot.png";
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                CommandManager.ExecuteCommand("damage" + Constants.CommandSymbol + "screenshot" + Constants.CommandSymbol + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/"));
            }
        }


        private void saveSummaryImageButton_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "png";
            dialog.Title = "Save Damage Image";
            if (File.Exists("summary_screenshot.png")) {
                int i = 1;
                while (File.Exists("summary_screenshot (" + i.ToString() + ").png")) i++;
                dialog.FileName = "summary_screenshot (" + i.ToString() + ").png";
            } else {
                dialog.FileName = "summary_screenshot.png";
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                CommandManager.ExecuteCommand("summary" + Constants.CommandSymbol + "screenshot" + Constants.CommandSymbol + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/"));
            }
        }

        private void commandTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                CommandManager.ExecuteCommand((sender as TextBox).Text);
                e.Handled = true;
            }
        }
        private void ignoreLowExperienceButton_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("IgnoreLowExperience", (sender as CheckBox).Checked);
            ignoreLowExperienceBox.Enabled = (sender as CheckBox).Checked;
        }

        private void ignoreLowExperienceBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            int value;
            if (int.TryParse(ignoreLowExperienceBox.Text, out value)) {
                SettingsManager.setSetting("IgnoreLowExperienceValue", value);
            }
        }

        private void executeCommand_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand(commandTextBox.Text);
        }

        private void gettingStartedGuide_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Quick-Start-Guide");
        }

        private void commandsGuide_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Loot-Management-Guide");
        }

        private void popupsGuide_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Popup-Guide");
        }

        private void issuesGuide_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Issues");
        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

        private void maxDamagePlayersBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("MaxDamageChartPlayers", maxDamagePlayersBox.SelectedIndex);
        }
    }
}
