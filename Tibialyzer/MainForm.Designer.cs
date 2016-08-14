
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
﻿namespace Tibialyzer {
    partial class MainForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tibialyzerLogo = new System.Windows.Forms.PictureBox();
            this.closeButton = new System.Windows.Forms.Label();
            this.minimizeButton = new System.Windows.Forms.Label();
            this.minimizeIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.loadTimerImage = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.explanationTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundLabel = new System.Windows.Forms.Label();
            this.horizontalSeparator1 = new System.Windows.Forms.Label();
            this.horizontalSeparator4 = new System.Windows.Forms.Label();
            this.horizontalSeparator2 = new System.Windows.Forms.Label();
            this.programTopBar = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.warningImageBox = new System.Windows.Forms.PictureBox();
            this.horizontalSeparator3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.aboutButton = new Tibialyzer.PrettyMenuTab();
            this.advancedButton = new Tibialyzer.PrettyMenuTab();
            this.hudButton = new Tibialyzer.PrettyMenuTab();
            this.summaryButton = new Tibialyzer.PrettyMenuTab();
            this.upgradeButton = new Tibialyzer.PrettyMenuTab();
            this.logButton = new Tibialyzer.PrettyMenuTab();
            this.huntButton = new Tibialyzer.PrettyMenuTab();
            this.helpButton = new Tibialyzer.PrettyMenuTab();
            this.browseButton = new Tibialyzer.PrettyMenuTab();
            this.screenshotButton = new Tibialyzer.PrettyMenuTab();
            this.autoHotkeyButton = new Tibialyzer.PrettyMenuTab();
            this.databaseButton = new Tibialyzer.PrettyMenuTab();
            this.popupButton = new Tibialyzer.PrettyMenuTab();
            this.notificationButton = new Tibialyzer.PrettyMenuTab();
            this.generalButton = new Tibialyzer.PrettyMenuTab();
            this.mainButton = new Tibialyzer.PrettyMenuTab();
            ((System.ComponentModel.ISupportInitialize)(this.tibialyzerLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadTimerImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // tibialyzerLogo
            // 
            this.tibialyzerLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.tibialyzerLogo.Image = ((System.Drawing.Image)(resources.GetObject("tibialyzerLogo.Image")));
            this.tibialyzerLogo.InitialImage = null;
            this.tibialyzerLogo.Location = new System.Drawing.Point(0, 19);
            this.tibialyzerLogo.Name = "tibialyzerLogo";
            this.tibialyzerLogo.Size = new System.Drawing.Size(102, 39);
            this.tibialyzerLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.tibialyzerLogo.TabIndex = 18;
            this.tibialyzerLogo.TabStop = false;
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.closeButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.closeButton.Location = new System.Drawing.Point(717, 4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Padding = new System.Windows.Forms.Padding(8, 1, 8, 1);
            this.closeButton.Size = new System.Drawing.Size(32, 17);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "X";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            this.closeButton.MouseEnter += new System.EventHandler(this.closeButton_MouseEnter);
            this.closeButton.MouseLeave += new System.EventHandler(this.closeButton_MouseLeave);
            // 
            // minimizeButton
            // 
            this.minimizeButton.AutoSize = true;
            this.minimizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.minimizeButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minimizeButton.Location = new System.Drawing.Point(685, 4);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Padding = new System.Windows.Forms.Padding(8, 1, 8, 1);
            this.minimizeButton.Size = new System.Drawing.Size(28, 17);
            this.minimizeButton.TabIndex = 2;
            this.minimizeButton.Text = "-";
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            this.minimizeButton.MouseEnter += new System.EventHandler(this.minimizeButton_MouseEnter);
            this.minimizeButton.MouseLeave += new System.EventHandler(this.minimizeButton_MouseLeave);
            // 
            // minimizeIcon
            // 
            this.minimizeIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.minimizeIcon.BalloonTipText = "Tibialyzer is now minimized.";
            this.minimizeIcon.BalloonTipTitle = "Tibialyzer";
            this.minimizeIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("minimizeIcon.Icon")));
            this.minimizeIcon.Text = "Tibialyzer";
            this.minimizeIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.minimizeIcon_MouseDoubleClick);
            // 
            // loadTimerImage
            // 
            this.loadTimerImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.loadTimerImage.Enabled = false;
            this.loadTimerImage.Location = new System.Drawing.Point(0, 2);
            this.loadTimerImage.Name = "loadTimerImage";
            this.loadTimerImage.Size = new System.Drawing.Size(102, 20);
            this.loadTimerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadTimerImage.TabIndex = 3;
            this.loadTimerImage.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // explanationTooltip
            // 
            this.explanationTooltip.AutoPopDelay = 150000;
            this.explanationTooltip.InitialDelay = 500;
            this.explanationTooltip.ReshowDelay = 100;
            // 
            // backgroundLabel
            // 
            this.backgroundLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.backgroundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.backgroundLabel.Location = new System.Drawing.Point(0, 0);
            this.backgroundLabel.Name = "backgroundLabel";
            this.backgroundLabel.Size = new System.Drawing.Size(102, 577);
            this.backgroundLabel.TabIndex = 5;
            // 
            // horizontalSeparator1
            // 
            this.horizontalSeparator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.horizontalSeparator1.Location = new System.Drawing.Point(0, 92);
            this.horizontalSeparator1.Name = "horizontalSeparator1";
            this.horizontalSeparator1.Size = new System.Drawing.Size(102, 5);
            this.horizontalSeparator1.TabIndex = 7;
            this.horizontalSeparator1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // horizontalSeparator4
            // 
            this.horizontalSeparator4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.horizontalSeparator4.Location = new System.Drawing.Point(0, 311);
            this.horizontalSeparator4.Name = "horizontalSeparator4";
            this.horizontalSeparator4.Size = new System.Drawing.Size(102, 5);
            this.horizontalSeparator4.TabIndex = 15;
            this.horizontalSeparator4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // horizontalSeparator2
            // 
            this.horizontalSeparator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.horizontalSeparator2.Location = new System.Drawing.Point(0, 341);
            this.horizontalSeparator2.Name = "horizontalSeparator2";
            this.horizontalSeparator2.Size = new System.Drawing.Size(102, 5);
            this.horizontalSeparator2.TabIndex = 17;
            this.horizontalSeparator2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // programTopBar
            // 
            this.programTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.programTopBar.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.programTopBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.programTopBar.Location = new System.Drawing.Point(89, 0);
            this.programTopBar.Name = "programTopBar";
            this.programTopBar.Size = new System.Drawing.Size(666, 25);
            this.programTopBar.TabIndex = 20;
            this.programTopBar.Text = "Tibialyzer";
            this.programTopBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.label30.Location = new System.Drawing.Point(0, 62);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(102, 5);
            this.label30.TabIndex = 22;
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // warningImageBox
            // 
            this.warningImageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.warningImageBox.Image = ((System.Drawing.Image)(resources.GetObject("warningImageBox.Image")));
            this.warningImageBox.Location = new System.Drawing.Point(27, 520);
            this.warningImageBox.Name = "warningImageBox";
            this.warningImageBox.Size = new System.Drawing.Size(48, 48);
            this.warningImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.warningImageBox.TabIndex = 24;
            this.warningImageBox.TabStop = false;
            this.warningImageBox.Visible = false;
            this.warningImageBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.warningImageBox_MouseDown);
            // 
            // horizontalSeparator3
            // 
            this.horizontalSeparator3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.horizontalSeparator3.Location = new System.Drawing.Point(0, 371);
            this.horizontalSeparator3.Name = "horizontalSeparator3";
            this.horizontalSeparator3.Size = new System.Drawing.Size(102, 5);
            this.horizontalSeparator3.TabIndex = 25;
            this.horizontalSeparator3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.label1.Location = new System.Drawing.Point(-1, 462);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 5);
            this.label1.TabIndex = 29;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // aboutButton
            // 
            this.aboutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.aboutButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.aboutButton.Location = new System.Drawing.Point(2, 494);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(100, 25);
            this.aboutButton.TabIndex = 31;
            this.aboutButton.Text = "About";
            this.aboutButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // advancedButton
            // 
            this.advancedButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.advancedButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.advancedButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.advancedButton.Location = new System.Drawing.Point(2, 467);
            this.advancedButton.Name = "advancedButton";
            this.advancedButton.Size = new System.Drawing.Size(100, 25);
            this.advancedButton.TabIndex = 30;
            this.advancedButton.Text = "Advanced";
            this.advancedButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.advancedButton.Click += new System.EventHandler(this.advancedButton_Click);
            // 
            // hudButton
            // 
            this.hudButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.hudButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hudButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.hudButton.Image = global::Tibialyzer.Properties.Resources.noflash;
            this.hudButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.hudButton.Location = new System.Drawing.Point(2, 430);
            this.hudButton.Name = "hudButton";
            this.hudButton.Size = new System.Drawing.Size(100, 32);
            this.hudButton.TabIndex = 28;
            this.hudButton.Text = "HUD";
            this.hudButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.hudButton.Click += new System.EventHandler(this.hudButton_Click);
            // 
            // summaryButton
            // 
            this.summaryButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.summaryButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summaryButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.summaryButton.Location = new System.Drawing.Point(2, 403);
            this.summaryButton.Name = "summaryButton";
            this.summaryButton.Size = new System.Drawing.Size(100, 25);
            this.summaryButton.TabIndex = 27;
            this.summaryButton.Text = "Summary";
            this.summaryButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.summaryButton.Click += new System.EventHandler(this.summaryButton_Click);
            // 
            // upgradeButton
            // 
            this.upgradeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.upgradeButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upgradeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.upgradeButton.Location = new System.Drawing.Point(0, 376);
            this.upgradeButton.Name = "upgradeButton";
            this.upgradeButton.Size = new System.Drawing.Size(100, 25);
            this.upgradeButton.TabIndex = 26;
            this.upgradeButton.Text = "System";
            this.upgradeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.upgradeButton.Click += new System.EventHandler(this.upgradeButton_Click);
            // 
            // logButton
            // 
            this.logButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.logButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.logButton.Location = new System.Drawing.Point(1, 151);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(100, 25);
            this.logButton.TabIndex = 23;
            this.logButton.Text = "Logs";
            this.logButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.logButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.logButton_Click);
            // 
            // huntButton
            // 
            this.huntButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.huntButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.huntButton.Location = new System.Drawing.Point(1, 124);
            this.huntButton.Name = "huntButton";
            this.huntButton.Size = new System.Drawing.Size(100, 25);
            this.huntButton.TabIndex = 19;
            this.huntButton.Text = "Hunts";
            this.huntButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.huntButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.huntButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.helpButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.helpButton.Location = new System.Drawing.Point(0, 346);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(100, 25);
            this.helpButton.TabIndex = 18;
            this.helpButton.Text = "Help";
            this.helpButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.helpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.helpButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.browseButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.browseButton.Location = new System.Drawing.Point(0, 316);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(100, 25);
            this.browseButton.TabIndex = 16;
            this.browseButton.Text = "Browse";
            this.browseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.browseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.browseButton_Click);
            // 
            // screenshotButton
            // 
            this.screenshotButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.screenshotButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenshotButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.screenshotButton.Location = new System.Drawing.Point(1, 286);
            this.screenshotButton.Name = "screenshotButton";
            this.screenshotButton.Size = new System.Drawing.Size(100, 25);
            this.screenshotButton.TabIndex = 14;
            this.screenshotButton.Text = "Screenshots";
            this.screenshotButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.screenshotButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screenshotButton_Click);
            // 
            // autoHotkeyButton
            // 
            this.autoHotkeyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.autoHotkeyButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoHotkeyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.autoHotkeyButton.Location = new System.Drawing.Point(1, 259);
            this.autoHotkeyButton.Name = "autoHotkeyButton";
            this.autoHotkeyButton.Size = new System.Drawing.Size(100, 25);
            this.autoHotkeyButton.TabIndex = 13;
            this.autoHotkeyButton.Text = "AutoHotkey";
            this.autoHotkeyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.autoHotkeyButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.autoHotkeyButton_Click);
            // 
            // databaseButton
            // 
            this.databaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.databaseButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.databaseButton.Location = new System.Drawing.Point(1, 232);
            this.databaseButton.Name = "databaseButton";
            this.databaseButton.Size = new System.Drawing.Size(100, 25);
            this.databaseButton.TabIndex = 12;
            this.databaseButton.Text = "Database";
            this.databaseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.databaseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.databaseButton_Click);
            // 
            // popupButton
            // 
            this.popupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.popupButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.popupButton.Location = new System.Drawing.Point(1, 205);
            this.popupButton.Name = "popupButton";
            this.popupButton.Size = new System.Drawing.Size(100, 25);
            this.popupButton.TabIndex = 11;
            this.popupButton.Text = "Popups";
            this.popupButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.popupButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.popupButton_Click);
            // 
            // notificationButton
            // 
            this.notificationButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.notificationButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.notificationButton.Location = new System.Drawing.Point(0, 178);
            this.notificationButton.Name = "notificationButton";
            this.notificationButton.Size = new System.Drawing.Size(100, 25);
            this.notificationButton.TabIndex = 10;
            this.notificationButton.Text = "Notifications";
            this.notificationButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.notificationButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.notificationButton_Click);
            // 
            // generalButton
            // 
            this.generalButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.generalButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generalButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.generalButton.Location = new System.Drawing.Point(0, 97);
            this.generalButton.Name = "generalButton";
            this.generalButton.Size = new System.Drawing.Size(100, 25);
            this.generalButton.TabIndex = 9;
            this.generalButton.Text = "Settings";
            this.generalButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.generalButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.generalButton_Click);
            // 
            // mainButton
            // 
            this.mainButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.mainButton.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.mainButton.Location = new System.Drawing.Point(0, 67);
            this.mainButton.Name = "mainButton";
            this.mainButton.Size = new System.Drawing.Size(100, 25);
            this.mainButton.TabIndex = 4;
            this.mainButton.Text = "Main";
            this.mainButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(755, 575);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.advancedButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hudButton);
            this.Controls.Add(this.summaryButton);
            this.Controls.Add(this.upgradeButton);
            this.Controls.Add(this.horizontalSeparator3);
            this.Controls.Add(this.warningImageBox);
            this.Controls.Add(this.logButton);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.loadTimerImage);
            this.Controls.Add(this.programTopBar);
            this.Controls.Add(this.huntButton);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.tibialyzerLogo);
            this.Controls.Add(this.horizontalSeparator2);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.horizontalSeparator4);
            this.Controls.Add(this.screenshotButton);
            this.Controls.Add(this.autoHotkeyButton);
            this.Controls.Add(this.databaseButton);
            this.Controls.Add(this.popupButton);
            this.Controls.Add(this.notificationButton);
            this.Controls.Add(this.generalButton);
            this.Controls.Add(this.horizontalSeparator1);
            this.Controls.Add(this.mainButton);
            this.Controls.Add(this.backgroundLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Tibialyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.tibialyzerLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadTimerImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.NotifyIcon minimizeIcon;
        private System.Windows.Forms.PictureBox loadTimerImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolTip explanationTooltip;
        private System.Windows.Forms.PictureBox tibialyzerLogo;
        private System.Windows.Forms.Label backgroundLabel;
        private System.Windows.Forms.Label horizontalSeparator1;
        private System.Windows.Forms.Label horizontalSeparator4;
        private System.Windows.Forms.Label horizontalSeparator2;
        private System.Windows.Forms.Label programTopBar;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.PictureBox warningImageBox;
        private System.Windows.Forms.Label horizontalSeparator3;
        private System.Windows.Forms.Label label1;
        private PrettyMenuTab huntButton;
        private PrettyMenuTab helpButton;
        private PrettyMenuTab browseButton;
        private PrettyMenuTab screenshotButton;
        private PrettyMenuTab autoHotkeyButton;
        private PrettyMenuTab databaseButton;
        private PrettyMenuTab popupButton;
        private PrettyMenuTab notificationButton;
        private PrettyMenuTab generalButton;
        private PrettyMenuTab mainButton;
        private System.Windows.Forms.Label minimizeButton;
        private System.Windows.Forms.Label closeButton;
        private PrettyMenuTab aboutButton;
        private PrettyMenuTab advancedButton;
        private PrettyMenuTab summaryButton;
        private PrettyMenuTab upgradeButton;
        private PrettyMenuTab hudButton;
        private PrettyMenuTab logButton;
    }
}

