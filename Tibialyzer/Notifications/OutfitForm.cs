
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
    class OutfitForm : NotificationForm {
        private Label outfitTitle;
        private PictureBox maleAddon1;
        private PictureBox maleAddon2;
        private PictureBox maleAddon3;
        private PictureBox femaleAddon0;
        private PictureBox femaleAddon1;
        private PictureBox femaleAddon2;
        private PictureBox femaleAddon3;
        private PictureBox maleAddon0;
        private Label obtainedLabel;
        public Outfit outfit;

        public OutfitForm(Outfit outfit) {
            this.outfit = outfit;
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutfitForm));
            this.obtainedLabel = new System.Windows.Forms.Label();
            this.femaleAddon3 = new System.Windows.Forms.PictureBox();
            this.femaleAddon2 = new System.Windows.Forms.PictureBox();
            this.femaleAddon1 = new System.Windows.Forms.PictureBox();
            this.femaleAddon0 = new System.Windows.Forms.PictureBox();
            this.maleAddon3 = new System.Windows.Forms.PictureBox();
            this.maleAddon2 = new System.Windows.Forms.PictureBox();
            this.maleAddon1 = new System.Windows.Forms.PictureBox();
            this.outfitTitle = new System.Windows.Forms.Label();
            this.maleAddon0 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon0)).BeginInit();
            this.SuspendLayout();
            //
            // obtainedLabel
            //
            this.obtainedLabel.AutoSize = true;
            this.obtainedLabel.BackColor = System.Drawing.Color.Transparent;
            this.obtainedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.obtainedLabel.ForeColor = StyleManager.NotificationTextColor;
            this.obtainedLabel.Location = new System.Drawing.Point(12, 172);
            this.obtainedLabel.Name = "obtainedLabel";
            this.obtainedLabel.Size = new System.Drawing.Size(145, 16);
            this.obtainedLabel.TabIndex = 9;
            this.obtainedLabel.Text = "Rewarded By Quest";
            //
            // femaleAddon3
            //
            this.femaleAddon3.BackColor = System.Drawing.Color.Transparent;
            this.femaleAddon3.Location = new System.Drawing.Point(226, 102);
            this.femaleAddon3.Name = "femaleAddon3";
            this.femaleAddon3.Size = new System.Drawing.Size(64, 64);
            this.femaleAddon3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.femaleAddon3.TabIndex = 8;
            this.femaleAddon3.TabStop = false;
            //
            // femaleAddon2
            //
            this.femaleAddon2.BackColor = System.Drawing.Color.Transparent;
            this.femaleAddon2.Location = new System.Drawing.Point(156, 102);
            this.femaleAddon2.Name = "femaleAddon2";
            this.femaleAddon2.Size = new System.Drawing.Size(64, 64);
            this.femaleAddon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.femaleAddon2.TabIndex = 7;
            this.femaleAddon2.TabStop = false;
            //
            // femaleAddon1
            //
            this.femaleAddon1.BackColor = System.Drawing.Color.Transparent;
            this.femaleAddon1.Location = new System.Drawing.Point(86, 102);
            this.femaleAddon1.Name = "femaleAddon1";
            this.femaleAddon1.Size = new System.Drawing.Size(64, 64);
            this.femaleAddon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.femaleAddon1.TabIndex = 6;
            this.femaleAddon1.TabStop = false;
            //
            // femaleAddon0
            //
            this.femaleAddon0.BackColor = System.Drawing.Color.Transparent;
            this.femaleAddon0.Location = new System.Drawing.Point(16, 102);
            this.femaleAddon0.Name = "femaleAddon0";
            this.femaleAddon0.Size = new System.Drawing.Size(64, 64);
            this.femaleAddon0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.femaleAddon0.TabIndex = 5;
            this.femaleAddon0.TabStop = false;
            //
            // maleAddon3
            //
            this.maleAddon3.BackColor = System.Drawing.Color.Transparent;
            this.maleAddon3.Location = new System.Drawing.Point(226, 32);
            this.maleAddon3.Name = "maleAddon3";
            this.maleAddon3.Size = new System.Drawing.Size(64, 64);
            this.maleAddon3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.maleAddon3.TabIndex = 4;
            this.maleAddon3.TabStop = false;
            //
            // maleAddon2
            //
            this.maleAddon2.BackColor = System.Drawing.Color.Transparent;
            this.maleAddon2.Location = new System.Drawing.Point(156, 32);
            this.maleAddon2.Name = "maleAddon2";
            this.maleAddon2.Size = new System.Drawing.Size(64, 64);
            this.maleAddon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.maleAddon2.TabIndex = 3;
            this.maleAddon2.TabStop = false;
            //
            // maleAddon1
            //
            this.maleAddon1.BackColor = System.Drawing.Color.Transparent;
            this.maleAddon1.Location = new System.Drawing.Point(86, 32);
            this.maleAddon1.Name = "maleAddon1";
            this.maleAddon1.Size = new System.Drawing.Size(64, 64);
            this.maleAddon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.maleAddon1.TabIndex = 2;
            this.maleAddon1.TabStop = false;
            //
            // outfitTitle
            //
            this.outfitTitle.AutoSize = true;
            this.outfitTitle.BackColor = System.Drawing.Color.Transparent;
            this.outfitTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outfitTitle.ForeColor = StyleManager.NotificationTextColor;
            this.outfitTitle.Location = new System.Drawing.Point(115, 9);
            this.outfitTitle.Name = "outfitTitle";
            this.outfitTitle.Size = new System.Drawing.Size(105, 20);
            this.outfitTitle.TabIndex = 1;
            this.outfitTitle.Text = "Outfit Name";
            //
            // maleAddon0
            //
            this.maleAddon0.BackColor = System.Drawing.Color.Transparent;
            this.maleAddon0.Location = new System.Drawing.Point(16, 32);
            this.maleAddon0.Name = "maleAddon0";
            this.maleAddon0.Size = new System.Drawing.Size(64, 64);
            this.maleAddon0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.maleAddon0.TabIndex = 0;
            this.maleAddon0.TabStop = false;
            //
            // OutfitForm
            //
            this.ClientSize = new System.Drawing.Size(314, 199);
            this.Controls.Add(this.obtainedLabel);
            this.Controls.Add(this.femaleAddon3);
            this.Controls.Add(this.femaleAddon2);
            this.Controls.Add(this.femaleAddon1);
            this.Controls.Add(this.femaleAddon0);
            this.Controls.Add(this.maleAddon3);
            this.Controls.Add(this.maleAddon2);
            this.Controls.Add(this.maleAddon1);
            this.Controls.Add(this.outfitTitle);
            this.Controls.Add(this.maleAddon0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OutfitForm";
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.femaleAddon0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maleAddon0)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override void LoadForm() {
            if (outfit == null) return;

            this.SuspendLayout();
            NotificationInitialize();

            this.outfitTitle.Text = outfit.name;
            this.maleAddon0.Image = outfit.maleImages[0];
            this.maleAddon1.Image = outfit.maleImages[1];
            this.maleAddon2.Image = outfit.maleImages[2];
            this.maleAddon3.Image = outfit.maleImages[3];
            this.femaleAddon0.Image = outfit.femaleImages[0];
            this.femaleAddon1.Image = outfit.femaleImages[1];
            this.femaleAddon2.Image = outfit.femaleImages[2];
            this.femaleAddon3.Image = outfit.femaleImages[3];

            if (outfit.questid > 0) {
                Quest quest = StorageManager.getQuest(outfit.questid);
                this.obtainedLabel.Text = "Rewarded by " + quest.name;
                obtainedCommand = "quest" + Constants.CommandSymbol + quest.name;
                this.obtainedLabel.Click += ObtainedLabel_Click;
            } else if (outfit.tibiastore) {
                this.obtainedLabel.Text = "Purchased from the Tibia Store.";
            } else {
                this.obtainedLabel.Visible = false;
            }

            base.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private string obtainedCommand = null;
        private void ObtainedLabel_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            CommandManager.ExecuteCommand(obtainedCommand);
        }

        public override string FormName() {
            return "OutfitForm";
        }
    }
}
