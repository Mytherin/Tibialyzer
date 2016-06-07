
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
    partial class AchievementForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                base.Cleanup();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AchievementForm));
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.pointsLabel = new System.Windows.Forms.Label();
            this.gradeLabel = new System.Windows.Forms.Label();
            this.mainImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).BeginInit();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.descriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.descriptionLabel.Location = new System.Drawing.Point(111, 31);
            this.descriptionLabel.MaximumSize = new System.Drawing.Size(200, 0);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Padding = new System.Windows.Forms.Padding(3);
            this.descriptionLabel.Size = new System.Drawing.Size(145, 58);
            this.descriptionLabel.TabIndex = 23;
            this.descriptionLabel.Text = "Description: Bla bla bla\r\n\r\n\r\nSpoilers: Bla bla bla";
            // 
            // nameLabel
            // 
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.nameLabel.Location = new System.Drawing.Point(9, 97);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(96, 28);
            this.nameLabel.TabIndex = 10;
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pointsLabel
            // 
            this.pointsLabel.BackColor = System.Drawing.Color.Transparent;
            this.pointsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pointsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.pointsLabel.Location = new System.Drawing.Point(9, 159);
            this.pointsLabel.Name = "pointsLabel";
            this.pointsLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pointsLabel.Size = new System.Drawing.Size(96, 19);
            this.pointsLabel.TabIndex = 2;
            this.pointsLabel.Text = "5 Points";
            this.pointsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeLabel
            // 
            this.gradeLabel.BackColor = System.Drawing.Color.Transparent;
            this.gradeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.gradeLabel.Location = new System.Drawing.Point(9, 141);
            this.gradeLabel.Name = "gradeLabel";
            this.gradeLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.gradeLabel.Size = new System.Drawing.Size(96, 19);
            this.gradeLabel.TabIndex = 1;
            this.gradeLabel.Text = "5 Grade";
            this.gradeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainImage
            // 
            this.mainImage.BackColor = System.Drawing.Color.Transparent;
            this.mainImage.Location = new System.Drawing.Point(25, 28);
            this.mainImage.Name = "mainImage";
            this.mainImage.Size = new System.Drawing.Size(64, 64);
            this.mainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mainImage.TabIndex = 0;
            this.mainImage.TabStop = false;
            // 
            // AchievementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 220);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.pointsLabel);
            this.Controls.Add(this.gradeLabel);
            this.Controls.Add(this.mainImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AchievementForm";
            this.Text = "Achievement Form";
            this.Load += new System.EventHandler(this.AchievementForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mainImage;
        private System.Windows.Forms.Label gradeLabel;
        private System.Windows.Forms.Label pointsLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label descriptionLabel;
    }
}