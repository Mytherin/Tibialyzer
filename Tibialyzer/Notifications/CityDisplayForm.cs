
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
using System.Windows.Forms;

namespace Tibialyzer {
    class CityDisplayForm : NotificationForm {
        private Label cityNameLabel;
        private MapPictureBox mapBox;
        private static Size mapSize = new Size(192, 192);
        private Label listLabel;
        private Label npcButton;
        private Label huntButton;
        private Label questButton;
        private List<Control> controlList = new List<Control>();
        private Label utilityButton;
        private List<TibiaObject> npcList = null;
        private Label previousButton;
        private Label nextButton;
        private PictureBox mapDownLevel;
        private PictureBox mapUpLevel;
        public City city;
        public CityDisplayForm() {
            this.InitializeComponent();
        }

        private void InitializeComponent() {
            this.cityNameLabel = new System.Windows.Forms.Label();
            this.listLabel = new System.Windows.Forms.Label();
            this.npcButton = new System.Windows.Forms.Label();
            this.huntButton = new System.Windows.Forms.Label();
            this.questButton = new System.Windows.Forms.Label();
            this.utilityButton = new System.Windows.Forms.Label();
            this.previousButton = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Label();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            this.SuspendLayout();
            //
            // cityNameLabel
            //
            this.cityNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.cityNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cityNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cityNameLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.cityNameLabel.Location = new System.Drawing.Point(5, 28);
            this.cityNameLabel.Name = "cityNameLabel";
            this.cityNameLabel.Size = new System.Drawing.Size(192, 25);
            this.cityNameLabel.TabIndex = 3;
            this.cityNameLabel.Text = "City";
            this.cityNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // listLabel
            //
            this.listLabel.BackColor = System.Drawing.Color.Transparent;
            this.listLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.listLabel.Location = new System.Drawing.Point(197, 28);
            this.listLabel.Name = "listLabel";
            this.listLabel.Size = new System.Drawing.Size(192, 25);
            this.listLabel.TabIndex = 4;
            this.listLabel.Text = "Utilities";
            this.listLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // npcButton
            //
            this.npcButton.BackColor = System.Drawing.Color.Transparent;
            this.npcButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.npcButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.npcButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.npcButton.Location = new System.Drawing.Point(5, 245);
            this.npcButton.Name = "npcButton";
            this.npcButton.Size = new System.Drawing.Size(96, 25);
            this.npcButton.TabIndex = 5;
            this.npcButton.Text = "NPCs";
            this.npcButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.npcButton.Click += new System.EventHandler(this.npcButton_Click);
            //
            // huntButton
            //
            this.huntButton.BackColor = System.Drawing.Color.Transparent;
            this.huntButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.huntButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.huntButton.Location = new System.Drawing.Point(5, 270);
            this.huntButton.Name = "huntButton";
            this.huntButton.Size = new System.Drawing.Size(96, 25);
            this.huntButton.TabIndex = 6;
            this.huntButton.Text = "Hunts";
            this.huntButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.huntButton.Click += new System.EventHandler(this.huntButton_Click);
            //
            // questButton
            //
            this.questButton.BackColor = System.Drawing.Color.Transparent;
            this.questButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.questButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.questButton.Location = new System.Drawing.Point(101, 270);
            this.questButton.Name = "questButton";
            this.questButton.Size = new System.Drawing.Size(96, 25);
            this.questButton.TabIndex = 7;
            this.questButton.Text = "Quests";
            this.questButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.questButton.Click += new System.EventHandler(this.questButton_Click);
            //
            // utilityButton
            //
            this.utilityButton.BackColor = System.Drawing.Color.Transparent;
            this.utilityButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.utilityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.utilityButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.utilityButton.Location = new System.Drawing.Point(101, 245);
            this.utilityButton.Name = "utilityButton";
            this.utilityButton.Size = new System.Drawing.Size(96, 25);
            this.utilityButton.TabIndex = 8;
            this.utilityButton.Text = "Utilities";
            this.utilityButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.utilityButton.Click += new System.EventHandler(this.utilityButton_Click);
            //
            // previousButton
            //
            this.previousButton.BackColor = System.Drawing.Color.Transparent;
            this.previousButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previousButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.previousButton.Location = new System.Drawing.Point(197, 270);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(96, 25);
            this.previousButton.TabIndex = 9;
            this.previousButton.Text = "Previous";
            this.previousButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            //
            // nextButton
            //
            this.nextButton.BackColor = System.Drawing.Color.Transparent;
            this.nextButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.nextButton.Location = new System.Drawing.Point(294, 270);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(96, 25);
            this.nextButton.TabIndex = 10;
            this.nextButton.Text = "Next";
            this.nextButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            //
            // mapDownLevel
            //
            this.mapDownLevel.Location = new System.Drawing.Point(5, 73);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 12;
            this.mapDownLevel.TabStop = false;
            this.mapDownLevel.Click += new System.EventHandler(this.mapDownLevel_Click);
            //
            // mapUpLevel
            //
            this.mapUpLevel.Location = new System.Drawing.Point(5, 52);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 11;
            this.mapUpLevel.TabStop = false;
            this.mapUpLevel.Click += new System.EventHandler(this.mapUpLevel_Click);
            //
            // CityDisplayForm
            //
            this.ClientSize = new System.Drawing.Size(396, 301);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.utilityButton);
            this.Controls.Add(this.questButton);
            this.Controls.Add(this.huntButton);
            this.Controls.Add(this.npcButton);
            this.Controls.Add(this.listLabel);
            this.Controls.Add(this.cityNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CityDisplayForm";
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            this.ResumeLayout(false);

        }

        private int baseHeight;
        public override void LoadForm() {
            this.SuspendForm();
            base.NotificationInitialize();

            this.cityNameLabel.Text = city.name.ToTitle();
            mapBox = UIManager.DrawRoute(city.location, new Coordinate(-1,-1,mapSize.Width), mapSize, mapSize, mapSize, new List<Color>(), new List<Target>());
            mapBox.Location = new Point(5, cityNameLabel.Location.Y + cityNameLabel.Height);
            mapBox.MapUpdated += ResetTimer;
            this.Controls.Add(mapBox);

            baseHeight = this.Size.Height;
            RefreshForm();

            UnregisterControl(npcButton);
            UnregisterControl(utilityButton);
            UnregisterControl(huntButton);
            UnregisterControl(questButton);
            UnregisterControl(nextButton);
            UnregisterControl(previousButton);
            mapUpLevel.Image = StyleManager.GetImage("mapup.png");
            UnregisterControl(mapUpLevel);
            mapDownLevel.Image = StyleManager.GetImage("mapdown.png");
            UnregisterControl(mapDownLevel);

            this.NotificationFinalize();
            this.ResumeForm();
        }

        private void UtilityBox_Click(object sender, EventArgs e) {
            if (npcList == null) {
                Utility utility = city.utilities[int.Parse((sender as Control).Name)];
                mapBox.mapCoordinate = new Coordinate(utility.location);
                mapBox.UpdateMap();
            } else {
                NPC npc = npcList[int.Parse((sender as Control).Name)] as NPC;
                CommandManager.ExecuteCommand("npc" + Constants.CommandSymbol + npc.name);
            }
        }

        private Stack<int> prevIndices = new Stack<int>();
        private int nextIndex = 0;
        private int baseIndex = 0;
        private int totalCount = 0;
        private int InitializeList(List<Utility> utilities, List<TibiaObject> tibiaObjects) {
            foreach(Control c in controlList) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            controlList.Clear();

            int width = tibiaObjects != null ? 48 : 32;
            int height = tibiaObjects != null ? 48 : 32;

            npcList = tibiaObjects;

            ToolTip nameTooltip = new ToolTip();
            nameTooltip.AutoPopDelay = 60000;
            nameTooltip.InitialDelay = 500;
            nameTooltip.ReshowDelay = 0;
            nameTooltip.ShowAlways = true;
            nameTooltip.UseFading = true;

            totalCount = tibiaObjects != null ? tibiaObjects.Count : utilities.Count;

            mapBox.targets.Clear();

            int totalHeight = 0;
            int baseX = listLabel.Location.X;
            int x = baseX;
            int y = listLabel.Location.Y + listLabel.Height;
            int index;
            for (index = baseIndex; index < totalCount; index++) {
                string name = "";
                Image image = null;
                Coordinate coordinate = null;

                if (tibiaObjects != null) {
                    name = tibiaObjects[index].GetName();
                    image = tibiaObjects[index].GetImage();
                    coordinate = new Coordinate((tibiaObjects[index] as NPC).pos);
                } else {
                    name = utilities[index].name.Replace(" ", "");
                    image = StyleManager.GetImage(name + ".png");
                    coordinate = new Coordinate(utilities[index].location);
                }

                Target target = new Target();
                target.coordinate = coordinate;
                target.image = image;
                target.size = width / 2;
                mapBox.targets.Add(target);

                PictureBox utilityBox = new PictureBox();
                utilityBox.BackColor = Color.Transparent;
                utilityBox.Size = new Size(width, height);
                utilityBox.Image = image;
                utilityBox.Location = new Point(x, y);
                utilityBox.Click += UtilityBox_Click;
                utilityBox.Name = index.ToString();
                utilityBox.SizeMode = PictureBoxSizeMode.Zoom;
                nameTooltip.SetToolTip(utilityBox, name.ToTitle());
                controlList.Add(utilityBox);
                this.Controls.Add(utilityBox);

                totalHeight = Math.Max(totalHeight, utilityBox.Height + utilityBox.Location.Y);

                x += width + 4;
                if (x + width + 4 > this.Size.Width) {
                    x = baseX;
                    y += height + 4;
                }
                if (y > (this.Size.Width == MinWidth() ? 570 : 270) - 31 - height - 4) {
                    break;
                }
            }
            nextIndex = index + 1;
            nextButton.Visible = nextIndex < totalCount - 1;
            previousButton.Visible = baseIndex > 0;
            mapBox.UpdateMap();
            return totalHeight;
        }

        private void npcButton_Click(object sender, EventArgs e) {
            this.SuspendForm();
            List<TibiaObject> npcs = StorageManager.getNPCWithCity(city.name.ToLower());
            int listHeight = InitializeList(null, npcs);

            this.ResumeForm();
            this.Refresh();
            this.refreshTimer();
        }

        private void huntButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("hunt" + Constants.CommandSymbol + city.name);
        }

        private void questButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("quest" + Constants.CommandSymbol + city.name);
        }

        private void utilityButton_Click(object sender, EventArgs e) {
            this.SuspendForm();
            int listHeight = InitializeList(city.utilities, null);

            this.ResumeForm();
            this.Refresh();
            this.refreshTimer();
        }

        private void previousButton_Click(object sender, EventArgs e) {
            this.SuspendForm();
            baseIndex = prevIndices.Pop();

            int listHeight = InitializeList(city.utilities, npcList);

            this.ResumeForm();
            this.Refresh();
            refreshTimer();
        }

        private void nextButton_Click(object sender, EventArgs e) {
            this.SuspendForm();
            prevIndices.Push(baseIndex);
            baseIndex = nextIndex;

            int listHeight = InitializeList(city.utilities, npcList);

            this.ResumeForm();
            this.Refresh();
            refreshTimer();
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

        public override string FormName() {
            return "CityDisplayForm";
        }


        public override int MinWidth() {
            return 204;
        }

        public override int MaxWidth() {
            return 396;
        }

        public override int WidthInterval() {
            return 172;
        }

        public override void RefreshForm() {
            int newWidth = GetWidth();
            if (newWidth == this.Size.Width) return;
            this.SuspendForm();
            int height = 0;
            if (newWidth == MinWidth()) {
                listLabel.Location = new Point(huntButton.Location.X, huntButton.Location.Y + huntButton.Height);
                previousButton.Location = new Point(listLabel.Location.X, listLabel.Location.Y + 242);
                nextButton.Location = new Point(listLabel.Location.X + previousButton.Width, listLabel.Location.Y + 242);
                height = 567;
            } else {
                this.previousButton.Location = new System.Drawing.Point(197, 270);
                this.nextButton.Location = new System.Drawing.Point(283, 270);
                this.listLabel.Location = new System.Drawing.Point(197, 28);
                height = 301;
            }
            this.Size = new Size(newWidth, height);
            utilityButton_Click(null, null);

            this.ResumeForm();
            this.Refresh();
        }
    }
}
