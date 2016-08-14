using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class MainTab : Form, TabInterface {
        ToolTip tooltip = UIManager.CreateTooltip();
        public MainTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            string language = SettingsManager.getSettingString("TibialyzerLanguage");
            switch(language) {
                case "en-US":
                    languageDropDownList.SelectedIndex = 0;
                    break;
                case "nl-NL":
                    languageDropDownList.SelectedIndex = 1;
                    break;
                case "pl-PL":
                    languageDropDownList.SelectedIndex = 2;
                    break;
                case "pt-BR":
                    languageDropDownList.SelectedIndex = 3;
                    break;
            }
            nameListBox.Items.Clear();
            foreach (string str in SettingsManager.getSetting("Names")) {
                nameListBox.Items.Add(str);
            }
            nameListBox.RefreshControl();
            nameListBox.ItemsChanged += NameListBox_ItemsChanged;

            automaticallyDetectCharacterCheckbox.Checked = SettingsManager.getSettingBool("AutomaticallyDetectCharacter");
            automaticallyDownloadAddressesCheckbox.Checked = SettingsManager.getSettingBool("AutomaticallyDownloadAddresses");

            SwitchClients(SettingsManager.getSettingString("TibiaClient"));
        }

        public void ApplyLocalization() {
            tooltip.RemoveAll();

            gettingStartedGuideButton.Text = Tibialyzer.Translation.MainTab.gettingStartedGuideButton;
            updateMemoryAddressesHeader.Text = Tibialyzer.Translation.MainTab.updateMemoryAddressesHeader;
            chromeFlashClientButton.Text = Tibialyzer.Translation.MainTab.chromeFlashClientButton;
            downloadNewAddressesButton.Text = Tibialyzer.Translation.MainTab.downloadNewAddressesButton;
            automaticallyDetectCharacterCheckbox.Text = Tibialyzer.Translation.MainTab.automaticallyDetectCharacterCheckbox;
            executeCommandButton.Text = Tibialyzer.Translation.MainTab.executeCommandButton;
            tibiaClientSelectionHeader.Text = Tibialyzer.Translation.MainTab.tibiaClientSelectionHeader;
            commonIssuesButton.Text = Tibialyzer.Translation.MainTab.commonIssuesButton;
            tibia11ClientButton.Text = Tibialyzer.Translation.MainTab.tibia11ClientButton;
            gettingStartedVideoButton.Text = Tibialyzer.Translation.MainTab.gettingStartedVideoButton;
            generatePartialAddressesButton.Text = Tibialyzer.Translation.MainTab.generatePartialAddressesButton;
            firefoxFlashClientButton.Text = Tibialyzer.Translation.MainTab.firefoxFlashClientButton;
            classicClientButton.Text = Tibialyzer.Translation.MainTab.classicClientButton;
            executeTibialyzerCommandHeader.Text = Tibialyzer.Translation.MainTab.executeTibialyzerCommandHeader;
            resourcesHeader.Text = Tibialyzer.Translation.MainTab.resourcesHeader;
            characterNamesHeader.Text = Tibialyzer.Translation.MainTab.characterNamesHeader;
            updateDatabaseButton.Text = Tibialyzer.Translation.MainTab.updateDatabaseButton;
            languageSelectionHeader.Text = Tibialyzer.Translation.MainTab.languageSelectionHeader;
            automaticallyDownloadAddressesCheckbox.Text = Tibialyzer.Translation.MainTab.automaticallyDownloadAddressesCheckbox;
            tooltip.SetToolTip(downloadNewAddressesButton, Tibialyzer.Translation.MainTab.downloadNewAddressesButtonTooltip);
            tooltip.SetToolTip(updateDatabaseButton, Tibialyzer.Translation.MainTab.updateDatabaseButtonTooltip);
            tooltip.SetToolTip(languageDropDownList, Tibialyzer.Translation.MainTab.languageDropDownListTooltip);
            tooltip.SetToolTip(executeCommandButton, Tibialyzer.Translation.MainTab.executeCommandButtonTooltip);
            tooltip.SetToolTip(generatePartialAddressesButton, Tibialyzer.Translation.MainTab.generatePartialAddressesButtonTooltip);
            tooltip.SetToolTip(automaticallyDownloadAddressesCheckbox, Tibialyzer.Translation.MainTab.automaticallyDownloadAddressesCheckboxTooltip);
            int languageDropDownListSelectedIndex = languageDropDownList.SelectedIndex;
            languageDropDownList.Items.Clear();
            languageDropDownList.Items.Add(Tibialyzer.Translation.MainTab.languageDropDownList_0);
            languageDropDownList.Items.Add(Tibialyzer.Translation.MainTab.languageDropDownList_1);
            languageDropDownList.Items.Add(Tibialyzer.Translation.MainTab.languageDropDownList_2);
            languageDropDownList.Items.Add(Tibialyzer.Translation.MainTab.languageDropDownList_3);
            languageDropDownList.SelectedIndex = languageDropDownListSelectedIndex;
        }

        private void NameListBox_ItemsChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            List<string> names = new List<string>();

            foreach (object obj in (sender as PrettyListBox).Items) {
                names.Add(obj.ToString());
            }
            SettingsManager.setSetting("Names", names);
        }

        private void commandTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                CommandManager.ExecuteCommand((sender as TextBox).Text);
                e.Handled = true;
            }
        }

        private void executeCommand_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand(commandTextBox.Text);
        }
        
        private static void SaveMemoryAddresses(string str) {
            try {
                using (StreamWriter writer = new StreamWriter(Constants.MemoryAddresses, false)) {
                    writer.Write(str);
                }
                MemoryReader.InitializeMemoryAddresses();
            } catch(Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }

        public static void DownloadNewAddresses() {
            try {
                using (WebClient client = new WebClient()) {
                    string html = client.DownloadString("https://raw.githubusercontent.com/Mytherin/Tibialyzer/master/Tibialyzer/Database/MemoryAddresses.txt");
                    SaveMemoryAddresses(html);
                }
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }

        private void downloadAddressButton_Click(object sender, EventArgs e) {
            DownloadNewAddresses();
        }

        private void generateAddressButton_Click(object sender, EventArgs e) {
            uint baseptr = 0x400000;

            Dictionary<string, string> addr_map = new Dictionary<string, string> {
                {"adrNameStart", "BattleListAddress"},
                {"adrXOR", "XORAddress"},
                {"adrMyHP", "HealthAddress"},
                {"adrMyMaxHP", "MaxHealthAddress"},
                {"adrMyMana", "ManaAddress"},
                {"adrMyMaxMana", "MaxManaAddress"},
                {"tibiachatlog_struct", "TabsBaseAddress"},
                {"adrNum", "PlayerIDAddress"},
             };

            using(WebClient client = new WebClient()) {
                string html = client.DownloadString("https://raw.githubusercontent.com/blackdtools/Blackd-Safe-Cheats/master/config.int");

                string addresses = "\n";
                foreach(var kvp in addr_map) {
                    Regex rx = new Regex(String.Format("{0}=&H([0-9A-Fa-f]+)", kvp.Key));
                    MatchCollection matches = rx.Matches(html);

                    foreach(Match match in matches) {
                        uint x = uint.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber) - baseptr;
                        addresses += String.Format("{0}=0x{1}\n", kvp.Value, x.ToString("X"));
                        break;
                    }
                }
                SaveMemoryAddresses(addresses);
            }
        }

        private void updateDatabaseLabel_Click(object sender, EventArgs e) {
            try {
                using (WebClient client = new WebClient()) {
                    client.DownloadFile("https://github.com/Mytherin/Tibialyzer/raw/master/Tibialyzer/Database/database.db", Constants.NewDatabaseFile);
                    MessageBox.Show("Database downloaded, please restart Tibialyzer to update.");
                }
            } catch {
                MessageBox.Show("Failed to download database.");
            }
        }

        private void languageBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            switch((sender as PrettyDropDownList).SelectedIndex) {
                case 0:
                    languageImageBox.Image = StyleManager.GetImage("flag-usa.png");
                    MainForm.mainForm.ChangeLanguage("en-US");
                    break;
                case 1:
                    languageImageBox.Image = StyleManager.GetImage("flag-nl.png");
                    MainForm.mainForm.ChangeLanguage("nl-NL");
                    break;
                case 2:
                    languageImageBox.Image = StyleManager.GetImage("flag-pl.png");
                    MainForm.mainForm.ChangeLanguage("pl-PL");
                    break;
                case 3:
                    languageImageBox.Image = StyleManager.GetImage("flag-br.png");
                    MainForm.mainForm.ChangeLanguage("pt-BR");
                    break;
                default:
                    break;
            }
        }

        private void gettingStartedGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Quick-Start-Guide");
        }

        private void commonIssuesButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Issues");
        }

        private void gettingStartedVideoButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://www.youtube.com/watch?v=j9u4DsJObBs");
        }

        private void SwitchClients(string clientType, bool writeSetting = true) {
            classicClientButton.Enabled = true;
            tibia11ClientButton.Enabled = true;
            firefoxFlashClientButton.Enabled = true;
            chromeFlashClientButton.Enabled = true;

            ProcessManager.TibiaClientType = clientType;
            ProcessManager.TibiaClientName = null;
            ReadMemoryManager.FlashClient = false;
            if (clientType == "Classic") {
                classicClientButton.Enabled = false;
                ProcessManager.TibiaClientName = MemoryReader.MemorySettings.ContainsKey("classicclientname") ? MemoryReader.MemorySettings["classicclientname"] : "Tibia";
            } else if (clientType == "Tibia11") {
                ProcessManager.TibiaClientName = MemoryReader.MemorySettings.ContainsKey("tibia11name") ? MemoryReader.MemorySettings["tibia11name"] : "client";
                tibia11ClientButton.Enabled = false;
            } else if (clientType == "Flash-Firefox") {
                ReadMemoryManager.FlashClient = true;
                firefoxFlashClientButton.Enabled = false;
            } else if (clientType == "Flash-Chrome") {
                ReadMemoryManager.FlashClient = true;
                chromeFlashClientButton.Enabled = false;
            }
            if (writeSetting) {
                SettingsManager.setSetting("TibiaClient", clientType);
            }
        }

        private void classicClientButton_Click(object sender, EventArgs e) {
            SwitchClients("Classic");
        }

        private void tibia11ClientButton_Click(object sender, EventArgs e) {
            SwitchClients("Tibia11");
        }

        private void firefoxFlashClientButton_Click(object sender, EventArgs e) {
            SwitchClients("Flash-Firefox");
        }

        private void chromeFlashClientButton_Click(object sender, EventArgs e) {
            SwitchClients("Flash-Chrome");
        }
        
        private void automaticallyDetectCharacterCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutomaticallyDetectCharacter", (sender as CheckBox).Checked);
        }

        private void automaticallyDownloadAddressesCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutomaticallyDownloadAddresses", (sender as CheckBox).Checked);
        }
    }
}
