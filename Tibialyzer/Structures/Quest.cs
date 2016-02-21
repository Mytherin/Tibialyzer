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
    public class QuestInstruction {
        public int questid;
        public Coordinate begin;
        public Coordinate end;
        public string description;
        public string settings;
        public int ordering;
        public string specialCommand = null;
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
                    Item item = StorageManager.getItem(i);
                    items.Add(item);
                }
                return items.OrderByDescending(o => o.GetMaxValue()).First();
            }
            return null;
        }

        public Image GetRewardImage() {
            if (this.rewardOutfits.Count > 0) {
                Outfit o = StorageManager.getOutfit(this.rewardOutfits[0]);
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
                    Creature cr = StorageManager.getCreature(i);
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
            return new List<Attribute> { new StringAttribute(name, 140), new StringAttribute(minlevel.ToString(), 50), new StringAttribute(premium ? "Yes" : "No", 50, premium ? Color.SeaGreen : Color.RoyalBlue), new StringAttribute(MainForm.ToTitle(city), 100) };
        }
        public override string GetCommand() {
            return "quest" + MainForm.commandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "Level", "Prem", "City" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                return minlevel;
            }
            if (header == hashes[2]) {
                return premium;
            }
            if (header == hashes[3]) {
                return city;
            }
            return base.GetHeaderValue(header);
        }
    }
}
