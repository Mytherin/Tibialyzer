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
using System.Drawing;

namespace Tibialyzer {
    public abstract class Attribute {
        public int MaxWidth;
    };

    public class StringAttribute : Attribute {
        public string value;
        public Color color;
        public StringAttribute(string value, int MaxWidth) {
            this.value = value;
            this.MaxWidth = MaxWidth;
            this.color = StyleManager.NotificationTextColor;
        }
        public StringAttribute(string value, int MaxWidth, Color color) {
            this.value = value;
            this.MaxWidth = MaxWidth;
            this.color = color;
        }
    }
    public class CommandAttribute : Attribute {
        public string value;
        public string command;
        public Color color;
        public CommandAttribute(string value, string command, int MaxWidth) {
            this.value = value;
            this.command = command;
            this.MaxWidth = MaxWidth;
            this.color = StyleManager.NotificationTextColor;
        }
        public CommandAttribute(string value, string command, int MaxWidth, Color color) {
            this.value = value;
            this.command = command;
            this.MaxWidth = MaxWidth;
            this.color = color;
        }
    }

    public class ImageAttribute : Attribute {
        public Image value;
        public ImageAttribute(Image value, int MaxWidth = 100) {
            this.value = value;
            this.MaxWidth = MaxWidth;
        }
    }
    public class BooleanAttribute : Attribute {
        public bool value;
        public BooleanAttribute(bool value, int MaxWidth = 100) {
            this.value = value;
            this.MaxWidth = MaxWidth;
        }
    }
}
