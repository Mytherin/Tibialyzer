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
    }
}
