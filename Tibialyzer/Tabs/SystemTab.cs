using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class SystemTab : Form, TabInterface {
        public List<SystemCommand> customCommands = new List<SystemCommand>();

        public SystemTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            customCommands.Clear();
            foreach (string str in SettingsManager.getSetting("CustomCommands")) {
                string[] split = str.Split('#');
                if (split.Length <= 2) continue;
                customCommands.Add(new SystemCommand { tibialyzer_command = split[0], command = split[1], parameters = split[2] });
            }

            if (customCommands.Count == 0) {
                customCommands.Add(new SystemCommand { tibialyzer_command = "Unknown Command", command = "", parameters = "" });
            }

            customCommandList.Items.Clear();
            foreach (SystemCommand c in customCommands) {
                customCommandList.Items.Add(c.tibialyzer_command);
            }
            customCommandList.ItemsChanged += CustomCommandList_ItemsChanged;
            customCommandList.ChangeTextOnly = true;
            customCommandList.AttemptDeleteItem += CustomCommandList_AttemptDeleteItem;
            customCommandList.AttemptNewItem += CustomCommandList_AttemptNewItem;
            customCommandList.RefreshControl();
            CustomCommandList_ItemsChanged(null, null);

            Constants.OBSEnableWindowCapture = SettingsManager.getSettingBool("OBSEnableWindowCapture");
            enableWindowCapture.Checked = Constants.OBSEnableWindowCapture;
        }

        public void InitializeTooltips() {
            ToolTip tooltip = UIManager.CreateTooltip();

            tooltip.SetToolTip(selectUpgradeTibialyzerButton, "Import settings from a previous Tibialyzer. Select the directory in which the previous Tibialyzer is located.");
        }

        public IEnumerable<SystemCommand> GetCustomCommands() {
            for (int i = 0; i < customCommands.Count; i++) {
                SystemCommand command = null;
                try {
                    command = customCommands[i];
                } catch {
                    break;
                }
                yield return command;
            }
            yield break;
        }

        private void RefreshCustomCommandList() {
            int selectedIndex = Math.Min(customCommandList.SelectedIndex, customCommands.Count - 1);

            customCommandList.Items.Clear();
            foreach (SystemCommand c in customCommands) {
                customCommandList.Items.Add(c.tibialyzer_command);
            }
            customCommandList.SelectedIndex = selectedIndex;
        }

        private void SaveCommands() {
            List<string> commands = new List<string>();
            foreach (SystemCommand c in customCommands) {
                commands.Add(string.Format("{0}#{1}#{2}", c.tibialyzer_command, c.command, c.parameters));
            }
            SettingsManager.setSetting("CustomCommands", commands);
        }

        private void CustomCommandList_ItemsChanged(object sender, EventArgs e) {
            for (int i = 0; i < customCommandList.Items.Count; i++) {
                string command = customCommandList.Items[i].ToString();

                customCommands[i].tibialyzer_command = command;
            }
            SaveCommands();
        }
        private void CustomCommandList_AttemptDeleteItem(object sender, EventArgs e) {
            if (customCommandList.SelectedIndex < 0) return;
            customCommands.RemoveAt(customCommandList.SelectedIndex);
            RefreshCustomCommandList();
            SaveCommands();
        }

        private void CustomCommandList_AttemptNewItem(object sender, EventArgs e) {
            customCommands.Add(new SystemCommand { tibialyzer_command = "", command = "", parameters = "" });
            RefreshCustomCommandList();
            SaveCommands();
        }

        private void selectUpgradeTibialyzerButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                string tibialyzerPath = folderBrowserDialog.SelectedPath;
                string settings = System.IO.Path.Combine(tibialyzerPath, "settings.txt");

                if (!File.Exists(settings)) {
                    settings = System.IO.Path.Combine(tibialyzerPath, Constants.SettingsFile);
                    if (!File.Exists(settings)) {
                        MainForm.mainForm.DisplayWarning("Could not find settings.txt in upgrade path.");
                        return;
                    }
                }
                SettingsManager.LoadSettings(settings);
                MainForm.mainForm.initializeSettings();

                string lootDatabase = System.IO.Path.Combine(tibialyzerPath, "loot.db");
                if (!File.Exists(lootDatabase)) {
                    lootDatabase = System.IO.Path.Combine(tibialyzerPath, Constants.LootDatabaseFile);
                    if (!File.Exists(lootDatabase)) {
                        MainForm.mainForm.DisplayWarning("Could not find loot.db in upgrade path.");
                        return;
                    }
                }

                try {
                    LootDatabaseManager.ReplaceDatabase(lootDatabase);
                } catch(Exception ex) {
                    MainForm.mainForm.DisplayWarning(String.Format("Error modifying loot database: {0}", ex.Message));
                    return;
                }

                HuntManager.Initialize();

                string database = System.IO.Path.Combine(tibialyzerPath, "database.db");
                if (!File.Exists(database)) {
                    database = System.IO.Path.Combine(tibialyzerPath, Constants.DatabaseFile);
                    if (!File.Exists(database)) {
                        MainForm.mainForm.DisplayWarning("Could not find database.db in upgrade path.");
                        return;
                    }
                }
                SQLiteConnection databaseConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", database));
                databaseConnection.Open();
                StorageManager.UpdateDatabase(databaseConnection);
            }
        }

        private void customCommandList_SelectedIndexChanged(object sender, EventArgs e) {
            if (customCommandList.SelectedIndex < 0) return;

            customCommandBox.Text = customCommands[customCommandList.SelectedIndex].command;
            customCommandParameterBox.Text = customCommands[customCommandList.SelectedIndex].parameters;
        }

        private void customCommandBox_TextChanged(object sender, EventArgs e) {
            if (customCommandList.SelectedIndex < 0) return;

            customCommands[customCommandList.SelectedIndex].command = customCommandBox.Text;
            SaveCommands();
        }

        private void customCommandParameterBox_TextChanged(object sender, EventArgs e) {
            if (customCommandList.SelectedIndex < 0) return;

            customCommands[customCommandList.SelectedIndex].parameters = customCommandParameterBox.Text;
            SaveCommands();
        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

        private void showPopupWindow_Click(object sender, EventArgs e) {
            PopupManager.ShowPopupContainer();
        }

        private void enableWindowCapture_CheckedChanged(object sender, EventArgs e) {
            SettingsManager.setSetting("OBSEnableWindowCapture", (sender as CheckBox).Checked);
            Constants.OBSEnableWindowCapture = (sender as CheckBox).Checked;
        }
    }
}
