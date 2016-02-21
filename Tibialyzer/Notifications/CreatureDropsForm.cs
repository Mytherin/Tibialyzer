
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
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Tibialyzer {

    public partial class CreatureDropsForm : NotificationForm {
        public Creature creature;

        public CreatureDropsForm() {
            InitializeComponent();
        }

        private void DisplayItem(ItemDrop drop, int base_x, int base_y, int x, int y, Size item_size, ToolTip droprate_tooltip, int dropbar_height, string prefix = "Drop rate of ") {

            Item dropItem = StorageManager.getItem(drop.itemid);
            // the main picture of the item
            PictureBox picture_box = new PictureBox();
            picture_box.Location = new System.Drawing.Point(base_x + x, base_y + y);
            picture_box.Name = dropItem.GetName();
            picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
            picture_box.TabIndex = 1;
            picture_box.TabStop = false;
            picture_box.Image = drop.max > 1 ? LootDropForm.DrawCountOnItem(dropItem, drop.max) : dropItem.GetImage();
            picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
            picture_box.BackgroundImage = StyleManager.GetImage("item_background.png");
            picture_box.Click += openItemBox;
            droprate_tooltip.SetToolTip(picture_box, prefix + dropItem.displayname + " is " + (drop.percentage >= 0 ? Math.Round(drop.percentage, 1).ToString() + "%." : "unknown."));
            this.Controls.Add(picture_box);

            // the 'dropbar' that roughly displays the droprate of the item
            PictureBox dropbar_box = new PictureBox();
            Brush brush;
            dropbar_box.Location = new System.Drawing.Point(base_x + x, base_y + y + item_size.Height);
            dropbar_box.Name = "dropbar_image";
            dropbar_box.Size = new System.Drawing.Size(item_size.Width, dropbar_height);
            dropbar_box.TabIndex = 1;
            dropbar_box.TabStop = false;
            Image image = new Bitmap(dropbar_box.Width, dropbar_box.Height);
            Graphics gr = Graphics.FromImage(image);
            gr.FillRectangle(Brushes.DarkGray, new Rectangle(0, 0, item_size.Width, dropbar_height)); //dropbar base bar
            if (drop.percentage < 1) {
                brush = Brushes.DarkRed; // <1% is red
            } else if (drop.percentage < 15) {
                brush = Brushes.Yellow; //<15% is yellow
            } else {
                brush = Brushes.ForestGreen; //everything else is green
            }
            gr.FillRectangle(brush, new Rectangle(0, 0, (int)(Math.Ceiling(item_size.Width * drop.percentage / 100) + 1), dropbar_height));
            dropbar_box.Image = image;
            this.Controls.Add(dropbar_box);
        }

        private void CombineItems() {
            Size item_size = new Size(32, 32); //size of item image
            int dropbar_height = 6; //height of dropbar
            int item_spacing = 6; //spacing between items
            int base_x = 110;
            int base_y = this.mainImage.Location.Y;
            int max_x = 250;
            int max_y = base_y + 134;

            // add a tooltip that displays the actual droprate when you mouseover
            ToolTip droprate_tooltip = new ToolTip();
            droprate_tooltip.AutoPopDelay = 60000;
            droprate_tooltip.InitialDelay = 500;
            droprate_tooltip.ReshowDelay = 0;
            droprate_tooltip.ShowAlways = true;
            droprate_tooltip.UseFading = true;

            int x = item_spacing, y = item_spacing;
            List<ItemDrop> sorted_items = creature.itemdrops.OrderByDescending(o => o.percentage).ToList();
            foreach (ItemDrop drop in sorted_items) {
                if (x > (max_x - item_size.Width - item_spacing)) {
                    x = item_spacing;
                    y += item_size.Height + item_spacing;
                }
                DisplayItem(drop, base_x, base_y, x, y, item_size, droprate_tooltip, dropbar_height);
                x += item_size.Width + item_spacing;
            }

            if (creature.skin != null) {
                Item skinItem = StorageManager.getItem(creature.skin.skinitemid);
                ItemDrop skinDrop = new ItemDrop();
                PictureBox picture_box = new PictureBox();
                picture_box.Location = new System.Drawing.Point(20, this.huntButton.Location.Y + this.huntButton.Size.Height + 10);
                picture_box.Name = skinItem.GetName();
                picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
                picture_box.TabIndex = 1;
                picture_box.TabStop = false;
                picture_box.Image = skinItem.GetImage();
                picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                picture_box.BackgroundImage = StyleManager.GetImage("item_background.png");
                picture_box.Click += openItemBox; droprate_tooltip.SetToolTip(picture_box, "You can skin this creature with the item " + skinItem.displayname + ".");
                this.Controls.Add(picture_box);

                skinDrop.itemid = creature.skin.dropitemid;
                skinDrop.percentage = creature.skin.percentage;
                skinDrop.min = 1;
                skinDrop.max = 1;
                DisplayItem(skinDrop, 20 + item_size.Width + item_spacing, this.huntButton.Location.Y + this.huntButton.Size.Height + 10, 0, 0, item_size, droprate_tooltip, dropbar_height, "Skin rate of ");
                if (y < this.huntButton.Location.Y + this.huntButton.Size.Height) y = this.huntButton.Location.Y + this.huntButton.Size.Height;
            }

            if (this.Height < (y + item_size.Height * 2 + item_spacing)) {
                this.Height = y + item_size.Height * 2 + item_spacing;
            }
            this.Refresh();
        }

        void openItemBox(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("item" + MainForm.commandSymbol + (sender as Control).Name);
        }

        public override void LoadForm() {
            this.SuspendForm();
            base.NotificationInitialize();
            statsButton.Click -= c_Click;
            huntButton.Click -= c_Click;
            // load image from the creature
            this.mainImage.Image = this.creature.GetImage();
            this.statsButton.Name = this.creature.GetName().ToLower();
            this.huntButton.Name = this.creature.GetName().ToLower();
            // set background of actual form to transparent
            this.BackColor = StyleManager.NotificationBackgroundColor;
            CombineItems();
            this.nameLabel.Text = MainForm.ToTitle(this.creature.displayname);
            Font f = StyleManager.FontList[0];
            Font prevFont = f;
            for(int i = 0; i < StyleManager.FontList.Count; i++) {
                Font font = StyleManager.FontList[i];
                int width = TextRenderer.MeasureText(this.nameLabel.Text, font).Width;
                if (width < this.mainImage.Size.Width) {
                    f = prevFont;
                } else {
                    break;
                }
                prevFont = font;
            }
            this.nameLabel.Font = f;
            this.nameLabel.Left = this.mainImage.Left + (mainImage.Width - this.nameLabel.Size.Width) / 2;
            this.NotificationFinalize();
            this.ResumeForm();
        }

        private void statsButton_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("stats" + MainForm.commandSymbol + (sender as Control).Name);
        }

        private void huntButton_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("hunt" + MainForm.commandSymbol + (sender as Control).Name);
        }
    }
}
