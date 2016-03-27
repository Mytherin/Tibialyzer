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
            for(int i = 0; i < s.Length; i++) {
                if (s[i] != null && s[i].Equals(text, StringComparison.InvariantCultureIgnoreCase)) {
                    return i;
                }
            }
            return -1;
        }
    }
}
