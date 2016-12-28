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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class Constants {
        public static Random Random = new Random();
        //! Cities in Tibia
        public static HashSet<string> cities = new HashSet<string>() { "ab'dendriel", "carlin", "kazordoon", "venore", "thais", "ankrahmun", "farmine", "gray beach", "liberty bay", "port hope", "rathleton", "roshamuul", "yalahar", "svargrond", "edron", "darashia", "rookgaard", "dawnport", "gray beach" };
        //! Game worlds of Tibia
        public static HashSet<string> worlds = new HashSet<string>() { "amera", "antica", "astera", "aurera", "aurora", "bellona", "belobra", "beneva", "calmera", "calva", "calvera", "candia", "celesta", "chrona", "danera", "dolera", "efidia", "eldera", "ferobra", "fidera", "fortera", "garnera", "guardia", "harmonia", "honera", "hydera", "inferna", "iona", "irmada", "julera", "justera", "kenora", "kronera", "laudera", "luminera", "magera", "menera", "morta", "mortera", "neptera", "nerana", "nika", "olympa", "osera", "pacera", "premia", "pythera", "quilia", "refugia", "rowana", "secura", "serdebra", "shivera", "silvera", "solera", "thera", "umera", "unitera", "veludera", "xantera", "xylana", "yanara", "zanera", "zeluna" };

        //! City Locations in Tibia
        public static Dictionary<string, string> cityCoordinates = new Dictionary<string, string>() {
                {"ab'dendriel", "916,697,7"},
                {"ankrahmun", "1396,1838,7"},
                {"carlin", "592,810,7"},
                {"darashia", "1483,1450,7"},
                {"edron", "1442,844,7"},
                {"farmine", "1275,557,10"},
                {"gray beach", "1708,343,7"},
                {"kazordoon", "883,944,8"},
                {"liberty bay", "576,1854,7"},
                {"port hope", "877,1774,7"},
                {"rathleton", "1877,921,7"},
                {"rookgaard", "352,1225,7"},
                {"roshamuul", "1799,1402,7"},
                {"svargrond", "514,170,7"},
                {"thais", "622,1234,7"},
                {"venore", "1173,1090,6"},
                {"yalahar", "1053,228,7"}
        };
        //! Vocations in Tibia
        public static List<string> vocations = new List<string> { "knight", "druid", "paladin", "sorcerer" };
        //! Location of Loot database; used for storing loot found by the player
        public static string LootDatabaseFile = @"Database\loot.db";
        //! Location of the main database; this database contains all Tibia-related information (creatures, items, npcs, etc)
        public static string DatabaseFile = @"Database\database.db";
        //! Location of temporary new database after being downloaded
        public static string NewDatabaseFile = @"Database\new_database.db";
        //! Location of temporary old database while being replaced
        public static string OldDatabaseFile = @"Database\old_database.db";
        //! Location of outfiter database
        public static string OutfiterDatabaseFile = @"Database\outfits.db";
        //! Location of file containing Tibia memory addresses
        public static string MemoryAddresses = @"Database\MemoryAddresses.txt";
        //! Location of the node database; this database contains information used by the pathfinder
        public static string NodeDatabase = @"Database\Nodes.db";
        //! Location of the plural map file; this file contains a map of plural items that don't follow the normal plural rules
        public static string PluralMapFile = @"Database\pluralMap.txt";
        //! Location of where to put the generated autohotkey script; Tibialyzer generates an autohotkey script and launches it
        public static string AutohotkeyFile = @"Database\autohotkey.ahk";
        //! Location of the settings file; this is where Tibialyzer stores all settings
        public static string SettingsFile = @"Database\settings.txt";
        //! This is the file that Tibialyzer will write new settings to
        public static string AttemptedSettingsFile = @"Database\tmpsettings.txt";
        //! Location of the settings backup file
        public static string SettingsBackupFile = @"Database\settings-backup.txt";
        //! Location of the temporary settings file (possibly incomplete)
        public static string SettingsTemporaryFile = @"Database\tempsettings.txt";
        //! Location of the temporary, complete file
        public static string SettingsTemporaryBackup = @"Database\settings-tempbackup.txt";

        //! Location of the big loot file; if enabled, all loot found is automatically written to this file
        public static string BigLootFile = @"Database\loot.txt";

        //! Map of { plural suffix: singular suffix } used to find singular form of item from plural
        public static Dictionary<string, string> pluralSuffixes = new Dictionary<string, string> {
            { "ches", "ch" },
            { "shes", "sh" },
            { "ies", "y" },
            { "ves", "fe" },
            { "oes", "o" },
            { "zes", "z" },
            { "s", "" }
        };

        //! Map of { plural word: singular word } used to find singular form of item from plural
        public static Dictionary<string, string> pluralWords = new Dictionary<string, string> {
            { "pieces of", "piece of" },
            { "bunches of", "bunch of" },
            { "haunches of", "haunch of" },
            { "flasks of", "flask of" },
            { "veins of", "vein of" },
            { "bowls of", "bowl of" }
        };


        public static string[] ScanSpeedText;

        public static char CommandSymbol = '@';

        public static List<string> NotificationTypes = new List<string> { "Loot Notification", "Damage Notification", "Object List", "City Information", "Creature Loot Information", "Creature Stats Information", "Hunt Information", "Item Information", "NPC Information", "Outfit Information", "Quest Information", "Spell Information", "Quest/Hunt Directions", "Task Form", "Experience Chart", "Waste Form", "Summary Form", "Route Form", "Map Form", "Healing Chart", "Damage Taken Chart", "Achievement Notification", "Player Notification", "Outfiter Notification", "House Form" };
        public static List<string> NotificationTestCommands = new List<string> { "loot@", "damage@", "creature@quara", "city@venore", "creature@demon", "stats@dragon lord", "hunt@formorgar mines", "item@heroic axe", "npc@rashid", "outfit@brotherhood", "quest@killing in the name of", "spell@light healing", "guide@desert dungeon quest", "task@crystal spider", "experience@", "waste@", "summary@", "route@1500,500,7", "map@", "healing@", "damagetaken@", "achievement@peazzekeeper", "player@mytherin", "outfiter@o=38&a1&a2&f=2&c2=68&c3=58&c4=11&m=61", "house@central circle 1" };
        public static List<string> NotificationTypeObjects = new List<string>() { "LootDropForm", "DamageChart", "CreatureList", "CityDisplayForm", "CreatureDropsForm", "CreatureStatsForm", "HuntingPlaceForm", "ItemViewForm", "NPCForm", "OutfitForm", "QuestForm", "SpellForm", "QuestGuideForm", "TaskForm", "ExperienceChart", "WasteForm", "SummaryForm", "RouteForm", "MapForm", "HealingChart", "DamageTakenChart", "AchievementForm", "PlayerForm", "OutfiterForm", "HouseForm" };

        public static List<string> HudTypes = new List<string> { "Health Bar", "Mana Bar", "Experience Bar", "Curved Bars", "Health List", "Portrait", "Task HUD" };
        public static List<string> HudTestCommands = new List<string> { "hud@healthbar", "hud@manabar", "hud@experiencebar", "hud@curvedbars", "hud@healthlist", "hud@portrait", "hud@task" };

        public static List<string> ImageExtensions = new List<string> { ".jpg", ".bmp", ".gif", ".png" };

        public static string AutoHotkeyURL = "http://ahkscript.org/download/ahk-install.exe";

        public static List<string> DisplayItemList = new List<string> { "Ghoul Snack", "Plate Armor", "Halberd", "Steel Helmet", "Gold Coin", "Platinum Coin", "Dragon Hammer", "Knight Armor", "Spider Silk", "Giant Sword", "Crown Armor", "Golden Armor" };
        public static List<string> ConvertUnstackableItemList = new List<string> { "Mace", "Plate Armor", "Halberd", "Steel Helmet", "War Hammer", "Dragon Hammer", "Knight Armor", "Giant Sword", "Crown Armor", "Golden Armor" };
        public static List<string> ConvertStackableItemList = new List<string> { "Spear", "Burst Arrow", "Mana Potion", "Strong Mana Potion", "Great Mana Potion", "Great Fireball Rune", "Black Hood", "Strand of Medusa Hair", "Small Ruby", "Spider Silk" };

        public static int MaximumNotificationDuration = 300;

        public static List<Color> DamageChartColors = new List<Color> { Color.FromArgb(65, 140, 240), Color.FromArgb(252, 180, 65), Color.FromArgb(224, 64, 10), Color.FromArgb(5, 100, 146), Color.FromArgb(191, 191, 191), Color.FromArgb(26, 59, 105), Color.FromArgb(255, 227, 130), Color.FromArgb(18, 156, 221), Color.FromArgb(202, 107, 75), Color.FromArgb(0, 92, 219), Color.FromArgb(243, 210, 136), Color.FromArgb(80, 99, 129), Color.FromArgb(241, 185, 168), Color.FromArgb(224, 131, 10), Color.FromArgb(120, 147, 190) };
        public static List<Color> DamageTakenChartColors = new List<Color> { Color.FromArgb(255, 128, 0), Color.FromArgb(184, 134, 11), Color.FromArgb(192, 64, 0), Color.FromArgb(107, 142, 35), Color.FromArgb(205, 133, 63), Color.FromArgb(192, 192, 0), Color.FromArgb(34, 139, 34), Color.FromArgb(210, 105, 30), Color.FromArgb(128, 128, 0), Color.FromArgb(32, 178, 170), Color.FromArgb(244, 164, 96), Color.FromArgb(0, 192, 0), Color.FromArgb(143, 188, 139), Color.FromArgb(178, 34, 34), Color.FromArgb(139, 69, 19), Color.FromArgb(192, 0, 0) };
        public static List<Color> HealingChartColors = new List<Color> { Color.FromArgb(160, 82, 45), Color.FromArgb(210, 105, 30), Color.FromArgb(139, 0, 0), Color.FromArgb(205, 133, 63), Color.FromArgb(165, 42, 42), Color.FromArgb(244, 164, 96), Color.FromArgb(139, 69, 19), Color.FromArgb(192, 64, 0), Color.FromArgb(178, 34, 34), Color.FromArgb(182, 92, 58) };

        // Not a constant, but don't tell anyone!
        public static bool OBSEnableWindowCapture = false;

        public static string[] OldMemoryAddresses = new string[] { "7.72", "8.57", "8.60", "8.61", "8.62", "8.70", "8.71", "8.72", "8.73", "8.74", "9.00", "9.10", "9.20", "9.31", "9.40", "9.41", "9.42", "9.43", "9.44", "9.45", "9.46", "9.50", "9.51", "9.52", "9.53", "9.54", "9.60", "9.61", "9.62", "9.63", "9.70", "9.71", "9.80", "9.81", "9.82", "9.83", "9.84", "9.85", "9.86", "9.90", "9.91", "9.92", "9.93", "10.00", "10.01", "10.02", "10.10", "10.11", "10.12", "10.20", "10.21", "10.22", "10.30", "10.31", "10.32", "10.33", "10.34", "10.35", "10.36", "10.37", "10.38", "10.39", "10.40", "10.41", "10.50", "10.51", "10.52", "10.53", "10.54", "10.55", "10.56", "10.57", "10.58", "10.59", "10.60", "10.61", "10.62", "10.63", "10.64", "10.70", "10.71", "10.72", "10.73", "10.74", "10.75", "10.76", "10.77", "10.78", "10.79", "10.80", "10.81", "10.82", "10.90", "10.91", "10.92", "10.93", "10.94", "10.95", "10.96", "10.97", "10.98" };

        public static void InitializeConstants() {
            ScanSpeedText = new string[]{ "Fastest", "Fast", "Fast", "Fast", "Medium", "Medium", "Medium", "Slow", "Slow", "Slow", "Slowest" };
        }
    }
}
