
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
    class HouseForm : NotificationForm {
        public House house;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private Label routeButton;
        private Label expHourLabel;
        private Label cityLabel;
        private Label sizeLabel;
        private Label label1;
        private Label label2;
        private Label bedLabel;
        private Label tibiaButton;
        private Label statusHeader;
        private Label statusLabel;
        private Label timeLeftHeader;
        private Label timeLeftLabel;
        private static Font text_font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);


        public HouseForm() {
            InitializeComponent();
        }

        private const int headerLength = 5;
        private string[] headers = { "Sell To", "Buy From", "Spells", "Transport", "Quests" };
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HouseForm));
            this.mapBox = new Tibialyzer.MapPictureBox();
            this.houseName = new System.Windows.Forms.Label();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            this.routeButton = new System.Windows.Forms.Label();
            this.expHourLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bedLabel = new System.Windows.Forms.Label();
            this.tibiaButton = new System.Windows.Forms.Label();
            this.statusHeader = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.timeLeftHeader = new System.Windows.Forms.Label();
            this.timeLeftLabel = new System.Windows.Forms.Label();
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
            // houseName
            // 
            this.houseName.BackColor = System.Drawing.Color.Transparent;
            this.houseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.houseName.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.houseName.Location = new System.Drawing.Point(11, 27);
            this.houseName.MaximumSize = new System.Drawing.Size(100, 28);
            this.houseName.Name = "houseName";
            this.houseName.Size = new System.Drawing.Size(100, 28);
            this.houseName.TabIndex = 2;
            this.houseName.Text = "Rashid";
            this.houseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // routeButton
            // 
            this.routeButton.BackColor = System.Drawing.Color.Transparent;
            this.routeButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.routeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.routeButton.Location = new System.Drawing.Point(12, 180);
            this.routeButton.Name = "routeButton";
            this.routeButton.Padding = new System.Windows.Forms.Padding(2);
            this.routeButton.Size = new System.Drawing.Size(103, 21);
            this.routeButton.TabIndex = 16;
            this.routeButton.Text = "Route";
            this.routeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.routeButton.Click += new System.EventHandler(this.routeButton_Click);
            // 
            // expHourLabel
            // 
            this.expHourLabel.AutoSize = true;
            this.expHourLabel.BackColor = System.Drawing.Color.Transparent;
            this.expHourLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expHourLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.expHourLabel.Location = new System.Drawing.Point(6, 79);
            this.expHourLabel.Name = "expHourLabel";
            this.expHourLabel.Size = new System.Drawing.Size(28, 13);
            this.expHourLabel.TabIndex = 45;
            this.expHourLabel.Text = "City";
            // 
            // cityLabel
            // 
            this.cityLabel.BackColor = System.Drawing.Color.Transparent;
            this.cityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.cityLabel.Location = new System.Drawing.Point(40, 79);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(80, 13);
            this.cityLabel.TabIndex = 46;
            this.cityLabel.Text = "Edron";
            this.cityLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sizeLabel
            // 
            this.sizeLabel.BackColor = System.Drawing.Color.Transparent;
            this.sizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.sizeLabel.Location = new System.Drawing.Point(40, 95);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(80, 13);
            this.sizeLabel.TabIndex = 48;
            this.sizeLabel.Text = "50 sqm";
            this.sizeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label1.Location = new System.Drawing.Point(6, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label2.Location = new System.Drawing.Point(6, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Beds";
            // 
            // bedLabel
            // 
            this.bedLabel.BackColor = System.Drawing.Color.Transparent;
            this.bedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.bedLabel.Location = new System.Drawing.Point(40, 110);
            this.bedLabel.Name = "bedLabel";
            this.bedLabel.Size = new System.Drawing.Size(80, 13);
            this.bedLabel.TabIndex = 50;
            this.bedLabel.Text = "3";
            this.bedLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tibiaButton
            // 
            this.tibiaButton.BackColor = System.Drawing.Color.Transparent;
            this.tibiaButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tibiaButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tibiaButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.tibiaButton.Location = new System.Drawing.Point(6, 57);
            this.tibiaButton.Name = "tibiaButton";
            this.tibiaButton.Size = new System.Drawing.Size(110, 21);
            this.tibiaButton.TabIndex = 51;
            this.tibiaButton.Text = "View On Tibia.com";
            this.tibiaButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tibiaButton.Click += new System.EventHandler(this.tibiaButton_Click);
            // 
            // statusHeader
            // 
            this.statusHeader.AutoSize = true;
            this.statusHeader.BackColor = System.Drawing.Color.Transparent;
            this.statusHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.statusHeader.Location = new System.Drawing.Point(6, 125);
            this.statusHeader.Name = "statusHeader";
            this.statusHeader.Size = new System.Drawing.Size(43, 13);
            this.statusHeader.TabIndex = 52;
            this.statusHeader.Text = "Status";
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.statusLabel.Location = new System.Drawing.Point(40, 125);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(80, 13);
            this.statusLabel.TabIndex = 53;
            this.statusLabel.Text = "Rented";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timeLeftHeader
            // 
            this.timeLeftHeader.AutoSize = true;
            this.timeLeftHeader.BackColor = System.Drawing.Color.Transparent;
            this.timeLeftHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLeftHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.timeLeftHeader.Location = new System.Drawing.Point(6, 140);
            this.timeLeftHeader.Name = "timeLeftHeader";
            this.timeLeftHeader.Size = new System.Drawing.Size(60, 13);
            this.timeLeftHeader.TabIndex = 54;
            this.timeLeftHeader.Text = "Time Left";
            // 
            // timeLeftLabel
            // 
            this.timeLeftLabel.BackColor = System.Drawing.Color.Transparent;
            this.timeLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLeftLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.timeLeftLabel.Location = new System.Drawing.Point(40, 140);
            this.timeLeftLabel.Name = "timeLeftLabel";
            this.timeLeftLabel.Size = new System.Drawing.Size(80, 13);
            this.timeLeftLabel.TabIndex = 55;
            this.timeLeftLabel.Text = "0h";
            this.timeLeftLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // HouseForm
            // 
            this.ClientSize = new System.Drawing.Size(328, 209);
            this.Controls.Add(this.timeLeftHeader);
            this.Controls.Add(this.timeLeftLabel);
            this.Controls.Add(this.statusHeader);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.tibiaButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bedLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sizeLabel);
            this.Controls.Add(this.expHourLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.routeButton);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.houseName);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HouseForm";
            this.Text = "NPC Form";
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MapPictureBox mapBox;
        private System.Windows.Forms.Label houseName;

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        public override void LoadForm() {
            if (house == null) return;

            this.SuspendForm();
            NotificationInitialize();
            houseName.Text = house.GetName().ToTitle();
            Font f = StyleManager.FontList[0];
            for (int i = 0; i < StyleManager.FontList.Count; i++) {
                Font font = StyleManager.FontList[i];
                Size size = TextRenderer.MeasureText(this.houseName.Text, font);
                if (size.Width < houseName.MaximumSize.Width && size.Height < houseName.MaximumSize.Height) {
                    f = font;
                } else {
                    break;
                }
            }
            this.houseName.Font = f;

            cityLabel.Text = house.city.ToString();
            sizeLabel.Text = String.Format("{0} sqm", house.sqm);
            bedLabel.Text = house.beds.ToString();
            if (house.world != null) {
                statusLabel.Text = house.occupied ? "rented" : (house.hoursleft <= 0 ? "free" : "auctioned");
                if (house.occupied || house.hoursleft < 0) {
                    timeLeftLabel.Visible = false;
                    timeLeftHeader.Visible = false;
                } else {
                    timeLeftLabel.Text = String.Format("{0}{1}", house.hoursleft > 24 ? house.hoursleft / 24 : house.hoursleft, house.hoursleft > 24 ? "D" : "h");
                }
            } else {
                timeLeftHeader.Visible = false;
                statusHeader.Visible = false;
                timeLeftLabel.Visible = false;
                statusLabel.Visible = false;
            }

            Map m = StorageManager.getMap(house.pos.z);

            mapBox.map = m;
            mapBox.mapImage = null;

            Target t = new Target();
            t.coordinate = new Coordinate(house.pos);
            t.image = house.GetImage();
            t.size = 20;

            mapBox.targets.Add(t);
            mapBox.sourceWidth = mapBox.Width;
            mapBox.mapCoordinate = new Coordinate(house.pos);
            mapBox.zCoordinate = house.pos.z;
            mapBox.UpdateMap();

            UnregisterControl(mapBox);

            this.mapUpLevel.Image = StyleManager.GetImage("mapup.png");
            this.UnregisterControl(mapUpLevel);
            this.mapUpLevel.Click += mapUpLevel_Click;
            this.mapDownLevel.Image = StyleManager.GetImage("mapdown.png");
            this.UnregisterControl(mapDownLevel);
            this.mapDownLevel.Click += mapDownLevel_Click;
            
            base.NotificationFinalize();
            this.ResumeForm();
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
            return "HouseForm";
        }

        private void routeButton_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand(String.Format("route{0}{1},{2},{3}{0}{4}", Constants.CommandSymbol, house.pos.x, house.pos.y, house.pos.z, house.GetName()));
        }

        private void tibiaButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl(String.Format("https://secure.tibia.com/community/?subtopic=houses&page=view&world={0}&houseid={1}", house.world == null ? "Antica" : house.world.ToTitle(), house.id));
        }
    }
}
