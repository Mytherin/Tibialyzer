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
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
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

        public Dictionary<string, Item> itemNameMap = new Dictionary<string, Item>();
        public Dictionary<int, Item> itemIdMap = new Dictionary<int, Item>();
        public Dictionary<string, Creature> creatureNameMap = new Dictionary<string, Creature>();
        public Dictionary<int, Creature> creatureIdMap = new Dictionary<int, Creature>();
        public Dictionary<string, NPC> npcNameMap = new Dictionary<string, NPC>();
        public Dictionary<int, NPC> npcIdMap = new Dictionary<int, NPC>();
        public Dictionary<string, HuntingPlace> huntingPlaceNameMap = new Dictionary<string, HuntingPlace>();
        public Dictionary<int, HuntingPlace> huntingPlaceIdMap = new Dictionary<int, HuntingPlace>();

        public static ScriptEngine pyEngine = null;
        public static Color background_color = Color.FromArgb(0, 51, 102);
        public static double opacity = 0.8;
        public static bool transparent = true;
        public static Image[] image_numbers = new Image[10];
        private Form tooltipForm = null;
        private static Image tibia_image = null;
        public static Image back_image = null;
        public static Image prevpage_image = null;
        public static Image nextpage_image = null;
        public static Image item_background = null;
        public static Image cross_image = null;
        public static Image[] star_image = new Image[5];
        public static Image mapup_image = null;
        public static Image mapdown_image = null;
        public static Image checkmark_yes = null;
        public static Image checkmark_no = null;
        public static Image infoIcon = null;
        private bool keep_working = true;
        private static string databaseFile = @"Database\Database.db";
        private static string settingsFile = @"Database\settings.txt";
        private static string pluralMapFile = @"Database\pluralMap.txt";
        private static string autohotkeyFile = @"Database\autohotkey.ahk";
        private List<string> character_names = new List<string>();
        public static List<Map> map_files = new List<Map>();
        public static Color label_text_color = Color.FromArgb(191, 191, 191);
        public static int max_creatures = 50;
        public List<string> new_names = null;
        private bool prevent_settings_update = false;
        private bool minimize_notification = true;
        public int notification_value = 2000;
        static HashSet<string> cities = new HashSet<string>() { "ab'dendriel", "carlin", "kazordoon", "venore", "thais", "ankrahmun", "farmine", "gray beach", "liberty bay", "port hope", "rathleton", "roshamuul", "yalahar", "svargrond", "edron", "darashia", "rookgaard", "dawnport", "gray beach" };
        public List<string> notification_items = new List<string>();
        private static List<string> extensions = new List<string>();
        private ToolTip scan_tooltip = new ToolTip();
        private Stack<string> command_stack = new Stack<string>();

        private SQLiteConnection conn;
        static Dictionary<string, Image> creatureImages = new Dictionary<string, Image>();

        enum ScanningState { Scanning, NoTibia, Stuck };
        ScanningState current_state;

        private Image loadingbar = null;
        private Image loadingbarred = null;
        private Image loadingbargray = null;

        public MainForm() {
            mainForm = this;
            InitializeComponent();

            conn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", databaseFile));
            conn.Open();

            back_image = Image.FromFile(@"Images\back.png");
            prevpage_image = Image.FromFile(@"Images\prevpage.png");
            nextpage_image = Image.FromFile(@"Images\nextpage.png");
            cross_image = Image.FromFile(@"Images\cross.png");
            tibia_image = Image.FromFile(@"Images\tibia.png");
            mapup_image = Image.FromFile(@"Images\mapup.png");
            mapdown_image = Image.FromFile(@"Images\mapdown.png");
            checkmark_no = Image.FromFile(@"Images\checkmark-no.png");
            checkmark_yes = Image.FromFile(@"Images\checkmark-yes.png");
            infoIcon = Image.FromFile(@"Images\defaulticon.png");

            item_background = System.Drawing.Image.FromFile(@"Images\item_background.png");
            for (int i = 0; i < 10; i++) {
                image_numbers[i] = System.Drawing.Image.FromFile(@"Images\" + i.ToString() + ".png");
            }

            if (Directory.Exists("Extensions")) {
                string[] files = Directory.GetFiles("Extensions");
                for (int i = 0; i < files.Length; i++) {
                    if (files[i].EndsWith(".py")) {
                        string[] split = files[i].Split('\\');
                        extensions.Add(split[split.Length - 1].Substring(0, split[split.Length - 1].Length - 3));
                    }
                }
            }
            NotificationForm.Initialize();
            CreatureStatsForm.InitializeCreatureStats();
            HuntListForm.Initialize();

            star_image[0] = Image.FromFile(@"Images\star0.png");
            star_image[1] = Image.FromFile(@"Images\star1.png");
            star_image[2] = Image.FromFile(@"Images\star2.png");
            star_image[3] = Image.FromFile(@"Images\star3.png");
            star_image[4] = Image.FromFile(@"Images\star4.png");

            prevent_settings_update = true;
            this.initializePluralMap();
            this.loadDatabaseData();
            this.loadSettings();
            this.initializeNames();
            this.initializeHunts();
            this.initializeSettings();
            this.initializeMaps();
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

            this.loadingbar = new Bitmap(@"Images\scanningbar.gif");
            this.loadingbarred = new Bitmap(@"Images\scanningbar-red.gif");
            this.loadingbargray = new Bitmap(@"Images\scanningbar-gray.gif");

            this.loadTimerImage.Image = this.loadingbarred;
            this.current_state = ScanningState.NoTibia;
            this.loadTimerImage.Enabled = true;
            scan_tooltip.SetToolTip(this.loadTimerImage, "No Tibia Client Found...");
        }

        public static int DATABASE_NULL = -127;
        public static string DATABASE_STRING_NULL = "";
        private void loadDatabaseData() {
            // first load creatures from the database
            SQLiteCommand command = new SQLiteCommand("SELECT id, name, health, experience, maxdamage, summon, illusionable, pushable, pushes, physical, holy, death, fire, energy, ice, earth, drown, lifedrain, paralysable, senseinvis, image, abilities FROM Creatures", conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                Creature cr = new Creature();
                cr.id = reader.GetInt32(0);
                cr.name = reader["name"].ToString();
                cr.health = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetInt32(2);
                cr.experience = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
                cr.maxdamage = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetInt32(4);
                cr.summoncost = reader.IsDBNull(5) ? DATABASE_NULL : reader.GetInt32(5);
                cr.illusionable = reader.GetBoolean(6);
                cr.pushable = reader.GetBoolean(7);
                cr.pushes = reader.GetBoolean(8);
                cr.res_phys = reader.GetInt32(9);
                cr.res_holy = reader.GetInt32(10);
                cr.res_death = reader.GetInt32(11);
                cr.res_fire = reader.GetInt32(12);
                cr.res_energy = reader.GetInt32(13);
                cr.res_ice = reader.GetInt32(14);
                cr.res_earth = reader.GetInt32(15);
                cr.res_drown = reader.GetInt32(16);
                cr.res_lifedrain = reader.GetInt32(17);
                cr.paralysable = reader.GetBoolean(18);
                cr.senseinvis = reader.GetBoolean(19);
                cr.image = Image.FromStream(reader.GetStream(20));
                cr.abilities = reader.IsDBNull(21) ? DATABASE_STRING_NULL : reader["abilities"].ToString();

                creatureNameMap.Add(cr.name.ToLower(), cr);
                creatureIdMap.Add(cr.id, cr);
            }

            // now load items
            command = new SQLiteCommand("SELECT id, name, actual_value, vendor_value, stackable, capacity, category, image, discard, convert_to_gold, look_text FROM Items", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Item item = new Item();
                item.id = reader.GetInt32(0);
                item.name = reader.GetString(1);
                item.actual_value = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetInt32(2);
                item.vendor_value = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
                item.stackable = reader.GetBoolean(4);
                item.capacity = reader.GetFloat(5);
                item.category = reader.GetString(6);
                item.image = Image.FromStream(reader.GetStream(7));
                item.discard = reader.GetBoolean(8);
                item.convert_to_gold = reader.GetBoolean(9);
                item.look_text = reader.GetString(10);

                if (item.image.RawFormat.Guid == ImageFormat.Gif.Guid) {
                    int frames = item.image.GetFrameCount(FrameDimension.Time);
                    if (frames == 1) {
                        Bitmap new_bitmap = new Bitmap(item.image);
                        new_bitmap.MakeTransparent();
                        item.image.Dispose();
                        item.image = new_bitmap;
                    }
                }

                itemNameMap.Add(item.name.ToLower(), item);
                itemIdMap.Add(item.id, item);
            }

            // skins for the creatures
            command = new SQLiteCommand("SELECT creatureid, itemid, skinitemid, percentage FROM Skins", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Skin skin = new Skin();
                int creatureid = reader.GetInt32(0);
                int itemid = reader.GetInt32(1);
                int skinitemid = reader.GetInt32(2);
                skin.percentage = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetFloat(3);

                Creature creature = creatureIdMap[creatureid];
                Item item = itemIdMap[itemid];
                Item skinItem = itemIdMap[skinitemid];

                skin.drop_item = item;
                skin.skin_item = skinItem;
                creature.skin = skin;
            }


            // creature drops
            command = new SQLiteCommand("SELECT creatureid, itemid, percentage FROM CreatureDrops;", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int creatureid = reader.GetInt32(0);
                int itemid = reader.GetInt32(1);
                float percentage = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetFloat(2);

                Item item = itemIdMap[itemid];
                Creature creature = creatureIdMap[creatureid];
                ItemDrop itemDrop = new ItemDrop();
                itemDrop.item = item;
                itemDrop.creature = creature;
                itemDrop.percentage = percentage;

                item.itemdrops.Add(itemDrop);
                creature.itemdrops.Add(itemDrop);
            }

            // NPCs
            command = new SQLiteCommand("SELECT id,name,city,x,y,z,image FROM NPCs;", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                NPC npc = new NPC();
                npc.id = reader.GetInt32(0);
                npc.name = reader["name"].ToString();
                npc.city = reader["city"].ToString();
                npc.pos.x = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetFloat(3);
                npc.pos.y = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetFloat(4);
                npc.pos.z = reader.IsDBNull(5) ? DATABASE_NULL : reader.GetInt32(5);
                npc.image = Image.FromStream(reader.GetStream(6));

                // special case for rashid: change location based on day of the week
                if (npc != null && npc.name == "Rashid") {
                    SQLiteCommand rashidCommand = new SQLiteCommand(String.Format("SELECT city, x, y, z FROM RashidPositions WHERE day='{0}'", DateTime.Now.DayOfWeek.ToString()), conn);
                    SQLiteDataReader rashidReader = rashidCommand.ExecuteReader();
                    if (rashidReader.Read()) {
                        npc.city = rashidReader["city"].ToString();
                        npc.pos.x = rashidReader.GetFloat(1);
                        npc.pos.y = rashidReader.GetFloat(2);
                        npc.pos.z = rashidReader.GetInt32(3);
                    }
                }
                npcNameMap.Add(npc.name.ToLower(), npc);
                npcIdMap.Add(npc.id, npc);
            }

            // items that you can buy from NPCs
            command = new SQLiteCommand("SELECT itemid, vendorid, value FROM BuyItems;", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemSold buyItem = new ItemSold();
                int itemid = reader.GetInt32(0);
                int vendorid = reader.GetInt32(1);
                buyItem.price = reader.GetInt32(2);

                Item item = itemIdMap[itemid];
                NPC npc = npcIdMap[vendorid];
                buyItem.npc = npc;
                buyItem.item = item;
                item.buyItems.Add(buyItem);
                npc.buyItems.Add(buyItem);
            }
            // items that you can sell to NPCs
            command = new SQLiteCommand("SELECT itemid, vendorid, value FROM SellItems;", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemSold sellItem = new ItemSold();
                int itemid = reader.GetInt32(0);
                int vendorid = reader.GetInt32(1);
                sellItem.price = reader.GetInt32(2);

                Item item = itemIdMap[itemid];
                NPC npc = npcIdMap[vendorid];
                sellItem.npc = npc;
                sellItem.item = item;
                item.sellItems.Add(sellItem);
                npc.sellItems.Add(sellItem);
            }

            // Hunting Places
            command = new SQLiteCommand("SELECT id, name, level, exprating, lootrating, image, city FROM HuntingPlaces;", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                HuntingPlace huntingPlace = new HuntingPlace();
                huntingPlace.id = reader.GetInt32(0);
                huntingPlace.name = reader["name"].ToString();
                huntingPlace.level = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetInt32(2);
                huntingPlace.exp_quality = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
                huntingPlace.loot_quality = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetInt32(4);
                huntingPlace.image = Image.FromStream(reader.GetStream(5));
                huntingPlace.city = reader["city"].ToString();

                huntingPlaceNameMap.Add(huntingPlace.name.ToLower(), huntingPlace);
                huntingPlaceIdMap.Add(huntingPlace.id, huntingPlace);
            }

            // Coordinates for hunting places
            command = new SQLiteCommand("SELECT huntingplaceid, x, y, z FROM HuntingPlaceCoordinates", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Coordinate c = new Coordinate();
                int huntingplaceid = reader.GetInt32(0);
                c.x = reader.IsDBNull(1) ? DATABASE_NULL : reader.GetFloat(1);
                c.y = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetFloat(2);
                c.z = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
                if (huntingPlaceIdMap.ContainsKey(huntingplaceid)) huntingPlaceIdMap[huntingplaceid].coordinates.Add(c);
            }

            // Hunting place directions
            command = new SQLiteCommand("SELECT huntingplaceid, x, y, z, ordering, name, notes FROM HuntingPlaceDirections ORDER BY ordering", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Directions d = new Directions();
                d.huntingplaceid = reader.GetInt32(0);
                d.x = reader.IsDBNull(1) ? DATABASE_NULL : reader.GetFloat(1);
                d.y = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetFloat(2);
                d.z = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
                d.name = reader["name"].ToString();
                d.notes = reader["notes"].ToString();
                if (huntingPlaceIdMap.ContainsKey(d.huntingplaceid)) huntingPlaceIdMap[d.huntingplaceid].directions.Add(d);
            }

            // Hunting place requirements
            command = new SQLiteCommand("SELECT huntingplaceid, questid, notes FROM HuntingPlaceRequirements", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Requirements r = new Requirements();
                r.huntingplaceid = reader.GetInt32(0);
                r.questid = reader.IsDBNull(1) ? DATABASE_NULL : reader.GetInt32(1);
                r.notes = reader["notes"].ToString();
                if (huntingPlaceIdMap.ContainsKey(r.huntingplaceid)) huntingPlaceIdMap[r.huntingplaceid].requirements.Add(r);
            }

            // Hunting place creatures
            command = new SQLiteCommand("SELECT huntingplaceid, creatureid FROM HuntingPlaceCreatures", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int huntingplaceid = reader.GetInt32(0);
                int creatureid = reader.GetInt32(1);
                if (huntingPlaceIdMap.ContainsKey(huntingplaceid) && creatureIdMap.ContainsKey(creatureid)) {
                    huntingPlaceIdMap[huntingplaceid].creatures.Add(creatureIdMap[creatureid]);
                }
            }
        }

        void initializePluralMap() {
            StreamReader reader = new StreamReader(pluralMapFile);
            string line;
            while ((line = reader.ReadLine()) != null) {
                if (line.Contains('=')) {
                    string[] split = line.Split('=');
                    if (!pluralMap.ContainsKey(split[0])) {
                        pluralMap.Add(split[0], split[1]);
                    }
                }
            }
            reader.Close();
        }

        class Hunt {
            public string name;
            public bool temporary;
            public bool trackAllCreatures;
            public string trackedCreatures;
            public Loot loot = new Loot();

            public override string ToString() {
                return name + "#" + trackAllCreatures.ToString() + "#" + trackedCreatures.Replace("\n", "#");
            }
        };

        class Loot {
            public Dictionary<string, List<string>> logMessages = new Dictionary<string, List<string>>();
            public Dictionary<Creature, Dictionary<Item, int>> creatureLoot = new Dictionary<Creature, Dictionary<Item, int>>();
            public Dictionary<Creature, int> killCount = new Dictionary<Creature, int>();
        };

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

        void saveSettings() {
            StreamWriter file = new StreamWriter(settingsFile);
            foreach (KeyValuePair<string, List<string>> pair in settings) {
                file.WriteLine("@" + pair.Key);
                foreach (string str in pair.Value) {
                    file.WriteLine(str);
                }
            }
            file.Close();
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
            //"Name#Track#Creature#Creature#Creature#Creature"
            if (!settings.ContainsKey("Hunts")) {
                settings.Add("Hunts", new List<string>() { "New Hunt#True#" });
            }
            int activeHuntIndex = 0, index = 0;
            foreach (string str in settings["Hunts"]) {
                SQLiteCommand command; SQLiteDataReader reader;
                Hunt hunt = new Hunt();
                string[] splits = str.Split('#');
                if (splits.Length >= 3) {
                    hunt.name = splits[0];
                    hunt.trackAllCreatures = splits[1] == "True";
                    hunt.temporary = false;
                    string massiveString = "";
                    for (int i = 2; i < splits.Length; i++) {
                        if (splits[i].Length > 0) {
                            massiveString += splits[i] + "\n";
                        }
                    }
                    hunt.trackedCreatures = massiveString;
                    // set this hunt to the active hunt if it is the active hunt
                    if (settings.ContainsKey("ActiveHunt") && settings["ActiveHunt"].Count > 0 && settings["ActiveHunt"][0] == hunt.name)
                        activeHuntIndex = index;

                    // create the hunt table if it does not exist
                    command = new SQLiteCommand(String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"(day INTEGER, hour INTEGER, minute INTEGER, message STRING);", hunt.name.ToLower()), conn);
                    command.ExecuteNonQuery();
                    // load the data for the hunt from the database
                    command = new SQLiteCommand(String.Format("SELECT message FROM \"{0}\" ORDER BY day, hour, minute;", hunt.name.ToLower()), conn);
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

            skip_hunt_refresh = true;
            huntBox.Items.Clear();
            foreach (Hunt h in hunts) {
                huntBox.Items.Add(h.name);
            }
            activeHunt = hunts[activeHuntIndex];
            skip_hunt_refresh = false;
            huntBox.SelectedIndex = activeHuntIndex;
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

            this.notificationLengthSlider.Value = notificationLength;
            this.enableSimpleNotifications.Checked = simpleNotifications;
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
                bool success = ScanMemory();
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
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM WorldMap", conn);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                Map m = new Map();
                m.z = reader.GetInt32(0);
                m.image = Image.FromStream(reader.GetStream(1));
                map_files.Add(m);
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

        private void simpleNotificationClosed(object sender, FormClosedEventArgs e) {
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

        private void ShowNotification(NotificationForm f, string command, bool screenshot = false) {
            if (!richNotifications) return;

            command_stack.Push(command);
            Console.WriteLine(command_stack.Count);
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

            if (screenshot) {
                f.Close();
                tooltipForm = null;
            } else tooltipForm = f;
        }

        public void Back() {
            if (command_stack.Count <= 1) return;
            command_stack.Pop(); // remove the current command
            string command = command_stack.Pop();
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

        private void ShowItemView(Item i, Dictionary<NPC, int> BuyNPCs, Dictionary<NPC, int> SellNPCs, List<Creature> creatures, string comm) {
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
            DamageChart f = new DamageChart(screenshot_path);
            f.dps = dps;
            f.filter = filter;

            ShowNotification(f, comm, screenshot_path != "");
        }

        private void ShowLootDrops(Dictionary<Creature, int> creatures, List<Tuple<Item, int>> items, string comm, string screenshot_path) {
            LootDropForm ldf = new LootDropForm(screenshot_path);
            ldf.creatures = creatures;
            ldf.items = items;

            ShowNotification(ldf, comm, screenshot_path != "");
        }

        private void ShowHuntList(List<HuntingPlace> h, string header, string comm) {
            if (h != null) h = h.OrderBy(o => o.level).ToList();
            HuntListForm f = new HuntListForm();
            f.hunting_places = h;
            f.header = header;

            ShowNotification(f, comm);
        }

        private void ShowHuntingPlace(HuntingPlace h, string comm) {
            HuntingPlaceForm f = new HuntingPlaceForm();
            f.hunting_place = h;

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

        public static int DisplayCreatureList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, int max_x, int spacing, bool transparent, Func<TibiaObject, string> tooltip_function = null, float magnification = 1.0f) {
            int x = 0, y = 0;
            int width = 0, height = 0;
            foreach (TibiaObject cr in l) {
                Image image = cr.GetImage();
                if (image.Width * magnification > width) width = (int)(image.Width * magnification);
                if (image.Height * magnification > height) height = (int)(image.Height * magnification);
            }

            // add a tooltip that displays the creature names
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            foreach (TibiaObject cr in l) {
                Image image = cr.GetImage();
                string name = cr.GetName();
                if (max_x < (x + base_x + (int)(image.Width * magnification) + spacing)) {
                    x = 0;
                    y = y + spacing + height;
                }
                PictureBox image_box;
                if (transparent) image_box = new PictureBox();
                else image_box = new PictureBox();
                image_box.Image = image;
                image_box.BackColor = Color.Transparent;
                image_box.Size = new Size((int)(image.Width * magnification), height);
                image_box.Location = new Point(base_x + x, base_y + y);
                image_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                image_box.Name = name;
                controls.Add(image_box);
                image_box.Image = image;
                if (tooltip_function == null) {
                    value_tooltip.SetToolTip(image_box, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name));
                } else {
                    value_tooltip.SetToolTip(image_box, tooltip_function(cr));
                }

                x = x + (int)(image.Width * magnification) + spacing;
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
            DisplayCreatureList(this.creaturePanel.Controls, creatureNameMap.Values.Where(o => o.name.ToLower().Contains(creature) && count++ < 40).ToList<TibiaObject>(), 10, 10, this.creaturePanel.Width - 20, 4, false);
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
            int count = 0;
            DisplayCreatureList(this.itemPanel.Controls, itemNameMap.Values.Where(o => o.name.ToLower().Contains(item) && count++ < 40).ToList<TibiaObject>(), 10, 10, this.itemPanel.Width - 20, 4, false);
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
            return hunts[huntBox.SelectedIndex];
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
            if (!nameExists("New Hunt")) {
                h.name = "New Hunt";
            } else {
                int index = 1;
                while (nameExists("New Hunt " + index)) index++;
                h.name = "New Hunt " + index;
            }
            activeHunt = h;
            h.trackAllCreatures = true;
            h.trackedCreatures = "";
            hunts.Add(h);
            refreshHunts();
        }

        private void deleteHuntButton_Click(object sender, EventArgs e) {
            if (hunts.Count <= 1) return;
            Hunt h = getSelectedHunt();
            hunts.Remove(h);
            saveHunts();
            refreshHunts(true);
        }

        private void startupHuntCheckbox_CheckedChanged(object sender, EventArgs e) {

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
            trackCreaturesBox.Enabled = !h.trackAllCreatures;
            trackCreaturesBox.Text = h.trackedCreatures;
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

            huntBox.Items.Clear();
            foreach (Hunt hunt in hunts) {
                huntBox.Items.Add(hunt.name);
                if (hunt == h) currentHunt = huntBox.Items.Count - 1;
            }
            huntBox.SelectedIndex = refreshSelection ? 0 : currentHunt;
            activeHunt = hunts[huntBox.SelectedIndex];

            skip_hunt_refresh = false;
            huntBox_SelectedIndexChanged(huntBox, null);
        }

        void saveHunts() {
            List<string> huntStrings = new List<string>();
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

        private void huntNameBox_TextChanged(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            string oldTable = h.name;
            string newTable = (sender as TextBox).Text;
            if (oldTable == newTable || newTable.Length <= 0) return;
            h.name = newTable;
            SQLiteCommand comm = new SQLiteCommand(String.Format("ALTER TABLE \"{0}\" RENAME TO \"{1}\";", oldTable, h.name), conn);
            comm.ExecuteNonQuery();
            saveHunts();
            refreshHunts();
        }

        private void activeHuntButton_Click(object sender, EventArgs e) {
            if (switch_hunt) return;
            Hunt h = getSelectedHunt();
            activeHuntButton.Text = "Currently Active";
            activeHuntButton.Enabled = false;
            activeHunt = h;
            saveHunts();
        }

        List<string> lootCreatures = new List<string>();
        void refreshHuntImages(Hunt h) {
            int spacing = 4;
            string[] creatures = h.trackedCreatures.Split('\n');
            List<TibiaObject> creatureObjects = new List<TibiaObject>();
            int totalWidth = spacing + spacing;
            int maxHeight = -1;
            foreach (string cr in creatures) {
                string name = cr.ToLower();
                if (creatureNameMap.ContainsKey(name) && !creatureObjects.Any(item => item.GetName() == name)) {
                    Creature cc = creatureNameMap[name];
                    totalWidth += cc.image.Width + spacing;
                    maxHeight = Math.Max(maxHeight, cc.image.Height);
                    creatureObjects.Add(cc);
                    lootCreatures.Add(name);
                }
            }
            float magnification = 1.0f;
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
            DisplayCreatureList(creatureImagePanel.Controls, creatureObjects, 0, 0, creatureImagePanel.Width, spacing, false, null, magnification);
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
            this.creatureTrackLabel.Visible = !chk;
            this.trackCreaturesBox.Enabled = !chk;

            Hunt h = getActiveHunt();
            h.trackAllCreatures = chk;

            saveHunts();
        }
        
        private bool getSettingBool(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return false;
            return settings[key][0] == "True";
        }

        private int getSettingInt(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return -1;
            int v;
            if (int.TryParse(settings[key][0], out v)) {
                return v;
            }
            return -1;
        }

        private bool getSetting(string key) {
            return settings.ContainsKey(key) && settings[key].Count > 0 && settings[key][0] == "True";
        }

        private void setSetting(string key, string value) {
            if (!settings.ContainsKey(key)) settings.Add(key, new List<string>());
            settings[key].Clear();
            settings[key].Add(value);
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
        
        private string modifyKeyString(string str) {
            string value = str.ToLower();
            
            if (value.Contains("alt+")) {
                value = value.Replace("alt+", "!");
            }
            if (value.Contains("ctrl+")) {
                value = value.Replace("ctrl+", "^");
            }
            if (value.Contains("shift+")) {
                value = value.Replace("shift+", "+");
            }
            return value;
        }

        private void autoHotkeyGridSettings_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            if (!settings.ContainsKey("AutoHotkeySettings")) settings.Add("AutoHotkeySettings", autoHotkeyGridSettings.Text.Split('\n').ToList());
            else settings["AutoHotkeySettings"] = autoHotkeyGridSettings.Text.Split('\n').ToList();
            saveSettings();
        }

        private void writeToAutoHotkeyFile() {
            if (!settings.ContainsKey("AutoHotkeySettings")) return;
            using (StreamWriter writer = new StreamWriter(autohotkeyFile)) {
                writer.WriteLine("#SingleInstance force");
                writer.WriteLine("#IfWinActive ahk_class TibiaClient");
                foreach (string line in settings["AutoHotkeySettings"]) {
                    if (line.Length == 0 || line[0] == '#') continue;
                    if (line.ToLower().Contains("suspend")) {
                        // if the key is set to suspend the hotkey layout, we set it up so it sends a message to us 
                        writer.WriteLine(line.ToLower().Split(new string[] { "suspend" }, StringSplitOptions.None)[0]);
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
            if (m != null && m.Msg == 0x317) {
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
    }
}
