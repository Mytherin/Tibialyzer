
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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tibialyzer {
    public partial class LootDropForm : NotificationForm {
        public List<Tuple<Item, int>> items;
        public Dictionary<Creature, int> creatures;
        public Hunt hunt;
        public int initialPage = 0;
        public int page = 0;
        public const int pageHeight = 400;
        public const int maxCreatureHeight = 700;
        public const int minLootWidth = 275;

        public static Font loot_font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
        public LootDropForm() {
            InitializeComponent();
        }

        public static Bitmap GetStackImage(Image image, int count, Item item) {
            if (image == null) return new Bitmap(item.image);
            int max = image.GetFrameCount(FrameDimension.Time);
            int index = 0;

            if (count <= 5) index = count - 1;
            else if (count <= 10) index = 5;
            else if (count <= 25) index = 6;
            else if (count <= 50) index = 7;
            else index = 8;

            if (index >= max) index = max - 1;
            image.SelectActiveFrame(FrameDimension.Time, index);
            return new Bitmap((Image)image.Clone());
        }

        public List<Control> createdControls = new List<Control>();
        public void RefreshLoot() {
            foreach (Control c in createdControls) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            createdControls.Clear();
            if (page < 0) page = 0;

            int base_x = 20, base_y = 30;
            int x = 0, y = 0;
            int item_spacing = 4;
            Size item_size = new Size(32, 32);
            int max_x = MainForm.mainForm.getSettingInt("LootFormWidth");
            if (max_x < minLootWidth) max_x = minLootWidth;
            int width_x = max_x + item_spacing * 2;

            // add a tooltip that displays the actual droprate when you mouseover
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            long total_value = 0;
            int currentPage = 0;
            bool prevPage = page > 0;
            bool nextPage = false;

            foreach (Tuple<Item, int> tpl in items) {
                total_value += tpl.Item1.GetMaxValue() * tpl.Item2;
            }
            foreach (Tuple<Item, int> tpl in items) {
                Item item = tpl.Item1;
                int count = tpl.Item2;
                while (count > 0) {
                    if (base_x + x >= (max_x - item_size.Width - item_spacing)) {
                        x = 0;
                        if (y + item_size.Height + item_spacing > pageHeight) {
                            currentPage++;
                            if (currentPage > page) {
                                nextPage = true;
                                break;
                            } else {
                                y = 0;
                            }
                        } else {
                            y = y + item_size.Height + item_spacing;
                        }
                    }
                    int mitems = 1;
                    if (item.stackable || count > 100) mitems = Math.Min(count, 100);
                    count -= mitems;
                    if (currentPage == page) {
                        PictureBox picture_box = new PictureBox();
                        picture_box.Location = new System.Drawing.Point(base_x + x, base_y + y);
                        picture_box.Name = item.GetName();
                        picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
                        picture_box.TabIndex = 1;
                        picture_box.TabStop = false;
                        if (item.stackable || mitems > 1) {
                            Bitmap image = GetStackImage(item.image, mitems, item);
                            Graphics gr = Graphics.FromImage(image);
                            int numbers = (int)Math.Floor(Math.Log(mitems, 10)) + 1;
                            int xoffset = 1, logamount = mitems;
                            for (int i = 0; i < numbers; i++) {
                                int imagenr = logamount % 10;
                                xoffset = xoffset + MainForm.image_numbers[imagenr].Width + 1;
                                gr.DrawImage(MainForm.image_numbers[imagenr],
                                    new Point(image.Width - xoffset, image.Height - MainForm.image_numbers[imagenr].Height - 3));
                                logamount /= 10;
                            }
                            picture_box.Image = image;
                        } else {
                            picture_box.Image = item.GetImage();
                        }

                        picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                        picture_box.BackgroundImage = MainForm.item_background;
                        picture_box.Click += openItemBox;
                        long individualValue = Math.Max(item.actual_value, item.vendor_value);
                        value_tooltip.SetToolTip(picture_box, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.displayname) + " value: " + (individualValue >= 0 ? (individualValue * mitems).ToString() : "Unknown"));
                        createdControls.Add(picture_box);
                        this.Controls.Add(picture_box);
                    }

                    x += item_size.Width + item_spacing;
                }
                if (currentPage > page) {
                    break;
                }
            }
            if (page > currentPage) {
                page = currentPage;
                RefreshLoot();
                return;
            }

            y = y + item_size.Height + item_spacing;
            if (prevPage) {
                PictureBox prevpage = new PictureBox();
                prevpage.Location = new Point(10, base_y + y);
                prevpage.Size = new Size(97, 23);
                prevpage.Image = MainForm.prevpage_image;
                prevpage.BackColor = Color.Transparent;
                prevpage.SizeMode = PictureBoxSizeMode.StretchImage;
                prevpage.Click += Prevpage_Click;
                this.Controls.Add(prevpage);
                createdControls.Add(prevpage);
            }
            if (nextPage) {
                PictureBox nextpage = new PictureBox();
                nextpage.Location = new Point(width_x - 108, base_y + y);
                nextpage.Size = new Size(98, 23);
                nextpage.BackColor = Color.Transparent;
                nextpage.Image = MainForm.nextpage_image;
                nextpage.SizeMode = PictureBoxSizeMode.StretchImage;
                nextpage.Click += Nextpage_Click;
                this.Controls.Add(nextpage);
                createdControls.Add(nextpage);
            }
            if (prevPage || nextPage) y += 23;

            x = 0;
            base_x = 5;
            Size creature_size = new Size(1, 1);
            Size labelSize = new Size(1, 1);

            foreach (KeyValuePair<Creature, int> tpl in creatures) {
                Creature creature = tpl.Key;
                creature_size.Width = Math.Max(creature_size.Width, creature.GetImage().Width);
                creature_size.Height = Math.Max(creature_size.Height, creature.GetImage().Height);
            }
            {
                int i = 0;
                foreach (Creature cr in creatures.Keys.OrderByDescending(o => creatures[o] * (1 + o.experience)).ToList<Creature>()) {
                    Creature creature = cr;
                    int killCount = creatures[cr];
                    if (x >= max_x - creature_size.Width - item_spacing * 2) {
                        x = 0;
                        y = y + creature_size.Height + 23;
                        if (y > maxCreatureHeight) {
                            break;
                        }
                    }
                    int xoffset = (creature_size.Width - creature.GetImage().Width) / 2;
                    int yoffset = (creature_size.Height - creature.GetImage().Height) / 2;

                    Label count = new Label();
                    count.Text = killCount.ToString() + "x";
                    count.Font = loot_font;
                    count.Size = new Size(1, 10);
                    count.Location = new Point(base_x + x + xoffset, base_y + y + creature_size.Height);
                    count.AutoSize = true;
                    count.TextAlign = ContentAlignment.MiddleCenter;
                    count.ForeColor = Color.FromArgb(191, 191, 191);
                    count.BackColor = Color.Transparent;

                    int measured_size = (int)count.CreateGraphics().MeasureString(count.Text, count.Font).Width;
                    int width = Math.Max(measured_size, creature.GetImage().Width);
                    PictureBox picture_box = new PictureBox();
                    picture_box.Location = new System.Drawing.Point(base_x + x + xoffset, base_y + y + yoffset + (creature_size.Height - creature.GetImage().Height) / 2);
                    picture_box.Name = creature.GetName();
                    picture_box.Size = new System.Drawing.Size(creature.GetImage().Width, creature.GetImage().Height);
                    picture_box.TabIndex = 1;
                    picture_box.TabStop = false;
                    picture_box.Image = creature.GetImage();
                    picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box.Click += openCreatureDrops;
                    picture_box.BackColor = Color.Transparent;

                    if (width > creature.GetImage().Width) {
                        picture_box.Location = new Point(picture_box.Location.X + (width - creature.GetImage().Width) / 2, picture_box.Location.Y);
                    } else {
                        count.Location = new Point(count.Location.X + (width - measured_size) / 2, count.Location.Y);
                    }

                    labelSize = count.Size;

                    i++;
                    x += width + xoffset;
                    createdControls.Add(picture_box);
                    createdControls.Add(count);
                    this.Controls.Add(picture_box);
                    this.Controls.Add(count);
                }
                y = y + creature_size.Height + labelSize.Height * 2;
            }

            int xPosition = width_x - totalValueValue.Size.Width - 5;
            y = base_y + y + item_spacing + 10;
            huntNameLabel.Text = hunt.name.ToString();
            totalValueLabel.Location = new Point(5, y);
            totalValueValue.Location = new Point(xPosition, y);
            totalValueValue.Text = total_value.ToString();
            totalExpLabel.Location = new Point(5, y += 20);
            totalExpValue.Location = new Point(xPosition, y);
            totalExpValue.Text = hunt.totalExp.ToString();
            totalTimeLabel.Location = new Point(5, y += 20);
            totalTimeValue.Location = new Point(xPosition, y);

            long totalSeconds = (long)hunt.totalTime;
            string displayString = "";
            if (totalSeconds >= 3600) {
                displayString += (totalSeconds / 3600).ToString() + "h ";
                totalSeconds = totalSeconds % 3600;
            }
            if (totalSeconds >= 60) {
                displayString += (totalSeconds / 60).ToString() + "m ";
                totalSeconds = totalSeconds % 60;
            }
            displayString += totalSeconds.ToString() + "s";

            totalTimeValue.Text = displayString;
            y += 20;


            int widthSize = width_x / 3 - 5;
            lootButton.Size = new Size(widthSize, lootButton.Size.Height);
            lootButton.Location = new Point(5, y);
            allLootButton.Size = new Size(widthSize, lootButton.Size.Height);
            allLootButton.Location = new Point(7 + widthSize, y);
            rawLootButton.Size = new Size(widthSize, lootButton.Size.Height);
            rawLootButton.Location = new Point(10 + 2 * widthSize, y);

            y += allLootButton.Size.Height + 2;

            huntNameLabel.Size = new Size(width_x, huntNameLabel.Size.Height);
            this.Size = new Size(width_x, y + 5);
            lootLarger.Location = new Point(Size.Width - lootLarger.Size.Width - 4, 4);
            lootSmaller.Location = new Point(Size.Width - 2 * lootLarger.Size.Width - 4, 4);
        }

        public override void LoadForm() {
            this.SuspendForm();
            this.NotificationInitialize();

            lootSmaller.Click -= c_Click;
            lootLarger.Click -= c_Click;

            RefreshLoot();

            base.NotificationFinalize();
            this.ResumeForm();
        }

        private void Prevpage_Click(object sender, EventArgs e) {
            page--;
            this.SuspendForm();
            this.RefreshLoot();
            this.ResumeForm();
            this.Refresh();
            this.refreshTimer();
        }

        private void Nextpage_Click(object sender, EventArgs e) {
            page++;
            this.SuspendForm();
            this.RefreshLoot();
            this.ResumeForm();
            this.Refresh();
            this.refreshTimer();
        }

        private bool clicked = false;
        void openItemBox(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("item" + MainForm.commandSymbol + (sender as Control).Name);
        }

        void openCreatureDrops(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            if (creatures.Keys.Count == 1) {
                MainForm.mainForm.ExecuteCommand("creature" + MainForm.commandSymbol + (sender as Control).Name);
            } else {
                MainForm.mainForm.ExecuteCommand("loot" + MainForm.commandSymbol + (sender as Control).Name);
            }
        }

        private void huntNameLabel_Click(object sender, EventArgs e) {

        }

        private void rawLootButton_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("loot" + MainForm.commandSymbol + "raw");
        }

        private void allLootButton_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("loot" + MainForm.commandSymbol + "all");
        }

        private void lootButton_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("loot" + MainForm.commandSymbol);
        }

        private void changeSize(int modification) {
            int max_x = MainForm.mainForm.getSettingInt("LootFormWidth");
            if (max_x < minLootWidth) max_x = minLootWidth;
            max_x += modification;
            if (max_x < minLootWidth) max_x = minLootWidth;
            MainForm.mainForm.setSetting("LootFormWidth", (max_x).ToString());
            this.SuspendForm();
            this.RefreshLoot();
            this.ResumeForm();
            this.Refresh();
            this.refreshTimer();
        }

        private void lootSmaller_Click(object sender, EventArgs e) {
            changeSize(-72);
        }

        private void lootLarger_Click(object sender, EventArgs e) {
            changeSize(72);
        }
    }
}
