namespace Tibialyzer {
    partial class SystemTab {
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
            this.customCommandParameterBox = new Tibialyzer.EnterTextBox();
            this.parametersHeader = new Tibialyzer.PrettyHeader();
            this.customCommandBox = new Tibialyzer.EnterTextBox();
            this.customCommandName = new System.Windows.Forms.Label();
            this.systemCommandHeader = new Tibialyzer.PrettyHeader();
            this.customCommandsHeader = new Tibialyzer.PrettyHeader();
            this.customCommandList = new Tibialyzer.PrettyListBox();
            this.selectUpgradeTibialyzerButton = new Tibialyzer.PrettyButton();
            this.importSettingsHeader = new Tibialyzer.PrettyHeader();
            this.obsSettingsHeader = new Tibialyzer.PrettyHeader();
            this.showPopupWindowButton = new Tibialyzer.PrettyButton();
            this.enableWindowCaptureCheckbox = new Tibialyzer.PrettyCheckBox();
            this.createBackupButton = new Tibialyzer.PrettyButton();
            this.backupSettingsHeader = new Tibialyzer.PrettyHeader();
            this.restoreBackupButton = new Tibialyzer.PrettyButton();
            this.automaticallyBackupSettingsCheckbox = new Tibialyzer.PrettyCheckBox();
            this.saveImagesHeader = new Tibialyzer.PrettyHeader();
            this.saveLootImageButton = new Tibialyzer.PrettyButton();
            this.saveDamageImageButton = new Tibialyzer.PrettyButton();
            this.saveSummaryImageButton = new Tibialyzer.PrettyButton();
            this.maxDamagePlayersHeader = new Tibialyzer.PrettyHeader();
            this.hudAnchorDropDownList = new Tibialyzer.PrettyDropDownList();
            this.SuspendLayout();
            // 
            // customCommandParameterBox
            // 
            this.customCommandParameterBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandParameterBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandParameterBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.customCommandParameterBox.Location = new System.Drawing.Point(327, 198);
            this.customCommandParameterBox.Name = "customCommandParameterBox";
            this.customCommandParameterBox.Size = new System.Drawing.Size(299, 23);
            this.customCommandParameterBox.TabIndex = 63;
            this.customCommandParameterBox.TextChanged += new System.EventHandler(this.customCommandParameterBox_TextChanged);
            // 
            // parametersHeader
            // 
            this.parametersHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.parametersHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parametersHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.parametersHeader.Location = new System.Drawing.Point(327, 168);
            this.parametersHeader.Name = "parametersHeader";
            this.parametersHeader.Size = new System.Drawing.Size(299, 30);
            this.parametersHeader.TabIndex = 62;
            this.parametersHeader.Text = "Parameters";
            this.parametersHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customCommandBox
            // 
            this.customCommandBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.customCommandBox.Location = new System.Drawing.Point(327, 145);
            this.customCommandBox.Name = "customCommandBox";
            this.customCommandBox.Size = new System.Drawing.Size(299, 23);
            this.customCommandBox.TabIndex = 61;
            this.customCommandBox.TextChanged += new System.EventHandler(this.customCommandBox_TextChanged);
            // 
            // customCommandName
            // 
            this.customCommandName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(128)))), ((int)(((byte)(176)))));
            this.customCommandName.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandName.Location = new System.Drawing.Point(327, 84);
            this.customCommandName.Name = "customCommandName";
            this.customCommandName.Size = new System.Drawing.Size(299, 30);
            this.customCommandName.TabIndex = 60;
            this.customCommandName.Text = "Command Information";
            this.customCommandName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // systemCommandHeader
            // 
            this.systemCommandHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.systemCommandHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.systemCommandHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.systemCommandHeader.Location = new System.Drawing.Point(327, 115);
            this.systemCommandHeader.Name = "systemCommandHeader";
            this.systemCommandHeader.Size = new System.Drawing.Size(299, 30);
            this.systemCommandHeader.TabIndex = 59;
            this.systemCommandHeader.Text = "System Command";
            this.systemCommandHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customCommandsHeader
            // 
            this.customCommandsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.customCommandsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customCommandsHeader.Location = new System.Drawing.Point(9, 84);
            this.customCommandsHeader.Name = "customCommandsHeader";
            this.customCommandsHeader.Size = new System.Drawing.Size(299, 30);
            this.customCommandsHeader.TabIndex = 58;
            this.customCommandsHeader.Text = "Custom Commands";
            this.customCommandsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customCommandList
            // 
            this.customCommandList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customCommandList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.customCommandList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandList.FormattingEnabled = true;
            this.customCommandList.ItemHeight = 20;
            this.customCommandList.Items.AddRange(new object[] {
            "Unknown Command"});
            this.customCommandList.Location = new System.Drawing.Point(9, 114);
            this.customCommandList.Name = "customCommandList";
            this.customCommandList.Size = new System.Drawing.Size(299, 135);
            this.customCommandList.TabIndex = 57;
            this.customCommandList.SelectedIndexChanged += new System.EventHandler(this.customCommandList_SelectedIndexChanged);
            // 
            // selectUpgradeTibialyzerButton
            // 
            this.selectUpgradeTibialyzerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.selectUpgradeTibialyzerButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectUpgradeTibialyzerButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.selectUpgradeTibialyzerButton.Location = new System.Drawing.Point(9, 39);
            this.selectUpgradeTibialyzerButton.Name = "selectUpgradeTibialyzerButton";
            this.selectUpgradeTibialyzerButton.Padding = new System.Windows.Forms.Padding(10);
            this.selectUpgradeTibialyzerButton.Size = new System.Drawing.Size(617, 38);
            this.selectUpgradeTibialyzerButton.TabIndex = 56;
            this.selectUpgradeTibialyzerButton.Text = "Select Tibialyzer";
            this.selectUpgradeTibialyzerButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.selectUpgradeTibialyzerButton.Click += new System.EventHandler(this.selectUpgradeTibialyzerButton_Click);
            // 
            // importSettingsHeader
            // 
            this.importSettingsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.importSettingsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importSettingsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.importSettingsHeader.Location = new System.Drawing.Point(9, 9);
            this.importSettingsHeader.Name = "importSettingsHeader";
            this.importSettingsHeader.Size = new System.Drawing.Size(617, 30);
            this.importSettingsHeader.TabIndex = 55;
            this.importSettingsHeader.Text = "Import Settings From Previous Tibialyzer";
            this.importSettingsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // obsSettingsHeader
            // 
            this.obsSettingsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.obsSettingsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.obsSettingsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.obsSettingsHeader.Location = new System.Drawing.Point(9, 254);
            this.obsSettingsHeader.Name = "obsSettingsHeader";
            this.obsSettingsHeader.Size = new System.Drawing.Size(299, 30);
            this.obsSettingsHeader.TabIndex = 64;
            this.obsSettingsHeader.Text = "OBS Settings";
            this.obsSettingsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showPopupWindowButton
            // 
            this.showPopupWindowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.showPopupWindowButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showPopupWindowButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.showPopupWindowButton.Location = new System.Drawing.Point(9, 324);
            this.showPopupWindowButton.Name = "showPopupWindowButton";
            this.showPopupWindowButton.Padding = new System.Windows.Forms.Padding(10);
            this.showPopupWindowButton.Size = new System.Drawing.Size(299, 38);
            this.showPopupWindowButton.TabIndex = 65;
            this.showPopupWindowButton.Text = "Show Popups In Window";
            this.showPopupWindowButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showPopupWindowButton.Click += new System.EventHandler(this.showPopupWindow_Click);
            // 
            // enableWindowCaptureCheckbox
            // 
            this.enableWindowCaptureCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.enableWindowCaptureCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableWindowCaptureCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.enableWindowCaptureCheckbox.Location = new System.Drawing.Point(9, 284);
            this.enableWindowCaptureCheckbox.Name = "enableWindowCaptureCheckbox";
            this.enableWindowCaptureCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.enableWindowCaptureCheckbox.Size = new System.Drawing.Size(299, 40);
            this.enableWindowCaptureCheckbox.TabIndex = 66;
            this.enableWindowCaptureCheckbox.Text = "Enable Window Capture";
            this.enableWindowCaptureCheckbox.UseVisualStyleBackColor = false;
            this.enableWindowCaptureCheckbox.CheckedChanged += new System.EventHandler(this.enableWindowCapture_CheckedChanged);
            // 
            // createBackupButton
            // 
            this.createBackupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.createBackupButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createBackupButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.createBackupButton.Location = new System.Drawing.Point(330, 286);
            this.createBackupButton.Name = "createBackupButton";
            this.createBackupButton.Padding = new System.Windows.Forms.Padding(10);
            this.createBackupButton.Size = new System.Drawing.Size(299, 38);
            this.createBackupButton.TabIndex = 68;
            this.createBackupButton.Text = "Create Backup";
            this.createBackupButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backupSettingsHeader
            // 
            this.backupSettingsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.backupSettingsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backupSettingsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.backupSettingsHeader.Location = new System.Drawing.Point(330, 254);
            this.backupSettingsHeader.Name = "backupSettingsHeader";
            this.backupSettingsHeader.Size = new System.Drawing.Size(299, 32);
            this.backupSettingsHeader.TabIndex = 67;
            this.backupSettingsHeader.Text = "Backup Settings";
            this.backupSettingsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // restoreBackupButton
            // 
            this.restoreBackupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.restoreBackupButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restoreBackupButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.restoreBackupButton.Location = new System.Drawing.Point(330, 324);
            this.restoreBackupButton.Name = "restoreBackupButton";
            this.restoreBackupButton.Padding = new System.Windows.Forms.Padding(10);
            this.restoreBackupButton.Size = new System.Drawing.Size(299, 38);
            this.restoreBackupButton.TabIndex = 69;
            this.restoreBackupButton.Text = "Restore Backup";
            this.restoreBackupButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // automaticallyBackupSettingsCheckbox
            // 
            this.automaticallyBackupSettingsCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.automaticallyBackupSettingsCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.automaticallyBackupSettingsCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.automaticallyBackupSettingsCheckbox.Location = new System.Drawing.Point(330, 362);
            this.automaticallyBackupSettingsCheckbox.Name = "automaticallyBackupSettingsCheckbox";
            this.automaticallyBackupSettingsCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.automaticallyBackupSettingsCheckbox.Size = new System.Drawing.Size(299, 40);
            this.automaticallyBackupSettingsCheckbox.TabIndex = 70;
            this.automaticallyBackupSettingsCheckbox.Text = "Automatically Backup Settings";
            this.automaticallyBackupSettingsCheckbox.UseVisualStyleBackColor = false;
            // 
            // saveImagesHeader
            // 
            this.saveImagesHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.saveImagesHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveImagesHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.saveImagesHeader.Location = new System.Drawing.Point(9, 370);
            this.saveImagesHeader.Name = "saveImagesHeader";
            this.saveImagesHeader.Size = new System.Drawing.Size(299, 32);
            this.saveImagesHeader.TabIndex = 71;
            this.saveImagesHeader.Text = "Save Images";
            this.saveImagesHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveLootImageButton
            // 
            this.saveLootImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveLootImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLootImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveLootImageButton.Location = new System.Drawing.Point(9, 402);
            this.saveLootImageButton.Name = "saveLootImageButton";
            this.saveLootImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.saveLootImageButton.Size = new System.Drawing.Size(299, 38);
            this.saveLootImageButton.TabIndex = 72;
            this.saveLootImageButton.Text = "Save Loot Image";
            this.saveLootImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveLootImageButton.Click += new System.EventHandler(this.saveLootImageButton_Click);
            // 
            // saveDamageImageButton
            // 
            this.saveDamageImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveDamageImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveDamageImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveDamageImageButton.Location = new System.Drawing.Point(9, 440);
            this.saveDamageImageButton.Name = "saveDamageImageButton";
            this.saveDamageImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.saveDamageImageButton.Size = new System.Drawing.Size(299, 38);
            this.saveDamageImageButton.TabIndex = 73;
            this.saveDamageImageButton.Text = "Save Damage Image";
            this.saveDamageImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveDamageImageButton.Click += new System.EventHandler(this.saveDamageImageButton_Click);
            // 
            // saveSummaryImageButton
            // 
            this.saveSummaryImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveSummaryImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveSummaryImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveSummaryImageButton.Location = new System.Drawing.Point(9, 478);
            this.saveSummaryImageButton.Name = "saveSummaryImageButton";
            this.saveSummaryImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.saveSummaryImageButton.Size = new System.Drawing.Size(299, 38);
            this.saveSummaryImageButton.TabIndex = 74;
            this.saveSummaryImageButton.Text = "Save Summary Image";
            this.saveSummaryImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveSummaryImageButton.Click += new System.EventHandler(this.saveSummaryImageButton_Click);
            // 
            // maxDamagePlayersHeader
            // 
            this.maxDamagePlayersHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.maxDamagePlayersHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxDamagePlayersHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.maxDamagePlayersHeader.Location = new System.Drawing.Point(327, 410);
            this.maxDamagePlayersHeader.Name = "maxDamagePlayersHeader";
            this.maxDamagePlayersHeader.Size = new System.Drawing.Size(299, 30);
            this.maxDamagePlayersHeader.TabIndex = 76;
            this.maxDamagePlayersHeader.Text = "Max Damage@ Players";
            this.maxDamagePlayersHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hudAnchorDropDownList
            // 
            this.hudAnchorDropDownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.hudAnchorDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hudAnchorDropDownList.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hudAnchorDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.hudAnchorDropDownList.FormattingEnabled = true;
            this.hudAnchorDropDownList.Items.AddRange(new object[] {
            "Any",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.hudAnchorDropDownList.Location = new System.Drawing.Point(327, 440);
            this.hudAnchorDropDownList.Name = "hudAnchorDropDownList";
            this.hudAnchorDropDownList.Size = new System.Drawing.Size(299, 24);
            this.hudAnchorDropDownList.TabIndex = 75;
            // 
            // SystemTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.maxDamagePlayersHeader);
            this.Controls.Add(this.hudAnchorDropDownList);
            this.Controls.Add(this.saveSummaryImageButton);
            this.Controls.Add(this.saveDamageImageButton);
            this.Controls.Add(this.saveLootImageButton);
            this.Controls.Add(this.saveImagesHeader);
            this.Controls.Add(this.automaticallyBackupSettingsCheckbox);
            this.Controls.Add(this.restoreBackupButton);
            this.Controls.Add(this.createBackupButton);
            this.Controls.Add(this.backupSettingsHeader);
            this.Controls.Add(this.enableWindowCaptureCheckbox);
            this.Controls.Add(this.showPopupWindowButton);
            this.Controls.Add(this.obsSettingsHeader);
            this.Controls.Add(this.customCommandParameterBox);
            this.Controls.Add(this.parametersHeader);
            this.Controls.Add(this.customCommandBox);
            this.Controls.Add(this.customCommandName);
            this.Controls.Add(this.systemCommandHeader);
            this.Controls.Add(this.customCommandsHeader);
            this.Controls.Add(this.customCommandList);
            this.Controls.Add(this.selectUpgradeTibialyzerButton);
            this.Controls.Add(this.importSettingsHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SystemTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private EnterTextBox customCommandParameterBox;
        private EnterTextBox customCommandBox;
        private System.Windows.Forms.Label customCommandName;
        private PrettyListBox customCommandList;
        #endregion
        private PrettyCheckBox enableWindowCaptureCheckbox;
        private PrettyCheckBox automaticallyBackupSettingsCheckbox;
        private PrettyDropDownList hudAnchorDropDownList;
        private PrettyHeader maxDamagePlayersHeader;
        private PrettyButton saveSummaryImageButton;
        private PrettyButton saveDamageImageButton;
        private PrettyButton saveLootImageButton;
        private PrettyHeader saveImagesHeader;
        private PrettyButton restoreBackupButton;
        private PrettyHeader backupSettingsHeader;
        private PrettyButton createBackupButton;
        private PrettyButton showPopupWindowButton;
        private PrettyHeader obsSettingsHeader;
        private PrettyHeader importSettingsHeader;
        private PrettyButton selectUpgradeTibialyzerButton;
        private PrettyHeader customCommandsHeader;
        private PrettyHeader systemCommandHeader;
        private PrettyHeader parametersHeader;
    }
}