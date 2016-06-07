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
    public class Achievement : TibiaObject {
        public int id;
        public string name = "Unknown";
        public int grade = 1;
        public int points = 1;
        public string description = "";
        public string spoiler = "";
        public int image = -1;
        public int imagetype = -1;
                
        public override string GetName() { return name; }
        public override Image GetImage() {
            TibiaObject obj = null;
            switch(imagetype) {
                case 1: obj = StorageManager.getOutfit(image); break;
                case 2: obj = StorageManager.getMount(image); break;
                case 3: obj = StorageManager.getQuest(image); break;
                case 4: obj = StorageManager.getHunt(image); break;
                case 5: obj = StorageManager.getCreature(image); break;
                case 6: obj = StorageManager.getNPC(image); break;
                case 7: obj = StorageManager.getItem(image); break;
                case 8: obj = StorageManager.getSpell(image); break;
            }
            if (obj != null) {
                Image img = obj.GetImage();
                if (img != null) return img;
            }
            switch(grade) {
                case 4: return StyleManager.GetImage("achievementgrade4.png");
                case 3: return StyleManager.GetImage("achievementgrade3.png");
                case 2: return StyleManager.GetImage("achievementgrade2.png");
                default: return StyleManager.GetImage("achievementgrade1.png");
            }
        }

        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 200), new StringAttribute(points > 0 ? points.ToString() : "-", 60), new StringAttribute(grade > 0 ? grade.ToString() : "-", 60) };
        }
        public override string GetCommand() {
            return "achievement" + Constants.CommandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "Points", "Grade" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                return points;
            }
            if (header == hashes[2]) {
                return grade;
            }
            return base.GetHeaderValue(header);
        }
    }
}
