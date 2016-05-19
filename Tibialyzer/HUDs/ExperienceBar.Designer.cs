namespace Tibialyzer {
    partial class ExperienceBar {
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
            this.experienceBarLabel = new Tibialyzer.ProgressBarLabel();
            this.SuspendLayout();
            // 
            // healthBarLabel
            // 
            this.experienceBarLabel.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold);
            this.experienceBarLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.experienceBarLabel.Location = new System.Drawing.Point(0, 0);
            this.experienceBarLabel.Name = "healthBarLabel";
            this.experienceBarLabel.Size = new System.Drawing.Size(284, 63);
            this.experienceBarLabel.TabIndex = 2;
            this.experienceBarLabel.Text = "150/150";
            this.experienceBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(284, 63);
            this.Controls.Add(this.experienceBarLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Tibialyzer (Status Bar)";
            this.Text = "Tibialyzer (Status Bar)";
            this.ResumeLayout(false);

        }

        #endregion
        private ProgressBarLabel experienceBarLabel;
    }
}