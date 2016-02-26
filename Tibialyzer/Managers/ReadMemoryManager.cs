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
        public Dictionary<string, Dictionary<string, int>> damageDealt = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, List<Tuple<string, string>>> commands = new Dictionary<string, List<Tuple<string, string>>>();
        public Dictionary<string, List<Tuple<string, string>>> urls = new Dictionary<string, List<Tuple<string, string>>>();
        public List<string> newAdvances = new List<string>();
        public List<Tuple<Event, string>> eventMessages = new List<Tuple<Event, string>>();
        public Dictionary<string, List<string>> lookMessages = new Dictionary<string, List<string>>();
        public Dictionary<string, bool> deaths = new Dictionary<string, bool>();
        public Dictionary<string, List<string>> duplicateMessages = new Dictionary<string, List<string>>();
        public Dictionary<string, HashSet<int>> usingMessages = new Dictionary<string, HashSet<int>>();
    }

    public class DamageResult {
        public int totalDamage;
        public double damagePerSecond;
    }

    public class ParseMemoryResults {
        public Dictionary<string, DamageResult> damagePerSecond = new Dictionary<string, DamageResult>();
        public List<string> newCommands = new List<string>();
        public List<string> newLooks = new List<string>();
        public List<Tuple<Creature, List<Tuple<Item, int>>>> newItems = new List<Tuple<Creature, List<Tuple<Item, int>>>>();
        public List<Tuple<Event, string>> newEventMessages = new List<Tuple<Event, string>>();
        public int expPerHour = 0;
        public bool death = false;
        public bool newDamage = false;
    }

    public static class ReadMemoryManager {
        private static bool flashClient = true;
        public static int ignoreStamp = 0;
        public static byte[] missingChunksBuffer;
        public static byte[] memoryBuffer;

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
                
                try
                {
                    while (proc_min_address_l < proc_max_address_l)
                    {
                        proc_min_address = new IntPtr(proc_min_address_l);
                        // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                        VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                        long addr = (long)proc_min_address;
                        // check if this memory chunk is accessible
                        if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                        {
                            if (!whitelist.Contains(addr))
                            {
                                if (missingChunksBuffer == null || missingChunksBuffer.Length < mem_basic_info.RegionSize)
                                {
                                    missingChunksBuffer = new byte[mem_basic_info.RegionSize];
                                }

                                // read everything in the buffer above
                                ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, missingChunksBuffer, mem_basic_info.RegionSize, ref bytesRead);
                                // scan the memory for strings that start with timestamps and end with the null terminator ('\0')
                                IEnumerable<string> timestampLines;
                                if (!flashClient)
                                {
                                    timestampLines = Parser.FindTimestamps(missingChunksBuffer, bytesRead);
                                    // if there are any timestamps found, add the address to the list of whitelisted addresses
                                    if (timestampLines.Any(x => stamps.Contains(TimestampManager.getStamp(int.Parse(x.Substring(0, 2)), int.Parse(x.Substring(3, 2))))))
                                    {
                                        whitelist.Add(addr);
                                    }
                                }
                                else {
                                    if (Parser.HasAnyValidTimestampsFlash(missingChunksBuffer, bytesRead, stamps))
                                    {
                                        whitelist.Add(addr);
                                    }
                                }
                            }
                        }
                        // move to the next memory chunk
                        proc_min_address_l += mem_basic_info.RegionSize;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }

            Interlocked.Exchange(ref whiteListedAddresses, newWhitelistedAddresses);
        }

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
            Dictionary<int, HashSet<long>> newWhitelistedAddresses = null;
            Interlocked.Exchange(ref newWhitelistedAddresses, whiteListedAddresses);
            foreach (Process process in processes) {
                HashSet<long> whitelist;
                if (!newWhitelistedAddresses.TryGetValue(process.Id, out whitelist))
                {
                    continue;
                }

                IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

                int bytesRead = 0;  // number of bytes read with ReadProcessMemory
                foreach (long addr in whitelist)
                {
                    proc_min_address = new IntPtr(addr);

                    MEMORY_BASIC_INFORMATION mem_basic_info;
                    VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                    if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                    {
                        if (memoryBuffer == null || memoryBuffer.Length < mem_basic_info.RegionSize)
                        {
                            memoryBuffer = new byte[mem_basic_info.RegionSize];
                        }

                        // read everything in the buffer above
                        ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, memoryBuffer, mem_basic_info.RegionSize, ref bytesRead);
                        // scan the memory for strings that start with timestamps and end with the null terminator ('\0')
                        IEnumerable<string> timestampLines;
                        if (!flashClient)
                        {
                            timestampLines = Parser.FindTimestamps(memoryBuffer, bytesRead);
                        }
                        else {
                            timestampLines = Parser.FindTimestampsFlash(memoryBuffer, bytesRead);
                        }

                        SearchChunk(timestampLines, results);
                    }
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

        private static void SearchChunk(IEnumerable<string> chunk, ReadMemoryResults res) {
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
                if (logMessage.Length > 14 && logMessage.Substring(5, 9) == " You see ") {
                    // the message contains "you see", so it's a look message
                    if (!res.lookMessages.ContainsKey(t)) res.lookMessages.Add(t, new List<string>());
                    res.lookMessages[t].Add(logMessage);
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
                } else if (logMessage.Length == 19 && logMessage.Substring(5, 14) == " You are dead.") {
                    if (!res.deaths.ContainsKey(t))
                        res.deaths.Add(t, true);
                } else if (logMessage.Length > 18) {
                    string[] split = message.Split(' ');
                    if (split.Contains("hitpoints") && split.ToList().IndexOf("hitpoints") > 0) {
                        // damage log message (X loses Y hitpoints due to an attack by Z.)
                        int damage = 0;
                        if (!int.TryParse(split[split.ToList().IndexOf("hitpoints") - 1], out damage)) {
                            continue;
                        }
                        string player;
                        if (logMessage.Substring(logMessage.Length - 12) == "your attack.") {
                            // X lost Y hitpoints because of your attack.
                            // attacker is the player himself
                            player = "You";
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
                    } else if (logMessage.Substring(5, 14) == " You advanced " && logMessage.Contains("level", StringComparison.OrdinalIgnoreCase)) {
                        // advancement log message (You advanced from level x to level x + 1.)
                        if (logMessage[logMessage.Length - 1] == '.') {
                            if (GlobalDataManager.AddLevelAdvance(logMessage)) {
                                res.newAdvances.Add(logMessage);
                            }
                        }
                    } else if (logMessage.Substring(5, 7) == " Using " && logMessage.Substring(logMessage.Length - 3, 3) == "...") {
                        // using log message (Using one of X items...)
                        var values = Parser.ParseUsingMessage(logMessage);
                        if (!res.usingMessages.ContainsKey(values.Item1)) res.usingMessages.Add(values.Item1, new HashSet<int>());
                        if (!res.usingMessages[values.Item1].Contains(values.Item2)) res.usingMessages[values.Item1].Add(values.Item2);
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
    }
}
