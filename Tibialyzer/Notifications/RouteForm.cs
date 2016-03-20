
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
    class RouteForm : NotificationForm {
        public TibiaObject imageObject = null;
        public Coordinate targetCoordinate = null;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private Label routeLabel;
        private Label nextStepButton;
        private static Font text_font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);


        public RouteForm(Coordinate targetCoordinate, TibiaObject imageObject) {
            InitializeComponent();
            this.targetCoordinate = targetCoordinate;
            this.imageObject = imageObject;
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RouteForm));
            this.mapBox = new Tibialyzer.MapPictureBox();
            this.npcImage = new System.Windows.Forms.PictureBox();
            this.creatureName = new System.Windows.Forms.Label();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            this.routeLabel = new System.Windows.Forms.Label();
            this.nextStepButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            this.SuspendLayout();
            //
            // mapBox
            //
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapBox.Location = new System.Drawing.Point(121, 12);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(195, 190);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            //
            // npcImage
            //
            this.npcImage.BackColor = System.Drawing.Color.Transparent;
            this.npcImage.Location = new System.Drawing.Point(12, 45);
            this.npcImage.Name = "npcImage";
            this.npcImage.Size = new System.Drawing.Size(100, 98);
            this.npcImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.npcImage.TabIndex = 1;
            this.npcImage.TabStop = false;
            //
            // creatureName
            //
            this.creatureName.BackColor = System.Drawing.Color.Transparent;
            this.creatureName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureName.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.creatureName.Location = new System.Drawing.Point(11, 146);
            this.creatureName.MaximumSize = new System.Drawing.Size(100, 28);
            this.creatureName.Name = "creatureName";
            this.creatureName.Size = new System.Drawing.Size(100, 28);
            this.creatureName.TabIndex = 2;
            this.creatureName.Text = "Rashid";
            this.creatureName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.creatureName.Click += new System.EventHandler(this.creatureName_Click);
            //
            // mapUpLevel
            //
            this.mapUpLevel.Location = new System.Drawing.Point(121, 13);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 3;
            this.mapUpLevel.TabStop = false;
            //
            // mapDownLevel
            //
            this.mapDownLevel.Location = new System.Drawing.Point(121, 34);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 4;
            this.mapDownLevel.TabStop = false;
            //
            // routeLabel
            //
            this.routeLabel.BackColor = System.Drawing.Color.Transparent;
            this.routeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.routeLabel.Location = new System.Drawing.Point(12, 207);
            this.routeLabel.Name = "routeLabel";
            this.routeLabel.Size = new System.Drawing.Size(308, 56);
            this.routeLabel.TabIndex = 5;
            //
            // nextStepButton
            //
            this.nextStepButton.BackColor = System.Drawing.Color.Transparent;
            this.nextStepButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextStepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextStepButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.nextStepButton.Location = new System.Drawing.Point(13, 177);
            this.nextStepButton.Name = "nextStepButton";
            this.nextStepButton.Padding = new System.Windows.Forms.Padding(2);
            this.nextStepButton.Size = new System.Drawing.Size(103, 21);
            this.nextStepButton.TabIndex = 17;
            this.nextStepButton.Text = "Next Step";
            this.nextStepButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nextStepButton.Click += new System.EventHandler(this.nextStepButton_Click);
            //
            // RouteForm
            //
            this.ClientSize = new System.Drawing.Size(328, 273);
            this.Controls.Add(this.nextStepButton);
            this.Controls.Add(this.routeLabel);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.creatureName);
            this.Controls.Add(this.npcImage);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RouteForm";
            this.Text = "NPC Form";
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            this.ResumeLayout(false);

        }

        private MapPictureBox mapBox;
        private System.Windows.Forms.PictureBox npcImage;
        private System.Windows.Forms.Label creatureName;

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        public override void LoadForm() {
            this.SuspendForm();
            NotificationInitialize();

            nextStepButton.Click -= c_Click;
            npcImage.Image = imageObject == null ? StyleManager.GetImage("cross.png") : imageObject.GetImage();
            lock(npcImage.Image) {
                if (npcImage.Image.Width > npcImage.Width || npcImage.Image.Height > npcImage.Height) {
                    npcImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            creatureName.Text = imageObject == null ? "Target" : imageObject.GetName().ToTitle();
            Font f = StyleManager.FontList[0];
            for (int i = 0; i < StyleManager.FontList.Count; i++) {
                Font font = StyleManager.FontList[i];
                Size size = TextRenderer.MeasureText(this.creatureName.Text, font);
                if (size.Width < creatureName.MaximumSize.Width && size.Height < creatureName.MaximumSize.Height) {
                    f = font;
                } else {
                    break;
                }
            }
            this.creatureName.Font = f;

            Map m = StorageManager.getMap(targetCoordinate.z);

            mapBox.map = m;
            mapBox.mapImage = null;

            Target t = new Target();
            t.coordinate = new Coordinate(targetCoordinate);
            t.image = imageObject == null ? StyleManager.GetImage("cross.png") : imageObject.GetImage();
            t.size = 20;

            mapBox.targets.Add(t);
            mapBox.sourceWidth = mapBox.Width;
            mapBox.mapCoordinate = new Coordinate(targetCoordinate);
            mapBox.SetTargetCoordinate(new Coordinate(targetCoordinate));
            mapBox.zCoordinate = targetCoordinate.z;
            mapBox.UpdateMap();
            mapBox.MapUpdated += MapBox_MapUpdated;

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

        private void MapBox_MapUpdated() {
            string targetString = "";
            if (mapBox.nextTarget != null) {
                targetString += mapBox.nextTarget + "\n";
            }
            if (mapBox.nextImportantTarget != null) {
                targetString += mapBox.nextImportantTarget + "\n";
            }
            routeLabel.Text = targetString;
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

        private void creatureName_Click(object sender, EventArgs e) {
            if (imageObject != null) {
                CommandManager.ExecuteCommand(imageObject.GetCommand());
            }
        }

        public override string FormName() {
            return "RouteForm";
        }

        private void nextStepButton_Click(object sender, EventArgs e) {
            mapBox.NextStep();
        }
    }
}
