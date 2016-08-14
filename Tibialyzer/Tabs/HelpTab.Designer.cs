namespace Tibialyzer {
    partial class HelpTab {
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
            this.helpPanel = new System.Windows.Forms.Panel();
            this.searchCommandsHeader = new PrettyHeader();
            this.searchCommandHelpBox = new Tibialyzer.EnterTextBox();
            this.SuspendLayout();
            // 
            // helpPanel
            // 
            this.helpPanel.AutoScroll = true;
            this.helpPanel.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.helpPanel.Location = new System.Drawing.Point(9, 64);
            this.helpPanel.Name = "helpPanel";
            this.helpPanel.Size = new System.Drawing.Size(617, 464);
            this.helpPanel.TabIndex = 8;
            // 
            // searchCommandsHeader
            // 
            this.searchCommandsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.searchCommandsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchCommandsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.searchCommandsHeader.Location = new System.Drawing.Point(9, 10);
            this.searchCommandsHeader.Name = "searchCommandsHeader";
            this.searchCommandsHeader.Size = new System.Drawing.Size(617, 30);
            this.searchCommandsHeader.TabIndex = 54;
            this.searchCommandsHeader.Text = "Search Commands";
            this.searchCommandsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // searchCommandHelpBox
            // 
            this.searchCommandHelpBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.searchCommandHelpBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchCommandHelpBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.searchCommandHelpBox.Location = new System.Drawing.Point(9, 40);
            this.searchCommandHelpBox.Name = "searchCommandHelpBox";
            this.searchCommandHelpBox.Size = new System.Drawing.Size(617, 23);
            this.searchCommandHelpBox.TabIndex = 56;
            this.searchCommandHelpBox.TextChanged += new System.EventHandler(this.helpSearchBox_TextChanged);
            this.searchCommandHelpBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.searchCommandHelpBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // HelpTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.helpPanel);
            this.Controls.Add(this.searchCommandsHeader);
            this.Controls.Add(this.searchCommandHelpBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HelpTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel helpPanel;
        private System.Windows.Forms.Label searchCommandsHeader;
        private EnterTextBox searchCommandHelpBox;
        #endregion
    }
}