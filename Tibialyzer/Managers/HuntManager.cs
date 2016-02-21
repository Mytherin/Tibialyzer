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
using System.Data.SQLite;
using System.Globalization;
using System.IO;

namespace Tibialyzer {
    public class HuntManager {

        public static Hunt activeHunt = null;
        public static List<Hunt> hunts = new List<Hunt>();

        public static void Initialize() {
            //"Name#DBTableID#Track#Time#Exp#SideHunt#AggregateHunt#ClearOnStartup#Creature#Creature#..."
            if (!SettingsManager.settingExists("Hunts")) {
                SettingsManager.setSetting("Hunts", new List<string>() { "New Hunt#True#0#0#False#True" });
            }
            hunts.Clear();
            int activeHuntIndex = 0, index = 0;
            List<int> dbTableIds = new List<int>();
            foreach (string str in SettingsManager.getSetting("Hunts")) {
                SQLiteDataReader reader;
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
                    if (SettingsManager.settingExists("ActiveHunt") && SettingsManager.getSettingString("ActiveHunt") == hunt.name)
                        activeHuntIndex = index;

                    refreshLootCreatures(hunt);

                    if (hunt.clearOnStartup) {
                        resetHunt(hunt);
                    }

                    // create the hunt table if it does not exist
                    LootDatabaseManager.CreateHuntTable(hunt);
                    // load the data for the hunt from the database
                    reader = LootDatabaseManager.GetHuntMessages(hunt);
                    while (reader.Read()) {
                        string message = reader["message"].ToString();
                        Tuple<Creature, List<Tuple<Item, int>>> resultList = Parser.ParseLootMessage(message);
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
            activeHunt = hunts[activeHuntIndex];
            MainForm.mainForm.InitializeHuntDisplay(activeHuntIndex);
        }

        public static void resetHunt(Hunt h) {
            lock (hunts) {
                h.loot.creatureLoot.Clear();
                h.loot.killCount.Clear();
                h.loot.logMessages.Clear();
                h.totalExp = 0;
                h.totalTime = 0;
            }
            LootDatabaseManager.DeleteHuntTable(h);
            LootDatabaseManager.CreateHuntTable(h);
            LootDatabaseManager.UpdateLoot();
        }

        public static bool resetHunt(string parameter) {
            lock (hunts) {
                foreach (Hunt h in hunts) {
                    if (h.name.ToLower() == parameter.ToLower()) {
                        resetHunt(h);
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<TibiaObject> refreshLootCreatures(Hunt h) {
            h.lootCreatures.Clear();
            string[] creatures = h.trackedCreatures.Split('\n');
            List<TibiaObject> creatureObjects = new List<TibiaObject>();
            foreach (string cr in creatures) {
                string name = cr.ToLower();
                Creature cc = StorageManager.getCreature(name);
                if (cc != null && !creatureObjects.Contains(cc)) {
                    creatureObjects.Add(cc);
                    h.lootCreatures.Add(name);
                } else if (cc == null) {
                    HuntingPlace hunt = StorageManager.getHunt(name);
                    if (hunt != null) {
                        foreach (int creatureid in hunt.creatures) {
                            cc = StorageManager.getCreature(creatureid);
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

        private static bool nameExists(string str) {
            foreach (Hunt h in hunts) {
                if (h.name == str) {
                    return true;
                }
            }
            return false;
        }


        public static void CreateNewHunt() {
            Hunt h = new Hunt();
            lock (hunts) {
                if (!nameExists("New Hunt")) {
                    h.name = "New Hunt";
                } else {
                    int index = 1;
                    while (nameExists("New Hunt " + index)) index++;
                    h.name = "New Hunt " + index;
                }

                h.dbtableid = 1;
                while (LootDatabaseManager.HuntTableExists(h)) {
                    h.dbtableid++;
                }
            }
            resetHunt(h);
            h.trackAllCreatures = true;
            h.trackedCreatures = "";
            hunts.Add(h);
        }

        public static void clearOldLog(Hunt h, int clearMinutes = 10) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            while (clearMinutes > 60) {
                hour--;
                clearMinutes -= 60;
            }
            if (minute >= clearMinutes) {
                minute -= clearMinutes;
            } else {
                hour--;
                minute = 60 + (minute - clearMinutes);
            }
            int stamp = TimestampManager.getDayStamp();
            while (hour < 0) {
                hour += 24;
                stamp--;
            }

            h.loot.creatureLoot.Clear();
            h.loot.killCount.Clear();
            h.loot.logMessages.Clear();
            h.totalExp = 0;
            h.totalTime = 0;
            HuntManager.SetHuntTime(h, clearMinutes);


            LootDatabaseManager.DeleteMessagesBefore(h, stamp, hour, minute);

            SQLiteDataReader reader = LootDatabaseManager.GetHuntMessages(h);
            Dictionary<string, List<string>> logMessages = new Dictionary<string, List<string>>();
            while (reader.Read()) {
                string line = reader["message"].ToString();
                if (line.Length < 15) continue;
                string t = line.Substring(0, 5);
                if (!(t[0].isDigit() && t[1].isDigit() && t[3].isDigit() && t[4].isDigit() && t[2] == ':')) continue; //not a valid timestamp
                if (!logMessages.ContainsKey(t)) logMessages.Add(t, new List<string>());
                logMessages[t].Add(line);
            }
            Parser.ParseLootMessages(h, logMessages, null, false, false, true);
            LootDatabaseManager.UpdateLoot();
        }

        public static void DeleteHunt(Hunt h) {
            lock (hunts) {
                hunts.Remove(h);
                if (h == activeHunt) {
                    activeHunt = hunts[0];
                }
            }
        }

        public static Hunt GetHunt(int index) {
            lock (hunts) {
                return index >= hunts.Count ? null : hunts[index];
            }
        }

        public static Hunt GetHunt(string search) {
            search = search.ToLower();
            lock(hunts) {
                foreach (Hunt h in hunts) {
                    if (h.name.ToLower() == search) {
                        return h;
                    }
                }
                foreach (Hunt h in hunts) {
                    if (h.name.ToLower().Contains(search)) {
                        return h;
                    }
                }
            }
            return null;
        }

        public static void SaveHunts() {
            List<string> huntStrings = new List<string>();
            lock (hunts) {
                foreach (Hunt hunt in hunts) {
                    if (hunt.temporary) continue;
                    huntStrings.Add(hunt.ToString());
                }
                SettingsManager.setSetting("Hunts", huntStrings);
                if (activeHunt != null) {
                    SettingsManager.setSetting("ActiveHunt", activeHunt.name);
                }
            }
        }

        public static void SetActiveHunt(Hunt h) {
            lock (hunts) {
                activeHunt = h;
            }
        }

        public static void SwitchHunt(string parameter) {
            lock (hunts) {
                foreach (Hunt h in hunts) {
                    if (h.name.ToLower().Contains(parameter)) {
                        activeHunt = h;
                        break;
                    }
                }
            }
        }

        public static Creature GetHighestKillCreature(Hunt h) {
            Creature cr = null;
            int kills = -1;
            lock (hunts) {
                foreach (KeyValuePair<Creature, int> kvp in activeHunt.loot.killCount) {
                    if (kvp.Value > kills && kvp.Key.skin != null) {
                        cr = kvp.Key;
                        kills = kvp.Value;
                    }
                }
            }
            return cr;
        }

        public static void deleteLogMessage(Hunt h, string logMessage) {
            string timeStamp = logMessage.Substring(0, 5);
            bool found = false;
            lock (hunts) {
                if (h.loot.logMessages.ContainsKey(timeStamp)) {
                    if (h.loot.logMessages[timeStamp].Contains(logMessage)) {
                        h.loot.logMessages[timeStamp].Remove(logMessage);
                        var logMessageItems = Parser.ParseLootMessage(logMessage);
                        Creature cr = logMessageItems.Item1;
                        if (h.loot.killCount.ContainsKey(cr)) {
                            h.loot.killCount[cr]--;
                            if (h.loot.killCount[cr] == 0) {
                                h.loot.killCount.Remove(cr);
                            }
                        }
                        foreach (Tuple<Item, int> tpl in logMessageItems.Item2) {
                            if (h.loot.creatureLoot[cr].ContainsKey(tpl.Item1)) {
                                h.loot.creatureLoot[cr][tpl.Item1] -= tpl.Item2;
                                if (h.loot.creatureLoot[cr][tpl.Item1] <= 0) {
                                    h.loot.creatureLoot[cr].Remove(tpl.Item1);
                                }
                            }
                        }
                        found = true;
                    }
                }
            }
            if (!found) return;
            LootDatabaseManager.DeleteMessage(h, logMessage, null);
            LootDatabaseManager.UpdateLoot();
        }

        public static void deleteCreatureFromLog(Creature cr) {
            lock (hunts) {
                if (activeHunt.loot.killCount.ContainsKey(cr)) {
                    activeHunt.loot.killCount.Remove(cr);
                }
                if (activeHunt.loot.creatureLoot.ContainsKey(cr)) {
                    activeHunt.loot.creatureLoot.Remove(cr);
                }
                using (var transaction = LootDatabaseManager.BeginTransaction()) {
                    foreach (KeyValuePair<string, List<string>> kvp in activeHunt.loot.logMessages) {
                        foreach (string msg in kvp.Value) {
                            if (Parser.ParseCreatureFromLootMessage(msg) == cr) {
                                LootDatabaseManager.DeleteMessage(activeHunt, msg, transaction);
                            }
                        }
                    }
                    transaction.Commit();
                }
            }
            LootDatabaseManager.UpdateLoot();
        }

        public static void deleteCreatureWithThreshold(int killThreshold) {
            List<Creature> deleteList = new List<Creature>();
            foreach (KeyValuePair<Creature, int> kvp in activeHunt.loot.killCount) {
                if (kvp.Value < killThreshold) deleteList.Add(kvp.Key);
            }
            foreach (Creature cr in deleteList) {
                deleteCreatureFromLog(cr);
            }
            LootDatabaseManager.UpdateLoot();
        }

        public static void AddSkin(Hunt h, string message, Creature cr, Item item, int count, string timestamp) {
            lock (hunts) {
                if (!activeHunt.loot.logMessages.ContainsKey(timestamp)) activeHunt.loot.logMessages.Add(timestamp, new List<string>());
                activeHunt.loot.logMessages[timestamp].Add(message);
                if (!activeHunt.loot.creatureLoot.ContainsKey(cr)) {
                    activeHunt.loot.creatureLoot.Add(cr, new Dictionary<Item, int>());
                }
                foreach (Item i in activeHunt.loot.creatureLoot[cr].Keys) {
                    if (i.id == cr.skin.dropitemid) {
                        activeHunt.loot.creatureLoot[cr][i] += count;
                        return;
                    }
                }
                activeHunt.loot.creatureLoot[cr].Add(item, count);
            }
        }


        public static void SaveLog(Hunt h, string logPath) {
            StreamWriter streamWriter = new StreamWriter(logPath);

            // we load the data from the database instead of from the stored dictionary so it is ordered properly
            SQLiteDataReader reader = LootDatabaseManager.GetHuntMessages(h);
            while (reader.Read()) {
                streamWriter.WriteLine(reader["message"].ToString());
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void LoadLog(Hunt h, string logPath) {
            resetHunt(h);
            StreamReader streamReader = new StreamReader(logPath);
            string line;
            Dictionary<string, List<string>> logMessages = new Dictionary<string, List<string>>();
            while ((line = streamReader.ReadLine()) != null) {
                if (line.Length < 15) continue;
                string t = line.Substring(0, 5);
                if (!(t[0].isDigit() && t[1].isDigit() && t[3].isDigit() && t[4].isDigit() && t[2] == ':')) continue; //not a valid timestamp
                if (!logMessages.ContainsKey(t)) logMessages.Add(t, new List<string>());
                logMessages[t].Add(line);
            }
            Parser.ParseLootMessages(h, logMessages, null, true, true);
            LootDatabaseManager.UpdateLoot();
        }

        public static void InsertSkin(Creature cr, int count = 1) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            int stamp = TimestampManager.getDayStamp();
            string timestamp = String.Format("{0}:{1}", (hour < 10 ? "0" + hour.ToString() : hour.ToString()), (minute < 10 ? "0" + minute.ToString() : minute.ToString()));
            Item item = StorageManager.getItem(cr.skin.dropitemid);
            if (item == null) return;
            string message = String.Format("{0} Loot of a {1}: {2} {3}", timestamp, cr.displayname.ToLower(), count, item.displayname.ToLower());
            Hunt h = HuntManager.activeHunt;
            LootDatabaseManager.InsertMessage(h, stamp, hour, minute, message);
            HuntManager.AddSkin(h, message, cr, item, count, timestamp);
            LootDatabaseManager.UpdateLoot();
        }

        public static void AddKillToHunt(Hunt h, Tuple<Creature, List<Tuple<Item, int>>> resultList, string t, string message, int stamp = 0, int hour = 0, int minute = 0, SQLiteTransaction transaction = null) {
            Creature cr = resultList.Item1;
            if (!h.loot.creatureLoot.ContainsKey(cr)) h.loot.creatureLoot.Add(cr, new Dictionary<Item, int>());
            foreach (Tuple<Item, int> tpl in resultList.Item2) {
                Item item = tpl.Item1;
                int count = tpl.Item2;
                if (!h.loot.creatureLoot[cr].ContainsKey(item)) h.loot.creatureLoot[cr].Add(item, count);
                else h.loot.creatureLoot[cr][item] += count;
            }
            if (!h.loot.killCount.ContainsKey(cr)) h.loot.killCount.Add(cr, 1);
            else h.loot.killCount[cr] += 1;

            if (!h.loot.logMessages.ContainsKey(t)) h.loot.logMessages.Add(t, new List<string>());
            h.loot.logMessages[t].Add(message);

            if (transaction != null) {
                LootDatabaseManager.InsertMessage(h, stamp, hour, minute, message);
            }
        }

        public static void SetHuntTime(Hunt h, int clearMinutes) {
            var expInformation = GlobalDataManager.GetTotalExperience(TimestampManager.getLatestTimes(clearMinutes));
            h.totalExp = expInformation.Item1;
            h.totalTime = expInformation.Item2 * 60;
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
}
