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
    public class Task : TibiaObject {
        public int id;
        public int groupid;
        public string name;
        public string groupname;
        public int count;
        public int taskpoints;
        public int bossid;
        public Coordinate bossposition;
        public List<int> creatures;
        public List<int> hunts;

        public Task() {
            creatures = new List<int>();
            hunts = new List<int>();
        }

        public Creature GetBoss() {
            if (bossid < 0) return null;
            return StorageManager.getCreature(bossid);
        }

        public override string GetName() {
            return name;
        }

        public override Image GetImage() {
            Creature cr = StorageManager.getCreature(creatures[0]);
            return cr.GetImage();
        }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> {
                new StringAttribute(GetName(), 150),
                new StringAttribute(count.ToString(), 60),
                new StringAttribute(taskpoints >= 0 ? taskpoints.ToString() : "-", 60),
                new CommandAttribute(groupname, "task" + Constants.CommandSymbol + groupname, 120),
            };
        }
        public override string GetCommand() {
            return "task" + Constants.CommandSymbol + id;
        }
        static List<string> headers = new List<string> { "Name", "Count", "Points", "Group" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return GetName();
            }
            if (header == hashes[1]) {
                return count;
            }
            if (header == hashes[2]) {
                return taskpoints;
            }
            if (header == hashes[3]) {
                return groupname;
            }
            return base.GetHeaderValue(header);
        }
    }
}
