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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class Constants {
        public static Random Random = new Random();
        //! Cities in Tibia
        public static HashSet<string> cities = new HashSet<string>() { "ab'dendriel", "carlin", "kazordoon", "venore", "thais", "ankrahmun", "farmine", "gray beach", "liberty bay", "port hope", "rathleton", "roshamuul", "yalahar", "svargrond", "edron", "darashia", "rookgaard", "dawnport", "gray beach" };
        //! Vocations in Tibia
        public static List<string> vocations = new List<string> { "knight", "druid", "paladin", "sorcerer" };
        //! Location of Loot database; used for storing loot found by the player
        public static string LootDatabaseFile = @"Database\Loot.db";
        //! Location of the main database; this database contains all Tibia-related information (creatures, items, npcs, etc)
        public static string DatabaseFile = @"Database\Database.db";
        //! Location of the node database; this database contains information used by the pathfinder
        public static string NodeDatabase = @"Database\Nodes.db";
        //! Location of the plural map file; this file contains a map of plural items that don't follow the normal plural rules
        public static string PluralMapFile = @"Database\pluralMap.txt";
        //! Location of where to put the generated autohotkey script; Tibialyzer generates an autohotkey script and launches it
        public static string AutohotkeyFile = @"Database\autohotkey.ahk";
        //! Location of the settings file; this is where Tibialyzer stores all the settings of the player
        public static string SettingsFile = @"Database\settings.txt";
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


        public static string[] ScanSpeedText = { "Fastest", "Fast", "Fast", "Fast", "Medium", "Medium", "Medium", "Slow", "Slow", "Slow", "Slowest" };

        public static char CommandSymbol = '@';

        public static List<string> NotificationTypes = new List<string> { "Loot Notification", "Damage Notification", "Object List", "City Information", "Creature Loot Information", "Creature Stats Information", "Hunt Information", "Item Information", "NPC Information", "Outfit Information", "Quest Information", "Spell Information", "Quest/Hunt Directions", "Task Form" };
        public static List<string> NotificationTestCommands = new List<string> { "loot@", "damage@", "creature@quara", "city@venore", "creature@demon", "stats@dragon lord", "hunt@formorgar mines", "item@heroic axe", "npc@rashid", "outfit@brotherhood", "quest@killing in the name of", "spell@light healing", "guide@desert dungeon quest", "task@crystal spider" };
        public static List<Type> NotificationTypeObjects = new List<Type>() { typeof(LootDropForm), typeof(DamageChart), typeof(CreatureList), typeof(CityDisplayForm), typeof(CreatureDropsForm), typeof(CreatureStatsForm), typeof(HuntingPlaceForm), typeof(ItemViewForm), typeof(NPCForm), typeof(OutfitForm), typeof(QuestForm), typeof(SpellForm), typeof(QuestGuideForm), typeof(TaskForm) };

        public static List<string> ImageExtensions = new List<string> { ".jpg", ".bmp", ".gif", ".png" };

        public static string AutoHotkeyURL = "http://ahkscript.org/download/ahk-install.exe";
    }
}
