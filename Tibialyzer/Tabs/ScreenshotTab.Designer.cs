namespace Tibialyzer {
    partial class ScreenshotTab {
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
            this.screenshotBox = new System.Windows.Forms.PictureBox();
            this.changeScreenshotDirectoryButton = new PrettyButton();
            this.openScreenshotDirectoryButton = new PrettyButton();
            this.screenshotOptionsHeader = new PrettyHeader();
            this.screenshotDirectoryHeader = new PrettyHeader();
            this.screenshotTitleHeader = new PrettyHeader();
            this.screenshotListHeader = new PrettyHeader();
            this.screenshotOnRareItemCheckbox = new Tibialyzer.PrettyCheckBox();
            this.screenshotDeathCheckbox = new Tibialyzer.PrettyCheckBox();
            this.screenshotAdvanceCheckbox = new Tibialyzer.PrettyCheckBox();
            this.enableScreenshotCheckbox = new Tibialyzer.PrettyCheckBox();
            this.screenshotPathBox = new Tibialyzer.EnterTextBox();
            this.screenshotDisplayList = new Tibialyzer.PrettyListBox();
            this.screenshotValueLabel = new System.Windows.Forms.Label();
            this.screenshotValueBox = new Tibialyzer.EnterTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenshotBox)).BeginInit();
            this.SuspendLayout();
            // 
            // screenshotBox
            // 
            this.screenshotBox.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.screenshotBox.Location = new System.Drawing.Point(327, 40);
            this.screenshotBox.Name = "screenshotBox";
            this.screenshotBox.Size = new System.Drawing.Size(299, 184);
            this.screenshotBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.screenshotBox.TabIndex = 0;
            this.screenshotBox.TabStop = false;
            this.screenshotBox.Click += new System.EventHandler(this.screenshotBox_Click);
            // 
            // changeScreenshotDirectoryButton
            // 
            this.changeScreenshotDirectoryButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.changeScreenshotDirectoryButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeScreenshotDirectoryButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.changeScreenshotDirectoryButton.Location = new System.Drawing.Point(509, 320);
            this.changeScreenshotDirectoryButton.Name = "changeScreenshotDirectoryButton";
            this.changeScreenshotDirectoryButton.Padding = new System.Windows.Forms.Padding(10);
            this.changeScreenshotDirectoryButton.Size = new System.Drawing.Size(115, 38);
            this.changeScreenshotDirectoryButton.TabIndex = 51;
            this.changeScreenshotDirectoryButton.Text = "Browse";
            this.changeScreenshotDirectoryButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.changeScreenshotDirectoryButton.Click += new System.EventHandler(this.screenshotBrowse_Click);
            // 
            // openScreenshotDirectoryButton
            // 
            this.openScreenshotDirectoryButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.openScreenshotDirectoryButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openScreenshotDirectoryButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.openScreenshotDirectoryButton.Location = new System.Drawing.Point(326, 320);
            this.openScreenshotDirectoryButton.Name = "openScreenshotDirectoryButton";
            this.openScreenshotDirectoryButton.Padding = new System.Windows.Forms.Padding(10);
            this.openScreenshotDirectoryButton.Size = new System.Drawing.Size(155, 38);
            this.openScreenshotDirectoryButton.TabIndex = 50;
            this.openScreenshotDirectoryButton.Text = "Open In Explorer";
            this.openScreenshotDirectoryButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.openScreenshotDirectoryButton.Click += new System.EventHandler(this.openInExplorer_Click);
            // 
            // screenshotOptionsHeader
            // 
            this.screenshotOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.screenshotOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.screenshotOptionsHeader.Location = new System.Drawing.Point(326, 359);
            this.screenshotOptionsHeader.Name = "screenshotOptionsHeader";
            this.screenshotOptionsHeader.Size = new System.Drawing.Size(299, 30);
            this.screenshotOptionsHeader.TabIndex = 48;
            this.screenshotOptionsHeader.Text = "Screenshot Options";
            this.screenshotOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // screenshotDirectoryHeader
            // 
            this.screenshotDirectoryHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.screenshotDirectoryHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotDirectoryHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.screenshotDirectoryHeader.Location = new System.Drawing.Point(326, 267);
            this.screenshotDirectoryHeader.Name = "screenshotDirectoryHeader";
            this.screenshotDirectoryHeader.Size = new System.Drawing.Size(299, 30);
            this.screenshotDirectoryHeader.TabIndex = 47;
            this.screenshotDirectoryHeader.Text = "Screenshot Directory";
            this.screenshotDirectoryHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // screenshotTitleHeader
            // 
            this.screenshotTitleHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(128)))), ((int)(((byte)(176)))));
            this.screenshotTitleHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.screenshotTitleHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotTitleHeader.Location = new System.Drawing.Point(327, 10);
            this.screenshotTitleHeader.Name = "screenshotTitleHeader";
            this.screenshotTitleHeader.Size = new System.Drawing.Size(299, 30);
            this.screenshotTitleHeader.TabIndex = 46;
            this.screenshotTitleHeader.Text = "Screenshot";
            this.screenshotTitleHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // screenshotListHeader
            // 
            this.screenshotListHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.screenshotListHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotListHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.screenshotListHeader.Location = new System.Drawing.Point(9, 10);
            this.screenshotListHeader.Name = "screenshotListHeader";
            this.screenshotListHeader.Size = new System.Drawing.Size(299, 30);
            this.screenshotListHeader.TabIndex = 44;
            this.screenshotListHeader.Text = "Screenshot List";
            this.screenshotListHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // screenshotOnRareItemCheckbox
            // 
            this.screenshotOnRareItemCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotOnRareItemCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotOnRareItemCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotOnRareItemCheckbox.Location = new System.Drawing.Point(326, 457);
            this.screenshotOnRareItemCheckbox.Name = "screenshotOnRareItemCheckbox";
            this.screenshotOnRareItemCheckbox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.screenshotOnRareItemCheckbox.Size = new System.Drawing.Size(299, 34);
            this.screenshotOnRareItemCheckbox.TabIndex = 60;
            this.screenshotOnRareItemCheckbox.Text = "Screenshot on Rare Item Drop";
            this.screenshotOnRareItemCheckbox.UseVisualStyleBackColor = false;
            this.screenshotOnRareItemCheckbox.CheckedChanged += new System.EventHandler(this.autoScreenshotDrop_CheckedChanged);
            // 
            // screenshotDeathCheckbox
            // 
            this.screenshotDeathCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotDeathCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotDeathCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotDeathCheckbox.Location = new System.Drawing.Point(326, 423);
            this.screenshotDeathCheckbox.Name = "screenshotDeathCheckbox";
            this.screenshotDeathCheckbox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.screenshotDeathCheckbox.Size = new System.Drawing.Size(299, 34);
            this.screenshotDeathCheckbox.TabIndex = 59;
            this.screenshotDeathCheckbox.Text = "Screenshot on Death";
            this.screenshotDeathCheckbox.UseVisualStyleBackColor = false;
            this.screenshotDeathCheckbox.CheckedChanged += new System.EventHandler(this.autoScreenshotDeath_CheckedChanged);
            // 
            // screenshotAdvanceCheckbox
            // 
            this.screenshotAdvanceCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotAdvanceCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotAdvanceCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotAdvanceCheckbox.Location = new System.Drawing.Point(326, 389);
            this.screenshotAdvanceCheckbox.Name = "screenshotAdvanceCheckbox";
            this.screenshotAdvanceCheckbox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.screenshotAdvanceCheckbox.Size = new System.Drawing.Size(299, 34);
            this.screenshotAdvanceCheckbox.TabIndex = 58;
            this.screenshotAdvanceCheckbox.Text = "Screenshot on Skill/Level Advance";
            this.screenshotAdvanceCheckbox.UseVisualStyleBackColor = false;
            this.screenshotAdvanceCheckbox.CheckedChanged += new System.EventHandler(this.autoScreenshot_CheckedChanged);
            // 
            // enableScreenshotCheckbox
            // 
            this.enableScreenshotCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.enableScreenshotCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableScreenshotCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.enableScreenshotCheckbox.Location = new System.Drawing.Point(326, 226);
            this.enableScreenshotCheckbox.Name = "enableScreenshotCheckbox";
            this.enableScreenshotCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.enableScreenshotCheckbox.Size = new System.Drawing.Size(299, 40);
            this.enableScreenshotCheckbox.TabIndex = 52;
            this.enableScreenshotCheckbox.Text = "Enable Screenshots";
            this.enableScreenshotCheckbox.UseVisualStyleBackColor = false;
            this.enableScreenshotCheckbox.CheckedChanged += new System.EventHandler(this.enableScreenshotBox_CheckedChanged);
            // 
            // screenshotPathBox
            // 
            this.screenshotPathBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotPathBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotPathBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.screenshotPathBox.Location = new System.Drawing.Point(326, 297);
            this.screenshotPathBox.Name = "screenshotPathBox";
            this.screenshotPathBox.ReadOnly = true;
            this.screenshotPathBox.Size = new System.Drawing.Size(299, 23);
            this.screenshotPathBox.TabIndex = 49;
            // 
            // screenshotDisplayList
            // 
            this.screenshotDisplayList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotDisplayList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.screenshotDisplayList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.screenshotDisplayList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotDisplayList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotDisplayList.FormattingEnabled = true;
            this.screenshotDisplayList.ItemHeight = 35;
            this.screenshotDisplayList.Items.AddRange(new object[] {
            "Main Hunt"});
            this.screenshotDisplayList.Location = new System.Drawing.Point(9, 40);
            this.screenshotDisplayList.Name = "screenshotDisplayList";
            this.screenshotDisplayList.Size = new System.Drawing.Size(299, 454);
            this.screenshotDisplayList.TabIndex = 45;
            this.screenshotDisplayList.SelectedIndexChanged += new System.EventHandler(this.screenshotList_SelectedIndexChanged);
            // 
            // screenshotValueLabel
            // 
            this.screenshotValueLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotValueLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotValueLabel.Location = new System.Drawing.Point(326, 491);
            this.screenshotValueLabel.Name = "screenshotValueLabel";
            this.screenshotValueLabel.Size = new System.Drawing.Size(175, 23);
            this.screenshotValueLabel.TabIndex = 62;
            this.screenshotValueLabel.Text = "Screenshot Value";
            this.screenshotValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // screenshotValueBox
            // 
            this.screenshotValueBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotValueBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotValueBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.screenshotValueBox.Location = new System.Drawing.Point(501, 491);
            this.screenshotValueBox.Name = "screenshotValueBox";
            this.screenshotValueBox.Size = new System.Drawing.Size(125, 23);
            this.screenshotValueBox.TabIndex = 61;
            this.screenshotValueBox.Text = "50000";
            // 
            // ScreenshotTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.screenshotValueLabel);
            this.Controls.Add(this.screenshotValueBox);
            this.Controls.Add(this.screenshotBox);
            this.Controls.Add(this.changeScreenshotDirectoryButton);
            this.Controls.Add(this.openScreenshotDirectoryButton);
            this.Controls.Add(this.screenshotOptionsHeader);
            this.Controls.Add(this.screenshotDirectoryHeader);
            this.Controls.Add(this.screenshotTitleHeader);
            this.Controls.Add(this.screenshotListHeader);
            this.Controls.Add(this.screenshotOnRareItemCheckbox);
            this.Controls.Add(this.screenshotDeathCheckbox);
            this.Controls.Add(this.screenshotAdvanceCheckbox);
            this.Controls.Add(this.enableScreenshotCheckbox);
            this.Controls.Add(this.screenshotPathBox);
            this.Controls.Add(this.screenshotDisplayList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ScreenshotTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.screenshotBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.PictureBox screenshotBox;
        private System.Windows.Forms.Label changeScreenshotDirectoryButton;
        private System.Windows.Forms.Label openScreenshotDirectoryButton;
        private System.Windows.Forms.Label screenshotOptionsHeader;
        private System.Windows.Forms.Label screenshotDirectoryHeader;
        private System.Windows.Forms.Label screenshotTitleHeader;
        private System.Windows.Forms.Label screenshotListHeader;
        private PrettyCheckBox screenshotOnRareItemCheckbox;
        private PrettyCheckBox screenshotDeathCheckbox;
        private PrettyCheckBox screenshotAdvanceCheckbox;
        private PrettyCheckBox enableScreenshotCheckbox;
        private EnterTextBox screenshotPathBox;
        private PrettyListBox screenshotDisplayList;
        #endregion

        private System.Windows.Forms.Label screenshotValueLabel;
        private EnterTextBox screenshotValueBox;
    }
}