
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
using System.Drawing;
using System.Windows.Forms;

namespace Tibialyzer {
    class SimpleLootNotification : SimpleNotification {
        private Label creatureDropLabel;
        private PictureBox creatureBox;
        public Creature creature;

        public SimpleLootNotification(Creature cr, List<Tuple<Item, int>> items, string message) : base() {
            this.InitializeComponent();
            this.creature = cr;

            this.InitializeSimpleNotification();

            creatureBox.Click -= c_Click;

            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;

            int max_x = this.Size.Width - creatureBox.Width - 32;
            int base_x = 64, base_y = 20;
            int x = 0;
            int y = 0;
            int item_spacing = 4;
            Size item_size = new Size(32, 32);

            List<Tuple<Item, int>> updatedItems = new List<Tuple<Item, int>>();
            foreach (Tuple<Item, int> tpl in items) {
                if (tpl.Item1.GetName().ToLower() == "gold coin" && tpl.Item2 > 100) {
                    Item platinumCoin = StorageManager.getItem("platinum coin");
                    updatedItems.Add(new Tuple<Item, int>(platinumCoin, tpl.Item2 / 100));
                    updatedItems.Add(new Tuple<Item, int>(tpl.Item1, tpl.Item2 % 100));
                } else {
                    updatedItems.Add(tpl);
                }
            }
            updatedItems = updatedItems.OrderByDescending(o => o.Item1.GetMaxValue() * o.Item2).ToList();

            x = 0;
            foreach (Tuple<Item, int> tpl in updatedItems) {
                Item item = tpl.Item1;
                int count = tpl.Item2;
                while (count > 0) {
                    if (x >= (max_x - item_size.Width - item_spacing)) {
                        item_size = new Size(24, 24);
                        x = 0;
                        base_y = 4;
                        creatureDropLabel.Visible = false;
                        break;
                    }
                    int mitems = 1;
                    if (item.stackable) mitems = Math.Min(count, 100);
                    count -= mitems;

                    x += item_size.Width + item_spacing;
                }
                if (x == 0) break;
            }

            x = 0;
            foreach (Tuple<Item, int> tpl in updatedItems) {
                Item item = tpl.Item1;
                int count = tpl.Item2;
                while (count > 0) {
                    if (x >= (max_x - item_size.Width - item_spacing)) {
                        x = 0;
                        y = y + item_size.Height + item_spacing;
                    }
                    int mitems = 1;
                    if (item.stackable) mitems = Math.Min(count, 100);
                    count -= mitems;

                    PictureBox picture_box = new PictureBox();
                    picture_box.Location = new System.Drawing.Point(base_x + x, base_y + y);
                    picture_box.Name = item.GetName();
                    picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
                    picture_box.TabIndex = 1;
                    picture_box.TabStop = false;
                    picture_box.Click += openItem_Click;
                    if (item.stackable) {
                        picture_box.Image = LootDropForm.DrawCountOnItem(item, mitems);
                    } else {
                        picture_box.Image = item.GetImage();
                    }

                    picture_box.SizeMode = PictureBoxSizeMode.Zoom;
                    picture_box.BackgroundImage = StyleManager.GetImage("item_background.png");
                    picture_box.BackgroundImageLayout = ImageLayout.Zoom;
                    value_tooltip.SetToolTip(picture_box, item.displayname.ToTitle() + " value: " + Math.Max(item.actual_value, item.vendor_value) * mitems);
                    this.Controls.Add(picture_box);

                    x += item_size.Width + item_spacing;
                }
            }

            Image creatureImage = cr.GetImage();
            if (creatureImage.Size.Width <= creatureBox.Width && creatureImage.Size.Height <= creatureBox.Height) {
                creatureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            } else {
                creatureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            this.creatureBox.Image = cr.GetImage();
            this.creatureDropLabel.Text = String.Format("Loot of {0}.", cr.displayname);

            PictureBox copyButton = new PictureBox();
            copyButton.Size = new Size(32, 32);
            copyButton.BackColor = Color.Transparent;
            copyButton.Location = new Point(this.Size.Width - copyButton.Size.Width - 4, (this.Size.Height - copyButton.Size.Height) / 2);
            copyButton.Click += CopyLootText;
            copyButton.Name = message;
            copyButton.Image = StyleManager.GetImage("copyicon.png");
            copyButton.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(copyButton);
        }

        private void CopyLootText(object sender, EventArgs e) {
            Clipboard.SetText((sender as Control).Name);
        }

        private void openItem_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("item" + Constants.CommandSymbol + (sender as Control).Name);
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleLootNotification));
            this.creatureDropLabel = new System.Windows.Forms.Label();
            this.creatureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.creatureBox)).BeginInit();
            this.SuspendLayout();
            //
            // creatureDropLabel
            //
            this.creatureDropLabel.AutoSize = true;
            this.creatureDropLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureDropLabel.ForeColor = StyleManager.NotificationTextColor;
            this.creatureDropLabel.Location = new System.Drawing.Point(64, 3);
            this.creatureDropLabel.Name = "";
            this.creatureDropLabel.Size = new System.Drawing.Size(121, 13);
            this.creatureDropLabel.TabIndex = 0;
            this.creatureDropLabel.Text = "Loot of a behemoth.";
            //
            // creatureBox
            //
            this.creatureBox.Location = new System.Drawing.Point(0, 0);
            this.creatureBox.Name = "creatureBox";
            this.creatureBox.Size = new System.Drawing.Size(60, 60);
            this.creatureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.creatureBox.TabIndex = 1;
            this.creatureBox.TabStop = false;
            this.creatureBox.Click += new System.EventHandler(this.creatureBox_Click);
            //
            // SimpleLootNotification
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(354, 60);
            this.Controls.Add(this.creatureBox);
            this.Controls.Add(this.creatureDropLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimpleLootNotification";
            this.Text = "Loot Notification";
            ((System.ComponentModel.ISupportInitialize)(this.creatureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void creatureBox_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("creature" + Constants.CommandSymbol + creature.GetName());
        }
    }
}
