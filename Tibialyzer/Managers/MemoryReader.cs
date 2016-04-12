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

namespace Tibialyzer {
    class MemoryReader {
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

        private static void ParseAddress(string setting, out UInt32 memoryAddress, UInt32 defaultValue) {
            memoryAddress = defaultValue;
            string hexString = SettingsManager.getSettingString(setting);
            if (hexString != null) {
                UInt32.TryParse(hexString.Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out memoryAddress);
            }
            InitializeBattleList();
        }

        public static void Initialize() {
            ParseAddress("XORAddress", out XORAddress, 0x534658);
            ParseAddress("HealthAddress", out HealthAddress, 0x6d2030);
            ParseAddress("MaxHealthAddress", out MaxHealthAddress, 0x6D2024);
            ParseAddress("ManaAddress", out ManaAddress, 0x534688);
            ParseAddress("MaxManaAddress", out MaxManaAddress, 0x53465C);
            ParseAddress("PlayerIDAddress", out PlayerIDAddress, 0x6D202C);
            ParseAddress("BattleListAddress", out BattleListAddress, 0x72DE20);
            ParseAddress("ExperienceAddress", out ExperienceAddress, 0x534660);
            ParseAddress("LevelAddress", out LevelAddress, 0x534670);
            ParseAddress("MagicLevelAddress", out MagicLevelAddress, 0x534678);
            ParseAddress("TabsBaseAddress", out TabsBaseAddress, 0x534970);
            ParseAddress("AmmunitionCountAddress", out AmmunitionCountAddress, 0x773E8C);
            ParseAddress("AmmunitionTypeAddress", out AmmunitionTypeAddress, 0x773E8C + 4);
            ParseAddress("WeaponCountAddress", out WeaponCountAddress, 0x773F0C);
            ParseAddress("WeaponTypeAddress", out WeaponTypeAddress, 0x773F0C + 4);
            ParseAddress("BootsTypeAddress", out BootsTypeAddress, 0x773ED0);
            ParseAddress("RingTypeAddress", out RingTypeAddress, 0x773EB0);
        }

        private static UInt32 baseAddress;
        private static IntPtr handle = new IntPtr(0);
        private static int processID = -1;
        public static void SetProcess(Process process) {
            if (processID == process.Id) return;

            MemoryReader.processID = process.Id;
            MemoryReader.handle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
            MemoryReader.baseAddress = (UInt32)process.MainModule.BaseAddress.ToInt32();
        }


        public static UInt32 GetAddress(UInt32 offset) {
            return baseAddress + offset;
        }

        public static byte[] ReadBytes(Int64 address, uint n, int processHandle = -1) {
            IntPtr ptrBytesRead;
            byte[] buf = new byte[n];
            ReadProcessMemory(processHandle < 0 ? handle : new IntPtr(processHandle), new IntPtr(address), buf, n, out ptrBytesRead);
            return buf;
        }

        public static Int32 ReadInt32(Int64 address, int handle = -1) {
            return BitConverter.ToInt32(ReadBytes(address, 4, handle), 0);
        }

        public static UInt32 ReadUInt32(Int64 address, int handle = -1) {
            return BitConverter.ToUInt32(ReadBytes(address, 4, handle), 0);
        }

        public static string ReadString(Int64 address, uint length = 32, int handle = -1) {
            byte[] bytes = ReadBytes(address, length, handle);
            string str = ASCIIEncoding.Default.GetString(bytes);
            return str.Split('\0')[0];
        }

        public static int XOR {
            get {
                return ReadInt32(GetAddress(XORAddress));
            }
        }

        public static int Health {
            get {
                return ReadInt32(GetAddress(HealthAddress)) ^ XOR;
            }
        }

        public static int MaxHealth {
            get {
                return ReadInt32(GetAddress(MaxHealthAddress)) ^ XOR;
            }
        }

        public static int Mana {
            get {
                return ReadInt32(GetAddress(ManaAddress)) ^ XOR;
            }
        }

        public static int MaxMana {
            get {
                return ReadInt32(GetAddress(MaxManaAddress)) ^ XOR;
            }
        }

        public static int PlayerId {
            get {
                return ReadInt32(GetAddress(PlayerIDAddress));
            }
        }

        public string PlayerName {
            get {
                return ReadString(findPlayerInBattleList() + 4);
            }
        }

        public static int X {
            get {
                return ReadInt32(findPlayerInBattleList() + BL_X_OFFSET) - START_X;
            }
        }

        public static int Y {
            get {
                return ReadInt32(findPlayerInBattleList() + BL_Y_OFFSET) - START_Y;
            }
        }

        public static int Z {
            get {
                return ReadInt32(findPlayerInBattleList() + BL_Z_OFFSET);
            }
        }

        public static int Experience {
            get {
                return ReadInt32(GetAddress(ExperienceAddress));
            }
        }

        public static int Level {
            get {
                return ReadInt32(GetAddress(LevelAddress));
            }
        }

        public static int MagicLevel {
            get {
                return ReadInt32(GetAddress(MagicLevelAddress));
            }
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

        private static UInt32 findPlayerInBattleList() {
            UInt32 creature = GetAddress(BattleListAddress);
            while (ReadInt32(creature) != PlayerId) {
                creature += BL_CREATURE_SIZE;
            }
            return creature;
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
