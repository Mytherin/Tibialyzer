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
    public class ReadMemoryResults {
        public Dictionary<string, List<string>> itemDrops = new Dictionary<string, List<string>>();
        public Dictionary<string, int> exp = new Dictionary<string, int>();
        public Dictionary<string, Dictionary<string, DamageEntry>> healingDone = new Dictionary<string, Dictionary<string, DamageEntry>>();
        public Dictionary<string, Dictionary<string, DamageEntry>> damageDealt = new Dictionary<string, Dictionary<string, DamageEntry>>();
        public Dictionary<string, List<Tuple<string, string>>> commands = new Dictionary<string, List<Tuple<string, string>>>();
        public Dictionary<string, List<Tuple<string, string>>> urls = new Dictionary<string, List<Tuple<string, string>>>();
        public List<string> newAdvances = new List<string>();
        public List<Tuple<Event, string>> eventMessages = new List<Tuple<Event, string>>();
        public List<Tuple<Achievement, string>> achievements = new List<Tuple<Achievement, string>>();
        public Dictionary<string, List<string>> lookMessages = new Dictionary<string, List<string>>();
        public Dictionary<string, bool> deaths = new Dictionary<string, bool>();
        public Dictionary<string, List<string>> duplicateMessages = new Dictionary<string, List<string>>();
        public Dictionary<string, Dictionary<string, HashSet<int>>> usingMessages = new Dictionary<string, Dictionary<string, HashSet<int>>>();
    }

    public class DamageResult {
        public int totalDamage;
        public double damagePerSecond;
        public Dictionary<string, int> damagePerCreature = new Dictionary<string, int>();
    }

    public class ParseMemoryResults {
        private object damageLock = new object();
        private object healingLock = new object();
        private object damageTakenLock = new object();
        private Dictionary<string, DamageResult> damagePerSecond = null;
        private Dictionary<string, DamageResult> healingPerSecond = null;
        private Dictionary<string, DamageResult> damageTakenPerSecond = null;
        public List<string> times = null;
        public Dictionary<string, DamageResult> DamagePerSecond { get {
                lock(damageLock) {
                    if (damagePerSecond == null) {
                        damagePerSecond = new Dictionary<string, DamageResult>();
                        GlobalDataManager.GenerateDamageResults(damagePerSecond, times, false);
                    }
                }
                return damagePerSecond;
            }
        }
        public Dictionary<string, DamageResult> HealingPerSecond {
            get {
                lock(healingLock) {
                    if (healingPerSecond == null) {
                        healingPerSecond = new Dictionary<string, DamageResult>();
                        GlobalDataManager.GenerateDamageResults(healingPerSecond, times, true);
                    }
                }
                return healingPerSecond;
            }
        }
        public Dictionary<string, DamageResult> DamageTakenPerSecond {
            get {
                lock (damageTakenLock) {
                    if (damageTakenPerSecond == null) {
                        damageTakenPerSecond = new Dictionary<string, DamageResult>();
                        GlobalDataManager.GenerateDamageTakenResults(damageTakenPerSecond, times);
                    }
                }
                return damageTakenPerSecond;
            }
        }
        public List<string> newCommands = new List<string>();
        public List<string> newLooks = new List<string>();
        public List<Tuple<Creature, List<Tuple<Item, int>>, string>> newItems = new List<Tuple<Creature, List<Tuple<Item, int>>, string>>();
        public List<Tuple<Event, string>> newEventMessages = new List<Tuple<Event, string>>();
        public List<Tuple<Achievement, string>> newAchievements = new List<Tuple<Achievement, string>>();
        public int expPerHour = 0;
        public bool death = false;
        public bool newDamage = false;
        public bool newHealing = false;
    }

    public static class ReadMemoryManager {
        private static bool flashClient = true;
        public static int ignoreStamp = 0;
        public static byte[] missingChunksBuffer;
        public static byte[] memoryBuffer;
        private static bool skipDuplicateCommands;

        public static void Initialize() {
            ignoreStamp = TimestampManager.createStamp();
        }

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

        private static Dictionary<int, HashSet<long>> whiteListedAddresses = new Dictionary<int, HashSet<long>>();

        /// <summary>
        /// Scan the memory for any chunks that are missing from the whitelist table
        /// </summary>
        public static void ScanMissingChunks() {
            if (UseInternalScan) return; // If we are scanning the internal tabs structure, we do not need to scan missing chunks
            SYSTEM_INFO sys_info;
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;

            long sys_min_address_l = (long)proc_min_address;

            Process[] processes = ProcessManager.GetTibiaProcesses();
            if (processes == null || processes.Length == 0) {
                return;
            }
            flashClient = ProcessManager.IsFlashClient();
            var newWhitelistedAddresses = whiteListedAddresses.ToDictionary(x => x.Key, x => new HashSet<long>(x.Value));

            foreach (Process process in processes) {
                HashSet<long> whitelist;
                if (!newWhitelistedAddresses.TryGetValue(process.Id, out whitelist)) {
                    whitelist = new HashSet<long>();
                    newWhitelistedAddresses[process.Id] = whitelist;
                }

                proc_min_address_l = sys_min_address_l;

                IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
                MEMORY_BASIC_INFORMATION mem_basic_info;
                List<int> stamps = TimestampManager.getLatestStamps(3, ignoreStamp);
                int bytesRead = 0;  // number of bytes read with ReadProcessMemory

                try {
                    while (proc_min_address_l < proc_max_address_l) {
                        proc_min_address = new IntPtr(proc_min_address_l);
                        // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                        VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                        long addr = (long)proc_min_address;
                        // check if this memory chunk is accessible
                        if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT) {
                            if (!whitelist.Contains(addr)) {
                                if (missingChunksBuffer == null || missingChunksBuffer.Length < mem_basic_info.RegionSize) {
                                    missingChunksBuffer = new byte[mem_basic_info.RegionSize];
                                }

                                // read everything in the buffer above
                                ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, missingChunksBuffer, mem_basic_info.RegionSize, ref bytesRead);
                                // scan the memory for strings that start with timestamps and end with the null terminator ('\0')
                                IEnumerable<string> timestampLines;
                                if (!flashClient) {
                                    timestampLines = Parser.FindTimestamps(missingChunksBuffer, bytesRead);
                                    // if there are any timestamps found, add the address to the list of whitelisted addresses
                                    if (timestampLines.Any(x => stamps.Contains(TimestampManager.getStamp(int.Parse(x.Substring(0, 2)), int.Parse(x.Substring(3, 2)))))) {
                                        whitelist.Add(addr);
                                    }
                                } else {
                                    if (Parser.HasAnyValidTimestampsFlash(missingChunksBuffer, bytesRead, stamps)) {
                                        whitelist.Add(addr);
                                    }
                                }
                            }
                        }
                        // move to the next memory chunk
                        proc_min_address_l += mem_basic_info.RegionSize;
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }

            Interlocked.Exchange(ref whiteListedAddresses, newWhitelistedAddresses);
        }

        public static void ReadMemoryWhiteList(Process process, Dictionary<int, HashSet<long>> newWhitelistedAddresses, bool flashClient, ReadMemoryResults results) {
            HashSet<long> whitelist;
            if (!newWhitelistedAddresses.TryGetValue(process.Id, out whitelist)) {
                return;
            }

            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            foreach (long addr in whitelist) {
                IntPtr proc_min_address = new IntPtr(addr);

                MEMORY_BASIC_INFORMATION mem_basic_info;
                VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT) {
                    if (memoryBuffer == null || memoryBuffer.Length < mem_basic_info.RegionSize) {
                        memoryBuffer = new byte[mem_basic_info.RegionSize];
                    }

                    // read everything in the buffer above
                    ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, memoryBuffer, mem_basic_info.RegionSize, ref bytesRead);
                    // scan the memory for strings that start with timestamps and end with the null terminator ('\0')
                    IEnumerable<string> timestampLines;
                    if (!flashClient) {
                        timestampLines = Parser.FindTimestamps(memoryBuffer, bytesRead);
                    } else {
                        timestampLines = Parser.FindTimestampsFlash(memoryBuffer, bytesRead);
                    }

                    SearchChunk(timestampLines, results);
                }
            }
        }

        // SPECIAL THANKS TO tony902304 FOR MAKING THIS POSSIBLE //
        // For the C client: Read the server log messages from the internal Tab Messages structure, rather than scanning the entire process memory
        // This results in significantly faster performance and eliminates duplicate message issues in Tibia's memory
        // The only pitfall is that the base address has to be updated whenever Tibia is updated
        // We leave the old "scan everything" option as a setting for when Tibia gets updated and the base address has not been found yet
        public static IEnumerable<string> ReadTabMessages(int processHandle, int tabMessagesDataStructure) {
            int tabMessageNodeAddress = MemoryReader.ReadInt32(tabMessagesDataStructure + 0x10, processHandle);
            int messageCount = 0;

            while (tabMessageNodeAddress != 0x0) {
                messageCount++;
                int tabMessageAddress = MemoryReader.ReadInt32(tabMessageNodeAddress + 0x4C, processHandle);
                //max message input is 255 characters, but the Advertising channel has 400+ character initial message
                string tabMessage = MemoryReader.ReadString(tabMessageAddress, 255, processHandle);
                if (tabMessage != null && tabMessage.Length > 5 && tabMessage[0].isDigit() && tabMessage[1].isDigit() && tabMessage[2] == ':' && tabMessage[3].isDigit() && tabMessage[4].isDigit()) {
                    yield return tabMessage;
                }

                //next tab messages node pointer is current tab messages node address + 0x5C
                tabMessageNodeAddress = MemoryReader.ReadInt32(tabMessageNodeAddress + 0x5C, processHandle);
            }
            yield break;
        }

        public static void ReadMemoryInternal(Process process, ReadMemoryResults results) {
            int currentAddress = process.MainModule.BaseAddress.ToInt32();

            IntPtr ptr = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
            if (ptr == null) {
                return;
            }
            int processHandle = ptr.ToInt32();

            currentAddress += (int)MemoryReader.TabsBaseAddress;
            currentAddress = MemoryReader.ReadInt32(currentAddress, processHandle);
            currentAddress = MemoryReader.ReadInt32(currentAddress + 0x24, processHandle);
            currentAddress = MemoryReader.ReadInt32(currentAddress + 0x10, processHandle);
            currentAddress = MemoryReader.ReadInt32(currentAddress + 0x10, processHandle);
            int tabsAddress = MemoryReader.ReadInt32(currentAddress + 0x30, processHandle);

            //first tab node address is tabsAddress + 0x24
            int tabNodeAddress = MemoryReader.ReadInt32(tabsAddress + 0x24, processHandle);
            int tabCount = 0;
            //repeat until tab node address = 0x0
            while (tabNodeAddress != 0x0) {
                tabCount++;

                //use 0x30 for longer name (possibly upto 30 bytes)
                //0x2C will use '...' for names longer than 15 chars
                int tabNamePointer = MemoryReader.ReadInt32(tabNodeAddress + 0x2C, processHandle);
                string tabName = MemoryReader.ReadString(tabNamePointer, 16, processHandle);
                int tabMessagesDataStructure = MemoryReader.ReadInt32(tabNodeAddress + 0x24, processHandle);
                bool serverLog = tabName == "Server Log"; // only read log messages from the server log; only read chat messages from other tabs

                IEnumerable<string> tabMessages = ReadTabMessages(processHandle, tabMessagesDataStructure);
                SearchChunk(tabMessages, results, !serverLog, serverLog);

                //next tab node pointer is current tab node address + 0x10
                tabNodeAddress = MemoryReader.ReadInt32(tabNodeAddress + 0x10, processHandle);
            }
        }

        public static bool UseInternalScan = false;
        public static ReadMemoryResults ReadMemory() {
            ReadMemoryResults results = null;
            SYSTEM_INFO sys_info;
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;
            Process[] processes = ProcessManager.GetTibiaProcesses();
            if (processes == null || processes.Length == 0) {
                return null;
            }

            results = new ReadMemoryResults();
            flashClient = ProcessManager.IsFlashClient();
            skipDuplicateCommands = (flashClient || !SettingsManager.getSettingBool("ScanInternalTabStructure")) && SettingsManager.getSettingBool("SkipDuplicateCommands");
            Dictionary<int, HashSet<long>> newWhitelistedAddresses = null;
            Interlocked.Exchange(ref newWhitelistedAddresses, whiteListedAddresses);

            foreach (Process process in processes) {
                if (!flashClient && SettingsManager.getSettingBool("ScanInternalTabStructure")) {
                    ReadMemoryInternal(process, results);
                    UseInternalScan = true;
                } else {
                    ReadMemoryWhiteList(process, newWhitelistedAddresses, flashClient, results);
                    UseInternalScan = false;
                }
                process.Dispose();
            }
            FinalCleanup(results);
            return results;
        }

        private static void FinalCleanup(ReadMemoryResults res) {
            foreach (KeyValuePair<string, List<Tuple<string, string>>> kvp in res.commands) {
                string time = kvp.Key;
                if (res.itemDrops.ContainsKey(time)) {
                    foreach (Tuple<string, string> command in kvp.Value) {
                        if (res.itemDrops[time].Contains(command.Item2.Trim())) {
                            res.itemDrops[time].Remove(command.Item2);
                        }
                    }
                }
            }
        }

        private static void SearchChunk(IEnumerable<string> chunk, ReadMemoryResults res, bool readChatMessages = true, bool readLogMessages = true) {
            List<int> stamps = TimestampManager.getLatestStamps(3, ignoreStamp);
            foreach (string it in chunk) {
                string logMessage = it;
                string t = logMessage.Substring(0, 5);
                int hour = int.Parse(logMessage.Substring(0, 2));
                int minute = int.Parse(logMessage.Substring(3, 2));
                if (!stamps.Contains(TimestampManager.getStamp(hour, minute))) continue; // the log message is not recent, so we skip parsing it

                if (flashClient) {
                    // there is some inconsistency with log messages, certain log messages use "12:00: Message.", others use "12:00 Message"
                    // if there is a : after the timestamp we remove it
                    if (logMessage[5] == ':') {
                        logMessage = logMessage.Remove(5, 1);
                    }
                }

                string message = logMessage.Substring(6); // message without timestamp
                if (readLogMessages) {
                    if (logMessage.Length > 14 && logMessage.Substring(5, 9) == " You see " && logMessage[logMessage.Length - 1] == '.') {
                        // the message contains "you see", so it's a look message
                        if (!res.lookMessages.ContainsKey(t)) res.lookMessages.Add(t, new List<string>());
                        if (!skipDuplicateCommands || !res.lookMessages[t].Contains(logMessage)) {
                            res.lookMessages[t].Add(logMessage);
                        }
                        continue;
                    } else if (message.Contains(':')) {
                        if (logMessage.Length > 14 && logMessage.Substring(5, 9) == " Loot of ") { // loot drop message
                            if (!res.itemDrops.ContainsKey(t)) res.itemDrops.Add(t, new List<string>());
                            res.itemDrops[t].Add(logMessage);
                            continue;
                        }
                    } else if (logMessage.Length > 17 && logMessage.Substring(5, 12) == " You gained " && logMessage.EndsWith("experience points.")) {
                        // the message is an experience string, "You gained x experience."
                        try {
                            int experience = int.Parse(logMessage.Substring(17).Split(' ')[0]);
                            if (!res.exp.ContainsKey(t)) res.exp.Add(t, experience);
                            else res.exp[t] = res.exp[t] + experience;
                        } catch {
                        }
                        continue;
                    } else if (logMessage.Length == 19 && logMessage.Substring(5, 14) == " You are dead.") {
                        if (!res.deaths.ContainsKey(t))
                            res.deaths.Add(t, true);
                    } else if (logMessage.Length > 18) {
                        string[] split = message.Split(' ');
                        int index = split.IndexOf("hitpoints");
                        if (index > 0) {
                            int ind;
                            // damage log message (X loses Y hitpoints due to an attack by Z.)
                            int damage = 0;
                            if (!int.TryParse(split[index - 1], out damage)) {
                                continue;
                            }
                            string player;
                            if ((logMessage.Substring(logMessage.Length - 12) == "your attack.") || (logMessage.Substring(logMessage.Length - 21) == "your critical attack.")) {
                                // X lost Y hitpoints because of your attack.
                                // attacker is the player himself
                                player = "You";
                            } else if (split.Contains("by")) {
                                // X lost Y hitpoints because of an attack by Z.
                                // Z is the attacker => after the word "by"
                                player = "";
                                ind = split.IndexOf("by") + 1;
                                for (int i = ind; i < split.Length; i++) {
                                    player = (player == "" ? player : player + " ") + split[i];
                                }
                            } else {
                                continue;
                            }
                            string splitTerm;
                            if (split.Contains("loses")) {
                                splitTerm = "loses";
                            } else if (split.Contains("lose")) {
                                splitTerm = "lose";
                            } else {
                                continue;
                            }
                            ind = split.IndexOf(splitTerm);
                            string target = "";
                            for (int i = 0; i < ind; i++) {
                                target = (target == "" ? target : target + " ") + split[i];
                            }
                            if (!res.damageDealt.ContainsKey(player)) {
                                res.damageDealt.Add(player, new Dictionary<string, DamageEntry>());
                            }
                            DamageEntry damageEntry;
                            if (!res.damageDealt[player].ContainsKey(t)) {
                                damageEntry = new DamageEntry();
                                damageEntry.damage = damage;
                                damageEntry.targetDamage.Add(target, damage);
                                res.damageDealt[player].Add(t, damageEntry);
                            } else {
                                damageEntry = res.damageDealt[player][t];
                                damageEntry.damage += damage;
                                if (damageEntry.targetDamage.ContainsKey(target)) {
                                    damageEntry.targetDamage[target] += damage;
                                } else {
                                    damageEntry.targetDamage.Add(target, damage);
                                }
                            }
                            continue;
                        } else {
                            index = split.IndexOf("hitpoints.");
                            if (index > 0) {
                                // heal log message (X healed Y for Z hitpoints.)
                                int healing = 0;
                                if (!int.TryParse(split[index - 1], out healing)) {
                                    continue;
                                }

                                int forIndex = split.IndexOf("for");
                                if (forIndex <= 0) {
                                    continue;
                                }


                                string splitTerm;
                                if (split.Contains("heal")) {
                                    splitTerm = "heal";
                                } else if (split.Contains("healed")) {
                                    splitTerm = "healed";
                                } else {
                                    continue;
                                }
                                int healIndex = split.IndexOf(splitTerm);
                                if (healIndex >= forIndex) {
                                    continue;
                                }

                                string source = "";
                                for (int i = 0; i < healIndex; i++) {
                                    if (split[i] == "was" || split[i] == "were") break;
                                    if (split[i] == "by") continue;
                                    source = (source == "" ? source : source + " ") + split[i];
                                }
                                string target = "";
                                for (int i = healIndex + 1; i < forIndex; i++) {
                                    if (split[i] == "was" || split[i] == "were") break;
                                    if (split[i] == "by") continue;
                                    target = (target == "" ? target : target + " ") + split[i];
                                }
                                if (target == "yourself" || target == "itself" || target == "himself" || target == "herself") {
                                    target = source;
                                }

                                if (target.Length == 0 || source.Length == 0) {
                                    continue;
                                }

                                if (split.Contains("by")) {
                                    // X healed Y for Z. => X is the source and Y is the target (default)
                                    // X was healed by Y for Z. => X is the target and Y is the source, so swap source and target
                                    string temp = source;
                                    source = target;
                                    target = temp;
                                }

                                if (!res.healingDone.ContainsKey(source)) {
                                    res.healingDone.Add(source, new Dictionary<string, DamageEntry>());
                                }
                                DamageEntry healingEntry;
                                if (!res.healingDone[source].ContainsKey(t)) {
                                    healingEntry = new DamageEntry();
                                    healingEntry.damage = healing;
                                    healingEntry.targetDamage.Add(target, healing);
                                    res.healingDone[source].Add(t, healingEntry);
                                } else {
                                    healingEntry = res.healingDone[source][t];
                                    healingEntry.damage += healing;
                                    if (healingEntry.targetDamage.ContainsKey(target)) {
                                        healingEntry.targetDamage[target] += healing;
                                    } else {
                                        healingEntry.targetDamage.Add(target, healing);
                                    }
                                }
                            } else if (logMessage.Substring(5, 14) == " You advanced " && logMessage.Contains("level", StringComparison.OrdinalIgnoreCase)) {
                                // advancement log message (You advanced from level x to level x + 1.)
                                if (logMessage[logMessage.Length - 1] == '.') {
                                    if (GlobalDataManager.AddLevelAdvance(logMessage)) {
                                        res.newAdvances.Add(logMessage);
                                    }
                                    continue;
                                }
                            } else if (logMessage.Substring(5, 7) == " Using " && logMessage.Substring(logMessage.Length - 3, 3) == "...") {
                                // using log message (Using one of X items...)
                                var values = Parser.ParseUsingMessage(logMessage);
                                if (!res.usingMessages.ContainsKey(values.Item1)) res.usingMessages.Add(values.Item1, new Dictionary<string, HashSet<int>>());
                                if (!res.usingMessages[values.Item1].ContainsKey(t)) res.usingMessages[values.Item1].Add(t, new HashSet<int>());
                                res.usingMessages[values.Item1][t].Add(values.Item2);
                                continue;
                            } else if (logMessage.Length > 50 && logMessage.Substring(5, 45) == " Congratulations! You earned the achievement ") {
                                Achievement achievement = StorageManager.getAchievement(Parser.GetAchievement(logMessage));
                                if (achievement != null) {
                                    res.achievements.Add(new Tuple<Achievement, string>(achievement, logMessage));
                                } else {
                                    Console.WriteLine("Unrecognized achievement {0}.", Parser.GetAchievement(logMessage));
                                }
                            } else {
                                foreach (Event ev in StorageManager.eventIdMap.Values) {
                                    foreach (string evMessage in ev.eventMessages) {
                                        if (logMessage.Length == evMessage.Length + 6 && logMessage.Contains(evMessage.Trim(), StringComparison.OrdinalIgnoreCase)) {
                                            res.eventMessages.Add(new Tuple<Event, string>(ev, logMessage));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (readChatMessages) {
                    if (message.Contains(':')) {
                        if (logMessage.Length > 14 && logMessage.Substring(5, 9) == " Loot of ") { // loot drop message
                            continue;
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
                                var tpl = new Tuple<string, string>(player, command);
                                if (!skipDuplicateCommands || !res.commands[t].Contains(tpl)) {
                                    res.commands[t].Add(tpl);
                                }
                            } else if (command.Contains("www") || command.Contains("http") || command.Contains(".com") || command.Contains(".net") || command.Contains(".tv") || command.Contains(".br")) {
                                // check if the command is an url, we aren't really smart about this, just check for a couple of common url-like things
                                if (!res.urls.ContainsKey(t)) res.urls.Add(t, new List<Tuple<string, string>>());
                                res.urls[t].Add(new Tuple<string, string>(player, command));
                            }
                        }
                    }
                }
            }
        }
    }
}
