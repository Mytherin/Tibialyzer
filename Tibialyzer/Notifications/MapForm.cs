
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
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer {
    class MapForm : NotificationForm {
        private System.Windows.Forms.PictureBox mapUpLevel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private EnterTextBox mapperBox;
        private Label usedItemsLabel;
        private Label label1;
        private EnterTextBox coordinateBox;
        private Label routeButton;
        private static Font text_font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);


        public MapForm() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapForm));
            this.mapBox = new Tibialyzer.MapPictureBox();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            this.mapperBox = new Tibialyzer.EnterTextBox();
            this.usedItemsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.coordinateBox = new Tibialyzer.EnterTextBox();
            this.routeButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
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
            // mapperBox
            // 
            this.mapperBox.Location = new System.Drawing.Point(6, 47);
            this.mapperBox.Name = "mapperBox";
            this.mapperBox.Size = new System.Drawing.Size(108, 20);
            this.mapperBox.TabIndex = 5;
            this.mapperBox.Text = "127.128,124.128,7";
            this.mapperBox.TextChanged += new System.EventHandler(this.mapperBox_TextChanged);
            // 
            // usedItemsLabel
            // 
            this.usedItemsLabel.AutoSize = true;
            this.usedItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.usedItemsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usedItemsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.usedItemsLabel.Location = new System.Drawing.Point(7, 29);
            this.usedItemsLabel.Name = "usedItemsLabel";
            this.usedItemsLabel.Size = new System.Drawing.Size(107, 16);
            this.usedItemsLabel.TabIndex = 42;
            this.usedItemsLabel.Text = "Mapper Coord";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label1.Location = new System.Drawing.Point(19, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 44;
            this.label1.Text = "Coordinate";
            // 
            // coordinateBox
            // 
            this.coordinateBox.Location = new System.Drawing.Point(6, 93);
            this.coordinateBox.Name = "coordinateBox";
            this.coordinateBox.Size = new System.Drawing.Size(108, 20);
            this.coordinateBox.TabIndex = 43;
            this.coordinateBox.Text = "1024,1024,7";
            this.coordinateBox.TextChanged += new System.EventHandler(this.coordinateBox_TextChanged);
            // 
            // routeButton
            // 
            this.routeButton.BackColor = System.Drawing.Color.Transparent;
            this.routeButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.routeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.routeButton.Location = new System.Drawing.Point(6, 181);
            this.routeButton.Name = "routeButton";
            this.routeButton.Padding = new System.Windows.Forms.Padding(2);
            this.routeButton.Size = new System.Drawing.Size(103, 21);
            this.routeButton.TabIndex = 45;
            this.routeButton.Text = "Route";
            this.routeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.routeButton.Click += new System.EventHandler(this.routeButton_Click);
            // 
            // MapForm
            // 
            this.ClientSize = new System.Drawing.Size(328, 221);
            this.Controls.Add(this.routeButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.coordinateBox);
            this.Controls.Add(this.usedItemsLabel);
            this.Controls.Add(this.mapperBox);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapForm";
            this.Text = "NPC Form";
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MapPictureBox mapBox;

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        public override void LoadForm() {
            this.SuspendForm();
            NotificationInitialize();

            Coordinate coord = new Coordinate();
            try {
                coord = new Coordinate(MemoryReader.X, MemoryReader.Y, MemoryReader.Z);
            } catch {
            }

            Map m = StorageManager.getMap(coord.z);

            mapBox.map = m;
            mapBox.mapImage = null;

            mapBox.sourceWidth = mapBox.Width;
            mapBox.beginCoordinate = new Coordinate(coord);
            mapBox.mapCoordinate = new Coordinate(coord);
            mapBox.zCoordinate = coord.z;
            mapBox.MapUpdated += MapBox_MapUpdated;
            mapBox.UpdateMap();

            coordinateBox.Text = String.Format("{0},{1},{2}", coord.x, coord.y, coord.z);

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
            updating = true;
            SetCoordinate(mapBox.mapCoordinate);
            updating = false;
        }

        private void SetCoordinate(Coordinate coordinate) {
            int x = coordinate.x, y = coordinate.y, z = coordinate.z;
            coordinateBox.Text = String.Format("{0},{1},{2}", x, y, z);
            mapperBox.Text = ((x / 256 + 124).ToString() + "." + (x % 256).ToString()) + "," + ((y / 256 + 121).ToString() + "." + (y % 256).ToString()) + "," + z.ToString();
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
            return "MapForm";
        }

        bool updating = false;

        private void mapperBox_TextChanged(object sender, EventArgs e) {
            if (updating) return;
            string[] split = mapperBox.Text.Split(',');
            if (split.Length == 3) {
                int x_big, x_small, y_big, y_small, z;
                string[] xsplit = split[0].Split('.');
                string[] ysplit = split[1].Split('.');
                if (xsplit.Length == 2 && ysplit.Length == 2 &&
                    int.TryParse(xsplit[0], out x_big) && int.TryParse(xsplit[1], out x_small) &&
                    int.TryParse(ysplit[0], out y_big) && int.TryParse(ysplit[1], out y_small) && int.TryParse(split[2], out z)) {
                    int x = 256 * (x_big - 124) + x_small;
                    int y = 256 * (y_big - 121) + y_small;

                    if (mapBox.mapCoordinate.x != x || mapBox.mapCoordinate.y != y || mapBox.mapCoordinate.z != z) {
                        updating = true;
                        mapBox.mapCoordinate = new Coordinate(x, y, z);
                        SetCoordinate(mapBox.mapCoordinate);
                        mapBox.UpdateMap();
                        updating = false;
                    }
                }
            }
        }

        private void coordinateBox_TextChanged(object sender, EventArgs e) {
            if (updating) return;
            string[] split = coordinateBox.Text.Split(',');
            int x, y, z;
            if (split.Length == 3 && int.TryParse(split[0], out x) && int.TryParse(split[1], out y) && int.TryParse(split[2], out z)) {
                if (mapBox.mapCoordinate.x != x || mapBox.mapCoordinate.y != y || mapBox.mapCoordinate.z != z) {
                    updating = true;
                    mapBox.mapCoordinate = new Coordinate(x, y, z);
                    SetCoordinate(mapBox.mapCoordinate);
                    mapBox.UpdateMap();
                    updating = false;
                }
            }
        }

        private void routeButton_Click(object sender, EventArgs e) {
            Coordinate c = mapBox.mapCoordinate;
            CommandManager.ExecuteCommand(String.Format("route@{0},{1},{2}", c.x, c.y, c.z));
        }
    }
}
