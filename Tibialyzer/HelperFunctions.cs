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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    static class HelperFunctions {
        public static bool isDigit(this char c) {
            return c >= '0' && c <= '9';
        }

        public static bool Contains(this byte[] array, int start, int length, string text) {
            for (int i = start; i < start + length; i++) {
                for (int j = 0; j < text.Length; j++) {
                    if (text[j] != array[i + j]) {
                        break;
                    }

                    if (j == text.Length - 1) {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string ToTitle(this string str) {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        public static bool Contains(this string s, string text, StringComparison stringComparison) {
            return s.IndexOf(text, stringComparison) > -1;
        }


        public static int IndexOf(this string[] s, string text) {
            for (int i = 0; i < s.Length; i++) {
                if (s[i] != null && s[i].Equals(text, StringComparison.InvariantCultureIgnoreCase)) {
                    return i;
                }
            }
            return -1;
        }

        public static float ClampPercentage(this float percentage) {
            if (float.IsNaN(percentage) || float.IsInfinity(percentage)) return 1;
            return Math.Min(Math.Max(percentage, 0), 1);
        }
        public static double ClampPercentage(this double percentage) {
            if (double.IsNaN(percentage) || double.IsInfinity(percentage)) return 1;
            return Math.Min(Math.Max(percentage, 0), 1);
        }

        public static Bitmap ToGrayscale(this Bitmap original) {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static Bitmap ToGrayscale(this Image original) {
            if (original is Bitmap) {
                return (original as Bitmap).ToGrayscale();
            }
            Bitmap bitmap = new Bitmap(original);
            Bitmap grayscale = bitmap.ToGrayscale();
            bitmap.Dispose();
            return grayscale;
        }
    }
}
