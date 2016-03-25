namespace Tibialyzer {
    partial class StatusBar {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.healthBarLabel = new Tibialyzer.ProgressBarLabel();
            this.SuspendLayout();
            // 
            // healthBarLabel
            // 
            this.healthBarLabel.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold);
            this.healthBarLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.healthBarLabel.Location = new System.Drawing.Point(0, 0);
            this.healthBarLabel.Name = "healthBarLabel";
            this.healthBarLabel.Size = new System.Drawing.Size(284, 63);
            this.healthBarLabel.TabIndex = 2;
            this.healthBarLabel.Text = "150/150";
            this.healthBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(284, 63);
            this.Controls.Add(this.healthBarLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StatusBar";
            this.Text = "HealthBar";
            this.ResumeLayout(false);

        }

        #endregion
        private ProgressBarLabel healthBarLabel;
    }
}