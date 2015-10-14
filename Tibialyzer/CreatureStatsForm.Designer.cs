namespace Tibialyzer
{
    partial class CreatureStatsForm
    {
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
                if (creature != null) creature.Dispose();
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.resistanceLabel7 = new Tibialyzer.TransparentPictureBox();
            this.resistanceLabel6 = new Tibialyzer.TransparentPictureBox();
            this.resistanceLabel5 = new Tibialyzer.TransparentPictureBox();
            this.resistanceLabel4 = new Tibialyzer.TransparentPictureBox();
            this.resistanceLabel3 = new Tibialyzer.TransparentPictureBox();
            this.resistanceLabel2 = new Tibialyzer.TransparentPictureBox();
            this.resistanceLabel1 = new Tibialyzer.TransparentPictureBox();
            this.expLabel = new System.Windows.Forms.Label();
            this.healthLabel = new System.Windows.Forms.Label();
            this.mainImage = new Tibialyzer.TransparentPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nameLabel.Location = new System.Drawing.Point(27, 94);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(60, 24);
            this.nameLabel.TabIndex = 10;
            this.nameLabel.Text = "Hydra";
            // 
            // resistanceLabel7
            // 
            this.resistanceLabel7.Location = new System.Drawing.Point(119, 126);
            this.resistanceLabel7.Name = "resistanceLabel7";
            this.resistanceLabel7.Size = new System.Drawing.Size(11, 11);
            this.resistanceLabel7.TabIndex = 9;
            this.resistanceLabel7.TabStop = false;
            // 
            // resistanceLabel6
            // 
            this.resistanceLabel6.Location = new System.Drawing.Point(119, 107);
            this.resistanceLabel6.Name = "resistanceLabel6";
            this.resistanceLabel6.Size = new System.Drawing.Size(11, 11);
            this.resistanceLabel6.TabIndex = 8;
            this.resistanceLabel6.TabStop = false;
            // 
            // resistanceLabel5
            // 
            this.resistanceLabel5.Location = new System.Drawing.Point(119, 88);
            this.resistanceLabel5.Name = "resistanceLabel5";
            this.resistanceLabel5.Size = new System.Drawing.Size(11, 11);
            this.resistanceLabel5.TabIndex = 7;
            this.resistanceLabel5.TabStop = false;
            // 
            // resistanceLabel4
            // 
            this.resistanceLabel4.Location = new System.Drawing.Point(119, 69);
            this.resistanceLabel4.Name = "resistanceLabel4";
            this.resistanceLabel4.Size = new System.Drawing.Size(11, 11);
            this.resistanceLabel4.TabIndex = 6;
            this.resistanceLabel4.TabStop = false;
            // 
            // resistanceLabel3
            // 
            this.resistanceLabel3.Location = new System.Drawing.Point(119, 50);
            this.resistanceLabel3.Name = "resistanceLabel3";
            this.resistanceLabel3.Size = new System.Drawing.Size(11, 11);
            this.resistanceLabel3.TabIndex = 5;
            this.resistanceLabel3.TabStop = false;
            // 
            // resistanceLabel2
            // 
            this.resistanceLabel2.Location = new System.Drawing.Point(119, 31);
            this.resistanceLabel2.Name = "resistanceLabel2";
            this.resistanceLabel2.Size = new System.Drawing.Size(11, 11);
            this.resistanceLabel2.TabIndex = 4;
            this.resistanceLabel2.TabStop = false;
            // 
            // resistanceLabel1
            // 
            this.resistanceLabel1.Location = new System.Drawing.Point(119, 12);
            this.resistanceLabel1.Name = "resistanceLabel1";
            this.resistanceLabel1.Size = new System.Drawing.Size(11, 11);
            this.resistanceLabel1.TabIndex = 3;
            this.resistanceLabel1.TabStop = false;
            // 
            // expLabel
            // 
            this.expLabel.AutoSize = true;
            this.expLabel.BackColor = System.Drawing.Color.Salmon;
            this.expLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.expLabel.Location = new System.Drawing.Point(10, 143);
            this.expLabel.Name = "expLabel";
            this.expLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.expLabel.Size = new System.Drawing.Size(52, 17);
            this.expLabel.TabIndex = 2;
            this.expLabel.Text = "2100 Exp";
            this.expLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // healthLabel
            // 
            this.healthLabel.AutoSize = true;
            this.healthLabel.BackColor = System.Drawing.Color.YellowGreen;
            this.healthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.healthLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.healthLabel.Location = new System.Drawing.Point(10, 126);
            this.healthLabel.Name = "healthLabel";
            this.healthLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.healthLabel.Size = new System.Drawing.Size(49, 17);
            this.healthLabel.TabIndex = 1;
            this.healthLabel.Text = "2500 HP";
            this.healthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainImage
            // 
            this.mainImage.Location = new System.Drawing.Point(9, 9);
            this.mainImage.Name = "mainImage";
            this.mainImage.Size = new System.Drawing.Size(96, 96);
            this.mainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.mainImage.TabIndex = 0;
            this.mainImage.TabStop = false;
            // 
            // CreatureStatsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 179);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.resistanceLabel7);
            this.Controls.Add(this.resistanceLabel6);
            this.Controls.Add(this.resistanceLabel5);
            this.Controls.Add(this.resistanceLabel4);
            this.Controls.Add(this.resistanceLabel3);
            this.Controls.Add(this.resistanceLabel2);
            this.Controls.Add(this.resistanceLabel1);
            this.Controls.Add(this.expLabel);
            this.Controls.Add(this.healthLabel);
            this.Controls.Add(this.mainImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreatureStatsForm";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.CreatureStatsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resistanceLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TransparentPictureBox mainImage;
        private System.Windows.Forms.Label healthLabel;
        private System.Windows.Forms.Label expLabel;
        private TransparentPictureBox resistanceLabel1;
        private TransparentPictureBox resistanceLabel2;
        private TransparentPictureBox resistanceLabel3;
        private TransparentPictureBox resistanceLabel4;
        private TransparentPictureBox resistanceLabel5;
        private TransparentPictureBox resistanceLabel6;
        private TransparentPictureBox resistanceLabel7;
        private System.Windows.Forms.Label nameLabel;
    }
}