namespace Tibialyzer {
    partial class HealthListTab {
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
            this.nameListBox = new Tibialyzer.PrettyListBox();
            this.namesLabel = new System.Windows.Forms.Label();
            this.playerImageBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.changePlayerImageButton = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.displayPlayerNameBox = new Tibialyzer.PrettyCheckBox();
            this.displayPlayerImageBox = new Tibialyzer.PrettyCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.playerImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // nameListBox
            // 
            this.nameListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.nameListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.nameListBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.nameListBox.FormattingEnabled = true;
            this.nameListBox.ItemHeight = 20;
            this.nameListBox.Items.AddRange(new object[] {
            "Mytherin",
            "Robin Razend",
            "Scheirke"});
            this.nameListBox.Location = new System.Drawing.Point(12, 67);
            this.nameListBox.Name = "nameListBox";
            this.nameListBox.Size = new System.Drawing.Size(296, 115);
            this.nameListBox.TabIndex = 2;
            this.nameListBox.SelectedIndexChanged += new System.EventHandler(this.nameListBox_SelectedIndexChanged);
            // 
            // namesLabel
            // 
            this.namesLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.namesLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.namesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.namesLabel.Location = new System.Drawing.Point(12, 37);
            this.namesLabel.Name = "namesLabel";
            this.namesLabel.Size = new System.Drawing.Size(296, 30);
            this.namesLabel.TabIndex = 3;
            this.namesLabel.Text = "Health List Names";
            this.namesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playerImageBox
            // 
            this.playerImageBox.BackColor = System.Drawing.Color.Transparent;
            this.playerImageBox.Location = new System.Drawing.Point(314, 71);
            this.playerImageBox.Name = "playerImageBox";
            this.playerImageBox.Size = new System.Drawing.Size(100, 100);
            this.playerImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.playerImageBox.TabIndex = 4;
            this.playerImageBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label1.Location = new System.Drawing.Point(314, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Player Image";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // changePlayerImageButton
            // 
            this.changePlayerImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.changePlayerImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changePlayerImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.changePlayerImageButton.Location = new System.Drawing.Point(420, 71);
            this.changePlayerImageButton.Name = "changePlayerImageButton";
            this.changePlayerImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.changePlayerImageButton.Size = new System.Drawing.Size(106, 38);
            this.changePlayerImageButton.TabIndex = 52;
            this.changePlayerImageButton.Text = "Browse";
            this.changePlayerImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.changePlayerImageButton.Click += new System.EventHandler(this.changePlayerImageButton_Click);
            this.changePlayerImageButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.changePlayerImageButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // label67
            // 
            this.label67.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label67.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label67.Location = new System.Drawing.Point(12, 205);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(296, 30);
            this.label67.TabIndex = 53;
            this.label67.Text = "Display Options";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // displayPlayerNameBox
            // 
            this.displayPlayerNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayPlayerNameBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayPlayerNameBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayPlayerNameBox.Location = new System.Drawing.Point(12, 235);
            this.displayPlayerNameBox.Name = "displayPlayerNameBox";
            this.displayPlayerNameBox.Padding = new System.Windows.Forms.Padding(10);
            this.displayPlayerNameBox.Size = new System.Drawing.Size(296, 40);
            this.displayPlayerNameBox.TabIndex = 54;
            this.displayPlayerNameBox.Text = "Display Player Name";
            this.displayPlayerNameBox.UseVisualStyleBackColor = false;
            this.displayPlayerNameBox.CheckedChanged += new System.EventHandler(this.displayPlayerNameBox_CheckedChanged);
            this.displayPlayerNameBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.displayPlayerNameBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // displayPlayerImageBox
            // 
            this.displayPlayerImageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayPlayerImageBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayPlayerImageBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayPlayerImageBox.Location = new System.Drawing.Point(12, 274);
            this.displayPlayerImageBox.Name = "displayPlayerImageBox";
            this.displayPlayerImageBox.Padding = new System.Windows.Forms.Padding(10);
            this.displayPlayerImageBox.Size = new System.Drawing.Size(296, 40);
            this.displayPlayerImageBox.TabIndex = 55;
            this.displayPlayerImageBox.Text = "Display Player Image";
            this.displayPlayerImageBox.UseVisualStyleBackColor = false;
            this.displayPlayerImageBox.CheckedChanged += new System.EventHandler(this.displayPlayerImageBox_CheckedChanged);
            this.displayPlayerImageBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.displayPlayerImageBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // HealthListTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.Controls.Add(this.label67);
            this.Controls.Add(this.displayPlayerNameBox);
            this.Controls.Add(this.displayPlayerImageBox);
            this.Controls.Add(this.changePlayerImageButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.playerImageBox);
            this.Controls.Add(this.nameListBox);
            this.Controls.Add(this.namesLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HealthListTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.playerImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PrettyListBox nameListBox;
        private System.Windows.Forms.Label namesLabel;
        private System.Windows.Forms.PictureBox playerImageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label changePlayerImageButton;
        private System.Windows.Forms.Label label67;
        private PrettyCheckBox displayPlayerNameBox;
        private PrettyCheckBox displayPlayerImageBox;
    }
}