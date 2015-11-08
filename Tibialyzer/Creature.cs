using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tibialyzer {
    public abstract class TibiaObject {
        public abstract string GetName();
        public abstract Image GetImage();
    }

    public class Command {
        public string player;
        public string command;
    }

    public class Coordinate {
        public float x;
        public float y;
        public int z;

        public Coordinate() {
            this.x = 0.5f;
            this.y = 0.5f;
            this.z = 7;
        }

        public Coordinate(Coordinate coordinate) {
            this.x = coordinate.x;
            this.y = coordinate.y;
            this.z = coordinate.z;
        }

        public Coordinate(float x, float y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class Directions {
        public int huntingplaceid;
        public float x;
        public float y;
        public int z;
        public int ordering;
        public string name;
        public string notes;
    }

    public class Requirements {
        public int huntingplaceid;
        public int questid;
        public string notes;
    }

    public class Quest {
        public int id;
        public string name;
        public int minlevel;
        public bool premium;
        public string city;
        public string url;
    }

    public class HuntingPlace : TibiaObject, IDisposable {
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

    public class NPC : TibiaObject, IDisposable {
        public int id;
        public string name;
        public string city;
        public Coordinate pos;
        public Image image;
        public int value;

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }

        public NPC() {
            pos = new Coordinate();
            image = null;
        }

        public void Dispose() {
            if (image != null) image.Dispose();
        }
    }

    public class Item : TibiaObject, IDisposable {
        public int id;
        public string name;
        public int vendor_value;
        public int actual_value;
        public float capacity;
        public bool stackable;
        public Image image;
        public string category;
        public bool discard;
        public bool convert_to_gold;
        public string look_text;
        public int drops = 0;
        public int current_npc_value = 0;

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }

        public void Dispose() {
            if (image != null) image.Dispose();
        }
    }

    public class Map {
        public Image image;
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
        public Item item;
        public float percentage;
    }

    public class Creature : TibiaObject, IDisposable {
        public string name;
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
        public Image image;
        public List<ItemDrop> itemdrops;
        public Skin skin;
        public int kills = 0;
        public float percentage;

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
