
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.Globalization;
using System.Xml;

namespace Tibialyzer {
    public partial class MainForm : Form {
        public static MainForm mainForm;

        private NotificationForm[] NotificationFormGroups = new NotificationForm[10];

        private bool prevent_settings_update = false;
        private bool minimize_notification = true;
        private ToolTip scan_tooltip = new ToolTip();
        private Stack<TibialyzerCommand> command_stack = new Stack<TibialyzerCommand>();
        public static List<string> NotificationTypes = new List<string> { "Loot Notification", "Damage Notification", "Object List", "City Information", "Creature Loot Information", "Creature Stats Information", "Hunt Information", "Item Information", "NPC Information", "Outfit Information", "Quest Information", "Spell Information", "Quest/Hunt Directions", "Task Form" };
        public static List<string> NotificationTestCommands = new List<string> { "loot@", "damage@", "creature@quara", "city@venore", "creature@demon", "stats@dragon lord", "hunt@formorgar mines", "item@heroic axe", "npc@rashid", "outfit@brotherhood", "quest@killing in the name of", "spell@light healing", "guide@desert dungeon quest", "task@crystal spider" };
        public static List<Type> NotificationTypeObjects = new List<Type>() { typeof(LootDropForm), typeof(DamageChart), typeof(CreatureList), typeof(CityDisplayForm), typeof(CreatureDropsForm), typeof(CreatureStatsForm), typeof(HuntingPlaceForm), typeof(ItemViewForm), typeof(NPCForm), typeof(OutfitForm), typeof(QuestForm), typeof(SpellForm), typeof(QuestGuideForm), typeof(TaskForm) };

        public static StreamWriter fileWriter = null;

        enum ScanningState { Scanning, NoTibia, Stuck };
        ScanningState current_state;

        public static void ExitWithError(string title, string text, bool exit = true) {
            MessageBox.Show(mainForm, text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (exit) {
                System.Environment.Exit(1);
            }
        }

        public MainForm() {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            mainForm = this;
            InitializeComponent();
            makeDraggable(this.Controls);
            this.InitializeTabs();
            switchTab(0);

            LootDatabaseManager.LootChanged += UpdateLootDisplay;

            if (!File.Exists(Constants.DatabaseFile)) {
                ExitWithError("Fatal Error", String.Format("Could not find database file {0}.", Constants.DatabaseFile));
            }

            if (!File.Exists(Constants.NodeDatabase)) {
                ExitWithError("Fatal Error", String.Format("Could not find database file {0}.", Constants.NodeDatabase));
            }

            LootDatabaseManager.Initialize();
            StyleManager.InitializeStyle();
            NotificationForm.Initialize();
            Parser.Initialize();

            prevent_settings_update = true;
            try {
                StorageManager.InitializeStorage();
            } catch (Exception e) {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.\nMessage: {1}", Constants.DatabaseFile, e.Message));
            }
            SettingsManager.LoadSettings(Constants.SettingsFile);
            ProcessManager.Initialize();
            this.initializeSettings();
            this.initializeTooltips();
            try {
                Pathfinder.LoadFromDatabase(Constants.NodeDatabase);
            } catch (Exception e) {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.\nMessage: {1}", Constants.NodeDatabase, e.Message));
            }
            prevent_settings_update = false;

            if (SettingsManager.getSettingBool("StartAutohotkeyAutomatically")) {
                startAutoHotkey_Click(null, null);
            }
            HuntManager.Initialize();

            fileWriter = new StreamWriter(Constants.BigLootFile, true);

            ignoreStamp = createStamp();

            browseTypeBox.SelectedIndex = 0;

            this.Load += MainForm_Load;

            tibialyzerLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync();

            BackgroundWorker bwMissingChunks = new BackgroundWorker();
            bwMissingChunks.DoWork += BwMissingChunks_DoWork;
            bwMissingChunks.RunWorkerAsync();

            MaximumNotificationDuration = notificationDurationBox.Maximum;
            scan_tooltip.AutoPopDelay = 60000;
            scan_tooltip.InitialDelay = 500;
            scan_tooltip.ReshowDelay = 0;
            scan_tooltip.ShowAlways = true;
            scan_tooltip.UseFading = true;

            this.loadTimerImage.Image = StyleManager.GetImage("scanningbar-red.gif");
            this.current_state = ScanningState.NoTibia;
            this.loadTimerImage.Enabled = true;
            scan_tooltip.SetToolTip(this.loadTimerImage, "No Tibia Client Found...");
        }


        private void UpdateLootDisplay() {
            for (int i = 0; i < NotificationFormGroups.Length; i++) {
                if (NotificationFormGroups[i] != null && NotificationFormGroups[i] is LootDropForm) {
                    (NotificationFormGroups[i] as LootDropForm).UpdateLoot();
                }
            }
            if (logButton.Enabled == false) {
                refreshHuntLog(getSelectedHunt());
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            HelpTimer_Elapsed(null, null);
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void initializeTooltips() {
            explanationTooltip.SetToolTip(saveDamageImageButton, "Saves an image of the damage chart (damage@) to a file.");
            explanationTooltip.SetToolTip(saveLootImageButton, "Saves an image of the loot command (loot@) to a file.");
            explanationTooltip.SetToolTip(clearLog, "WARNING: Clears the active hunt, removing all loot from it.");
            explanationTooltip.SetToolTip(saveLogToFileButton, "Saves all the log messages of the currently selected hunt to a file.");
            explanationTooltip.SetToolTip(loadLogFromFileButton, "Loads a set of log messages from a file into the currently selected hunt. ");
            explanationTooltip.SetToolTip(setActiveHuntButton, "Sets the currently selected hunt as the active hunt. Any creatures killed will be added to the currently active hunt. ");
            explanationTooltip.SetToolTip(displayAllCreaturesBox, "In the loot@ command, only creatures specified in the box below are shown if this is selected.");
            explanationTooltip.SetToolTip(switchOnKillBox, "When a creature specified in the box below is killed, this hunt is made the currently active hunt.");
            explanationTooltip.SetToolTip(gatherTrackedKillsBox, "When a creature specified in the box below is killed, the loot of that creature is always added to this hunt (in addition to the active hunt).");
            explanationTooltip.SetToolTip(clearHuntOnStartupBox, "If this is checked, this hunt will be automatically cleared when Tibialyzer is restarted.");
            explanationTooltip.SetToolTip(this.lookModeCheckbox, "When you look (shift+click) at an item, creature or npc in-game, Tibialyzer will automatically open a box displaying information about that object.");
            explanationTooltip.SetToolTip(outfitGenderCheckbox, "Outfit gender displayed in outfit@ searches.");
            explanationTooltip.SetToolTip(copyAdvancesCheckbox, "When you advance in level or skill, the advancement text will be automatically copied for you, so you can easily paste it and notify your friends.");
            explanationTooltip.SetToolTip(eventPopupBox, "When a raid message is send, a notification will appear informing you of the raid.");
            explanationTooltip.SetToolTip(unrecognizedPopupBox, "When you type in an unrecognized command in Tibia chat (unrecognized@), a notification will appear notifying you of this.");
            explanationTooltip.SetToolTip(resetSettingsButton, "Clears all settings and resets them back to the default settings, except for the hunt settings. ");
            explanationTooltip.SetToolTip(popupTypeBox, "Rich notifications are Windows Forms notifications that look pretty. Simple notifications are default Windows bubble notifications. ");
            /*explanationTooltip.SetToolTip(alwaysShowLoot, "When this box is checked, a rich notification is shown every time a creature is killed with the loot of the creature, regardless of what that loot is.");
            explanationTooltip.SetToolTip(rareDropNotificationValueCheckbox, "When an item that is worth at least this amount of gold drops, a notification is displayed.");
            explanationTooltip.SetToolTip(goldCapRatioCheckbox, "When an item that has at least this gold/cap ratio drops, a notification is displayed.");*/
            //explanationTooltip.SetToolTip(specificNotificationCheckbox, "When any item that is specified in the box below drops, a notification is displayed informing you of the dropped item.");
            //explanationTooltip.SetToolTip(notificationLengthSlider, "The amount of time that rich notifications (loot@, creature@) remain on the screen before fading.");
            explanationTooltip.SetToolTip(downloadAutoHotkeyButton, "Download AutoHotkey to the temporary directory and launches an installer. Complete the installer to install AutoHotkey.");
            explanationTooltip.SetToolTip(scanningSpeedTrack, "Set the memory scanning speed of Tibialyzer. Lower settings drastically reduce CPU usage, but increase response time for Tibialyzer to respond to events in-game (such as in-game commands, look events and loot parsing).");
            explanationTooltip.SetToolTip(stackAllItemsCheckbox, "In the loot@ view, display all items as if they were stackable.");
            explanationTooltip.SetToolTip(ignoreLowExperienceButton, "In the loot@ view, do not display creatures that give less than {Exp Value} experience.");
            explanationTooltip.SetToolTip(saveAllLootCheckbox, String.Format("Whenever you find loot, save the loot message to the file {0}.", Constants.BigLootFile));
            explanationTooltip.SetToolTip(selectClientProgramButton, "Select the Tibia client to scan from. This should be either the C++ Client or the Flash Client, although you can select any program.");
            explanationTooltip.SetToolTip(executeButton, "Execute a Tibialyzer command directly.");
            explanationTooltip.SetToolTip(popupAnimationBox, "Whether or not popups should be animated or simply appear.");
            explanationTooltip.SetToolTip(notificationAnchorBox, "The screen anchor to which the offsets should be applied.");
            explanationTooltip.SetToolTip(notificationGroupBox, "The display group to which this notification type belongs. Only one notification can be active per group.");
            explanationTooltip.SetToolTip(notificationDurationBox, "How long the notification should be alive before fading. If it is set to INF it will never fade away.");
            explanationTooltip.SetToolTip(applyNotificationSettingsToAllButton, "Apply the settings of this notification type to all notifications.");
            explanationTooltip.SetToolTip(popupSetValueButton, "Set it so popups appear when an item drops that is worth more than {Item Value}");
            explanationTooltip.SetToolTip(popupSetGoldCapRatioButton, "Set it so popups appear when an item drops that has a gold/cap ratio higher than {Ratio}");
            explanationTooltip.SetToolTip(popupTestButton, "Test if the specified loot message produces a popup.");
            explanationTooltip.SetToolTip(selectUpgradeTibialyzerButton, "Import settings from a previous Tibialyzer. Select the directory in which the previous Tibialyzer is located.");
        }

        public void InitializeHuntDisplay(int activeHuntIndex) {
            skip_hunt_refresh = true;

            huntList.Items.Clear();
            foreach (Hunt h in HuntManager.hunts) {
                huntList.Items.Add(h.name);
            }
            skip_hunt_refresh = false;

            huntList.SelectedIndex = activeHuntIndex;
            huntList.ItemsChanged += HuntList_ItemsChanged;
            huntList.ChangeTextOnly = true;
            huntList.AttemptDeleteItem += HuntList_AttemptDeleteItem;
            huntList.AttemptNewItem += HuntList_AttemptNewItem;

            logMessageCollection.ReadOnly = true;
            logMessageCollection.TextAlign = HorizontalAlignment.Left;
            logMessageCollection.AttemptDeleteItem += LogMessageCollection_AttemptDeleteItem;
            logMessageCollection.DrawMode = DrawMode.OwnerDrawVariable;
        }

        private void LogMessageCollection_AttemptDeleteItem(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null && logMessageCollection.SelectedIndex >= 0) {
                string logMessage = logMessageCollection.Items[logMessageCollection.SelectedIndex].ToString();
                HuntManager.deleteLogMessage(h, logMessage);
                refreshHunts();
            }
        }

        public void SetHuntTime(Hunt h, int clearMinutes) {
            foreach (string t in getLatestTimes(clearMinutes)) {
                if (totalExperienceResults.ContainsKey(t)) {
                    h.totalExp += totalExperienceResults[t];
                    h.totalTime += 60;
                }
            }
        }

        private void showAllLootButton_Click(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            this.ExecuteCommand("loot" + MainForm.commandSymbol + (h == null ? "" : h.name));
        }

        private void showPopupButton_Click(object sender, EventArgs e) {
            if (logMessageCollection.SelectedIndex >= 0) {
                string message = logMessageCollection.Items[logMessageCollection.SelectedIndex].ToString();
                var result = Parser.ParseLootMessage(message);
                if (result != null) {
                    ShowSimpleNotification(new SimpleLootNotification(result.Item1, result.Item2));
                }
            }
        }

        private void HuntList_AttemptNewItem(object sender, EventArgs e) {
            HuntManager.CreateNewHunt();
            refreshHunts();
        }

        private void HuntList_AttemptDeleteItem(object sender, EventArgs e) {
            if (HuntManager.hunts.Count <= 1) return;
            Hunt h = getSelectedHunt();
            HuntManager.DeleteHunt(h);
            HuntManager.SaveHunts();
            refreshHunts(true);
        }

        private void HuntList_ItemsChanged(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null) {
                h.name = (sender as PrettyListBox).Items[(sender as PrettyListBox).SelectedIndex].ToString();
            }
        }

        public void SuspendForm() {
            this.SuspendLayout();
            NotificationForm.SendMessage(this.Handle, NotificationForm.WM_SETREDRAW, false, 0);
        }

        public void ResumeForm() {
            this.ResumeLayout(false);
            NotificationForm.SendMessage(this.Handle, NotificationForm.WM_SETREDRAW, true, 0);
            this.Refresh();
        }

        private static List<string> displayItemList = new List<string> { "Mace", "Plate Armor", "Halberd", "Steel Helmet", "Gold Coin", "Dragon Hammer", "Knight Armor", "Giant Sword", "Crown Armor", "Golden Armor" };
        private static List<string> convertUnstackableItemList = new List<string> { "Mace", "Plate Armor", "Halberd", "Steel Helmet", "War Hammer", "Dragon Hammer", "Knight Armor", "Giant Sword", "Crown Armor", "Golden Armor" };
        private static List<string> convertStackableItemList = new List<string> { "Spear", "Burst Arrow", "Mana Potion", "Strong Mana Potion", "Great Mana Potion", "Great Fireball Rune", "Black Hood", "Strand of Medusa Hair", "Small Ruby", "Spider Silk" };
        void initializeSettings() {
            SettingsManager.ApplyDefaultSettings();

            bool copyAdvances = SettingsManager.getSettingBool("CopyAdvances");
            bool lootNotificationRich = SettingsManager.getSettingBool("UseRichNotificationType");

            this.popupAnimationBox.Checked = SettingsManager.getSettingBool("EnableSimpleNotificationAnimation");
            this.eventPopupBox.Checked = SettingsManager.getSettingBool("EnableEventNotifications");
            this.unrecognizedPopupBox.Checked = SettingsManager.getSettingBool("EnableUnrecognizedNotifications");
            this.copyAdvancesCheckbox.Checked = copyAdvances;
            this.popupTypeBox.SelectedIndex = lootNotificationRich ? 1 : 0;
            this.outfitGenderCheckbox.SelectedIndex = SettingsManager.getSettingBool("OutfitGenderMale") ? 0 : 1;
            this.lookModeCheckbox.Checked = SettingsManager.getSettingBool("LookMode");
            this.startScriptOnStartupBox.Checked = SettingsManager.getSettingBool("StartAutohotkeyAutomatically");
            this.exitScriptOnShutdownBox.Checked = SettingsManager.getSettingBool("ShutdownAutohotkeyOnExit");
            this.popupAnchorBox.SelectedIndex = Math.Min(Math.Max(SettingsManager.getSettingInt("SimpleNotificationAnchor"), 0), 3);
            this.popupXOffsetBox.Text = SettingsManager.getSettingInt("SimpleNotificationXOffset").ToString();
            this.popupYOffsetBox.Text = SettingsManager.getSettingInt("SimpleNotificationYOffset").ToString();
            this.suspendedAnchorBox.SelectedIndex = Math.Min(Math.Max(SettingsManager.getSettingInt("SuspendedNotificationAnchor"), 0), 3);
            this.suspendedXOffsetBox.Text = SettingsManager.getSettingInt("SuspendedNotificationXOffset").ToString();
            this.suspendedYOffsetBox.Text = SettingsManager.getSettingInt("SuspendedNotificationYOffset").ToString();
            this.stackAllItemsCheckbox.Checked = SettingsManager.getSettingBool("StackAllItems");
            this.ignoreLowExperienceButton.Checked = SettingsManager.getSettingBool("IgnoreLowExperience");
            this.ignoreLowExperienceBox.Enabled = this.ignoreLowExperienceButton.Checked;
            this.ignoreLowExperienceBox.Text = SettingsManager.getSettingInt("IgnoreLowExperienceValue").ToString();
            this.saveAllLootCheckbox.Checked = SettingsManager.getSettingBool("AutomaticallyWriteLootToFile");

            popupSpecificItemBox.Items.Clear();
            foreach (string str in SettingsManager.getSetting("NotificationItems")) {
                popupSpecificItemBox.Items.Add(str);
            }
            popupSpecificItemBox.ItemsChanged += PopupSpecificItemBox_ItemsChanged;
            popupSpecificItemBox.verifyItem = StorageManager.itemExists;
            popupSpecificItemBox.RefreshControl();

            nameListBox.Items.Clear();
            foreach (string str in SettingsManager.getSetting("Names")) {
                nameListBox.Items.Add(str);
            }
            nameListBox.RefreshControl();
            nameListBox.ItemsChanged += NameListBox_ItemsChanged;

            trackedCreatureList.ItemsChanged += TrackedCreatureList_ItemsChanged;
            trackedCreatureList.verifyItem = StorageManager.creatureExists;

            notificationTypeList.ReadOnly = true;
            this.screenshotAdvanceBox.Checked = SettingsManager.getSettingBool("AutoScreenshotAdvance");
            this.screenshotRareBox.Checked = SettingsManager.getSettingBool("AutoScreenshotItemDrop");
            this.screenshotDeathBox.Checked = SettingsManager.getSettingBool("AutoScreenshotDeath");

            this.enableScreenshotCheckbox.Checked = SettingsManager.getSettingBool("EnableScreenshots");
            if (SettingsManager.getSettingString("ScreenshotPath") == null || !Directory.Exists(SettingsManager.getSettingString("ScreenshotPath"))) {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                SettingsManager.setSetting("ScreenshotPath", path);
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            }

            CreateRatioDisplay(MainForm.displayItemList, discardItemsHeader.Location.X + 10, discardItemsHeader.Location.Y + discardItemsHeader.Size.Height + 8, UpdateDiscardRatio, discardLabels);
            UpdateDiscardDisplay();
            CreateRatioDisplay(MainForm.convertUnstackableItemList, convertUnstackableHeader.Location.X + 10, convertUnstackableHeader.Location.Y + convertUnstackableHeader.Size.Height + 8, UpdateConvertRatio, convertLabels);
            CreateRatioDisplay(MainForm.convertStackableItemList, convertStackableHeader.Location.X + 10, convertStackableHeader.Location.Y + convertStackableHeader.Size.Height + 8, UpdateConvertRatio, convertLabels);
            UpdateConvertDisplay();
            
            screenshotPathBox.Text = SettingsManager.getSettingString("ScreenshotPath");
            refreshScreenshots();

            // convert legacy settings
            bool legacy = false;
            if (SettingsManager.settingExists("NotificationGoldRatio") || SettingsManager.settingExists("NotificationValue")) {
                // convert old notification conditions to new SQL conditions
                List<string> conditions = new List<string>();
                if (SettingsManager.settingExists("NotificationValue") && SettingsManager.getSettingBool("ShowNotificationsValue")) {
                    double value = SettingsManager.getSettingDouble("NotificationValue");
                    conditions.Add(String.Format("item.value >= {0}", value.ToString(CultureInfo.InvariantCulture)));
                }
                if (SettingsManager.settingExists("NotificationGoldRatio") && SettingsManager.getSettingBool("ShowNotificationsGoldRatio")) {
                    double value = SettingsManager.getSettingDouble("NotificationGoldRatio");
                    conditions.Add(String.Format("item.value / item.capacity >= {0}", value.ToString(CultureInfo.InvariantCulture)));
                }
                if (SettingsManager.getSettingBool("AlwaysShowLoot")) {
                    conditions.Add("1");
                }
                SettingsManager.removeSetting("NotificationGoldRatio");
                SettingsManager.removeSetting("NotificationValue");
                SettingsManager.removeSetting("ShowNotificationsGoldRatio");
                SettingsManager.removeSetting("ShowNotificationsValue");
                SettingsManager.removeSetting("AlwaysShowLoot");
                SettingsManager.setSetting("NotificationConditions", conditions);
                legacy = true;
            }
            if (SettingsManager.settingExists("NotificationDuration")) {
                int notificationLength = SettingsManager.getSettingInt("NotificationDuration") < 0 ? 30 : SettingsManager.getSettingInt("NotificationDuration");
                int anchor = Math.Min(Math.Max(SettingsManager.getSettingInt("RichNotificationAnchor"), 0), 3);
                int xOffset = SettingsManager.getSettingInt("RichNotificationXOffset") == -1 ? 30 : SettingsManager.getSettingInt("RichNotificationXOffset");
                int yOffset = SettingsManager.getSettingInt("RichNotificationYOffset") == -1 ? 30 : SettingsManager.getSettingInt("RichNotificationYOffset");
                foreach (string obj in NotificationTypes) {
                    string settingObject = obj.Replace(" ", "");
                    SettingsManager.setSetting(settingObject + "Anchor", anchor);
                    SettingsManager.setSetting(settingObject + "XOffset", xOffset);
                    SettingsManager.setSetting(settingObject + "YOffset", yOffset);
                    SettingsManager.setSetting(settingObject + "Duration", notificationLength);
                    SettingsManager.setSetting(settingObject + "Group", 0);
                }
                SettingsManager.removeSetting("NotificationDuration");
                SettingsManager.removeSetting("RichNotificationAnchor");
                SettingsManager.removeSetting("RichNotificationXOffset");
                SettingsManager.removeSetting("RichNotificationYOffset");
                legacy = true;
            }
            if (legacy) {
                // legacy settings had "#" as comment symbol in AutoHotkey text, replace that with the new comment symbol ";"
                List<string> newAutoHotkeySettings = new List<string>();
                foreach (string str in SettingsManager.getSetting("AutoHotkeySettings")) {
                    newAutoHotkeySettings.Add(str.Replace('#', ';'));
                }
                SettingsManager.setSetting("AutoHotkeySettings", newAutoHotkeySettings);

                SettingsManager.setSetting("ScanSpeed", Math.Min(Math.Max(SettingsManager.getSettingInt("ScanSpeed") + 5, scanningSpeedTrack.Minimum), scanningSpeedTrack.Maximum));
            }

            this.scanningSpeedTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("ScanSpeed"), scanningSpeedTrack.Minimum), scanningSpeedTrack.Maximum);
            this.scanSpeedDisplayLabel.Text = Constants.ScanSpeedText[scanningSpeedTrack.Value / 10] + String.Format("({0})", scanningSpeedTrack.Value);

            string massiveString = "";
            foreach (string str in SettingsManager.getSetting("AutoHotkeySettings")) {
                massiveString += str + "\n";
            }
            this.autoHotkeyGridSettings.Text = massiveString;
            (this.autoHotkeyGridSettings as RichTextBoxAutoHotkey).RefreshSyntax();


            notificationTypeList.Items.Clear();
            foreach (string str in NotificationTypes) {
                notificationTypeList.Items.Add(str);
            }
            notificationTypeList.SelectedIndex = 0;

            popupConditionBox.Items.Clear();
            foreach (string str in SettingsManager.getSetting("NotificationConditions")) {
                popupConditionBox.Items.Add(str);
            }
            popupConditionBox.ItemsChanged += PopupConditionBox_ItemsChanged;
            popupConditionBox.verifyItem = NotificationConditionManager.ValidCondition;
            popupConditionBox.RefreshControl();

            screenshotDisplayList.ReadOnly = true;
            screenshotDisplayList.AttemptDeleteItem += ScreenshotDisplayList_AttemptDeleteItem;

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

        private void CreateRatioDisplay(List<string> itemList, int baseX, int baseY, EventHandler itemClick, List<Control> labelControls) {
            int it = 0;
            foreach (string itemName in itemList) {
                Item item = StorageManager.getItem(itemName);
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = item.image;
                pictureBox.Location = new Point(baseX + it * 52, baseY);
                pictureBox.BackgroundImage = StyleManager.GetImage("item_background.png");
                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Size = new Size(48, 48);
                pictureBox.Name = itemName;
                pictureBox.Click += itemClick;

                double goldRatio = item.GetMaxValue() / item.capacity;
                Label label = new Label();
                label.Text = String.Format(goldRatio < 100 ? "{0:0.#}" : "{0:0.}", goldRatio);
                label.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + pictureBox.Size.Height);
                label.Font = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                label.Size = new Size(48, 24);
                label.ForeColor = StyleManager.MainFormButtonColor;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Name = itemName;
                labelControls.Add(label);

                tabControls[6].Add(pictureBox);
                tabControls[6].Add(label);
                it++;
            }
        }
        private void UpdateDiscardRatio(object sender, EventArgs e) {
            string itemName = (sender as Control).Name;
            Item item = StorageManager.getItem(itemName);
            double ratio = item.GetMaxValue() / item.capacity;
            this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + Math.Floor(ratio));
            UpdateDiscardDisplay();
        }

        private List<Control> discardLabels = new List<Control>();
        private void UpdateDiscardDisplay() {
            foreach (Control c in discardLabels) {
                string itemName = c.Name;
                Item item = StorageManager.getItem(itemName);
                if (item.discard) {
                    c.BackColor = StyleManager.DatabaseDiscardColor;
                } else {
                    c.BackColor = StyleManager.DatabaseNoDiscardColor;
                }
            }
        }

        private void UpdateConvertRatio(object sender, EventArgs e) {
            string itemName = (sender as Control).Name;
            Item item = StorageManager.getItem(itemName);
            double ratio = item.GetMaxValue() / item.capacity;
            this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + (item.stackable ? "1-" : "0-") + Math.Ceiling(ratio + 0.01));
            UpdateConvertDisplay();
        }

        private List<Control> convertLabels = new List<Control>();
        private void UpdateConvertDisplay() {
            foreach (Control c in convertLabels) {
                string itemName = c.Name;
                Item item = StorageManager.getItem(itemName);
                if (item.convert_to_gold) {
                    c.BackColor = StyleManager.ItemGoldColor;
                } else {
                    c.BackColor = StyleManager.DatabaseNoConvertColor;
                }
            }
        }


        private void applyDiscardRatioButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customDiscardRatioBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + Math.Floor(ratio));
                UpdateDiscardDisplay();
            }
        }

        private void customDiscardRatioBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                applyDiscardRatioButton_Click(null, null);
                e.Handled = true;
            }
        }

        private void applyConvertRatioButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customConvertRatioBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "0-" + Math.Floor(ratio));
                this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "1-" + Math.Floor(ratio));
                UpdateConvertDisplay();
            }
        }

        private void customConvertRatioBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                applyConvertRatioButton_Click(null, null);
                e.Handled = true;
            }
        }

        private void PopupConditionBox_ItemsChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            List<string> conditions = new List<string>();
            foreach (object obj in popupConditionBox.Items) {
                conditions.Add(obj.ToString());
            }
            SettingsManager.setSetting("NotificationConditions", conditions);
        }

        private void PopupSpecificItemBox_ItemsChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            List<string> items = new List<string>();

            foreach (object obj in (sender as PrettyListBox).Items) {
                items.Add(obj.ToString());
            }
            SettingsManager.setSetting("NotificationItems", items);
        }

        private void setValuePopupButton_Click(object sender, EventArgs e) {
            int value = 0;
            if (int.TryParse(popupValueBox.Text.Trim(), out value)) {
                string valueString = String.Format("item.value >= {0}", value);
                for (int i = 0; i < popupConditionBox.Items.Count; i++) {
                    string testObject = popupConditionBox.Items[i].ToString().Replace(" ", "");
                    if (testObject.Trim().Length == 0 || testObject.StartsWith("item.value>=")) {
                        popupConditionBox.Items[i] = valueString;
                        if (testObject.Trim().Length == 0) {
                            popupConditionBox.Items.Add("");
                        }
                        PopupConditionBox_ItemsChanged(popupConditionBox, null);
                        return;
                    }
                }
                popupConditionBox.Items.Add(valueString);
                PopupConditionBox_ItemsChanged(popupConditionBox, null);
            }
        }

        private void popupSetGoldCapRatioButton_Click(object sender, EventArgs e) {
            int value = 0;
            if (int.TryParse(popupGoldCapRatioBox.Text.Trim(), out value)) {
                string valueString = String.Format("(item.value / item.capacity) >= {0}", value);
                for (int i = 0; i < popupConditionBox.Items.Count; i++) {
                    string testObject = popupConditionBox.Items[i].ToString().Replace(" ", "");
                    if (testObject.Trim().Length == 0 || testObject.StartsWith("(item.value/item.capacity)>=")) {
                        popupConditionBox.Items[i] = valueString;
                        if (testObject.Trim().Length == 0) {
                            popupConditionBox.Items.Add("");
                        }
                        PopupConditionBox_ItemsChanged(popupConditionBox, null);
                        return;
                    }
                }
                popupConditionBox.Items.Add(valueString);
                PopupConditionBox_ItemsChanged(popupConditionBox, null);
            }
        }

        void makeDraggable(Control.ControlCollection controls) {
            foreach (Control c in controls) {
                if ((c is Label && !c.Name.ToLower().Contains("button")) || c is Panel) {
                    c.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);
                }
                if (c is Panel || c is TabPage || c is TabControl) {
                    makeDraggable(c.Controls);
                }
            }
        }

        System.Timers.Timer circleTimer = null;
        void bw_DoWork(object sender, DoWorkEventArgs e) {
            while (true) {
                if (circleTimer == null) {
                    circleTimer = new System.Timers.Timer(10000);
                    circleTimer.Elapsed += circleTimer_Elapsed;
                    circleTimer.Enabled = true;
                }
                bool success = false;
                try {
                    success = ScanMemory();
                } catch (Exception ex) {
                    this.BeginInvoke((MethodInvoker)delegate {
                        DisplayWarning(String.Format("Database Scan Error (Non-Fatal): {0}", ex.Message));
                        Console.WriteLine(ex.Message);
                    });
                }
                circleTimer.Dispose();
                circleTimer = null;
                if (success) {
                    if (this.current_state != ScanningState.Scanning) {
                        this.current_state = ScanningState.Scanning;
                        this.BeginInvoke((MethodInvoker)delegate {
                            this.loadTimerImage.Image = StyleManager.GetImage("scanningbar.gif");
                            this.loadTimerImage.Enabled = true;
                            scan_tooltip.SetToolTip(this.loadTimerImage, "Scanning Memory...");
                        });
                    }
                } else {
                    if (this.current_state != ScanningState.NoTibia) {
                        this.current_state = ScanningState.NoTibia;
                        this.BeginInvoke((MethodInvoker)delegate {
                            this.loadTimerImage.Image = StyleManager.GetImage("scanningbar-red.gif");
                            this.loadTimerImage.Enabled = true;
                            scan_tooltip.SetToolTip(this.loadTimerImage, "No Tibia Client Found...");
                        });
                    }
                }
            }
        }

        private void BwMissingChunks_DoWork(object sender, DoWorkEventArgs e) {
            while (true) {
                ScanMissingChunks();
            }
        }

        void circleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (this.current_state != ScanningState.Stuck) {
                this.current_state = ScanningState.Stuck;
                this.Invoke((MethodInvoker)delegate {
                    this.loadTimerImage.Image = StyleManager.GetImage("scanningbar-gray.gif");
                    scan_tooltip.SetToolTip(this.loadTimerImage, "Waiting, possibly stuck...");
                    this.loadTimerImage.Enabled = false;
                });
            }
        }


        private string lastWarning;
        public void DisplayWarning(string message) {
            warningImageBox.Visible = true;
            if (lastWarning != message) {
                explanationTooltip.SetToolTip(warningImageBox, message);
                lastWarning = message;
            }
        }

        private void ClearWarning(string message) {
            if (lastWarning == message) {
                warningImageBox.Visible = false;
            }
        }

        public static string ToTitle(string str) {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        /*private void initializeMaps() {
        }*/

        private void ShowSimpleNotification(string title, string text, Image image) {
            notifyIcon1.BalloonTipText = text;
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.Icon = Icon.FromHandle(((Bitmap)image).GetHicon());
            notifyIcon1.ShowBalloonTip(5000);
        }

        bool clearSimpleNotifications = false;
        int notificationSpacing = 5;
        List<SimpleNotification> notificationStack = new List<SimpleNotification>();
        private void ShowSimpleNotification(SimpleNotification f) {
            int position_x = 0, position_y = 0;
            Screen screen;
            Process tibia_process = ProcessManager.GetTibiaProcess();
            if (tibia_process == null) {
                screen = Screen.FromControl(this);
            } else {
                screen = Screen.FromHandle(tibia_process.MainWindowHandle);
            }
            int simpleX = SettingsManager.getSettingInt("SimpleNotificationXOffset");
            int simpleY = SettingsManager.getSettingInt("SimpleNotificationYOffset");

            int xOffset = simpleX < 0 ? 30 : simpleX;
            int yOffset = simpleY < 0 ? 30 : simpleY;
            int anchor = SettingsManager.getSettingInt("SimpleNotificationAnchor");
            int sign = 1;
            int basePosition = screen.WorkingArea.Bottom - yOffset;
            int startX = 0;
            switch (anchor) {
                case 0:
                case 1:
                    // Top
                    sign = -1;
                    basePosition = screen.WorkingArea.Top + yOffset;
                    break;
                case 2:
                default:
                    // Bottom
                    break;
            }
            switch (anchor) {
                case 0:
                case 2:
                    // Left
                    position_x = screen.WorkingArea.Left + xOffset;
                    startX = position_x - (f.Width + notificationSpacing);
                    break;
                case 1:
                default:
                    // Right
                    position_x = screen.WorkingArea.Right - f.Width - notificationSpacing - xOffset;
                    startX = position_x + f.Width + notificationSpacing;
                    break;
            }

            foreach (SimpleNotification notification in notificationStack) {
                basePosition -= sign * (notification.Height + notificationSpacing);
            }
            position_y = basePosition - sign * f.Height;
            f.StartPosition = FormStartPosition.Manual;
            if (!SettingsManager.getSettingBool("EnableSimpleNotificationAnimation")) {
                startX = position_x;
            }

            f.SetDesktopLocation(startX, position_y);
            f.targetPositionX = position_x;
            f.targetPositionY = position_y;
            f.FormClosed += simpleNotificationClosed;

            notificationStack.Add(f);

            f.TopMost = true;
            f.Show();
        }

        private void ClearSimpleNotifications() {
            clearSimpleNotifications = true;
            foreach (SimpleNotification f in notificationStack) {
                f.ClearTimers();
                f.Close();
            }
            notificationStack.Clear();
            clearSimpleNotifications = false;
        }

        private void simpleNotificationClosed(object sender, FormClosedEventArgs e) {
            if (clearSimpleNotifications) return;
            SimpleNotification notification = sender as SimpleNotification;
            if (notification == null) return;
            bool moveDown = false;
            int positionModification = 0;
            int anchor = SettingsManager.getSettingInt("SimpleNotificationAnchor");
            int sign = 1;
            switch (anchor) {
                case 0:
                case 1:
                    sign = -1;
                    break;
            }
            foreach (SimpleNotification f in notificationStack) {
                if (f == notification) {
                    positionModification = sign * (f.Height + notificationSpacing);
                    moveDown = true;
                } else if (moveDown) {
                    f.targetPositionY += positionModification;
                }
            }
            notificationStack.Remove(notification);
        }

        private void ShowNotification(NotificationForm f, string command, string screenshot_path = "") {
            if (f == null) return;

            if (screenshot_path == "") {
                TibialyzerCommand cmd = new TibialyzerCommand(command);
                command_stack.Push(cmd);
                f.command = cmd;
            }
            f.Visible = false;
            int richX = -1;
            int richY = -1;
            int anchor = 0;
            int duration = 5;
            int group = 0;
            for (int it = 0; it < NotificationTypeObjects.Count; it++) {
                if (f.GetType() == NotificationTypeObjects[it]) {
                    string settingObject = NotificationTypes[it].Replace(" ", "");
                    richX = SettingsManager.getSettingInt(settingObject + "XOffset");
                    richY = SettingsManager.getSettingInt(settingObject + "YOffset");
                    anchor = SettingsManager.getSettingInt(settingObject + "Anchor");
                    duration = SettingsManager.getSettingInt(settingObject + "Duration");
                    group = Math.Min(Math.Max(SettingsManager.getSettingInt(settingObject + "Group"), 0), 9);
                    break;
                }
            }
            f.notificationDuration = duration;
            f.LoadForm();
            if (screenshot_path != "") {
                Bitmap bitmap = new Bitmap(f.Width, f.Height);
                f.DrawToBitmap(bitmap, new Rectangle(0, 0, f.Width, f.Height));
                foreach (Control c in f.Controls) {
                    c.DrawToBitmap(bitmap, new Rectangle(new Point(Math.Min(Math.Max(c.Location.X, 0), f.Width), Math.Min(Math.Max(c.Location.Y, 0), f.Height)), c.Size));
                }
                bitmap.Save(screenshot_path);
                bitmap.Dispose();
                f.Dispose();
                return;
            }
            if (NotificationFormGroups[group] != null) {
                NotificationFormGroups[group].close();
            }
            int position_x = 0, position_y = 0;
            Screen screen;
            Process tibia_process = ProcessManager.GetTibiaProcess();
            if (tibia_process == null) {
                screen = Screen.FromControl(this);
            } else {
                screen = Screen.FromHandle(tibia_process.MainWindowHandle);
            }

            int xOffset = richX == -1 ? 30 : richX;
            int yOffset = richY == -1 ? 30 : richY;
            switch (anchor) {
                case 3:
                    position_x = screen.WorkingArea.Right - xOffset - f.Width;
                    position_y = screen.WorkingArea.Bottom - yOffset - f.Height;
                    break;
                case 2:
                    position_x = screen.WorkingArea.Left + xOffset;
                    position_y = screen.WorkingArea.Bottom - yOffset - f.Height;
                    break;
                case 1:
                    position_x = screen.WorkingArea.Right - xOffset - f.Width;
                    position_y = screen.WorkingArea.Top + yOffset;
                    break;
                default:
                    position_x = screen.WorkingArea.Left + xOffset;
                    position_y = screen.WorkingArea.Top + yOffset;
                    break;
            }

            f.StartPosition = FormStartPosition.Manual;
            f.SetDesktopLocation(position_x, position_y);
            f.TopMost = true;
            f.Show();
            NotificationFormGroups[group] = f;
        }

        public void Back() {
            if (command_stack.Count <= 1) return;
            command_stack.Pop(); // remove the current command
            string command = command_stack.Pop().command;
            this.ExecuteCommand(command);
        }

        public bool HasBack() {
            return command_stack.Count > 1;
        }

        private void ShowCreatureDrops(Creature c, string comm) {
            if (c == null) return;
            CreatureDropsForm f = new CreatureDropsForm();
            f.creature = c;

            ShowNotification(f, comm);
        }

        private void ShowCreatureStats(Creature c, string comm) {
            if (c == null) return;
            CreatureStatsForm f = new CreatureStatsForm();
            f.creature = c;

            ShowNotification(f, comm);
        }
        private void ShowCreatureList(List<TibiaObject> c, string title, string command, bool conditionalAttributes = false) {
            if (c == null) return;
            string[] split = command.Split(commandSymbol);
            string parameter = split[1].Trim().ToLower();
            int page = 0;
            int displayType = 0;
            bool desc = false;
            string sortedHeader = null;

            if (split.Length > 2 && int.TryParse(split[2], out page)) { }
            if (split.Length > 3 && int.TryParse(split[3], out displayType)) { }
            if (split.Length > 4) { desc = split[4] == "1"; }
            if (split.Length > 5) { sortedHeader = split[5]; }
            CreatureList f = new CreatureList(page, displayType == 1 ? DisplayType.Images : DisplayType.Details, sortedHeader, desc);
            f.addConditionalAttributes = conditionalAttributes;
            f.objects = c;
            f.title = title;

            ShowNotification(f, command);
        }

        private void ShowItemView(Item i, int currentPage, int currentDisplay, string comm) {
            if (i == null) return;
            ItemViewForm f = new ItemViewForm(currentPage, currentDisplay);
            f.item = i;

            ShowNotification(f, comm);
        }

        private void ShowNPCForm(NPC c, string command) {
            if (c == null) return;
            string[] split = command.Split(commandSymbol);
            int page = 0;
            int currentDisplay = -1;
            if (split.Length > 2 && int.TryParse(split[2], out page)) { }
            if (split.Length > 3 && int.TryParse(split[3], out currentDisplay)) { }
            NPCForm f = new NPCForm(page, currentDisplay);
            f.npc = c;

            ShowNotification(f, command);
        }

        private void ShowDamageMeter(Dictionary<string, Tuple<int, int>> dps, string comm, string filter = "", string screenshot_path = "") {
            DamageChart f = new DamageChart();
            f.dps = dps;
            f.filter = filter;

            ShowNotification(f, comm, screenshot_path);
        }

        private void ShowLootDrops(Hunt h, string comm, string screenshot_path) {
            LootDropForm ldf = new LootDropForm(comm);
            ldf.hunt = h;

            ShowNotification(ldf, comm, screenshot_path);
        }

        private void ShowHuntingPlace(HuntingPlace h, string comm) {
            HuntingPlaceForm f = new HuntingPlaceForm();
            f.hunting_place = h;

            ShowNotification(f, comm);
        }

        private void ShowSpellNotification(Spell spell, int initialVocation, string comm) {
            SpellForm f = new SpellForm(spell, initialVocation);

            ShowNotification(f, comm);
        }

        private void ShowOutfitNotification(Outfit outfit, string comm) {
            OutfitForm f = new OutfitForm(outfit);

            ShowNotification(f, comm);
        }
        private void ShowQuestNotification(Quest quest, string comm) {
            QuestForm f = new QuestForm(quest);

            ShowNotification(f, comm);
        }

        private void ShowHuntGuideNotification(HuntingPlace hunt, string comm, int page) {
            if (hunt.directions.Count == 0) return;
            QuestGuideForm f = new QuestGuideForm(hunt);
            f.initialPage = page;

            ShowNotification(f, comm);
        }

        private void ShowTaskNotification(Task task, string comm) {
            TaskForm f = new TaskForm(task);

            ShowNotification(f, comm);
        }

        private void ShowQuestGuideNotification(Quest quest, string comm, int page, string mission) {
            if (quest.questInstructions.Count == 0) return;
            QuestGuideForm f = new QuestGuideForm(quest);
            f.initialPage = page;
            f.initialMission = mission;

            ShowNotification(f, comm);
        }
        private void ShowMountNotification(Mount mount, string comm) {
            MountForm f = new MountForm(mount);

            ShowNotification(f, comm);
        }
        private void ShowCityDisplayForm(City city, string comm) {
            CityDisplayForm f = new CityDisplayForm();
            f.city = city;

            ShowNotification(f, comm);
        }

        private void ShowListNotification(List<Command> commands, int type, string comm) {
            ListNotification f = new ListNotification(commands);
            f.type = type;

            ShowNotification(f, comm);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            notifyIcon1.Visible = false;
        }


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void draggable_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public static int convertX(double x, Rectangle sourceRectangle, Rectangle pictureRectangle) {
            return (int)((x - (double)sourceRectangle.X) / (double)sourceRectangle.Width * (double)pictureRectangle.Width);
        }
        public static int convertY(double y, Rectangle sourceRectangle, Rectangle pictureRectangle) {
            return (int)((y - (double)sourceRectangle.Y) / (double)sourceRectangle.Height * (double)pictureRectangle.Height);
        }

        public static Pen pathPen = new Pen(StyleManager.PathFinderPathColor, 3);
        public static MapPictureBox DrawRoute(Coordinate begin, Coordinate end, Size pictureBoxSize, Size minSize, Size maxSize, List<Color> additionalWalkableColors, List<Target> targetList = null) {
            if (end.x >= 0 && begin.z != end.z) {
                throw new Exception("Can't draw route with different z-coordinates");
            }
            Rectangle sourceRectangle;
            MapPictureBox pictureBox = new MapPictureBox();
            if (pictureBoxSize.Width != 0) {
                pictureBox.Size = pictureBoxSize;
            }
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            if (targetList != null) {
                foreach (Target target in targetList) {
                    pictureBox.targets.Add(target);
                }
                if (end.x < 0) {
                    if (pictureBoxSize.Width == 0) {
                        pictureBoxSize = new Size(Math.Min(Math.Max(end.z, minSize.Width), maxSize.Width),
                            Math.Min(Math.Max(end.z, minSize.Height), maxSize.Height));
                        pictureBox.Size = pictureBoxSize;
                    }
                    Map map = StorageManager.getMap(begin.z);
                    pictureBox.map = map;
                    pictureBox.sourceWidth = end.z;
                    pictureBox.mapCoordinate = new Coordinate(begin.x, begin.y, begin.z);
                    pictureBox.zCoordinate = begin.z;
                    pictureBox.UpdateMap();
                    return pictureBox;
                }

            }

            // First find the route at a high level
            Node beginNode = Pathfinder.GetNode(begin.x, begin.y, begin.z);
            Node endNode = Pathfinder.GetNode(end.x, end.y, end.z);

            List<Rectangle> collisionBounds = null;
            DijkstraNode highresult = Dijkstra.FindRoute(beginNode, endNode);
            if (highresult != null) {
                collisionBounds = new List<Rectangle>();
                while (highresult != null) {
                    highresult.rect.Inflate(5, 5);
                    collisionBounds.Add(highresult.rect);
                    highresult = highresult.previous;
                }
                if (collisionBounds.Count == 0) collisionBounds = null;
            }

            Map m = StorageManager.getMap(begin.z);
            DijkstraPoint result = Dijkstra.FindRoute(m.image, new Point(begin.x, begin.y), new Point(end.x, end.y), collisionBounds, additionalWalkableColors);
            if (result == null) {
                throw new Exception("Couldn't find route.");
            }

            // create a rectangle from the result
            double minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            DijkstraPoint node = result;
            while (node != null) {
                if (node.point.X < minX) minX = node.point.X;
                if (node.point.Y < minY) minY = node.point.Y;
                if (node.point.X > maxX) maxX = node.point.X;
                if (node.point.Y > maxY) maxY = node.point.Y;
                node = node.previous;
            }

            minX -= 10;
            minY -= 10;
            maxX += 10;
            maxY += 10;

            int size = (int)Math.Max(maxX - minX, maxY - minY);
            sourceRectangle = new Rectangle((int)minX, (int)minY, size, size);
            if (pictureBoxSize.Width == 0) {
                pictureBoxSize = new Size(Math.Min(Math.Max(sourceRectangle.Width, minSize.Width), maxSize.Width),
                    Math.Min(Math.Max(sourceRectangle.Height, minSize.Height), maxSize.Height));
                pictureBox.Size = pictureBoxSize;
            }
            TibiaPath path = new TibiaPath();
            path.begin = new Coordinate(begin);
            path.end = new Coordinate(end);
            path.path = result;
            pictureBox.paths.Add(path);

            pictureBox.map = m;
            pictureBox.sourceWidth = size;
            pictureBox.mapCoordinate = new Coordinate(sourceRectangle.X + sourceRectangle.Width / 2, sourceRectangle.Y + sourceRectangle.Height / 2, begin.z);
            pictureBox.zCoordinate = begin.z;
            pictureBox.UpdateMap();

            return pictureBox;
        }

        public class PageInfo {
            public bool prevPage = false;
            public bool nextPage = false;
            public int startDisplay = 0;
            public int endDisplay = 0;
            public int currentPage = 0;
            public PageInfo(bool prevPage, bool nextPage) {
                this.prevPage = prevPage;
                this.nextPage = nextPage;
            }
        }

        enum HeaderType { Numeric = 0, String = 1 };
        private static IComparable CoerceTypes(IComparable value, HeaderType type) {
            if (type == HeaderType.Numeric) {
                string valueString = value.ToString();
                double dblVal;
                if (double.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out dblVal)) {
                    return dblVal;
                }
                return (double)-127;
            } else if (type == HeaderType.String) {
                return value.ToString();
            }
            return value;
        }

        public static int DisplayCreatureAttributeList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, out int maxwidth, Func<TibiaObject, string> tooltip_function = null, List<Control> createdControls = null, int page = 0, int pageitems = 20, PageInfo pageInfo = null, string extraAttribute = null, Func<TibiaObject, Attribute> attributeFunction = null, EventHandler headerSortFunction = null, string sortedHeader = null, bool desc = false, Func<TibiaObject, IComparable> extraSort = null, List<string> removedAttributes = null, bool conditional = false) {
            const int size = 24;
            const int imageSize = size - 4;
            // add a tooltip that displays the creature names
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            int currentPage = 0;
            if (pageInfo != null) {
                pageInfo.prevPage = page > 0;
            }
            int offset = 0;
            if (sortedHeader != "" && sortedHeader != null) {
                int hash = sortedHeader.GetHashCode();
                HeaderType type = HeaderType.String;
                foreach (TibiaObject obj in l) {
                    List<string> headers = conditional ? obj.GetConditionalHeaders() : obj.GetAttributeHeaders();
                    if (headers.Contains(sortedHeader)) {
                        IComparable value = conditional ? obj.GetConditionalHeaderValue(sortedHeader) : obj.GetHeaderValue(hash);
                        if (value is string) {
                            type = HeaderType.String;
                        } else {
                            type = HeaderType.Numeric;
                        }
                        break;
                    }
                }

                if (desc) {
                    if (sortedHeader == extraAttribute && extraSort != null) {
                        l = l.OrderByDescending(o => extraSort(o)).ToList();
                    } else {
                        l = l.OrderByDescending(o => CoerceTypes(conditional ? o.GetConditionalHeaderValue(sortedHeader) : o.GetHeaderValue(hash), type)).ToList();
                    }
                } else {
                    if (sortedHeader == extraAttribute && extraSort != null) {
                        l = l.OrderBy(o => extraSort(o)).ToList();
                    } else {
                        l = l.OrderBy(o => CoerceTypes(conditional ? o.GetConditionalHeaderValue(sortedHeader) : o.GetHeaderValue(hash), type)).ToList();
                    }
                }
            }
            int start = 0;
            List<TibiaObject> pageItems = new List<TibiaObject>();
            Dictionary<string, int> totalAttributes = new Dictionary<string, int>();
            foreach (TibiaObject cr in l) {
                if (offset > pageitems) {
                    if (page > currentPage) {
                        offset = 0;
                        currentPage += 1;
                    } else {
                        if (pageInfo != null) {
                            pageInfo.nextPage = true;
                        }
                        break;
                    }
                }
                if (currentPage == page) {
                    pageItems.Add(cr);
                } else {
                    start++;
                }
                offset++;
            }
            if (pageInfo != null) {
                pageInfo.startDisplay = start;
                pageInfo.endDisplay = start + pageItems.Count;
            }
            Dictionary<string, double> sortValues = new Dictionary<string, double>();
            foreach (TibiaObject obj in conditional ? l : pageItems) {
                List<string> headers = conditional ? obj.GetConditionalHeaders() : new List<string>(obj.GetAttributeHeaders());
                List<Attribute> attributes = conditional ? obj.GetConditionalAttributes() : obj.GetAttributes();
                if (extraAttribute != null) {
                    headers.Add(extraAttribute);
                    attributes.Add(attributeFunction(obj));
                }
                for (int i = 0; i < headers.Count; i++) {
                    string header = headers[i];
                    Attribute attribute = attributes[i];
                    if (!sortValues.ContainsKey(header)) {
                        sortValues.Add(header, i);
                    } else {
                        sortValues[header] = Math.Max(sortValues[header], i);
                    }
                    if (removedAttributes != null && removedAttributes.Contains(header)) continue;
                    int width = TextRenderer.MeasureText(header, StyleManager.TextFont).Width + 10;
                    if (attribute is StringAttribute || attribute is CommandAttribute) {
                        string text = attribute is StringAttribute ? (attribute as StringAttribute).value : (attribute as CommandAttribute).value;
                        width = Math.Max(TextRenderer.MeasureText(text, StyleManager.TextFont).Width, width);
                    } else if (attribute is ImageAttribute) {
                        width = Math.Max((attribute as ImageAttribute).value == null ? 0 : (attribute as ImageAttribute).value.Width, width);
                    } else if (attribute is BooleanAttribute) {
                        width = Math.Max(20, width);
                    } else {
                        throw new Exception("Unrecognized attribute.");
                    }
                    width = Math.Min(width, attribute.MaxWidth);
                    if (!totalAttributes.ContainsKey(header)) {
                        int headerWidth = TextRenderer.MeasureText(header, StyleManager.TextFont).Width;
                        totalAttributes.Add(header, Math.Max(headerWidth, width));
                    } else if (totalAttributes[header] < width) {
                        totalAttributes[header] = width;
                    }
                }
            }
            base_x += 24;
            maxwidth = base_x;
            List<string> keys = totalAttributes.Keys.ToList();
            if (conditional) {
                keys = keys.OrderBy(o => sortValues[o]).ToList();
            }
            // create header information
            int x = base_x;
            foreach (string k in keys) {
                int val = totalAttributes[k];
                Label label = new Label();
                label.Name = k;
                label.Text = k;
                label.Location = new Point(x, base_y);
                label.ForeColor = StyleManager.NotificationTextColor;
                label.Size = new Size(val, size);
                label.Font = StyleManager.TextFont;
                label.BackColor = Color.Transparent;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.BorderStyle = BorderStyle.FixedSingle;
                if (headerSortFunction != null)
                    label.Click += headerSortFunction;
                controls.Add(label);
                if (createdControls != null) {
                    createdControls.Add(label);
                }
                x += val;
                maxwidth += val;
            }
            maxwidth += 10;
            offset = 0;

            // create object information
            foreach (TibiaObject obj in pageItems) {
                List<string> headers = conditional ? obj.GetConditionalHeaders() : new List<string>(obj.GetAttributeHeaders());
                List<Attribute> attributes = conditional ? obj.GetConditionalAttributes() : obj.GetAttributes();
                if (extraAttribute != null) {
                    headers.Add(extraAttribute);
                    attributes.Add(attributeFunction(obj));
                }
                string command = obj.GetCommand();

                // Every row is rendered on a single picture box for performance reasons
                PictureBox picture;
                picture = new PictureBox();
                picture.Image = obj.GetImage();
                picture.Size = new Size(imageSize, imageSize);
                picture.SizeMode = PictureBoxSizeMode.Zoom;
                picture.Location = new Point(base_x - 24, size * (offset + 1) + base_y);
                picture.BackColor = Color.Transparent;
                if (obj.AsItem() != null) {
                    picture.BackgroundImage = StyleManager.GetImage("item_background.png");
                }
                if (createdControls != null) {
                    createdControls.Add(picture);
                }
                controls.Add(picture);
                if (tooltip_function == null) {
                    if (obj.AsItem() != null) {
                        value_tooltip.SetToolTip(picture, obj.AsItem().look_text);
                    } else {
                        value_tooltip.SetToolTip(picture, obj.GetName());
                    }
                } else {
                    value_tooltip.SetToolTip(picture, tooltip_function(obj));
                }
                x = base_x;
                foreach (string k in keys) {
                    int val = totalAttributes[k];
                    int index = headers.IndexOf(k);
                    if (index < 0) {
                        x += val;
                        continue;
                    }
                    Attribute attribute = attributes[index];
                    Control c;
                    if (attribute is StringAttribute || attribute is CommandAttribute) {
                        string text = attribute is StringAttribute ? (attribute as StringAttribute).value : (attribute as CommandAttribute).value;
                        Color color = attribute is StringAttribute ? (attribute as StringAttribute).color : (attribute as CommandAttribute).color;
                        // create label
                        Label label = new Label();
                        label.Text = text;
                        label.ForeColor = color;
                        label.Size = new Size(val, size);
                        label.Font = StyleManager.TextFont;
                        label.Location = new Point(x, size * (offset + 1) + base_y);
                        label.BackColor = Color.Transparent;
                        if (createdControls != null) {
                            createdControls.Add(label);
                        }
                        controls.Add(label);
                        c = label;
                    } else if (attribute is ImageAttribute || attribute is BooleanAttribute) {
                        // create picturebox
                        picture = new PictureBox();
                        picture.Image = (attribute is ImageAttribute) ? (attribute as ImageAttribute).value : ((attribute as BooleanAttribute).value ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png"));
                        picture.Size = new Size(imageSize, imageSize);
                        picture.SizeMode = PictureBoxSizeMode.Zoom;
                        picture.Location = new Point(x + (val - imageSize) / 2, size * (offset + 1) + base_y);
                        picture.BackColor = Color.Transparent;
                        if (createdControls != null) {
                            createdControls.Add(picture);
                        }
                        controls.Add(picture);
                        c = picture;
                    } else {
                        throw new Exception("Unrecognized attribute.");
                    }
                    if (attribute is CommandAttribute) {
                        c.Name = (attribute as CommandAttribute).command;
                    } else {
                        c.Name = obj.GetCommand();
                    }
                    c.Click += executeNameCommand;
                    if (tooltip_function == null) {
                        if (attribute is StringAttribute || attribute is CommandAttribute) {
                            string text = attribute is StringAttribute ? (attribute as StringAttribute).value : (attribute as CommandAttribute).value;
                            value_tooltip.SetToolTip(c, text);
                        } else {
                            value_tooltip.SetToolTip(c, obj.GetName());
                        }
                    } else {
                        value_tooltip.SetToolTip(c, tooltip_function(obj));
                    }
                    x += val;
                }

                offset++;
            }
            return (offset + 1) * size;
        }

        private static void executeNameCommand(object sender, EventArgs e) {
            mainForm.ExecuteCommand((sender as Control).Name);
        }

        public static int DisplayCreatureList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, int max_x, int spacing, Func<TibiaObject, string> tooltip_function = null, float magnification = 1.0f, List<Control> createdControls = null, int page = 0, int pageheight = 10000, PageInfo pageInfo = null, int currentDisplay = -1) {
            int x = 0, y = 0;
            int height = 0;
            // add a tooltip that displays the creature names
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            int currentPage = 0;
            if (pageInfo != null) {
                pageInfo.prevPage = page > 0;
            }
            int start = 0, end = 0;
            int pageStart = 0;
            if (currentDisplay >= 0) {
                page = int.MaxValue;
            }
            for (int i = 0; i < l.Count; i++) {
                TibiaObject cr = l[i];
                int imageWidth;
                int imageHeight;
                Image image = cr.GetImage();
                string name = cr.GetName();

                if (cr.AsItem() != null || cr.AsSpell() != null) {
                    imageWidth = 32;
                    imageHeight = 32;
                } else {
                    imageWidth = image.Width;
                    imageHeight = image.Height;
                }

                if (currentDisplay >= 0 && i == currentDisplay) {
                    currentDisplay = -1;
                    i = pageStart;
                    start = i;
                    page = currentPage;
                    pageInfo.prevPage = page > 0;
                    pageInfo.currentPage = page;
                    x = 0;
                    y = 0;
                    continue;
                }

                if (max_x < (x + base_x + (int)(imageWidth * magnification) + spacing)) {
                    x = 0;
                    y = y + spacing + height;
                    height = 0;
                    if (y > pageheight) {
                        if (page > currentPage) {
                            y = 0;
                            currentPage += 1;
                            pageStart = start;
                        } else {
                            if (pageInfo != null) {
                                pageInfo.nextPage = true;
                            }
                            break;
                        }
                    }
                }
                if ((int)(imageHeight * magnification) > height) {
                    height = (int)(imageHeight * magnification);
                }
                if (currentPage == page) {
                    PictureBox image_box;
                    image_box = new PictureBox();
                    image_box.Image = image;
                    image_box.BackColor = Color.Transparent;
                    image_box.Size = new Size((int)(imageWidth * magnification), height);
                    image_box.Location = new Point(base_x + x, base_y + y);
                    image_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    image_box.Name = cr.GetCommand();
                    image_box.Click += executeNameCommand;
                    if (cr.AsItem() != null) {
                        image_box.BackgroundImage = StyleManager.GetImage("item_background.png");
                    }
                    controls.Add(image_box);
                    if (createdControls != null) createdControls.Add(image_box);
                    image_box.Image = image;
                    if (tooltip_function == null) {
                        value_tooltip.SetToolTip(image_box, MainForm.ToTitle(name));
                    } else {
                        string prefix = "";
                        if (cr.AsNPC() != null) {
                            NPC npc = cr is NPC ? cr as NPC : (cr as LazyTibiaObject).getTibiaObject() as NPC;
                            prefix = MainForm.ToTitle(name) + " (" + MainForm.ToTitle(npc.city) + ")\n";
                        }
                        value_tooltip.SetToolTip(image_box, prefix + tooltip_function(cr));
                    }
                    end++;
                } else {
                    start++;
                }

                x = x + (int)(imageWidth * magnification) + spacing;
            }
            if (pageInfo != null) {
                pageInfo.startDisplay = start;
                pageInfo.endDisplay = start + end;
            }
            x = 0;
            y = y + height;
            return y;
        }


        private void refreshItems(Control suspend, Control.ControlCollection controls, List<TibiaObject> tibiaObjects, string sortedHeader, bool desc, EventHandler eventHandler, int maxItems = 20) {
            int maxWidth = 0;

            this.SuspendLayout();
            NotificationForm.SuspendDrawing(suspend);
            foreach (Control c in controls) {
                c.Dispose();
            }
            controls.Clear();
            DisplayCreatureAttributeList(controls, tibiaObjects, 0, 10, out maxWidth, null, null, 0, maxItems, null, null, null, eventHandler, sortedHeader, desc);
            NotificationForm.ResumeDrawing(suspend);
            this.ResumeLayout(false);
        }

        private List<TibiaObject> creatureObjects = new List<TibiaObject>();
        private string creatureSortedHeader = null;
        private bool creatureDesc = false;


        object creatureLock = new object();
        System.Timers.Timer creatureTimer = null;
        protected void refreshCreatureTimer() {
            lock (creatureLock) {
                if (creatureTimer != null) {
                    creatureTimer.Dispose();
                }
                creatureTimer = new System.Timers.Timer(250);
                creatureTimer.Elapsed += CreatureTimer_Elapsed;
                creatureTimer.Enabled = true;
            }
        }

        private void CreatureTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            lock (creatureLock) {
                creatureTimer.Dispose();
                creatureTimer = null;
                mainForm.Invoke((MethodInvoker)delegate {
                    string searchTerm = browseTextBox.Text;
                    switch (browseTypeBox.SelectedIndex) {
                        case 0:
                            creatureObjects = StorageManager.searchCreature(searchTerm);
                            break;
                        case 1:
                            creatureObjects = StorageManager.searchItem(searchTerm);
                            break;
                        case 2:
                            creatureObjects = StorageManager.searchNPC(searchTerm);
                            break;
                        case 3:
                            creatureObjects = StorageManager.searchHunt(searchTerm).ToList<TibiaObject>();
                            break;
                        case 4:
                            creatureObjects = StorageManager.searchQuest(searchTerm);
                            break;
                        case 5:
                            creatureObjects = StorageManager.searchMount(searchTerm);
                            break;
                        case 6:
                            creatureObjects = StorageManager.searchOutfit(searchTerm);
                            break;
                    }
                    refreshItems(creaturePanel, creaturePanel.Controls, creatureObjects, creatureSortedHeader, creatureDesc, sortCreatures);
                });
            }
        }

        private void creatureSearch_TextChanged(object sender, EventArgs e) {
            refreshCreatureTimer();
        }

        private void browseSelectionBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (browseTextBox.Text == "") {
                return;
            }
            refreshCreatureTimer();
        }

        private void sortCreatures(object sender, EventArgs e) {
            if (creatureSortedHeader == (sender as Control).Name) {
                creatureDesc = !creatureDesc;
            } else {
                creatureSortedHeader = (sender as Control).Name;
                creatureDesc = false;
            }
            refreshItems(creaturePanel, creaturePanel.Controls, creatureObjects, creatureSortedHeader, creatureDesc, sortCreatures);
        }

        object helpLock = new object();
        System.Timers.Timer helpTimer = null;
        protected void refreshHelpTimer() {
            lock (helpLock) {
                if (helpTimer != null) {
                    helpTimer.Dispose();
                }
                helpTimer = new System.Timers.Timer(250);
                helpTimer.Elapsed += HelpTimer_Elapsed;
                helpTimer.Enabled = true;
            }
        }

        private void HelpTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            lock (helpLock) {
                if (helpTimer != null)
                    helpTimer.Dispose();
                helpTimer = null;
                mainForm.Invoke((MethodInvoker)delegate {
                    string helpText = searchCommandHelpBox.Text.ToLower();
                    commands.Clear();
                    foreach (HelpCommand command in StorageManager.helpCommands) {
                        if (helpText == "" || command.command.ToLower().Contains(helpText) || command.description.ToLower().Contains(helpText)) {
                            commands.Add(command);
                        }
                    }
                    refreshItems(helpPanel, helpPanel.Controls, commands, helpSortedHeader, helpDesc, sortHelp, 100);
                });
            }
        }
        List<TibiaObject> commands = new List<TibiaObject>();

        private string helpSortedHeader = null;
        private bool helpDesc = false;

        private void helpSearchBox_TextChanged(object sender, EventArgs e) {
            refreshHelpTimer();
        }
        private void sortHelp(object sender, EventArgs e) {
            if (helpSortedHeader == (sender as Control).Name) {
                helpDesc = !helpDesc;
            } else {
                helpSortedHeader = (sender as Control).Name;
                helpDesc = false;
            }
            refreshItems(helpPanel, helpPanel.Controls, commands, helpSortedHeader, helpDesc, sortHelp, 100);
        }

        void ShowCreatureInformation(object sender, EventArgs e) {
            string creature_name = (sender as Control).Name;
            this.ExecuteCommand("creature" + MainForm.commandSymbol + creature_name);
        }

        void ShowItemInformation(object sender, EventArgs e) {
            string item_name = (sender as Control).Name;
            this.ExecuteCommand("item" + MainForm.commandSymbol + item_name);
        }

        private void exportLogButton_Click(object sender, MouseEventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Export Log File";
            if (File.Exists("exported_log")) {
                int i = 1;
                while (File.Exists("exported_log (" + i.ToString() + ")")) i++;
                dialog.FileName = "exported_log (" + i.ToString() + ")";
            } else {
                dialog.FileName = "exported_log";
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                saveLog(getSelectedHunt(), dialog.FileName);
            }
        }

        private void resetButton_Click(object sender, MouseEventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null) {
                ExecuteCommand("reset" + MainForm.commandSymbol + h.name);
            }
        }

        private void importLogFile_Click(object sender, MouseEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Import Log File";
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                loadLog(getSelectedHunt(), dialog.FileName);
                refreshHunts();
            }
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
                this.ExecuteCommand("loot" + MainForm.commandSymbol + "screenshot" + MainForm.commandSymbol + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/"));
            }

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
                this.ExecuteCommand("damage" + MainForm.commandSymbol + "screenshot" + MainForm.commandSymbol + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/"));
            }
        }

        public static void OpenUrl(string str) {
            // Weird command prompt escape characters
            str = str.Trim().Replace(" ", "%20").Replace("&", "^&").Replace("|", "^|").Replace("(", "^(").Replace(")", "^)");
            // Always start with http:// or https://
            if (!str.StartsWith("http://") && !str.StartsWith("https://")) {
                str = "http://" + str;
            }
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C start " + str);

            procStartInfo.UseShellExecute = true;

            // Do not show the cmd window to the user.
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(procStartInfo);
        }

        private void closeButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void minimizeButton_Click(object sender, EventArgs e) {
            this.Hide();
            this.minimizeIcon.Visible = true;
            if (minimize_notification) {
                this.minimize_notification = false;
                this.minimizeIcon.ShowBalloonTip(3000);
            }
        }

        private void closeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.CloseButtonHoverColor;
        }

        private void closeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.CloseButtonNormalColor;
        }

        private void minimizeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MinimizeButtonHoverColor;
        }

        private void minimizeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MinimizeButtonNormalColor;
        }

        private void minimizeIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.minimizeIcon.Visible = false;
            this.Show();
        }

        private void commandTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                this.ExecuteCommand((sender as TextBox).Text);
                e.Handled = true;
            }
        }

        private void executeCommand_Click(object sender, EventArgs e) {
            this.ExecuteCommand(commandTextBox.Text);
        }

        private Hunt getActiveHunt() {
            return HuntManager.activeHunt;
        }
        private Hunt getSelectedHunt() {
            if (huntList.SelectedIndex < 0) return null;
            return HuntManager.GetHunt(huntList.SelectedIndex);
        }

        private void deleteHuntButton_Click(object sender, EventArgs e) {
            if (huntList.Items.Count <= 1) return;
            Hunt h = getSelectedHunt();
            HuntManager.DeleteHunt(h);
            HuntManager.SaveHunts();
            refreshHunts(true);
        }

        bool skip_hunt_refresh = false;
        bool switch_hunt = false;
        private void huntBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (skip_hunt_refresh) return;
            if (huntList.SelectedIndex < 0) return;
            switch_hunt = true;
            Hunt h = getSelectedHunt();
            displayAllCreaturesBox.Checked = h.trackAllCreatures;
            if (h == HuntManager.activeHunt) {
                setActiveHuntButton.Text = "Currently Active";
                setActiveHuntButton.Enabled = false;
            } else {
                setActiveHuntButton.Text = "Set As Active Hunt";
                setActiveHuntButton.Enabled = true;
            }
            string[] split = h.trackedCreatures.Split('\n');
            trackedCreatureList.Items.Clear();
            foreach (string str in split) {
                trackedCreatureList.Items.Add(str);
            }
            clearHuntOnStartupBox.Checked = h.clearOnStartup;
            switchOnKillBox.Checked = h.sideHunt;
            gatherTrackedKillsBox.Checked = h.aggregateHunt;
            refreshHuntImages(h);
            refreshHuntLog(h);
            switch_hunt = false;
        }

        void refreshHuntLog(Hunt h) {
            if (h == null) return;
            const int maxLogLines = 250;
            List<string> timestamps = h.loot.logMessages.Keys.OrderByDescending(o => o).ToList();
            int count = 0;
            logMessageCollection.Items.Clear();
            foreach (string t in timestamps) {
                List<string> strings = h.loot.logMessages[t].ToList();
                strings.Reverse();
                foreach (string str in strings) {
                    logMessageCollection.Items.Add(str);
                    if (count++ > maxLogLines) break;
                }
                if (count > maxLogLines) break;
            }
        }

        public void refreshHunts(bool refreshSelection = false) {
            Hunt h = getSelectedHunt();
            int currentHunt = 0;
            skip_hunt_refresh = true;

            lock (HuntManager.hunts) {
                huntList.Items.Clear();
                foreach (Hunt hunt in HuntManager.hunts) {
                    huntList.Items.Add(hunt.name);
                    if (hunt == h) currentHunt = huntList.Items.Count - 1;
                }
                huntList.SelectedIndex = refreshSelection ? 0 : currentHunt;
            }

            skip_hunt_refresh = false;
            huntBox_SelectedIndexChanged(huntList, null);
        }

        private void activeHuntButton_Click(object sender, MouseEventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            HuntManager.SetActiveHunt(h);
            setActiveHuntButton.Text = "Currently Active";
            setActiveHuntButton.Enabled = false;
            HuntManager.SaveHunts();
        }

        private void refreshHuntImages(Hunt h) {
            int spacing = 4;
            int totalWidth = spacing + spacing;
            int maxHeight = -1;
            float magnification = 1.0f;
            List<TibiaObject> creatureObjects = HuntManager.refreshLootCreatures(h);
            foreach (TibiaObject obj in creatureObjects) {
                Creature cc = obj as Creature;
                totalWidth += cc.image.Width + spacing;
                maxHeight = Math.Max(maxHeight, cc.image.Height);
            }

            if (totalWidth < creatureImagePanel.Width) {
                // fits on one line
                magnification = ((float)creatureImagePanel.Width) / totalWidth;
                //also consider the height
                float maxMagnification = ((float)creatureImagePanel.Height) / maxHeight;
                if (magnification > maxMagnification) magnification = maxMagnification;
            } else if (totalWidth < creatureImagePanel.Width * 2) {
                // make it fit on two lines
                magnification = (creatureImagePanel.Width * 1.7f) / totalWidth;
                //also consider the height
                float maxMagnification = creatureImagePanel.Height / (maxHeight * 2.0f);
                if (magnification > maxMagnification) magnification = maxMagnification;
            } else {
                // make it fit on three lines
                magnification = (creatureImagePanel.Width * 2.7f) / totalWidth;
                //also consider the height
                float maxMagnification = creatureImagePanel.Height / (maxHeight * 3.0f);
                if (magnification > maxMagnification) magnification = maxMagnification;
            }
            creatureImagePanel.Controls.Clear();
            DisplayCreatureList(creatureImagePanel.Controls, creatureObjects, 0, 0, creatureImagePanel.Width, spacing, null, magnification);
        }

        private void startupHuntCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.clearOnStartup = (sender as CheckBox).Checked;
            HuntManager.SaveHunts();
        }

        private void sideHuntBox_CheckedChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.sideHunt = (sender as CheckBox).Checked;
            HuntManager.SaveHunts();
        }

        private void aggregateHuntBox_CheckedChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.aggregateHunt = (sender as CheckBox).Checked;
            HuntManager.SaveHunts();
        }

        private void trackCreaturesBox_TextChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.trackedCreatures = (sender as RichTextBox).Text;

            HuntManager.SaveHunts();
            refreshHuntImages(h);
        }

        private void TrackedCreatureList_ItemsChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            string str = "";
            foreach (object obj in (sender as PrettyListBox).Items) {
                str += obj.ToString() + "\n";
            }
            h.trackedCreatures = str.Trim();

            HuntManager.SaveHunts();
            refreshHuntImages(h);
        }

        private void trackCreaturesCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            bool chk = (sender as CheckBox).Checked;

            Hunt h = getSelectedHunt();
            h.trackAllCreatures = chk;

            HuntManager.SaveHunts();
        }

        private void rareDropNotificationValueCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("ShowNotificationsValue", (sender as CheckBox).Checked.ToString());
        }

        private void notificationValue_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            int value;
            if (int.TryParse((sender as TextBox).Text, out value)) {
                SettingsManager.setSetting("NotificationValue", value);
            }
        }

        private void goldCapRatioCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("ShowNotificationsGoldRatio", (sender as CheckBox).Checked.ToString());
        }

        private void goldCapRatioValue_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            double value;
            if (double.TryParse((sender as TextBox).Text, NumberStyles.Any, CultureInfo.InvariantCulture, out value)) {
                SettingsManager.setSetting("NotificationGoldRatio", value);
            }
        }

        private void specificNotificationTextbox_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            List<string> names = new List<string>();

            string[] lines = (sender as RichTextBox).Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
                names.Add(lines[i].ToLower());
            SettingsManager.setSetting("NotificationItems", names);
        }

        private void notificationTypeBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("UseRichNotificationType", ((sender as ComboBox).SelectedIndex == 1).ToString());
        }

        private void outfitGenderBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("OutfitGenderMale", ((sender as ComboBox).SelectedIndex == 0).ToString());
        }

        private void eventNotificationEnable_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("EnableEventNotifications", (sender as CheckBox).Checked.ToString());
        }

        private void unrecognizedCommandNotification_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("EnableUnrecognizedNotifications", (sender as CheckBox).Checked.ToString());
        }

        private void advanceCopyCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("CopyAdvances", (sender as CheckBox).Checked.ToString());
        }

        private void NameListBox_ItemsChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            List<string> names = new List<string>();

            foreach (object obj in (sender as PrettyListBox).Items) {
                names.Add(obj.ToString());
            }
            SettingsManager.setSetting("Names", names);
        }

        private void lookCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("LookMode", (sender as CheckBox).Checked.ToString());
        }

        private void enableScreenshotBox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("EnableScreenshots", (sender as CheckBox).Checked.ToString());
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

        Bitmap takeScreenshot() {
            Process tibia_process = ProcessManager.GetTibiaProcess();
            if (tibia_process == null) return null; //no tibia to take screenshot of

            RECT Rect = new RECT();
            if (!GetWindowRect(tibia_process.MainWindowHandle, ref Rect)) return null;

            Bitmap bitmap = new Bitmap(Rect.right - Rect.left, Rect.bottom - Rect.top);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                gr.CopyFromScreen(new Point(Rect.left, Rect.top), Point.Empty, bitmap.Size);
            }
            return bitmap;

        }

        void saveScreenshot(string name, Bitmap bitmap) {
            if (bitmap == null) return;
            string path = SettingsManager.getSettingString("ScreenshotPath");
            if (path == null) return;

            DateTime dt = DateTime.Now;
            name = String.Format("{0} - {1}-{2}-{3} {4}h{5}m{6}s{7}ms.png", name, dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"), dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"), dt.Millisecond.ToString("D4"));
            path = Path.Combine(path, name);
            bitmap.Save(path, ImageFormat.Png);
            bitmap.Dispose();
            refreshScreenshots();
        }

        List<string> imageExtensions = new List<string> { ".jpg", ".bmp", ".gif", ".png" };
        void refreshScreenshots() {
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
                if (imageExtensions.Contains(Path.GetExtension(file).ToLower())) { //check if file is an image
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

        private void screenshotBrowse_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = SettingsManager.getSettingString("ScreenshotPath");
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                SettingsManager.setSetting("ScreenshotPath", folderBrowserDialog1.SelectedPath);
                screenshotPathBox.Text = folderBrowserDialog1.SelectedPath;
                refreshScreenshots();
            }
        }

        private void autoScreenshot_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("AutoScreenshotAdvance", (sender as CheckBox).Checked.ToString());
        }

        private void autoScreenshotDrop_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("AutoScreenshotItemDrop", (sender as CheckBox).Checked.ToString());
        }

        private void autoScreenshotDeath_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

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
                        if (screenshotBox.Image != null) {
                            screenshotBox.Image.Dispose();
                        }
                        screenshotBox.Image = image;
                        screenshotTitleLabel.Text = selectedImage;
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

                screenshotBox.Image.Dispose();
                screenshotBox.Image = null;

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

        private void startAutohotkeyScript_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("StartAutohotkeyAutomatically", (sender as CheckBox).Checked.ToString());
        }
        private void shutdownOnExit_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("ShutdownAutohotkeyOnExit", (sender as CheckBox).Checked.ToString());
        }

        static string autoHotkeyURL = "http://ahkscript.org/download/ahk-install.exe";
        private void downloadAutoHotkey_Click(object sender, EventArgs e) {
            WebClient client = new WebClient();

            client.DownloadDataCompleted += Client_DownloadDataCompleted;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;

            downloadBar.Visible = true;

            client.DownloadDataAsync(new Uri(autoHotkeyURL));
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

        private string modifyKeyString(string value) {
            if (value.Contains("alt+")) {
                value = value.Replace("alt+", "!");
            }
            if (value.Contains("ctrl+")) {
                value = value.Replace("ctrl+", "^");
            }
            if (value.Contains("shift+")) {
                value = value.Replace("shift+", "+");
            }
            if (value.Contains("command=")) {
                string[] split = value.Split(new string[] { "command=" }, StringSplitOptions.None);
                value = split[0] + "SendMessage, 0xC, 0, \"" + split[1] + "\",,Tibialyzer"; //command is send through the WM_SETTEXT message
            }

            return value;
        }

        private string autoHotkeyWarning = "Warning: Modified AutoHotkey settings have not taken effect. Restart AutoHotkey to apply changes.";
        private void autoHotkeyGridSettings_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("AutoHotkeySettings", autoHotkeyGridSettings.Text.Split('\n').ToList());
            DisplayWarning(autoHotkeyWarning);
        }

        private void writeToAutoHotkeyFile() {
            if (!SettingsManager.settingExists("AutoHotkeySettings")) return;
            using (StreamWriter writer = new StreamWriter(Constants.AutohotkeyFile)) {
                writer.WriteLine("#SingleInstance force");
                if (ProcessManager.IsFlashClient()) {
                    Process p = ProcessManager.GetTibiaProcess();
                    writer.WriteLine("SetTitleMatchMode 2");
                    writer.WriteLine(String.Format("#IfWinActive Tibia Flash Client", p == null ? 0 : p.Id));
                } else {
                    writer.WriteLine("#IfWinActive ahk_class TibiaClient");
                }
                foreach (string l in SettingsManager.getSetting("AutoHotkeySettings")) {
                    string line = l.ToLower();
                    if (line.Length == 0 || line[0] == ';') continue;
                    if (line.Contains("suspend")) {
                        // if the key is set to suspend the hotkey layout, we set it up so it sends a message to us
                        writer.WriteLine(modifyKeyString(line.ToLower().Split(new string[] { "suspend" }, StringSplitOptions.None)[0]));
                        writer.WriteLine("suspend");
                        writer.WriteLine("if (A_IsSuspended)");
                        // message 32 is suspend
                        writer.WriteLine("PostMessage, 0x317,32,32,,Tibialyzer");
                        writer.WriteLine("else");
                        // message 33 is not suspended
                        writer.WriteLine("PostMessage, 0x317,33,33,,Tibialyzer");
                        writer.WriteLine("return");
                    } else {
                        writer.WriteLine(modifyKeyString(line));
                    }
                }
            }
        }

        public static void RestartAutoHotkey() {
            mainForm.startAutoHotkey_Click(null, null);
        }

        private void startAutoHotkey_Click(object sender, EventArgs e) {
            ClearWarning(autoHotkeyWarning);
            writeToAutoHotkeyFile();
            System.Diagnostics.Process.Start(Constants.AutohotkeyFile);
        }

        private void shutdownAutoHotkey_Click(object sender, EventArgs e) {
            foreach (var process in Process.GetProcessesByName("AutoHotkey")) {
                process.Kill();
            }
            CloseSuspendedWindow();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (SettingsManager.getSettingBool("ShutdownAutohotkeyOnExit")) {
                shutdownAutoHotkey_Click(null, null);
            }
            if (fileWriter != null) {
                fileWriter.Close();
            }
        }

        AutoHotkeySuspendedMode window = null;
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0xC) {
                // This messages is send by AutoHotkey to execute a command
                string command = Marshal.PtrToStringUni(m.LParam);
                if (command != null) {
                    if (this.ExecuteCommand(command)) {
                        return; //if the passed along string is a command, we have executed it successfully
                    }
                }
            }
            if (m.Msg == 0x317) {
                // We intercept this message because this message signifies the AutoHotkey state (suspended or not)

                int wParam = m.WParam.ToInt32();
                if (wParam == 32) {
                    // 32 signifies we have entered suspended mode, so we warn the user with a popup
                    ShowSuspendedWindow();
                } else if (wParam == 33) {
                    // 33 signifies we are not suspended, destroy the suspended window (if it exists)
                    CloseSuspendedWindow();
                }
            }
            base.WndProc(ref m);
        }

        private object suspendedLock = new object();
        private void ShowSuspendedWindow(bool alwaysShow = false) {
            lock (suspendedLock) {
                if (window != null) {
                    window.Close();
                    window = null;
                }
                Screen screen;
                Process tibia_process = ProcessManager.GetTibiaProcess();
                if (tibia_process == null) {
                    screen = Screen.FromControl(this);
                } else {
                    screen = Screen.FromHandle(tibia_process.MainWindowHandle);
                }
                window = new AutoHotkeySuspendedMode(alwaysShow);
                int position_x = 0, position_y = 0;

                int suspendedX = SettingsManager.getSettingInt("SuspendedNotificationXOffset");
                int suspendedY = SettingsManager.getSettingInt("SuspendedNotificationYOffset");

                int xOffset = suspendedX < 0 ? 10 : suspendedX;
                int yOffset = suspendedY < 0 ? 10 : suspendedY;
                int anchor = SettingsManager.getSettingInt("SuspendedNotificationAnchor");
                switch (anchor) {
                    case 3:
                        position_x = screen.WorkingArea.Right - xOffset - window.Width;
                        position_y = screen.WorkingArea.Bottom - yOffset - window.Height;
                        break;
                    case 2:
                        position_x = screen.WorkingArea.Left + xOffset;
                        position_y = screen.WorkingArea.Bottom - yOffset - window.Height;
                        break;
                    case 0:
                        position_x = screen.WorkingArea.Left + xOffset;
                        position_y = screen.WorkingArea.Top + yOffset;
                        break;
                    default:
                        position_x = screen.WorkingArea.Right - xOffset - window.Width;
                        position_y = screen.WorkingArea.Top + yOffset;
                        break;
                }

                window.StartPosition = FormStartPosition.Manual;
                window.SetDesktopLocation(position_x, position_y);
                window.TopMost = true;
                window.Show();
            }
        }
        private void CloseSuspendedWindow() {
            lock(suspendedLock) {
                if (window != null) {
                    window.Close();
                    window = null;
                }
            }
        }

        private void resetToDefaultButton_Click(object sender, EventArgs e) {
            SettingsManager.ResetSettingsToDefault();
            SettingsManager.SaveSettings();
            shutdownAutoHotkey_Click(null, null);
            initializeSettings();
        }

        private void scanningSpeedTrack_Scroll(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("ScanSpeed", scanningSpeedTrack.Value);
            scanSpeedDisplayLabel.Text = Constants.ScanSpeedText[scanningSpeedTrack.Value / 10] + String.Format("({0})", scanningSpeedTrack.Value);
        }

        private void showLootButton_Click(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null) {
                ExecuteCommand("loot" + MainForm.commandSymbol + h.name);
            }
        }

        private void simpleAnchor_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("SimpleNotificationAnchor", (sender as ComboBox).SelectedIndex);
        }

        private void simpleXOffset_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            int xOffset;
            if (int.TryParse((sender as TextBox).Text, out xOffset)) {
                SettingsManager.setSetting("SimpleNotificationXOffset", xOffset);
            }
        }

        private void simpleYOffset_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            int yOffset;
            if (int.TryParse((sender as TextBox).Text, out yOffset)) {
                SettingsManager.setSetting("SimpleNotificationYOffset", yOffset);
            }
        }

        private void simpleTestDisplay_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("exp@");
        }

        private void clearNotifications_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("close@");
        }

        private void enableSimpleNotificationAnimations_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("EnableSimpleNotificationAnimation", (sender as CheckBox).Checked);
        }

        private void popupTestButton_Click(object sender, EventArgs e) {
            string message = popupTestLootBox.Text;
            if (message[5] == ':') { //if the time stamp is in the form of hh:mm: (i.e. flash client format) remove the second colon
                message = message.Remove(5, 1);
            }
            var parseResult = Parser.ParseLootMessage(message);
            if (parseResult != null) {
                bool showNotification = ShowDropNotification(parseResult);
                if (showNotification) {
                    this.Invoke((MethodInvoker)delegate {
                        ShowSimpleNotification(new SimpleLootNotification(parseResult.Item1, parseResult.Item2));
                    });
                }
            } else {
                DisplayWarning(String.Format("Could not parse loot message: {0}", popupTestLootBox.Text));
            }

        }

        private void suspendedTest_Click(object sender, EventArgs e) {
            ShowSuspendedWindow(true);
        }

        private void closeSuspendedWindow_Click(object sender, EventArgs e) {
            CloseSuspendedWindow();
        }

        private void suspendedAnchor_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("SuspendedNotificationAnchor", (sender as ComboBox).SelectedIndex);
        }

        private void suspendedXOffset_TextChanged(object sender, EventArgs e) {
            int xOffset;
            if (int.TryParse((sender as TextBox).Text, out xOffset)) {
                SettingsManager.setSetting("SuspendedNotificationXOffset", xOffset);
            }
        }

        private void suspendedYOffset_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            int yOffset;
            if (int.TryParse((sender as TextBox).Text, out yOffset)) {
                SettingsManager.setSetting("SuspendedNotificationYOffset", yOffset);
            }
        }

        private void selectClientButton_Click(object sender, EventArgs e) {
            SelectProcessForm form = new SelectProcessForm();
            form.StartPosition = FormStartPosition.Manual;

            form.SetDesktopLocation(this.DesktopLocation.X + (this.Width - form.Width) / 2, this.DesktopLocation.Y + (this.Height - form.Height) / 2);
            form.Show();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void mainButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void mainButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }

        private void backgroundLabel_Click(object sender, EventArgs e) {

        }

        private List<Control> activeControls = new List<Control>();
        private List<List<Control>> tabControls = new List<List<Control>>();
        private void InitializeTabs() {
            foreach (TabPage tabPage in mainTabs.TabPages) {
                List<Control> controlList = new List<Control>();
                foreach (Control c in tabPage.Controls) {
                    controlList.Add(c);
                    c.Location = new Point(c.Location.X + mainTabs.Location.X + 6, c.Location.Y + mainTabs.Location.Y + 12);
                }
                tabPage.Controls.Clear();
                tabControls.Add(controlList);
            }
            this.Controls.Remove(mainTabs);
            mainTabs.Dispose();
            // Manually add controls that appear on multiple pages
            tabControls[3].Add(huntList);
            tabControls[3].Add(huntListLabel);
        }

        private void switchTab(int tab) {
            foreach (Control c in activeControls) {
                this.Controls.Remove(c);
            }
            activeControls.Clear();
            foreach (Control c in tabControls[tab]) {
                activeControls.Add(c);
                this.Controls.Add(c);
            }

            mainButton.Enabled = true;
            generalButton.Enabled = true;
            huntButton.Enabled = true;
            logButton.Enabled = true;
            notificationButton.Enabled = true;
            popupButton.Enabled = true;
            databaseButton.Enabled = true;
            autoHotkeyButton.Enabled = true;
            screenshotButton.Enabled = true;
            browseButton.Enabled = true;
            helpButton.Enabled = true;
            upgradeButton.Enabled = true;
            switch (tab) {
                case 0:
                    mainButton.Enabled = false; break;
                case 1:
                    generalButton.Enabled = false; break;
                case 2:
                    huntButton.Enabled = false; break;
                case 3:
                    logButton.Enabled = false; break;
                case 4:
                    notificationButton.Enabled = false; break;
                case 5:
                    popupButton.Enabled = false; break;
                case 6:
                    databaseButton.Enabled = false; break;
                case 7:
                    autoHotkeyButton.Enabled = false; break;
                case 8:
                    screenshotButton.Enabled = false; break;
                case 9:
                    browseButton.Enabled = false; break;
                case 10:
                    helpButton.Enabled = false; break;
                case 11:
                    upgradeButton.Enabled = false; break;
            }
        }

        private void mainButton_Click(object sender, MouseEventArgs e) {
            switchTab(0);
        }

        private void generalButton_Click(object sender, MouseEventArgs e) {
            switchTab(1);
        }

        private void huntButton_Click(object sender, MouseEventArgs e) {
            switchTab(2);
        }

        private void logButton_Click(object sender, MouseEventArgs e) {
            switchTab(3);
            refreshHuntLog(getSelectedHunt());
        }

        private void notificationButton_Click(object sender, MouseEventArgs e) {
            switchTab(4);
        }

        private void popupButton_Click(object sender, MouseEventArgs e) {
            switchTab(5);
        }

        private void databaseButton_Click(object sender, MouseEventArgs e) {
            switchTab(6);
        }

        private void autoHotkeyButton_Click(object sender, MouseEventArgs e) {
            switchTab(7);
        }

        private void screenshotButton_Click(object sender, MouseEventArgs e) {
            switchTab(8);
        }

        private void browseButton_Click(object sender, MouseEventArgs e) {
            switchTab(9);
        }

        private void helpButton_Click(object sender, MouseEventArgs e) {
            switchTab(10);
        }

        private void upgradeButton_Click(object sender, EventArgs e) {
            switchTab(11);
        }

        private void gettingStartedGuide_Click(object sender, EventArgs e) {
            OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Quick-Start-Guide");
        }

        private void commandsGuide_Click(object sender, EventArgs e) {
            OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Loot-Management-Guide");
        }

        private void popupsGuide_Click(object sender, EventArgs e) {
            OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Popup-Guide");
        }

        private void issuesGuide_Click(object sender, EventArgs e) {
            OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Issues");
        }

        private void unlockResetButton_Click(object sender, MouseEventArgs e) {
            if (resetSettingsButton.Enabled) {
                resetSettingsButton.Enabled = false;
                resetSettingsButton.Text = "(Locked)";
                (sender as Control).Text = "Unlock Reset Button";
                unlockLabel.Text = "Unlock";
                unlockLabel.BackColor = StyleManager.MainFormDangerColor;
            } else {
                resetSettingsButton.Enabled = true;
                resetSettingsButton.Text = "Reset Settings To Default";
                (sender as Control).Text = "Lock Reset Button";
                unlockLabel.Text = "Lock";
                unlockLabel.BackColor = StyleManager.MainFormSafeColor;
            }
        }


        private string selectedNotificationObject() {
            return notificationTypeList.Items[notificationTypeList.SelectedIndex].ToString().Replace(" ", ""); ;
        }

        private void notificationTypeList_SelectedIndexChanged(object sender, EventArgs e) {
            string settingObject = selectedNotificationObject();

            selectedWindowLabel.Text = notificationTypeList.Items[notificationTypeList.SelectedIndex].ToString();

            int anchor = Math.Max(Math.Min(SettingsManager.getSettingInt(settingObject + "Anchor"), 3), 0);
            int xOffset = SettingsManager.getSettingInt(settingObject + "XOffset");
            int yOffset = SettingsManager.getSettingInt(settingObject + "YOffset");
            int notificationLength = SettingsManager.getSettingInt(settingObject + "Duration");
            int groupnr = Math.Max(Math.Min(SettingsManager.getSettingInt(settingObject + "Group"), 9), 0);
            int sliderValue = Math.Max(Math.Min(notificationLength, notificationDurationBox.Maximum), notificationDurationBox.Minimum);

            prevent_settings_update = true;
            notificationDurationLabel.Text = String.Format("Duration ({0})", sliderValue == notificationDurationBox.Maximum ? "INF" : sliderValue.ToString() + "s");
            notificationDurationBox.Value = sliderValue;
            notificationGroupBox.SelectedIndex = groupnr;
            notificationXOffsetBox.Text = xOffset.ToString();
            notificationYOffsetBox.Text = yOffset.ToString();
            notificationAnchorBox.SelectedIndex = anchor;
            prevent_settings_update = false;
        }

        private void notificationAnchorBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            SettingsManager.setSetting(selectedNotificationObject() + "Anchor", notificationAnchorBox.SelectedIndex);
        }

        private void groupSelectionList_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            SettingsManager.setSetting(selectedNotificationObject() + "Group", notificationGroupBox.SelectedIndex);
        }

        private void notificationXOffsetBox_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            int value;
            if (int.TryParse(notificationXOffsetBox.Text, out value)) {
                SettingsManager.setSetting(selectedNotificationObject() + "XOffset", value);
            }
        }

        private void notificationYOffsetBox_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            int value;
            if (int.TryParse(notificationYOffsetBox.Text, out value)) {
                SettingsManager.setSetting(selectedNotificationObject() + "YOffset", value);
            }
        }

        private void notificationDurationBox_Scroll(object sender, EventArgs e) {
            int sliderValue = notificationDurationBox.Value;
            notificationDurationLabel.Text = String.Format("Duration ({0})", sliderValue == notificationDurationBox.Maximum ? "INF" : sliderValue.ToString() + "s");
            SettingsManager.setSetting(selectedNotificationObject() + "Duration", sliderValue);
        }

        public static int MaximumNotificationDuration;
        private void applyNotificationSettingsToAllButton_Click(object sender, EventArgs e) {
            string selectedSettingObject = selectedNotificationObject();

            int anchor = Math.Max(Math.Min(SettingsManager.getSettingInt(selectedSettingObject + "Anchor"), 3), 0);
            int xOffset = SettingsManager.getSettingInt(selectedSettingObject + "XOffset");
            int yOffset = SettingsManager.getSettingInt(selectedSettingObject + "YOffset");
            int notificationLength = SettingsManager.getSettingInt(selectedSettingObject + "Duration");
            int groupnr = Math.Max(Math.Min(SettingsManager.getSettingInt(selectedSettingObject + "Group"), 9), 0);
            int sliderValue = Math.Max(Math.Min(notificationLength, notificationDurationBox.Maximum), notificationDurationBox.Minimum);

            foreach (string str in NotificationTypes) {
                string settingObject = str.Replace(" ", "");
                SettingsManager.setSetting(settingObject + "Anchor", anchor);
                SettingsManager.setSetting(settingObject + "XOffset", xOffset);
                SettingsManager.setSetting(settingObject + "YOffset", yOffset);
                SettingsManager.setSetting(settingObject + "Duration", notificationLength);
                SettingsManager.setSetting(settingObject + "Group", groupnr);
            }
        }

        private void testNotificationDisplayButton_Click(object sender, EventArgs e) {
            string command = NotificationTestCommands[notificationTypeList.SelectedIndex];
            MainForm.mainForm.ExecuteCommand(command);
        }

        private void clearNotificationDisplayButton_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("close@");
        }

        private void warningImageBox_MouseDown(object sender, MouseEventArgs e) {
            (sender as Control).Visible = false;
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
                (sender as Control).Location = new Point(screenshotListLabel.Location.X, screenshotListLabel.Location.Y);
                (sender as Control).Size = new Size(534, 497);
            }
        }

        private void selectUpgradeTibialyzerButton_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                string tibialyzerPath = folderBrowserDialog1.SelectedPath;
                string settings = System.IO.Path.Combine(tibialyzerPath, "settings.txt");
                lock (HuntManager.hunts) {
                    if (!File.Exists(settings)) {
                        settings = System.IO.Path.Combine(tibialyzerPath, Constants.SettingsFile);
                        if (!File.Exists(settings)) {
                            DisplayWarning("Could not find settings.txt in upgrade path.");
                            return;
                        }
                    }
                    SettingsManager.LoadSettings(settings);
                    initializeSettings();

                    string lootDatabase = System.IO.Path.Combine(tibialyzerPath, "loot.db");
                    if (!File.Exists(lootDatabase)) {
                        lootDatabase = System.IO.Path.Combine(tibialyzerPath, Constants.LootDatabaseFile);
                        if (!File.Exists(lootDatabase)) {
                            DisplayWarning("Could not find loot.db in upgrade path.");
                            return;
                        }
                    }

                    LootDatabaseManager.Close();
                    try {
                        File.Delete(Constants.LootDatabaseFile);
                        File.Copy(lootDatabase, Constants.LootDatabaseFile);
                    } catch (Exception ex) {
                        DisplayWarning(String.Format("Error modifying loot database: {0}", ex.Message));
                        return;
                    }
                    LootDatabaseManager.Initialize();

                    HuntManager.Initialize();

                    string database = System.IO.Path.Combine(tibialyzerPath, "database.db");
                    if (!File.Exists(database)) {
                        database = System.IO.Path.Combine(tibialyzerPath, Constants.DatabaseFile);
                        if (!File.Exists(database)) {
                            DisplayWarning("Could not find database.db in upgrade path.");
                            return;
                        }
                    }
                    SQLiteConnection databaseConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", database));
                    databaseConnection.Open();
                    StorageManager.UpdateDatabase(databaseConnection);
                }
            }
        }

        private List<SystemCommand> customCommands = new List<SystemCommand>();
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

        private void stackAllItemsCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("StackAllItems", (sender as CheckBox).Checked);
        }

        private void ignoreLowExperienceButton_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("IgnoreLowExperience", (sender as CheckBox).Checked);
            ignoreLowExperienceBox.Enabled = (sender as CheckBox).Checked;
        }

        private void ignoreLowExperienceBox_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            int value;
            if (int.TryParse(ignoreLowExperienceBox.Text, out value)) {
                SettingsManager.setSetting("IgnoreLowExperienceValue", value);
            }
        }

        private void saveAllLootCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            SettingsManager.setSetting("AutomaticallyWriteLootToFile", (sender as CheckBox).Checked);
        }

        private void popupConditionBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void detectFlashClientButton_Click(object sender, EventArgs e) {
            ProcessManager.DetectFlashClient();
        }

        private void popupTestLootBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                popupTestButton_Click(popupTestButton, null);
                e.Handled = true;
            }
        }
    }
}