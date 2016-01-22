using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tibialyzer {
    public abstract class TibiaObject {
        public bool permanent;
        public abstract string GetName();
        public abstract Image GetImage();
        public abstract List<string> GetAttributeHeaders();
        public abstract List<Attribute> GetAttributes();
        public abstract string GetCommand();
        public virtual IComparable GetHeaderValue(int hash) {
            return "";
        }
    }

    public class Command {
        public string player;
        public string command;
    }

    public abstract class Attribute {
        public int MaxWidth;
    };

    public class StringAttribute : Attribute {
        public string value;
        public Color color;
        public StringAttribute(string value, int MaxWidth) {
            this.value = value;
            this.MaxWidth = MaxWidth;
            this.color = MainForm.label_text_color;
        }
        public StringAttribute(string value, int MaxWidth, Color color) {
            this.value = value;
            this.MaxWidth = MaxWidth;
            this.color = color;
        }
    }

    public class ImageAttribute : Attribute {
        public Image value;
        public ImageAttribute(Image value, int MaxWidth = 100) {
            this.value = value;
            this.MaxWidth = MaxWidth;
        }
    }
    public class BooleanAttribute : Attribute {
        public bool value;
        public BooleanAttribute(bool value, int MaxWidth = 100) {
            this.value = value;
            this.MaxWidth = MaxWidth;
        }
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
        public int spellid;
        public int npcid;
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

    public class WorldObject : TibiaObject {
        public string title;
        public string name;
        public Image image;

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
        public override List<string> GetAttributeHeaders() {
            return new List<string> { "Name" };
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 120) };
        }
        public override string GetCommand() {
            return "worldobject" + MainForm.commandSymbol + name;
        }
    }

    public class Outfit : TibiaObject {
        public int id;
        public string title;
        public string name;
        public bool premium;
        public bool tibiastore;
        public Image[] maleImages = new Image[4];
        public Image[] femaleImages = new Image[4];
        public int questid;

        public override string GetName() { return name; }
        public override Image GetImage() {
            for (int i = 3; i >= 0; i--) {
                if (maleImages[i] != null) return maleImages[i];
                if (femaleImages[i] != null) return femaleImages[i];
            }
            throw new Exception("Outfit without image");
        }
        public override List<string> GetAttributeHeaders() {
            return new List<string> { "Name", "Prem", "Store", "Quest Name" };
        }
        public override List<Attribute> GetAttributes() {
            Quest q = MainForm.getQuest(questid);
            return new List<Attribute> { new StringAttribute(name, 140), new BooleanAttribute(premium), new BooleanAttribute(tibiastore), new StringAttribute(q == null ? "-" : q.name, 100) };
        }
        public override string GetCommand() {
            return "outfit" + MainForm.commandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "Prem", "Store", "Quest Name" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int hash) {
            if (hash == hashes[0]) {
                return name;
            }
            if (hash == hashes[1]) {
                return premium;
            }
            if (hash == hashes[2]) {
                return tibiastore;
            }
            if (hash == hashes[3]) {
                Quest q = MainForm.getQuest(questid);
                if (q == null) return "";
                return q.name;
            }
            return base.GetHeaderValue(hash);
        }
    }

    public class Mount : TibiaObject {
        public int id;
        public string title;
        public string name;
        public int tameitemid;
        public int tamecreatureid;
        public int speed;
        public bool tibiastore;
        public Image image;

        public Mount() {
        }

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            Item i = MainForm.getItem(tameitemid);
            Creature cr = MainForm.getCreature(tamecreatureid);
            string itemName = i == null ? "-" : MainForm.ToTitle(i.displayname);
            string creatureName = cr == null ? "-" : MainForm.ToTitle(cr.displayname);
            return new List<Attribute> { new StringAttribute(name, 120), new StringAttribute(itemName, 80), new StringAttribute(creatureName, 80), new BooleanAttribute(tibiastore) };
        }
        public override string GetCommand() {
            return "mount" + MainForm.commandSymbol + title;
        }
        static List<string> headers = new List<string> { "Name", "Tame", "Creature", "Store" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                Item i = MainForm.getItem(tameitemid);
                if (i == null) return "";
                return i.displayname;
            }
            if (header == hashes[2]) {
                Creature cr = MainForm.getCreature(tamecreatureid);
                if (cr == null) return "";
                return cr.displayname;
            }
            if (header == hashes[3]) {
                return tibiastore;
            }
            return base.GetHeaderValue(header);
        }
    }

    public class QuestInstruction {
        public int questid;
        public Coordinate begin;
        public Coordinate end;
        public string description;
        public string settings;
        public int ordering;
    }

    public class Utility {
        public string name;
        public Coordinate location;
    }

    public class City {
        public int id;
        public string name;
        public Coordinate location;
        public List<Utility> utilities;
        public City() {
            utilities = new List<Utility>();
        }
    }

    public class Quest : TibiaObject {
        public int id;
        public string title;
        public string name;
        public int minlevel;
        public bool premium;
        public string city;
        public string legend;
        public List<int> rewardOutfits;
        public List<int> rewardItems;
        public List<int> questDangers;
        public List<Tuple<int, int>> questRequirements;
        public List<string> additionalRequirements;
        public Dictionary<string, List<QuestInstruction>> questInstructions;

        public Quest() {
            rewardOutfits = new List<int>();
            rewardItems = new List<int>();
            questDangers = new List<int>();
            questRequirements = new List<Tuple<int, int>>();
            additionalRequirements = new List<string>();
            questInstructions = new Dictionary<string, List<QuestInstruction>>();
        }

        public Item GetRewardItem() {
            if (this.rewardItems.Count > 0) {
                List<Item> items = new List<Item>();
                foreach (int i in this.rewardItems) {
                    Item item = MainForm.getItem(i, false);
                    items.Add(item);
                }
                return items.OrderByDescending(o => o.GetMaxValue()).First();
            }
            return null;
        }

        public Image GetRewardImage() {
            if (this.rewardOutfits.Count > 0) {
                Outfit o = MainForm.getOutfit(this.rewardOutfits[0]);
                return o.GetImage();
            } else if (this.rewardItems.Count > 0) {
                Item it = GetRewardItem();
                return it.GetImage();
            }
            return null;
        }

        public Creature GetDangerCreature() {
            if (this.questDangers.Count > 0) {
                List<Creature> creatures = new List<Creature>();
                foreach (int i in this.questDangers) {
                    Creature cr = MainForm.getCreature(i, false);
                    creatures.Add(cr);
                }
                return creatures.OrderByDescending(o => o.experience).First();
            }
            return null;
        }

        public Image GetDangerImage() {
            Creature cr = GetDangerCreature();
            if (cr != null) return cr.GetImage();
            return null;
        }

        public override string GetName() { return name; }
        public override Image GetImage() {
            Image image = GetRewardImage();
            if (image == null) image = GetDangerImage();
            return image;
        }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 160), new StringAttribute(minlevel.ToString(), 40), new StringAttribute(MainForm.ToTitle(city), 100), new ImageAttribute(GetDangerImage()) };
        }
        public override string GetCommand() {
            return "quest" + MainForm.commandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "Level", "City", "Danger" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                return minlevel;
            }
            if (header == hashes[2]) {
                return city;
            }
            if (header == hashes[3]) {
                Creature cr = GetDangerCreature();
                return cr == null ? 0 : cr.experience;
            }
            return base.GetHeaderValue(header);
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

        private static Color ManaCostColor = Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(180)))), ((int)(((byte)(176)))));

        public List<SpellTaught> teachNPCs = new List<SpellTaught>();
        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 140), new StringAttribute(words, 100), new StringAttribute(manacost >= 0 ? manacost.ToString() : "-", 40, ManaCostColor), new StringAttribute(levelrequired > 0 ? levelrequired.ToString() : "-", 50) };
        }
        public override string GetCommand() {
            return "spell" + MainForm.commandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "Words", "Mana", "Level" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                return words;
            }
            if (header == hashes[2]) {
                return manacost;
            }
            if (header == hashes[3]) {
                return levelrequired;
            }
            return base.GetHeaderValue(header);
        }
    }

    public class Directions {
        public int huntingplaceid;
        public Coordinate begin;
        public Coordinate end;
        public int ordering;
        public string description;
        public string settings;
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
        public List<int> creatures;
        public List<Coordinate> coordinates;
        public List<Directions> directions;
        public List<Requirements> requirements;

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }

        public HuntingPlace() {
            this.creatures = new List<int>();
            this.coordinates = new List<Coordinate>();
            this.directions = new List<Directions>();
            this.requirements = new List<Requirements>();
            this.image = null;
        }
        public override List<string> GetAttributeHeaders() {
            return new List<string> { "Name", "Level", "Exp", "Loot", "City" };
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 120), new StringAttribute(level < 0 ? "-" : level.ToString(), 40),
                new ImageAttribute(MainForm.star_image_text[exp_quality]), new ImageAttribute(MainForm.star_image_text[loot_quality]),
                new StringAttribute(city, 60) };
        }
        public override string GetCommand() {
            return "hunt" + MainForm.commandSymbol + name;
        }
    }

    public class NPC : TibiaObject {
        public int id;
        public string name;
        public string city;
        public string job;
        public Coordinate pos;
        public Image image;
        public List<ItemSold> buyItems;
        public List<ItemSold> sellItems;
        public List<SpellTaught> spellsTaught;

        public NPC() {
            sellItems = new List<ItemSold>();
            buyItems = new List<ItemSold>();
            spellsTaught = new List<SpellTaught>();
            pos = new Coordinate();
            image = null;
        }
        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
        public override List<string> GetAttributeHeaders() {
            return new List<string> { "Name", "City" };
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 120),
                new StringAttribute(MainForm.ToTitle(city), 80) };
        }
        public override string GetCommand() {
            return "npc" + MainForm.commandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "City" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                return city;
            }
            return base.GetHeaderValue(header);
        }
    }

    public class Item : TibiaObject {
        public int id;
        public string displayname;
        public string title;
        public long vendor_value;
        public long actual_value;
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
        public List<Quest> rewardedBy;

        public long GetMaxValue() {
            return Math.Max(vendor_value, actual_value);
        }

        public string GetMaxValueString() {
            long val = GetMaxValue();
            double resval = val;
            string denom = "";
            if (val >= 1000) {
                resval = val / 1000.0;
                denom = "K";
            }
            if (resval >= 1000) {
                resval = resval / 1000.0;
                denom = "M";
            }
            if (resval >= 1000) {
                resval = resval / 1000.0;
                denom = "B";
            }
            return String.Format("{0:0.#}", resval) + denom;
        }

        public static Color GoldColor = Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(226)))), ((int)(((byte)(24)))));
        public override string GetName() { return title; }
        public override Image GetImage() {
            if (image == null) {
                MainForm.loadItemImage(id);
            }
            return image;
        }
        public Item() {
            displayname = "Unknown";
            image = null;
            itemdrops = new List<ItemDrop>();
            sellItems = new List<ItemSold>();
            buyItems = new List<ItemSold>();
            rewardedBy = new List<Quest>();
        }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(title, 170),
                new StringAttribute(GetMaxValue() > 0 ? GetMaxValueString() : "-", 50, GoldColor), new StringAttribute(capacity > 0 ? String.Format("{0:0.0} oz.", capacity) : "-", 70) };
        }
        public override string GetCommand() {
            return "item" + MainForm.commandSymbol + title;
        }
        static List<string> headers = new List<string> { "Name", "Value", "Cap" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return title;
            }
            if (header == hashes[1]) {
                return GetMaxValue();
            }
            if (header == hashes[2]) {
                return capacity;
            }
            return base.GetHeaderValue(header);
        }
    }

    public class Event {
        public int id;
        public string title;
        public string location;
        public List<string> eventMessages = new List<string>();
        public int creatureid;
    }

    public class Map : TibiaObject {
        public Bitmap image;
        public int z;
        public int references = 0;

        public Map() {
        }

        public override Image GetImage() {
            return image;
        }
        public override string GetName() {
            return z.ToString();
        }
        public void Dispose() {
            references--;
            if (references <= 0) {
                image.Dispose();
                image = null;
            }
        }
        public override List<string> GetAttributeHeaders() {
            return null;
        }
        public override List<Attribute> GetAttributes() {
            return null;
        }
        public override string GetCommand() {
            return null;
        }
    }

    public class Skin {
        public int dropitemid;
        public int skinitemid;
        public float percentage;
    }

    public class ItemDrop {
        public int creatureid;
        public int itemid;
        public float percentage;
    }

    public class ItemSold {
        public int npcid;
        public int itemid;
        public int price;
    }

    public class Creature : TibiaObject {
        public string displayname;
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
            displayname = "Unknown";
            image = null;
            skin = null;
            itemdrops = new List<ItemDrop>();
        }

        private static Color HealthColor = Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(179)))), ((int)(((byte)(60)))));

        public int GetResistance(int index) {
            if (index == 0) {
                return res_phys;
            }
            if (index == 1) {
                return res_holy;
            }
            if (index == 2) {
                return res_death;
            }
            if (index == 3) {
                return res_fire;
            }
            if (index == 4) {
                return res_energy;
            }
            if (index == 5) {
                return res_ice;
            }
            if (index == 6) {
                return res_earth;
            }
            throw new Exception("Index out of bounds");
        }

        public static string GetResistanceType(int index) {
            if (index == 0) {
                return "Phys";
            }
            if (index == 1) {
                return "Holy";
            }
            if (index == 2) {
                return "Death";
            }
            if (index == 3) {
                return "Fire";
            }
            if (index == 4) {
                return "Energy";
            }
            if (index == 5) {
                return "Ice";
            }
            if (index == 6) {
                return "Earth";
            }
            throw new Exception("Index out of bounds");

        }

        public string GetWeakness() {
            string weakness = "";
            int maxDamage = int.MinValue;
            for (int i = 0; i <= 6; i++) {
                if (GetResistance(i) > maxDamage) {
                    maxDamage = GetResistance(i);
                    weakness = GetResistanceType(i);
                }
            }
            return weakness;
        }

        public override string GetName() { return title; }
        public override Image GetImage() {
            if (image == null) {
                MainForm.loadCreatureImage(id);
            }
            return image;
        }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(title, 120), new StringAttribute(experience > 0 ? experience.ToString() : "-", 60), new StringAttribute(health > 0 ? health.ToString() : "-", 60, HealthColor),
            new StringAttribute(GetWeakness(), 60, CreatureStatsForm.resistance_colors[GetWeakness()]) };
        }
        public override string GetCommand() {
            return "creature" + MainForm.commandSymbol + title;
        }
        static List<string> headers = new List<string> { "Name", "Exp", "HP", "Weak" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return title;
            }
            if (header == hashes[1]) {
                return experience;
            }
            if (header == hashes[2]) {
                return health;
            }
            if (header == hashes[3]) {
                return GetWeakness();
            }
            return base.GetHeaderValue(header);
        }
    }

    enum TibiaObjectType { Creature, Item, NPC, Outfit, Mount, Spell };

    class LazyTibiaObject : TibiaObject {
        public int id;
        public TibiaObjectType type;
        private TibiaObject tibiaObject = null;
        public TibiaObject getTibiaObject() {
            if (tibiaObject == null) {
                switch (type) {
                    case TibiaObjectType.Creature:
                        tibiaObject = MainForm.getCreature(id, false);
                        break;
                    case TibiaObjectType.Item:
                        tibiaObject = MainForm.getItem(id, false);
                        break;
                    case TibiaObjectType.NPC:
                        tibiaObject = MainForm.getNPC(id);
                        break;
                    case TibiaObjectType.Mount:
                        tibiaObject = MainForm.getMount(id);
                        break;
                    case TibiaObjectType.Outfit:
                        tibiaObject = MainForm.getOutfit(id);
                        break;
                    case TibiaObjectType.Spell:
                        tibiaObject = MainForm.getSpell(id);
                        break;
                }
            }
            return tibiaObject;
        }


        public override Image GetImage() {
            return getTibiaObject().GetImage();
        }
        public override string GetName() {
            return getTibiaObject().GetName();
        }
        public override List<string> GetAttributeHeaders() {
            return getTibiaObject().GetAttributeHeaders();
        }
        public override List<Attribute> GetAttributes() {
            return getTibiaObject().GetAttributes();
        }
        public override string GetCommand() {
            return getTibiaObject().GetCommand();
        }
        public override IComparable GetHeaderValue(int header) {
            return getTibiaObject().GetHeaderValue(header);
        }
    }
}
