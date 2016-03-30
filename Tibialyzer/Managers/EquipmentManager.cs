using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class EquipmentManager {
        public static Item currentAmmunition = null;
        public static int ammunitionCount = 0;
        public static Item currentWeapon = null;
        public static int weaponCount = 0;

        public static void UpdateUsedItems() {
            // Update the used items based on the equipment currently equiped
            // Update used ammunition
            // First, check the currently equipped ammunition (if any) and how much ammunition is equipped
            int newCount = MemoryReader.AmmunitionCount;
            int ammunitionID = MemoryReader.AmmunitionType;
            Item item = StorageManager.getItemFromTibiaID(ammunitionID);
            if (newCount > 0) {
                //if there is any ammunition equipped, and the current ammunition count is smaller than the previous ammunition count
                //AND the same ammunition type is equipped; then ammunition has been fired
                if (newCount < ammunitionCount && item != null && currentAmmunition == item) {
                    // add the used ammunition to the used items of the current hunt
                    int waste = ammunitionCount - newCount;
                    HuntManager.AddUsedItems(HuntManager.activeHunt, currentAmmunition, waste);
                }
                currentAmmunition = item;
                ammunitionCount = newCount;
            }
            // do the same for the weapon slot, for throwing weapons/spears
            newCount = MemoryReader.WeaponCount;
            int weaponID = MemoryReader.WeaponType;
            Item weapon = StorageManager.getItemFromTibiaID(weaponID);
            if (newCount > 0) {
                if (newCount < weaponCount && weapon != null && currentWeapon == weapon) {
                    int waste = weaponCount - newCount;
                    HuntManager.AddUsedItems(HuntManager.activeHunt, currentWeapon, waste);
                }
                currentWeapon = weapon;
                weaponCount = newCount;
            }
        }
    }
}
