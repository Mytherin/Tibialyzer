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
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            string expQuality = exp_quality < 0 ? "unknown" : (exp_quality - 1).ToString();
            string lootQuality = loot_quality < 0 ? "unknown" : (loot_quality - 1).ToString();
            return new List<Attribute> { new StringAttribute(name, 120), new StringAttribute(level < 0 ? "-" : level.ToString(), 50),
                new ImageAttribute(StyleManager.GetImage(String.Format("star{0}_text.png", expQuality))), new ImageAttribute(StyleManager.GetImage(String.Format("star{0}_text.png", lootQuality))),
                new StringAttribute(city, 100) };
        }
        public override string GetCommand() {
            return "hunt" + Constants.CommandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "Level", "Exp", "Loot", "City" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode(), headers[4].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                return level;
            }
            if (header == hashes[2]) {
                return exp_quality;
            }
            if (header == hashes[3]) {
                return loot_quality;
            }
            if (header == hashes[4]) {
                return city;
            }
            return base.GetHeaderValue(header);
        }
    }
}
