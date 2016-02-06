
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

namespace Tibialyzer {
    public partial class MainForm : Form {
        public static MainForm mainForm;

        public static List<string> vocations = new List<string> { "knight", "druid", "paladin", "sorcerer" };

        public static Color background_color = Color.FromArgb(0, 51, 102);
        public static double opacity = 0.8;
        public static bool transparent = true;
        public static Image[] image_numbers = new Image[10];
        private Form tooltipForm = null;
        public static Image tibia_store_image = null;
        private static Image tibia_image = null;
        public static Image back_image = null;
        public static Image prevpage_image = null;
        public static Image nextpage_image = null;
        public static Image item_background = null;
        public static Image cross_image = null;
        public static Image[] star_image = new Image[6];
        public static Image[] star_image_text = new Image[6];
        public static Image mapup_image = null;
        public static Image mapdown_image = null;
        public static Image checkmark_yes = null;
        public static Image checkmark_no = null;
        public static Image infoIcon = null;
        public static Image nomapavailable = null;
        public static Dictionary<string, Image> vocationImages = new Dictionary<string, Image>();
        private bool keep_working = true;
        private static string databaseFile = @"Database\Database.db";
        private static string lootDatabaseFile = @"Database\Loot.db";
        private static string settingsFile = @"Database\settings.txt";
        private static string nodeDatabase = @"Database\Nodes.db";
        private static string pluralMapFile = @"Database\pluralMap.txt";
        private static string autohotkeyFile = @"Database\autohotkey.ahk";
        public static Color label_text_color = Color.FromArgb(191, 191, 191);
        public static int max_creatures = 50;
        public List<string> new_names = null;
        private bool prevent_settings_update = false;
        private bool minimize_notification = true;
        public int notification_value = 2000;
        public double notification_goldratio = 40.0;
        static HashSet<string> cities = new HashSet<string>() { "ab'dendriel", "carlin", "kazordoon", "venore", "thais", "ankrahmun", "farmine", "gray beach", "liberty bay", "port hope", "rathleton", "roshamuul", "yalahar", "svargrond", "edron", "darashia", "rookgaard", "dawnport", "gray beach" };
        public List<string> notification_items = new List<string>();
        private ToolTip scan_tooltip = new ToolTip();
        private Stack<TibialyzerCommand> command_stack = new Stack<TibialyzerCommand>();
        public static List<Font> fontList = new List<Font>();

        public static Font text_font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);

        private SQLiteConnection conn;
        private SQLiteConnection lootConn;
        static Dictionary<string, Image> creatureImages = new Dictionary<string, Image>();

        enum ScanningState { Scanning, NoTibia, Stuck };
        ScanningState current_state;

        private Image loadingbar = null;
        private Image loadingbarred = null;
        private Image loadingbargray = null;

        public Image LoadImage(string file) {
            Image image = null;
            if (!File.Exists(file)) {
                ExitWithError("Fatal Error", String.Format("Could not find image {0}", file));
            }
            image = Image.FromFile(file);
            if (image == null) {
                ExitWithError("Fatal Error", String.Format("Failed to load image {0}", file));
            }
            return image;
        }

        bool errorVisible = true;
        public void ExitWithError(string title, string text, bool exit = true) {
            MessageBox.Show(this, text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (exit) {
                System.Environment.Exit(1);
            }
        }

        public MainForm() {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            mainForm = this;
            InitializeComponent();

            if (!File.Exists(databaseFile)) {
                ExitWithError("Fatal Error", String.Format("Could not find database file {0}.", databaseFile));
            }

            if (!File.Exists(nodeDatabase)) {
                ExitWithError("Fatal Error", String.Format("Could not find database file {0}.", nodeDatabase));
            }

            conn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", databaseFile));
            conn.Open();

            lootConn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", lootDatabaseFile));
            lootConn.Open();

            back_image = LoadImage(@"Images\back.png");
            prevpage_image = LoadImage(@"Images\prevpage.png");
            nextpage_image = LoadImage(@"Images\nextpage.png");
            cross_image = LoadImage(@"Images\cross.png");
            tibia_image = LoadImage(@"Images\tibia.png");
            mapup_image = LoadImage(@"Images\mapup.png");
            mapdown_image = LoadImage(@"Images\mapdown.png");
            checkmark_no = LoadImage(@"Images\checkmark-no.png");
            checkmark_yes = LoadImage(@"Images\checkmark-yes.png");
            infoIcon = LoadImage(@"Images\defaulticon.png");
            tibia_store_image = LoadImage(@"Images\tibiastore.png");
            nomapavailable = LoadImage(@"Images\nomapavailable.png");
            utilityImages.Add("offline training", LoadImage(@"Images\offlinetraining.png"));
            utilityImages.Add("offline training melee", utilityImages["offline training"]);
            utilityImages.Add("offline training magic", LoadImage(@"Images\offlinetrainingmagic.png"));
            utilityImages.Add("offline training distance", LoadImage(@"Images\offlinetrainingdistance.png"));
            utilityImages.Add("potion", LoadImage(@"Images\potionstore.png"));
            utilityImages.Add("boat", LoadImage(@"Images\boat.png"));
            utilityImages.Add("depot", LoadImage(@"Images\depot.png"));
            utilityImages.Add("bank", LoadImage(@"Images\bank.png"));
            utilityImages.Add("temple", LoadImage(@"Images\temple.png"));
            utilityImages.Add("ore wagon", LoadImage(@"Images\orewagon.png"));
            utilityImages.Add("whirlpool", LoadImage(@"Images\whirlpool.png"));
            utilityImages.Add("post office", LoadImage(@"Images\postoffice.png"));

            item_background = LoadImage(@"Images\item_background.png");
            for (int i = 0; i < 10; i++) {
                image_numbers[i] = LoadImage(@"Images\" + i.ToString() + ".png");
            }

            vocationImages.Add("knight", LoadImage(@"Images\Knight.png"));
            vocationImages.Add("paladin", LoadImage(@"Images\Paladin.png"));
            vocationImages.Add("druid", LoadImage(@"Images\Druid.png"));
            vocationImages.Add("sorcerer", LoadImage(@"Images\Sorcerer.png"));

            NotificationForm.Initialize();
            CreatureStatsForm.InitializeCreatureStats();

            for (int i = 0; i < 5; i++) {
                star_image[i] = LoadImage(@"Images\star" + i + ".png");
                star_image_text[i] = LoadImage(@"Images\star" + i + "_text.png");
            }
            star_image[5] = LoadImage(@"Images\starunknown.png");
            star_image_text[5] = LoadImage(@"Images\starunknown_text.png");

            prevent_settings_update = true;
            this.initializePluralMap();
            try {
                this.loadDatabaseData();
            } catch (Exception e) {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.\nMessage: {1}", databaseFile, e.Message));
            }
            this.loadSettings();
            MainForm.initializeFonts();
            this.initializeNames();
            this.initializeHunts();
            this.initializeSettings();
            this.initializeMaps();
            this.initializeTooltips();
            try {
                Pathfinder.LoadFromDatabase(nodeDatabase);
            } catch (Exception e) {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.\nMessage: {1}", nodeDatabase, e.Message));
            }
            prevent_settings_update = false;

            if (getSettingBool("StartAutohotkeyAutomatically")) {
                startAutoHotkey_Click(null, null);
            }

            ignoreStamp = createStamp();

            browseSelectionBox.SelectedIndex = 0;
            browseTab.BackgroundImage = NotificationForm.background_image;
            browseTab.BackgroundImageLayout = ImageLayout.Tile;
            commandListTab.BackgroundImage = NotificationForm.background_image;
            commandListTab.BackgroundImageLayout = ImageLayout.Tile;
            this.backgroundBox.Image = NotificationForm.background_image;

            this.Load += MainForm_Load;

            BackgroundWorker bw = new BackgroundWorker();
            makeDraggable(this.Controls);
            tibialyzerLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync();

            scan_tooltip.AutoPopDelay = 60000;
            scan_tooltip.InitialDelay = 500;
            scan_tooltip.ReshowDelay = 0;
            scan_tooltip.ShowAlways = true;
            scan_tooltip.UseFading = true;

            this.loadingbar = LoadImage(@"Images\scanningbar.gif");
            this.loadingbarred = LoadImage(@"Images\scanningbar-red.gif");
            this.loadingbargray = LoadImage(@"Images\scanningbar-gray.gif");

            this.loadTimerImage.Image = this.loadingbarred;
            this.current_state = ScanningState.NoTibia;
            this.loadTimerImage.Enabled = true;
            scan_tooltip.SetToolTip(this.loadTimerImage, "No Tibia Client Found...");
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

        public static void initializeFonts() {
            for (int i = 7; i < 20; i++) {
                fontList.Add(new System.Drawing.Font("Microsoft Sans Serif", i, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            }
        }

        public static int DATABASE_NULL = -127;
        public static string DATABASE_STRING_NULL = "";
        private void loadDatabaseData() {
            SQLiteCommand command;
            SQLiteDataReader reader;
            // Quests
            command = new SQLiteCommand("SELECT id, title, name, minlevel, premium, city, legend FROM Quests", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Quest quest = new Quest();
                quest.id = reader.GetInt32(0);
                quest.title = reader.GetString(1);
                quest.name = reader.GetString(2);
                quest.minlevel = reader.GetInt32(3);
                quest.premium = reader.GetBoolean(4);
                quest.city = reader.IsDBNull(5) ? "-" : reader.GetString(5);
                quest.legend = reader.IsDBNull(6) ? "No legend available." : reader.GetString(6);
                if (quest.legend == "..." || quest.legend == "")
                    quest.legend = "No legend available.";

                questIdMap.Add(quest.id, quest);
                questNameMap.Add(quest.name.ToLower(), quest);
            }

            // Quest Rewards
            command = new SQLiteCommand("SELECT questid, itemid FROM QuestRewards", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                questIdMap[reader.GetInt32(0)].rewardItems.Add(reader.GetInt32(1));
            }

            // Quest Outfits
            command = new SQLiteCommand("SELECT questid, outfitid FROM QuestOutfits", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int questid = reader.GetInt32(0);
                int outfitid = reader.GetInt32(1);
                questIdMap[questid].rewardOutfits.Add(outfitid);
            }

            // Quest Dangers
            command = new SQLiteCommand("SELECT questid, creatureid FROM QuestDangers", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                questIdMap[reader.GetInt32(0)].questDangers.Add(reader.GetInt32(1));
            }

            // Quest Item Requirements
            command = new SQLiteCommand("SELECT questid, count, itemid FROM QuestItemRequirements", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                questIdMap[reader.GetInt32(0)].questRequirements.Add(new Tuple<int, int>(reader.GetInt32(1), reader.GetInt32(2)));
            }

            // Quest Additional Requirements
            command = new SQLiteCommand("SELECT questid, requirementtext FROM QuestAdditionalRequirements", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                questIdMap[reader.GetInt32(0)].additionalRequirements.Add(reader.GetString(1));
            }

            // Quest Instructions
            command = new SQLiteCommand("SELECT questid, beginx, beginy, beginz, endx, endy, endz, description, ordering, missionname, settings FROM QuestInstructions ORDER BY ordering", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                QuestInstruction instruction = new QuestInstruction();
                instruction.questid = reader.GetInt32(0);
                instruction.begin = new Coordinate(reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));
                if (reader.IsDBNull(4)) {
                    instruction.end = new Coordinate(DATABASE_NULL, DATABASE_NULL, reader.GetInt32(6));
                } else {
                    instruction.end = new Coordinate(reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6));
                }
                instruction.description = reader.IsDBNull(7) ? "" : reader.GetString(7);
                instruction.ordering = reader.GetInt32(8);
                instruction.settings = reader.IsDBNull(10) ? null : reader.GetString(10);
                string missionName = reader.IsDBNull(9) ? "Guide" : reader.GetString(9);

                Quest quest = questIdMap[instruction.questid];

                if (!quest.questInstructions.ContainsKey(missionName))
                    quest.questInstructions.Add(missionName, new List<QuestInstruction>());
                quest.questInstructions[missionName].Add(instruction);
            }
            // Cities
            command = new SQLiteCommand("SELECT id, name, x, y, z FROM Cities", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                City city = new City();
                city.id = reader.GetInt32(0);
                city.name = reader.GetString(1).ToLower();
                city.location = new Coordinate(reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));

                cityIdMap.Add(city.id, city);
                cityNameMap.Add(city.name, city);
            }
            // City Utilities
            command = new SQLiteCommand("SELECT cityid,name,x,y,z FROM CityUtilities", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int cityid = reader.GetInt32(0);
                Utility utility = new Utility();
                utility.name = reader.GetString(1).ToLower();
                utility.location = new Coordinate(reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));

                cityIdMap[cityid].utilities.Add(utility);
            }
            // Events
            command = new SQLiteCommand("SELECT id, title, location, creatureid FROM Events", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int eventid = reader.GetInt32(0);
                Event ev = new Event();
                ev.id = eventid;
                ev.title = reader.GetString(1);
                ev.location = reader.GetString(2);
                ev.creatureid = reader.GetInt32(3);
                eventIdMap.Add(eventid, ev);
            }
            // Event Messages
            command = new SQLiteCommand("SELECT eventid,message FROM EventMessages ", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Event ev = eventIdMap[reader.GetInt32(0)];
                ev.eventMessages.Add(reader.GetString(1));
            }
            // Task Groups
            command = new SQLiteCommand("SELECT id,name FROM TaskGroups", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                taskList.Add(name.ToLower(), new List<Task>());
                taskGroups.Add(id, name);
                questNameMap["killing in the name of... quest"].questInstructions.Add(name, new List<QuestInstruction> { new QuestInstruction { specialCommand = "task" + MainForm.commandSymbol + name } });
            }
            // Tasks
            command = new SQLiteCommand("SELECT id,groupid,count,taskpoints,bossid,bossx,bossy,bossz,name FROM Tasks", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Task task = new Task();
                task.id = reader.GetInt32(0);
                task.groupid = reader.GetInt32(1);
                task.groupname = taskGroups[task.groupid];
                task.count = reader.GetInt32(2);
                task.taskpoints = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
                task.bossid = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetInt32(4);
                task.bossposition = new Coordinate();
                task.bossposition.x = reader.IsDBNull(5) ? task.bossposition.x : reader.GetInt32(5);
                task.bossposition.y = reader.IsDBNull(6) ? task.bossposition.y : reader.GetInt32(6);
                task.bossposition.z = reader.IsDBNull(7) ? task.bossposition.z : reader.GetInt32(7);
                task.name = reader.GetString(8);

                // Task Creatures
                SQLiteCommand command2 = new SQLiteCommand(String.Format("SELECT creatureid FROM TaskCreatures WHERE taskid={0}", task.id), conn);
                SQLiteDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read()) {
                    task.creatures.Add(reader2.GetInt32(0));
                }
                command2 = new SQLiteCommand(String.Format("SELECT huntingplaceid FROM TaskHunts WHERE taskid={0}", task.id), conn);
                reader2 = command2.ExecuteReader();
                while (reader2.Read()) {
                    task.hunts.Add(reader2.GetInt32(0));
                }
                taskList[task.groupname.ToLower()].Add(task);
            }
            command = new SQLiteCommand("SELECT command, description FROM CommandHelp", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                helpCommands.Add(new HelpCommand { command = reader["command"].ToString(), description = reader["description"].ToString() });
            }
        }

        private void initializeTooltips() {
            explanationTooltip.SetToolTip(damageButton, "Saves an image of the damage chart (damage@) to a file.");
            explanationTooltip.SetToolTip(saveLootImage, "Saves an image of the loot command (loot@) to a file.");
            explanationTooltip.SetToolTip(resetButton, "WARNING: Clears the active hunt, removing all loot from it.");
            explanationTooltip.SetToolTip(exportLogButton, "Saves all the log messages of the currently selected hunt to a file.");
            explanationTooltip.SetToolTip(importLogFile, "Loads a set of log messages from a file into the currently selected hunt. ");
            explanationTooltip.SetToolTip(activeHuntButton, "Sets the currently selected hunt as the active hunt. Any creatures killed will be added to the currently active hunt. ");
            explanationTooltip.SetToolTip(trackCreaturesCheckbox, "In the loot@ command, only creatures specified in the box below are shown if this is selected.");
            explanationTooltip.SetToolTip(sideHuntBox, "When a creature specified in the box below is killed, this hunt is made the currently active hunt.");
            explanationTooltip.SetToolTip(aggregateHuntBox, "When a creature specified in the box below is killed, the loot of that creature is always added to this hunt (in addition to the active hunt).");
            explanationTooltip.SetToolTip(clearHuntOnStartup, "If this is checked, this hunt will be automatically cleared when Tibialyzer is restarted.");
            explanationTooltip.SetToolTip(lookCheckBox, "When you look (shift+click) at an item, creature or npc in-game, Tibialyzer will automatically open a box displaying information about that object.");
            explanationTooltip.SetToolTip(outfitGenderBox, "Outfit gender displayed in outfit@ searches.");
            explanationTooltip.SetToolTip(advanceCopyCheckbox, "When you advance in level or skill, the advancement text will be automatically copied for you, so you can easily paste it and notify your friends.");
            explanationTooltip.SetToolTip(eventNotificationEnable, "When a raid message is send, a notification will appear informing you of the raid.");
            explanationTooltip.SetToolTip(unrecognizedCommandNotification, "When you type in an unrecognized command in Tibia chat (unrecognized@), a notification will appear notifying you of this.");
            explanationTooltip.SetToolTip(resetToDefaultButton, "Clears all settings and resets them back to the default settings, except for the hunt settings. ");
            explanationTooltip.SetToolTip(notificationTypeBox, "Rich notifications are Windows Forms notifications that look pretty. Simple notifications are default Windows bubble notifications. ");
            explanationTooltip.SetToolTip(alwaysShowLoot, "When this box is checked, a rich notification is shown every time a creature is killed with the loot of the creature, regardless of what that loot is.");
            explanationTooltip.SetToolTip(rareDropNotificationValueCheckbox, "When an item that is worth at least this amount of gold drops, a notification is displayed.");
            explanationTooltip.SetToolTip(goldCapRatioCheckbox, "When an item that has at least this gold/cap ratio drops, a notification is displayed.");
            explanationTooltip.SetToolTip(specificNotificationCheckbox, "When any item that is specified in the box below drops, a notification is displayed informing you of the dropped item.");
            explanationTooltip.SetToolTip(notificationLengthSlider, "The amount of time that rich notifications (loot@, creature@) remain on the screen before fading.");
            explanationTooltip.SetToolTip(downloadAutoHotkey, "Download AutoHotkey to the temporary directory and launches an installer. Complete the installer to install AutoHotkey.");
            explanationTooltip.SetToolTip(exceptionLabel, "An error occurred, please go to github.com/Mytherin/Tibialyzer/issues and report it. Include the error as displayed here and try to describe what you were doing while it occurred.");
        }

        void initializePluralMap() {
            if (File.Exists(pluralMapFile)) {
                using (StreamReader reader = new StreamReader(pluralMapFile)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        if (line.Contains('=')) {
                            string[] split = line.Split('=');
                            if (!pluralMap.ContainsKey(split[0])) {
                                pluralMap.Add(split[0], split[1]);
                            }
                        }
                    }
                }
            }
        }

        private Hunt activeHunt = null;
        public List<Hunt> hunts = new List<Hunt>();
        bool showNotifications = true;
        bool showNotificationsValue = true;
        bool showNotificationsSpecific = false;
        bool lootNotificationRich = false;
        bool copyAdvances = true;
        bool simpleNotifications = true;
        bool richNotifications = true;
        public int notificationLength = 20;

        public Dictionary<string, List<string>> settings = new Dictionary<string, List<string>>();
        void loadSettings() {
            string line;
            string currentSetting = null;

            if (!File.Exists(settingsFile)) {
                ResetSettingsToDefault();
                saveSettings();
            } else {
                StreamReader file = new StreamReader(settingsFile);
                while ((line = file.ReadLine()) != null) {
                    if (line.Length == 0) continue;
                    if (line[0] == '@') {
                        currentSetting = line.Substring(1, line.Length - 1);
                        if (!settings.ContainsKey(currentSetting))
                            settings.Add(currentSetting, new List<string>());
                    } else if (currentSetting != null) {
                        settings[currentSetting].Add(line);
                    }
                }
                file.Close();
            }
        }

        void saveSettings() {
            try {
                lock (settings) {
                    using (StreamWriter file = new StreamWriter(settingsFile)) {
                        foreach (KeyValuePair<string, List<string>> pair in settings) {
                            file.WriteLine("@" + pair.Key);
                            foreach (string str in pair.Value) {
                                file.WriteLine(str);
                            }
                        }
                    }
                }
            } catch {
            }
        }

        void initializeNames() {
            if (!settings.ContainsKey("Names")) settings.Add("Names", new List<string>());

            string massiveString = "";
            foreach (string str in settings["Names"]) {
                massiveString += str + "\n";
            }
            this.nameTextBox.Text = massiveString;
        }

        void initializeHunts() {
            //"Name#DBTableID#Track#Time#Exp#SideHunt#AggregateHunt#ClearOnStartup#Creature#Creature#..."
            if (!settings.ContainsKey("Hunts")) {
                settings.Add("Hunts", new List<string>() { "New Hunt#True#0#0#False#True" });
            }
            int activeHuntIndex = 0, index = 0;
            List<int> dbTableIds = new List<int>();
            foreach (string str in settings["Hunts"]) {
                SQLiteCommand command; SQLiteDataReader reader;
                Hunt hunt = new Hunt();
                string[] splits = str.Split('#');
                if (splits.Length >= 7) {
                    hunt.name = splits[0];
                    if (!int.TryParse(splits[1].Trim(), out hunt.dbtableid)) continue;
                    if (dbTableIds.Contains(hunt.dbtableid)) continue;
                    dbTableIds.Add(hunt.dbtableid);

                    hunt.totalTime = 0;
                    hunt.trackAllCreatures = splits[2] == "True";
                    double.TryParse(splits[3], NumberStyles.Any, CultureInfo.InvariantCulture, out hunt.totalTime);
                    long.TryParse(splits[4], out hunt.totalExp);
                    hunt.sideHunt = splits[5] == "True";
                    hunt.aggregateHunt = splits[6] == "True";
                    hunt.clearOnStartup = splits[7] == "True";
                    hunt.temporary = false;
                    string massiveString = "";
                    for (int i = 8; i < splits.Length; i++) {
                        if (splits[i].Length > 0) {
                            massiveString += splits[i] + "\n";
                        }
                    }
                    hunt.trackedCreatures = massiveString;
                    // set this hunt to the active hunt if it is the active hunt
                    if (settings.ContainsKey("ActiveHunt") && settings["ActiveHunt"].Count > 0 && settings["ActiveHunt"][0] == hunt.name)
                        activeHuntIndex = index;

                    refreshLootCreatures(hunt);

                    if (hunt.clearOnStartup) {
                        resetHunt(hunt);
                    }

                    // create the hunt table if it does not exist
                    command = new SQLiteCommand(String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"(day INTEGER, hour INTEGER, minute INTEGER, message STRING);", hunt.GetTableName()), lootConn);
                    command.ExecuteNonQuery();
                    // load the data for the hunt from the database
                    command = new SQLiteCommand(String.Format("SELECT message FROM \"{0}\" ORDER BY day, hour, minute;", hunt.GetTableName()), lootConn);
                    reader = command.ExecuteReader();
                    while (reader.Read()) {
                        string message = reader["message"].ToString();
                        Tuple<Creature, List<Tuple<Item, int>>> resultList = ParseLootMessage(message);
                        if (resultList == null) continue;

                        string t = message.Substring(0, 5);
                        if (!hunt.loot.logMessages.ContainsKey(t)) hunt.loot.logMessages.Add(t, new List<string>());
                        hunt.loot.logMessages[t].Add(message);

                        Creature cr = resultList.Item1;
                        if (!hunt.loot.creatureLoot.ContainsKey(cr)) hunt.loot.creatureLoot.Add(cr, new Dictionary<Item, int>());
                        foreach (Tuple<Item, int> tpl in resultList.Item2) {
                            Item item = tpl.Item1;
                            int count = tpl.Item2;
                            if (!hunt.loot.creatureLoot[cr].ContainsKey(item)) hunt.loot.creatureLoot[cr].Add(item, count);
                            else hunt.loot.creatureLoot[cr][item] += count;
                        }
                        if (!hunt.loot.killCount.ContainsKey(cr)) hunt.loot.killCount.Add(cr, 1);
                        else hunt.loot.killCount[cr] += 1;
                    }
                    hunts.Add(hunt);
                    index++;
                }
            }
            if (hunts.Count == 0) {
                Hunt h = new Hunt();
                h.name = "New Hunt";
                h.dbtableid = 1;
                hunts.Add(h);
                resetHunt(h);
            }

            skip_hunt_refresh = true;
            huntBox.Items.Clear();
            foreach (Hunt h in hunts) {
                huntBox.Items.Add(h.name);
            }
            activeHunt = hunts[activeHuntIndex];
            skip_hunt_refresh = false;
            huntBox.SelectedIndex = activeHuntIndex;
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

        void initializeSettings() {
            this.notificationLength = getSettingInt("NotificationDuration") < 0 ? notificationLength : getSettingInt("NotificationDuration");
            this.simpleNotifications = getSettingBool("EnableSimpleNotifications");
            this.richNotifications = getSettingBool("EnableRichNotifications");
            this.copyAdvances = getSettingBool("CopyAdvances");
            this.showNotifications = getSettingBool("ShowNotifications");
            this.lootNotificationRich = getSettingBool("UseRichNotificationType");
            this.showNotificationsValue = getSettingBool("ShowNotificationsValue");
            this.notification_value = getSettingInt("NotificationValue") < 0 ? notification_value : getSettingInt("NotificationValue");
            this.notification_goldratio = getSettingDouble("NotificationGoldRatio") < 0 ? notification_goldratio : getSettingDouble("NotificationGoldRatio");
            this.showNotificationsSpecific = getSettingBool("ShowNotificationsSpecific");

            this.notificationPanel.Enabled = showNotifications;
            this.goldCapRatioValue.Text = notification_goldratio.ToString(CultureInfo.InvariantCulture);
            this.goldCapRatioCheckbox.Checked = getSettingBool("ShowNotificationsGoldRatio");
            this.enableSimpleNotificationAnimations.Checked = getSettingBool("EnableSimpleNotificationAnimation");
            this.specificNotificationTextbox.Enabled = showNotificationsSpecific;
            this.notificationLabel.Text = "Notification Length: " + notificationLength.ToString() + " Seconds";
            this.scanningSpeedTrack.Value = Math.Min(Math.Max(getSettingInt("ScanSpeed"), 0), 4);
            this.scanSpeedDisplayLabel.Text = scanSpeedText[scanningSpeedTrack.Value];
            this.notificationLengthSlider.Value = notificationLength;
            this.enableSimpleNotifications.Checked = simpleNotifications;
            this.eventNotificationEnable.Checked = getSettingBool("EnableEventNotifications");
            this.unrecognizedCommandNotification.Checked = getSettingBool("EnableUnrecognizedNotifications");
            this.enableRichNotificationsCheckbox.Checked = richNotifications;
            this.advanceCopyCheckbox.Checked = copyAdvances;
            this.showNotificationCheckbox.Checked = showNotifications;
            this.notificationTypeBox.SelectedIndex = lootNotificationRich ? 1 : 0;
            this.outfitGenderBox.SelectedIndex = getSettingBool("OutfitGenderMale") ? 0 : 1;
            this.rareDropNotificationValueCheckbox.Checked = showNotificationsValue;
            this.notificationValue.Text = notification_value.ToString();
            this.specificNotificationCheckbox.Checked = showNotificationsSpecific;
            this.lookCheckBox.Checked = getSettingBool("LookMode");
            this.alwaysShowLoot.Checked = getSettingBool("AlwaysShowLoot");
            this.startAutohotkeyScript.Checked = getSettingBool("StartAutohotkeyAutomatically");
            this.shutdownOnExit.Checked = getSettingBool("ShutdownAutohotkeyOnExit");
            this.richAnchor.SelectedIndex = Math.Min(Math.Max(getSettingInt("RichNotificationAnchor"), 0), 3);
            this.richXOffset.Text = getSettingInt("RichNotificationXOffset").ToString();
            this.richYOffset.Text = getSettingInt("RichNotificationYOffset").ToString();
            this.simpleAnchor.SelectedIndex = Math.Min(Math.Max(getSettingInt("SimpleNotificationAnchor"), 0), 3);
            this.simpleXOffset.Text = getSettingInt("SimpleNotificationXOffset").ToString();
            this.simpleYOffset.Text = getSettingInt("SimpleNotificationYOffset").ToString();
            this.suspendedAnchor.SelectedIndex = Math.Min(Math.Max(getSettingInt("SuspendedNotificationAnchor"), 0), 3);
            this.suspendedXOffset.Text = getSettingInt("SuspendedNotificationXOffset").ToString();
            this.suspendedYOffset.Text = getSettingInt("SuspendedNotificationYOffset").ToString();

            string massiveString = "";
            if (settings.ContainsKey("NotificationItems")) {
                foreach (string str in settings["NotificationItems"]) {
                    massiveString += str + "\n";
                }
            }
            this.specificNotificationTextbox.Text = massiveString;
            massiveString = "";
            if (settings.ContainsKey("AutoHotkeySettings")) {
                foreach (string str in settings["AutoHotkeySettings"]) {
                    massiveString += str + "\n";
                }
            }
            this.autoHotkeyGridSettings.Text = massiveString;
            (this.autoHotkeyGridSettings as RichTextBoxAutoHotkey).RefreshSyntax();

            this.autoScreenshotAdvance.Checked = getSettingBool("AutoScreenshotAdvance");
            this.autoScreenshotDrop.Checked = getSettingBool("AutoScreenshotItemDrop");
            this.autoScreenshotDeath.Checked = getSettingBool("AutoScreenshotDeath");

            this.enableScreenshotBox.Checked = getSettingBool("EnableScreenshots");
            this.screenshotPanel.Enabled = enableScreenshotBox.Checked;
            if (getSettingString("ScreenshotPath") == null || !Directory.Exists(getSettingString("ScreenshotPath"))) {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                setSetting("ScreenshotPath", path);
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            }

            TibiaClientName = settingExists("TibiaClientName") ? getSettingString("TibiaClientName") : TibiaClientName;

            screenshotDirectoryBox.Text = getSettingString("ScreenshotPath");
            refreshScreenshots();
        }

        void makeDraggable(Control.ControlCollection controls) {
            foreach (Control c in controls) {
                if (c == this.closeButton || c == this.minimizeButton) continue;
                if (c is Label || c is Panel) {
                    c.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);
                }
                if (c is Panel || c is TabPage || c is TabControl) {
                    makeDraggable(c.Controls);
                }
            }
        }

        System.Timers.Timer circleTimer = null;
        void bw_DoWork(object sender, DoWorkEventArgs e) {
            while (keep_working) {
                if (circleTimer == null) {
                    circleTimer = new System.Timers.Timer(10000);
                    circleTimer.Elapsed += circleTimer_Elapsed;
                    circleTimer.Enabled = true;
                }
                bool success = false;
                try {
                    success = ScanMemory();
                } catch (Exception ex) {
                    if (errorVisible) {
                        errorVisible = false;
                        ExitWithError("Database Scan Error (Non-Fatal)", ex.Message, false);
                    }
                    Console.WriteLine(ex.Message);
                }
                circleTimer.Dispose();
                circleTimer = null;
                if (success) {
                    if (this.current_state != ScanningState.Scanning) {
                        this.current_state = ScanningState.Scanning;
                        this.BeginInvoke((MethodInvoker)delegate {
                            this.loadTimerImage.Image = this.loadingbar;
                            this.loadTimerImage.Enabled = true;
                            scan_tooltip.SetToolTip(this.loadTimerImage, "Scanning Memory...");
                        });
                    }
                } else {
                    if (this.current_state != ScanningState.NoTibia) {
                        this.current_state = ScanningState.NoTibia;
                        this.BeginInvoke((MethodInvoker)delegate {
                            this.loadTimerImage.Image = this.loadingbarred;
                            this.loadTimerImage.Enabled = true;
                            scan_tooltip.SetToolTip(this.loadTimerImage, "No Tibia Client Found...");
                        });
                    }
                }
            }
        }

        void circleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (this.current_state != ScanningState.Stuck) {
                this.current_state = ScanningState.Stuck;
                this.Invoke((MethodInvoker)delegate {
                    this.loadTimerImage.Image = this.loadingbargray;
                    scan_tooltip.SetToolTip(this.loadTimerImage, "Waiting, possibly stuck...");
                    this.loadTimerImage.Enabled = false;
                });
            }
        }


        public static string ToTitle(string str) {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        private void initializeMaps() {
            SQLiteCommand command = new SQLiteCommand("SELECT z FROM WorldMap", conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                Map m = new Map();
                m.z = reader.GetInt32(0);
                mapFiles.Add(m);
            }
        }

        private void ShowSimpleNotification(string title, string text, Image image) {
            if (!simpleNotifications) return;
            notifyIcon1.BalloonTipText = text;
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.Icon = Icon.FromHandle(((Bitmap)image).GetHicon());
            notifyIcon1.ShowBalloonTip(5000);
        }

        public void CloseNotification() {
            if (tooltipForm != null) {
                tooltipForm.Close();
            }
        }


        bool clearSimpleNotifications = false;
        int notificationSpacing = 5;
        List<SimpleNotification> notificationStack = new List<SimpleNotification>();
        private void ShowSimpleNotification(SimpleNotification f) {
            int position_x = 0, position_y = 0;
            Screen screen;
            Process tibia_process = GetTibiaProcess();
            if (tibia_process == null) {
                screen = Screen.FromControl(this);
            } else {
                screen = Screen.FromHandle(tibia_process.MainWindowHandle);
            }
            int xOffset = getSettingInt("SimpleNotificationXOffset") < 0 ? 30 : getSettingInt("SimpleNotificationXOffset");
            int yOffset = getSettingInt("SimpleNotificationYOffset") < 0 ? 30 : getSettingInt("SimpleNotificationYOffset");
            int anchor = getSettingInt("SimpleNotificationAnchor");
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
            if (!getSettingBool("EnableSimpleNotificationAnimation")) {
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
            int anchor = getSettingInt("SimpleNotificationAnchor");
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
            if (!richNotifications) return;

            if (screenshot_path == "") {
                TibialyzerCommand cmd = new TibialyzerCommand(command);
                command_stack.Push(cmd);
                f.command = cmd;
            }
            f.Visible = false;
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
            if (tooltipForm != null) {
                tooltipForm.Close();
            }
            int position_x = 0, position_y = 0;
            Screen screen;
            Process tibia_process = GetTibiaProcess();
            if (tibia_process == null) {
                screen = Screen.FromControl(this);
            } else {
                screen = Screen.FromHandle(tibia_process.MainWindowHandle);
            }
            int xOffset = getSettingInt("RichNotificationXOffset") == -1 ? 30 : getSettingInt("RichNotificationXOffset");
            int yOffset = getSettingInt("RichNotificationYOffset") == -1 ? 30 : getSettingInt("RichNotificationYOffset");
            int anchor = getSettingInt("RichNotificationAnchor");
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
            tooltipForm = f;
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

        public static Pen pathPen = new Pen(Color.FromArgb(25, 25, 25), 3);
        public static Pen startPen = new Pen(Color.FromArgb(191, 191, 191), 2);
        public static Pen endPen = new Pen(Color.FromArgb(34, 139, 34), 2);
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
                    Map map = getMap(begin.z);
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

            Map m = getMap(begin.z);
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
                    int width = TextRenderer.MeasureText(header, MainForm.text_font).Width + 10;
                    if (attribute is StringAttribute || attribute is CommandAttribute) {
                        string text = attribute is StringAttribute ? (attribute as StringAttribute).value : (attribute as CommandAttribute).value;
                        width = Math.Max(TextRenderer.MeasureText(text, MainForm.text_font).Width, width);
                    } else if (attribute is ImageAttribute) {
                        width = Math.Max((attribute as ImageAttribute).value == null ? 0 : (attribute as ImageAttribute).value.Width, width);
                    } else if (attribute is BooleanAttribute) {
                        width = Math.Max(20, width);
                    } else {
                        throw new Exception("Unrecognized attribute.");
                    }
                    width = Math.Min(width, attribute.MaxWidth);
                    if (!totalAttributes.ContainsKey(header)) {
                        int headerWidth = TextRenderer.MeasureText(header, MainForm.text_font).Width;
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
                label.ForeColor = MainForm.label_text_color;
                label.Size = new Size(val, size);
                label.Font = MainForm.text_font;
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
                    picture.BackgroundImage = MainForm.item_background;
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
                        label.Font = MainForm.text_font;
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
                        picture.Image = (attribute is ImageAttribute) ? (attribute as ImageAttribute).value : ((attribute as BooleanAttribute).value ? MainForm.checkmark_yes : MainForm.checkmark_no);
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
                    if (transparent) image_box = new PictureBox();
                    else image_box = new PictureBox();
                    image_box.Image = image;
                    image_box.BackColor = Color.Transparent;
                    image_box.Size = new Size((int)(imageWidth * magnification), height);
                    image_box.Location = new Point(base_x + x, base_y + y);
                    image_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    image_box.Name = cr.GetCommand();
                    image_box.Click += executeNameCommand;
                    if (cr.AsItem() != null) {
                        image_box.BackgroundImage = MainForm.item_background;
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
                    string searchTerm = creatureSearch.Text;
                    switch (browseSelectionBox.SelectedIndex) {
                        case 0:
                            creatureObjects = searchCreature(searchTerm);
                            break;
                        case 1:
                            creatureObjects = searchItem(searchTerm);
                            break;
                        case 2:
                            creatureObjects = searchNPC(searchTerm);
                            break;
                        case 3:
                            creatureObjects = searchHunt(searchTerm).ToList<TibiaObject>();
                            break;
                        case 4:
                            creatureObjects = searchQuest(searchTerm);
                            break;
                        case 5:
                            creatureObjects = searchMount(searchTerm);
                            break;
                        case 6:
                            creatureObjects = searchOutfit(searchTerm);
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
            if (creatureSearch.Text == "") {
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
                    string helpText = helpSearchBox.Text.ToLower();
                    commands.Clear();
                    foreach (HelpCommand command in helpCommands) {
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

        private void exportLogButton_Click(object sender, EventArgs e) {
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

        private void resetButton_Click(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null) {
                ExecuteCommand("reset" + MainForm.commandSymbol + h.name);
            }
        }

        private void importLogFile_Click(object sender, EventArgs e) {
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

        private static Color hoverColor = Color.FromArgb(200, 55, 55);
        private static Color normalColor = Color.FromArgb(172, 24, 24);
        private void closeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = hoverColor;
        }

        private void closeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = normalColor;
        }

        private static Color minimizeHoverColor = Color.FromArgb(191, 191, 191);
        private static Color minimizeNormalColor = Color.Transparent;
        private void minimizeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = minimizeHoverColor;
        }

        private void minimizeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = minimizeNormalColor;
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
            return activeHunt;
        }
        private Hunt getSelectedHunt() {
            if (huntBox.SelectedIndex < 0) return null;
            lock (hunts) {

                return huntBox.SelectedIndex >= hunts.Count ? null : hunts[huntBox.SelectedIndex];
            }
        }

        bool nameExists(string str) {
            foreach (Hunt h in hunts) {
                if (h.name == str) {
                    return true;
                }
            }
            return false;
        }

        private void newHuntButton_Click(object sender, EventArgs e) {
            Hunt h = new Hunt();
            lock (hunts) {
                SQLiteCommand command;
                if (!nameExists("New Hunt")) {
                    h.name = "New Hunt";
                } else {
                    int index = 1;
                    while (nameExists("New Hunt " + index)) index++;
                    h.name = "New Hunt " + index;
                }

                h.dbtableid = 1;
                while (true) {
                    command = new SQLiteCommand(String.Format("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{0}';", h.GetTableName()), lootConn);
                    int value = int.Parse(command.ExecuteScalar().ToString());
                    if (value == 0) {
                        break;
                    }
                    h.dbtableid++;
                }
            }
            resetHunt(h);
            h.trackAllCreatures = true;
            h.trackedCreatures = "";
            hunts.Add(h);
            refreshHunts();
        }

        private void deleteHuntButton_Click(object sender, EventArgs e) {
            if (hunts.Count <= 1) return;
            Hunt h = getSelectedHunt();
            lock (hunts) {
                hunts.Remove(h);
            }
            saveHunts();
            refreshHunts(true);
        }

        bool skip_hunt_refresh = false;
        bool switch_hunt = false;
        private void huntBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (skip_hunt_refresh) return;
            switch_hunt = true;
            Hunt h = getSelectedHunt();
            this.huntNameBox.Text = h.name;
            trackCreaturesCheckbox.Checked = h.trackAllCreatures;
            if (h == activeHunt) {
                activeHuntButton.Text = "Currently Active";
                activeHuntButton.Enabled = false;
            } else {
                activeHuntButton.Text = "Set As Active Hunt";
                activeHuntButton.Enabled = true;
            }
            trackCreaturesBox.Text = h.trackedCreatures;
            clearHuntOnStartup.Checked = h.clearOnStartup;
            sideHuntBox.Checked = h.sideHunt;
            aggregateHuntBox.Checked = h.aggregateHunt;
            refreshHuntImages(h);
            refreshHuntLog(h);
            switch_hunt = false;
        }

        void refreshHuntLog(Hunt h) {
            const int maxLogLines = 1000;
            string massiveString = "";
            List<string> timestamps = h.loot.logMessages.Keys.OrderByDescending(o => o).ToList();
            int count = 0;
            foreach (string t in timestamps) {
                List<string> strings = h.loot.logMessages[t].ToList();
                strings.Reverse();
                foreach (string str in strings) {
                    massiveString += str + "\n";
                    if (count++ > maxLogLines) break;
                }
                if (count > maxLogLines) break;
            }
            this.logMessageTextBox.Text = massiveString;
        }

        void refreshHunts(bool refreshSelection = false) {
            Hunt h = getSelectedHunt();
            int currentHunt = 0;
            skip_hunt_refresh = true;

            lock (hunts) {
                huntBox.Items.Clear();
                foreach (Hunt hunt in hunts) {
                    huntBox.Items.Add(hunt.name);
                    if (hunt == h) currentHunt = huntBox.Items.Count - 1;
                }
                huntBox.SelectedIndex = refreshSelection ? 0 : currentHunt;
            }

            skip_hunt_refresh = false;
            huntBox_SelectedIndexChanged(huntBox, null);
        }

        void saveHunts() {
            List<string> huntStrings = new List<string>();
            lock (hunts) {
                foreach (Hunt hunt in hunts) {
                    if (hunt.temporary) continue;
                    huntStrings.Add(hunt.ToString());
                }
                settings["Hunts"] = huntStrings;
                if (activeHunt != null) {
                    setSetting("ActiveHunt", activeHunt.name);
                }
                saveSettings();
            }
        }

        private void huntNameBox_TextChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.name = (sender as TextBox).Text;
            saveHunts();
            refreshHunts();
        }

        private void activeHuntButton_Click(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            lock (hunts) {
                activeHunt = h;
            }
            activeHuntButton.Text = "Currently Active";
            activeHuntButton.Enabled = false;
            saveHunts();
        }

        List<TibiaObject> refreshLootCreatures(Hunt h) {
            h.lootCreatures.Clear();
            string[] creatures = h.trackedCreatures.Split('\n');
            List<TibiaObject> creatureObjects = new List<TibiaObject>();
            foreach (string cr in creatures) {
                string name = cr.ToLower();
                Creature cc = getCreature(name);
                if (cc != null && !creatureObjects.Contains(cc)) {
                    creatureObjects.Add(cc);
                    h.lootCreatures.Add(name);
                } else if (cc == null) {
                    HuntingPlace hunt = getHunt(name);
                    if (hunt != null) {
                        foreach (int creatureid in hunt.creatures) {
                            cc = getCreature(creatureid);
                            if (cc != null && !creatureObjects.Any(item => item.GetName() == name)) {
                                creatureObjects.Add(cc);
                                h.lootCreatures.Add(cc.GetName());
                            }
                        }
                    }
                }
            }
            return creatureObjects;
        }

        void refreshHuntImages(Hunt h) {
            int spacing = 4;
            int totalWidth = spacing + spacing;
            int maxHeight = -1;
            float magnification = 1.0f;
            List<TibiaObject> creatureObjects = refreshLootCreatures(h);
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
            saveHunts();
        }

        private void sideHuntBox_CheckedChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.sideHunt = (sender as CheckBox).Checked;
            saveHunts();
        }

        private void aggregateHuntBox_CheckedChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.aggregateHunt = (sender as CheckBox).Checked;
            saveHunts();
        }

        private void trackCreaturesBox_TextChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = hunts[huntBox.SelectedIndex];
            h.trackedCreatures = (sender as RichTextBox).Text;

            saveHunts();
            refreshHuntImages(h);
        }

        private void trackCreaturesCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            bool chk = (sender as CheckBox).Checked;

            Hunt h = getActiveHunt();
            h.trackAllCreatures = chk;

            saveHunts();
        }

        public bool getSettingBool(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return false;
            return settings[key][0] == "True";
        }

        public int getSettingInt(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return -1;
            int v;
            if (int.TryParse(settings[key][0], out v)) {
                return v;
            }
            return -1;
        }
        public double getSettingDouble(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return -1;
            double v;
            if (double.TryParse(settings[key][0], NumberStyles.Any, CultureInfo.InvariantCulture, out v)) {
                return v;
            }
            return -1;
        }

        public string getSettingString(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return null;
            return settings[key][0];
        }

        public void setSetting(string key, string value) {
            if (!settings.ContainsKey(key)) settings.Add(key, new List<string>());
            settings[key].Clear();
            settings[key].Add(value);
        }

        public void setSetting(string key, int value) {
            setSetting(key, value.ToString());
        }
        public void setSetting(string key, bool value) {
            setSetting(key, value.ToString());
        }
        public void setSetting(string key, List<string> value) {
            if (!settings.ContainsKey(key)) settings.Add(key, value);
            else settings[key] = value;
        }

        public bool settingExists(string key) {
            return settings.ContainsKey(key) && settings[key].Count > 0;
        }

        private void rareDropNotificationValueCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("ShowNotificationsValue", (sender as CheckBox).Checked.ToString());
            saveSettings();

            this.showNotificationsValue = (sender as CheckBox).Checked;
        }

        private void notificationValue_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            int value;
            if (int.TryParse((sender as TextBox).Text, out value)) {
                this.notification_value = value;
                setSetting("NotificationValue", notification_value.ToString());
                saveSettings();
            }
        }

        private void goldCapRatioCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("ShowNotificationsGoldRatio", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        private void goldCapRatioValue_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            double value;
            if (double.TryParse((sender as TextBox).Text, NumberStyles.Any, CultureInfo.InvariantCulture, out value)) {
                this.notification_goldratio = value;
                setSetting("NotificationGoldRatio", notification_goldratio.ToString(CultureInfo.InvariantCulture));
                saveSettings();
            }
        }

        private void notificationSpecific_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("ShowNotificationsSpecific", (sender as CheckBox).Checked.ToString());
            saveSettings();

            this.showNotificationsSpecific = (sender as CheckBox).Checked;
            specificNotificationTextbox.Enabled = (sender as CheckBox).Checked;
        }

        private void specificNotificationTextbox_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            List<string> names = new List<string>();

            string[] lines = (sender as RichTextBox).Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
                names.Add(lines[i].ToLower());
            settings["NotificationItems"] = names;

            saveSettings();
        }

        private void showNotificationCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            string chk = (sender as CheckBox).Checked.ToString();

            setSetting("ShowNotifications", chk);
            saveSettings();

            this.showNotifications = (sender as CheckBox).Checked;

            notificationPanel.Enabled = (sender as CheckBox).Checked;
        }

        private void notificationTypeBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("UseRichNotificationType", ((sender as ComboBox).SelectedIndex == 1).ToString());
            saveSettings();

            this.lootNotificationRich = (sender as ComboBox).SelectedIndex == 1;
        }

        private void outfitGenderBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("OutfitGenderMale", ((sender as ComboBox).SelectedIndex == 0).ToString());
            saveSettings();
        }

        private void notificationLengthSlider_Scroll(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            setSetting("NotificationDuration", (sender as TrackBar).Value.ToString());
            saveSettings();

            this.notificationLength = (sender as TrackBar).Value;
            this.notificationLabel.Text = "Notification Length: " + notificationLength.ToString() + " Seconds";
        }

        private void enableRichNotificationsCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("EnableRichNotifications", (sender as CheckBox).Checked.ToString());
            saveSettings();

            this.richNotifications = (sender as CheckBox).Checked;
        }

        private void enableSimpleNotifications_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("EnableSimpleNotifications", (sender as CheckBox).Checked.ToString());
            saveSettings();

            this.simpleNotifications = (sender as CheckBox).Checked;
        }

        private void eventNotificationEnable_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("EnableEventNotifications", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        private void unrecognizedCommandNotification_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("EnableUnrecognizedNotifications", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        private void advanceCopyCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("CopyAdvances", (sender as CheckBox).Checked.ToString());
            saveSettings();

            this.copyAdvances = (sender as CheckBox).Checked;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            List<string> names = new List<string>();

            string[] lines = (sender as RichTextBox).Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
                names.Add(lines[i]);
            settings["Names"] = names;

            saveSettings();
        }

        private void lookCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("LookMode", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        private void alwaysShowLoot_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("AlwaysShowLoot", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        private void enableScreenshotBox_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("EnableScreenshots", (sender as CheckBox).Checked.ToString());
            saveSettings();

            this.screenshotPanel.Enabled = (sender as CheckBox).Checked;
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
            Process tibia_process = GetTibiaProcess();
            if (tibia_process == null)  return null; //no tibia to take screenshot of

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
            string path = getSettingString("ScreenshotPath");
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
            string selectedValue = screenshotList.SelectedIndex >= 0 ? screenshotList.Items[screenshotList.SelectedIndex].ToString() : null;
            int index = 0;

            string path = getSettingString("ScreenshotPath");
            if (path == null) return;

            if (!Directory.Exists(path)) {
                return;
            }

            string[] files = Directory.GetFiles(path);

            refreshingScreenshots = true;

            screenshotList.Items.Clear();
            foreach (string file in files) {
                if (imageExtensions.Contains(Path.GetExtension(file).ToLower())) { //check if file is an image
                    string f = Path.GetFileName(file);
                    if (f == selectedValue) {
                        index = screenshotList.Items.Count;
                    }
                    screenshotList.Items.Add(f);
                }
            }

            refreshingScreenshots = false;
            if (screenshotList.Items.Count > 0) {
                screenshotList.SelectedIndex = index;
            }
        }

        private void screenshotBrowse_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = getSettingString("ScreenshotPath");
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                setSetting("ScreenshotPath", folderBrowserDialog1.SelectedPath);
                screenshotDirectoryBox.Text = getSettingString("ScreenshotPath");
                refreshScreenshots();
            }
        }

        private void autoScreenshot_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("AutoScreenshotAdvance", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        private void autoScreenshotDrop_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("AutoScreenshotItemDrop", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        private void autoScreenshotDeath_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("AutoScreenshotDeath", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        bool refreshingScreenshots = false;
        private void screenshotList_SelectedIndexChanged(object sender, EventArgs e) {
            if (refreshingScreenshots) return;
            if (screenshotList.SelectedIndex >= 0) {
                string selectedImage = screenshotList.Items[screenshotList.SelectedIndex].ToString();

                string path = getSettingString("ScreenshotPath");
                if (path == null) return;

                string imagePath = Path.Combine(path, selectedImage);

                Image image = Image.FromFile(imagePath);
                if (image != null) {
                    if (screenshotBox.Image != null) {
                        screenshotBox.Image.Dispose();
                    }
                    screenshotBox.Image = image;
                }
            }
        }

        private void openInExplorer_Click(object sender, EventArgs e) {
            string path = getSettingString("ScreenshotPath");
            if (path == null) return;
            Process.Start(path);
        }

        private void startAutohotkeyScript_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("StartAutohotkeyAutomatically", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }
        private void shutdownOnExit_CheckedChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("ShutdownAutohotkeyOnExit", (sender as CheckBox).Checked.ToString());
            saveSettings();
        }

        static string autoHotkeyURL = "http://ahkscript.org/download/ahk-install.exe";
        private void downloadAutoHotkey_Click(object sender, EventArgs e) {
            WebClient client = new WebClient();

            client.DownloadDataCompleted += Client_DownloadDataCompleted;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;

            downloadBar.Visible = true;
            downloadLabel.Visible = true;
            downloadLabel.Text = "Downloading...";

            client.DownloadDataAsync(new Uri(autoHotkeyURL));
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            this.downloadBar.Value = e.ProgressPercentage;
            this.downloadBar.Maximum = 100;
        }

        private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
            downloadLabel.Text = "Writing...";
            try {
                string filepath = System.IO.Path.GetTempPath() + "autohotkeyinstaller.exe";
                Console.WriteLine(filepath);
                File.WriteAllBytes(filepath, e.Result);
                System.Diagnostics.Process.Start(filepath);
                downloadLabel.Text = "Download successful.";
            } catch {
                downloadLabel.Text = "Failed to download.";
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

        private void autoHotkeyGridSettings_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            if (!settings.ContainsKey("AutoHotkeySettings")) settings.Add("AutoHotkeySettings", autoHotkeyGridSettings.Text.Split('\n').ToList());
            else settings["AutoHotkeySettings"] = autoHotkeyGridSettings.Text.Split('\n').ToList();
            saveSettings();
            this.autohotkeyWarningLabel.Visible = true;
        }

        private void writeToAutoHotkeyFile() {
            if (!settings.ContainsKey("AutoHotkeySettings")) return;
            using (StreamWriter writer = new StreamWriter(autohotkeyFile)) {
                writer.WriteLine("#SingleInstance force");
                writer.WriteLine("#IfWinActive ahk_class TibiaClient");
                foreach (string l in settings["AutoHotkeySettings"]) {
                    string line = l.ToLower();
                    if (line.Length == 0 || line[0] == '#') continue;
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

        private void startAutoHotkey_Click(object sender, EventArgs e) {
            this.autohotkeyWarningLabel.Visible = false;
            writeToAutoHotkeyFile();
            System.Diagnostics.Process.Start(autohotkeyFile);
        }

        private void shutdownAutoHotkey_Click(object sender, EventArgs e) {
            foreach (var process in Process.GetProcessesByName("AutoHotkey")) {
                process.Kill();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (getSettingBool("ShutdownAutohotkeyOnExit")) {
                shutdownAutoHotkey_Click(null, null);
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
                Process tibia_process = GetTibiaProcess();
                if (tibia_process == null) {
                    screen = Screen.FromControl(this);
                } else {
                    screen = Screen.FromHandle(tibia_process.MainWindowHandle);
                }
                window = new AutoHotkeySuspendedMode(alwaysShow);
                int position_x = 0, position_y = 0;
                int xOffset = getSettingInt("SuspendedNotificationXOffset") < 0 ? 10 : getSettingInt("SuspendedNotificationXOffset");
                int yOffset = getSettingInt("SuspendedNotificationYOffset") < 0 ? 10 : getSettingInt("SuspendedNotificationYOffset");
                int anchor = getSettingInt("SuspendedNotificationAnchor");
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
            lock (suspendedLock) {
                if (window != null && !window.IsDisposed) {
                    try {
                        window.Close();
                    } catch {

                    }
                    window = null;
                }
            }
        }

        private void unlockResetSettingsButton_CheckedChanged(object sender, EventArgs e) {
            resetSettingsPanel.Enabled = (sender as CheckBox).Checked;
        }

        private string defaultWASDSettings = @"# Suspend autohotkey mode with Ctrl+Enter
Ctrl+Enter::Suspend
# Enable WASD Movement
W::Up
A::Left
S::Down
D::Right
# Enable diagonal movement with QEZC
Q::NumpadHome
E::NumpadPgUp
Z::NumpadEnd
C::NumpadPgDn
# Hotkey Tibialyzer commands
# Open loot window with the [ key
[::Command=loot@
# Show exp with ] key 
]::Command=exp@ 
# Close all windows when = key is pressed
=::Command=close@ 
# Open last window with - key
-::Command=refresh@ 
";

        private void ResetSettingsToDefault() {
            settings = new Dictionary<string, List<string>>();
            setSetting("NotificationDuration", 30);
            setSetting("EnableSimpleNotifications", true);
            setSetting("EnableEventNotifications", true);
            setSetting("EnableUnrecognizedNotifications", true);
            setSetting("EnableRichNotifications", true);
            setSetting("CopyAdvances", true);
            setSetting("ShowNotifications", true);
            setSetting("UseRichNotificationType", true);
            setSetting("ShowNotificationsValue", true);
            setSetting("NotificationValue", 2000);
            setSetting("ShowNotificationsRatio", false);
            setSetting("NotificationGoldRatio", 100);
            setSetting("ShowNotificationsSpecific", true);
            setSetting("LookMode", true);
            setSetting("AlwaysShowLoot", false);
            setSetting("StartAutohotkeyAutomatically", false);
            setSetting("ShutdownAutohotkeyOnExit", false);
            setSetting("NotificationItems", "");
            setSetting("AutoHotkeySettings", defaultWASDSettings.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList());
            setSetting("AutoScreenshotAdvance", false);
            setSetting("AutoScreenshotItemDrop", false);
            setSetting("AutoScreenshotDeath", false);
            setSetting("EnableScreenshots", false);
            setSetting("Names", "Mytherin");
            setSetting("ScanSpeed", "0");
            setSetting("OutfitGenderMale", true);
            setSetting("RichNotificationXOffset", 30);
            setSetting("RichNotificationYOffset", 30);
            setSetting("RichNotificationAnchor", 0);
            setSetting("SimpleNotificationXOffset", 5);
            setSetting("SimpleNotificationYOffset", 10);
            setSetting("SimpleNotificationAnchor", 3);
            setSetting("EnableSimpleNotificationAnimation", true);
            setSetting("SuspendedNotificationXOffset", 10);
            setSetting("SuspendedNotificationYOffset", 10);
            setSetting("SuspendedNotificationAnchor", 1);
            setSetting("TibiaClientName", "Tibia");

            saveSettings();
        }

        private void resetToDefaultButton_Click(object sender, EventArgs e) {
            ResetSettingsToDefault();
            shutdownAutoHotkey_Click(null, null);
            initializeSettings();
            initializeNames();
        }

        public string[] scanSpeedText = { "Fastest", "Fast", "Medium", "Slow", "Slowest" };

        private void scanningSpeedTrack_Scroll(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("ScanSpeed", scanningSpeedTrack.Value);
            saveSettings();
            scanSpeedDisplayLabel.Text = scanSpeedText[scanningSpeedTrack.Value];
        }

        private void showLootButton_Click(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null) {
                ExecuteCommand("loot" + MainForm.commandSymbol + h.name);
            }
        }


        private void pickupRare_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + "100");
        }

        private void pickupHigh_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + "25");

        }

        private void pickupGold_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + "10");
        }

        private void pickupPlate_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + "3");

        }

        private void pickupEverything_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + "0");
        }

        private void convertAllStackable_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "1-9999999999");
        }

        private void noConvertStackable_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "1-0");
        }

        private void convertNonStackable_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "0-9999999999");
        }

        private void convertCheapNonStackable_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "0-10");
        }

        private void dontConvert_Click(object sender, EventArgs e) {
            this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "0-0");
        }

        private void PickupCreatureDrops(bool pickup) {
            using (var transaction = conn.BeginTransaction()) {
                SQLiteCommand command;
                command = new SQLiteCommand(String.Format("UPDATE Items SET discard={0} WHERE category='{1}';", !pickup ? "1" : "0", "Creature Products"), conn, transaction);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            foreach (int id in _itemIdMap.Keys.ToArray().ToList()) {
                Item item = _itemIdMap[id];
                if (item.category == "Creature Products") {
                    item.discard = !pickup;
                }
            }
        }

        private void pickupCreature_Click(object sender, EventArgs e) {
            PickupCreatureDrops(true);
        }

        private void nopickupCreature_Click(object sender, EventArgs e) {
            PickupCreatureDrops(false);
        }
        private void applyRatioButton_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(goldRatioTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) {
                this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + goldRatioTextBox.Text);
            }
        }

        private void stackableConvertApply_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(stackableConvertTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) {
                this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "1-" + stackableConvertTextBox.Text);
            }
        }

        private void unstackableConvertApply_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(unstackableConvertTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out val)) {
                this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "0-" + unstackableConvertTextBox.Text);
            }
        }

        private void richAnchor_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("RichNotificationAnchor", richAnchor.SelectedIndex);
            saveSettings();
        }

        private void richXOffset_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            int xOffset;
            if (int.TryParse((sender as TextBox).Text, out xOffset)) {
                setSetting("RichNotificationXOffset", xOffset);
                saveSettings();
            }
        }

        private void richYOffset_TextChanged(object sender, EventArgs e) {
            int yOffset;
            if (int.TryParse((sender as TextBox).Text, out yOffset)) {
                setSetting("RichNotificationYOffset", yOffset);
                saveSettings();
            }
        }

        private void richTestDisplay_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("creature@demon");
        }

        private void simpleAnchor_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("SimpleNotificationAnchor", simpleAnchor.SelectedIndex);
            saveSettings();
        }

        private void simpleXOffset_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            int xOffset;
            if (int.TryParse((sender as TextBox).Text, out xOffset)) {
                setSetting("SimpleNotificationXOffset", xOffset);
                saveSettings();
            }
        }

        private void simpleYOffset_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            int yOffset;
            if (int.TryParse((sender as TextBox).Text, out yOffset)) {
                setSetting("SimpleNotificationYOffset", yOffset);
                saveSettings();
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

            setSetting("EnableSimpleNotificationAnimation", (sender as CheckBox).Checked);
            saveSettings();
        }

        private void suspendedTest_Click(object sender, EventArgs e) {
            ShowSuspendedWindow(true);
        }

        private void closeSuspendedWindow_Click(object sender, EventArgs e) {
            CloseSuspendedWindow();
        }

        private void suspendedAnchor_SelectedIndexChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            setSetting("SuspendedNotificationAnchor", suspendedAnchor.SelectedIndex);
            saveSettings();
        }

        private void suspendedXOffset_TextChanged(object sender, EventArgs e) {
            int xOffset;
            if (int.TryParse((sender as TextBox).Text, out xOffset)) {
                setSetting("SuspendedNotificationXOffset", xOffset);
                saveSettings();
            }
        }

        private void suspendedYOffset_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;

            int yOffset;
            if (int.TryParse((sender as TextBox).Text, out yOffset)) {
                setSetting("SuspendedNotificationYOffset", yOffset);
                saveSettings();
            }
        }

        private void selectClientButton_Click(object sender, EventArgs e) {
            SelectProcessForm form = new SelectProcessForm();
            form.Show();
        }
    }

    public class Loot {
        public Dictionary<string, List<string>> logMessages = new Dictionary<string, List<string>>();
        public Dictionary<Creature, Dictionary<Item, int>> creatureLoot = new Dictionary<Creature, Dictionary<Item, int>>();
        public Dictionary<Creature, int> killCount = new Dictionary<Creature, int>();
    };

    public class Hunt {
        public int dbtableid;
        public string name;
        public bool temporary = false;
        public bool trackAllCreatures = true;
        public bool sideHunt = false;
        public bool aggregateHunt = false;
        public bool clearOnStartup = false;
        public string trackedCreatures = "";
        public long totalExp = 0;
        public double totalTime = 0;
        public Loot loot = new Loot();
        public List<string> lootCreatures = new List<string>();

        public string GetTableName() {
            return "LootMessageTable" + dbtableid.ToString();
        }

        public override string ToString() {
            return name + "#" + dbtableid.ToString() + "#" + trackAllCreatures.ToString() + "#" + totalTime.ToString(CultureInfo.InvariantCulture) + "#" + totalExp.ToString() + "#" + sideHunt.ToString() + "#" + aggregateHunt.ToString() + "#" + clearOnStartup.ToString() + "#" + trackedCreatures.Replace("\n", "#");
        }
    };

    public class TibialyzerCommand {
        public string command;
        public TibialyzerCommand(string command) {
            this.command = command;
        }
    }
}
