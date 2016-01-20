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
        static HashSet<string> cities = new HashSet<string>() { "ab'dendriel", "carlin", "kazordoon", "venore", "thais", "ankrahmun", "farmine", "gray beach", "liberty bay", "port hope", "rathleton", "roshamuul", "yalahar", "svargrond", "edron", "darashia", "rookgaard", "dawnport", "gray beach" };
        public List<string> notification_items = new List<string>();
        private ToolTip scan_tooltip = new ToolTip();
        private Stack<TibialyzerCommand> command_stack = new Stack<TibialyzerCommand>();
        public static List<Font> fontList = new List<Font>();

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
            HuntListForm.Initialize();

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
            } catch {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.", databaseFile));
            }
            this.loadSettings();
            MainForm.initializeFonts();
            this.initializeNames();
            this.initializeHunts();
            this.initializeSettings();
            this.initializeMaps();
            try {
                Pathfinder.LoadFromDatabase(nodeDatabase);
            } catch {
                ExitWithError("Fatal Error", String.Format("Corrupted database {0}.", nodeDatabase));
            }
            prevent_settings_update = false;

            if (getSettingBool("StartAutohotkeyAutomatically")) {
                startAutoHotkey_Click(null, null);
            }

            ignoreStamp = createStamp();

            this.backgroundBox.Image = NotificationForm.background_image;

            BackgroundWorker bw = new BackgroundWorker();
            makeDraggable(this.Controls);
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
                quest.city = reader.IsDBNull(5) ? "Unknown" : reader.GetString(5);
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
        List<Hunt> hunts = new List<Hunt>();
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
                    double.TryParse(splits[3], out hunt.totalTime);
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
                        command = new SQLiteCommand(String.Format("DROP TABLE IF EXISTS \"{0}\"", hunt.GetTableName()), lootConn);
                        command.ExecuteNonQuery();
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
            this.showNotificationsSpecific = getSettingBool("ShowNotificationsSpecific");

            this.richNotificationsPanel.Enabled = richNotifications;
            this.notificationPanel.Enabled = showNotifications;
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
            this.rareDropNotificationValueCheckbox.Checked = showNotificationsValue;
            this.notificationValue.Text = notification_value.ToString();
            this.specificNotificationCheckbox.Checked = showNotificationsSpecific;
            this.lookCheckBox.Checked = getSettingBool("LookMode");
            this.alwaysShowLoot.Checked = getSettingBool("AlwaysShowLoot");
            this.startAutohotkeyScript.Checked = getSettingBool("StartAutohotkeyAutomatically");
            this.shutdownOnExit.Checked = getSettingBool("ShutdownAutohotkeyOnExit");
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
                    circleTimer = new System.Timers.Timer(1000);
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
            Process[] tibia_process = Process.GetProcessesByName("Tibia");
            if (tibia_process.Length == 0) {
                screen = Screen.FromControl(this);
            } else {
                Process tibia = tibia_process[0];
                screen = Screen.FromHandle(tibia.MainWindowHandle);
            }
            position_x = screen.WorkingArea.Right - f.Width - notificationSpacing;
            int basePosition = screen.WorkingArea.Bottom;
            foreach (SimpleNotification notification in notificationStack) {
                basePosition -= notification.Height + notificationSpacing;
            }
            position_y = basePosition - (f.Height + notificationSpacing);
            f.StartPosition = FormStartPosition.Manual;
            f.SetDesktopLocation(position_x + f.Width + notificationSpacing, position_y);
            Console.WriteLine(position_y);
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
            foreach (SimpleNotification f in notificationStack) {
                if (f == notification) {
                    positionModification = f.Height + notificationSpacing;
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
            Process[] tibia_process = Process.GetProcessesByName("Tibia");
            if (tibia_process.Length == 0) {
                screen = Screen.FromControl(this);
            } else {
                Process tibia = tibia_process[0];
                screen = Screen.FromHandle(tibia.MainWindowHandle);
            }
            position_x = screen.WorkingArea.Left + 30;
            position_y = screen.WorkingArea.Top + 30;
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
        private void ShowCreatureList(List<TibiaObject> c, string title, string prefix, string comm) {
            if (c == null) return;
            CreatureList f = new CreatureList();
            f.objects = c;
            f.title = title;
            f.prefix = prefix;

            ShowNotification(f, comm);
        }

        private void ShowItemView(Item i, Dictionary<NPC, int> BuyNPCs, Dictionary<NPC, int> SellNPCs, Dictionary<Creature, float> creatures, string comm) {
            if (i == null) return;
            ItemViewForm f = new ItemViewForm();
            f.item = i;
            f.buyNPCs = BuyNPCs;
            f.sellNPCs = SellNPCs;
            f.creatures = creatures;

            ShowNotification(f, comm);
        }

        private void ShowNPCForm(NPC c, string comm) {
            if (c == null) return;
            NPCForm f = new NPCForm();
            f.npc = c;

            ShowNotification(f, comm);
        }

        private void ShowDamageMeter(Dictionary<string, int> dps, string comm, string filter = "", string screenshot_path = "") {
            DamageChart f = new DamageChart();
            f.dps = dps;
            f.filter = filter;

            ShowNotification(f, comm, screenshot_path);
        }

        private void ShowLootDrops(Dictionary<Creature, int> creatures, List<Tuple<Item, int>> items, Hunt h, string comm, string screenshot_path) {
            LootDropForm ldf = new LootDropForm();
            ldf.creatures = creatures;
            ldf.items = items;
            ldf.hunt = h;

            ShowNotification(ldf, comm, screenshot_path);
        }

        private void ShowHuntList(List<HuntingPlace> h, string header, string comm, int page) {
            if (h != null) h = h.OrderBy(o => o.level).ToList();
            HuntListForm f = new HuntListForm();
            f.hunting_places = h;
            f.header = header;
            f.initialPage = page;

            ShowNotification(f, comm);
        }

        private void ShowHuntingPlace(HuntingPlace h, string comm) {
            HuntingPlaceForm f = new HuntingPlaceForm();
            f.hunting_place = h;

            ShowNotification(f, comm);
        }

        private void ShowSpellNotification(Spell spell, string comm) {
            SpellForm f = new SpellForm(spell);

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

        private void ShowQuestList(List<Quest> questList, string header, string comm, int page) {
            if (questList != null) questList = questList.OrderBy(o => o.minlevel).ToList();
            HuntListForm f = new HuntListForm();
            f.quests = questList;
            f.header = header;
            f.initialPage = page;

            ShowNotification(f, comm);
        }

        private void ShowHuntGuideNotification(HuntingPlace hunt, string comm, int page) {
            if (hunt.directions.Count == 0) return;
            QuestGuideForm f = new QuestGuideForm(hunt);
            f.initialPage = page;

            ShowNotification(f, comm);
        }

        private void ShowQuestGuideNotification(Quest quest, string comm, int page, string mission) {
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
            public PageInfo(bool prevPage, bool nextPage) {
                this.prevPage = prevPage;
                this.nextPage = nextPage;
            }
        }

        public static int DisplayCreatureAttributeList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, out int maxwidth, Func<TibiaObject, string> tooltip_function = null, List<Control> createdControls = null, int page = 0, int pageitems = 20, PageInfo pageInfo = null, string extraAttribute = null, Func<TibiaObject, Attribute> attributeFunction = null) {
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
            List<TibiaObject> pageItems = new List<TibiaObject>();
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
                }
                offset++;
            }
            Dictionary<string, int> totalAttributes = new Dictionary<string, int>();
            foreach (TibiaObject obj in pageItems) {
                List<string> headers = obj.GetAttributeHeaders();
                List<Attribute> attributes = obj.GetAttributes();
                if (extraAttribute != null) {
                    headers.Add(extraAttribute);
                    attributes.Add(attributeFunction(obj));
                }
                for (int i = 0; i < headers.Count; i++) {
                    string header = headers[i];
                    Attribute attribute = attributes[i];
                    int width = -1;
                    if (attribute is StringAttribute) {
                        width = TextRenderer.MeasureText((attribute as StringAttribute).value, HuntListForm.text_font).Width;
                    } else if (attribute is ImageAttribute) {
                        width = (attribute as ImageAttribute).value.Width;
                    } else if (attribute is BooleanAttribute) {
                        width = 20;
                    } else {
                        throw new Exception("Unrecognized attribute.");
                    }
                    width = Math.Min(width, attribute.MaxWidth);
                    if (!totalAttributes.ContainsKey(header)) {
                        int headerWidth = TextRenderer.MeasureText(header, HuntListForm.text_font).Width;
                        totalAttributes.Add(header, Math.Max(headerWidth, width));
                    } else if (totalAttributes[header] < width) {
                        totalAttributes[header] = width;
                    }
                }
            }
            base_x += 24;
            maxwidth = base_x;
            // create header information
            int x = base_x;
            foreach (KeyValuePair<string, int> kvp in totalAttributes) {
                Label label = new Label();
                label.Text = kvp.Key;
                label.Location = new Point(x, base_y);
                label.ForeColor = MainForm.label_text_color;
                label.Size = new Size(kvp.Value, size);
                label.Font = HuntListForm.text_font;
                label.BackColor = Color.Transparent;
                controls.Add(label);
                if (createdControls != null) {
                    createdControls.Add(label);
                }
                x += kvp.Value;
                maxwidth += kvp.Value;
            }
            offset = 0;
            // create object information
            foreach (TibiaObject obj in pageItems) {
                List<string> headers = obj.GetAttributeHeaders();
                List<Attribute> attributes = obj.GetAttributes();
                if (extraAttribute != null) {
                    headers.Add(extraAttribute);
                    attributes.Add(attributeFunction(obj));
                }
                string command = obj.GetCommand();
                x = base_x;
                int y = size * (offset + 1) + base_y;
                // create main image
                PictureBox picture = new PictureBox();
                picture.Image = obj.GetImage();
                picture.Size = new Size(size, size);
                picture.SizeMode = PictureBoxSizeMode.Zoom;
                picture.Location = new Point(base_x - 24, y);
                picture.Click += executeNameCommand;
                picture.BackColor = Color.Transparent;
                picture.Name = command;
                if (obj is Item || (obj is LazyTibiaObject && (obj as LazyTibiaObject).type == TibiaObjectType.Item)) {
                    picture.BackgroundImage = MainForm.item_background;
                }
                controls.Add(picture);
                if (createdControls != null) {
                    createdControls.Add(picture);
                }
                // iterate over all attributes
                foreach (KeyValuePair<string, int> kvp in totalAttributes) {
                    int index = headers.IndexOf(kvp.Key);
                    Attribute attribute = attributes[index];
                    Control c;
                    if (attribute is StringAttribute) {
                        // create label
                        Label label = new Label();
                        label.Text = (attribute as StringAttribute).value;
                        label.Location = new Point(x, y);
                        label.ForeColor = (attribute as StringAttribute).color;
                        label.Size = new Size(kvp.Value, size);
                        label.Font = HuntListForm.text_font;
                        label.Click += executeNameCommand;
                        label.Name = command;
                        label.BackColor = Color.Transparent;
                        controls.Add(label);
                        if (createdControls != null) {
                            createdControls.Add(label);
                        }
                        c = label;
                    } else if (attribute is ImageAttribute || attribute is BooleanAttribute) {
                        // create picturebox
                        picture = new PictureBox();
                        picture.Image = (attribute is ImageAttribute) ? (attribute as ImageAttribute).value : ((attribute as BooleanAttribute).value ? MainForm.checkmark_yes : MainForm.checkmark_no);
                        picture.Size = new Size(imageSize, imageSize);
                        picture.SizeMode = PictureBoxSizeMode.Zoom;
                        picture.Location = new Point(x + (kvp.Value - imageSize) / 2, y);
                        picture.Click += executeNameCommand;
                        picture.BackColor = Color.Transparent;
                        picture.Name = command;
                        controls.Add(picture);
                        if (createdControls != null) {
                            createdControls.Add(picture);
                        }
                        c = picture;
                    } else {
                        throw new Exception("Unrecognized attribute.");
                    }
                    if (tooltip_function == null) {
                        value_tooltip.SetToolTip(c, obj.GetName());
                    } else {
                        value_tooltip.SetToolTip(c, tooltip_function(obj));
                    }
                    x += kvp.Value;
                }
                offset++;
            }
            clicked = false;
            return (offset + 1) * size;
        }

        private static bool clicked = false;
        private static void executeNameCommand(object sender, EventArgs e) {
            if (clicked) return;
            mainForm.ExecuteCommand((sender as Control).Name);
        }

        public static int DisplayCreatureList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, int max_x, int spacing, Func<TibiaObject, string> tooltip_function = null, float magnification = 1.0f, List<Control> createdControls = null, int page = 0, int pageheight = 10000, PageInfo pageInfo = null) {
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
            foreach (TibiaObject cr in l) {
                int imageWidth;
                int imageHeight;
                Image image = cr.GetImage();
                string name = cr.GetName();

                if ((cr is Item || (cr is LazyTibiaObject && (cr as LazyTibiaObject).type == TibiaObjectType.Item)) ||
                    (cr is Spell || (cr is LazyTibiaObject && (cr as LazyTibiaObject).type == TibiaObjectType.Spell))) {
                    imageWidth = 32;
                    imageHeight = 32;
                } else {
                    imageWidth = image.Width;
                    imageHeight = image.Height;
                }


                if (max_x < (x + base_x + (int)(imageWidth * magnification) + spacing)) {
                    x = 0;
                    y = y + spacing + height;
                    height = 0;
                    if (y > pageheight) {
                        if (page > currentPage) {
                            y = 0;
                            currentPage += 1;
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
                    image_box.Name = name;
                    if (cr is Item || (cr is LazyTibiaObject && (cr as LazyTibiaObject).type == TibiaObjectType.Item)) {
                        image_box.BackgroundImage = MainForm.item_background;
                    }
                    controls.Add(image_box);
                    if (createdControls != null) createdControls.Add(image_box);
                    image_box.Image = image;
                    if (tooltip_function == null) {
                        value_tooltip.SetToolTip(image_box, MainForm.ToTitle(name));
                    } else {
                        string prefix = "";
                        if (cr is NPC || (cr is LazyTibiaObject && (cr as LazyTibiaObject).type == TibiaObjectType.NPC)) {
                            NPC npc = cr is NPC ? cr as NPC : (cr as LazyTibiaObject).getTibiaObject() as NPC;
                            prefix = MainForm.ToTitle(name) + " (" + MainForm.ToTitle(npc.city) + ")\n";
                        }
                        value_tooltip.SetToolTip(image_box, prefix + tooltip_function(cr));
                    }
                }

                x = x + (int)(imageWidth * magnification) + spacing;
            }
            x = 0;
            y = y + height;
            return y;
        }


        private void creatureSearch_TextChanged(object sender, EventArgs e) {
            string creature = (sender as TextBox).Text.ToLower();
            this.SuspendLayout();
            this.creaturePanel.Controls.Clear();
            int count = 0;
            DisplayCreatureList(this.creaturePanel.Controls, MainForm.searchCreature(creature), 10, 10, this.creaturePanel.Width - 20, 4);
            foreach (Control c in creaturePanel.Controls) {
                if (c is PictureBox) {
                    c.Click += ShowCreatureInformation;
                }
            }
            this.ResumeLayout(false);
        }
        private void itemSearchBox_TextChanged(object sender, EventArgs e) {
            string item = (sender as TextBox).Text;
            this.SuspendLayout();
            this.itemPanel.Controls.Clear();
            DisplayCreatureList(this.itemPanel.Controls, MainForm.searchItem(item), 10, 10, this.itemPanel.Width - 20, 4);
            foreach (Control c in itemPanel.Controls) {
                if (c is PictureBox) {
                    c.Click += ShowItemInformation;
                }
            }
            this.ResumeLayout(false);
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
            this.ExecuteCommand("reset" + MainForm.commandSymbol);
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

        private void applyRatioButton_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(goldRatioTextBox.Text, out val)) {
                this.ExecuteCommand("setdiscardgoldratio" + MainForm.commandSymbol + goldRatioTextBox.Text);
            }
        }

        private void stackableConvertApply_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(stackableConvertTextBox.Text, out val)) {
                this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "1-" + stackableConvertTextBox.Text);
            }
        }

        private void unstackableConvertApply_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(unstackableConvertTextBox.Text, out val)) {
                this.ExecuteCommand("setconvertgoldratio" + MainForm.commandSymbol + "0-" + unstackableConvertTextBox.Text);
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
                return hunts[huntBox.SelectedIndex];
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
            string massiveString = "";
            List<string> timestamps = h.loot.logMessages.Keys.OrderByDescending(o => o).ToList();
            foreach (string t in timestamps) {
                List<string> strings = h.loot.logMessages[t].ToArray().ToList();
                strings.Reverse();
                foreach (string str in strings) {
                    massiveString += str + "\n";
                }
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

            richNotificationsPanel.Enabled = (sender as CheckBox).Checked;
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
            Process[] tibia_process = Process.GetProcessesByName("Tibia");
            if (tibia_process.Length == 0) return null; //no tibia to take screenshot of

            RECT Rect = new RECT();
            if (!GetWindowRect(tibia_process[0].MainWindowHandle, ref Rect)) return null;

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
                    if (window == null) {
                        Screen screen;
                        Process[] tibia_process = Process.GetProcessesByName("Tibia");
                        if (tibia_process.Length == 0) {
                            screen = Screen.FromControl(this);
                        } else {
                            Process tibia = tibia_process[0];
                            screen = Screen.FromHandle(tibia.MainWindowHandle);
                        }
                        window = new AutoHotkeySuspendedMode();
                        window.StartPosition = FormStartPosition.Manual;
                        window.SetDesktopLocation(screen.WorkingArea.Right - window.Width - 10, screen.WorkingArea.Top + 10);
                        window.TopMost = true;
                        window.Show();
                    }
                } else if (wParam == 33) {
                    // 33 signifies we are not suspended, destroy the suspended window (if it exists)
                    if (window != null && !window.IsDisposed) {
                        try {
                            window.Close();
                        } catch {

                        }
                        window = null;
                    }
                }
            }
            base.WndProc(ref m);
        }

        private void stackableConvertTextBox_TextChanged(object sender, EventArgs e) {

        }

        private void stackableConvertApply_Click_1(object sender, EventArgs e) {

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
C::NumpadPgDn";

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
            setSetting("ShowNotificationsSpecific", true);
            setSetting("LookMode", true);
            setSetting("AlwaysShowLoot", false);
            setSetting("StartAutohotkeyAutomatically", false);
            setSetting("ShutdownAutohotkeyOnExit", false);
            setSetting("NotificationItems", "");
            setSetting("AutoHotkeySettings", defaultWASDSettings);
            setSetting("AutoScreenshotAdvance", false);
            setSetting("AutoScreenshotItemDrop", false);
            setSetting("AutoScreenshotDeath", false);
            setSetting("EnableScreenshots", false);
            setSetting("Names", "Mytherin");
            setSetting("ScanSpeed", "0");

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
            scanSpeedDisplayLabel.Text = scanSpeedText[scanningSpeedTrack.Value];
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
            return name + "#" + dbtableid.ToString() + "#" + trackAllCreatures.ToString() + "#" + totalTime.ToString() + "#" + totalExp.ToString() + "#" + sideHunt.ToString() + "#" + aggregateHunt.ToString() + "#" + clearOnStartup.ToString() + "#" + trackedCreatures.Replace("\n", "#");
        }
    };

    public class TibialyzerCommand {
        public string command;
        public TibialyzerCommand(string command) {
            this.command = command;
        }
    }
}
