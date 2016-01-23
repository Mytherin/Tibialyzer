
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace Tibialyzer {
    class QuestForm : NotificationForm {
        private Label levelLabel;
        private Label label2;
        private PictureBox premiumBox;
        private Label summonableLabel;
        private Label cityLabel;
        private Label label3;
        private Label legendLabel;
        private Label guideButton;
        private Label questTitle;
        public Quest quest;

        public QuestForm(Quest q) {
            this.quest = q;

            this.InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestForm));
            this.guideButton = new System.Windows.Forms.Label();
            this.legendLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.premiumBox = new System.Windows.Forms.PictureBox();
            this.summonableLabel = new System.Windows.Forms.Label();
            this.questTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.premiumBox)).BeginInit();
            this.SuspendLayout();
            // 
            // guideButton
            // 
            this.guideButton.BackColor = System.Drawing.Color.Transparent;
            this.guideButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.guideButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guideButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.guideButton.Location = new System.Drawing.Point(15, 69);
            this.guideButton.Name = "guideButton";
            this.guideButton.Padding = new System.Windows.Forms.Padding(2);
            this.guideButton.Size = new System.Drawing.Size(96, 21);
            this.guideButton.TabIndex = 30;
            this.guideButton.Text = "Guide";
            this.guideButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.guideButton.Click += new System.EventHandler(this.guideButton_Click);
            // 
            // legendLabel
            // 
            this.legendLabel.AutoSize = true;
            this.legendLabel.BackColor = System.Drawing.Color.Transparent;
            this.legendLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.legendLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.legendLabel.Location = new System.Drawing.Point(12, 93);
            this.legendLabel.MaximumSize = new System.Drawing.Size(300, 60);
            this.legendLabel.Name = "legendLabel";
            this.legendLabel.Padding = new System.Windows.Forms.Padding(3);
            this.legendLabel.Size = new System.Drawing.Size(160, 19);
            this.legendLabel.TabIndex = 29;
            this.legendLabel.Text = "This quest has no legend.";
            // 
            // cityLabel
            // 
            this.cityLabel.BackColor = System.Drawing.Color.Transparent;
            this.cityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.cityLabel.Location = new System.Drawing.Point(161, 59);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(100, 16);
            this.cityLabel.TabIndex = 28;
            this.cityLabel.Text = "Edron";
            this.cityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label3.Location = new System.Drawing.Point(259, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "City";
            // 
            // levelLabel
            // 
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
            this.levelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.levelLabel.Location = new System.Drawing.Point(211, 43);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(50, 16);
            this.levelLabel.TabIndex = 26;
            this.levelLabel.Text = "0";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label2.Location = new System.Drawing.Point(259, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Min Level";
            // 
            // premiumBox
            // 
            this.premiumBox.BackColor = System.Drawing.Color.Transparent;
            this.premiumBox.Location = new System.Drawing.Point(241, 26);
            this.premiumBox.Name = "premiumBox";
            this.premiumBox.Size = new System.Drawing.Size(16, 16);
            this.premiumBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.premiumBox.TabIndex = 23;
            this.premiumBox.TabStop = false;
            // 
            // summonableLabel
            // 
            this.summonableLabel.AutoSize = true;
            this.summonableLabel.BackColor = System.Drawing.Color.Transparent;
            this.summonableLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.summonableLabel.Location = new System.Drawing.Point(259, 28);
            this.summonableLabel.Name = "summonableLabel";
            this.summonableLabel.Size = new System.Drawing.Size(47, 13);
            this.summonableLabel.TabIndex = 24;
            this.summonableLabel.Text = "Premium";
            // 
            // questTitle
            // 
            this.questTitle.BackColor = System.Drawing.Color.Transparent;
            this.questTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.questTitle.Location = new System.Drawing.Point(11, 30);
            this.questTitle.Name = "questTitle";
            this.questTitle.Size = new System.Drawing.Size(200, 16);
            this.questTitle.TabIndex = 4;
            this.questTitle.Text = "Quest Name";
            // 
            // QuestForm
            // 
            this.ClientSize = new System.Drawing.Size(318, 153);
            this.Controls.Add(this.guideButton);
            this.Controls.Add(this.legendLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.premiumBox);
            this.Controls.Add(this.summonableLabel);
            this.Controls.Add(this.questTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QuestForm";
            ((System.ComponentModel.ISupportInitialize)(this.premiumBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override void LoadForm() {
            if (quest == null) return;
            this.SuspendLayout();
            NotificationInitialize();

            this.questTitle.Text = quest.name;
            this.premiumBox.Image = quest.premium ? MainForm.checkmark_yes : MainForm.checkmark_no;
            this.cityLabel.Text = quest.city == null ? "Unknown" : MainForm.ToTitle(quest.city);
            this.levelLabel.Text = quest.minlevel.ToString();
            this.legendLabel.Text = quest.legend;

            List<TibiaObject> rewards = new List<TibiaObject>();
            foreach(int reward in quest.rewardItems) {
                Item item = MainForm.getItem(reward);
                rewards.Add(item);
            }
            rewards = rewards.OrderByDescending(o => (o as Item).GetMaxValue()).ToList<TibiaObject>();
            int y = -1;
            using (Graphics gr = Graphics.FromHwnd(legendLabel.Handle)) {
                y = this.legendLabel.Location.Y + (int)gr.MeasureString(this.legendLabel.Text, this.legendLabel.Font, this.legendLabel.MaximumSize.Width).Height + 20;
            }
           
            if (rewards.Count > 0 || quest.rewardOutfits.Count > 0) {
                Label label = new Label();
                label.Text = "Rewards";
                label.Location = new Point(40, y);
                label.ForeColor = MainForm.label_text_color;
                label.BackColor = Color.Transparent;
                label.Font = questTitle.Font;
                this.Controls.Add(label);
                y += 25;
                if (rewards.Count > 0) {
                    List<Control> itemControls = new List<Control>();
                    y = y + MainForm.DisplayCreatureList(this.Controls, rewards, 10, y, this.Size.Width - 10, 1, null, 1, itemControls);
                }
                if (quest.rewardOutfits.Count > 0) {
                    List<Control> outfitControls = new List<Control>();

                    List<TibiaObject> rewardOutfits = new List<TibiaObject>();
                    foreach (int reward in quest.rewardOutfits) {
                        Outfit outfit = MainForm.getOutfit(reward);
                        rewardOutfits.Add(outfit);
                    }

                    y = y + MainForm.DisplayCreatureList(this.Controls, rewardOutfits, 10, y, this.Size.Width - 10, 4, null, 1, outfitControls);
                }
            }
            this.Size = new Size(this.Size.Width, y + 20);

            base.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private bool clicked = false;
        private void outfitClick(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("outfit" + MainForm.commandSymbol + (sender as Control).Name);
        }

        private void itemClick(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("item" + MainForm.commandSymbol + (sender as Control).Name);
        }

        private void guideButton_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("guide" + MainForm.commandSymbol + quest.name.ToLower());
        }
    }
}
