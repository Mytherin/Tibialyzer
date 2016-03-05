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
using System.Drawing;

namespace Tibialyzer {
    public class Skin {
        public int dropitemid;
        public int skinitemid;
        public float percentage;
    }

    public class ItemDrop {
        public int creatureid;
        public int itemid;
        public float percentage;
        public int min;
        public int max;
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
        public bool boss;
        public Image image;
        public List<ItemDrop> itemdrops;
        public Skin skin;

        public Creature() {
            displayname = "Unknown";
            image = null;
            skin = null;
            itemdrops = new List<ItemDrop>();
        }
        public override Creature AsCreature() { return this; }

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

        public string GetStrength() {
            string weakness = "";
            int minDamage = int.MaxValue;
            for (int i = 0; i <= 6; i++) {
                if (GetResistance(i) < minDamage) {
                    minDamage = GetResistance(i);
                    weakness = GetResistanceType(i);
                }
            }
            return weakness;
        }

        public override string GetName() { return title; }
        public override Image GetImage() {
            if (image == null) {
                StorageManager.loadCreatureImage(id);
            }
            return image;
        }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(title, 200, boss ? StyleManager.CreatureBossColor : StyleManager.NotificationTextColor), new StringAttribute(experience > 0 ? experience.ToString() : "-", 60), new StringAttribute(health > 0 ? health.ToString() : "-", 60, StyleManager.CreatureHealthColor),
            new StringAttribute(GetWeakness(), 60, StyleManager.GetElementColor(GetWeakness())) };
        }
        public override string GetCommand() {
            return "creature" + Constants.CommandSymbol + title;
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
}
