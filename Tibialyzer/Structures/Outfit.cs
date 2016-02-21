using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tibialyzer {
    public class Outfit : TibiaObject {
        public int id;
        public string title;
        public string name;
        public bool premium;
        public bool tibiastore;
        public Image[] maleImages = new Image[4];
        public Image[] femaleImages = new Image[4];
        public int questid;

        public override Outfit AsOutfit() { return this; }
        public override string GetName() { return name; }
        public override Image GetImage() {
            for (int i = 3; i >= 0; i--) {
                if (SettingsManager.getSettingBool("OutfitGenderMale")) {
                    if (maleImages[i] != null) return maleImages[i];
                    if (femaleImages[i] != null) return femaleImages[i];
                } else {
                    if (femaleImages[i] != null) return femaleImages[i];
                    if (maleImages[i] != null) return maleImages[i];
                }
            }
            throw new Exception("Outfit without image");
        }
        public override List<string> GetAttributeHeaders() {
            return new List<string> { "Name", "Prem", "Store", "Quest Name" };
        }
        public override List<Attribute> GetAttributes() {
            Quest q = StorageManager.getQuest(questid);
            return new List<Attribute> { new StringAttribute(name, 140), new BooleanAttribute(premium), new BooleanAttribute(tibiastore), new StringAttribute(q == null ? "-" : q.name, 100) };
        }
        public override string GetCommand() {
            return "outfit" + Constants.CommandSymbol + name;
        }
        static List<string> headers = new List<string> { "Name", "Prem", "Store", "Quest Name" };
        static int[] hashes = { headers[0].GetHashCode(), headers[1].GetHashCode(), headers[2].GetHashCode(), headers[3].GetHashCode() };
        public override IComparable GetHeaderValue(int hash) {
            if (hash == hashes[0]) {
                return name;
            }
            if (hash == hashes[1]) {
                return premium;
            }
            if (hash == hashes[2]) {
                return tibiastore;
            }
            if (hash == hashes[3]) {
                Quest q = StorageManager.getQuest(questid);
                if (q == null) return "";
                return q.name;
            }
            return base.GetHeaderValue(hash);
        }
    }
}
