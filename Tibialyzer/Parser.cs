﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace Tibialyzer {
    class Parser {
        private static Dictionary<string, string> pluralMap = new Dictionary<string, string>();

        public static void Initialize() {
            if (File.Exists(Constants.PluralMapFile)) {
                using (StreamReader reader = new StreamReader(Constants.PluralMapFile)) {
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

        public static IEnumerable<string> FindTimestamps(byte[] array) {
            int index = 0;
            // scan the memory for "timestamp values"
            // i.e. values that are like "xx:xx" where x = a number
            // we consider timestamps the "starting point" of a string, and the null terminator the "ending point"
            int start = 0, i = 0;
            for (i = 0; i < array.Length; i++) {
                if (index < 5) {
                    if (array[i] > 47 && array[i] < 59) { // digits are 47-57, colon is 58
                        index++;
                        start = i;
                    } else {
                        index = 0;
                    }
                } else if (array[i] == 0 || i == array.Length - 1) { // scan for the null terminator
                    start -= 4;
                    string str = System.Text.Encoding.UTF8.GetString(array, start, (i - start));
                    if (str[0].isDigit() && str[1].isDigit() && str[3].isDigit() && str[4].isDigit() && str[2] == ':') {
                        yield return str;
                    }
                    index = 0;
                }
            }

            yield break;
        }

        public static IEnumerable<string> FindTimestampsFlash(byte[] array) {
            // scan the memory for "timestamp values"
            // i.e. values that are like "xx:xx" where x = a number
            // we consider timestamps the "starting point" of a string, and the null terminator the "ending point"
            for (int i = 0; i < array.Length - 6; i++) {
                if (array[i] >= '0' && array[i] <= '9'
                    && array[i + 1] >= '0' && array[i + 1] <= '9'
                    && array[i + 2] == ':'
                    && array[i + 3] >= '0' && array[i + 3] <= '9'
                    && array[i + 4] >= '0' && array[i + 4] <= '9'
                    && (array[i + 5] == ' ' || array[i + 5] == ':')) {
                    int start = i;
                    i += 6;
                    while (array[i] != '\0') {
                        ++i;
                    }

                    if (!EndsWith(array, start, i, "</font></p>") && !EndsWith(array, start, i, "</font>")) {
                        yield return Encoding.UTF8.GetString(array, start, i - start);
                    }
                }
            }

            yield break;
        }

        private static bool EndsWith(byte[] array, int start, int end, string text) {
            int strLen = text.Length;

            if (end - start < strLen) {
                return false;
            }

            for (int i = 0; i < strLen; ++i) {
                if (text[i] != array[end - strLen + i]) {
                    return false;
                }
            }

            return true;
        }

        public static string parseLookItem(string logMessage) {
            string[] splits = logMessage.Substring(14).Split('(')[0].Split('.')[0].Split(' ');
            string itemName = "";
            foreach (string split in splits) {
                if (split.Length == 0) continue;
                if (split == "that") break;
                if (itemName == "" && (split == "a" || split == "an")) continue;
                if (split[0].isDigit()) continue;
                itemName = itemName == "" ? split : itemName + " " + split;
            }
            if (pluralMap.ContainsKey(itemName)) itemName = pluralMap[itemName];
            if (!StorageManager.itemExists(itemName) && itemName.Length > 0) {
                string singular = itemName.Substring(0, itemName.Length - 1);
                if (StorageManager.itemExists(singular)) {
                    itemName = singular;
                }
            }
            return itemName;
        }

        public static Tuple<string, int> preprocessItem(string item) {
            int count = 1;
            if (item == "nothing") return new Tuple<string, int>("nothing", 0);
            string itemName = "";
            string[] split = item.Split(' ');
            for (int i = 0; i < split.Length; i++) {
                if (split[i].Length == 0) continue;
                if ((split[i] == "a" || split[i] == "an") && itemName == "") continue;
                if (split[i][0].isDigit()) {
                    if (int.TryParse(split[i], out count)) {
                        continue;
                    }
                }
                itemName = itemName == "" ? split[i] : itemName + " " + split[i];
            }
            if (count > 1) {
                if (pluralMap.ContainsKey(itemName)) {
                    itemName = pluralMap[itemName];
                } else {
                    itemName = getSingularItem(itemName);
                }
            }
            return new Tuple<string, int>(itemName, count);
        }

        private static string getSingularItem(string item) {
            item = item.Trim().ToLower();
            foreach (KeyValuePair<string, string> kvp in Constants.pluralWords) {
                if (item.Contains(kvp.Key)) {
                    return item.Replace(kvp.Key, kvp.Value);
                }
            }
            foreach (KeyValuePair<string, string> kvp in Constants.pluralSuffixes) {
                if (item.EndsWith(kvp.Key)) {
                    return item.Substring(0, item.Length - kvp.Key.Length) + kvp.Value;
                }
            }
            if (StorageManager.getItem(item) == null) {
                string[] words = item.Split(' ');
                if (words.Length > 1) {
                    string word = getSingularItem(words[0]);
                    if (word != words[0]) {
                        return word + item.Substring(words[0].Length, item.Length - words[0].Length);
                    }
                }
                Console.WriteLine(String.Format("Warning, could not find singular form of plural item: {0}", item));
            }
            return item;
        }


        public static Dictionary<string, List<string>> globalMessages = new Dictionary<string, List<string>>();
        public static void ParseLootMessages(Hunt h, Dictionary<string, List<string>> newDrops, List<Tuple<Creature, List<Tuple<Item, int>>>> newItems, bool commit = true, bool switchHunt = false, bool addEverything = false) {
            lock (HuntManager.hunts) {

                SQLiteTransaction transaction = null;
                if (commit) {
                    transaction = LootDatabaseManager.BeginTransaction();
                }

                int stamp = MainForm.getDayStamp();
                Dictionary<string, List<string>> itemDrops = addEverything ? new Dictionary<string, List<string>>() : globalMessages;
                // now the big one: parse the log messages and check the dropped items
                foreach (KeyValuePair<string, List<string>> kvp in newDrops) {
                    string t = kvp.Key;
                    List<string> itemList = kvp.Value;
                    if (!itemDrops.ContainsKey(t)) {
                        itemDrops.Add(t, new List<string>());
                    }
                    if (itemList.Count > itemDrops[t].Count) {
                        int hour = int.Parse(t.Substring(0, 2));
                        int minute = int.Parse(t.Substring(3, 2));
                        foreach (string message in itemList) {
                            if (!itemDrops[t].Contains(message)) {
                                // new log message, scan it for new items
                                Tuple<Creature, List<Tuple<Item, int>>> resultList = ParseLootMessage(message);
                                if (resultList == null) continue;

                                Creature cr = resultList.Item1;

                                if (switchHunt && commit) {
                                    foreach (Hunt potentialHunt in HuntManager.hunts) {
                                        if (potentialHunt.lootCreatures.Contains(cr.GetName().ToLower())) {
                                            if (potentialHunt.sideHunt) {
                                                h = potentialHunt;
                                                HuntManager.activeHunt = potentialHunt;
                                            } else if (potentialHunt.aggregateHunt && potentialHunt != h) {
                                                MainForm.addKillToHunt(potentialHunt, resultList, t, message, stamp, hour, minute, transaction);
                                            }
                                        }
                                    }
                                }

                                MainForm.addKillToHunt(h, resultList, t, message, stamp, hour, minute, transaction);
                                if (MainForm.fileWriter != null && SettingsManager.getSettingBool("AutomaticallyWriteLootToFile")) {
                                    MainForm.fileWriter.WriteLine(message);
                                    MainForm.fileWriter.Flush();
                                }

                                if (newItems != null) {
                                    newItems.Add(resultList);
                                }
                            } else {
                                itemDrops[t].Remove(message);
                            }
                        }
                        itemDrops[t] = itemList;
                    }
                }
                if (transaction != null) {
                    transaction.Commit();
                }
            }
        }

        public static Tuple<Creature, List<Tuple<Item, int>>> ParseLootMessage(string message) {
            if (message.Length <= 14) return null;
            string lootMessage = message.Substring(14);
            // split on : because the message is Loot of a x: a, b, c, d
            if (!lootMessage.Contains(':')) return null;
            string[] matches = lootMessage.Split(':');
            string creature = matches[0];
            // non-boss creatures start with 'a' (e.g. 'Loot of a wyvern'); remove the 'a'
            if (creature[0] == 'a') {
                creature = creature.Split(new char[] { ' ' }, 2)[1];
            }
            Creature cr = StorageManager.getCreature(creature.ToLower());
            if (cr == null) {
                Console.WriteLine(String.Format("Warning, creature {0} was not found in the database.", creature));
                return null;
            }
            // now parse the individual items, they are comma separated
            List<Tuple<Item, int>> itemList = new List<Tuple<Item, int>>();
            string[] items = matches[1].Split(',');
            foreach (string item in items) {
                // process the item to find out how much dropped and to convert to singular form (e.g. '4 small amethysts' => ('small amethyst', 4))
                Tuple<string, int> processedItem = Parser.preprocessItem(item);
                string itemName = processedItem.Item1.Trim();
                if (itemName == "nothing") continue;
                int itemCount = processedItem.Item2;
                Item it = StorageManager.getItem(itemName);
                if (it == null) {
                    Console.WriteLine(String.Format("Warning, item {0} was not found in the database.", itemName));
                    return null;
                } else {
                    itemList.Add(new Tuple<Item, int>(it, itemCount));
                }
            }
            return new Tuple<Creature, List<Tuple<Item, int>>>(cr, itemList);
        }
    }
}