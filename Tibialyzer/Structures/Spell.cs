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

        public override Spell AsSpell() { return this; }

        private static Color ManaCostColor = Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(180)))), ((int)(((byte)(176)))));

        public List<SpellTaught> teachNPCs = new List<SpellTaught>();
        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 140), new StringAttribute(words, 100), new StringAttribute(manacost >= 0 ? manacost.ToString() : "-", 50, ManaCostColor), new StringAttribute(levelrequired > 0 ? levelrequired.ToString() : "-", 50) };
        }
        public override string GetCommand() {
            return "spell" + Constants.CommandSymbol + name;
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
}
