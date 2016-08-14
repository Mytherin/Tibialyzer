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
            this.unlockResetButtonHeader = new PrettyHeader();
            this.unlockResetButton = new PrettyButton();
            this.resetSettingsButton = new PrettyButton();
            this.resetSettingsToDefaultHeader = new PrettyHeader();
            this.defaultOutfitGenderHeader = new PrettyHeader();
            this.popupOptionsHeader = new PrettyHeader();
            this.optionsHeader = new PrettyHeader();
            this.scanningSpeedDisplayHeader = new PrettyHeader();
            this.scanningSpeedHeader = new PrettyHeader();
            this.scanningSpeedTrack = new System.Windows.Forms.TrackBar();
            this.experienceComputationHeader = new PrettyHeader();
            this.scanningOptionsHeader = new PrettyHeader();
            this.skipDuplicateLootCheckbox = new Tibialyzer.PrettyCheckBox();
            this.experienceComputationDropDownList = new Tibialyzer.PrettyDropDownList();
            this.viewLookedAtObjectsCheckbox = new Tibialyzer.PrettyCheckBox();
            this.defaultOutfitGenderDropDownList = new Tibialyzer.PrettyDropDownList();
            this.copyAdvancesCheckbox = new Tibialyzer.PrettyCheckBox();
            this.enablePopupAnimationsCheckbox = new Tibialyzer.PrettyCheckBox();
            this.popupOnEventCheckbox = new Tibialyzer.PrettyCheckBox();
            this.unrecognizedPopupCheckbox = new Tibialyzer.PrettyCheckBox();
            this.skipDuplicateCommandsCheckbox = new Tibialyzer.PrettyCheckBox();
            this.extraPlayerLookInformationCheckbox = new Tibialyzer.PrettyCheckBox();
            this.memoryScanSettingsHeader = new PrettyHeader();
            this.scanInternalTabsCheckbox = new Tibialyzer.PrettyCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.scanningSpeedTrack)).BeginInit();
            this.SuspendLayout();
            // 
            // unlockResetButtonHeader
            // 
            this.unlockResetButtonHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.unlockResetButtonHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unlockResetButtonHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.unlockResetButtonHeader.Location = new System.Drawing.Point(9, 446);
            this.unlockResetButtonHeader.Name = "unlockResetButtonHeader";
            this.unlockResetButtonHeader.Size = new System.Drawing.Size(299, 30);
            this.unlockResetButtonHeader.TabIndex = 34;
            this.unlockResetButtonHeader.Text = "Unlock ";
            this.unlockResetButtonHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // unlockResetButton
            // 
            this.unlockResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.unlockResetButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unlockResetButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.unlockResetButton.Location = new System.Drawing.Point(9, 476);
            this.unlockResetButton.Name = "unlockResetButton";
            this.unlockResetButton.Padding = new System.Windows.Forms.Padding(10);
            this.unlockResetButton.Size = new System.Drawing.Size(299, 38);
            this.unlockResetButton.TabIndex = 33;
            this.unlockResetButton.Text = "Unlock Reset Button";
            this.unlockResetButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.unlockResetButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.unlockResetButton_Click);
            // 
            // resetSettingsButton
            // 
            this.resetSettingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.resetSettingsButton.Enabled = false;
            this.resetSettingsButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetSettingsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.resetSettingsButton.Location = new System.Drawing.Point(330, 446);
            this.resetSettingsButton.Name = "resetSettingsButton";
            this.resetSettingsButton.Padding = new System.Windows.Forms.Padding(10);
            this.resetSettingsButton.Size = new System.Drawing.Size(296, 68);
            this.resetSettingsButton.TabIndex = 32;
            this.resetSettingsButton.Text = "(Locked)";
            this.resetSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.resetSettingsButton.Click += new System.EventHandler(this.resetToDefaultButton_Click);
            // 
            // resetSettingsToDefaultHeader
            // 
            this.resetSettingsToDefaultHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.resetSettingsToDefaultHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetSettingsToDefaultHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.resetSettingsToDefaultHeader.Location = new System.Drawing.Point(9, 412);
            this.resetSettingsToDefaultHeader.Name = "resetSettingsToDefaultHeader";
            this.resetSettingsToDefaultHeader.Size = new System.Drawing.Size(617, 30);
            this.resetSettingsToDefaultHeader.TabIndex = 31;
            this.resetSettingsToDefaultHeader.Text = "Reset Settings To Default";
            this.resetSettingsToDefaultHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defaultOutfitGenderHeader
            // 
            this.defaultOutfitGenderHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.defaultOutfitGenderHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultOutfitGenderHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.defaultOutfitGenderHeader.Location = new System.Drawing.Point(9, 162);
            this.defaultOutfitGenderHeader.Name = "defaultOutfitGenderHeader";
            this.defaultOutfitGenderHeader.Size = new System.Drawing.Size(299, 30);
            this.defaultOutfitGenderHeader.TabIndex = 30;
            this.defaultOutfitGenderHeader.Text = "Default Outfit Gender";
            this.defaultOutfitGenderHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // popupOptionsHeader
            // 
            this.popupOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.popupOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.popupOptionsHeader.Location = new System.Drawing.Point(327, 9);
            this.popupOptionsHeader.Name = "popupOptionsHeader";
            this.popupOptionsHeader.Size = new System.Drawing.Size(299, 30);
            this.popupOptionsHeader.TabIndex = 48;
            this.popupOptionsHeader.Text = "Popup Options";
            this.popupOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // optionsHeader
            // 
            this.optionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.optionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.optionsHeader.Location = new System.Drawing.Point(9, 9);
            this.optionsHeader.Name = "optionsHeader";
            this.optionsHeader.Size = new System.Drawing.Size(299, 30);
            this.optionsHeader.TabIndex = 29;
            this.optionsHeader.Text = "Options";
            this.optionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scanningSpeedDisplayHeader
            // 
            this.scanningSpeedDisplayHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.scanningSpeedDisplayHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanningSpeedDisplayHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.scanningSpeedDisplayHeader.Location = new System.Drawing.Point(9, 326);
            this.scanningSpeedDisplayHeader.Name = "scanningSpeedDisplayHeader";
            this.scanningSpeedDisplayHeader.Size = new System.Drawing.Size(299, 30);
            this.scanningSpeedDisplayHeader.TabIndex = 25;
            this.scanningSpeedDisplayHeader.Text = "Fastest";
            this.scanningSpeedDisplayHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scanningSpeedHeader
            // 
            this.scanningSpeedHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.scanningSpeedHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanningSpeedHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.scanningSpeedHeader.Location = new System.Drawing.Point(9, 251);
            this.scanningSpeedHeader.Name = "scanningSpeedHeader";
            this.scanningSpeedHeader.Size = new System.Drawing.Size(299, 30);
            this.scanningSpeedHeader.TabIndex = 24;
            this.scanningSpeedHeader.Text = "Scanning Speed";
            this.scanningSpeedHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scanningSpeedTrack
            // 
            this.scanningSpeedTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.scanningSpeedTrack.Location = new System.Drawing.Point(9, 281);
            this.scanningSpeedTrack.Maximum = 100;
            this.scanningSpeedTrack.Name = "scanningSpeedTrack";
            this.scanningSpeedTrack.Size = new System.Drawing.Size(299, 45);
            this.scanningSpeedTrack.TabIndex = 2;
            this.scanningSpeedTrack.Scroll += new System.EventHandler(this.scanningSpeedTrack_Scroll);
            // 
            // experienceComputationHeader
            // 
            this.experienceComputationHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.experienceComputationHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.experienceComputationHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.experienceComputationHeader.Location = new System.Drawing.Point(327, 162);
            this.experienceComputationHeader.Name = "experienceComputationHeader";
            this.experienceComputationHeader.Size = new System.Drawing.Size(299, 30);
            this.experienceComputationHeader.TabIndex = 55;
            this.experienceComputationHeader.Text = "Experience Computation";
            this.experienceComputationHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scanningOptionsHeader
            // 
            this.scanningOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.scanningOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanningOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.scanningOptionsHeader.Location = new System.Drawing.Point(327, 253);
            this.scanningOptionsHeader.Name = "scanningOptionsHeader";
            this.scanningOptionsHeader.Size = new System.Drawing.Size(299, 30);
            this.scanningOptionsHeader.TabIndex = 56;
            this.scanningOptionsHeader.Text = "Scanning Options";
            this.scanningOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // skipDuplicateLootCheckbox
            // 
            this.skipDuplicateLootCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.skipDuplicateLootCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skipDuplicateLootCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.skipDuplicateLootCheckbox.Location = new System.Drawing.Point(327, 360);
            this.skipDuplicateLootCheckbox.Name = "skipDuplicateLootCheckbox";
            this.skipDuplicateLootCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.skipDuplicateLootCheckbox.Size = new System.Drawing.Size(299, 38);
            this.skipDuplicateLootCheckbox.TabIndex = 58;
            this.skipDuplicateLootCheckbox.Text = "Skip Duplicate Loot";
            this.skipDuplicateLootCheckbox.UseVisualStyleBackColor = false;
            this.skipDuplicateLootCheckbox.CheckedChanged += new System.EventHandler(this.skipDuplicateLootCheckbox_CheckedChanged);
            // 
            // experienceComputationDropDownList
            // 
            this.experienceComputationDropDownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.experienceComputationDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.experienceComputationDropDownList.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.experienceComputationDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.experienceComputationDropDownList.FormattingEnabled = true;
            this.experienceComputationDropDownList.Items.AddRange(new object[] {
            "Standard (Tibia Client Style)",
            "Weighted"});
            this.experienceComputationDropDownList.Location = new System.Drawing.Point(327, 192);
            this.experienceComputationDropDownList.Name = "experienceComputationDropDownList";
            this.experienceComputationDropDownList.Size = new System.Drawing.Size(299, 24);
            this.experienceComputationDropDownList.TabIndex = 54;
            this.experienceComputationDropDownList.SelectedIndexChanged += new System.EventHandler(this.experienceComputationDropdown_SelectedIndexChanged);
            // 
            // viewLookedAtObjectsCheckbox
            // 
            this.viewLookedAtObjectsCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.viewLookedAtObjectsCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewLookedAtObjectsCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.viewLookedAtObjectsCheckbox.Location = new System.Drawing.Point(9, 39);
            this.viewLookedAtObjectsCheckbox.Name = "viewLookedAtObjectsCheckbox";
            this.viewLookedAtObjectsCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.viewLookedAtObjectsCheckbox.Size = new System.Drawing.Size(299, 40);
            this.viewLookedAtObjectsCheckbox.TabIndex = 23;
            this.viewLookedAtObjectsCheckbox.Text = "View Looked At Objects";
            this.viewLookedAtObjectsCheckbox.UseVisualStyleBackColor = false;
            this.viewLookedAtObjectsCheckbox.CheckedChanged += new System.EventHandler(this.lookCheckBox_CheckedChanged);
            // 
            // defaultOutfitGenderDropDownList
            // 
            this.defaultOutfitGenderDropDownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.defaultOutfitGenderDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultOutfitGenderDropDownList.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultOutfitGenderDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.defaultOutfitGenderDropDownList.FormattingEnabled = true;
            this.defaultOutfitGenderDropDownList.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.defaultOutfitGenderDropDownList.Location = new System.Drawing.Point(9, 192);
            this.defaultOutfitGenderDropDownList.Name = "defaultOutfitGenderDropDownList";
            this.defaultOutfitGenderDropDownList.Size = new System.Drawing.Size(299, 24);
            this.defaultOutfitGenderDropDownList.TabIndex = 22;
            this.defaultOutfitGenderDropDownList.SelectedIndexChanged += new System.EventHandler(this.outfitGenderBox_SelectedIndexChanged);
            // 
            // copyAdvancesCheckbox
            // 
            this.copyAdvancesCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.copyAdvancesCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyAdvancesCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.copyAdvancesCheckbox.Location = new System.Drawing.Point(9, 79);
            this.copyAdvancesCheckbox.Name = "copyAdvancesCheckbox";
            this.copyAdvancesCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.copyAdvancesCheckbox.Size = new System.Drawing.Size(299, 40);
            this.copyAdvancesCheckbox.TabIndex = 20;
            this.copyAdvancesCheckbox.Text = "Copy Advances To Clipboard";
            this.copyAdvancesCheckbox.UseVisualStyleBackColor = false;
            this.copyAdvancesCheckbox.CheckedChanged += new System.EventHandler(this.advanceCopyCheckbox_CheckedChanged);
            // 
            // enablePopupAnimationsCheckbox
            // 
            this.enablePopupAnimationsCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.enablePopupAnimationsCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enablePopupAnimationsCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.enablePopupAnimationsCheckbox.Location = new System.Drawing.Point(327, 118);
            this.enablePopupAnimationsCheckbox.Name = "enablePopupAnimationsCheckbox";
            this.enablePopupAnimationsCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.enablePopupAnimationsCheckbox.Size = new System.Drawing.Size(299, 40);
            this.enablePopupAnimationsCheckbox.TabIndex = 51;
            this.enablePopupAnimationsCheckbox.Text = "Enable Popup Animations";
            this.enablePopupAnimationsCheckbox.UseVisualStyleBackColor = false;
            this.enablePopupAnimationsCheckbox.CheckedChanged += new System.EventHandler(this.enableSimpleNotificationAnimations_CheckedChanged);
            // 
            // popupOnEventCheckbox
            // 
            this.popupOnEventCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.popupOnEventCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupOnEventCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.popupOnEventCheckbox.Location = new System.Drawing.Point(327, 39);
            this.popupOnEventCheckbox.Name = "popupOnEventCheckbox";
            this.popupOnEventCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.popupOnEventCheckbox.Size = new System.Drawing.Size(299, 40);
            this.popupOnEventCheckbox.TabIndex = 49;
            this.popupOnEventCheckbox.Text = "Popup on Event";
            this.popupOnEventCheckbox.UseVisualStyleBackColor = false;
            this.popupOnEventCheckbox.CheckedChanged += new System.EventHandler(this.eventNotificationEnable_CheckedChanged);
            // 
            // unrecognizedPopupCheckbox
            // 
            this.unrecognizedPopupCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.unrecognizedPopupCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unrecognizedPopupCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.unrecognizedPopupCheckbox.Location = new System.Drawing.Point(327, 78);
            this.unrecognizedPopupCheckbox.Name = "unrecognizedPopupCheckbox";
            this.unrecognizedPopupCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.unrecognizedPopupCheckbox.Size = new System.Drawing.Size(299, 40);
            this.unrecognizedPopupCheckbox.TabIndex = 50;
            this.unrecognizedPopupCheckbox.Text = "Popup on Unrecognized Command";
            this.unrecognizedPopupCheckbox.UseVisualStyleBackColor = false;
            this.unrecognizedPopupCheckbox.CheckedChanged += new System.EventHandler(this.unrecognizedCommandNotification_CheckedChanged);
            // 
            // skipDuplicateCommandsCheckbox
            // 
            this.skipDuplicateCommandsCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.skipDuplicateCommandsCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skipDuplicateCommandsCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.skipDuplicateCommandsCheckbox.Location = new System.Drawing.Point(327, 322);
            this.skipDuplicateCommandsCheckbox.Name = "skipDuplicateCommandsCheckbox";
            this.skipDuplicateCommandsCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.skipDuplicateCommandsCheckbox.Size = new System.Drawing.Size(299, 38);
            this.skipDuplicateCommandsCheckbox.TabIndex = 59;
            this.skipDuplicateCommandsCheckbox.Text = "Skip Duplicate Commands";
            this.skipDuplicateCommandsCheckbox.UseVisualStyleBackColor = false;
            this.skipDuplicateCommandsCheckbox.CheckedChanged += new System.EventHandler(this.skipDuplicateCommandsCheckbox_CheckedChanged);
            // 
            // extraPlayerLookInformationCheckbox
            // 
            this.extraPlayerLookInformationCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.extraPlayerLookInformationCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.extraPlayerLookInformationCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.extraPlayerLookInformationCheckbox.Location = new System.Drawing.Point(9, 119);
            this.extraPlayerLookInformationCheckbox.Name = "extraPlayerLookInformationCheckbox";
            this.extraPlayerLookInformationCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.extraPlayerLookInformationCheckbox.Size = new System.Drawing.Size(299, 40);
            this.extraPlayerLookInformationCheckbox.TabIndex = 60;
            this.extraPlayerLookInformationCheckbox.Text = "Extra Player Look Information";
            this.extraPlayerLookInformationCheckbox.UseVisualStyleBackColor = false;
            this.extraPlayerLookInformationCheckbox.CheckedChanged += new System.EventHandler(this.gatherOnlineInformation_CheckedChanged);
            // 
            // memoryScanSettingsHeader
            // 
            this.memoryScanSettingsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.memoryScanSettingsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoryScanSettingsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.memoryScanSettingsHeader.Location = new System.Drawing.Point(9, 219);
            this.memoryScanSettingsHeader.Name = "memoryScanSettingsHeader";
            this.memoryScanSettingsHeader.Size = new System.Drawing.Size(617, 30);
            this.memoryScanSettingsHeader.TabIndex = 61;
            this.memoryScanSettingsHeader.Text = "Memory Scan Settings";
            this.memoryScanSettingsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scanInternalTabsCheckbox
            // 
            this.scanInternalTabsCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.scanInternalTabsCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanInternalTabsCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.scanInternalTabsCheckbox.Image = global::Tibialyzer.Properties.Resources.noflash;
            this.scanInternalTabsCheckbox.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.scanInternalTabsCheckbox.Location = new System.Drawing.Point(327, 283);
            this.scanInternalTabsCheckbox.Name = "scanInternalTabsCheckbox";
            this.scanInternalTabsCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.scanInternalTabsCheckbox.Size = new System.Drawing.Size(299, 40);
            this.scanInternalTabsCheckbox.TabIndex = 62;
            this.scanInternalTabsCheckbox.Text = "Scan Internal Tabs Structure";
            this.scanInternalTabsCheckbox.UseVisualStyleBackColor = false;
            this.scanInternalTabsCheckbox.CheckedChanged += new System.EventHandler(this.scanInternalTabsCheckbox_CheckedChanged);
            // 
            // SettingsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.scanInternalTabsCheckbox);
            this.Controls.Add(this.memoryScanSettingsHeader);
            this.Controls.Add(this.extraPlayerLookInformationCheckbox);
            this.Controls.Add(this.skipDuplicateCommandsCheckbox);
            this.Controls.Add(this.skipDuplicateLootCheckbox);
            this.Controls.Add(this.scanningOptionsHeader);
            this.Controls.Add(this.experienceComputationHeader);
            this.Controls.Add(this.experienceComputationDropDownList);
            this.Controls.Add(this.unlockResetButtonHeader);
            this.Controls.Add(this.unlockResetButton);
            this.Controls.Add(this.resetSettingsButton);
            this.Controls.Add(this.resetSettingsToDefaultHeader);
            this.Controls.Add(this.defaultOutfitGenderHeader);
            this.Controls.Add(this.popupOptionsHeader);
            this.Controls.Add(this.optionsHeader);
            this.Controls.Add(this.scanningSpeedDisplayHeader);
            this.Controls.Add(this.scanningSpeedHeader);
            this.Controls.Add(this.scanningSpeedTrack);
            this.Controls.Add(this.viewLookedAtObjectsCheckbox);
            this.Controls.Add(this.defaultOutfitGenderDropDownList);
            this.Controls.Add(this.copyAdvancesCheckbox);
            this.Controls.Add(this.enablePopupAnimationsCheckbox);
            this.Controls.Add(this.popupOnEventCheckbox);
            this.Controls.Add(this.unrecognizedPopupCheckbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.scanningSpeedTrack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label unlockResetButtonHeader;
        private System.Windows.Forms.Label unlockResetButton;
        private System.Windows.Forms.Label resetSettingsButton;
        private System.Windows.Forms.Label resetSettingsToDefaultHeader;
        private System.Windows.Forms.Label defaultOutfitGenderHeader;
        private System.Windows.Forms.Label popupOptionsHeader;
        private System.Windows.Forms.Label optionsHeader;
        private System.Windows.Forms.Label scanningSpeedDisplayHeader;
        private System.Windows.Forms.Label scanningSpeedHeader;
        private System.Windows.Forms.TrackBar scanningSpeedTrack;
        private PrettyCheckBox viewLookedAtObjectsCheckbox;
        private PrettyDropDownList defaultOutfitGenderDropDownList;
        private PrettyCheckBox copyAdvancesCheckbox;
        private PrettyCheckBox enablePopupAnimationsCheckbox;
        private PrettyCheckBox popupOnEventCheckbox;
        private PrettyCheckBox unrecognizedPopupCheckbox;
        #endregion

        private System.Windows.Forms.Label experienceComputationHeader;
        private PrettyDropDownList experienceComputationDropDownList;
        private System.Windows.Forms.Label scanningOptionsHeader;
        private PrettyCheckBox skipDuplicateLootCheckbox;
        private PrettyCheckBox skipDuplicateCommandsCheckbox;
        private PrettyCheckBox extraPlayerLookInformationCheckbox;
        private System.Windows.Forms.Label memoryScanSettingsHeader;
        private PrettyCheckBox scanInternalTabsCheckbox;
    }
}