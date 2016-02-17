
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer {
    class HuntingPlaceForm : NotificationForm {
        public HuntingPlace hunting_place = null;
        private Label huntingPlaceName;
        private Label creatureLabel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private Label experienceLabel;
        private Label label1;
        private System.Windows.Forms.PictureBox experienceStarBox;
        private System.Windows.Forms.PictureBox lootStarBox;
        private Label levelLabel;
        private Label cityLabel;
        private Label requirementLabel;
        private Label guideButton;
        private static Font requirement_font = new Font(FontFamily.GenericSansSerif, 7.5f, FontStyle.Bold);
        private Coordinate targetCoordinate;

        public HuntingPlaceForm() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HuntingPlaceForm));
            this.requirementLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.lootStarBox = new System.Windows.Forms.PictureBox();
            this.experienceStarBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.experienceLabel = new System.Windows.Forms.Label();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.creatureLabel = new System.Windows.Forms.Label();
            this.huntingPlaceName = new System.Windows.Forms.Label();
            this.mapBox = new Tibialyzer.MapPictureBox();
            this.guideButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lootStarBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.experienceStarBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            this.SuspendLayout();
            // 
            // requirementLabel
            // 
            this.requirementLabel.AutoSize = true;
            this.requirementLabel.BackColor = System.Drawing.Color.Transparent;
            this.requirementLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requirementLabel.ForeColor = StyleManager.NotificationTextColor;
            this.requirementLabel.Location = new System.Drawing.Point(37, 121);
            this.requirementLabel.Name = "requirementLabel";
            this.requirementLabel.Size = new System.Drawing.Size(104, 16);
            this.requirementLabel.TabIndex = 13;
            this.requirementLabel.Text = "Requirements";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.BackColor = System.Drawing.Color.Transparent;
            this.cityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cityLabel.ForeColor = StyleManager.NotificationTextColor;
            this.cityLabel.Location = new System.Drawing.Point(7, 58);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(34, 16);
            this.cityLabel.TabIndex = 12;
            this.cityLabel.Text = "City";
            this.cityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cityLabel.Click += new System.EventHandler(this.cityLabel_Click);
            // 
            // levelLabel
            // 
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
            this.levelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelLabel.ForeColor = StyleManager.NotificationTextColor;
            this.levelLabel.Location = new System.Drawing.Point(111, 58);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(54, 16);
            this.levelLabel.TabIndex = 11;
            this.levelLabel.Text = "Level: ";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lootStarBox
            // 
            this.lootStarBox.BackColor = System.Drawing.Color.Transparent;
            this.lootStarBox.Location = new System.Drawing.Point(100, 96);
            this.lootStarBox.Name = "lootStarBox";
            this.lootStarBox.Size = new System.Drawing.Size(61, 20);
            this.lootStarBox.TabIndex = 10;
            this.lootStarBox.TabStop = false;
            // 
            // experienceStarBox
            // 
            this.experienceStarBox.BackColor = System.Drawing.Color.Transparent;
            this.experienceStarBox.Location = new System.Drawing.Point(8, 96);
            this.experienceStarBox.Name = "experienceStarBox";
            this.experienceStarBox.Size = new System.Drawing.Size(61, 20);
            this.experienceStarBox.TabIndex = 9;
            this.experienceStarBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = StyleManager.NotificationTextColor;
            this.label1.Location = new System.Drawing.Point(111, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Loot";
            // 
            // experienceLabel
            // 
            this.experienceLabel.AutoSize = true;
            this.experienceLabel.BackColor = System.Drawing.Color.Transparent;
            this.experienceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.experienceLabel.ForeColor = StyleManager.NotificationTextColor;
            this.experienceLabel.Location = new System.Drawing.Point(23, 77);
            this.experienceLabel.Name = "experienceLabel";
            this.experienceLabel.Size = new System.Drawing.Size(34, 16);
            this.experienceLabel.TabIndex = 7;
            this.experienceLabel.Text = "Exp";
            // 
            // mapDownLevel
            // 
            this.mapDownLevel.Location = new System.Drawing.Point(170, 33);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 6;
            this.mapDownLevel.TabStop = false;
            // 
            // mapUpLevel
            // 
            this.mapUpLevel.Location = new System.Drawing.Point(170, 12);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 5;
            this.mapUpLevel.TabStop = false;
            // 
            // creatureLabel
            // 
            this.creatureLabel.AutoSize = true;
            this.creatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.creatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureLabel.ForeColor = StyleManager.NotificationTextColor;
            this.creatureLabel.Location = new System.Drawing.Point(39, 211);
            this.creatureLabel.Name = "creatureLabel";
            this.creatureLabel.Size = new System.Drawing.Size(75, 16);
            this.creatureLabel.TabIndex = 4;
            this.creatureLabel.Text = "Creatures";
            // 
            // huntingPlaceName
            // 
            this.huntingPlaceName.BackColor = System.Drawing.Color.Transparent;
            this.huntingPlaceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntingPlaceName.ForeColor = StyleManager.NotificationTextColor;
            this.huntingPlaceName.Location = new System.Drawing.Point(6, 27);
            this.huntingPlaceName.Name = "huntingPlaceName";
            this.huntingPlaceName.Size = new System.Drawing.Size(158, 34);
            this.huntingPlaceName.TabIndex = 3;
            this.huntingPlaceName.Text = "Brimstone Cave";
            this.huntingPlaceName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // mapBox
            // 
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapBox.Location = new System.Drawing.Point(170, 12);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(195, 190);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            // 
            // guideButton
            // 
            this.guideButton.BackColor = System.Drawing.Color.Transparent;
            this.guideButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.guideButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guideButton.ForeColor = StyleManager.NotificationTextColor;
            this.guideButton.Location = new System.Drawing.Point(269, 209);
            this.guideButton.Name = "guideButton";
            this.guideButton.Padding = new System.Windows.Forms.Padding(2);
            this.guideButton.Size = new System.Drawing.Size(96, 21);
            this.guideButton.TabIndex = 31;
            this.guideButton.Text = "Guide";
            this.guideButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.guideButton.Click += new System.EventHandler(this.guideButton_Click);
            // 
            // HuntingPlaceForm
            // 
            this.ClientSize = new System.Drawing.Size(382, 273);
            this.Controls.Add(this.guideButton);
            this.Controls.Add(this.requirementLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.lootStarBox);
            this.Controls.Add(this.experienceStarBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.experienceLabel);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.creatureLabel);
            this.Controls.Add(this.huntingPlaceName);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HuntingPlaceForm";
            this.Text = "Hunting Place";
            ((System.ComponentModel.ISupportInitialize)(this.lootStarBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.experienceStarBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MapPictureBox mapBox;

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                base.Cleanup();
            }
            base.Dispose(disposing);
        }

        private List<Control> creatureControls = new List<Control>();
        private int currentPage = 0;
        private string sortedHeader = "";
        private bool desc = false;
        private int baseY;

        public override void LoadForm() {
            this.SuspendForm();
            NotificationInitialize();
            if (hunting_place == null) return;
            this.cityLabel.Text = hunting_place.city;
            this.huntingPlaceName.Text = MainForm.ToTitle(hunting_place.name);
            this.levelLabel.Text = hunting_place.level < 0 ? "--" : hunting_place.level.ToString();

            if (hunting_place.directions.Count == 0) {
                guideButton.Visible = false;
            }

            int y;
            ToolTip tooltip = new ToolTip();
            tooltip.AutoPopDelay = 60000;
            tooltip.InitialDelay = 500;
            tooltip.ReshowDelay = 0;
            tooltip.ShowAlways = true;
            tooltip.UseFading = true;

            if (this.hunting_place.coordinates != null && this.hunting_place.coordinates.Count > 0) {
                int count = 1;
                foreach (Coordinate coordinate in this.hunting_place.coordinates) {
                    Label label = new Label();
                    label.ForeColor = StyleManager.NotificationTextColor;
                    label.BackColor = Color.Transparent;
                    label.Name = (count - 1).ToString();
                    label.Font = LootDropForm.loot_font;
                    label.Text = count.ToString();
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Size = new Size(1, 1);
                    label.AutoSize = true;
                    label.Location = new Point(mapBox.Location.X + (count - 1) * 25, mapBox.Location.Y + mapBox.Size.Height + 5);
                    label.Click += label_Click;
                    this.Controls.Add(label);
                    count++;
                }
                targetCoordinate = this.hunting_place.coordinates[0];

            } else {
                targetCoordinate = new Coordinate();
            }

            if (hunting_place.requirements != null && hunting_place.requirements.Count > 0) {
                int count = 0;
                y = 3;
                foreach (Requirements requirement in hunting_place.requirements) {
                    Label label = new Label();
                    label.ForeColor = Color.Firebrick;
                    label.BackColor = Color.Transparent;
                    label.Font = requirement_font;
                    label.Location = new Point(3, requirementLabel.Location.Y + requirementLabel.Size.Height + y);
                    label.AutoSize = true;
                    label.MaximumSize = new Size(170, 0);
                    label.Text = "- " + requirement.notes;
                    label.Name = requirement.quest.name.ToString();
                    label.Click += openQuest;
                    using (Graphics graphics = label.CreateGraphics()) {
                        y += (int)(Math.Ceiling(graphics.MeasureString(label.Text, label.Font).Width / 170.0)) * 14;
                    }
                    this.Controls.Add(label);
                    count++;
                }
            } else {
                this.requirementLabel.Hide();
            }

            baseY = this.creatureLabel.Location.Y + this.creatureLabel.Height + 5;


            //y = MainForm.DisplayCreatureList(this.Controls, creatures, 10, base_y, this.Size.Width, 4, null, 0.8f);

            Font f = MainForm.fontList[0];
            for (int i = 0; i < MainForm.fontList.Count; i++) {
                Font font = MainForm.fontList[i];
                int width = TextRenderer.MeasureText(this.huntingPlaceName.Text, font).Width;
                if (width < this.huntingPlaceName.Size.Width) {
                    f = font;
                } else {
                    break;
                }
            }

            Bitmap bitmap = new Bitmap(experienceStarBox.Size.Width, experienceStarBox.Size.Height);
            Graphics gr = Graphics.FromImage(bitmap);
            for (int i = 0; i < (this.hunting_place.exp_quality < 0 ? 5 : Math.Min(this.hunting_place.exp_quality, 5)); i++) {
                string huntQuality = this.hunting_place.exp_quality < 0 ? "unknown" : (this.hunting_place.exp_quality - 1).ToString();
                gr.DrawImage(StyleManager.GetImage(String.Format("star{0}.png", huntQuality)), new Rectangle(i * experienceStarBox.Size.Width / 5, 0, experienceStarBox.Size.Width / 5, experienceStarBox.Size.Width / 5));
            }
            experienceStarBox.Image = bitmap;

            bitmap = new Bitmap(lootStarBox.Size.Width, lootStarBox.Size.Height);
            gr = Graphics.FromImage(bitmap);
            for (int i = 0; i < (this.hunting_place.loot_quality < 0 ? 5 : Math.Min(this.hunting_place.loot_quality, 5)); i++) {
                string huntQuality = this.hunting_place.loot_quality < 0 ? "unknown" : (this.hunting_place.loot_quality - 1).ToString();
                gr.DrawImage(StyleManager.GetImage(String.Format("star{0}.png", huntQuality)), new Rectangle(i * lootStarBox.Size.Width / 5, 0, lootStarBox.Size.Width / 5, lootStarBox.Size.Width / 5));
            }
            lootStarBox.Image = bitmap;

            this.huntingPlaceName.Font = f;
            this.refreshCreatures();
            UpdateMap();

            mapBox.Click -= c_Click;

            this.mapUpLevel.Image = StyleManager.GetImage("mapup.png");
            this.mapUpLevel.Click -= c_Click;
            this.mapUpLevel.Click += mapUpLevel_Click;
            this.mapDownLevel.Image = StyleManager.GetImage("mapdown.png");
            this.mapDownLevel.Click -= c_Click;
            this.mapDownLevel.Click += mapDownLevel_Click;
            base.NotificationFinalize();
            this.ResumeForm();
        }

        private void refreshCreatures() {
            foreach(Control c in creatureControls) {
                Controls.Remove(c);
                c.Dispose();
            }

            List<TibiaObject> creatures = new List<TibiaObject>();
            foreach (int creatureid in hunting_place.creatures) {
                Creature cr = MainForm.getCreature(creatureid);
                creatures.Add(cr);
            }

            MainForm.PageInfo pageInfo = new MainForm.PageInfo(false, false);
            int newWidth;
            int y = baseY + MainForm.DisplayCreatureAttributeList(this.Controls, creatures, 10, baseY, out newWidth, null, creatureControls, currentPage, 10, pageInfo, null, null, sortFunction, sortedHeader, desc);

            if (pageInfo.prevPage || pageInfo.nextPage) {
                if (pageInfo.prevPage) {
                    PictureBox prevpage = new PictureBox();
                    prevpage.Location = new Point(10, y);
                    prevpage.Size = new Size(97, 23);
                    prevpage.Image = StyleManager.GetImage("prevpage.png");
                    prevpage.BackColor = Color.Transparent;
                    prevpage.SizeMode = PictureBoxSizeMode.Zoom;
                    prevpage.Click += Prevpage_Click;
                    this.Controls.Add(prevpage);
                    creatureControls.Add(prevpage);

                }
                if (pageInfo.nextPage) {
                    PictureBox nextpage = new PictureBox();
                    nextpage.Location = new Point(Math.Max(newWidth, this.Size.Width) - 108, y);
                    nextpage.Size = new Size(98, 23);
                    nextpage.BackColor = Color.Transparent;
                    nextpage.Image = StyleManager.GetImage("nextpage.png");
                    nextpage.SizeMode = PictureBoxSizeMode.Zoom;
                    nextpage.Click += Nextpage_Click;
                    this.Controls.Add(nextpage);
                    creatureControls.Add(nextpage);
                }
                y += 25;
            }
            refreshTimer();
            this.Size = new Size(Math.Max(this.Size.Width, newWidth), y + 10);
        }

        private void Nextpage_Click(object sender, EventArgs e) {
            currentPage++;
            this.SuspendForm();
            refreshCreatures();
            this.ResumeForm();
        }

        private void Prevpage_Click(object sender, EventArgs e) {
            currentPage--;
            this.SuspendForm();
            refreshCreatures();
            this.ResumeForm();
        }

        protected void sortFunction(object sender, EventArgs e) {
            if (sortedHeader == (sender as Control).Name) {
                desc = !desc;
            } else {
                sortedHeader = (sender as Control).Name;
                desc = false;
            }
            this.SuspendForm();
            refreshCreatures();
            this.ResumeForm();
        }


        protected void openCreatureMenu(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            string name = (sender as Control).Name;
            MainForm.mainForm.ExecuteCommand("creature" + MainForm.commandSymbol + name.ToLower());
        }
        private void openQuest(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            string name = (sender as Control).Name;
            MainForm.mainForm.ExecuteCommand("quest" + MainForm.commandSymbol + name.ToLower());
        }
        private void guideButton_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            string name = (sender as Control).Name;
            MainForm.mainForm.ExecuteCommand("direction" + MainForm.commandSymbol + hunting_place.name.ToLower());
        }

        void label_Click(object sender, EventArgs e) {
            Coordinate new_coordinate = hunting_place.coordinates[int.Parse((sender as Control).Name)];
            this.targetCoordinate = new Coordinate(new_coordinate);
            UpdateMap();
        }
        
        private void UpdateMap() {
            Target target = new Target();
            target.coordinate = new Coordinate(targetCoordinate);
            target.image = StyleManager.GetImage("cross.png");
            target.size = 12;

            if (mapBox.map != null) {
                mapBox.map.Dispose();
            }
            mapBox.map = MainForm.getMap(targetCoordinate.z);
            mapBox.targets.Clear();
            mapBox.targets.Add(target);
            mapBox.mapCoordinate = new Coordinate(targetCoordinate);
            mapBox.sourceWidth = mapBox.Width;
            mapBox.UpdateMap();
        }

        void mapUpLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z--;
            mapBox.UpdateMap();
            base.ResetTimer();
        }

        void mapDownLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z++;
            mapBox.UpdateMap();
            base.ResetTimer();
        }
        
        private void cityLabel_Click(object sender, EventArgs e) {
            MainForm.mainForm.ExecuteCommand("city" + MainForm.commandSymbol + this.hunting_place.city);            
        }
    }
}
