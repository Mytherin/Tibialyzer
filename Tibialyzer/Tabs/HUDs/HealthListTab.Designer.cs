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
            this.healthListHeader = new PrettyHeader();
            this.playerImageBox = new System.Windows.Forms.PictureBox();
            this.playerImageHeader = new PrettyHeader();
            this.browseButton = new PrettyButton();
            this.displayOptionsHeader = new PrettyHeader();
            this.displayPlayerNameCheckbox = new Tibialyzer.PrettyCheckBox();
            this.displayPlayerImageCheckbox = new Tibialyzer.PrettyCheckBox();
            this.refreshButton = new PrettyButton();
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
            this.nameListBox.Location = new System.Drawing.Point(12, 39);
            this.nameListBox.Name = "nameListBox";
            this.nameListBox.Size = new System.Drawing.Size(296, 115);
            this.nameListBox.TabIndex = 2;
            this.nameListBox.SelectedIndexChanged += new System.EventHandler(this.nameListBox_SelectedIndexChanged);
            // 
            // healthListHeader
            // 
            this.healthListHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.healthListHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.healthListHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.healthListHeader.Location = new System.Drawing.Point(9, 9);
            this.healthListHeader.Name = "healthListHeader";
            this.healthListHeader.Size = new System.Drawing.Size(299, 30);
            this.healthListHeader.TabIndex = 3;
            this.healthListHeader.Text = "Health List Names";
            this.healthListHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playerImageBox
            // 
            this.playerImageBox.BackColor = System.Drawing.Color.Transparent;
            this.playerImageBox.Location = new System.Drawing.Point(330, 42);
            this.playerImageBox.Name = "playerImageBox";
            this.playerImageBox.Size = new System.Drawing.Size(100, 100);
            this.playerImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.playerImageBox.TabIndex = 4;
            this.playerImageBox.TabStop = false;
            // 
            // playerImageHeader
            // 
            this.playerImageHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.playerImageHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerImageHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.playerImageHeader.Location = new System.Drawing.Point(327, 9);
            this.playerImageHeader.Name = "playerImageHeader";
            this.playerImageHeader.Size = new System.Drawing.Size(299, 30);
            this.playerImageHeader.TabIndex = 5;
            this.playerImageHeader.Text = "Player Image";
            this.playerImageHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // browseButton
            // 
            this.browseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.browseButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.browseButton.Location = new System.Drawing.Point(520, 42);
            this.browseButton.Name = "browseButton";
            this.browseButton.Padding = new System.Windows.Forms.Padding(10);
            this.browseButton.Size = new System.Drawing.Size(106, 38);
            this.browseButton.TabIndex = 52;
            this.browseButton.Text = "Browse";
            this.browseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.browseButton.Click += new System.EventHandler(this.changePlayerImageButton_Click);
            // 
            // displayOptionsHeader
            // 
            this.displayOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.displayOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayOptionsHeader.Location = new System.Drawing.Point(12, 160);
            this.displayOptionsHeader.Name = "displayOptionsHeader";
            this.displayOptionsHeader.Size = new System.Drawing.Size(296, 30);
            this.displayOptionsHeader.TabIndex = 53;
            this.displayOptionsHeader.Text = "Display Options";
            this.displayOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // displayPlayerNameCheckbox
            // 
            this.displayPlayerNameCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayPlayerNameCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayPlayerNameCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayPlayerNameCheckbox.Location = new System.Drawing.Point(12, 193);
            this.displayPlayerNameCheckbox.Name = "displayPlayerNameCheckbox";
            this.displayPlayerNameCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.displayPlayerNameCheckbox.Size = new System.Drawing.Size(296, 40);
            this.displayPlayerNameCheckbox.TabIndex = 54;
            this.displayPlayerNameCheckbox.Text = "Display Player Name";
            this.displayPlayerNameCheckbox.UseVisualStyleBackColor = false;
            this.displayPlayerNameCheckbox.CheckedChanged += new System.EventHandler(this.displayPlayerNameBox_CheckedChanged);
            // 
            // displayPlayerImageCheckbox
            // 
            this.displayPlayerImageCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayPlayerImageCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayPlayerImageCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayPlayerImageCheckbox.Location = new System.Drawing.Point(12, 233);
            this.displayPlayerImageCheckbox.Name = "displayPlayerImageCheckbox";
            this.displayPlayerImageCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.displayPlayerImageCheckbox.Size = new System.Drawing.Size(296, 40);
            this.displayPlayerImageCheckbox.TabIndex = 55;
            this.displayPlayerImageCheckbox.Text = "Display Player Image";
            this.displayPlayerImageCheckbox.UseVisualStyleBackColor = false;
            this.displayPlayerImageCheckbox.CheckedChanged += new System.EventHandler(this.displayPlayerImageBox_CheckedChanged);
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.refreshButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.refreshButton.Location = new System.Drawing.Point(258, 285);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Padding = new System.Windows.Forms.Padding(10);
            this.refreshButton.Size = new System.Drawing.Size(172, 38);
            this.refreshButton.TabIndex = 72;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // HealthListTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.displayOptionsHeader);
            this.Controls.Add(this.displayPlayerNameCheckbox);
            this.Controls.Add(this.displayPlayerImageCheckbox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.playerImageHeader);
            this.Controls.Add(this.playerImageBox);
            this.Controls.Add(this.nameListBox);
            this.Controls.Add(this.healthListHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HealthListTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.playerImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PrettyListBox nameListBox;
        private System.Windows.Forms.Label healthListHeader;
        private System.Windows.Forms.PictureBox playerImageBox;
        private System.Windows.Forms.Label playerImageHeader;
        private System.Windows.Forms.Label browseButton;
        private System.Windows.Forms.Label displayOptionsHeader;
        private PrettyCheckBox displayPlayerNameCheckbox;
        private PrettyCheckBox displayPlayerImageCheckbox;
        private System.Windows.Forms.Label refreshButton;
    }
}