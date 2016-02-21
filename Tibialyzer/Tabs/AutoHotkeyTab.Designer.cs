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
            this.SuspendLayout();

            ///
            /// downloadBar
            ///
            this.downloadBar = new System.Windows.Forms.ProgressBar();
            this.downloadBar.Location = new System.Drawing.Point(333, 75);
            this.downloadBar.Name = "downloadBar";
            this.downloadBar.Size = new System.Drawing.Size(204, 23);
            this.downloadBar.TabIndex = 2;
            this.downloadBar.Visible = false;
            ///
            /// closeSuspendedWindowButton
            ///
            this.closeSuspendedWindowButton = new System.Windows.Forms.Label();
            this.closeSuspendedWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.closeSuspendedWindowButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeSuspendedWindowButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.closeSuspendedWindowButton.Location = new System.Drawing.Point(333, 469);
            this.closeSuspendedWindowButton.Name = "closeSuspendedWindowButton";
            this.closeSuspendedWindowButton.Size = new System.Drawing.Size(204, 25);
            this.closeSuspendedWindowButton.TabIndex = 61;
            this.closeSuspendedWindowButton.Text = "Close Suspended Window";
            this.closeSuspendedWindowButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.closeSuspendedWindowButton.Click += new System.EventHandler(this.closeSuspendedWindow_Click);
            this.closeSuspendedWindowButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.closeSuspendedWindowButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// suspendedTestButton
            ///
            this.suspendedTestButton = new System.Windows.Forms.Label();
            this.suspendedTestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedTestButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedTestButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.suspendedTestButton.Location = new System.Drawing.Point(333, 444);
            this.suspendedTestButton.Name = "suspendedTestButton";
            this.suspendedTestButton.Size = new System.Drawing.Size(204, 25);
            this.suspendedTestButton.TabIndex = 60;
            this.suspendedTestButton.Text = "Test Suspended Window";
            this.suspendedTestButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.suspendedTestButton.Click += new System.EventHandler(this.suspendedTest_Click);
            this.suspendedTestButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.suspendedTestButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// label92
            ///
            this.label92 = new System.Windows.Forms.Label();
            this.label92.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label92.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label92.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label92.Location = new System.Drawing.Point(333, 414);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(204, 30);
            this.label92.TabIndex = 59;
            this.label92.Text = "Testing";
            this.label92.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// shutdownAutoHotkeyButton
            ///
            this.shutdownAutoHotkeyButton = new System.Windows.Forms.Label();
            this.shutdownAutoHotkeyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.shutdownAutoHotkeyButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shutdownAutoHotkeyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.shutdownAutoHotkeyButton.Location = new System.Drawing.Point(333, 149);
            this.shutdownAutoHotkeyButton.Name = "shutdownAutoHotkeyButton";
            this.shutdownAutoHotkeyButton.Padding = new System.Windows.Forms.Padding(10);
            this.shutdownAutoHotkeyButton.Size = new System.Drawing.Size(204, 34);
            this.shutdownAutoHotkeyButton.TabIndex = 56;
            this.shutdownAutoHotkeyButton.Text = "Shutdown AutoHotkey";
            this.shutdownAutoHotkeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.shutdownAutoHotkeyButton.Click += new System.EventHandler(this.shutdownAutoHotkey_Click);
            this.shutdownAutoHotkeyButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.shutdownAutoHotkeyButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// startAutoHotkeyButton
            ///
            this.startAutoHotkeyButton = new System.Windows.Forms.Label();
            this.startAutoHotkeyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.startAutoHotkeyButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startAutoHotkeyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.startAutoHotkeyButton.Location = new System.Drawing.Point(333, 115);
            this.startAutoHotkeyButton.Name = "startAutoHotkeyButton";
            this.startAutoHotkeyButton.Size = new System.Drawing.Size(204, 34);
            this.startAutoHotkeyButton.TabIndex = 55;
            this.startAutoHotkeyButton.Text = "(Re)start AutoHotkey";
            this.startAutoHotkeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.startAutoHotkeyButton.Click += new System.EventHandler(this.startAutoHotkey_Click);
            this.startAutoHotkeyButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.startAutoHotkeyButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// downloadAutoHotkeyButton
            ///
            this.downloadAutoHotkeyButton = new System.Windows.Forms.Label();
            this.downloadAutoHotkeyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.downloadAutoHotkeyButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAutoHotkeyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.downloadAutoHotkeyButton.Location = new System.Drawing.Point(333, 49);
            this.downloadAutoHotkeyButton.Name = "downloadAutoHotkeyButton";
            this.downloadAutoHotkeyButton.Padding = new System.Windows.Forms.Padding(10);
            this.downloadAutoHotkeyButton.Size = new System.Drawing.Size(204, 34);
            this.downloadAutoHotkeyButton.TabIndex = 54;
            this.downloadAutoHotkeyButton.Text = "Download AutoHotkey";
            this.downloadAutoHotkeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.downloadAutoHotkeyButton.Click += new System.EventHandler(this.downloadAutoHotkey_Click);
            this.downloadAutoHotkeyButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.downloadAutoHotkeyButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// label88
            ///
            this.label88 = new System.Windows.Forms.Label();
            this.label88.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label88.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label88.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label88.Location = new System.Drawing.Point(333, 85);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(204, 30);
            this.label88.TabIndex = 53;
            this.label88.Text = "Start";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// label84
            ///
            this.label84 = new System.Windows.Forms.Label();
            this.label84.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label84.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label84.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.label84.Location = new System.Drawing.Point(333, 336);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(98, 23);
            this.label84.TabIndex = 52;
            this.label84.Text = "Y Offset";
            this.label84.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// label85
            ///
            this.label85 = new System.Windows.Forms.Label();
            this.label85.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label85.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label85.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.label85.Location = new System.Drawing.Point(333, 313);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(98, 23);
            this.label85.TabIndex = 50;
            this.label85.Text = "X Offset";
            this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// label86
            ///
            this.label86 = new System.Windows.Forms.Label();
            this.label86.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label86.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label86.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label86.Location = new System.Drawing.Point(333, 283);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(204, 30);
            this.label86.TabIndex = 48;
            this.label86.Text = "Position (Offset)";
            this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// label87
            ///
            this.label87 = new System.Windows.Forms.Label();
            this.label87.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label87.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label87.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label87.Location = new System.Drawing.Point(333, 360);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(204, 30);
            this.label87.TabIndex = 47;
            this.label87.Text = "Anchor";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// autoHotkeyOptionsHeader
            ///
            this.autoHotkeyOptionsHeader = new System.Windows.Forms.Label();
            this.autoHotkeyOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.autoHotkeyOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.autoHotkeyOptionsHeader.Location = new System.Drawing.Point(333, 184);
            this.autoHotkeyOptionsHeader.Name = "autoHotkeyOptionsHeader";
            this.autoHotkeyOptionsHeader.Size = new System.Drawing.Size(204, 30);
            this.autoHotkeyOptionsHeader.TabIndex = 45;
            this.autoHotkeyOptionsHeader.Text = "Options";
            this.autoHotkeyOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// autoHotkeyDownloadHeader
            ///
            this.autoHotkeyDownloadHeader = new System.Windows.Forms.Label();
            this.autoHotkeyDownloadHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.autoHotkeyDownloadHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyDownloadHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.autoHotkeyDownloadHeader.Location = new System.Drawing.Point(333, 19);
            this.autoHotkeyDownloadHeader.Name = "autoHotkeyDownloadHeader";
            this.autoHotkeyDownloadHeader.Size = new System.Drawing.Size(204, 30);
            this.autoHotkeyDownloadHeader.TabIndex = 44;
            this.autoHotkeyDownloadHeader.Text = "Download";
            this.autoHotkeyDownloadHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// autoHotkeyScriptHeader
            ///
            this.autoHotkeyScriptHeader = new System.Windows.Forms.Label();
            this.autoHotkeyScriptHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.autoHotkeyScriptHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyScriptHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.autoHotkeyScriptHeader.Location = new System.Drawing.Point(3, 19);
            this.autoHotkeyScriptHeader.Name = "autoHotkeyScriptHeader";
            this.autoHotkeyScriptHeader.Size = new System.Drawing.Size(324, 30);
            this.autoHotkeyScriptHeader.TabIndex = 43;
            this.autoHotkeyScriptHeader.Text = "AutoHotkey Script";
            this.autoHotkeyScriptHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// suspendedXOffsetBox
            ///
            this.suspendedXOffsetBox = new Tibialyzer.EnterTextBox();
            this.suspendedXOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedXOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedXOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.suspendedXOffsetBox.Location = new System.Drawing.Point(431, 313);
            this.suspendedXOffsetBox.Name = "suspendedXOffsetBox";
            this.suspendedXOffsetBox.Size = new System.Drawing.Size(106, 23);
            this.suspendedXOffsetBox.TabIndex = 49;
            this.suspendedXOffsetBox.TextChanged += new System.EventHandler(this.suspendedXOffset_TextChanged);
            this.suspendedXOffsetBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.suspendedXOffsetBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// suspendedYOffsetBox
            ///
            this.suspendedYOffsetBox = new Tibialyzer.EnterTextBox();
            this.suspendedYOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedYOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedYOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.suspendedYOffsetBox.Location = new System.Drawing.Point(431, 336);
            this.suspendedYOffsetBox.Name = "suspendedYOffsetBox";
            this.suspendedYOffsetBox.Size = new System.Drawing.Size(106, 23);
            this.suspendedYOffsetBox.TabIndex = 51;
            this.suspendedYOffsetBox.TextChanged += new System.EventHandler(this.suspendedYOffset_TextChanged);
            this.suspendedYOffsetBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.suspendedYOffsetBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// exitScriptOnShutdownBox
            ///
            this.exitScriptOnShutdownBox = new Tibialyzer.PrettyCheckBox();
            this.exitScriptOnShutdownBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.exitScriptOnShutdownBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitScriptOnShutdownBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.exitScriptOnShutdownBox.Location = new System.Drawing.Point(333, 248);
            this.exitScriptOnShutdownBox.Name = "exitScriptOnShutdownBox";
            this.exitScriptOnShutdownBox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.exitScriptOnShutdownBox.Size = new System.Drawing.Size(204, 34);
            this.exitScriptOnShutdownBox.TabIndex = 58;
            this.exitScriptOnShutdownBox.Text = "Exit Script On Exit";
            this.exitScriptOnShutdownBox.UseVisualStyleBackColor = false;
            this.exitScriptOnShutdownBox.CheckedChanged += new System.EventHandler(this.shutdownOnExit_CheckedChanged);
            this.exitScriptOnShutdownBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.exitScriptOnShutdownBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// startScriptOnStartupBox
            ///
            this.startScriptOnStartupBox = new Tibialyzer.PrettyCheckBox();
            this.startScriptOnStartupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.startScriptOnStartupBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startScriptOnStartupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.startScriptOnStartupBox.Location = new System.Drawing.Point(333, 214);
            this.startScriptOnStartupBox.Name = "startScriptOnStartupBox";
            this.startScriptOnStartupBox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.startScriptOnStartupBox.Size = new System.Drawing.Size(204, 34);
            this.startScriptOnStartupBox.TabIndex = 57;
            this.startScriptOnStartupBox.Text = "Start Script On StartUp";
            this.startScriptOnStartupBox.UseVisualStyleBackColor = false;
            this.startScriptOnStartupBox.CheckedChanged += new System.EventHandler(this.startAutohotkeyScript_CheckedChanged);
            this.startScriptOnStartupBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.startScriptOnStartupBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// suspendedAnchorBox
            ///
            this.suspendedAnchorBox = new Tibialyzer.PrettyDropDownList();
            this.suspendedAnchorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.suspendedAnchorBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.suspendedAnchorBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suspendedAnchorBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.suspendedAnchorBox.FormattingEnabled = true;
            this.suspendedAnchorBox.Items.AddRange(new object[] {
            "Top Left",
            "Top Right",
            "Bottom Left",
            "Bottom Right"});
            this.suspendedAnchorBox.Location = new System.Drawing.Point(333, 390);
            this.suspendedAnchorBox.Name = "suspendedAnchorBox";
            this.suspendedAnchorBox.Size = new System.Drawing.Size(204, 24);
            this.suspendedAnchorBox.TabIndex = 46;
            this.suspendedAnchorBox.SelectedIndexChanged += new System.EventHandler(this.suspendedAnchor_SelectedIndexChanged);
            this.suspendedAnchorBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.suspendedAnchorBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// autoHotkeyGridSettings
            ///
            this.autoHotkeyGridSettings = new Tibialyzer.RichTextBoxAutoHotkey();
            this.autoHotkeyGridSettings.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyGridSettings.Location = new System.Drawing.Point(3, 54);
            this.autoHotkeyGridSettings.Name = "autoHotkeyGridSettings";
            this.autoHotkeyGridSettings.Size = new System.Drawing.Size(324, 440);
            this.autoHotkeyGridSettings.TabIndex = 5;
            this.autoHotkeyGridSettings.Text = "";
            this.autoHotkeyGridSettings.TextChanged += new System.EventHandler(this.autoHotkeyGridSettings_TextChanged);
            //
            // TabBase
            //
            this.Controls.Add(downloadBar);
            this.Controls.Add(closeSuspendedWindowButton);
            this.Controls.Add(suspendedTestButton);
            this.Controls.Add(label92);
            this.Controls.Add(shutdownAutoHotkeyButton);
            this.Controls.Add(startAutoHotkeyButton);
            this.Controls.Add(downloadAutoHotkeyButton);
            this.Controls.Add(label88);
            this.Controls.Add(label84);
            this.Controls.Add(label85);
            this.Controls.Add(label86);
            this.Controls.Add(label87);
            this.Controls.Add(autoHotkeyOptionsHeader);
            this.Controls.Add(autoHotkeyDownloadHeader);
            this.Controls.Add(autoHotkeyScriptHeader);
            this.Controls.Add(suspendedXOffsetBox);
            this.Controls.Add(suspendedYOffsetBox);
            this.Controls.Add(exitScriptOnShutdownBox);
            this.Controls.Add(startScriptOnStartupBox);
            this.Controls.Add(suspendedAnchorBox);
            this.Controls.Add(autoHotkeyGridSettings);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabBase";
            this.Text = "TabBase";
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.ProgressBar downloadBar;
        private System.Windows.Forms.Label closeSuspendedWindowButton;
        private System.Windows.Forms.Label suspendedTestButton;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label shutdownAutoHotkeyButton;
        private System.Windows.Forms.Label startAutoHotkeyButton;
        private System.Windows.Forms.Label downloadAutoHotkeyButton;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label autoHotkeyOptionsHeader;
        private System.Windows.Forms.Label autoHotkeyDownloadHeader;
        private System.Windows.Forms.Label autoHotkeyScriptHeader;
        private EnterTextBox suspendedXOffsetBox;
        private EnterTextBox suspendedYOffsetBox;
        private PrettyCheckBox exitScriptOnShutdownBox;
        private PrettyCheckBox startScriptOnStartupBox;
        private PrettyDropDownList suspendedAnchorBox;
        private RichTextBoxAutoHotkey autoHotkeyGridSettings;
        #endregion
    }
}