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
    public class ItemSold {
        public int npcid;
        public int itemid;
        public int price;
    }

    public class Transport : TibiaObject {
        public string destination;
        public int cost;
        public string notes;

        public override string GetName() { return notes.Length > 0 ? notes : "Travel to " + destination + " for " + cost + " gold."; }
        public override Image GetImage() { return null; }

        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(destination, 120), new StringAttribute(cost.ToString(), 50), new StringAttribute(notes, 180) };
        }
        public override string GetCommand() {
            return "city@" + destination.ToLower();
        }
        static List<string> headers = new List<string> { "Destination", "Cost", "Notes" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return destination;
            }
            if (header == hashes[1]) {
                return cost;
            }
            if (header == hashes[2]) {
                return notes;
            }
            return base.GetHeaderValue(header);
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
        public List<Quest> involvedQuests;
        public List<Transport> transportOffered;

        public NPC() {
            sellItems = new List<ItemSold>();
            buyItems = new List<ItemSold>();
            spellsTaught = new List<SpellTaught>();
            involvedQuests = new List<Quest>();
            transportOffered = new List<Transport>();
            pos = new Coordinate();
            image = null;
        }
        public override NPC AsNPC() { return this; }
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

}
