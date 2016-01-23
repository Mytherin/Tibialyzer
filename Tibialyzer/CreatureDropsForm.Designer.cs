
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
﻿namespace Tibialyzer
{
    partial class CreatureDropsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatureDropsForm));
            this.huntButton = new System.Windows.Forms.Label();
            this.statsButton = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.mainImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).BeginInit();
            this.SuspendLayout();
            // 
            // huntButton
            // 
            this.huntButton.BackColor = System.Drawing.Color.Transparent;
            this.huntButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.huntButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.huntButton.Location = new System.Drawing.Point(12, 155);
            this.huntButton.Name = "huntButton";
            this.huntButton.Padding = new System.Windows.Forms.Padding(2);
            this.huntButton.Size = new System.Drawing.Size(96, 21);
            this.huntButton.TabIndex = 3;
            this.huntButton.Text = "Hunts";
            this.huntButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.huntButton.Click += new System.EventHandler(this.huntButton_Click);
            // 
            // statsButton
            // 
            this.statsButton.BackColor = System.Drawing.Color.Transparent;
            this.statsButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.statsButton.Location = new System.Drawing.Point(12, 132);
            this.statsButton.Name = "statsButton";
            this.statsButton.Padding = new System.Windows.Forms.Padding(2);
            this.statsButton.Size = new System.Drawing.Size(96, 21);
            this.statsButton.TabIndex = 2;
            this.statsButton.Text = "Stats";
            this.statsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.statsButton.Click += new System.EventHandler(this.statsButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.nameLabel.Location = new System.Drawing.Point(30, 102);
            this.nameLabel.MaximumSize = new System.Drawing.Size(96, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(0, 24);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainImage
            // 
            this.mainImage.BackColor = System.Drawing.Color.Transparent;
            this.mainImage.Location = new System.Drawing.Point(12, 12);
            this.mainImage.Name = "mainImage";
            this.mainImage.Size = new System.Drawing.Size(96, 96);
            this.mainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mainImage.TabIndex = 0;
            this.mainImage.TabStop = false;
            // 
            // CreatureDropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 183);
            this.Controls.Add(this.huntButton);
            this.Controls.Add(this.statsButton);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.mainImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreatureDropsForm";
            this.Text = "Creature Drops";
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mainImage;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label statsButton;
        private System.Windows.Forms.Label huntButton;
    }
}