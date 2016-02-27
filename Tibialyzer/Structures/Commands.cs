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
    public class Command {
        public string player;
        public string command;
    }

    public class SystemCommand {
        public string tibialyzer_command;
        public string command;
        public string parameters;
    }

    public class TibialyzerCommand {
        public string command;
        public TibialyzerCommand(string command) {
            this.command = command;
        }
    }

    class HelpCommand : TibiaObject {
        public string command;
        public string description;

        public override Image GetImage() {
            return null;
        }
        public override string GetName() {
            return command;
        }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        static List<string> headers = new List<string> { "Command", "Description" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode() };
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(command, 180), new StringAttribute(description, 320) };
        }
        public override string GetCommand() {
            return "";
        }
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return command;
            }
            if (header == hashes[1]) {
                return description;
            }
            return base.GetHeaderValue(header);
        }
    }


    class DamageObject : TibiaObject {
        public string name;
        public int totalDamage;
        public double dps;
        public double percentage;
        public Color color;

        public override Image GetImage() {
            return null;
        }
        public override string GetName() {
            return name;
        }
        public override List<string> GetAttributeHeaders() {
            return headers;
        }
        static List<string> headers = new List<string> { "Name", "Total", "DPS", "%" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override List<Attribute> GetAttributes() {
            return new List<Attribute> { new StringAttribute(name, 200, color), new StringAttribute(totalDamage.ToString(), 150),
                new StringAttribute(dps.ToString(System.Globalization.CultureInfo.InvariantCulture), 100), new StringAttribute(percentage.ToString(System.Globalization.CultureInfo.InvariantCulture) + "%", 100) };
        }
        public override string GetCommand() {
            return "";
        }
        public override IComparable GetHeaderValue(int header) {
            if (header == hashes[0]) {
                return name;
            }
            if (header == hashes[1]) {
                return totalDamage;
            }
            if (header == hashes[1]) {
                return dps;
            }
            if (header == hashes[1]) {
                return percentage;
            }
            return base.GetHeaderValue(header);
        }
    }
}
