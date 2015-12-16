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

        //based on http://www.codeproject.com/Articles/716227/Csharp-How-to-Scan-a-Process-Memory
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;
        public struct MEMORY_BASIC_INFORMATION {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;   // size of the region allocated by the program
            public int State;   // check if allocated (MEM_COMMIT)
            public int Protect; // page protection (must be PAGE_READWRITE)
            public int lType;
        }
        public struct SYSTEM_INFO {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }
        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        public class ReadMemoryResults {
            public Dictionary<string, List<string>> itemDrops = new Dictionary<string, List<string>>();
            public Dictionary<string, int> exp = new Dictionary<string, int>();
            public Dictionary<string, Dictionary<string, int>> damageDealt = new Dictionary<string, Dictionary<string, int>>();
            public Dictionary<string, List<Tuple<string, string>>> commands = new Dictionary<string, List<Tuple<string, string>>>();
            public Dictionary<string, List<Tuple<string, string>>> urls = new Dictionary<string, List<Tuple<string, string>>>();
            public List<string> newAdvances = new List<string>();
        }

        public class ParseMemoryResults {
            public Dictionary<string, int> damagePerSecond = new Dictionary<string, int>();
            public List<string> newCommands = new List<string>();
            public List<Tuple<Creature, Item>> newItems = new List<Tuple<Creature, Item>>();
            public int expPerHour = 0;
        }

        private Dictionary<string, HashSet<string>> totalLooks = new Dictionary<string, HashSet<string>>();
        private HashSet<string> levelAdvances = new HashSet<string>();
        private ReadMemoryResults ReadMemory() {
            ReadMemoryResults results = null;
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;
            
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;
            Process[] processes = Process.GetProcessesByName("Tibia");
            if (processes.Length == 0) {
                // Tibia process could not be found, wait for a bit and return
                Thread.Sleep(250);
                return null;
            }
            Process process = processes[0];
            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            try {
                results = new ReadMemoryResults();
                while (proc_min_address_l < proc_max_address_l) {
                    proc_min_address = new IntPtr(proc_min_address_l);
                    // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                    VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                    // check if this memory chunk is accessible
                    if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT) {
                        byte[] buffer = new byte[mem_basic_info.RegionSize];

                        // read everything in the buffer above
                        ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);
                        // scan the memory for strings that start with timestamps and end with the null terminator ('\0')
                        List<string> strings = FindTimestamps(buffer); 
                        if (strings.Count > 0) {
                            // if any timestamp strings were found, scan the chunk for any messages
                            SearchChunk(strings, results); 
                        }
                    }

                    // move to the next memory chunk
                    proc_min_address_l += mem_basic_info.RegionSize;
                }
            } catch {
                return null;
            }
            process.Dispose();
            return results;
        }

        public static List<string> FindTimestamps(byte[] array) {
            int index = 0;
            List<string> strings = new List<string>();
            // scan the memory for "timestamp values"
            // i.e. values that are like "xx:xx" where x = a number
            // we consider timestamps the "starting point" of a string, and the null terminator the "ending point"
            int start = 0;
            for (int i = 0; i < array.Length; i++) {
                if (index == 6) {
                    if (array[i] == 0) {
                        strings.Add(System.Text.Encoding.UTF8.GetString(array, start, (i - start)));
                        index = 0;
                    }
                } else if (index == 5) {
                    if (array[i] == ' ') {
                        index++;
                    } else {
                        index = 0;
                    }
                } else if (array[i] > 47 && array[i] < 59) {
                    if (index == 2) {
                        if (array[i] == 58) {
                            start = i - 2;
                            index++;
                        } else {
                            index = 0;
                        }
                    } else if (array[i] < 58) {
                        index++;
                    } else {
                        index = 0;
                    }
                } else {
                    index = 0;
                }
            }
            return strings;
        }

        private Tuple<int, int> parseTimeStamp(string stamp) {
            if (stamp.Length < 5) return null;
            try {
                int hour = int.Parse(stamp.Substring(0, 2));
                int minute = int.Parse(stamp.Substring(3, 2));
                return new Tuple<int, int>(hour, minute);
            } catch {
                return null;
            }
        }

        private int createStamp() {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            return getStamp(hour, minute);

        }

        private int getStamp(int hour, int minute) { return hour * 60 + minute; }

        private List<string> getLatestTimes(int count, int ignoreStamp = -1) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            List<string> stamps = new List<string>();
            for (int i = 0; i < count; i++) {
                if (getStamp(hour, minute) == ignoreStamp) return stamps;

                stamps.Add(string.Format("{0}:{1}", (hour < 10 ? "0" + hour.ToString() : hour.ToString()), (minute < 10 ? "0" + minute.ToString() : minute.ToString())));

                if (minute == 0) {
                    hour = hour > 0 ? hour - 1 : 23;
                    minute = 59;
                } else {
                    minute = minute - 1;
                }
            }
            return stamps;
        }

        bool isDigit(char c) {
            return
                c == '0' ||
                c == '1' ||
                c == '2' ||
                c == '3' ||
                c == '4' ||
                c == '5' ||
                c == '6' ||
                c == '7' ||
                c == '8' ||
                c == '9';
        }
        
        int getCount(string item) {
            int begin = -1, end = -1;
            for (int i = 0; i < item.Length; i++) {
                if (isDigit(item[i])) {
                    if (begin < 0) begin = i;
                } else if (begin >= 0) {
                    end = i;
                    break;
                }
            }
            if (begin >= 0) {
                return int.Parse(item.Substring(begin, end - begin));
            }
            return 1;
        }

        string parseLookItem(string logMessage) {
            string[] splits = logMessage.Substring(14).Split('(')[0].Split('.')[0].Split(' ');
            string itemName = "";
            foreach (string split in splits) {
                if (split.Length == 0) continue;
                if (split == "a" || split == "an") continue;
                if (isDigit(split[0])) continue;
                itemName = itemName == "" ? split : itemName + " " + split;
            }
            return itemName;
        }

        private Dictionary<string, string> pluralMap = new Dictionary<string, string>();
        Tuple<string, int> preprocessItem(string item) {
            int count = 1;
            if (item == "nothing") return new Tuple<string, int>("nothing", 0);
            if (item.Contains("coin")) {
                string coinName = "gold coin";
                count = -1;
                if (item.Contains("platinum coin")) {
                    count = getCount(item) * 100;
                } else if (item.Contains("gold coin")) {
                    count = getCount(item);
                } else if (item.Contains("crystal coin")) {
                    count = getCount(item) * 10000;
                }
                if (count > 0) return new Tuple<string, int>(coinName, count);
            }
            string itemName = "";
            string[] split = item.Split(' ');
            for (int i = split.Length - 1; i >= 0; i--) {
                if (split[i].Length == 0) continue;
                if (split[i] == "a" || split[i] == "an") {
                    break;
                } else if (isDigit(split[i][0])) {
                    if (int.TryParse(split[i], out count)) {
                        break;
                    }
                } else {
                    itemName = itemName == "" ? split[i] : split[i] + " " + itemName;
                }
            }

            if (count > 1) {
                if (pluralMap.ContainsKey(itemName)) {
                    itemName = pluralMap[itemName];
                } else if (itemName[itemName.Length - 1] == 's') {
                    itemName = itemName.Substring(0, itemName.Length - 1);
                } else {
                    Console.WriteLine(String.Format("Warning, Item {0} has a count of {1}, but was not in plural map and does not end with the letter s.", itemName, count));
                }
            }
            return new Tuple<string, int>(itemName, count);
        }
        private List<int> getLatestStamps(int count, int ignoreStamp = -1) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            List<int> stamps = new List<int>();
            for (int i = 0; i < count; i++) {
                int stamp = getStamp(hour, minute);
                stamps.Add(stamp);
                if (stamp == ignoreStamp) return stamps;

                hour = hour > 0 ? hour - 1 : 23;
                minute = minute > 0 ? minute - 1 : 59;

            }
            return stamps;
        }

        private int getDayStamp() {
            var t = DateTime.Now;
            return t.Year * 400 + t.Month * 40 + t.Day;
        }

        private void deleteCreatureFromLog(Creature cr) {
            if (activeHunt.loot.killCount.ContainsKey(cr)) {
                activeHunt.loot.killCount.Remove(cr);
            }
            if (activeHunt.loot.creatureLoot.ContainsKey(cr)) {
                activeHunt.loot.creatureLoot.Remove(cr);
            }
            using (var transaction = conn.BeginTransaction()) {
                SQLiteCommand command;
                foreach (KeyValuePair<string, List<string>> kvp in activeHunt.loot.logMessages) {
                    foreach (string msg in kvp.Value) {
                        if (ParseCreatureFromLootMessage(msg) == cr) {
                            command = new SQLiteCommand(String.Format("DELETE FROM \"{0}\" WHERE message=\"{1}\"", activeHunt.name.ToLower(), msg.Replace("\"", "\\\"")), conn, transaction);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                transaction.Commit();
            }
        }

        void resetHunt(Hunt h) {
            h.loot.creatureLoot.Clear();
            h.loot.killCount.Clear();
            h.loot.logMessages.Clear();
            string huntTable = h.name.ToLower().Replace("\"", "\\\"");
            SQLiteCommand comm = new SQLiteCommand(String.Format("DROP TABLE \"{0}\";", huntTable), conn);
            comm.ExecuteNonQuery();
            comm = new SQLiteCommand(String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"(day INTEGER, hour INTEGER, minute INTEGER, message STRING);", huntTable), conn);
            comm.ExecuteNonQuery();
        }

        void saveLog(string logPath) {
            StreamWriter streamWriter = new StreamWriter(logPath);

            // we load the data from the database instead of from the stored dictionary so it is ordered properly
            SQLiteCommand comm = new SQLiteCommand("SELECT message FROM \"{0}\"", conn);
            SQLiteDataReader reader = comm.ExecuteReader();
            while (reader.Read()) {
                streamWriter.WriteLine(reader["message"].ToString());
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        void loadLog(string logPath) {
            resetHunt(activeHunt);
            StreamReader streamReader = new StreamReader(logPath);
            string line;
            Dictionary<string, List<string>> logMessages = new Dictionary<string, List<string>>();
            while ((line = streamReader.ReadLine()) != null) {
                if (line.Length < 15) continue;
                string t = line.Substring(0, 5);
                if (!(isDigit(t[0]) && isDigit(t[1]) && isDigit(t[3]) && isDigit(t[4]) && t[2] == ':')) continue; //not a valid timestamp
                if (!logMessages.ContainsKey(t)) logMessages.Add(t, new List<string>());
                logMessages[t].Add(line);
            }
            ParseLootMessages(activeHunt, logMessages, null);
        }

        void setGoldRatio(double ratio) {
            using (var transaction = conn.BeginTransaction()) {
                SQLiteCommand command;
                foreach (Item item in itemNameMap.Values) {
                    double goldratio = Math.Max(item.actual_value, item.vendor_value) / item.capacity;
                    item.discard = goldratio < ratio;
                    command = new SQLiteCommand(String.Format("UPDATE Items SET discard={0} WHERE id={1}", item.discard ? 1 : 0, item.id), conn);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        void setConvertRatio(double ratio, bool stackable) {
            using (var transaction = conn.BeginTransaction()) {
                SQLiteCommand command;
                foreach (Item item in itemNameMap.Values) {
                    if (item.stackable == stackable) {
                        double goldratio = Math.Max(item.actual_value, item.vendor_value) / item.capacity;
                        item.convert_to_gold = goldratio < ratio;
                        command = new SQLiteCommand(String.Format("UPDATE Items SET convert_to_gold={0} WHERE id={1}", item.convert_to_gold ? 1 : 0, item.id), conn);
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
        }

        void setItemDiscard(Item item, bool discard) {
            item.discard = discard;
            SQLiteCommand command = new SQLiteCommand(String.Format("UPDATE Items SET discard={0} WHERE id={1}", item.discard ? 1 : 0, item.id), conn);
            command.ExecuteNonQuery();
        }

        void setItemConvert(Item item, bool convert) {
            item.convert_to_gold = convert;
            SQLiteCommand command = new SQLiteCommand(String.Format("UPDATE Items SET convert_to_gold={0} WHERE id={1}", item.convert_to_gold ? 1 : 0, item.id), conn);
            command.ExecuteNonQuery();
        }

        void setItemValue(Item item, int value) {
            item.actual_value = value;
            SQLiteCommand command = new SQLiteCommand(String.Format("UPDATE Items SET actual_value={0} WHERE id={1}", value, item.id), conn);
            command.ExecuteNonQuery();
        }

        List<Tuple<string, string>> getRecentCommands(int type, int max_entries = 15) {
            List<string> times = getLatestTimes(5);
            times.Reverse();

            Dictionary<string, List<Tuple<string, string>>> dict = type == 0 ? totalCommands : totalURLs;

            List<Tuple<string, string>> results = new List<Tuple<string, string>>();
            foreach (string t in times) {
                if (dict.ContainsKey(t)) {
                    foreach (Tuple<string, string> tpl in dict[t]) {
                        if (tpl.Item2.ToLower().Contains("recent") || tpl.Item2.ToLower().Contains("last")) continue;
                        results.Add(tpl);
                        if (results.Count >= max_entries) return results;
                    }
                }
            }
            return results;
        }

        private void deleteCreatureWithThreshold(int killThreshold) {
            List<Creature> deleteList = new List<Creature>();
            foreach (KeyValuePair<Creature, int> kvp in activeHunt.loot.killCount) {
                if (kvp.Value < killThreshold) deleteList.Add(kvp.Key);
            }
            foreach (Creature cr in deleteList) {
                deleteCreatureFromLog(cr);
            }
        }

        private void insertSkin(Creature cr) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            int stamp = getDayStamp();
            string timestamp = String.Format("{0}:{1}", (hour < 10 ? "0" + hour.ToString() : hour.ToString()), (minute < 10 ? "0" + minute.ToString() : minute.ToString()));
            string itemName = cr.skin.drop_item.name;
            string message = String.Format("{0} Loot of a {1}: {2}", timestamp, cr.name.ToLower(), itemName.ToLower());
            SQLiteCommand command = new SQLiteCommand(String.Format("INSERT INTO \"{4}\" VALUES({0}, {1}, {2}, \"{3}\");", stamp, hour, minute, message.Replace("\"", "\\\""), activeHunt.name.ToLower()), conn);
            command.ExecuteNonQuery();
        }

        private void ParseLootMessages(Hunt h, Dictionary<string, List<string>> newDrops, List<Tuple<Creature, Item>> newItems) {
            int stamp = getDayStamp();
            Dictionary<string, List<string>> itemDrops = h.loot.logMessages;
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
                    List<string> newMessages = new List<string>();
                    foreach (string message in itemList) {
                        if (!itemDrops[t].Contains(message)) {
                            // new log message, scan it for new items
                            Tuple<Creature, List<Tuple<Item, int>>> resultList = ParseLootMessage(message);
                            if (resultList == null) continue;

                            newMessages.Add(message);
                            Creature cr = resultList.Item1;
                            if (!h.loot.creatureLoot.ContainsKey(cr)) h.loot.creatureLoot.Add(cr, new Dictionary<Item, int>());
                            foreach (Tuple<Item, int> tpl in resultList.Item2) {
                                Item item = tpl.Item1;
                                int count = tpl.Item2;
                                if (newItems != null) newItems.Add(new Tuple<Creature, Item>(cr, item));
                                if (!h.loot.creatureLoot[cr].ContainsKey(item)) h.loot.creatureLoot[cr].Add(item, count);
                                else h.loot.creatureLoot[cr][item] += count;
                            }

                            if (!h.loot.killCount.ContainsKey(cr)) h.loot.killCount.Add(cr, 1);
                            else h.loot.killCount[cr] += 1;
                        } else {
                            itemDrops[t].Remove(message);
                        }
                    }
                    itemDrops[t] = itemList;
                    using (var transaction = conn.BeginTransaction()) {
                        SQLiteCommand command;
                        foreach (string msg in newMessages) {
                            string query = String.Format("INSERT INTO \"{4}\" VALUES({0}, {1}, {2}, \"{3}\");", stamp, hour, minute, msg.Replace("\"", "\\\""), activeHunt.name.ToLower());
                            command = new SQLiteCommand(query, conn, transaction);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
            }
        }

        private Dictionary<string, List<string>> totalItemDrops = new Dictionary<string, List<string>>();
        private Dictionary<string, List<Tuple<string, string>>> totalCommands = new Dictionary<string, List<Tuple<string, string>>>();
        private Dictionary<string, Dictionary<string, int>> totalDamageResults = new Dictionary<string, Dictionary<string, int>>();
        private Dictionary<string, List<Tuple<string, string>>> totalURLs = new Dictionary<string, List<Tuple<string, string>>>();
        private Dictionary<string, int> totalExperienceResults = new Dictionary<string, int>();
        private ParseMemoryResults ParseLogResults(ReadMemoryResults res) {
            if (res == null) return null;
            ParseMemoryResults o = new ParseMemoryResults();
            // first we add the new parsed damage logs to the totalDamageResults
            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in res.damageDealt) {
                string player = kvp.Key;
                Dictionary<string, int> playerDamage = kvp.Value;
                if (!totalDamageResults.ContainsKey(player)) totalDamageResults.Add(player, new Dictionary<string, int>());
                foreach (KeyValuePair<string, int> kvp2 in playerDamage) {
                    string timestamp = kvp2.Key;
                    int damage = kvp2.Value;
                    // if the damage for the given timestamp does not exist yet, add it
                    if (!totalDamageResults[player].ContainsKey(timestamp)) totalDamageResults[player].Add(timestamp, damage);
                    // if it does exist, select the biggest of the two
                    // the reason we select the biggest of the two is: 
                    // - if the timestamp is 'the current time', totalDamageResults may hold an old value, so we update it
                    // - if timestamp is old, a part of the log for the time could have already been removed (because the log was full)
                    //    so the 'new' damage is only part of the damage for this timestamp
                    else if (totalDamageResults[player][timestamp] < damage) totalDamageResults[player][timestamp] = damage;
                }
            }
            // now that we have updated the damage results, fill in the DPS meter, we use damage from the last 15 minutes for this
            List<string> times = getLatestTimes(15);
            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in totalDamageResults) {
                string player = kvp.Key;
                int damage = 0;
                foreach (string t in times) {
                    if (totalDamageResults[player].ContainsKey(t)) {
                        damage += totalDamageResults[player][t];
                    }
                }
                o.damagePerSecond.Add(player, damage);
            }

            // similar to damage, we keep a totalExperienceResults list
            // first update it with the new information
            foreach (KeyValuePair<string, int> kvp in res.exp) {
                string time = kvp.Key;
                int experience = kvp.Value;
                if (!totalExperienceResults.ContainsKey(time)) totalExperienceResults.Add(time, experience);
                else if (totalExperienceResults[time] < experience) totalExperienceResults[time] = experience;
            }
            // now compute the experience per hour
            // we use the same formula Tibia itself does so we get the same value
            // this formula is basically, take the experience in the last 15 minutes and multiply it by 4
            foreach (string t in times) {
                if (totalExperienceResults.ContainsKey(t)) o.expPerHour += totalExperienceResults[t];
            }
            o.expPerHour *= 4;

            // now parse any new commands given by users
            foreach (KeyValuePair<string, List<Tuple<string, string>>> kvp in res.commands) {
                string t = kvp.Key;
                List<Tuple<string, string>> currentCommands = kvp.Value;
                if (!totalCommands.ContainsKey(t)) totalCommands[t] = new List<Tuple<string, string>>();
                if (currentCommands.Count > totalCommands[t].Count) {
                    List<Tuple<string, string>> unseenCommands = new List<Tuple<string, string>>();
                    List<Tuple<string, string>> commandsList = totalCommands[t].ToArray().ToList(); //hopefully this copies the list
                    foreach (Tuple<string, string> command in currentCommands) {
                        if (!totalCommands[t].Contains(command)) { //this might not work, because it might just check the reference instead of the actual values: test
                            unseenCommands.Add(command);
                            string player = command.Item1;
                            string cmd = command.Item2;
                            if (settings["Names"].Contains(player)) {
                                o.newCommands.Add(cmd);
                            }
                        } else {
                            totalCommands[t].Remove(command);
                        }
                    }
                    commandsList.AddRange(unseenCommands);
                    totalCommands[t] = commandsList;
                }
            }

            // check new urls
            foreach (KeyValuePair<string, List<Tuple<string, string>>> kvp in res.urls) {
                string t = kvp.Key;
                List<Tuple<string, string>> currentURLs = kvp.Value;
                if (!totalURLs.ContainsKey(t)) {
                    totalURLs.Add(t, currentURLs);
                } else if (currentURLs.Count > totalURLs[t].Count) {
                    totalURLs[t] = currentURLs;
                }
            }


            ParseLootMessages(activeHunt, res.itemDrops, o.newItems);
            return o;
        }

        public Creature ParseCreatureFromLootMessage(string message) {
            string lootMessage = message.Substring(14);
            // split on : because the message is Loot of a x: a, b, c, d
            if (!lootMessage.Contains(':')) return null;
            string[] matches = lootMessage.Split(':');
            string creature = matches[0];
            // non-boss creatures start with 'a' (e.g. 'Loot of a wyvern'); remove the 'a'
            if (creature[0] == 'a') {
                creature = creature.Split(new char[] { ' ' }, 2)[1];
            }
            Creature cr = null;
            if (creatureNameMap.ContainsKey(creature.ToLower())) {
                return creatureNameMap[creature.ToLower()];
            } else {
                Console.WriteLine(String.Format("Warning, creature {0} was not found in the database.", creature));
                return null;
            }
        }

        public Tuple<Creature, List<Tuple<Item, int>>> ParseLootMessage(string message) {
            string lootMessage = message.Substring(14);
            // split on : because the message is Loot of a x: a, b, c, d
            if (!lootMessage.Contains(':')) return null;
            string[] matches = lootMessage.Split(':');
            string creature = matches[0];
            // non-boss creatures start with 'a' (e.g. 'Loot of a wyvern'); remove the 'a'
            if (creature[0] == 'a') {
                creature = creature.Split(new char[] { ' ' }, 2)[1];
            }
            Creature cr = null;
            if (creatureNameMap.ContainsKey(creature.ToLower())) {
                cr = creatureNameMap[creature.ToLower()];
            } else {
                Console.WriteLine(String.Format("Warning, creature {0} was not found in the database.", creature));
                return null;
            }
            // now parse the individual items, they are comma separated
            List<Tuple<Item, int>> itemList = new List<Tuple<Item, int>>();
            string[] items = matches[1].Split(',');
            foreach (string item in items) {
                // process the item to find out how much dropped and to convert to singular form (e.g. '4 small amethysts' => ('small amethyst', 4))
                Tuple<string, int> processedItem = preprocessItem(item);
                string itemName = processedItem.Item1;
                int itemCount = processedItem.Item2;
                if (!itemNameMap.ContainsKey(itemName)) {
                    Console.WriteLine(String.Format("Warning, item {0} was not found in the database.", itemName));
                } else {
                    Item it = itemNameMap[itemName];
                    itemList.Add(new Tuple<Item, int>(it, itemCount));
                }
            }
            return new Tuple<Creature, List<Tuple<Item, int>>>(cr, itemList);
        }

        private int ignoreStamp = 0;
        private void SearchChunk(List<string> chunk, ReadMemoryResults res) {
            List<int> stamps = getLatestStamps(5, ignoreStamp);
            foreach (string logMessage in chunk) {
                string t = logMessage.Substring(0, 5);
                int hour = int.Parse(logMessage.Substring(0, 2));
                int minute = int.Parse(logMessage.Substring(3, 2));
                if (!stamps.Contains(getStamp(hour, minute))) continue; // the log message is not recent, so we skip parsing it

                string message = logMessage.Substring(6); // message without timestamp
                if (logMessage.Length > 14 && logMessage.Substring(5, 9) == " You see ") {
                    // the message contains "you see", so it's a look message
                    if (!totalLooks.ContainsKey(t)) totalLooks.Add(t, new HashSet<string>());
                    if (!totalLooks[t].Contains(logMessage)) totalLooks[t].Add(logMessage);
                } else if (message.Contains(':')) {
                    if (logMessage.Length > 14 && logMessage.Substring(5, 9) == " Loot of ") { // loot drop message
                        if (!res.itemDrops.ContainsKey(t)) res.itemDrops.Add(t, new List<string>());
                        res.itemDrops[t].Add(logMessage);
                    } else { // if the message contains the ':' symbol but is not a loot drop message, it is a chat message, i.e. a command or url
                             // we only split at most once, because the chat message can contain the ':' symbol as well and we don't want to discard that
                        string[] split = message.Split(new char[] { ':' }, 2);
                        string command = split[1];
                        // now get the player name, we have to discard the level that is between brackets
                        // players can also have spaces in their name, so we take that into account
                        string[] playersplit = split[0].Split(' ');
                        string player = "";
                        foreach (string str in playersplit) {
                            if (str.Contains('[')) break;
                            player = (player == "" ? player : player + " ") + str;
                        }
                        if (player == "http" || player == "https") continue; // I don't remember why we do this, possible http link in a log message? not sure
                        if (command.Contains('@')) {
                            // @ symbol symbolizes a command, so if there is an @ symbol, we treat the string as a command
                            if (!res.commands.ContainsKey(t)) res.commands.Add(t, new List<Tuple<string, string>>());
                            res.commands[t].Add(new Tuple<string, string>(player, command));
                        } else if (command.Contains("www") || command.Contains("http") || command.Contains(".com") || command.Contains(".net") || command.Contains(".tv") || command.Contains(".br")) {
                            // check if the command is an url, we aren't really smart about this, just check for a couple of common url-like things
                            if (!res.urls.ContainsKey(t)) res.urls.Add(t, new List<Tuple<string, string>>());
                            res.urls[t].Add(new Tuple<string, string>(player, command));
                        }
                    }
                } else if (logMessage.Length > 17 && logMessage.Substring(5, 12) == " You gained ") {
                    // the message is an experience string, "You gained x experience."
                    try {
                        int experience = int.Parse(logMessage.Substring(17).Split(' ')[0]);
                        if (!res.exp.ContainsKey(t)) res.exp.Add(t, experience);
                        else res.exp[t] = res.exp[t] + experience;
                    } catch {
                        continue;
                    }
                } else {
                    string[] split = message.Split(' ');
                    if (split.Contains("hitpoints")) {
                        // damage log message (X loses Y hitpoints due to an attack by Z.)
                        int damage = 0;
                        try {
                            // damage always occurs one 'word' before the "hitpoints" word <X hitpoints>
                            damage = int.Parse(split[split.ToList().IndexOf("hitpoints") - 1]);
                        } catch {
                            continue;
                        }
                        string player;
                        if (logMessage.Substring(logMessage.Length - 12) == "your attack.") {
                            // X lost Y hitpoints because of your attack.
                            // attacker is the player himself
                            player = "you";
                        } else {
                            // X lost Y hitpoints because of an attack by Z.
                            // Z is the attacker => after the word "by"
                            if (!split.Contains("by")) continue;
                            player = "";
                            int ind = split.ToList().IndexOf("by") + 1;
                            for (int i = ind; i < split.Length; i++) {
                                player = (player == "" ? player : player + " ") + split[i];
                            }
                        }
                        if (!res.damageDealt.ContainsKey(player)) res.damageDealt.Add(player, new Dictionary<string, int>());
                        if (!res.damageDealt[player].ContainsKey(t)) res.damageDealt[player].Add(t, damage);
                        else res.damageDealt[player][t] = res.damageDealt[player][t] + damage;
                    } else if (split.Contains("advanced")) {
                        // advancement log message (You advanced from level x to level x + 1.)
                        if (logMessage.Substring(6, 13) == "You advanced " && logMessage.ToLower().Contains("level") && logMessage[logMessage.Length - 1] == '.' && !levelAdvances.Contains(logMessage)) {
                            res.newAdvances.Add(logMessage);
                            levelAdvances.Add(logMessage);
                        }
                    }
                }
            }
        }
    }
}
