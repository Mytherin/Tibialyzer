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
        public override Mount AsMount() { return this; }

        public override string GetName() { return name; }
        public override Image GetImage() { return image; }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            Item i = StorageManager.getItem(tameitemid);
            Creature cr = StorageManager.getCreature(tamecreatureid);
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
                Item i = StorageManager.getItem(tameitemid);
                if (i == null) return "";
                return i.displayname;
            }
            if (header == hashes[2]) {
                Creature cr = StorageManager.getCreature(tamecreatureid);
                if (cr == null) return "";
                return cr.displayname;
            }
            if (header == hashes[3]) {
                return tibiastore;
            }
            return base.GetHeaderValue(header);
        }
    }

}
