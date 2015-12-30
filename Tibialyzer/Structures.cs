using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tibialyzer {
    public abstract class TibiaObject : IDisposable {
        public abstract string GetName();
        public abstract Image GetImage();
        public virtual void Dispose() {

        }
    }

    public class Command {
        public string player;
        public string command;
    }

    public class Coordinate {
        public int x;
        public int y;
        public int z;

        public const int MaxWidth = 2048;
        public const int MaxHeight = 2304;

        public Coordinate() {
            this.x = MaxWidth / 2;
            this.y = MaxHeight / 2;
            this.z = 7;
        }

        public Coordinate(Coordinate coordinate) {
            this.x = coordinate.x;
            this.y = coordinate.y;
            this.z = coordinate.z;
        }

        public Coordinate(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct SpellTaught {
        public Spell spell;
        public NPC npc;
        public bool knight;
        public bool druid;
        public bool paladin;
        public bool sorcerer;
            
        public bool GetVocation(int i) {
            if (i == 0) return knight;
            if (i == 1) return druid;
            if (i == 2) return paladin;
            if (i == 3) return sorcerer;
            throw new Exception("Unsupported index");
        }
    }

    public class Outfit : TibiaObject {
        public int id;
        public string title;
        public string name;
        public bool premium;
        public Image[] maleImages = new Image[4];
        public Image[] femaleImages = new Image[4];
        public Quest quest;
        
        public override string GetName() { return name; }
        public override Image GetImage() {
            for(int i = 3; i >= 0; i--) {
                if (maleImages[i] != null) return maleImages[i];
                if (femaleImages[i] != null) return femaleImages[i];
            }
            throw new Exception("Outfit without image");
        }
    }

    public class Mount : TibiaObject {
        public int id;
        public string title;
        public string name;
        public Item tameitem;
        public Creature tamecreature;
        public int speed;
        public bool tibiastore;
        public Image image;

        public Mount() {
            tameitem = null;
            tamecreature = null;
        }

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
    }

    public class QuestInstruction {
        public Quest quest;
        public Coordinate begin;
        public Coordinate end;
        public string description;
        public int ordering;
    }

    public class Quest {
        public int id;
        public string title;
        public string name;
        public int minlevel;
        public bool premium;
        public string city;
        public string legend;
        public List<Outfit> rewardOutfits;
        public List<Item> rewardItems;
        public List<Creature> questDangers;
        public List<Tuple<int,Item>> questRequirements;
        public List<string> additionalRequirements;
        public Dictionary<string, List<QuestInstruction>> questInstructions;

        public Quest() {
            rewardOutfits = new List<Outfit>();
            rewardItems = new List<Item>();
            questDangers = new List<Creature>();
            questRequirements = new List<Tuple<int, Item>>();
            additionalRequirements = new List<string>();
            questInstructions = new Dictionary<string, List<QuestInstruction>>();
        }
    }

    public class Spell : TibiaObject {
        public int id;
        public string name;
        public string words;
        public string element;
        public int cooldown;
        public bool premium;
        public bool promotion;
        public int levelrequired;
        public int goldcost;
        public int manacost;
        public bool knight;
        public bool paladin;
        public bool sorcerer;
        public bool druid;
        public Image image;

        public List<SpellTaught> teachNPCs = new List<SpellTaught>();
        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
    }

    public class Directions {
        public int huntingplaceid;
        public Coordinate begin;
        public Coordinate end;
        public int ordering;
        public string description;
    }

    public class Requirements {
        public int huntingplaceid;
        public Quest quest;
        public string notes;
    }
    
    public class HuntingPlace : TibiaObject {
        public int id;
        public string name;
        public string city;
        public int level;
        public int exp_quality;
        public int loot_quality;
        public Image image;
        public List<Creature> creatures;
        public List<Coordinate> coordinates;
        public List<Directions> directions;
        public List<Requirements> requirements;

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }

        public HuntingPlace() {
            this.creatures = new List<Creature>();
            this.coordinates = new List<Coordinate>();
            this.directions = new List<Directions>();
            this.requirements = new List<Requirements>();
            this.image = null;
        }

        public void Dispose() {
            if (image != null) image.Dispose();
            if (creatures != null)
                foreach (Creature creature in creatures)
                    creature.Dispose();
        }
    }

    public class NPC : TibiaObject {
        public int id;
        public string name;
        public string city;
        public string job;
        public Coordinate pos;
        public Image image;
        public int value;
        public List<ItemSold> buyItems;
        public List<ItemSold> sellItems;
        public List<SpellTaught> spellsTaught;

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }

        public NPC() {
            sellItems = new List<ItemSold>();
            buyItems = new List<ItemSold>();
            spellsTaught = new List<SpellTaught>();
            pos = new Coordinate();
            image = null;
        }

        public void Dispose() {
            if (image != null) image.Dispose();
        }
    }

    public class Item : TibiaObject {
        public int id;
        public string name;
        public string title;
        public int vendor_value;
        public int actual_value;
        public float capacity;
        public bool stackable;
        public Image image;
        public string category;
        public bool discard;
        public bool convert_to_gold;
        public string look_text;
        public int currency;
        public List<ItemDrop> itemdrops;
        public List<ItemSold> buyItems;
        public List<ItemSold> sellItems;

        public int GetMaxValue() {
            return Math.Max(vendor_value, actual_value);
        }

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
        public Item() {
            name = "Unknown";
            image = null;
            itemdrops = new List<ItemDrop>();
            sellItems = new List<ItemSold>();
            buyItems = new List<ItemSold>();
        }

        public void Dispose() {
            if (image != null) image.Dispose();
        }
    }

    public class Map {
        public Bitmap image;
        public int z;
    }

    public class Skin : IDisposable {
        public Item drop_item;
        public Item skin_item;
        public float percentage;
        public Skin() {
            drop_item = null;
            skin_item = null;
        }

        public void Dispose() {
            if (drop_item != null) drop_item.Dispose();
            if (skin_item != null) skin_item.Dispose();
        }
    }

    public class ItemDrop {
        public Creature creature;
        public Item item;
        public float percentage;
    }

    public class ItemSold {
        public NPC npc;
        public Item item;
        public int price;
    }

    public class Creature : TibiaObject {
        public string name;
        public string title;
        public int id;
        public int health;
        public int experience;
        public int maxdamage;
        public int summoncost;
        public bool illusionable;
        public bool pushable;
        public bool pushes;
        public int res_phys;
        public int res_holy;
        public int res_death;
        public int res_fire;
        public int res_energy;
        public int res_ice;
        public int res_earth;
        public int res_drown;
        public int res_lifedrain;
        public bool paralysable;
        public bool senseinvis;
        public string abilities;
        public int armor;
        public int speed;
        public Image image;
        public List<ItemDrop> itemdrops;
        public Skin skin;

        public Creature() {
            name = "Unknown";
            image = null;
            skin = null;
            itemdrops = new List<ItemDrop>();
        }

        public void Dispose() {
            if (image != null) image.Dispose();
            if (skin != null) {
                skin.Dispose();
            }
            if (itemdrops != null) {
                foreach (ItemDrop drop in itemdrops)
                    if (drop.item != null)
                        drop.item.Dispose();
            }
        }

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
    }
}
