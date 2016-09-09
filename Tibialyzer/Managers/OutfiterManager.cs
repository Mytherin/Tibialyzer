using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is basically a (small) rewrite of the tibia.wikia outfiter application (http://tibia.wikia.com/wiki/Outfiter, Code: http://tibia.wikia.com/wiki/Outfiter/Code)
// All credit goes to them for actually developing it

namespace Tibialyzer {
    public enum Facing { Up = 0, Right = 1, Down = 2, Left = 3 };

    class OutfiterManager {
        public static List<string> outfiterOutfitNames = new List<string>() { "Citizen", "Hunter", "Mage", "Knight", "Nobleman", "Summoner", "Warrior", "Barbarian", "Druid", "Wizard", "Oriental", "Pirate", "Assassin", "Beggar", "Shaman", "Norseman", "Jester", "Brotherhood", "Nightmare", "Demon_Hunter", "Yalaharian", "Newly_Wed", "Warmaster", "Wayfarer", "Afflicted", "Elementalist", "Deepling", "Insectoid", "Entrepreneur", "Crystal_Warlord", "Soil_Guardian", "Demon", "Cave_Explorer", "Dream_Warden", "Jersey", "Glooth_Engineer", "Beastmaster", "Champion", "Conjurer", "Chaos_Acolyte", "Ranger", "Death_Herald", "Ceremonial_Garb", "Puppeteer", "Spirit_Caller", "Evoker", "Seaweaver", "Recruiter", "Sea_Dog", "Royal_Pumpkin", "Rift_Warrior", "Winter_Warden", "Philosopher", "Arena_Champion" };
        public static List<string> outfiterMountNames = new List<string>() { "None", "Widow_Queen", "Racing_Bird", "War_Bear", "Black_Sheep", "Midnight_Panther", "Draptor", "Titanica", "Tin_Lizzard", "Blazebringer", "Rapid_Boar", "Stampor", "Undead_Cavebear", "Crystal_Wolf", "Dromedary", "Kingly_Deer", "Donkey", "King_Scorpion", "Tamed_Panda", "Tiger_Slug", "Uniwheel", "Rented_Horse_(A)", "Rented_Horse_(B)", "Rented_Horse_(C)", "Armoured_War_Horse", "War_Horse", "Lady_Bug", "Manta_Ray", "Shadow_Draptor", "Gnarlhound", "Dragonling", "Magma_Crawler", "Ironblight", "Crimson_Ray", "Steelbeak", "Water_Buffalo", "Tombstinger", "Platesaurian", "Ursagrodon", "Hellgrip", "Noble_Lion", "Desert_King", "Shock_Head", "Walker", "Azudocus", "Carpacosaurus", "Death_Crawler", "Flamesteed", "Jade_Lion", "Jade_Pincer", "Nethersteed", "Tempest", "Winter_King", "Blackpelt", "Shadow_Hart", "Black_Stag", "Emperor_Deer", "Flying_Divan", "Magic_Carpet", "Floating_Kashmir", "Doombringer", "Tundra_Rambler", "Highland_Yak", "Glacier_Vagabond", "Golden_Dragonfly", "Steel_Bee", "Copper_Fly", "Hailstorm_Fury", "Poisonbane", "Siegebreaker", "Woodland_Prince", "Glooth_Glider", "Ringtail_Wacoon", "Night_Wacoon", "Emerald_Wacoon", "Flitterkatzen", "Venompaw", "Batcat", "Sea Devil", "Coralripper", "Plumfish", "Gorongra", "Noctungra", "Silverneck", "Rented_Horse_(Recruiter)", "Slagsnare", "Nightstinger", "Razorcreep", "Rift_Runner", "Nightdweller", "Frostflare", "Cinderhoof", "Bloodcurl", "Leafscuttler", "Mouldpincer", "Sparkion", "Swamp_Snapper", "Mould_Shell", "Reed_Lurker", "Neon_Sparkid", "Vortexion" };
        private static Dictionary<string, int> outfiterSpritesStanding = new Dictionary<string, int> { { "Chaos_Acolyte", 8 }, { "Evoker", 8 } };
        private static Dictionary<string, int> outfiterSpritesMountStanding = new Dictionary<string, int> { { "Flying_Divan", 8 }, { "Magic_Carpet", 8 }, { "Floating_Kashmir", 8 }, { "Copper_Fly", 10 }, { "Flamesteed", 8 }, { "Glooth_Glider", 10 }, { "Golden_Dragonfly", 10 }, { "Nethersteed", 8 }, { "Steel_Bee", 10 }, { "Tempest", 8 }, { "Flitterkatzen", 8 }, { "Venompaw", 8 }, { "Batcat", 8 }, { "Sea Devil", 8 }, { "Coralripper", 8 }, { "Plumfish", 8 }, { "Nightdweller", 8 }, { "Frostflare", 8 }, { "Cinderhoof", 8 } };
        private static Dictionary<string, string> outfiterFemaleNames = new Dictionary<string, string> { { "Nobleman", "Noblewoman" }, { "Norseman", "Norsewoman" } };
        public static List<string> outfiterNoAddons = new List<string> { "Newly_Wed", "Jersey" };
        private static List<string> outfiterOneAddon = new List<string> { "Yalaharian" };
        public static List<Color> outfitColors = new List<Color> { Color.FromArgb(255, 255, 255), Color.FromArgb(255, 212, 191), Color.FromArgb(255, 233, 191), Color.FromArgb(255, 255, 191), Color.FromArgb(233, 255, 191), Color.FromArgb(212, 255, 191), Color.FromArgb(191, 255, 191), Color.FromArgb(191, 255, 212), Color.FromArgb(191, 255, 233), Color.FromArgb(191, 255, 255), Color.FromArgb(191, 233, 255), Color.FromArgb(191, 212, 255), Color.FromArgb(191, 191, 255), Color.FromArgb(212, 191, 255), Color.FromArgb(233, 191, 255), Color.FromArgb(255, 191, 255), Color.FromArgb(255, 191, 233), Color.FromArgb(255, 191, 212), Color.FromArgb(255, 191, 191), Color.FromArgb(218, 218, 218), Color.FromArgb(191, 159, 143), Color.FromArgb(191, 175, 143), Color.FromArgb(191, 191, 143), Color.FromArgb(175, 191, 143), Color.FromArgb(159, 191, 143), Color.FromArgb(143, 191, 143), Color.FromArgb(143, 191, 159), Color.FromArgb(143, 191, 175), Color.FromArgb(143, 191, 191), Color.FromArgb(143, 175, 191), Color.FromArgb(143, 159, 191), Color.FromArgb(143, 143, 191), Color.FromArgb(159, 143, 191), Color.FromArgb(175, 143, 191), Color.FromArgb(191, 143, 191), Color.FromArgb(191, 143, 175), Color.FromArgb(191, 143, 159), Color.FromArgb(191, 143, 143), Color.FromArgb(182, 182, 181), Color.FromArgb(191, 127, 95), Color.FromArgb(191, 159, 95), Color.FromArgb(191, 191, 95), Color.FromArgb(159, 191, 95), Color.FromArgb(127, 191, 95), Color.FromArgb(95, 191, 95), Color.FromArgb(95, 191, 127), Color.FromArgb(95, 191, 159), Color.FromArgb(95, 191, 191), Color.FromArgb(95, 159, 191), Color.FromArgb(95, 127, 191), Color.FromArgb(95, 95, 191), Color.FromArgb(127, 95, 191), Color.FromArgb(159, 95, 191), Color.FromArgb(191, 95, 191), Color.FromArgb(191, 95, 159), Color.FromArgb(191, 95, 127), Color.FromArgb(191, 95, 95), Color.FromArgb(145, 145, 144), Color.FromArgb(191, 106, 63), Color.FromArgb(191, 148, 63), Color.FromArgb(191, 191, 63), Color.FromArgb(148, 191, 63), Color.FromArgb(106, 191, 63), Color.FromArgb(63, 191, 63), Color.FromArgb(63, 191, 106), Color.FromArgb(63, 191, 148), Color.FromArgb(63, 191, 191), Color.FromArgb(63, 148, 191), Color.FromArgb(63, 106, 191), Color.FromArgb(63, 63, 191), Color.FromArgb(106, 63, 191), Color.FromArgb(148, 63, 191), Color.FromArgb(191, 63, 191), Color.FromArgb(191, 63, 148), Color.FromArgb(191, 63, 106), Color.FromArgb(191, 63, 63), Color.FromArgb(109, 109, 109), Color.FromArgb(255, 85, 0), Color.FromArgb(255, 170, 0), Color.FromArgb(255, 255, 0), Color.FromArgb(170, 255, 0), Color.FromArgb(84, 255, 0), Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 84), Color.FromArgb(0, 255, 170), Color.FromArgb(0, 255, 255), Color.FromArgb(0, 169, 255), Color.FromArgb(0, 85, 255), Color.FromArgb(0, 0, 255), Color.FromArgb(85, 0, 255), Color.FromArgb(169, 0, 255), Color.FromArgb(254, 0, 255), Color.FromArgb(255, 0, 170), Color.FromArgb(255, 0, 85), Color.FromArgb(255, 0, 0), Color.FromArgb(72, 72, 68), Color.FromArgb(191, 63, 0), Color.FromArgb(191, 127, 0), Color.FromArgb(191, 191, 0), Color.FromArgb(127, 191, 0), Color.FromArgb(63, 191, 0), Color.FromArgb(0, 191, 0), Color.FromArgb(0, 191, 63), Color.FromArgb(0, 191, 127), Color.FromArgb(0, 191, 191), Color.FromArgb(0, 127, 191), Color.FromArgb(0, 63, 191), Color.FromArgb(0, 0, 191), Color.FromArgb(63, 0, 191), Color.FromArgb(127, 0, 191), Color.FromArgb(191, 0, 191), Color.FromArgb(191, 0, 127), Color.FromArgb(191, 0, 63), Color.FromArgb(191, 0, 0), Color.FromArgb(36, 36, 36), Color.FromArgb(127, 42, 0), Color.FromArgb(127, 85, 0), Color.FromArgb(127, 127, 0), Color.FromArgb(85, 127, 0), Color.FromArgb(42, 127, 0), Color.FromArgb(0, 127, 0), Color.FromArgb(0, 127, 42), Color.FromArgb(0, 127, 85), Color.FromArgb(0, 127, 127), Color.FromArgb(0, 84, 127), Color.FromArgb(0, 42, 127), Color.FromArgb(0, 0, 127), Color.FromArgb(42, 0, 127), Color.FromArgb(84, 0, 127), Color.FromArgb(127, 0, 127), Color.FromArgb(127, 0, 85), Color.FromArgb(127, 0, 42), Color.FromArgb(127, 0, 0) };

        private static SQLiteConnection outfiterConn;
        private static object outfiterLock = new object();

        public const int OutfitColorBoxSize = 16;
        public const int OutfitColorsPerRow = 19;
        public static void Initialize() {
            outfiterConn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", Constants.OutfiterDatabaseFile));
            outfiterConn.Open();
        }

        public static bool OutfitHasNoAddons(int outfit) {
            return outfiterNoAddons.Contains(outfiterOutfitNames[outfit]);
        }

        public static Bitmap OutfitSheet(int outfit, Gender gender) {
            lock (outfiterLock) {
                string outfitName = outfiterOutfitNames[outfit];
                bool male = gender == Gender.Male;
                SQLiteCommand comm = new SQLiteCommand(String.Format("SELECT image FROM OutfitImages WHERE name='{0}' AND male={1}", outfitName, male ? 1 : 0), outfiterConn);
                SQLiteDataReader reader = comm.ExecuteReader();
                while (reader.Read()) {
                    return reader.IsDBNull(0) ? null : new Bitmap(Image.FromStream(reader.GetStream(0)));
                }
                return null;
            }
        }

        public static Bitmap MountSheet(int mount) {
            lock (outfiterLock) {
                string mountName = outfiterMountNames[mount];
                SQLiteCommand comm = new SQLiteCommand(String.Format("SELECT image FROM MountImages WHERE name='{0}'", mountName), outfiterConn);
                SQLiteDataReader reader = comm.ExecuteReader();
                while (reader.Read()) {
                    return reader.IsDBNull(0) ? null : new Bitmap(Image.FromStream(reader.GetStream(0)));
                }
                return null;
            }
        }

        public static Color GetColor(int color) {
            return outfitColors[color];
        }

        public static Bitmap GenerateColorImage(int pressedIndex) {
            Bitmap bitmap = new Bitmap(OutfitColorBoxSize * OutfitColorsPerRow, OutfitColorBoxSize * 8);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                int index = 0;
                for (int i = 0; i < outfitColors.Count; i++) {
                    int x = OutfitColorBoxSize * (index % OutfitColorsPerRow);
                    int y = OutfitColorBoxSize * (index / OutfitColorsPerRow);
                    using (Brush brush = new SolidBrush(outfitColors[i]))
                        gr.FillRectangle(brush, new Rectangle(x, y, OutfitColorBoxSize - 1, OutfitColorBoxSize - 1));
                    gr.DrawImage(i == pressedIndex ? StyleManager.GetImage("color_bevel_pressed.png") : StyleManager.GetImage("color_bevel.png"), new Rectangle(x, y, OutfitColorBoxSize, OutfitColorBoxSize));
                    index++;
                }
            }
            return bitmap;
        }

        public static int ColorIndex(int x, int y) {
            int index = (x / OutfitColorBoxSize) + OutfitColorsPerRow * (y / OutfitColorBoxSize);
            if (index < 0 || index >= outfitColors.Count) return -1;
            return index;
        }
    }

    class OutfiterOutfit {
        public int outfit = 0;
        public int mount = 0;
        public int[] colors = new int[4] { 0, 0, 0, 0 };
        public bool addon1 = false;
        public bool addon2 = false;
        public Gender gender;
        public Facing facing = Facing.Down;
        
        /// <summary>
        /// Get a 64x64 rectangle from the input image at the specified (x,y) indexes (i.e. actual coordinates used are x * 64, y * 64)
        /// </summary>
        private static Bitmap GetRegion(Bitmap source, int x, int y) {
            Bitmap bitmap = new Bitmap(64, 64);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                gr.DrawImage(source, new Rectangle(0, 0, 64, 64), new Rectangle(x * 64, y * 64, 64, 64), GraphicsUnit.Pixel);
            }
            bitmap.MakeTransparent(Color.FromArgb(255, 0, 255));
            return bitmap;
        }

        /// <summary>
        /// Overlay the top image over the bottom image, always returns a new bitmap
        /// </summary>
        private static Bitmap MergePixels(Bitmap bottom, Bitmap top) {
            if (bottom == null) return new Bitmap(top);
            if (top == null) return new Bitmap(bottom);


            Bitmap result = new Bitmap(bottom);
            for (int x = 0; x < top.Width; x++) {
                for (int y = 0; y < top.Height; y++) {
                    Color c = top.GetPixel(x, y);
                    if (c.A > 0) {
                        result.SetPixel(x, y, c);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Blends the specified colors onto "main", according to the blend pattern specified by "blend"
        /// Blend contains either (255,255,0) for colors[0]; (255,0,0) for colors [1]; (0,255,0) for colors[2]; (0,0,255) for colors[3]; or Transparent for nothing
        /// </summary>
        private static Bitmap BlendPixels(Bitmap main, Bitmap blend, int[] colors) {
            Color c1 = OutfiterManager.GetColor(colors[0]);
            Color c2 = OutfiterManager.GetColor(colors[1]);
            Color c3 = OutfiterManager.GetColor(colors[2]);
            Color c4 = OutfiterManager.GetColor(colors[3]);
            for (int x = 0; x < main.Width; x++) {
                for (int y = 0; y < main.Height; y++) {
                    Color blendColor = blend.GetPixel(x, y);
                    Color resultColor = Color.Transparent;
                    if (blendColor.R == 255 && blendColor.G == 255 && blendColor.B == 0) {
                        resultColor = c1;
                    } else if (blendColor.R == 255 && blendColor.G == 0 && blendColor.B == 0) {
                        resultColor = c2;
                    } else if (blendColor.R == 0 && blendColor.G == 255 && blendColor.B == 0) {
                        resultColor = c3;
                    } else if (blendColor.R == 0 && blendColor.G == 0 && blendColor.B == 255) {
                        resultColor = c4;
                    }
                    if (blendColor.A > 0) {
                        Color mainColor = main.GetPixel(x, y);
                        main.SetPixel(x, y, Color.FromArgb(
                            (byte)((((double)mainColor.R / 255.0)) * resultColor.R),
                            (byte)((((double)mainColor.G / 255.0)) * resultColor.G),
                            (byte)((((double)mainColor.B / 255.0)) * resultColor.B)));
                    }
                }
            }
            return main;
        }

        private bool GetAddon(int i) {
            if (i == 0) return addon1;
            if (i == 1) return addon2;
            return false;
        }

        public void Rotate(int offset) {
            if (offset < 0) {
                switch(facing) {
                    case Facing.Down:
                        facing = Facing.Right;
                        break;
                    case Facing.Right:
                        facing = Facing.Up;
                        break;
                    case Facing.Up:
                        facing = Facing.Left;
                        break;
                    case Facing.Left:
                        facing = Facing.Down;
                        break;
                }
            } else if (offset > 0) {
                switch (facing) {
                    case Facing.Down:
                        facing = Facing.Left;
                        break;
                    case Facing.Right:
                        facing = Facing.Down;
                        break;
                    case Facing.Up:
                        facing = Facing.Right;
                        break;
                    case Facing.Left:
                        facing = Facing.Up;
                        break;
                }
            }
        }

        public void SwitchOutfit(int offset) {
            outfit += offset;
            if (outfit < 0) {
                outfit = OutfiterManager.outfiterOutfitNames.Count - 1;
            } else if (outfit >= OutfiterManager.outfiterOutfitNames.Count) {
                outfit = 0;
            }
        }

        public void SwitchMount(int offset) {
            mount += offset;
            if (mount < 0) {
                mount = OutfiterManager.outfiterMountNames.Count - 1;
            } else if (mount >= OutfiterManager.outfiterMountNames.Count) {
                mount = 0;
            }
        }

        public void Validate() {
            if (outfit < 0 || outfit >= OutfiterManager.outfiterOutfitNames.Count) outfit = 0;
            if (mount < 0 || mount >= OutfiterManager.outfiterMountNames.Count) mount = 0;
            for(int i = 0; i < 4; i++) {
                if (colors[i] < 0 || colors[i] >= OutfiterManager.outfitColors.Count) {
                    colors[i] = 0;
                }
            }
        }

        /// <summary>
        /// Modify the current outfit based on the specified string; this is either a tibia.wikia outfiter URL or just the GET parameters of one.
        /// </summary>
        public void FromString(string str) {
            if (str == null) return;
            str = str.Replace("http://tibia.wikia.com/wiki/Outfiter?", "").ToLower();
            string[] splits = str.Split('&');
            gender = Gender.Male;
            addon1 = false;
            addon2 = false;
            mount = 0;
            outfit = 0;
            for (int i = 0; i < 4; i++) colors[i] = 0;

            foreach(string split in splits) {
                if (split == "fm") gender = Gender.Female;
                else if (split == "a1") addon1 = true;
                else if (split == "a2") addon2 = true;
                else if (split.Contains("=")) {
                    string[] kv = split.Split('=');
                    string key = kv[0].Trim();
                    string value = kv[1].Trim();
                    if (key == "m") {
                        int.TryParse(value, out mount);
                    } else if (key == "o") {
                        int.TryParse(value, out outfit);
                    } else if (key == "c1") {
                        int.TryParse(value, out colors[0]);
                    } else if (key == "c2") {
                        int.TryParse(value, out colors[1]);
                    } else if (key == "c3") {
                        int.TryParse(value, out colors[2]);
                    } else if (key == "c4") {
                        int.TryParse(value, out colors[3]);
                    }
                }
            }
            this.Validate();
        }

        /// <summary>
        /// Convert the Outfit to a string, this is the same format as the GET used by the tibia.wikia outfiter
        /// </summary>
        public override string ToString() {
            return String.Format("f={0}&o={1}&m={2}{3}{4}{5}&c1={6}&c2={7}&c3={8}&c4={9}",
                (int)facing,
                outfit,
                mount,
                gender == Gender.Female ? "&fm" : "",
                addon1 ? "&a1" : "",
                addon2 ? "&a2" : "",
                colors[0],
                colors[1],
                colors[2],
                colors[3]);
        }

        /// <summary>
        /// Get the image of the current outfit
        /// </summary>
        public Image GetImage() {
            int anim = 0;
            bool noAddons = OutfiterManager.OutfitHasNoAddons(outfit);
            int mult_y = noAddons ? 1 : 3;
            // if there is a mount, choose the "mounted" animation, otherwise choose the "normal" animation
            int y = (mount != 0 ? (noAddons ? 1 : 3) : 0) + (anim * mult_y);
            Bitmap outfitImage;
            // first get the outfit sheet (this contains all the different sprites for different facings/mounted/addons/etc)
            using (Bitmap outfitSheet = OutfiterManager.OutfitSheet(outfit, gender)) {
                outfitImage = GetRegion(outfitSheet, (int)facing * 2, y);
                using (Bitmap blendImage = GetRegion(outfitSheet, (int)facing * 2 + 1, y)) {
                    // colorize the outfit based on the outfits' colors
                    outfitImage = BlendPixels(outfitImage, blendImage, colors);
                    // if there are any addons selected, merge them on top of the outfit
                    for(int i = 0; i < 2; i++) {
                        if (!noAddons && GetAddon(i)) {
                            using (Bitmap oldImage = outfitImage) {
                                // get the addon region
                                using (Bitmap addonImage = GetRegion(outfitSheet, (int)facing * 2, y + i + 1)) {
                                    // blend the addon with the colors
                                    using (Bitmap addonBlendImage = GetRegion(outfitSheet, (int)facing * 2 + 1, y + i + 1)) {
                                        // merge the addon on top of the outfit
                                        outfitImage = MergePixels(oldImage, BlendPixels(addonImage, addonBlendImage, colors));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (mount != 0) {
                // if there is a mount, merge the outfit with the mount
                using (Bitmap mountSheet = OutfiterManager.MountSheet(mount)) {
                    using (Bitmap mountImage = GetRegion(mountSheet, (int)facing, 0)) {
                        Bitmap combinedImage = MergePixels(mountImage, outfitImage);
                        outfitImage.Dispose();
                        return combinedImage;
                    }
                }
            } else {
                return outfitImage;
            }
        }
    }
}
