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
            this.SuspendLayout();
            ///
            /// screenshotBox
            ///
            this.screenshotBox = new System.Windows.Forms.PictureBox();
            this.screenshotBox.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.screenshotBox.Location = new System.Drawing.Point(227, 49);
            this.screenshotBox.Name = "screenshotBox";
            this.screenshotBox.Size = new System.Drawing.Size(310, 184);
            this.screenshotBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.screenshotBox.TabIndex = 0;
            this.screenshotBox.TabStop = false;
            this.screenshotBox.Click += new System.EventHandler(this.screenshotBox_Click);
            ///
            /// changeScreenshotDirectoryButton
            ///
            this.changeScreenshotDirectoryButton = new System.Windows.Forms.Label();
            this.changeScreenshotDirectoryButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.changeScreenshotDirectoryButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeScreenshotDirectoryButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.changeScreenshotDirectoryButton.Location = new System.Drawing.Point(422, 323);
            this.changeScreenshotDirectoryButton.Name = "changeScreenshotDirectoryButton";
            this.changeScreenshotDirectoryButton.Padding = new System.Windows.Forms.Padding(10);
            this.changeScreenshotDirectoryButton.Size = new System.Drawing.Size(115, 38);
            this.changeScreenshotDirectoryButton.TabIndex = 51;
            this.changeScreenshotDirectoryButton.Text = "Browse";
            this.changeScreenshotDirectoryButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.changeScreenshotDirectoryButton.Click += new System.EventHandler(this.screenshotBrowse_Click);
            this.changeScreenshotDirectoryButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.changeScreenshotDirectoryButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// openScreenshotDirectoryButton
            ///
            this.openScreenshotDirectoryButton = new System.Windows.Forms.Label();
            this.openScreenshotDirectoryButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.openScreenshotDirectoryButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openScreenshotDirectoryButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.openScreenshotDirectoryButton.Location = new System.Drawing.Point(226, 323);
            this.openScreenshotDirectoryButton.Name = "openScreenshotDirectoryButton";
            this.openScreenshotDirectoryButton.Padding = new System.Windows.Forms.Padding(10);
            this.openScreenshotDirectoryButton.Size = new System.Drawing.Size(155, 38);
            this.openScreenshotDirectoryButton.TabIndex = 50;
            this.openScreenshotDirectoryButton.Text = "Open In Explorer";
            this.openScreenshotDirectoryButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.openScreenshotDirectoryButton.Click += new System.EventHandler(this.openInExplorer_Click);
            this.openScreenshotDirectoryButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.openScreenshotDirectoryButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// label96
            ///
            this.label96 = new System.Windows.Forms.Label();
            this.label96.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label96.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label96.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label96.Location = new System.Drawing.Point(226, 362);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(311, 30);
            this.label96.TabIndex = 48;
            this.label96.Text = "Screenshot Options";
            this.label96.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// label95
            ///
            this.label95 = new System.Windows.Forms.Label();
            this.label95.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label95.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label95.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label95.Location = new System.Drawing.Point(226, 270);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(311, 30);
            this.label95.TabIndex = 47;
            this.label95.Text = "Screenshot Directory";
            this.label95.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// screenshotTitleLabel
            ///
            this.screenshotTitleLabel = new System.Windows.Forms.Label();
            this.screenshotTitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(128)))), ((int)(((byte)(176)))));
            this.screenshotTitleLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotTitleLabel.Location = new System.Drawing.Point(226, 19);
            this.screenshotTitleLabel.Name = "screenshotTitleLabel";
            this.screenshotTitleLabel.Size = new System.Drawing.Size(311, 30);
            this.screenshotTitleLabel.TabIndex = 46;
            this.screenshotTitleLabel.Text = "Screenshot";
            this.screenshotTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// screenshotListLabel
            ///
            this.screenshotListLabel = new System.Windows.Forms.Label();
            this.screenshotListLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.screenshotListLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotListLabel.Location = new System.Drawing.Point(3, 19);
            this.screenshotListLabel.Name = "screenshotListLabel";
            this.screenshotListLabel.Size = new System.Drawing.Size(219, 30);
            this.screenshotListLabel.TabIndex = 44;
            this.screenshotListLabel.Text = "Screenshot List";
            this.screenshotListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// screenshotRareBox
            ///
            this.screenshotRareBox = new Tibialyzer.PrettyCheckBox();
            this.screenshotRareBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotRareBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotRareBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotRareBox.Location = new System.Drawing.Point(226, 460);
            this.screenshotRareBox.Name = "screenshotRareBox";
            this.screenshotRareBox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.screenshotRareBox.Size = new System.Drawing.Size(311, 34);
            this.screenshotRareBox.TabIndex = 60;
            this.screenshotRareBox.Text = "Screenshot on Rare Item Drop";
            this.screenshotRareBox.UseVisualStyleBackColor = false;
            this.screenshotRareBox.CheckedChanged += new System.EventHandler(this.autoScreenshotDrop_CheckedChanged);
            this.screenshotRareBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.screenshotRareBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// screenshotDeathBox
            ///
            this.screenshotDeathBox = new Tibialyzer.PrettyCheckBox();
            this.screenshotDeathBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotDeathBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotDeathBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotDeathBox.Location = new System.Drawing.Point(226, 426);
            this.screenshotDeathBox.Name = "screenshotDeathBox";
            this.screenshotDeathBox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.screenshotDeathBox.Size = new System.Drawing.Size(311, 34);
            this.screenshotDeathBox.TabIndex = 59;
            this.screenshotDeathBox.Text = "Screenshot on Death";
            this.screenshotDeathBox.UseVisualStyleBackColor = false;
            this.screenshotDeathBox.CheckedChanged += new System.EventHandler(this.autoScreenshotDeath_CheckedChanged);
            this.screenshotDeathBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.screenshotDeathBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// screenshotAdvanceBox
            ///
            this.screenshotAdvanceBox = new Tibialyzer.PrettyCheckBox();
            this.screenshotAdvanceBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotAdvanceBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotAdvanceBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotAdvanceBox.Location = new System.Drawing.Point(226, 392);
            this.screenshotAdvanceBox.Name = "screenshotAdvanceBox";
            this.screenshotAdvanceBox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.screenshotAdvanceBox.Size = new System.Drawing.Size(311, 34);
            this.screenshotAdvanceBox.TabIndex = 58;
            this.screenshotAdvanceBox.Text = "Screenshot on Skill/Level Advance";
            this.screenshotAdvanceBox.UseVisualStyleBackColor = false;
            this.screenshotAdvanceBox.CheckedChanged += new System.EventHandler(this.autoScreenshot_CheckedChanged);
            this.screenshotAdvanceBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.screenshotAdvanceBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// enableScreenshotCheckbox
            ///
            this.enableScreenshotCheckbox = new Tibialyzer.PrettyCheckBox();
            this.enableScreenshotCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.enableScreenshotCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableScreenshotCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.enableScreenshotCheckbox.Location = new System.Drawing.Point(226, 229);
            this.enableScreenshotCheckbox.Name = "enableScreenshotCheckbox";
            this.enableScreenshotCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.enableScreenshotCheckbox.Size = new System.Drawing.Size(312, 40);
            this.enableScreenshotCheckbox.TabIndex = 52;
            this.enableScreenshotCheckbox.Text = "Enable Screenshots";
            this.enableScreenshotCheckbox.UseVisualStyleBackColor = false;
            this.enableScreenshotCheckbox.CheckedChanged += new System.EventHandler(this.enableScreenshotBox_CheckedChanged);
            this.enableScreenshotCheckbox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.enableScreenshotCheckbox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// screenshotPathBox
            ///
            this.screenshotPathBox = new Tibialyzer.EnterTextBox();
            this.screenshotPathBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotPathBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotPathBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.screenshotPathBox.Location = new System.Drawing.Point(226, 300);
            this.screenshotPathBox.Name = "screenshotPathBox";
            this.screenshotPathBox.ReadOnly = true;
            this.screenshotPathBox.Size = new System.Drawing.Size(311, 23);
            this.screenshotPathBox.TabIndex = 49;
            ///
            /// screenshotDisplayList
            ///
            this.screenshotDisplayList = new Tibialyzer.PrettyListBox();
            this.screenshotDisplayList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotDisplayList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.screenshotDisplayList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.screenshotDisplayList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotDisplayList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.screenshotDisplayList.FormattingEnabled = true;
            this.screenshotDisplayList.ItemHeight = 35;
            this.screenshotDisplayList.Items.AddRange(new object[] {
            "Main Hunt"});
            this.screenshotDisplayList.Location = new System.Drawing.Point(3, 49);
            this.screenshotDisplayList.Name = "screenshotDisplayList";
            this.screenshotDisplayList.Size = new System.Drawing.Size(219, 454);
            this.screenshotDisplayList.TabIndex = 45;
            this.screenshotDisplayList.SelectedIndexChanged += new System.EventHandler(this.screenshotList_SelectedIndexChanged);
            //
            // TabBase
            //
            this.Controls.Add(screenshotBox);
            this.Controls.Add(changeScreenshotDirectoryButton);
            this.Controls.Add(openScreenshotDirectoryButton);
            this.Controls.Add(label96);
            this.Controls.Add(label95);
            this.Controls.Add(screenshotTitleLabel);
            this.Controls.Add(screenshotListLabel);
            this.Controls.Add(screenshotRareBox);
            this.Controls.Add(screenshotDeathBox);
            this.Controls.Add(screenshotAdvanceBox);
            this.Controls.Add(enableScreenshotCheckbox);
            this.Controls.Add(screenshotPathBox);
            this.Controls.Add(screenshotDisplayList);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabBase";
            this.Text = "TabBase";
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.PictureBox screenshotBox;
        private System.Windows.Forms.Label changeScreenshotDirectoryButton;
        private System.Windows.Forms.Label openScreenshotDirectoryButton;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label label95;
        private System.Windows.Forms.Label screenshotTitleLabel;
        private System.Windows.Forms.Label screenshotListLabel;
        private PrettyCheckBox screenshotRareBox;
        private PrettyCheckBox screenshotDeathBox;
        private PrettyCheckBox screenshotAdvanceBox;
        private PrettyCheckBox enableScreenshotCheckbox;
        private EnterTextBox screenshotPathBox;
        private PrettyListBox screenshotDisplayList;
        #endregion
    }
}