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
    enum TibiaObjectType { Creature, Item, NPC, Outfit, Mount, Spell };

    class LazyTibiaObject : TibiaObject {
        public int id;
        public TibiaObjectType type;
        private TibiaObject tibiaObject = null;
        public TibiaObject getTibiaObject() {
            if (tibiaObject == null) {
                switch (type) {
                    case TibiaObjectType.Creature:
                        tibiaObject = StorageManager.getCreature(id);
                        break;
                    case TibiaObjectType.Item:
                        tibiaObject = StorageManager.getItem(id);
                        break;
                    case TibiaObjectType.NPC:
                        tibiaObject = StorageManager.getNPC(id);
                        break;
                    case TibiaObjectType.Mount:
                        tibiaObject = StorageManager.getMount(id);
                        break;
                    case TibiaObjectType.Outfit:
                        tibiaObject = StorageManager.getOutfit(id);
                        break;
                    case TibiaObjectType.Spell:
                        tibiaObject = StorageManager.getSpell(id);
                        break;
                }
            }
            return tibiaObject;
        }

        public override Image GetImage() {
            return getTibiaObject().GetImage();
        }
        public override string GetName() {
            return getTibiaObject().GetName();
        }
        public override List<string> GetAttributeHeaders() {
            return getTibiaObject().GetAttributeHeaders();
        }
        public override List<Attribute> GetAttributes() {
            return getTibiaObject().GetAttributes();
        }
        public override string GetCommand() {
            return getTibiaObject().GetCommand();
        }
        public override IComparable GetHeaderValue(int header) {
            return getTibiaObject().GetHeaderValue(header);
        }

        public override List<string> GetConditionalHeaders() {
            return getTibiaObject().GetConditionalHeaders();
        }
        public override IComparable GetConditionalHeaderValue(string header) {
            return getTibiaObject().GetConditionalHeaderValue(header);
        }
        public override List<Attribute> GetConditionalAttributes() {
            return getTibiaObject().GetConditionalAttributes();
        }
        public override Creature AsCreature() { return type == TibiaObjectType.Creature ? getTibiaObject() as Creature : null; }
        public override Item AsItem() { return type == TibiaObjectType.Item ? getTibiaObject() as Item : null; }
        public override NPC AsNPC() { return type == TibiaObjectType.NPC ? getTibiaObject() as NPC : null; }
        public override Mount AsMount() { return type == TibiaObjectType.Mount ? getTibiaObject() as Mount : null; }
        public override Outfit AsOutfit() { return type == TibiaObjectType.Outfit ? getTibiaObject() as Outfit : null; }
        public override Spell AsSpell() { return type == TibiaObjectType.Spell ? getTibiaObject() as Spell : null; }
    }
}
