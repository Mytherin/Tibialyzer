namespace Tibialyzer {
    partial class AutoHotkeyTab {
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
            this.downloadBar = new System.Windows.Forms.ProgressBar();
            this.closeSuspendedWindowButton = new Tibialyzer.PrettyButton();
            this.suspendedTestButton = new Tibialyzer.PrettyButton();
            this.testingHeader = new Tibialyzer.PrettyHeader();
            this.shutdownAutoHotkeyButton = new Tibialyzer.PrettyButton();
            this.startAutoHotkeyButton = new Tibialyzer.PrettyButton();
            this.downloadAutoHotkeyButton = new Tibialyzer.PrettyButton();
            this.startAutoHotkeyHeader = new Tibialyzer.PrettyHeader();
            this.yOffsetLabel = new System.Windows.Forms.Label();
            this.xOffsetLabel = new System.Windows.Forms.Label();
            this.positionOffsetHeader = new Tibialyzer.PrettyHeader();
            this.anchorHeader = new Tibialyzer.PrettyHeader();
            this.autoHotkeyOptionsHeader = new Tibialyzer.PrettyHeader();
            this.autoHotkeyDownloadHeader = new Tibialyzer.PrettyHeader();
            this.autoHotkeyScriptHeader = new Tibialyzer.PrettyHeader();
            this.suspendedXOffsetBox = new Tibialyzer.EnterTextBox();
            this.suspendedYOffsetBox = new Tibialyzer.EnterTextBox();
            this.exitScriptOnExitCheckbox = new Tibialyzer.PrettyCheckBox();
            this.startScriptOnStartupBox = new Tibialyzer.PrettyCheckBox();
            this.suspendedAnchorDropDownList = new Tibialyzer.PrettyDropDownList();
            this.autoHotkeyGridSettings = new Tibialyzer.RichTextBoxAutoHotkey();
            this.SuspendLayout();
            // 
            // downloadBar
            // 
            this.downloadBar.Location = new System.Drawing.Point(333, 79);
            this.downloadBar.Name = "downloadBar";
            this.downloadBar.Size = new System.Drawing.Size(304, 23);
            this.downloadBar.TabIndex = 2;
            this.downloadBar.Visible = false;
            // 
            // closeSuspendedWindowButton
            // 
            this.closeSuspendedWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.closeSuspendedWindowButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeSuspendedWindowButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.closeSuspendedWindowButton.Location = new System.Drawing.Point(333, 493);
            this.closeSuspendedWindowButton.Name = "closeSuspendedWindowButton";
            this.closeSuspendedWindowButton.Size = new System.Drawing.Size(304, 25);
            this.closeSuspendedWindowButton.TabIndex = 61;
            this.closeSuspendedWindowButton.Text = "Close Suspended Window";
            this.closeSuspendedWindowButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.closeSuspendedWindowButton.Click += new System.EventHandler(this.closeSuspendedWindow_Click);
            // 
            // suspendedTestButton
            // 
            this.suspendedTestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedTestButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedTestButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.suspendedTestButton.Location = new System.Drawing.Point(333, 468);
            this.suspendedTestButton.Name = "suspendedTestButton";
            this.suspendedTestButton.Size = new System.Drawing.Size(304, 25);
            this.suspendedTestButton.TabIndex = 60;
            this.suspendedTestButton.Text = "Test Suspended Window";
            this.suspendedTestButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.suspendedTestButton.Click += new System.EventHandler(this.suspendedTest_Click);
            // 
            // testingHeader
            // 
            this.testingHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.testingHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testingHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.testingHeader.Location = new System.Drawing.Point(333, 438);
            this.testingHeader.Name = "testingHeader";
            this.testingHeader.Size = new System.Drawing.Size(304, 30);
            this.testingHeader.TabIndex = 59;
            this.testingHeader.Text = "Testing";
            this.testingHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shutdownAutoHotkeyButton
            // 
            this.shutdownAutoHotkeyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.shutdownAutoHotkeyButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shutdownAutoHotkeyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.shutdownAutoHotkeyButton.Location = new System.Drawing.Point(333, 166);
            this.shutdownAutoHotkeyButton.Name = "shutdownAutoHotkeyButton";
            this.shutdownAutoHotkeyButton.Padding = new System.Windows.Forms.Padding(10);
            this.shutdownAutoHotkeyButton.Size = new System.Drawing.Size(304, 42);
            this.shutdownAutoHotkeyButton.TabIndex = 56;
            this.shutdownAutoHotkeyButton.Text = "Shutdown AutoHotkey";
            this.shutdownAutoHotkeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.shutdownAutoHotkeyButton.Click += new System.EventHandler(this.shutdownAutoHotkey_Click);
            // 
            // startAutoHotkeyButton
            // 
            this.startAutoHotkeyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.startAutoHotkeyButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startAutoHotkeyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.startAutoHotkeyButton.Location = new System.Drawing.Point(333, 132);
            this.startAutoHotkeyButton.Name = "startAutoHotkeyButton";
            this.startAutoHotkeyButton.Size = new System.Drawing.Size(304, 34);
            this.startAutoHotkeyButton.TabIndex = 55;
            this.startAutoHotkeyButton.Text = "(Re)start AutoHotkey";
            this.startAutoHotkeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.startAutoHotkeyButton.Click += new System.EventHandler(this.startAutoHotkey_Click);
            // 
            // downloadAutoHotkeyButton
            // 
            this.downloadAutoHotkeyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.downloadAutoHotkeyButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAutoHotkeyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.downloadAutoHotkeyButton.Location = new System.Drawing.Point(333, 38);
            this.downloadAutoHotkeyButton.Name = "downloadAutoHotkeyButton";
            this.downloadAutoHotkeyButton.Padding = new System.Windows.Forms.Padding(10);
            this.downloadAutoHotkeyButton.Size = new System.Drawing.Size(304, 40);
            this.downloadAutoHotkeyButton.TabIndex = 54;
            this.downloadAutoHotkeyButton.Text = "Download AutoHotkey";
            this.downloadAutoHotkeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.downloadAutoHotkeyButton.Click += new System.EventHandler(this.downloadAutoHotkey_Click);
            // 
            // startAutoHotkeyHeader
            // 
            this.startAutoHotkeyHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.startAutoHotkeyHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startAutoHotkeyHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.startAutoHotkeyHeader.Location = new System.Drawing.Point(333, 102);
            this.startAutoHotkeyHeader.Name = "startAutoHotkeyHeader";
            this.startAutoHotkeyHeader.Size = new System.Drawing.Size(304, 30);
            this.startAutoHotkeyHeader.TabIndex = 53;
            this.startAutoHotkeyHeader.Text = "Start";
            this.startAutoHotkeyHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // yOffsetLabel
            // 
            this.yOffsetLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.yOffsetLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.yOffsetLabel.Location = new System.Drawing.Point(333, 360);
            this.yOffsetLabel.Name = "yOffsetLabel";
            this.yOffsetLabel.Size = new System.Drawing.Size(138, 23);
            this.yOffsetLabel.TabIndex = 52;
            this.yOffsetLabel.Text = "Y Offset";
            this.yOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xOffsetLabel
            // 
            this.xOffsetLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.xOffsetLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.xOffsetLabel.Location = new System.Drawing.Point(333, 337);
            this.xOffsetLabel.Name = "xOffsetLabel";
            this.xOffsetLabel.Size = new System.Drawing.Size(138, 23);
            this.xOffsetLabel.TabIndex = 50;
            this.xOffsetLabel.Text = "X Offset";
            this.xOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // positionOffsetHeader
            // 
            this.positionOffsetHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.positionOffsetHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positionOffsetHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.positionOffsetHeader.Location = new System.Drawing.Point(333, 307);
            this.positionOffsetHeader.Name = "positionOffsetHeader";
            this.positionOffsetHeader.Size = new System.Drawing.Size(304, 30);
            this.positionOffsetHeader.TabIndex = 48;
            this.positionOffsetHeader.Text = "Position (Offset)";
            this.positionOffsetHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // anchorHeader
            // 
            this.anchorHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anchorHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.anchorHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.anchorHeader.Location = new System.Drawing.Point(333, 384);
            this.anchorHeader.Name = "anchorHeader";
            this.anchorHeader.Size = new System.Drawing.Size(304, 30);
            this.anchorHeader.TabIndex = 47;
            this.anchorHeader.Text = "Anchor";
            this.anchorHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoHotkeyOptionsHeader
            // 
            this.autoHotkeyOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.autoHotkeyOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.autoHotkeyOptionsHeader.Location = new System.Drawing.Point(333, 208);
            this.autoHotkeyOptionsHeader.Name = "autoHotkeyOptionsHeader";
            this.autoHotkeyOptionsHeader.Size = new System.Drawing.Size(304, 30);
            this.autoHotkeyOptionsHeader.TabIndex = 45;
            this.autoHotkeyOptionsHeader.Text = "Options";
            this.autoHotkeyOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoHotkeyDownloadHeader
            // 
            this.autoHotkeyDownloadHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.autoHotkeyDownloadHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyDownloadHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.autoHotkeyDownloadHeader.Location = new System.Drawing.Point(333, 8);
            this.autoHotkeyDownloadHeader.Name = "autoHotkeyDownloadHeader";
            this.autoHotkeyDownloadHeader.Size = new System.Drawing.Size(304, 30);
            this.autoHotkeyDownloadHeader.TabIndex = 44;
            this.autoHotkeyDownloadHeader.Text = "Download";
            this.autoHotkeyDownloadHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoHotkeyScriptHeader
            // 
            this.autoHotkeyScriptHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.autoHotkeyScriptHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyScriptHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.autoHotkeyScriptHeader.Location = new System.Drawing.Point(9, 8);
            this.autoHotkeyScriptHeader.Name = "autoHotkeyScriptHeader";
            this.autoHotkeyScriptHeader.Size = new System.Drawing.Size(299, 30);
            this.autoHotkeyScriptHeader.TabIndex = 43;
            this.autoHotkeyScriptHeader.Text = "AutoHotkey Script";
            this.autoHotkeyScriptHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // suspendedXOffsetBox
            // 
            this.suspendedXOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedXOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedXOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.suspendedXOffsetBox.Location = new System.Drawing.Point(471, 337);
            this.suspendedXOffsetBox.Name = "suspendedXOffsetBox";
            this.suspendedXOffsetBox.Size = new System.Drawing.Size(166, 23);
            this.suspendedXOffsetBox.TabIndex = 49;
            this.suspendedXOffsetBox.TextChanged += new System.EventHandler(this.suspendedXOffset_TextChanged);
            // 
            // suspendedYOffsetBox
            // 
            this.suspendedYOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedYOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedYOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.suspendedYOffsetBox.Location = new System.Drawing.Point(471, 360);
            this.suspendedYOffsetBox.Name = "suspendedYOffsetBox";
            this.suspendedYOffsetBox.Size = new System.Drawing.Size(166, 23);
            this.suspendedYOffsetBox.TabIndex = 51;
            this.suspendedYOffsetBox.TextChanged += new System.EventHandler(this.suspendedYOffset_TextChanged);
            // 
            // exitScriptOnExitCheckbox
            // 
            this.exitScriptOnExitCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.exitScriptOnExitCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitScriptOnExitCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.exitScriptOnExitCheckbox.Location = new System.Drawing.Point(333, 272);
            this.exitScriptOnExitCheckbox.Name = "exitScriptOnExitCheckbox";
            this.exitScriptOnExitCheckbox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.exitScriptOnExitCheckbox.Size = new System.Drawing.Size(304, 34);
            this.exitScriptOnExitCheckbox.TabIndex = 58;
            this.exitScriptOnExitCheckbox.Text = "Exit Script On Exit";
            this.exitScriptOnExitCheckbox.UseVisualStyleBackColor = false;
            this.exitScriptOnExitCheckbox.CheckedChanged += new System.EventHandler(this.shutdownOnExit_CheckedChanged);
            // 
            // startScriptOnStartupBox
            // 
            this.startScriptOnStartupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.startScriptOnStartupBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startScriptOnStartupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.startScriptOnStartupBox.Location = new System.Drawing.Point(333, 238);
            this.startScriptOnStartupBox.Name = "startScriptOnStartupBox";
            this.startScriptOnStartupBox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.startScriptOnStartupBox.Size = new System.Drawing.Size(304, 34);
            this.startScriptOnStartupBox.TabIndex = 57;
            this.startScriptOnStartupBox.Text = "Start Script On StartUp";
            this.startScriptOnStartupBox.UseVisualStyleBackColor = false;
            this.startScriptOnStartupBox.CheckedChanged += new System.EventHandler(this.startAutohotkeyScript_CheckedChanged);
            // 
            // suspendedAnchorDropDownList
            // 
            this.suspendedAnchorDropDownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedAnchorDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.suspendedAnchorDropDownList.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedAnchorDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.suspendedAnchorDropDownList.FormattingEnabled = true;
            this.suspendedAnchorDropDownList.Items.AddRange(new object[] {
            "Top Left",
            "Top Right",
            "Bottom Left",
            "Bottom Right"});
            this.suspendedAnchorDropDownList.Location = new System.Drawing.Point(333, 414);
            this.suspendedAnchorDropDownList.Name = "suspendedAnchorDropDownList";
            this.suspendedAnchorDropDownList.Size = new System.Drawing.Size(304, 24);
            this.suspendedAnchorDropDownList.TabIndex = 46;
            this.suspendedAnchorDropDownList.SelectedIndexChanged += new System.EventHandler(this.suspendedAnchor_SelectedIndexChanged);
            // 
            // autoHotkeyGridSettings
            // 
            this.autoHotkeyGridSettings.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyGridSettings.Location = new System.Drawing.Point(9, 43);
            this.autoHotkeyGridSettings.Name = "autoHotkeyGridSettings";
            this.autoHotkeyGridSettings.Size = new System.Drawing.Size(299, 492);
            this.autoHotkeyGridSettings.TabIndex = 5;
            this.autoHotkeyGridSettings.Text = "";
            this.autoHotkeyGridSettings.TextChanged += new System.EventHandler(this.autoHotkeyGridSettings_TextChanged);
            // 
            // AutoHotkeyTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.downloadBar);
            this.Controls.Add(this.closeSuspendedWindowButton);
            this.Controls.Add(this.suspendedTestButton);
            this.Controls.Add(this.testingHeader);
            this.Controls.Add(this.shutdownAutoHotkeyButton);
            this.Controls.Add(this.startAutoHotkeyButton);
            this.Controls.Add(this.downloadAutoHotkeyButton);
            this.Controls.Add(this.startAutoHotkeyHeader);
            this.Controls.Add(this.yOffsetLabel);
            this.Controls.Add(this.xOffsetLabel);
            this.Controls.Add(this.positionOffsetHeader);
            this.Controls.Add(this.anchorHeader);
            this.Controls.Add(this.autoHotkeyOptionsHeader);
            this.Controls.Add(this.autoHotkeyDownloadHeader);
            this.Controls.Add(this.autoHotkeyScriptHeader);
            this.Controls.Add(this.suspendedXOffsetBox);
            this.Controls.Add(this.suspendedYOffsetBox);
            this.Controls.Add(this.exitScriptOnExitCheckbox);
            this.Controls.Add(this.startScriptOnStartupBox);
            this.Controls.Add(this.suspendedAnchorDropDownList);
            this.Controls.Add(this.autoHotkeyGridSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoHotkeyTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.ProgressBar downloadBar;
        private System.Windows.Forms.Label yOffsetLabel;
        private System.Windows.Forms.Label xOffsetLabel;
        private EnterTextBox suspendedXOffsetBox;
        private EnterTextBox suspendedYOffsetBox;
        private PrettyCheckBox exitScriptOnExitCheckbox;
        private PrettyCheckBox startScriptOnStartupBox;
        private PrettyDropDownList suspendedAnchorDropDownList;
        private RichTextBoxAutoHotkey autoHotkeyGridSettings;
        #endregion

        private PrettyHeader autoHotkeyScriptHeader;
        private PrettyHeader autoHotkeyDownloadHeader;
        private PrettyHeader autoHotkeyOptionsHeader;
        private PrettyHeader anchorHeader;
        private PrettyHeader positionOffsetHeader;
        private PrettyHeader startAutoHotkeyHeader;
        private PrettyButton downloadAutoHotkeyButton;
        private PrettyButton startAutoHotkeyButton;
        private PrettyButton shutdownAutoHotkeyButton;
        private PrettyHeader testingHeader;
        private PrettyButton suspendedTestButton;
        private PrettyButton closeSuspendedWindowButton;
    }
}