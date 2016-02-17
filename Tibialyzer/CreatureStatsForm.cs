
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
using System.Drawing.Drawing2D;

namespace Tibialyzer {
    public partial class CreatureStatsForm : NotificationForm {
        struct Resistance {
            public string name;
            public int resistance;
            public Resistance(string name, int resistance) {
                this.name = name;
                this.resistance = resistance;
            }
        }

        private ToolTip resistance_tooltip = new ToolTip();
        private System.Windows.Forms.PictureBox[] resistance_controls = new PictureBox[7];
        public Creature creature;
        public CreatureStatsForm() {
            InitializeComponent();

            resistance_tooltip.AutoPopDelay = 60000;
            resistance_tooltip.InitialDelay = 500;
            resistance_tooltip.ReshowDelay = 500;
            resistance_tooltip.ShowAlways = true;
            resistance_tooltip.UseFading = true;
            
            // add colors for every resistance
            resistance_controls[0] = resistanceLabel1;
            resistance_controls[1] = resistanceLabel2;
            resistance_controls[2] = resistanceLabel3;
            resistance_controls[3] = resistanceLabel4;
            resistance_controls[4] = resistanceLabel5;
            resistance_controls[5] = resistanceLabel6;
            resistance_controls[6] = resistanceLabel7;
        }
        
        private void AddResistances(List<Resistance> resistances) {
            List<Resistance> sorted_list = resistances.OrderByDescending(o => o.resistance).ToList();
            int i = 0;
            foreach (Resistance resistance in sorted_list) {
                resistance_tooltip.SetToolTip(resistance_controls[i], "Damage taken from " + resistance.name + ": " + resistance.resistance.ToString() + "%");

                // add a tooltip that displays the actual resistance when you mouseover
                Bitmap bitmap = new Bitmap(19 + resistance.resistance, 19);
                Graphics gr = Graphics.FromImage(bitmap);
                using (Brush brush = new SolidBrush(StyleManager.GetElementColor(resistance.name))) {
                    gr.FillRectangle(brush, new Rectangle(19, 0, bitmap.Width - 19, bitmap.Height));
                }
                gr.DrawRectangle(Pens.Black, new Rectangle(19, 0, bitmap.Width - 20, bitmap.Height - 1));
                gr.DrawImage(StyleManager.GetElementImage(resistance.name), new Point(2, 2));
                resistance_controls[i].Width = bitmap.Width;
                resistance_controls[i].Height = bitmap.Height;
                resistance_controls[i].Image = bitmap;
                i++;
            }
        }

        public override void LoadForm() {
            this.SuspendForm();
            int horizontal, left, right;
            this.statsButton.Name = creature.GetName().ToLower();
            this.huntButton.Name = creature.GetName().ToLower();
            int health = creature.health;
            int experience = creature.experience;
            List<Resistance> resistances = new List<Resistance>();
            resistances.Add(new Resistance("Ice", creature.res_ice));
            resistances.Add(new Resistance("Holy", creature.res_holy));
            resistances.Add(new Resistance("Death", creature.res_death));
            resistances.Add(new Resistance("Phys", creature.res_phys));
            resistances.Add(new Resistance("Earth", creature.res_earth));
            resistances.Add(new Resistance("Energy", creature.res_energy));
            resistances.Add(new Resistance("Fire", creature.res_fire));
            // load image from the creature
            this.mainImage.Image = creature.GetImage();
            // set health of creature
            this.healthLabel.Text = (health > 0 ? health.ToString() + " Health" : "Unknown");
            horizontal = 96 - healthLabel.Size.Width;
            left = horizontal / 2;
            right = horizontal - left;
            this.healthLabel.Padding = new Padding(left, 2, right, 2);
            // set exp of creature
            this.expLabel.Text = (experience >= 0 ? experience.ToString() : "Unknown") + " Exp";
            horizontal = 96 - expLabel.Size.Width;
            left = horizontal / 2;
            right = horizontal - left;
            this.expLabel.Padding = new Padding(left, 2, right, 2);
            // add resistances of creature in order
            AddResistances(resistances);
            // set background of actual form to transparent
            this.BackColor = StyleManager.NotificationBackgroundColor; ;
            this.Opacity = MainForm.opacity;
            if (MainForm.transparent) {
                this.TransparencyKey = StyleManager.NotificationBackgroundColor; ;
                this.Opacity = 1;
            }
            this.nameLabel.Text = MainForm.ToTitle(this.creature.displayname);
            Font f = MainForm.fontList[0];
            Font prevFont = f;
            for (int i = 0; i < MainForm.fontList.Count; i++) {
                Font font = MainForm.fontList[i];
                int width = TextRenderer.MeasureText(this.nameLabel.Text, font).Width;
                if (width < this.mainImage.Size.Width) {
                    f = prevFont;
                } else {
                    break;
                }
                prevFont = font;
            }

            string goldstring = "";
            double averageGold = 0;
            foreach(ItemDrop itemDrop in creature.itemdrops) {
                if (itemDrop.percentage > 0) {
                    Item item = MainForm.getItem(itemDrop.itemid);
                    averageGold += ((itemDrop.max + itemDrop.min) / 2.0) * itemDrop.percentage * item.GetMaxValue() / 100;
                }
            }
            if (averageGold < 10000) {
                goldstring = ((long)averageGold).ToString();
            } else if (averageGold < 1000000) {
                goldstring = ((long)averageGold / 1000).ToString() + "K";
            } else {
                goldstring = ((long)averageGold / 1000000).ToString() + "M";
            }
            this.averageGoldLabel.Text = "Average Gold: " + goldstring;
            
            this.maxDamageLabel.Text = "Max Damage: " + (this.creature.maxdamage >= 0 ? this.creature.maxdamage.ToString() : "-");
            this.abilitiesLabel.Text = RemoveTextInBrackets(this.creature.abilities.Replace(", ", "\n"));
            this.abilitiesLabel.BorderStyle = BorderStyle.FixedSingle;

            string tooltip;
            this.illusionableBox.Image = creature.illusionable ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            tooltip = creature.illusionable ? "Creature illusion works for this creature." : "Creature illusion does not work for this creature.";
            resistance_tooltip.SetToolTip(illusionableBox, tooltip);
            resistance_tooltip.SetToolTip(illusionableLabel, tooltip);
            this.summonableBox.Image = creature.summoncost > 0 ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            tooltip = creature.summoncost > 0 ? "This creature can be summoned for " + creature.summoncost + " mana." : "This creature cannot be summoned.";
            resistance_tooltip.SetToolTip(summonableBox, tooltip);
            resistance_tooltip.SetToolTip(summonableLabel, tooltip);
            this.invisibleBox.Image = !creature.senseinvis ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            tooltip = !creature.senseinvis ? "This creature does not detect invisibility." : "This creature detects invisibility.";
            resistance_tooltip.SetToolTip(invisibleBox, tooltip);
            resistance_tooltip.SetToolTip(invisibleLabel, tooltip);
            this.paralysableBox.Image = creature.paralysable ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            tooltip = creature.paralysable ? "This creature can be paralysed." : "This creature cannot be paralysed.";
            resistance_tooltip.SetToolTip(paralysableBox, tooltip);
            resistance_tooltip.SetToolTip(paralysableLabel, tooltip);
            this.pushableBox.Image = creature.pushable ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            tooltip = creature.pushable ? "This creature can be pushed." : "This creature cannot be pushed.";
            resistance_tooltip.SetToolTip(pushableBox, tooltip);
            resistance_tooltip.SetToolTip(pushableLabel, tooltip);
            this.pushesBox.Image = creature.pushes ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            tooltip = creature.pushes ? "This creature pushes smaller creatures." : "This creature cannot push smaller creatures.";
            resistance_tooltip.SetToolTip(pushesBox, tooltip);
            resistance_tooltip.SetToolTip(pushesLabel, tooltip);

            this.Size = new Size(this.Size.Width, (int)Math.Max(this.abilitiesLabel.Location.Y + this.abilitiesLabel.Size.Height + 10, this.expLabel.Location.Y + this.expLabel.Height + 10));
            this.nameLabel.Font = f;
            this.nameLabel.Left = this.mainImage.Left + (mainImage.Width - this.nameLabel.Size.Width) / 2;
            base.NotificationInitialize();

            List<Task> involvedTasks = new List<Task>();
            foreach(KeyValuePair<string, List<Task>> kvp in MainForm.taskList) {
                foreach(Task t in kvp.Value) {
                    if (t.bossid == creature.id) {
                        involvedTasks.Add(t);
                    }
                    foreach(int cr in t.creatures) {
                        if (cr == creature.id) {
                            involvedTasks.Add(t);
                        }
                    }
                }
            }
            if (involvedTasks.Count > 0) {
                int baseY = this.Size.Height;
                int newWidth = 0;
                int y = MainForm.DisplayCreatureAttributeList(Controls, involvedTasks.ToList<TibiaObject>(), 10, baseY, out newWidth);
                this.Size = new Size(Math.Max(newWidth, Size.Width), baseY + y);
            }

            base.NotificationFinalize();
            this.ResumeForm();
        }

        public string RemoveTextInBrackets(string str) {
            string ss = "";
            int kk;
            bool bracket = false;
            int items = 0;
            for (int i = 0; i < str.Length; i++) {
                if (bracket) {
                    if (str[i] == '-' || int.TryParse(str[i].ToString(), out kk)) {
                        ss = ss + str[i];
                        items++;
                    } else if (str[i] == ')') {
                        if (items == 0) {
                            ss = ss.Substring(0, ss.Length - 1);
                        } else {
                            ss = ss + str[i];
                        }
                        bracket = false;
                    }
                } else {
                    ss = ss + str[i];
                    if (str[i] == '(') {
                        bracket = true;
                        items = 0;
                    }
                }
            }
            return ss;
        }

        private bool clicked = false;
        private void statsButton_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("creature" + MainForm.commandSymbol + (sender as Control).Name);
        }

        private void huntButton_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("hunt" + MainForm.commandSymbol + (sender as Control).Name);
        }
    }
}
