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

namespace Tibialyzer {
    public static class MemoryReader {
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        static int PROCESS_QUERY_INFORMATION = 0x0400;
        static int PROCESS_WM_READ = 0x0010;

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

        private static int START_X = 124 * 256;
        private static int START_Y = 121 * 256;
        
        public static event EventHandler<PlayerAttributes> AttributesChanged;

        private static SafeTimer readTimer;

        public static void Initialize() {
            try {
                Dictionary<string, UInt32> memoryAddresses = ParseAddresses();
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

                readTimer = new SafeTimer(10, UpdateAttributes);
                readTimer.Start();
            } catch(Exception ex) {
                MainForm.mainForm.DisplayWarning("Failed to read memory addresses file: " + ex.Message);
            }
            InitializeBattleList();
        }

        private static Dictionary<string, UInt32> ParseAddresses() {
            Dictionary<string, UInt32> addresses = new Dictionary<string, UInt32>();
            using (StreamReader reader = new StreamReader(Constants.MemoryAddresses)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.Contains("=")) {
                        string[] split = line.Split('=');

                        string key = split[0].Trim().ToLower();
                        UInt32 value = 0;
                        if (UInt32.TryParse(split[1].Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value)) {
                            addresses.Add(key, value);
                        }
                    }
                }
            }
            return addresses;
        }

        private static UInt32 baseAddress;
        private static IntPtr handle = new IntPtr(0);
        private static int processID = -1;
        public static void SetProcess(Process process) {
            if (processID == process.Id) return;

            try {
                MemoryReader.baseAddress = (UInt32)process.MainModule.BaseAddress.ToInt32();
                MemoryReader.handle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
                MemoryReader.processID = process.Id;
            } catch {

            }
        }

        private static void UpdateAttributes() {
            bool attributesChanged =  ReadHealth();
            attributesChanged |= ReadMaxHealth();
            attributesChanged |= ReadMana();
            attributesChanged |= ReadMaxMana();
            attributesChanged |= ReadExperience();
            attributesChanged |= ReadLevel();
            attributesChanged |= ReadMagicLevel();

            if (attributesChanged && AttributesChanged != null) {
                var attributes = new PlayerAttributes
                {
                    Health = health,
                    MaxHealth = maxHealth,
                    Mana = mana,
                    MaxMana = maxMana,
                    Level = level,
                    Experience = experience,
                    MagicLevel = magicLevel
                };

                AttributesChanged(null, attributes);
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

        private static int XOR {
            get {
                return ReadInt32(GetAddress(XORAddress));
            }
        }

        private static int health;

        private static bool ReadHealth() {
            int currentHealth = ReadInt32(GetAddress(HealthAddress)) ^ XOR;
            bool healthChanged = currentHealth == health;
            health = currentHealth;

            return healthChanged;
        }

        private static int maxHealth;

        private static bool ReadMaxHealth() {
            int currentMaxHealth = ReadInt32(GetAddress(MaxHealthAddress)) ^ XOR;
            bool maxHealthChanged = currentMaxHealth == maxHealth;
            maxHealth = currentMaxHealth;

            return maxHealthChanged;
        }

        private static int mana;

        private static bool ReadMana() {
            int currentMana = ReadInt32(GetAddress(ManaAddress)) ^ XOR;
            bool manaChanged = currentMana == mana;
            mana = currentMana;

            return manaChanged;
        }

        private static int maxMana;

        private static bool ReadMaxMana() {
            int currentMaxMana = ReadInt32(GetAddress(MaxManaAddress)) ^ XOR;
            bool maxManaChanged = currentMaxMana == maxMana;
            maxMana = currentMaxMana;

            return maxManaChanged;
        }

        public static int PlayerId {
            get {
                return ReadInt32(GetAddress(PlayerIDAddress));
            }
        }

        private static int playerAddress = -1;
        
        public static string PlayerName {
            get {
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

        private static long experience;

        private static bool ReadExperience() {
            long currentExperience = ReadInt64(GetAddress(ExperienceAddress));
            bool experienceChanged = currentExperience == experience;
            experience = currentExperience;

            return experienceChanged;
        }

        private static int level;

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
                if (battleList[battlelistentry].id == id) {
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


        public static int GetHealthPercentage(int id, ref int battlelistentry) {
            lock (battleList) {
                if (battleList[battlelistentry].id == id) {
                    return battleList[battlelistentry].hp;
                }

                for (int i = 0; i < battleList.Length; i++) {
                    if (battleList[i].id == id) {
                        battlelistentry = i;
                        return battleList[i].hp;
                    }
                }
                return -1;
            }
        }
    }

    class BattleListEntry {
        public int id;
        public string name;
        public int x;
        public int y;
        public int z;
        public int hp;
    }
}
