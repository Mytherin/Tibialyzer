
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class SpellForm : NotificationForm {
        private Label spellTitle;
        private PictureBox spellImageBox;
        private Label spellWords;
        private Label manaCostLabel;
        private Label goldLabel;
        private PictureBox premiumBox;
        private Label summonableLabel;
        private PictureBox promotionBox;
        private Label label1;
        private Label label4;
        private Label label3;
        private Label cooldownLabel;
        private Label label2;
        private Label levelLabel;
        private Label label6;
        private PictureBox knightBox;
        private PictureBox paladinBox;
        private PictureBox druidBox;
        private PictureBox sorcererBox;
        public Spell spell;

        public SpellForm(Spell spell, int initialVocation) {
            this.InitializeComponent();
            this.spell = spell;
            this.currentVocation = initialVocation;
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpellForm));
            this.levelLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cooldownLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.promotionBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.premiumBox = new System.Windows.Forms.PictureBox();
            this.summonableLabel = new System.Windows.Forms.Label();
            this.goldLabel = new System.Windows.Forms.Label();
            this.manaCostLabel = new System.Windows.Forms.Label();
            this.spellWords = new System.Windows.Forms.Label();
            this.spellImageBox = new System.Windows.Forms.PictureBox();
            this.spellTitle = new System.Windows.Forms.Label();
            this.knightBox = new System.Windows.Forms.PictureBox();
            this.paladinBox = new System.Windows.Forms.PictureBox();
            this.druidBox = new System.Windows.Forms.PictureBox();
            this.sorcererBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.promotionBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.premiumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spellImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.knightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paladinBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.druidBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sorcererBox)).BeginInit();
            this.SuspendLayout();
            // 
            // levelLabel
            // 
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
            this.levelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelLabel.ForeColor = StyleManager.NotificationTextColor;
            this.levelLabel.Location = new System.Drawing.Point(229, 108);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(50, 16);
            this.levelLabel.TabIndex = 24;
            this.levelLabel.Text = "14";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = StyleManager.NotificationTextColor;
            this.label6.Location = new System.Drawing.Point(277, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Level";
            // 
            // cooldownLabel
            // 
            this.cooldownLabel.BackColor = System.Drawing.Color.Transparent;
            this.cooldownLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cooldownLabel.ForeColor = StyleManager.NotificationTextColor;
            this.cooldownLabel.Location = new System.Drawing.Point(229, 90);
            this.cooldownLabel.Name = "cooldownLabel";
            this.cooldownLabel.Size = new System.Drawing.Size(50, 16);
            this.cooldownLabel.TabIndex = 22;
            this.cooldownLabel.Text = "2s";
            this.cooldownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = StyleManager.NotificationTextColor;
            this.label2.Location = new System.Drawing.Point(277, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Cooldown";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = StyleManager.NotificationTextColor;
            this.label3.Location = new System.Drawing.Point(277, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Goldcost";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = StyleManager.NotificationTextColor;
            this.label4.Location = new System.Drawing.Point(277, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Manacost";
            // 
            // promotionBox
            // 
            this.promotionBox.BackColor = System.Drawing.Color.Transparent;
            this.promotionBox.Location = new System.Drawing.Point(259, 36);
            this.promotionBox.Name = "promotionBox";
            this.promotionBox.Size = new System.Drawing.Size(16, 16);
            this.promotionBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.promotionBox.TabIndex = 15;
            this.promotionBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = StyleManager.NotificationTextColor;
            this.label1.Location = new System.Drawing.Point(277, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Promotion";
            // 
            // premiumBox
            // 
            this.premiumBox.BackColor = System.Drawing.Color.Transparent;
            this.premiumBox.Location = new System.Drawing.Point(259, 18);
            this.premiumBox.Name = "premiumBox";
            this.premiumBox.Size = new System.Drawing.Size(16, 16);
            this.premiumBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.premiumBox.TabIndex = 13;
            this.premiumBox.TabStop = false;
            // 
            // summonableLabel
            // 
            this.summonableLabel.AutoSize = true;
            this.summonableLabel.BackColor = System.Drawing.Color.Transparent;
            this.summonableLabel.ForeColor = StyleManager.NotificationTextColor;
            this.summonableLabel.Location = new System.Drawing.Point(277, 20);
            this.summonableLabel.Name = "summonableLabel";
            this.summonableLabel.Size = new System.Drawing.Size(47, 13);
            this.summonableLabel.TabIndex = 14;
            this.summonableLabel.Text = "Premium";
            // 
            // goldLabel
            // 
            this.goldLabel.BackColor = System.Drawing.Color.Transparent;
            this.goldLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goldLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(226)))), ((int)(((byte)(24)))));
            this.goldLabel.Location = new System.Drawing.Point(229, 72);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(50, 16);
            this.goldLabel.TabIndex = 4;
            this.goldLabel.Text = "800";
            this.goldLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // manaCostLabel
            // 
            this.manaCostLabel.BackColor = System.Drawing.Color.Transparent;
            this.manaCostLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manaCostLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(180)))), ((int)(((byte)(176)))));
            this.manaCostLabel.Location = new System.Drawing.Point(229, 55);
            this.manaCostLabel.Name = "manaCostLabel";
            this.manaCostLabel.Size = new System.Drawing.Size(50, 16);
            this.manaCostLabel.TabIndex = 3;
            this.manaCostLabel.Text = "20";
            this.manaCostLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // spellWords
            // 
            this.spellWords.AutoSize = true;
            this.spellWords.BackColor = System.Drawing.Color.Transparent;
            this.spellWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spellWords.ForeColor = StyleManager.NotificationTextColor;
            this.spellWords.Location = new System.Drawing.Point(82, 49);
            this.spellWords.Name = "spellWords";
            this.spellWords.Size = new System.Drawing.Size(66, 16);
            this.spellWords.TabIndex = 2;
            this.spellWords.Text = "exori frigo";
            // 
            // spellImageBox
            // 
            this.spellImageBox.BackColor = System.Drawing.Color.Transparent;
            this.spellImageBox.Location = new System.Drawing.Point(13, 28);
            this.spellImageBox.Name = "spellImageBox";
            this.spellImageBox.Size = new System.Drawing.Size(64, 64);
            this.spellImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.spellImageBox.TabIndex = 1;
            this.spellImageBox.TabStop = false;
            // 
            // spellTitle
            // 
            this.spellTitle.AutoSize = true;
            this.spellTitle.BackColor = System.Drawing.Color.Transparent;
            this.spellTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spellTitle.ForeColor = StyleManager.NotificationTextColor;
            this.spellTitle.Location = new System.Drawing.Point(82, 29);
            this.spellTitle.Name = "spellTitle";
            this.spellTitle.Size = new System.Drawing.Size(86, 20);
            this.spellTitle.TabIndex = 0;
            this.spellTitle.Text = "Ice Strike";
            // 
            // knightBox
            // 
            this.knightBox.BackColor = System.Drawing.Color.Transparent;
            this.knightBox.Location = new System.Drawing.Point(86, 74);
            this.knightBox.Name = "knightBox";
            this.knightBox.Size = new System.Drawing.Size(24, 24);
            this.knightBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.knightBox.TabIndex = 25;
            this.knightBox.TabStop = false;
            // 
            // paladinBox
            // 
            this.paladinBox.BackColor = System.Drawing.Color.Transparent;
            this.paladinBox.Location = new System.Drawing.Point(116, 74);
            this.paladinBox.Name = "paladinBox";
            this.paladinBox.Size = new System.Drawing.Size(24, 24);
            this.paladinBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.paladinBox.TabIndex = 26;
            this.paladinBox.TabStop = false;
            // 
            // druidBox
            // 
            this.druidBox.BackColor = System.Drawing.Color.Transparent;
            this.druidBox.Location = new System.Drawing.Point(146, 74);
            this.druidBox.Name = "druidBox";
            this.druidBox.Size = new System.Drawing.Size(24, 24);
            this.druidBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.druidBox.TabIndex = 27;
            this.druidBox.TabStop = false;
            // 
            // sorcererBox
            // 
            this.sorcererBox.BackColor = System.Drawing.Color.Transparent;
            this.sorcererBox.Location = new System.Drawing.Point(176, 74);
            this.sorcererBox.Name = "sorcererBox";
            this.sorcererBox.Size = new System.Drawing.Size(24, 24);
            this.sorcererBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sorcererBox.TabIndex = 28;
            this.sorcererBox.TabStop = false;
            // 
            // SpellForm
            // 
            this.ClientSize = new System.Drawing.Size(346, 139);
            this.Controls.Add(this.sorcererBox);
            this.Controls.Add(this.druidBox);
            this.Controls.Add(this.paladinBox);
            this.Controls.Add(this.knightBox);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cooldownLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.promotionBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.premiumBox);
            this.Controls.Add(this.summonableLabel);
            this.Controls.Add(this.goldLabel);
            this.Controls.Add(this.manaCostLabel);
            this.Controls.Add(this.spellWords);
            this.Controls.Add(this.spellImageBox);
            this.Controls.Add(this.spellTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpellForm";
            ((System.ComponentModel.ISupportInitialize)(this.promotionBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.premiumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spellImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.knightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paladinBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.druidBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sorcererBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private List<TibiaObject>[] npcList = new List<TibiaObject>[4];
        private Control[] vocationControls = new Control[4];
        public override void LoadForm() {
            if (spell == null) return;
            this.SuspendLayout();
            NotificationInitialize();

            this.spellImageBox.Image = spell.GetImage();
            this.spellTitle.Text = spell.name;
            this.spellWords.Text = spell.words;
            if (StyleManager.ElementExists(spell.element)) {
                this.spellTitle.ForeColor = StyleManager.GetElementColor(spell.element);
                this.spellWords.ForeColor = StyleManager.GetElementColor(spell.element);
            }
            this.goldLabel.Text = spell.goldcost.ToString();
            this.manaCostLabel.Text = spell.manacost.ToString();
            this.cooldownLabel.Text = spell.cooldown.ToString() + "s";
            this.levelLabel.Text = spell.levelrequired.ToString();
            this.premiumBox.Image = spell.premium ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            this.promotionBox.Image = spell.promotion ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");

            if (this.spell.knight) {
                this.knightBox.Image = StyleManager.GetImage("knight.png");
            }
            if (this.spell.paladin) {
                this.paladinBox.Image = StyleManager.GetImage("paladin.png");
            }
            if (this.spell.sorcerer) {
                this.sorcererBox.Image = StyleManager.GetImage("sorcerer.png");
            }
            if (this.spell.druid) {
                this.druidBox.Image = StyleManager.GetImage("druid.png");
            }

            string[] titles = new string[] { "Knight", "Druid", "Paladin", "Sorcerer" };
            for (int i = 0; i < 4; i++) {
                npcList[i] = new List<TibiaObject>();
            }
            for (int i = 0; i < 4; i++) {
                foreach (SpellTaught teach in spell.teachNPCs) {
                    if (teach.GetVocation(i)) {
                        npcList[i].Add(new LazyTibiaObject { id = teach.npcid, type = TibiaObjectType.NPC });
                    }
                }
            }

            int y = this.Height - 10;
            baseY = y + 35;
            int x = 5;
            for (int i = 0; i < 4; i++) {
                if (npcList[i].Count > 0) {
                    Label label = new Label();
                    label.Text = titles[i];
                    label.Location = new Point(x, y);
                    label.ForeColor = StyleManager.NotificationTextColor;
                    label.BackColor = Color.Transparent;
                    label.Font = MainForm.text_font;
                    label.Size = new Size(70, 25);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    x += 70;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Name = i.ToString();
                    label.Click += toggleVocationSpells;
                    vocationControls[i] = label;
                    this.Controls.Add(label);
                    if (currentVocation < 0 || currentVocation > 3) {
                        currentVocation = i;
                    }
                } else {
                    vocationControls[i] = null;
                }
            }
            refreshVocationSpells();
            base.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private void toggleVocationSpells(object sender, EventArgs e) {
            this.currentVocation = int.Parse((sender as Control).Name);
            this.SuspendForm();
            refreshVocationSpells();
            this.ResumeForm();
        }

        int baseY;
        int currentVocation = 0;
        List<Control> npcControls = new List<Control>();

        void updateCommand() {
            string[] split = command.command.Split(MainForm.commandSymbol);
            command.command = split[0] + MainForm.commandSymbol + split[1] + MainForm.commandSymbol + currentVocation.ToString();
        }

        private string sortedHeader = null;
        private bool desc = false;
        public void sortHeader(object sender, EventArgs e) {
            if (sortedHeader == (sender as Control).Name) {
                desc = !desc;
            } else {
                sortedHeader = (sender as Control).Name;
                desc = false;
            }
            this.SuspendForm();
            refreshVocationSpells();
            this.ResumeForm();
        }

        private void refreshVocationSpells() {
            updateCommand();
            foreach (Control c in npcControls) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            npcControls.Clear();
            for (int i = 0; i < 4; i++) {
                if (vocationControls[i] != null) {
                    vocationControls[i].Enabled = i != currentVocation;
                    if (i == currentVocation) {
                        (vocationControls[i] as Label).BorderStyle = BorderStyle.Fixed3D;
                    } else {
                        (vocationControls[i] as Label).BorderStyle = BorderStyle.FixedSingle;
                    }
                }
            }
            int newwidth = 0;
            int y = baseY;
            if (currentVocation >= 0) {
                y = baseY + MainForm.DisplayCreatureAttributeList(this.Controls, npcList[currentVocation], 10, baseY, out newwidth, null, npcControls, 0, 10, null, "Cost", goldCostFunction, sortHeader, sortedHeader, desc);
            }
            this.Size = new Size(Math.Max(this.Size.Width, newwidth), y + 20);
        }

        private Attribute goldCostFunction(TibiaObject obj) {
            return new StringAttribute(spell.goldcost.ToString(), 50, StyleManager.ItemGoldColor);
        }

        private string command_start = "npc" + MainForm.commandSymbol;
        private bool clicked = false;
        private void npcClick(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand(command_start + (sender as Control).Name);
        }
    }
}
