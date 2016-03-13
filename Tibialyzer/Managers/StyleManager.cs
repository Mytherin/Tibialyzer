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
using System.IO;

namespace Tibialyzer {
    class StyleManager {
        #region UI Colors
        public static Color NotificationBackgroundColor = Color.FromArgb(0, 51, 102);
        public static Color NotificationTextColor = Color.FromArgb(191, 191, 191);

        public static Color AutoHotkeyKeywordColor = Color.FromArgb(25, 25, 112);
        public static Color AutoHotkeyModifierColor = Color.FromArgb(178, 34, 34);
        public static Color AutoHotkeyOperatorColor = Color.FromArgb(31, 31, 31);
        public static Color AutoHotkeyOperatorBackColor = Color.FromArgb(191, 191, 191);
        public static Color AutoHotkeySpecialTokenColor = Color.FromArgb(64, 128, 176);
        public static Color AutoHotkeyCommentColor = Color.FromArgb(34, 139, 34);
        public static Color AutoHotkeyCommandColor = Color.FromArgb(140, 95, 20);

        public static Color CloseButtonHoverColor = Color.FromArgb(200, 55, 55);
        public static Color CloseButtonNormalColor = Color.FromArgb(172, 24, 24);

        public static Color MinimizeButtonHoverColor = Color.FromArgb(191, 191, 191);
        public static Color MinimizeButtonNormalColor = Color.FromArgb(155, 155, 155);

        public static Color MainFormHoverColor = Color.FromArgb(43, 47, 51);
        public static Color MainFormButtonColor = Color.FromArgb(51, 55, 59);
        public static Color MainFormHoverForeColor = Color.FromArgb(190, 204, 217);
        public static Color MainFormButtonForeColor = Color.FromArgb(124, 133, 142);
        public static Color MainFormDangerColor = Color.FromArgb(152, 52, 52);
        public static Color MainFormSmallDangerColor = Color.FromArgb(186, 158, 29);
        public static Color MainFormSafeColor = Color.FromArgb(76, 128, 176);
        public static Color MainFormErrorColor = Color.FromArgb(174, 33, 33);

        public static Color ElementIceColor = Color.DodgerBlue;
        public static Color ElementFireColor = Color.FromArgb(255, 64, 64);
        public static Color ElementHolyColor = Color.DarkOrange;
        public static Color ElementPhysColor = Color.DimGray;
        public static Color ElementEarthColor = Color.ForestGreen;
        public static Color ElementDeathColor = Color.FromArgb(32, 32, 32);
        public static Color ElementEnergyColor = Color.MidnightBlue;

        public static Color DatabaseDiscardColor = Color.FromArgb(174, 33, 33);
        public static Color DatabaseNoDiscardColor = Color.FromArgb(56, 156, 56);
        public static Color DatabaseNoConvertColor = Color.FromArgb(76, 128, 176);

        public static Color ItemGoldColor = Color.FromArgb(237, 226, 24);

        public static Color ClickableLinkColor = Color.FromArgb(65, 105, 225);

        public static Color PathFinderPathColor = Color.FromArgb(25, 25, 25);

        public static Color CreatureHealthColor = Color.FromArgb(60, 179, 60);
        public static Color CreatureBossColor = Color.FromArgb(205, 102, 102);
        #endregion

        #region UI Fonts
        public static List<Font> FontList = new List<Font>();

        public static Font TextFont = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        public static Font MainFormLabelFontBig = new System.Drawing.Font("Verdana", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        public static Font MainFormLabelFont = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        public static Font MainFormLabelFontSmall = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        #endregion

        private static Dictionary<string, Image> images = new Dictionary<string, Image>();
        public static void InitializeStyle() {
            // Load Images
            foreach(string image in Directory.GetFiles(@"Images\")) {
                LoadImage(image, image.Split('\\')[1]);
            }
            // Generate Fonts of different sizes
            for (int i = 7; i < 20; i++) {
                FontList.Add(new System.Drawing.Font("Microsoft Sans Serif", i, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            }
            Initialized = true;
        }

        public static bool Initialized { get; private set; }

        private static void LoadImage(string file, string name) {
            Image image = null;
            if (!File.Exists(file)) {
                MainForm.ExitWithError("Fatal Error", String.Format("Could not find image {0}", file));
            }
            image = Image.FromFile(file);
            if (image == null) {
                MainForm.ExitWithError("Fatal Error", String.Format("Failed to load image {0}", file));
            }
            images.Add(name.ToLower(), image);
        }

        public static Image GetImage(string name) {
            if (!images.ContainsKey(name.ToLower())) {
                Console.WriteLine("Unknown image: {0}", name.ToLower());
            }
            return images[name.ToLower()];
        }

        public static Color GetElementColor(string element) {
            switch (element.ToLower()) {
                case "ice": return StyleManager.ElementIceColor;
                case "fire": return StyleManager.ElementFireColor;
                case "holy": return StyleManager.ElementHolyColor;
                case "phys": return StyleManager.ElementPhysColor;
                case "earth": return StyleManager.ElementEarthColor;
                case "death": return StyleManager.ElementDeathColor;
                case "energy": return StyleManager.ElementEnergyColor;
                default:
                    throw new Exception("Unrecognized element " + element);
            }
        }

        public static Image GetElementImage(string element) {
            return StyleManager.GetImage(element.ToLower() + ".png");
        }

        public static bool ElementExists(string element) {
            switch (element.ToLower()) {
                case "ice":
                case "fire":
                case "holy":
                case "phys":
                case "earth":
                case "death":
                case "energy":
                    return true;
                default:
                    return false;
            }
        }

        public static string GoldToText(long gold) {
            if (gold < 10000) {
                return ((long)gold).ToString();
            } else if (gold < 1000000) {
                return ((long)gold / 1000).ToString() + "K";
            } else {
                return ((long)gold / 1000000).ToString() + "M";
            }
        }
    }
}
