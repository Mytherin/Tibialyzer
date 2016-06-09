
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
    partial class PlayerForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerForm));
            this.nameLabel = new System.Windows.Forms.Label();
            this.manaLabel = new System.Windows.Forms.Label();
            this.hpLabel = new System.Windows.Forms.Label();
            this.mainImage = new System.Windows.Forms.PictureBox();
            this.levelLabel = new System.Windows.Forms.Label();
            this.vocationLabel = new System.Windows.Forms.Label();
            this.capLabel = new System.Windows.Forms.Label();
            this.sharedLabel = new System.Windows.Forms.Label();
            this.houseLabel = new System.Windows.Forms.Label();
            this.guildLabel = new System.Windows.Forms.Label();
            this.marriageLabel = new System.Windows.Forms.Label();
            this.worldLabel = new System.Windows.Forms.Label();
            this.accountLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).BeginInit();
            this.SuspendLayout();
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
            // manaLabel
            // 
            this.manaLabel.BackColor = System.Drawing.Color.Transparent;
            this.manaLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.manaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manaLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(51)))), ((int)(((byte)(167)))));
            this.manaLabel.Location = new System.Drawing.Point(95, 46);
            this.manaLabel.Name = "manaLabel";
            this.manaLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.manaLabel.Size = new System.Drawing.Size(96, 19);
            this.manaLabel.TabIndex = 2;
            this.manaLabel.Text = "5";
            this.manaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hpLabel
            // 
            this.hpLabel.BackColor = System.Drawing.Color.Transparent;
            this.hpLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(145)))), ((int)(((byte)(72)))));
            this.hpLabel.Location = new System.Drawing.Point(95, 28);
            this.hpLabel.Name = "hpLabel";
            this.hpLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.hpLabel.Size = new System.Drawing.Size(96, 19);
            this.hpLabel.TabIndex = 1;
            this.hpLabel.Text = "5";
            this.hpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainImage
            // 
            this.mainImage.BackColor = System.Drawing.Color.Transparent;
            this.mainImage.Location = new System.Drawing.Point(25, 28);
            this.mainImage.Name = "mainImage";
            this.mainImage.Size = new System.Drawing.Size(64, 64);
            this.mainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mainImage.TabIndex = 0;
            this.mainImage.TabStop = false;
            // 
            // levelLabel
            // 
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
            this.levelLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.levelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelLabel.ForeColor = System.Drawing.Color.Silver;
            this.levelLabel.Location = new System.Drawing.Point(9, 130);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.levelLabel.Size = new System.Drawing.Size(96, 19);
            this.levelLabel.TabIndex = 11;
            this.levelLabel.Text = "Level 5";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // vocationLabel
            // 
            this.vocationLabel.BackColor = System.Drawing.Color.Transparent;
            this.vocationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vocationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vocationLabel.ForeColor = System.Drawing.Color.Silver;
            this.vocationLabel.Location = new System.Drawing.Point(9, 148);
            this.vocationLabel.Name = "vocationLabel";
            this.vocationLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.vocationLabel.Size = new System.Drawing.Size(96, 19);
            this.vocationLabel.TabIndex = 12;
            this.vocationLabel.Text = "Elite Knight";
            this.vocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // capLabel
            // 
            this.capLabel.BackColor = System.Drawing.Color.Transparent;
            this.capLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.capLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.capLabel.ForeColor = System.Drawing.Color.Silver;
            this.capLabel.Location = new System.Drawing.Point(95, 64);
            this.capLabel.Name = "capLabel";
            this.capLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.capLabel.Size = new System.Drawing.Size(96, 19);
            this.capLabel.TabIndex = 13;
            this.capLabel.Text = "5 Cap";
            this.capLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sharedLabel
            // 
            this.sharedLabel.BackColor = System.Drawing.Color.Transparent;
            this.sharedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.sharedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.sharedLabel.Location = new System.Drawing.Point(10, 176);
            this.sharedLabel.Name = "sharedLabel";
            this.sharedLabel.Size = new System.Drawing.Size(96, 30);
            this.sharedLabel.TabIndex = 18;
            this.sharedLabel.Text = "Shared Range\r\n600 - 1400";
            this.sharedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // houseLabel
            // 
            this.houseLabel.BackColor = System.Drawing.Color.Transparent;
            this.houseLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.houseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.houseLabel.ForeColor = System.Drawing.Color.Silver;
            this.houseLabel.Location = new System.Drawing.Point(197, 46);
            this.houseLabel.Name = "houseLabel";
            this.houseLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.houseLabel.Size = new System.Drawing.Size(168, 19);
            this.houseLabel.TabIndex = 19;
            this.houseLabel.Text = "House: ";
            this.houseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // guildLabel
            // 
            this.guildLabel.BackColor = System.Drawing.Color.Transparent;
            this.guildLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.guildLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guildLabel.ForeColor = System.Drawing.Color.Silver;
            this.guildLabel.Location = new System.Drawing.Point(197, 64);
            this.guildLabel.Name = "guildLabel";
            this.guildLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.guildLabel.Size = new System.Drawing.Size(168, 19);
            this.guildLabel.TabIndex = 20;
            this.guildLabel.Text = "Guild:";
            this.guildLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // marriageLabel
            // 
            this.marriageLabel.BackColor = System.Drawing.Color.Transparent;
            this.marriageLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.marriageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.marriageLabel.ForeColor = System.Drawing.Color.Silver;
            this.marriageLabel.Location = new System.Drawing.Point(197, 82);
            this.marriageLabel.Name = "marriageLabel";
            this.marriageLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.marriageLabel.Size = new System.Drawing.Size(168, 19);
            this.marriageLabel.TabIndex = 21;
            this.marriageLabel.Text = "Marriage:";
            this.marriageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // worldLabel
            // 
            this.worldLabel.BackColor = System.Drawing.Color.Transparent;
            this.worldLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.worldLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.worldLabel.ForeColor = System.Drawing.Color.Silver;
            this.worldLabel.Location = new System.Drawing.Point(197, 28);
            this.worldLabel.Name = "worldLabel";
            this.worldLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.worldLabel.Size = new System.Drawing.Size(168, 19);
            this.worldLabel.TabIndex = 22;
            this.worldLabel.Text = "World:";
            this.worldLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // accountLabel
            // 
            this.accountLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountLabel.ForeColor = System.Drawing.Color.Maroon;
            this.accountLabel.Location = new System.Drawing.Point(95, 82);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.accountLabel.Size = new System.Drawing.Size(96, 19);
            this.accountLabel.TabIndex = 23;
            this.accountLabel.Text = "Free Account";
            this.accountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 220);
            this.Controls.Add(this.accountLabel);
            this.Controls.Add(this.worldLabel);
            this.Controls.Add(this.marriageLabel);
            this.Controls.Add(this.guildLabel);
            this.Controls.Add(this.houseLabel);
            this.Controls.Add(this.sharedLabel);
            this.Controls.Add(this.capLabel);
            this.Controls.Add(this.vocationLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.manaLabel);
            this.Controls.Add(this.hpLabel);
            this.Controls.Add(this.mainImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlayerForm";
            this.Text = "Player Form";
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mainImage;
        private System.Windows.Forms.Label hpLabel;
        private System.Windows.Forms.Label manaLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Label vocationLabel;
        private System.Windows.Forms.Label capLabel;
        private System.Windows.Forms.Label sharedLabel;
        private System.Windows.Forms.Label houseLabel;
        private System.Windows.Forms.Label guildLabel;
        private System.Windows.Forms.Label marriageLabel;
        private System.Windows.Forms.Label worldLabel;
        private System.Windows.Forms.Label accountLabel;
    }
}