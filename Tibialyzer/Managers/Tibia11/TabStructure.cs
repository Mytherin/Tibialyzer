using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class TabStructure {
        public UInt32 address;
        public int size = 0;
        public int prevsize = 0;

        public TabStructure(UInt32 address) {
            this.address = address;
        }

        public IEnumerable<string> GetMessages() {
            int maxsize = MemoryReader.ReadInt32(address + 8);
            int size = MemoryReader.ReadInt32(address + 16);
            if (size < 0 || size > maxsize || maxsize > 1200) {
                yield break;
            }
            for (int i = 0; i < maxsize; i++) {
                string msg = GetMessage(i);
                if (msg != null) {
                    yield return msg;
                }
            }
            yield break;
        }

        public string GetMessage(int i) {
            int chatMessages = MemoryReader.ReadInt32(address + 4);
            int chatMessage = MemoryReader.ReadInt32(chatMessages + 4 * i);
            return MemoryReader.ReadChatMessage(chatMessage);
        }
    }

    static class TabStructureScanner {
        private static bool IsPowerOfTwo(int x) {
            return (x & (x - 1)) == 0;
        }

        public static List<TabStructure> FindTabStructures(Process p) {
            bool firstScan = ReadMemoryManager.TabStructureCount() == 0;
            List<TabStructure> structs = new List<TabStructure>();
            int statsValue = -1;
            if (MemoryReader.MemorySettings.ContainsKey("tibia11statsvalue")) {
                int.TryParse(MemoryReader.MemorySettings["tibia11statsvalue"], out statsValue);
            }
            foreach (var tpl in ReadMemoryManager.ScanProcess(p)) {
                int length = tpl.Item1.RegionSize;
                int baseAddress = tpl.Item1.BaseAddress;
                byte[] bytes = tpl.Item2;
                for (int i = 0; i < length - 20; i += 4) {
                    int value = BitConverter.ToInt32(bytes, i);

                    if (value > 0x40000 && value != baseAddress + i) {
                        int messageptr = BitConverter.ToInt32(bytes, i + 4);
                        int maxsize = BitConverter.ToInt32(bytes, i + 8);
                        int size = BitConverter.ToInt32(bytes, i + 16);
                        if (messageptr > 0x40000 && IsPowerOfTwo(maxsize) && size < maxsize && maxsize < 10000 && size >= 0 && maxsize > 0) {
                            if ((baseAddress + i) == MemoryReader.ReadInt32(value)) {
                                structs.Add(new TabStructure((uint)(baseAddress + i)));
                            }
                        }
                    }
                    if (statsValue != -1) {
                        if (value == statsValue) {
                            int health = BitConverter.ToInt32(bytes, i + 0x24);
                            int maxhealth = BitConverter.ToInt32(bytes, i + 0x28);
                            int mana = BitConverter.ToInt32(bytes, i + 0x2C);
                            int maxmana = BitConverter.ToInt32(bytes, i + 0x30);
                            if (health <= maxhealth && mana <= maxmana && maxhealth < 30000 && maxmana < 60000 &&
                                health >= 0 && maxhealth > 0 && mana >= 0 && maxmana > 0) {
                                MemoryReader.SetStatsAddress((uint)(baseAddress + i));
                            }
                        }
                    }
                }
                if (!firstScan) {
                    System.Threading.Thread.Sleep(10);
                }
            }
            return structs;
        }
    }
}
