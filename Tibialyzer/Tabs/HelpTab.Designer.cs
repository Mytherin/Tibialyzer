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
            this.SuspendLayout();
            ///
            /// helpPanel
            ///
            this.helpPanel = new System.Windows.Forms.Panel();
            this.helpPanel.AutoScroll = true;
            this.helpPanel.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.helpPanel.Location = new System.Drawing.Point(6, 73);
            this.helpPanel.Name = "helpPanel";
            this.helpPanel.Size = new System.Drawing.Size(531, 424);
            this.helpPanel.TabIndex = 8;
            ///
            /// searchCommandsHeader
            ///
            this.searchCommandsHeader = new System.Windows.Forms.Label();
            this.searchCommandsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.searchCommandsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchCommandsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.searchCommandsHeader.Location = new System.Drawing.Point(3, 19);
            this.searchCommandsHeader.Name = "searchCommandsHeader";
            this.searchCommandsHeader.Size = new System.Drawing.Size(534, 30);
            this.searchCommandsHeader.TabIndex = 54;
            this.searchCommandsHeader.Text = "Search Commands";
            this.searchCommandsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// searchCommandHelpBox
            ///
            this.searchCommandHelpBox = new Tibialyzer.EnterTextBox();
            this.searchCommandHelpBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.searchCommandHelpBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchCommandHelpBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.searchCommandHelpBox.Location = new System.Drawing.Point(3, 49);
            this.searchCommandHelpBox.Name = "searchCommandHelpBox";
            this.searchCommandHelpBox.Size = new System.Drawing.Size(534, 23);
            this.searchCommandHelpBox.TabIndex = 56;
            this.searchCommandHelpBox.TextChanged += new System.EventHandler(this.helpSearchBox_TextChanged);
            this.searchCommandHelpBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.searchCommandHelpBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            //
            // TabBase
            //
            this.Controls.Add(helpPanel);
            this.Controls.Add(searchCommandsHeader);
            this.Controls.Add(searchCommandHelpBox);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabBase";
            this.Text = "TabBase";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel helpPanel;
        private System.Windows.Forms.Label searchCommandsHeader;
        private EnterTextBox searchCommandHelpBox;
        #endregion
    }
}