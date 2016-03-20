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
        private static List<Hunt> hunts = new List<Hunt>();

        public static void Initialize() {
            lock(hunts) {
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

                            hunt.AddKillToHunt(resultList, t, message);
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
        }

        public static IEnumerable<Hunt> IterateHunts() {
            lock(hunts) {
                foreach(Hunt h in hunts) {
                    yield return h;
                }
            }
            yield break;
        }

        public static int HuntCount() {
            return hunts.Count;
        }

        public static void resetHunt(Hunt h) {
            h.Reset();
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
            return h.RefreshLootCreatures();
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

            h.Reset(clearMinutes);
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
            lock (hunts) {
                foreach (Hunt h in hunts) {
                    if (string.Equals(h.name, search, StringComparison.OrdinalIgnoreCase)) {
                        return h;
                    }
                }
                foreach (Hunt h in hunts) {
                    if (h.name.Contains(search, StringComparison.OrdinalIgnoreCase)) {
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
                    if (h.name.Contains(parameter, StringComparison.OrdinalIgnoreCase)) {
                        activeHunt = h;
                        break;
                    }
                }
            }
        }

        public static Creature GetHighestKillCreature(Hunt h) {
            return h.GetHighestKillCreature();
        }

        public static void deleteLogMessage(Hunt h, string logMessage) {
            bool found = h.DeleteLogMessage(logMessage);
            if (!found) return;
            LootDatabaseManager.DeleteMessage(h, logMessage, null);
            LootDatabaseManager.UpdateLoot();
        }

        public static void deleteCreatureFromLog(Creature cr) {
            activeHunt.DeleteCreature(cr);
            LootDatabaseManager.UpdateLoot();
        }

        public static void deleteCreatureWithThreshold(int killThreshold) {
            activeHunt.DeleteCreatureWithThreshold(killThreshold);
            LootDatabaseManager.UpdateLoot();
        }

        public static void AddSkin(Hunt h, string message, Creature cr, Item item, int count, string timestamp) {
            h.AddSkin(message, cr, item, count, timestamp);
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
            h.AddKillToHunt(resultList, t, message);
            if (transaction != null) {
                LootDatabaseManager.InsertMessage(h, stamp, hour, minute, message);
            }
        }

        public static Hunt CheckTrackedHunts(Hunt h, Tuple<Creature, List<Tuple<Item, int>>> resultList, string t, string message, int stamp = 0, int hour = 0, int minute = 0, SQLiteTransaction transaction = null) {
            lock (hunts) {
                foreach (Hunt potentialHunt in hunts) {
                    if (potentialHunt.ContainsLootCreature(resultList.Item1)) {
                        if (potentialHunt.sideHunt) {
                            h = potentialHunt;
                            activeHunt = potentialHunt;
                        } else if (potentialHunt.aggregateHunt && potentialHunt != h) {
                            AddKillToHunt(potentialHunt, resultList, t, message, stamp, hour, minute, transaction);
                        }
                    }
                }
            }
            return h;
        }

        public static void AddUsedItems(Hunt hunt, Dictionary<string, Dictionary<string, HashSet<int>>> usedItems) {
            bool newValues = hunt.AddUsedItems(usedItems);
            if (newValues) {
                GlobalDataManager.UpdateUsedItems();
            }
        }

        public static List<Tuple<Item, int>> GetUsedItems(Hunt hunt) {
            return hunt.GetUsedItems();
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
        private Loot loot = new Loot();
        private List<string> lootCreatures = new List<string>();
        private Dictionary<Item, OrderedHashSetCollection> usedItems = new Dictionary<Item, OrderedHashSetCollection>();
        private object huntLock = new object();

        public string GetTableName() {
            return "LootMessageTable" + dbtableid.ToString();
        }

        public override string ToString() {
            return name + "#" + dbtableid.ToString() + "#" + trackAllCreatures.ToString() + "#" + totalTime.ToString(CultureInfo.InvariantCulture) + "#" + totalExp.ToString() + "#" + sideHunt.ToString() + "#" + aggregateHunt.ToString() + "#" + clearOnStartup.ToString() + "#" + trackedCreatures.Replace("\n", "#");
        }

        public bool ContainsLootCreature(Creature cr) {
            lock (huntLock) {
                return lootCreatures.Contains(cr.GetName(), StringComparer.OrdinalIgnoreCase);
            }
        }

        public bool AddUsedItems(Dictionary<string, Dictionary<string, HashSet<int>>> newItems) {
            bool newValues = false;
            lock (huntLock) {
                foreach (var val in newItems) {
                    Item item = StorageManager.getItem(val.Key);
                    if (item == null) continue;
                    if (!this.usedItems.ContainsKey(item)) {
                        this.usedItems.Add(item, new OrderedHashSetCollection());
                    }
                    var collection = this.usedItems[item];

                    int currentCount = collection.GetItemCount();
                    collection.UpdateHashSet(val.Value);
                    if (collection.GetItemCount() > currentCount) {
                        newValues = true;
                    }
                }
            }
            return newValues;
        }

        public List<Tuple<Item, int>> GetUsedItems() {
            lock (huntLock) {
                List<Tuple<Item, int>> items = new List<Tuple<Item, int>>();
                foreach (var val in usedItems) {
                    int count = val.Value.GetItemCount();
                    if (count > 1) {
                        items.Add(new Tuple<Item, int>(val.Key, count));
                    }
                }
                return items;
            }
        }

        public void AddKillToHunt(Tuple<Creature, List<Tuple<Item, int>>> resultList, string t, string message) {
            lock (huntLock) {
                Creature cr = resultList.Item1;
                if (!this.loot.creatureLoot.ContainsKey(cr)) this.loot.creatureLoot.Add(cr, new Dictionary<Item, int>());
                foreach (Tuple<Item, int> tpl in resultList.Item2) {
                    Item item = tpl.Item1;
                    int count = tpl.Item2;
                    if (!this.loot.creatureLoot[cr].ContainsKey(item)) this.loot.creatureLoot[cr].Add(item, count);
                    else this.loot.creatureLoot[cr][item] += count;
                }
                if (!this.loot.killCount.ContainsKey(cr)) this.loot.killCount.Add(cr, 1);
                else this.loot.killCount[cr] += 1;

                if (!this.loot.logMessages.ContainsKey(t)) this.loot.logMessages.Add(t, new List<string>());
                this.loot.logMessages[t].Add(message);
            }
        }

        public void AddSkin(string message, Creature cr, Item item, int count, string timestamp) {
            lock (huntLock) {
                if (!this.loot.logMessages.ContainsKey(timestamp)) this.loot.logMessages.Add(timestamp, new List<string>());
                this.loot.logMessages[timestamp].Add(message);
                if (!this.loot.creatureLoot.ContainsKey(cr)) {
                    this.loot.creatureLoot.Add(cr, new Dictionary<Item, int>());
                }
                foreach (Item i in this.loot.creatureLoot[cr].Keys) {
                    if (i.id == cr.skin.dropitemid) {
                        this.loot.creatureLoot[cr][i] += count;
                        return;
                    }
                }
                this.loot.creatureLoot[cr].Add(item, count);
            }
        }

        public IEnumerable<Creature> IterateCreatures() {
            lock(huntLock) {
                foreach(Creature cr in loot.killCount.Keys) {
                    yield return cr;
                }
            }
            yield break;
        }

        public IEnumerable<KeyValuePair<Creature, Dictionary<Item, int>>> IterateLoot() {
            lock(huntLock) {
                foreach(var value in loot.creatureLoot) {
                    yield return value;
                }
            }
            yield break;
        }

        public Dictionary<Creature, int> GetCreatureKills() {
            Dictionary<Creature, int> creatureKills = new Dictionary<Creature, int>();
            lock(huntLock) {
                foreach(KeyValuePair<Creature, int> kvp in loot.killCount) {
                    creatureKills.Add(kvp.Key, kvp.Value);
                }
            }
            return creatureKills;
        }

        public Dictionary<Creature, int> GetCreatureKills(Creature filter) {
            return GetCreatureKills(new List<Creature> { filter });
        }

        public Dictionary<Creature, int> GetCreatureKills(List<Creature> filterCreatures) {
            Dictionary<Creature, int> creatureKills = new Dictionary<Creature, int>();
            lock(huntLock) {
                foreach (Creature cr in filterCreatures) {
                    if (!this.loot.killCount.ContainsKey(cr)) continue;

                    creatureKills.Add(cr, this.loot.killCount[cr]);
                }
            }
            return creatureKills;
        }

        public IEnumerable<string> IterateLogMessages() {
            lock(huntLock) {
                List<string> timestamps = this.loot.logMessages.Keys.OrderByDescending(o => o).ToList();
                foreach (string t in timestamps) {
                    List<string> strings = this.loot.logMessages[t].ToList();
                    strings.Reverse();
                    foreach (string str in strings) {
                        yield return str;
                    }
                }
            }
        }

        public void DeleteCreature(Creature cr) {
            lock(huntLock) {
                if (this.loot.killCount.ContainsKey(cr)) {
                    this.loot.killCount.Remove(cr);
                }
                if (this.loot.creatureLoot.ContainsKey(cr)) {
                    this.loot.creatureLoot.Remove(cr);
                }
                using (var transaction = LootDatabaseManager.BeginTransaction()) {
                    foreach (KeyValuePair<string, List<string>> kvp in this.loot.logMessages) {
                        foreach (string msg in kvp.Value) {
                            if (Parser.ParseCreatureFromLootMessage(msg) == cr) {
                                LootDatabaseManager.DeleteMessage(this, msg, transaction);
                            }
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        public void DeleteCreatureWithThreshold(int killThreshold) {
            List<Creature> deleteList = new List<Creature>();
            lock (huntLock) {
                foreach (KeyValuePair<Creature, int> kvp in this.loot.killCount) {
                    if (kvp.Value < killThreshold) deleteList.Add(kvp.Key);
                }
            }
            foreach (Creature cr in deleteList) {
                this.DeleteCreature(cr);
            }
        }

        public Creature GetHighestKillCreature() {
            Creature cr = null;
            int kills = -1;
            lock (huntLock) {
                foreach (KeyValuePair<Creature, int> kvp in this.loot.killCount) {
                    if (kvp.Value > kills && kvp.Key.skin != null) {
                        cr = kvp.Key;
                        kills = kvp.Value;
                    }
                }
            }
            return cr;
        }

        public bool DeleteLogMessage(string logMessage) {
            string timestamp = logMessage.Substring(0, 5);
            lock (huntLock) {
                if (this.loot.logMessages.ContainsKey(timestamp)) {
                    if (this.loot.logMessages[timestamp].Contains(logMessage)) {
                        this.loot.logMessages[timestamp].Remove(logMessage);
                        var logMessageItems = Parser.ParseLootMessage(logMessage);
                        Creature cr = logMessageItems.Item1;
                        if (this.loot.killCount.ContainsKey(cr)) {
                            this.loot.killCount[cr]--;
                            if (this.loot.killCount[cr] == 0) {
                                this.loot.killCount.Remove(cr);
                            }
                        }
                        foreach (Tuple<Item, int> tpl in logMessageItems.Item2) {
                            if (this.loot.creatureLoot[cr].ContainsKey(tpl.Item1)) {
                                this.loot.creatureLoot[cr][tpl.Item1] -= tpl.Item2;
                                if (this.loot.creatureLoot[cr][tpl.Item1] <= 0) {
                                    this.loot.creatureLoot[cr].Remove(tpl.Item1);
                                }
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public List<TibiaObject> RefreshLootCreatures() {
            List<TibiaObject> creatureObjects = new List<TibiaObject>();
            lock (huntLock) {
                this.lootCreatures.Clear();
                string[] creatures = this.trackedCreatures.Split('\n');
                foreach (string cr in creatures) {
                    string name = cr.ToLower();
                    Creature cc = StorageManager.getCreature(name);
                    if (cc != null && !creatureObjects.Contains(cc)) {
                        creatureObjects.Add(cc);
                        this.lootCreatures.Add(name);
                    } else if (cc == null) {
                        HuntingPlace hunt = StorageManager.getHunt(name);
                        if (hunt != null) {
                            foreach (int creatureid in hunt.creatures) {
                                cc = StorageManager.getCreature(creatureid);
                                if (cc != null && !creatureObjects.Any(item => item.GetName() == name)) {
                                    creatureObjects.Add(cc);
                                    this.lootCreatures.Add(cc.GetName());
                                }
                            }
                        }
                    }
                }
            }
            return creatureObjects;
        }

        public List<Creature> GetTrackedCreatures() {
            List<Creature> trackedCreatures = new List<Creature>();
            lock(huntLock) {
                foreach (string creature in this.lootCreatures) {
                    Creature cr = StorageManager.getCreature(creature.ToLower());
                    if (cr != null) {
                        trackedCreatures.Add(cr);
                    }
                }
            }
            return trackedCreatures;
        }

        public void Reset(int clearMinutes = 0) {
            lock(huntLock) {
                this.loot.creatureLoot.Clear();
                this.loot.killCount.Clear();
                this.loot.logMessages.Clear();
                if (clearMinutes == 0) {
                    this.usedItems.Clear();
                } else {
                    foreach(var item in usedItems.Keys.ToList()) {
                        usedItems[item].ClearItems(clearMinutes);
                        if (usedItems[item].GetItemCount() == 0) {
                            usedItems.Remove(item);
                        }
                    }
                }
                this.totalExp = 0;
                this.totalTime = 0;
            }
        }
    }

    class OrderedHashSetCollection {
        public List<Tuple<string, HashSet<int>>> hashSets = new List<Tuple<string, HashSet<int>>>();
        int baseCount = 0;

        public void UpdateHashSet(Dictionary<string, HashSet<int>> values) {
            lock (hashSets) {
                foreach (var val in values.OrderBy(o => o.Key)) {
                    var hashSet = hashSets.Find(o => o.Item1 == val.Key);
                    if (hashSet == null) {
                        hashSet = new Tuple<string, HashSet<int>>(val.Key, val.Value);
                        hashSets.Add(hashSet);
                    } else {
                        int count = hashSet.Item2.Count;
                        hashSet.Item2.UnionWith(val.Value);
                        if (count == hashSet.Item2.Count) // no new items
                            continue;
                    }
                    int index = hashSets.IndexOf(hashSet);
                    if (index <= 0) {
                        continue;
                    } else {
                        Tuple<string, HashSet<int>> previous = null;
                        while((previous == null || previous.Item2.Count == 0) && index >= 0) {
                            index--;
                            previous = hashSets[index];
                        }
                        foreach(int itemCount in previous.Item2) {
                            if (hashSet.Item2.Contains(itemCount)) {
                                hashSet.Item2.Remove(itemCount);
                            }
                        }
                    }
                }
            }
        }

        public void ClearItems(int clearMinutes) {
            string currentTime = TimestampManager.getCurrentTime();
            lock (hashSets) {
                for (int i = 0; i < hashSets.Count; i++) {
                    if (TimestampManager.Distance(currentTime, hashSets[i].Item1) > clearMinutes) {
                        hashSets.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public int GetItemCount() {
            int count = baseCount;
            lock (hashSets) {
                foreach (var tpl in hashSets) {
                    count += tpl.Item2.Count;
                }
            }
            return count;
        }
    }
}
