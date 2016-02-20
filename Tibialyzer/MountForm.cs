
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

namespace Tibialyzer {
    class MountForm : NotificationForm {
        private PictureBox mountImageBox;
        private PictureBox tameItemImageBox;
        private PictureBox tameCreatureImageBox;
        private Label tameCreatureLabel;
        private Label tameItemLabel;
        private Label mountTitle;

        public Mount mount;
        public MountForm(Mount mount) {
            this.mount = mount;
            this.InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MountForm));
            this.tameItemLabel = new System.Windows.Forms.Label();
            this.tameCreatureLabel = new System.Windows.Forms.Label();
            this.tameCreatureImageBox = new System.Windows.Forms.PictureBox();
            this.tameItemImageBox = new System.Windows.Forms.PictureBox();
            this.mountImageBox = new System.Windows.Forms.PictureBox();
            this.mountTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tameCreatureImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tameItemImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mountImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tameItemLabel
            // 
            this.tameItemLabel.BackColor = System.Drawing.Color.Transparent;
            this.tameItemLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tameItemLabel.ForeColor = StyleManager.NotificationTextColor;
            this.tameItemLabel.Location = new System.Drawing.Point(118, 10);
            this.tameItemLabel.Name = "tameItemLabel";
            this.tameItemLabel.Size = new System.Drawing.Size(190, 20);
            this.tameItemLabel.TabIndex = 9;
            this.tameItemLabel.Text = "Tame Item";
            this.tameItemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tameCreatureLabel
            // 
            this.tameCreatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.tameCreatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tameCreatureLabel.ForeColor = StyleManager.NotificationTextColor;
            this.tameCreatureLabel.Location = new System.Drawing.Point(118, 84);
            this.tameCreatureLabel.Name = "tameCreatureLabel";
            this.tameCreatureLabel.Size = new System.Drawing.Size(190, 20);
            this.tameCreatureLabel.TabIndex = 8;
            this.tameCreatureLabel.Text = "Tame Creature";
            this.tameCreatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tameCreatureImageBox
            // 
            this.tameCreatureImageBox.BackColor = System.Drawing.Color.Transparent;
            this.tameCreatureImageBox.Location = new System.Drawing.Point(222, 110);
            this.tameCreatureImageBox.Name = "tameCreatureImageBox";
            this.tameCreatureImageBox.Size = new System.Drawing.Size(80, 80);
            this.tameCreatureImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.tameCreatureImageBox.TabIndex = 7;
            this.tameCreatureImageBox.TabStop = false;
            // 
            // tameItemImageBox
            // 
            this.tameItemImageBox.BackColor = System.Drawing.Color.Transparent;
            this.tameItemImageBox.Location = new System.Drawing.Point(254, 33);
            this.tameItemImageBox.Name = "tameItemImageBox";
            this.tameItemImageBox.Size = new System.Drawing.Size(48, 48);
            this.tameItemImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.tameItemImageBox.TabIndex = 6;
            this.tameItemImageBox.TabStop = false;
            // 
            // mountImageBox
            // 
            this.mountImageBox.BackColor = System.Drawing.Color.Transparent;
            this.mountImageBox.Location = new System.Drawing.Point(12, 32);
            this.mountImageBox.Name = "mountImageBox";
            this.mountImageBox.Size = new System.Drawing.Size(96, 96);
            this.mountImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mountImageBox.TabIndex = 4;
            this.mountImageBox.TabStop = false;
            // 
            // mountTitle
            // 
            this.mountTitle.AutoSize = true;
            this.mountTitle.BackColor = System.Drawing.Color.Transparent;
            this.mountTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mountTitle.ForeColor = StyleManager.NotificationTextColor;
            this.mountTitle.Location = new System.Drawing.Point(8, 134);
            this.mountTitle.Name = "mountTitle";
            this.mountTitle.Size = new System.Drawing.Size(110, 20);
            this.mountTitle.TabIndex = 3;
            this.mountTitle.Text = "Mount Name";
            // 
            // MountForm
            // 
            this.ClientSize = new System.Drawing.Size(318, 213);
            this.Controls.Add(this.tameItemLabel);
            this.Controls.Add(this.tameCreatureLabel);
            this.Controls.Add(this.tameCreatureImageBox);
            this.Controls.Add(this.tameItemImageBox);
            this.Controls.Add(this.mountImageBox);
            this.Controls.Add(this.mountTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MountForm";
            ((System.ComponentModel.ISupportInitialize)(this.tameCreatureImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tameItemImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mountImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override void LoadForm() {
            if (mount == null) return;
            this.SuspendLayout();
            NotificationInitialize();

            this.mountTitle.Text = mount.name;
            this.mountImageBox.Image = mount.image;
            if (mount.tibiastore) {
                this.tameItemImageBox.Visible = false;
                this.tameItemLabel.Visible = false;
                this.tameCreatureImageBox.Image = StyleManager.GetImage("tibiastore.png");
                this.tameCreatureLabel.Text = "Tibia Store";
                this.tameCreatureImageBox.Location = new System.Drawing.Point(this.tameCreatureImageBox.Location.X, this.tameCreatureImageBox.Location.Y - 50);
                this.tameCreatureLabel.Location = new System.Drawing.Point(this.tameCreatureLabel.Location.X, this.tameCreatureLabel.Location.Y - 50);
            } else if (mount.tamecreatureid > 0 && mount.tameitemid > 0) {
                Creature tameCreature = MainForm.getCreature(mount.tamecreatureid);
                Item tameItem = MainForm.getItem(mount.tameitemid);

                this.tameCreatureImageBox.Image = tameCreature.image;
                this.tameCreatureLabel.Text = MainForm.ToTitle(tameCreature.displayname);
                this.tameItemImageBox.Image = tameItem.image;
                this.tameItemLabel.Text = MainForm.ToTitle(tameItem.displayname);

                tameCreatureImageBox.Name = tameCreature.GetName();
                tameCreatureLabel.Name = tameCreature.GetName();
                tameItemImageBox.Name = tameItem.GetName();
                tameItemLabel.Name = tameItem.GetName();

                this.tameCreatureLabel.Click += TameCreatureImageBox_Click;
                this.tameCreatureImageBox.Click += TameCreatureImageBox_Click;
                this.tameItemImageBox.Click += TameItemImageBox_Click;
                this.tameItemLabel.Click += TameItemImageBox_Click;
            } else {
                this.tameCreatureImageBox.Visible = false;
                this.tameCreatureLabel.Visible = false;
                this.tameItemImageBox.Visible = false;
                this.tameItemLabel.Visible = false;
            }


            base.NotificationFinalize();
            this.ResumeLayout(false);
        }
        
        private void TameItemImageBox_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("item@" + (sender as Control).Name);
        }

        private void TameCreatureImageBox_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("creature@" + (sender as Control).Name);
        }
    }
}
