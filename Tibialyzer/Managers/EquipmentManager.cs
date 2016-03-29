using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class EquipmentManager {
        public static Item currentAmmunition;
        public static int ammunitionCount;

        public static void UpdateUsedItems() {
            int newCount = MemoryReader.AmmunitionCount;
            int ammunitionID = MemoryReader.AmmunitionType;
            Item item = StorageManager.getItemFromTibiaID(ammunitionID);
            if (newCount > 0) {
                if (newCount < ammunitionCount && item != null && currentAmmunition == item) {
                    int waste = ammunitionCount - newCount;
                    HuntManager.AddUsedItems(HuntManager.activeHunt, currentAmmunition, waste);
                }
                currentAmmunition = item;
                ammunitionCount = newCount;
            }
        }
    }
}
