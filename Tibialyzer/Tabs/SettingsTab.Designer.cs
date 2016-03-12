namespace Tibialyzer {
    partial class SettingsTab {
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
            this.unlockLabel = new System.Windows.Forms.Label();
            this.unlockResetButton = new System.Windows.Forms.Label();
            this.resetSettingsButton = new System.Windows.Forms.Label();
            this.resetSettingsLabel = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.settingsOptionsHeader = new System.Windows.Forms.Label();
            this.scanSpeedDisplayLabel = new System.Windows.Forms.Label();
            this.scanSpeedLabel = new System.Windows.Forms.Label();
            this.scanningSpeedTrack = new System.Windows.Forms.TrackBar();
            this.label68 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.experienceComputationDropdown = new Tibialyzer.PrettyDropDownList();
            this.lookModeCheckbox = new Tibialyzer.PrettyCheckBox();
            this.outfitGenderCheckbox = new Tibialyzer.PrettyDropDownList();
            this.copyAdvancesCheckbox = new Tibialyzer.PrettyCheckBox();
            this.popupAnimationBox = new Tibialyzer.PrettyCheckBox();
            this.eventPopupBox = new Tibialyzer.PrettyCheckBox();
            this.unrecognizedPopupBox = new Tibialyzer.PrettyCheckBox();
            this.popupTypeBox = new Tibialyzer.PrettyDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.scanningSpeedTrack)).BeginInit();
            this.SuspendLayout();
            //
            // unlockLabel
            //
            this.unlockLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.unlockLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unlockLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.unlockLabel.Location = new System.Drawing.Point(145, 439);
            this.unlockLabel.Name = "unlockLabel";
            this.unlockLabel.Size = new System.Drawing.Size(250, 30);
            this.unlockLabel.TabIndex = 34;
            this.unlockLabel.Text = "Unlock ";
            this.unlockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // unlockResetButton
            //
            this.unlockResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.unlockResetButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unlockResetButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.unlockResetButton.Location = new System.Drawing.Point(145, 469);
            this.unlockResetButton.Name = "unlockResetButton";
            this.unlockResetButton.Padding = new System.Windows.Forms.Padding(10);
            this.unlockResetButton.Size = new System.Drawing.Size(250, 38);
            this.unlockResetButton.TabIndex = 33;
            this.unlockResetButton.Text = "Unlock Reset Button";
            this.unlockResetButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.unlockResetButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.unlockResetButton_Click);
            this.unlockResetButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.unlockResetButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // resetSettingsButton
            //
            this.resetSettingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.resetSettingsButton.Enabled = false;
            this.resetSettingsButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetSettingsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.resetSettingsButton.Location = new System.Drawing.Point(6, 388);
            this.resetSettingsButton.Name = "resetSettingsButton";
            this.resetSettingsButton.Padding = new System.Windows.Forms.Padding(10);
            this.resetSettingsButton.Size = new System.Drawing.Size(528, 38);
            this.resetSettingsButton.TabIndex = 32;
            this.resetSettingsButton.Text = "(Locked)";
            this.resetSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.resetSettingsButton.Click += new System.EventHandler(this.resetToDefaultButton_Click);
            this.resetSettingsButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.resetSettingsButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // resetSettingsLabel
            //
            this.resetSettingsLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.resetSettingsLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetSettingsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.resetSettingsLabel.Location = new System.Drawing.Point(6, 358);
            this.resetSettingsLabel.Name = "resetSettingsLabel";
            this.resetSettingsLabel.Size = new System.Drawing.Size(528, 30);
            this.resetSettingsLabel.TabIndex = 31;
            this.resetSettingsLabel.Text = "Reset Settings To Default";
            this.resetSettingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // label29
            //
            this.label29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label29.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label29.Location = new System.Drawing.Point(3, 132);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(240, 30);
            this.label29.TabIndex = 30;
            this.label29.Text = "Default Outfit Gender";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // label67
            //
            this.label67.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label67.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label67.Location = new System.Drawing.Point(245, 75);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(289, 30);
            this.label67.TabIndex = 48;
            this.label67.Text = "Popup Options";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // settingsOptionsHeader
            //
            this.settingsOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.settingsOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.settingsOptionsHeader.Location = new System.Drawing.Point(3, 19);
            this.settingsOptionsHeader.Name = "settingsOptionsHeader";
            this.settingsOptionsHeader.Size = new System.Drawing.Size(240, 30);
            this.settingsOptionsHeader.TabIndex = 29;
            this.settingsOptionsHeader.Text = "Options";
            this.settingsOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // scanSpeedDisplayLabel
            //
            this.scanSpeedDisplayLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.scanSpeedDisplayLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanSpeedDisplayLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.scanSpeedDisplayLabel.Location = new System.Drawing.Point(6, 323);
            this.scanSpeedDisplayLabel.Name = "scanSpeedDisplayLabel";
            this.scanSpeedDisplayLabel.Size = new System.Drawing.Size(528, 30);
            this.scanSpeedDisplayLabel.TabIndex = 25;
            this.scanSpeedDisplayLabel.Text = "Fastest";
            this.scanSpeedDisplayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // scanSpeedLabel
            //
            this.scanSpeedLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.scanSpeedLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanSpeedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.scanSpeedLabel.Location = new System.Drawing.Point(6, 248);
            this.scanSpeedLabel.Name = "scanSpeedLabel";
            this.scanSpeedLabel.Size = new System.Drawing.Size(528, 30);
            this.scanSpeedLabel.TabIndex = 24;
            this.scanSpeedLabel.Text = "Scanning Speed";
            this.scanSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // scanningSpeedTrack
            //
            this.scanningSpeedTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.scanningSpeedTrack.Location = new System.Drawing.Point(6, 278);
            this.scanningSpeedTrack.Maximum = 100;
            this.scanningSpeedTrack.Name = "scanningSpeedTrack";
            this.scanningSpeedTrack.Size = new System.Drawing.Size(528, 45);
            this.scanningSpeedTrack.TabIndex = 2;
            this.scanningSpeedTrack.Scroll += new System.EventHandler(this.scanningSpeedTrack_Scroll);
            this.scanningSpeedTrack.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.scanningSpeedTrack.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // label68
            //
            this.label68.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label68.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label68.Location = new System.Drawing.Point(245, 19);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(289, 30);
            this.label68.TabIndex = 52;
            this.label68.Text = "Popup Type";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // label1
            //
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label1.Location = new System.Drawing.Point(2, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 30);
            this.label1.TabIndex = 55;
            this.label1.Text = "Experience Computation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // experienceComputationDropdown
            //
            this.experienceComputationDropdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.experienceComputationDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.experienceComputationDropdown.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.experienceComputationDropdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.experienceComputationDropdown.FormattingEnabled = true;
            this.experienceComputationDropdown.Items.AddRange(new object[] {
            "Standard (Tibia Client Style)",
            "Weighted"});
            this.experienceComputationDropdown.Location = new System.Drawing.Point(2, 216);
            this.experienceComputationDropdown.Name = "experienceComputationDropdown";
            this.experienceComputationDropdown.Size = new System.Drawing.Size(240, 24);
            this.experienceComputationDropdown.TabIndex = 54;
            this.experienceComputationDropdown.SelectedIndexChanged += new System.EventHandler(this.experienceComputationDropdown_SelectedIndexChanged);
            this.experienceComputationDropdown.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.experienceComputationDropdown.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // lookModeCheckbox
            //
            this.lookModeCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.lookModeCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookModeCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.lookModeCheckbox.Location = new System.Drawing.Point(3, 49);
            this.lookModeCheckbox.Name = "lookModeCheckbox";
            this.lookModeCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.lookModeCheckbox.Size = new System.Drawing.Size(240, 40);
            this.lookModeCheckbox.TabIndex = 23;
            this.lookModeCheckbox.Text = "View Looked At Objects";
            this.lookModeCheckbox.UseVisualStyleBackColor = false;
            this.lookModeCheckbox.CheckedChanged += new System.EventHandler(this.lookCheckBox_CheckedChanged);
            this.lookModeCheckbox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.lookModeCheckbox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // outfitGenderCheckbox
            //
            this.outfitGenderCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.outfitGenderCheckbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outfitGenderCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outfitGenderCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.outfitGenderCheckbox.FormattingEnabled = true;
            this.outfitGenderCheckbox.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.outfitGenderCheckbox.Location = new System.Drawing.Point(3, 162);
            this.outfitGenderCheckbox.Name = "outfitGenderCheckbox";
            this.outfitGenderCheckbox.Size = new System.Drawing.Size(240, 24);
            this.outfitGenderCheckbox.TabIndex = 22;
            this.outfitGenderCheckbox.SelectedIndexChanged += new System.EventHandler(this.outfitGenderBox_SelectedIndexChanged);
            this.outfitGenderCheckbox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.outfitGenderCheckbox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // copyAdvancesCheckbox
            //
            this.copyAdvancesCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.copyAdvancesCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyAdvancesCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.copyAdvancesCheckbox.Location = new System.Drawing.Point(3, 89);
            this.copyAdvancesCheckbox.Name = "copyAdvancesCheckbox";
            this.copyAdvancesCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.copyAdvancesCheckbox.Size = new System.Drawing.Size(240, 40);
            this.copyAdvancesCheckbox.TabIndex = 20;
            this.copyAdvancesCheckbox.Text = "Copy Advances To Clipboard";
            this.copyAdvancesCheckbox.UseVisualStyleBackColor = false;
            this.copyAdvancesCheckbox.CheckedChanged += new System.EventHandler(this.advanceCopyCheckbox_CheckedChanged);
            this.copyAdvancesCheckbox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.copyAdvancesCheckbox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // popupAnimationBox
            //
            this.popupAnimationBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.popupAnimationBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupAnimationBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.popupAnimationBox.Location = new System.Drawing.Point(245, 184);
            this.popupAnimationBox.Name = "popupAnimationBox";
            this.popupAnimationBox.Padding = new System.Windows.Forms.Padding(10);
            this.popupAnimationBox.Size = new System.Drawing.Size(289, 40);
            this.popupAnimationBox.TabIndex = 51;
            this.popupAnimationBox.Text = "Enable Popup Animations";
            this.popupAnimationBox.UseVisualStyleBackColor = false;
            this.popupAnimationBox.CheckedChanged += new System.EventHandler(this.enableSimpleNotificationAnimations_CheckedChanged);
            this.popupAnimationBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.popupAnimationBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // eventPopupBox
            //
            this.eventPopupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.eventPopupBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventPopupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.eventPopupBox.Location = new System.Drawing.Point(245, 105);
            this.eventPopupBox.Name = "eventPopupBox";
            this.eventPopupBox.Padding = new System.Windows.Forms.Padding(10);
            this.eventPopupBox.Size = new System.Drawing.Size(289, 40);
            this.eventPopupBox.TabIndex = 49;
            this.eventPopupBox.Text = "Popup on Event";
            this.eventPopupBox.UseVisualStyleBackColor = false;
            this.eventPopupBox.CheckedChanged += new System.EventHandler(this.eventNotificationEnable_CheckedChanged);
            this.eventPopupBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.eventPopupBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // unrecognizedPopupBox
            //
            this.unrecognizedPopupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.unrecognizedPopupBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unrecognizedPopupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.unrecognizedPopupBox.Location = new System.Drawing.Point(245, 144);
            this.unrecognizedPopupBox.Name = "unrecognizedPopupBox";
            this.unrecognizedPopupBox.Padding = new System.Windows.Forms.Padding(10);
            this.unrecognizedPopupBox.Size = new System.Drawing.Size(289, 40);
            this.unrecognizedPopupBox.TabIndex = 50;
            this.unrecognizedPopupBox.Text = "Popup on Unrecognized Command";
            this.unrecognizedPopupBox.UseVisualStyleBackColor = false;
            this.unrecognizedPopupBox.CheckedChanged += new System.EventHandler(this.unrecognizedCommandNotification_CheckedChanged);
            this.unrecognizedPopupBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.unrecognizedPopupBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // popupTypeBox
            //
            this.popupTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.popupTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.popupTypeBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupTypeBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.popupTypeBox.FormattingEnabled = true;
            this.popupTypeBox.Items.AddRange(new object[] {
            "System Popup",
            "Rich Popup"});
            this.popupTypeBox.Location = new System.Drawing.Point(245, 48);
            this.popupTypeBox.Name = "popupTypeBox";
            this.popupTypeBox.Size = new System.Drawing.Size(289, 24);
            this.popupTypeBox.TabIndex = 53;
            this.popupTypeBox.SelectedIndexChanged += new System.EventHandler(this.notificationTypeBox_SelectedIndexChanged);
            this.popupTypeBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.popupTypeBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            //
            // SettingsTab
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.experienceComputationDropdown);
            this.Controls.Add(this.unlockLabel);
            this.Controls.Add(this.unlockResetButton);
            this.Controls.Add(this.resetSettingsButton);
            this.Controls.Add(this.resetSettingsLabel);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label67);
            this.Controls.Add(this.settingsOptionsHeader);
            this.Controls.Add(this.scanSpeedDisplayLabel);
            this.Controls.Add(this.scanSpeedLabel);
            this.Controls.Add(this.scanningSpeedTrack);
            this.Controls.Add(this.label68);
            this.Controls.Add(this.lookModeCheckbox);
            this.Controls.Add(this.outfitGenderCheckbox);
            this.Controls.Add(this.copyAdvancesCheckbox);
            this.Controls.Add(this.popupAnimationBox);
            this.Controls.Add(this.eventPopupBox);
            this.Controls.Add(this.unrecognizedPopupBox);
            this.Controls.Add(this.popupTypeBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.scanningSpeedTrack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label unlockLabel;
        private System.Windows.Forms.Label unlockResetButton;
        private System.Windows.Forms.Label resetSettingsButton;
        private System.Windows.Forms.Label resetSettingsLabel;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label settingsOptionsHeader;
        private System.Windows.Forms.Label scanSpeedDisplayLabel;
        private System.Windows.Forms.Label scanSpeedLabel;
        private System.Windows.Forms.TrackBar scanningSpeedTrack;
        private System.Windows.Forms.Label label68;
        private PrettyCheckBox lookModeCheckbox;
        private PrettyDropDownList outfitGenderCheckbox;
        private PrettyCheckBox copyAdvancesCheckbox;
        private PrettyCheckBox popupAnimationBox;
        private PrettyCheckBox eventPopupBox;
        private PrettyCheckBox unrecognizedPopupBox;
        private PrettyDropDownList popupTypeBox;
        #endregion

        private System.Windows.Forms.Label label1;
        private PrettyDropDownList experienceComputationDropdown;
    }
}