/*
The functions in this file are based on the work by Viktor Gustavsson (https://github.com/villor/TibiaReader), which has the following license.

The MIT License (MIT)

Copyright (c) 2016 Viktor Gustavsson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using Tibialyzer.Structures;
using System.Drawing;

namespace Tibialyzer {
    public static class MemoryReader {
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        static extern void CloseHandle(IntPtr handle);

        static int PROCESS_QUERY_INFORMATION = 0x0400;
        static int PROCESS_WM_READ = 0x0010;

        private static bool tibia11_addresses = false;
        private static UInt32 XORAddress;
        private static UInt32 HealthAddress;
        private static UInt32 MaxHealthAddress;
        private static UInt32 ManaAddress;
        private static UInt32 MaxManaAddress;
        private static UInt32 PlayerIDAddress;
        private static UInt32 BattleListAddress;
        private static UInt32 ExperienceAddress;
        private static UInt32 LevelAddress;
        private static UInt32 MagicLevelAddress;
        private static UInt32 AmmunitionCountAddress;
        private static UInt32 AmmunitionTypeAddress;
        private static UInt32 WeaponCountAddress;
        private static UInt32 WeaponTypeAddress;
        private static UInt32 BootsTypeAddress;
        private static UInt32 RingTypeAddress;
        public static UInt32 TabsBaseAddress;

        private static uint BL_CREATURE_SIZE = 220;
        private static int BL_Z_OFFSET = 36;
        private static int BL_Y_OFFSET = 40;
        private static int BL_X_OFFSET = 44;
        private static int BL_HP_OFFSET = 140;

        public static int START_X = 124 * 256;
        public static int START_Y = 121 * 256;
        public static int END_X = 132 * 256;
        public static int END_Y = 129 * 256;

        public static event EventHandler<PlayerHealth> HealthChanged;
        public static event EventHandler<PlayerMana> ManaChanged;
        public static event EventHandler<PlayerExperience> ExperienceChanged;

        private static SafeTimer healthTimer;
        private static SafeTimer manaTimer;
        private static SafeTimer expTimer;

        private static bool screenshotTaken;
        private static object screenshotLock = new object();
        private static System.Timers.Timer screenshotTimer;

        public static Dictionary<string, string> MemorySettings = new Dictionary<string, string>();

        public static void InitializeMemoryAddresses() {
            Dictionary<string, UInt32> memoryAddresses = ParseAddresses();
            XORAddress = 0;
            memoryAddresses.TryGetValue("xoraddress", out XORAddress);
            memoryAddresses.TryGetValue("healthaddress", out HealthAddress);
            memoryAddresses.TryGetValue("maxhealthaddress", out MaxHealthAddress);
            memoryAddresses.TryGetValue("manaaddress", out ManaAddress);
            memoryAddresses.TryGetValue("maxmanaaddress", out MaxManaAddress);
            memoryAddresses.TryGetValue("playeridaddress", out PlayerIDAddress);
            memoryAddresses.TryGetValue("battlelistaddress", out BattleListAddress);
            memoryAddresses.TryGetValue("experienceaddress", out ExperienceAddress);
            memoryAddresses.TryGetValue("leveladdress", out LevelAddress);
            memoryAddresses.TryGetValue("magicleveladdress", out MagicLevelAddress);
            memoryAddresses.TryGetValue("tabsbaseaddress", out TabsBaseAddress);
            memoryAddresses.TryGetValue("ammunitioncountaddress", out AmmunitionCountAddress);
            memoryAddresses.TryGetValue("ammunitiontypeaddress", out AmmunitionTypeAddress);
            memoryAddresses.TryGetValue("weaponcountaddress", out WeaponCountAddress);
            memoryAddresses.TryGetValue("weapontypeaddress", out WeaponTypeAddress);
            memoryAddresses.TryGetValue("bootstypeaddress", out BootsTypeAddress);
            memoryAddresses.TryGetValue("ringtypeaddress", out RingTypeAddress);
            tibia11_addresses = false;
        }

        public static void Initialize() {
            try {
                InitializeMemoryAddresses();

                healthTimer = new SafeTimer(10, UpdateHealth);
                healthTimer.Start();

                manaTimer = new SafeTimer(10, UpdateMana);
                manaTimer.Start();

                expTimer = new SafeTimer(10, UpdateExp);
                expTimer.Start();

                screenshotTaken = false;
                screenshotTimer = new System.Timers.Timer(30000);
                screenshotTimer.Elapsed += ResetScreenshotTaken;
                screenshotTimer.AutoReset = false;
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning("Failed to read memory addresses file: " + ex.Message);
            }
            InitializeBattleList();
        }

        private static void ResetScreenshotTaken(object sender, System.Timers.ElapsedEventArgs e) {
            lock (screenshotLock) {
                screenshotTaken = false;
                screenshotTimer.Stop();
                screenshotTimer.Enabled = false;
            }
        }

        private static Dictionary<string, UInt32> ParseAddresses() {
            MemorySettings.Clear();
            Dictionary<string, UInt32> addresses = new Dictionary<string, UInt32>();
            using (StreamReader reader = new StreamReader(Constants.MemoryAddresses)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.Contains("=")) {
                        string[] split = line.Split('=');

                        string key = split[0].Trim().ToLower();
                        UInt32 value = 0;
                        if (!key.Contains("noparse") && UInt32.TryParse(split[1].Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value)) {
                            addresses.Add(key, value);
                        } else {
                            key = key.Replace("noparse", "");
                            if (!MemorySettings.ContainsKey(key)) {
                                MemorySettings.Add(key, split[1]);
                            }
                        }
                    }
                }
            }
            return addresses;
        }

        public static void SetStatsAddress(uint address) {
            HealthAddress = address + 0x24;
            MaxHealthAddress = address + 0x28;
            ManaAddress = address + 0x2C;
            MaxManaAddress = address + 0x30;
            tibia11_addresses = true;
        }
        public static void SetExpAddress(uint address) {
            ExperienceAddress = address + 0x18 - baseAddress;
            tibia11_addresses = true;
        }

        private static UInt32 baseAddress;
        private static IntPtr handle = new IntPtr(0);
        private static int processID = -1;
        public static IntPtr OpenProcess(Process process) {
            if (processID == process.Id) return MemoryReader.handle;

            if (processID >= 0) {
                CloseHandle(MemoryReader.handle);
            }

            try {
                MemoryReader.baseAddress = (UInt32)process.MainModule.BaseAddress.ToInt32();
                MemoryReader.handle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
                MemoryReader.processID = process.Id;
            } catch {

            }
            return MemoryReader.handle;
        }

        private static void UpdateHealth() {
            bool attributesChanged = ReadHealth();
            attributesChanged |= ReadMaxHealth();

            if (attributesChanged) {

                if (SettingsManager.getSettingBool("AutoScreenshotLowLife")) {
                    if (health < maxHealth / 10 && health > 0 && maxHealth > health) {
                        lock (screenshotLock) {
                            if (!screenshotTaken) {
                                ScreenshotManager.saveScreenshot("LowLife", ScreenshotManager.takeScreenshot());
                                screenshotTaken = true;
                                screenshotTimer.Start();
                            }
                        }
                    }
                }

                if (HealthChanged != null) {
                    var playerHealth = new PlayerHealth {
                        Health = health,
                        MaxHealth = maxHealth
                    };
                    HealthChanged(null, playerHealth);
                }
            }
        }

        private static void UpdateMana() {
            bool attributesChanged = ReadMana();
            attributesChanged |= ReadMaxMana();

            if (attributesChanged && ManaChanged != null) {
                var playerMana = new PlayerMana {
                    Mana = mana,
                    MaxMana = maxMana
                };

                ManaChanged(null, playerMana);
            }
        }

        private static void UpdateExp() {
            bool attributesChanged = ReadExperience();
            if (!tibia11_addresses) {
                attributesChanged |= ReadLevel();
            }

            if (attributesChanged && ExperienceChanged != null) {
                if (tibia11_addresses) {
                    level = GetLevelFromExperience(experience);
                }
                var playerExperience = new PlayerExperience {
                    Level = level,
                    Experience = experience
                };

                ExperienceChanged(null, playerExperience);
            }
        }

        private static UInt32 GetAddress(UInt32 offset) {
            return baseAddress + offset;
        }

        private static byte[] ReadBytes(Int64 address, uint n, int processHandle = -1) {
            IntPtr ptrBytesRead;
            byte[] buf = new byte[n];
            ReadProcessMemory(processHandle < 0 ? handle : new IntPtr(processHandle), new IntPtr(address), buf, n, out ptrBytesRead);
            return buf;
        }

        public static Int32 ReadInt32(Int64 address, int handle = -1) {
            return BitConverter.ToInt32(ReadBytes(address, 4, handle), 0);
        }

        public static Int64 ReadInt64(Int64 address, int handle = -1) {
            return BitConverter.ToInt64(ReadBytes(address, 8, handle), 0);
        }

        public static UInt32 ReadUInt32(Int64 address, int handle = -1) {
            return BitConverter.ToUInt32(ReadBytes(address, 4, handle), 0);
        }

        public static string ReadString(Int64 address, uint length = 32, int handle = -1) {
            byte[] bytes = ReadBytes(address, length, handle);
            string str = ASCIIEncoding.Default.GetString(bytes);
            return str.Split('\0')[0];
        }

        public static string ReadUnicodeString(Int64 address, uint length = 32, int handle = -1) {
            byte[] bytes = ReadBytes(address, length, handle);
            string str = System.Text.UnicodeEncoding.Unicode.GetString(bytes);
            return str.Split('\0')[0];
        }

        public static string ReadQString(Int32 address) {
            byte[] qstringdata = ReadBytes(address, 16);
            // message size
            int size = BitConverter.ToInt32(qstringdata, 4);
            int offset = BitConverter.ToInt32(qstringdata, 12);
            if (size <= 0 || size > 1000) {
                return null;
            }
            return ReadUnicodeString(address + offset, (uint)(size * 2));
        }

        public static bool UnixTimeStampToDateTime(long unixTimeStamp, out DateTime dtDateTime) {
            dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            if (unixTimeStamp < 0 || unixTimeStamp > 4123078141000) {
                return false;
            }
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return true;
        }

        public static string ReadChatMessage(Int32 address) {
            byte[] chat_message = ReadBytes(address, 40);
            // system or written message?
            int messageType = BitConverter.ToInt32(chat_message, 0);
            int timeAddress = BitConverter.ToInt32(chat_message, 4);
            // get the timestamp of the message
            long utctimestamp = MemoryReader.ReadInt64(timeAddress + 8);
            DateTime time;
            if (!UnixTimeStampToDateTime(utctimestamp, out time)) {
                return null;
            }
            string timestamp = String.Format("{0:00}:{1:00} ", time.Hour, time.Minute);
            if (messageType == 0) {
                // system message, no speaker
                string qstring = ReadQString(BitConverter.ToInt32(chat_message, 8));
                if (qstring != null && qstring.Length > 5) {
                    return timestamp + qstring;
                } else {
                    return null;
                }
            } else {
                // npc or player message
                string message = ReadQString(BitConverter.ToInt32(chat_message, 8));
                string speaker = ReadQString(BitConverter.ToInt32(chat_message, 16));
                if (message == null || speaker == null) {
                    return null;
                }
                if (messageType == 1) {
                    // npc, no level
                    return timestamp + String.Format("{0}: {1}", speaker, message);
                } else if (messageType == 2) {
                    // player, add level information
                    short level = BitConverter.ToInt16(chat_message, 20);
                    return timestamp + String.Format("{0} [{1}]: {2}", speaker, level, message);
                }
            }
            return null;
        }

        private static int XOR {
            get {
                if (XORAddress == 0) return 0;
                return ReadInt32(GetAddress(XORAddress));
            }
        }

        public static int ReadProperty(uint address) {
            if (tibia11_addresses) {
                return ReadInt32(address);
            }
            return ReadInt32(GetAddress(address)) ^ XOR;
        }

        private static int health;

        private static bool ReadHealth() {
            int currentHealth = ReadProperty(HealthAddress);
            bool healthChanged = currentHealth != health;
            health = currentHealth;

            return healthChanged;
        }

        private static int maxHealth;

        private static bool ReadMaxHealth() {
            int currentMaxHealth = ReadProperty(MaxHealthAddress);
            bool maxHealthChanged = currentMaxHealth != maxHealth;
            maxHealth = currentMaxHealth;

            return maxHealthChanged;
        }

        private static int mana;

        private static bool ReadMana() {
            int currentMana = ReadProperty(ManaAddress);
            bool manaChanged = currentMana != mana;
            mana = currentMana;

            return manaChanged;
        }

        private static int maxMana;

        private static bool ReadMaxMana() {
            int currentMaxMana = ReadProperty(MaxManaAddress);
            bool maxManaChanged = currentMaxMana != maxMana;
            maxMana = currentMaxMana;

            return maxManaChanged;
        }

        public static int PlayerId {
            get {
                return ReadInt32(GetAddress(PlayerIDAddress));
            }
        }

        private static int playerAddress = -1;
        public static string playerName = null;

        public static string PlayerName {
            get {
                if (tibia11_addresses) {
                    return playerName;
                }
                MemoryReader.UpdateBattleList();
                return GetPlayerName(PlayerId);
            }
        }

        public static int X {
            get {
                if (playerAddress == -1) playerAddress = GetPlayerPosition(PlayerId);
                return battleList[playerAddress].x - START_X;
            }
        }

        public static int Y {
            get {
                if (playerAddress == -1) playerAddress = GetPlayerPosition(PlayerId);
                return battleList[playerAddress].y - START_Y;
            }
        }

        public static int Z {
            get {
                if (playerAddress == -1) playerAddress = GetPlayerPosition(PlayerId);
                return battleList[playerAddress].z;
            }
        }

        public static long experience;

        private static bool ReadExperience() {
            long currentExperience = ReadInt64(GetAddress(ExperienceAddress));
            bool experienceChanged = currentExperience == experience;
            experience = currentExperience;

            return experienceChanged;
        }

        public static int level;


        public static int GetLevelFromExperience(long experience, int level = 150, int adjustment = 75, int iterations = 100) {
            if (iterations <= 0) return -1;
            if (experience < ExperienceBar.GetExperience(level)) {
                return GetLevelFromExperience(experience, level - adjustment, adjustment / 2, iterations - 1);
            } else {
                if (experience < ExperienceBar.GetExperience(level + 1)) {
                    return level + 1;
                } else {
                    return GetLevelFromExperience(experience, level + adjustment, adjustment, iterations - 1);
                }
            }
        }

        private static bool ReadLevel() {
            int currentLevel = ReadInt32(GetAddress(LevelAddress));
            bool levelChanged = level == currentLevel;
            level = currentLevel;

            return levelChanged;
        }

        private static int magicLevel;

        private static bool ReadMagicLevel() {
            int currentMagicLevel = ReadInt32(GetAddress(MagicLevelAddress));
            bool magicLevelChanged = currentMagicLevel == magicLevel;
            magicLevel = currentMagicLevel;

            return magicLevelChanged;
        }

        public static int AmmunitionType {
            get {
                return ReadInt32(GetAddress(AmmunitionTypeAddress));
            }
        }

        public static int AmmunitionCount {
            get {
                return ReadInt32(GetAddress(AmmunitionCountAddress));
            }
        }

        public static int WeaponCount {
            get {
                return ReadInt32(GetAddress(WeaponCountAddress));
            }
        }

        public static int WeaponType {
            get {
                return ReadInt32(GetAddress(WeaponTypeAddress));
            }
        }

        public static int BootsType {
            get {
                return ReadInt32(GetAddress(BootsTypeAddress));
            }
        }

        public static int RingType {
            get {
                return ReadInt32(GetAddress(RingTypeAddress));
            }
        }


        private static BattleListEntry[] battleList = new BattleListEntry[1300];

        private static void InitializeBattleList() {
            for (int i = 0; i < battleList.Length; i++) {
                battleList[i] = new BattleListEntry();
            }
        }

        public static void UpdateBattleList() {
            lock (battleList) {
                UInt32 creature = GetAddress(BattleListAddress);
                for (int i = 0; i < battleList.Length; i++) {
                    battleList[i].id = ReadInt32(creature);
                    battleList[i].name = ReadString(creature + 4);
                    battleList[i].x = ReadInt32(creature + BL_X_OFFSET);
                    battleList[i].y = ReadInt32(creature + BL_Y_OFFSET);
                    battleList[i].z = ReadInt32(creature + BL_Z_OFFSET);
                    battleList[i].hp = ReadInt32(creature + BL_HP_OFFSET);
                    creature += BL_CREATURE_SIZE;
                }
            }
        }

        public static BattleListEntry GetPlayerEntry(int id) {
            int location = GetPlayerPosition(id);
            if (location >= 0) {
                return battleList[location];
            }
            return new BattleListEntry { id = -1, x = -1, y = -1, z = -1, name = null, hp = -1 };
        }

        public static int GetPlayerPosition(int id) {
            lock (battleList) {
                for (int i = 0; i < battleList.Length; i++) {
                    if (battleList[i].name != null && battleList[i].id == id) {
                        return i;
                    }
                }
                return -1;
            }
        }

        public static int GetPlayerID(string name) {
            lock (battleList) {
                for (int i = 0; i < battleList.Length; i++) {
                    if (battleList[i].name != null && battleList[i].name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) {
                        return battleList[i].id;
                    }
                }
                return -1;
            }
        }

        public static string GetPlayerName(int id, int battlelistentry = -1) {
            lock (battleList) {
                if (battlelistentry > 0 && battleList[battlelistentry].id == id) {
                    return battleList[battlelistentry].name;
                }
                for (int i = 0; i < battleList.Length; i++) {
                    if (battleList[i].name != null && battleList[i].id == id) {
                        return battleList[i].name;
                    }
                }
                return null;
            }
        }

        public static bool PlayerInVision(BattleListEntry a, BattleListEntry b) {
            if (a.z != b.z) {
                return false;
            }
            return Math.Abs(a.x - b.x) <= 7 && Math.Abs(a.y - b.y) <= 5;
        }

        public static int GetHealthPercentage(int id, ref int battlelistentry) {
            lock (battleList) {
                if (playerAddress == -1) playerAddress = GetPlayerPosition(PlayerId);
                if (battleList[battlelistentry].id == id) {
                    return PlayerInVision(battleList[playerAddress], battleList[battlelistentry]) ? battleList[battlelistentry].hp : -1;
                }

                for (int i = 0; i < battleList.Length; i++) {
                    if (battleList[i].id == id) {
                        battlelistentry = i;
                        return PlayerInVision(battleList[playerAddress], battleList[battlelistentry]) ? battleList[battlelistentry].hp : -1;
                    }
                }
                return -1;
            }
        }
    }

    public class BattleListEntry {
        public int id;
        public string name;
        public int x;
        public int y;
        public int z;
        public int hp;
    }
}
