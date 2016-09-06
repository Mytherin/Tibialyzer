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
using System.Linq;
using System.Drawing;

namespace Tibialyzer {
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
        public int armor = -1;
        public int attack = -1;
        public int defense = -1;
        public string defensestr = "";
        public string flavor = "";
        public int level = -1;
        public string vocation = "";
        public string attrib = "";
        public int range = -1;
        public string type = "";
        public int atkmod = -1;
        public int hitmod = 0;

        public override Item AsItem() { return this; }

        public long GetMaxValue() {
            return actual_value;
        }

        public static string ConvertGoldToString(long val) {
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

        public string GetMaxValueString() {
            long val = GetMaxValue();
            return ConvertGoldToString(val);
        }

        public override string GetName() { return title; }
        public override Image GetImage() {
            if (image == null) {
                StorageManager.loadItemImage(id);
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

        public override List<string> GetConditionalHeaders() {
            List<string> newHeaders = new List<string>();
            newHeaders.Add(headers[0]);
            if (armor >= 0) newHeaders.Add("Arm");
            if (attack >= 0 || atkmod >= 0) newHeaders.Add("Atk");
            if (defense > 0) newHeaders.Add("Def");
            if (hitmod != 0) newHeaders.Add("Hit+");
            if (level >= 0) newHeaders.Add("Level");
            if (range >= 0) newHeaders.Add("Range");
            //if (vocation != "") newHeaders.Add("Voc");
            if (attrib != "") newHeaders.Add("Attrib");
            if (type != "") newHeaders.Add("Type");
            newHeaders.Add(headers[1]);
            newHeaders.Add(headers[2]);
            return newHeaders;
        }

        public override IComparable GetConditionalHeaderValue(string header) {
            if (header == "Arm") return armor;
            if (header == "Atk") return attack >= 0 ? attack : atkmod;
            if (header == "Def") return defense;
            if (header == "Hit+") return hitmod;
            if (header == "Level") return level;
            //if (header == "Voc") return vocation;
            if (header == "Attrib") return attrib;
            if (header == "Range") return range;
            if (header == "Name") return title;
            if (header == "Type") return type;
            if (header == "Value") return GetMaxValue();
            if (header == "Cap") return capacity;
            return "";
        }

        public override List<Attribute> GetConditionalAttributes() {
            List<Attribute> att = new List<Attribute>();
            List<Attribute> regularAttributes = GetAttributes();
            att.Add(new StringAttribute(title, 120));
            if (armor >= 0) att.Add(new StringAttribute(armor.ToString(), 50));
            if (attack >= 0) att.Add(new StringAttribute(attack.ToString(), 60));
            else if (atkmod >= 0) att.Add(new StringAttribute("+" + atkmod, 60));
            if (defense > 0) att.Add(new StringAttribute(defensestr, 60));
            if (hitmod != 0) att.Add(new StringAttribute(hitmod > 0 ? "+" + hitmod.ToString() : hitmod.ToString(), 50));
            if (level >= 0) att.Add(new StringAttribute(level.ToString(), 50));
            if (range >= 0) att.Add(new StringAttribute(range.ToString(), 60));
            //if (vocation != "") att.Add(new StringAttribute(vocation, 100));
            if (attrib != "") att.Add(new StringAttribute(attrib, 120));
            if (type != "") att.Add(new StringAttribute(type, 70, StyleManager.ElementExists(type) ? StyleManager.GetElementColor(type) : StyleManager.NotificationTextColor));
            att.Add(regularAttributes[1]);
            att.Add(regularAttributes[2]);
            return att;
        }

        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            List<Attribute> attributeList = new List<Attribute> { new StringAttribute(title, 170),
                new StringAttribute(GetMaxValue() > 0 ? GetMaxValueString() : "-", 50, StyleManager.ItemGoldColor), new StringAttribute(capacity > 0 ? String.Format("{0:0.0} oz.", capacity) : "-", 70),
                new CommandAttribute(category, "category" + Constants.CommandSymbol + category, 100)};
            return attributeList;
        }
        public override string GetCommand() {
            return "item" + Constants.CommandSymbol + title;
        }
        static List<string> headers = new List<string> { "Name", "Value", "Cap", "Category" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
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
            if (header == hashes[3]) {
                return category;
            }
            return base.GetHeaderValue(header);
        }
    }
}
