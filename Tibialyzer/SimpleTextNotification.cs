using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tibialyzer {
    class SimpleTextNotification : SimpleNotification {
        private Label titleLabel;
        private Label textLabel;
        private PictureBox notificationImage;
        public SimpleTextNotification(Image image, string title, string text) {
            this.InitializeComponent();

            this.notificationImage.Image = image == null ? MainForm.infoIcon : image;
            this.titleLabel.Text = title;
            this.textLabel.Text = text;

            this.InitializeSimpleNotification();
        }

        private void InitializeComponent() {
            this.textLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.notificationImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.notificationImage)).BeginInit();
            this.SuspendLayout();
            // 
            // textLabel
            // 
            this.textLabel.AutoSize = true;
            this.textLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.textLabel.Location = new System.Drawing.Point(60, 25);
            this.textLabel.MaximumSize = new System.Drawing.Size(290, 0);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(286, 32);
            this.textLabel.TabIndex = 2;
            this.textLabel.Text = "Currently gaining 0 experience per hour. Currently gaining 0 experience per hour." +
    "";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.titleLabel.Location = new System.Drawing.Point(60, 5);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(86, 16);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Experience";
            // 
            // notificationImage
            // 
            this.notificationImage.Location = new System.Drawing.Point(7, 7);
            this.notificationImage.Name = "notificationImage";
            this.notificationImage.Size = new System.Drawing.Size(45, 45);
            this.notificationImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.notificationImage.TabIndex = 0;
            this.notificationImage.TabStop = false;
            // 
            // SimpleTextNotification
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(354, 60);
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.notificationImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SimpleTextNotification";
            ((System.ComponentModel.ISupportInitialize)(this.notificationImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
