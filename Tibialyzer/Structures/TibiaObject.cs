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
    enum HeaderType { Numeric = 0, String = 1 };
    public abstract class TibiaObject {
        public bool permanent;
        public abstract string GetName();
        public abstract Image GetImage();
        public abstract List<string> GetAttributeHeaders();
        public abstract List<Attribute> GetAttributes();
        public abstract string GetCommand();
        public virtual IComparable GetHeaderValue(int hash) {
            return "";
        }
        public virtual List<string> GetConditionalHeaders() { return GetAttributeHeaders(); }
        public virtual IComparable GetConditionalHeaderValue(string header) {
            List<string> headers = GetAttributeHeaders();
            return GetHeaderValue(headers.IndexOf(header));
        }
        public virtual List<Attribute> GetConditionalAttributes() { return GetAttributes(); }
        public virtual Creature AsCreature() { return null; }
        public virtual Item AsItem() { return null; }
        public virtual NPC AsNPC() { return null; }
        public virtual Mount AsMount() { return null; }
        public virtual Outfit AsOutfit() { return null; }
        public virtual Spell AsSpell() { return null; }
    }

}
