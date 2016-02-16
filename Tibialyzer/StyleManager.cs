using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Tibialyzer {
    public enum Colors {

    }
    class StyleManager {
        private static Dictionary<Colors, Color> colors = new Dictionary<Colors, Color>();
        private static Dictionary<string, Image> images = new Dictionary<string, Image>();
        public static void InitializeStyle() {
            foreach(string image in Directory.GetFiles(@"Images\")) {
                LoadImage(image, image.Split('\\')[1]);
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

        public static Color GetColor(Colors color) {
            return colors[color];
        }
    }
}
