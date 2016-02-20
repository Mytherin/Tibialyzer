using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class Constants {
        public static Random Random = new Random();
        public static List<string> vocations = new List<string> { "knight", "druid", "paladin", "sorcerer" };
        public static string LootDatabaseFile = @"Database\Loot.db";
        public static string DatabaseFile = @"Database\Database.db";
        public static string NodeDatabase = @"Database\Nodes.db";
        public static string PluralMapFile = @"Database\pluralMap.txt";
        public static string AutohotkeyFile = @"Database\autohotkey.ahk";
        public static string SettingsFile = @"Database\settings.txt";
        public static string BigLootFile = @"Database\loot.txt";
        public static HashSet<string> cities = new HashSet<string>() { "ab'dendriel", "carlin", "kazordoon", "venore", "thais", "ankrahmun", "farmine", "gray beach", "liberty bay", "port hope", "rathleton", "roshamuul", "yalahar", "svargrond", "edron", "darashia", "rookgaard", "dawnport", "gray beach" };
    }
}
