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
        ToolTip tooltip = UIManager.CreateTooltip();

        public SystemTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
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

            automaticallyBackupSettingsCheckbox.Checked = SettingsManager.getSettingBool("AutomaticSettingsBackup");
            Constants.OBSEnableWindowCapture = SettingsManager.getSettingBool("OBSEnableWindowCapture");
            enableWindowCaptureCheckbox.Checked = Constants.OBSEnableWindowCapture;

            this.maxDamageDropDownList.SelectedIndex = Math.Min(Math.Max(SettingsManager.getSettingInt("MaxDamageChartPlayers"), 0), maxDamageDropDownList.Items.Count - 1);
        }

        public void ApplyLocalization() {
            tooltip.RemoveAll();
            customCommandsHeader.Text = Tibialyzer.Translation.SystemTab.customCommandsHeader;
            showPopupWindowButton.Text = Tibialyzer.Translation.SystemTab.showPopupWindowButton;
            selectUpgradeTibialyzerButton.Text = Tibialyzer.Translation.SystemTab.selectUpgradeTibialyzerButton;
            importSettingsHeader.Text = Tibialyzer.Translation.SystemTab.importSettingsHeader;
            saveImagesHeader.Text = Tibialyzer.Translation.SystemTab.saveImagesHeader;
            enableWindowCaptureCheckbox.Text = Tibialyzer.Translation.SystemTab.enableWindowCaptureCheckbox;
            obsSettingsHeader.Text = Tibialyzer.Translation.SystemTab.obsSettingsHeader;
            createBackupButton.Text = Tibialyzer.Translation.SystemTab.createBackupButton;
            restoreBackupButton.Text = Tibialyzer.Translation.SystemTab.restoreBackupButton;
            backupSettingsHeader.Text = Tibialyzer.Translation.SystemTab.backupSettingsHeader;
            saveLootImageButton.Text = Tibialyzer.Translation.SystemTab.saveLootImageButton;
            saveDamageImageButton.Text = Tibialyzer.Translation.SystemTab.saveDamageImageButton;
            automaticallyBackupSettingsCheckbox.Text = Tibialyzer.Translation.SystemTab.automaticallyBackupSettingsCheckbox;
            maxDamagePlayersHeader.Text = Tibialyzer.Translation.SystemTab.maxDamagePlayersHeader;
            parametersHeader.Text = Tibialyzer.Translation.SystemTab.parametersHeader;
            saveSummaryImageButton.Text = Tibialyzer.Translation.SystemTab.saveSummaryImageButton;
            systemCommandHeader.Text = Tibialyzer.Translation.SystemTab.systemCommandHeader;
            tooltip.SetToolTip(selectUpgradeTibialyzerButton, Tibialyzer.Translation.SystemTab.selectUpgradeTibialyzerButtonTooltip);
            tooltip.SetToolTip(saveDamageImageButton, Tibialyzer.Translation.SystemTab.saveDamageImageButtonTooltip);
            tooltip.SetToolTip(saveLootImageButton, Tibialyzer.Translation.SystemTab.saveLootImageButtonTooltip);
            tooltip.SetToolTip(saveSummaryImageButton, Tibialyzer.Translation.SystemTab.saveSummaryImageButtonTooltip);
            int hudAnchorDropDownListSelectedIndex = maxDamageDropDownList.SelectedIndex;
            maxDamageDropDownList.Items.Clear();
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_0);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_1);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_2);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_3);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_4);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_5);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_6);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_7);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_8);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_9);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_10);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_11);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_12);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_13);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_14);
            maxDamageDropDownList.Items.Add(Tibialyzer.Translation.SystemTab.hudAnchorDropDownList_15);
            maxDamageDropDownList.SelectedIndex = hudAnchorDropDownListSelectedIndex;
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
        
        private void showPopupWindow_Click(object sender, EventArgs e) {
            PopupManager.ShowPopupContainer();
        }

        private void enableWindowCapture_CheckedChanged(object sender, EventArgs e) {
            SettingsManager.setSetting("OBSEnableWindowCapture", (sender as CheckBox).Checked);
            Constants.OBSEnableWindowCapture = (sender as CheckBox).Checked;
        }

        private void saveLootImageButton_Click(object sender, EventArgs e) {
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
        private void saveDamageImageButton_Click(object sender, EventArgs e) {
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

        private void maxDamageDropDownList_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("MaxDamageChartPlayers", maxDamageDropDownList.SelectedIndex);
        }

        private void createBackupButton_Click(object sender, EventArgs e) {
            SettingsManager.CreateBackup();
        }

        private void restoreBackupButton_Click(object sender, EventArgs e) {
            SettingsManager.RestoreBackup();
        }

        private void automaticallyBackupSettingsCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutomaticSettingsBackup", (sender as CheckBox).Checked);
        }
    }
}
