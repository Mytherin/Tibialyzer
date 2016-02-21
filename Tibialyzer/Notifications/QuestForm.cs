
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
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Tibialyzer {
    class QuestForm : NotificationForm {
        private Label levelLabel;
        private Label label2;
        private PictureBox premiumBox;
        private Label summonableLabel;
        private Label cityLabel;
        private Label label3;
        private Label legendLabel;
        private Label questTitle;
        private Label wikiButton;
        public Quest quest;

        public QuestForm(Quest q) {
            this.quest = q;

            this.InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestForm));
            this.legendLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.premiumBox = new System.Windows.Forms.PictureBox();
            this.summonableLabel = new System.Windows.Forms.Label();
            this.questTitle = new System.Windows.Forms.Label();
            this.wikiButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.premiumBox)).BeginInit();
            this.SuspendLayout();
            //
            // legendLabel
            //
            this.legendLabel.AutoSize = true;
            this.legendLabel.BackColor = System.Drawing.Color.Transparent;
            this.legendLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.legendLabel.ForeColor = StyleManager.NotificationTextColor;
            this.legendLabel.Location = new System.Drawing.Point(12, 101);
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
            this.cityLabel.ForeColor = StyleManager.NotificationTextColor;
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
            this.label3.ForeColor = StyleManager.NotificationTextColor;
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
            this.levelLabel.ForeColor = StyleManager.NotificationTextColor;
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
            this.label2.ForeColor = StyleManager.NotificationTextColor;
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
            this.summonableLabel.ForeColor = StyleManager.NotificationTextColor;
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
            this.questTitle.ForeColor = StyleManager.NotificationTextColor;
            this.questTitle.Location = new System.Drawing.Point(11, 30);
            this.questTitle.Name = "questTitle";
            this.questTitle.Size = new System.Drawing.Size(200, 16);
            this.questTitle.TabIndex = 4;
            this.questTitle.Text = "Quest Name";
            //
            // wikiButton
            //
            this.wikiButton.BackColor = System.Drawing.Color.Transparent;
            this.wikiButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wikiButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wikiButton.ForeColor = StyleManager.NotificationTextColor;
            this.wikiButton.Location = new System.Drawing.Point(11, 53);
            this.wikiButton.Name = "wikiButton";
            this.wikiButton.Padding = new System.Windows.Forms.Padding(2);
            this.wikiButton.Size = new System.Drawing.Size(96, 21);
            this.wikiButton.TabIndex = 31;
            this.wikiButton.Text = "Wiki";
            this.wikiButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.wikiButton.Click += new System.EventHandler(this.wikiButton_Click);
            //
            // QuestForm
            //
            this.ClientSize = new System.Drawing.Size(318, 153);
            this.Controls.Add(this.wikiButton);
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

            wikiButton.Click -= c_Click;

            this.questTitle.Text = quest.name;
            this.premiumBox.Image = quest.premium ? StyleManager.GetImage("checkmark-yes.png") : StyleManager.GetImage("checkmark-no.png");
            this.cityLabel.Text = quest.city == null ? "Unknown" : quest.city.ToTitle();
            this.levelLabel.Text = quest.minlevel.ToString();
            this.legendLabel.Text = quest.legend;

            List<TibiaObject> rewards = new List<TibiaObject>();
            foreach(int reward in quest.rewardItems) {
                Item item = StorageManager.getItem(reward);
                rewards.Add(item);
            }
            rewards = rewards.OrderByDescending(o => (o as Item).GetMaxValue()).ToList<TibiaObject>();
            int x = 5;
            int y = 77;
            foreach (string missionName in quest.questInstructions.Keys) {
                if (quest.questInstructions[missionName].Count == 0) continue;
                if (x + 150 >= this.Size.Width) {
                    x = 5;
                    y += 25;
                }
                Label missionButton = new Label();
                missionButton.BackColor = System.Drawing.Color.Transparent;
                missionButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                missionButton.Font = wikiButton.Font;
                missionButton.ForeColor = StyleManager.NotificationTextColor;
                missionButton.Location = new System.Drawing.Point(x, y);
                missionButton.Name = quest.questInstructions[missionName][0].specialCommand != null ? quest.questInstructions[missionName][0].specialCommand : "guide" + Constants.CommandSymbol + quest.name.ToLower() + Constants.CommandSymbol + "1" + Constants.CommandSymbol + missionName;
                missionButton.Padding = new System.Windows.Forms.Padding(2);
                missionButton.Text = missionName;
                missionButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                missionButton.Click += MissionButton_Click;
                missionButton.Size = new Size(150, 21);
                this.Controls.Add(missionButton);
                x += missionButton.Width + 5;
            }
            y += 25;
            using (Graphics gr = Graphics.FromHwnd(legendLabel.Handle)) {
                this.legendLabel.Location = new Point(legendLabel.Location.X, y);
                y += (int)gr.MeasureString(this.legendLabel.Text, this.legendLabel.Font, this.legendLabel.MaximumSize.Width).Height + 20;
            }

            if (this.quest.additionalRequirements.Count > 0 || this.quest.questRequirements.Count > 0) {
                Label label = new Label();
                label.Text = "Requirements";
                label.Location = new Point(5, y);
                label.ForeColor = StyleManager.NotificationTextColor;
                label.BackColor = Color.Transparent;
                label.Font = questTitle.Font;
                label.Size = new Size(this.Size.Width - 10, label.Height);
                this.Controls.Add(label);
                y += 25;

                // Item requirements
                if (this.quest.questRequirements.Count > 0) {

                    List<Tuple<int, Item>> requirements = new List<Tuple<int, Item>>();
                    foreach (Tuple<int, int> tpl in quest.questRequirements) {
                        Item item = StorageManager.getItem(tpl.Item2);
                        requirements.Add(new Tuple<int, Item>(tpl.Item1, item));
                    }
                    requirements = requirements.OrderBy(o => o.Item1 * o.Item2.GetMaxValue()).ToList();
                    List<TibiaObject> itemList = requirements.Select(o => o.Item2).ToList<TibiaObject>();

                    List<Control> itemControls = new List<Control>();
                    y = y + MainForm.DisplayCreatureList(this.Controls, itemList, 10, y, this.Size.Width - 10, 1, null, 1, itemControls);
                    int itemnr = 0;
                    foreach (Control control in itemControls) {
                        control.BackgroundImage = StyleManager.GetImage("item_background.png");
                        int itemCount = requirements[itemnr].Item1;
                        Item item = requirements[itemnr].Item2;

                        (control as PictureBox).Image = LootDropForm.DrawCountOnItem(item, itemCount);

                        itemnr++;
                    }
                }

                // Text requirements
                if (this.quest.additionalRequirements.Count > 0) {
                    List<string> requirementStrings = this.quest.additionalRequirements.ToArray().ToList();
                    if (this.quest.minlevel > 0) {
                        requirementStrings.Add(String.Format("You must be at least level {0}.", this.quest.minlevel));
                    }

                    y += 5;
                    Regex questRegex = new Regex("\\[([^]]+)\\]");
                    foreach (string text in requirementStrings) {
                        label = new Label();
                        string txt = text;
                        Match m = questRegex.Match(txt);
                        label.ForeColor = StyleManager.NotificationTextColor;
                        if (m != null && m.Groups.Count > 1) {
                            string requiredQuestName = m.Groups[1].Value;
                            txt = txt.Replace(m.Groups[0].Value, requiredQuestName);
                            label.Name = StorageManager.getQuest(requiredQuestName.ToLower()).GetCommand();
                            label.ForeColor = StyleManager.ClickableLinkColor;
                            label.Click += MissionButton_Click;
                        }
                        label.Text = txt == "" ? "" : "- " + txt;
                        label.Location = new Point(5, y);
                        label.BackColor = Color.Transparent;
                        label.Font = QuestGuideForm.requirementFont;
                        Size size;
                        using (Graphics gr = Graphics.FromHwnd(label.Handle)) {
                            size = gr.MeasureString(label.Text, label.Font, this.Size.Width - 50).ToSize();
                            label.Size = new Size(this.Size.Width - 10, (int)(size.Height * 1.2));
                        }
                        this.Controls.Add(label);
                        y += label.Size.Height;
                    }
                }
            }

            if (rewards.Count > 0 || quest.rewardOutfits.Count > 0) {
                Label label = new Label();
                label.Text = "Rewards";
                label.Location = new Point(40, y);
                label.ForeColor = StyleManager.NotificationTextColor;
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
                        Outfit outfit = StorageManager.getOutfit(reward);
                        rewardOutfits.Add(outfit);
                    }

                    y = y + MainForm.DisplayCreatureList(this.Controls, rewardOutfits, 10, y, this.Size.Width - 10, 4, null, 1, outfitControls);
                }
            }
            this.Size = new Size(this.Size.Width, y + 20);

            base.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private void outfitClick(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            CommandManager.ExecuteCommand("outfit" + Constants.CommandSymbol + (sender as Control).Name);
        }

        private void itemClick(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            CommandManager.ExecuteCommand("item" + Constants.CommandSymbol + (sender as Control).Name);
        }
        private void MissionButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand((sender as Control).Name);
        }

        private void wikiButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl(String.Format("http://tibia.wikia.com/wiki/{0}/Spoiler", quest.title));
        }
    }
}
