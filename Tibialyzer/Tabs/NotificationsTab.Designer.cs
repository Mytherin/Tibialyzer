namespace Tibialyzer {
    partial class NotificationsTab {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationsTab));
            this.selectedWindowHeader = new Tibialyzer.PrettyHeader();
            this.notificationLengthHeader = new Tibialyzer.PrettyHeader();
            this.notificationDurationBox = new System.Windows.Forms.TrackBar();
            this.notificationTypeListHeader = new Tibialyzer.PrettyHeader();
            this.displayGroupHeader = new Tibialyzer.PrettyHeader();
            this.overwriteSettingsHeader = new Tibialyzer.PrettyHeader();
            this.applyTheseSettingsToAllButton = new Tibialyzer.PrettyButton();
            this.clearDisplayButton = new Tibialyzer.PrettyButton();
            this.displayNotificationButton = new Tibialyzer.PrettyButton();
            this.testingHeader = new Tibialyzer.PrettyHeader();
            this.yOffsetLabel = new System.Windows.Forms.Label();
            this.xOffsetLabel = new System.Windows.Forms.Label();
            this.positionOffsetHeader = new Tibialyzer.PrettyHeader();
            this.anchorHeader = new Tibialyzer.PrettyHeader();
            this.monitorAnchorHeader = new Tibialyzer.PrettyHeader();
            this.onlyShowWhenTibiaIsActiveCheckbox = new Tibialyzer.PrettyCheckBox();
            this.monitorAnchorDropdown = new Tibialyzer.PrettyDropDownList();
            this.notificationTypeList = new Tibialyzer.PrettyListBox();
            this.displayGroupDropDownList = new Tibialyzer.PrettyDropDownList();
            this.notificationYOffsetBox = new Tibialyzer.EnterTextBox();
            this.notificationXOffsetBox = new Tibialyzer.EnterTextBox();
            this.notificationAnchorDropDownList = new Tibialyzer.PrettyDropDownList();
            this.requireDoubleClickCheckbox = new Tibialyzer.PrettyCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.notificationDurationBox)).BeginInit();
            this.SuspendLayout();
            // 
            // selectedWindowHeader
            // 
            this.selectedWindowHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(128)))), ((int)(((byte)(176)))));
            this.selectedWindowHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedWindowHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.selectedWindowHeader.Location = new System.Drawing.Point(9, 9);
            this.selectedWindowHeader.Name = "selectedWindowHeader";
            this.selectedWindowHeader.Size = new System.Drawing.Size(299, 30);
            this.selectedWindowHeader.TabIndex = 55;
            this.selectedWindowHeader.Text = "Loot Window";
            this.selectedWindowHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // notificationLengthHeader
            // 
            this.notificationLengthHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.notificationLengthHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationLengthHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.notificationLengthHeader.Location = new System.Drawing.Point(327, 203);
            this.notificationLengthHeader.Name = "notificationLengthHeader";
            this.notificationLengthHeader.Size = new System.Drawing.Size(299, 30);
            this.notificationLengthHeader.TabIndex = 54;
            this.notificationLengthHeader.Text = "Notification Length";
            this.notificationLengthHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // notificationDurationBox
            // 
            this.notificationDurationBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationDurationBox.LargeChange = 50;
            this.notificationDurationBox.Location = new System.Drawing.Point(327, 233);
            this.notificationDurationBox.Maximum = 300;
            this.notificationDurationBox.Minimum = 1;
            this.notificationDurationBox.Name = "notificationDurationBox";
            this.notificationDurationBox.Size = new System.Drawing.Size(299, 45);
            this.notificationDurationBox.SmallChange = 5;
            this.notificationDurationBox.TabIndex = 53;
            this.notificationDurationBox.Value = 5;
            this.notificationDurationBox.Scroll += new System.EventHandler(this.notificationDurationBox_Scroll);
            // 
            // notificationTypeListHeader
            // 
            this.notificationTypeListHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.notificationTypeListHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationTypeListHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.notificationTypeListHeader.Location = new System.Drawing.Point(9, 39);
            this.notificationTypeListHeader.Name = "notificationTypeListHeader";
            this.notificationTypeListHeader.Size = new System.Drawing.Size(299, 30);
            this.notificationTypeListHeader.TabIndex = 52;
            this.notificationTypeListHeader.Text = "Notification Type List";
            this.notificationTypeListHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // displayGroupHeader
            // 
            this.displayGroupHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.displayGroupHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayGroupHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.displayGroupHeader.Location = new System.Drawing.Point(327, 146);
            this.displayGroupHeader.Name = "displayGroupHeader";
            this.displayGroupHeader.Size = new System.Drawing.Size(299, 30);
            this.displayGroupHeader.TabIndex = 50;
            this.displayGroupHeader.Text = "Display Group";
            this.displayGroupHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overwriteSettingsHeader
            // 
            this.overwriteSettingsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.overwriteSettingsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overwriteSettingsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.overwriteSettingsHeader.Location = new System.Drawing.Point(327, 391);
            this.overwriteSettingsHeader.Name = "overwriteSettingsHeader";
            this.overwriteSettingsHeader.Size = new System.Drawing.Size(299, 30);
            this.overwriteSettingsHeader.TabIndex = 48;
            this.overwriteSettingsHeader.Text = "Overwrite Settings";
            this.overwriteSettingsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // applyTheseSettingsToAllButton
            // 
            this.applyTheseSettingsToAllButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.applyTheseSettingsToAllButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyTheseSettingsToAllButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.applyTheseSettingsToAllButton.Image = ((System.Drawing.Image)(resources.GetObject("applyTheseSettingsToAllButton.Image")));
            this.applyTheseSettingsToAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.applyTheseSettingsToAllButton.Location = new System.Drawing.Point(327, 421);
            this.applyTheseSettingsToAllButton.Name = "applyTheseSettingsToAllButton";
            this.applyTheseSettingsToAllButton.Padding = new System.Windows.Forms.Padding(10);
            this.applyTheseSettingsToAllButton.Size = new System.Drawing.Size(299, 45);
            this.applyTheseSettingsToAllButton.TabIndex = 41;
            this.applyTheseSettingsToAllButton.Text = "Apply These Settings To All";
            this.applyTheseSettingsToAllButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.applyTheseSettingsToAllButton.Click += new System.EventHandler(this.applyNotificationSettingsToAllButton_Click);
            // 
            // clearDisplayButton
            // 
            this.clearDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.clearDisplayButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearDisplayButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.clearDisplayButton.Location = new System.Drawing.Point(327, 349);
            this.clearDisplayButton.Name = "clearDisplayButton";
            this.clearDisplayButton.Padding = new System.Windows.Forms.Padding(10);
            this.clearDisplayButton.Size = new System.Drawing.Size(299, 38);
            this.clearDisplayButton.TabIndex = 40;
            this.clearDisplayButton.Text = "Clear Display";
            this.clearDisplayButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.clearDisplayButton.Click += new System.EventHandler(this.clearNotificationDisplayButton_Click);
            // 
            // displayNotificationButton
            // 
            this.displayNotificationButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayNotificationButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayNotificationButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayNotificationButton.Location = new System.Drawing.Point(327, 311);
            this.displayNotificationButton.Name = "displayNotificationButton";
            this.displayNotificationButton.Padding = new System.Windows.Forms.Padding(10);
            this.displayNotificationButton.Size = new System.Drawing.Size(299, 38);
            this.displayNotificationButton.TabIndex = 39;
            this.displayNotificationButton.Text = "Display Notification";
            this.displayNotificationButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.displayNotificationButton.Click += new System.EventHandler(this.testNotificationDisplayButton_Click);
            // 
            // testingHeader
            // 
            this.testingHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.testingHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testingHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.testingHeader.Location = new System.Drawing.Point(327, 281);
            this.testingHeader.Name = "testingHeader";
            this.testingHeader.Size = new System.Drawing.Size(299, 30);
            this.testingHeader.TabIndex = 38;
            this.testingHeader.Text = "Testing";
            this.testingHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // yOffsetLabel
            // 
            this.yOffsetLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.yOffsetLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.yOffsetLabel.Location = new System.Drawing.Point(327, 62);
            this.yOffsetLabel.Name = "yOffsetLabel";
            this.yOffsetLabel.Size = new System.Drawing.Size(99, 23);
            this.yOffsetLabel.TabIndex = 37;
            this.yOffsetLabel.Text = "Y Offset";
            this.yOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xOffsetLabel
            // 
            this.xOffsetLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.xOffsetLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.xOffsetLabel.Location = new System.Drawing.Point(327, 39);
            this.xOffsetLabel.Name = "xOffsetLabel";
            this.xOffsetLabel.Size = new System.Drawing.Size(99, 23);
            this.xOffsetLabel.TabIndex = 35;
            this.xOffsetLabel.Text = "X Offset";
            this.xOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // positionOffsetHeader
            // 
            this.positionOffsetHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.positionOffsetHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positionOffsetHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.positionOffsetHeader.Location = new System.Drawing.Point(327, 9);
            this.positionOffsetHeader.Name = "positionOffsetHeader";
            this.positionOffsetHeader.Size = new System.Drawing.Size(299, 30);
            this.positionOffsetHeader.TabIndex = 33;
            this.positionOffsetHeader.Text = "Position (Offset)";
            this.positionOffsetHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // anchorHeader
            // 
            this.anchorHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anchorHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.anchorHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.anchorHeader.Location = new System.Drawing.Point(327, 88);
            this.anchorHeader.Name = "anchorHeader";
            this.anchorHeader.Size = new System.Drawing.Size(299, 30);
            this.anchorHeader.TabIndex = 32;
            this.anchorHeader.Text = "Anchor";
            this.anchorHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // monitorAnchorHeader
            // 
            this.monitorAnchorHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.monitorAnchorHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monitorAnchorHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.monitorAnchorHeader.Location = new System.Drawing.Point(9, 389);
            this.monitorAnchorHeader.Name = "monitorAnchorHeader";
            this.monitorAnchorHeader.Size = new System.Drawing.Size(299, 30);
            this.monitorAnchorHeader.TabIndex = 57;
            this.monitorAnchorHeader.Text = "Monitor Anchor";
            this.monitorAnchorHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // onlyShowWhenTibiaIsActiveCheckbox
            // 
            this.onlyShowWhenTibiaIsActiveCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.onlyShowWhenTibiaIsActiveCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onlyShowWhenTibiaIsActiveCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.onlyShowWhenTibiaIsActiveCheckbox.Location = new System.Drawing.Point(9, 446);
            this.onlyShowWhenTibiaIsActiveCheckbox.Name = "onlyShowWhenTibiaIsActiveCheckbox";
            this.onlyShowWhenTibiaIsActiveCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.onlyShowWhenTibiaIsActiveCheckbox.Size = new System.Drawing.Size(299, 40);
            this.onlyShowWhenTibiaIsActiveCheckbox.TabIndex = 58;
            this.onlyShowWhenTibiaIsActiveCheckbox.Text = "Only Show When Tibia is Active";
            this.onlyShowWhenTibiaIsActiveCheckbox.UseVisualStyleBackColor = false;
            this.onlyShowWhenTibiaIsActiveCheckbox.CheckedChanged += new System.EventHandler(this.hideWhenTibiaMinimized_CheckedChanged);
            // 
            // monitorAnchorDropdown
            // 
            this.monitorAnchorDropdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.monitorAnchorDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.monitorAnchorDropdown.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monitorAnchorDropdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.monitorAnchorDropdown.FormattingEnabled = true;
            this.monitorAnchorDropdown.Items.AddRange(new object[] {
            "Tibia Client Monitor",
            "Tibialyzer Monitor"});
            this.monitorAnchorDropdown.Location = new System.Drawing.Point(9, 419);
            this.monitorAnchorDropdown.Name = "monitorAnchorDropdown";
            this.monitorAnchorDropdown.Size = new System.Drawing.Size(299, 24);
            this.monitorAnchorDropdown.TabIndex = 56;
            this.monitorAnchorDropdown.SelectedIndexChanged += new System.EventHandler(this.monitorAnchorDropdown_SelectedIndexChanged);
            // 
            // notificationTypeList
            // 
            this.notificationTypeList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationTypeList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.notificationTypeList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.notificationTypeList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationTypeList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationTypeList.FormattingEnabled = true;
            this.notificationTypeList.ItemHeight = 20;
            this.notificationTypeList.Items.AddRange(new object[] {
            "Loot Window"});
            this.notificationTypeList.Location = new System.Drawing.Point(9, 69);
            this.notificationTypeList.Name = "notificationTypeList";
            this.notificationTypeList.Size = new System.Drawing.Size(299, 315);
            this.notificationTypeList.TabIndex = 51;
            this.notificationTypeList.SelectedIndexChanged += new System.EventHandler(this.notificationTypeList_SelectedIndexChanged);
            // 
            // displayGroupDropDownList
            // 
            this.displayGroupDropDownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayGroupDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displayGroupDropDownList.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayGroupDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayGroupDropDownList.FormattingEnabled = true;
            this.displayGroupDropDownList.Items.AddRange(new object[] {
            "Group 1",
            "Group 2",
            "Group 3",
            "Group 4",
            "Group 5",
            "Group 6",
            "Group 7",
            "Group 8",
            "Group 9",
            "Group 10"});
            this.displayGroupDropDownList.Location = new System.Drawing.Point(327, 176);
            this.displayGroupDropDownList.Name = "displayGroupDropDownList";
            this.displayGroupDropDownList.Size = new System.Drawing.Size(299, 24);
            this.displayGroupDropDownList.TabIndex = 49;
            this.displayGroupDropDownList.SelectedIndexChanged += new System.EventHandler(this.groupSelectionList_SelectedIndexChanged);
            // 
            // notificationYOffsetBox
            // 
            this.notificationYOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationYOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationYOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.notificationYOffsetBox.Location = new System.Drawing.Point(425, 62);
            this.notificationYOffsetBox.Name = "notificationYOffsetBox";
            this.notificationYOffsetBox.Size = new System.Drawing.Size(201, 23);
            this.notificationYOffsetBox.TabIndex = 36;
            this.notificationYOffsetBox.TextChanged += new System.EventHandler(this.notificationYOffsetBox_TextChanged);
            // 
            // notificationXOffsetBox
            // 
            this.notificationXOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationXOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationXOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.notificationXOffsetBox.Location = new System.Drawing.Point(425, 39);
            this.notificationXOffsetBox.Name = "notificationXOffsetBox";
            this.notificationXOffsetBox.Size = new System.Drawing.Size(201, 23);
            this.notificationXOffsetBox.TabIndex = 34;
            this.notificationXOffsetBox.TextChanged += new System.EventHandler(this.notificationXOffsetBox_TextChanged);
            // 
            // notificationAnchorDropDownList
            // 
            this.notificationAnchorDropDownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationAnchorDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.notificationAnchorDropDownList.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationAnchorDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.notificationAnchorDropDownList.FormattingEnabled = true;
            this.notificationAnchorDropDownList.Items.AddRange(new object[] {
            "Top Left",
            "Top Right",
            "Bottom Left",
            "Bottom Right"});
            this.notificationAnchorDropDownList.Location = new System.Drawing.Point(327, 118);
            this.notificationAnchorDropDownList.Name = "notificationAnchorDropDownList";
            this.notificationAnchorDropDownList.Size = new System.Drawing.Size(299, 24);
            this.notificationAnchorDropDownList.TabIndex = 31;
            this.notificationAnchorDropDownList.SelectedIndexChanged += new System.EventHandler(this.notificationAnchorBox_SelectedIndexChanged);
            // 
            // requireDoubleClickCheckbox
            // 
            this.requireDoubleClickCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.requireDoubleClickCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requireDoubleClickCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.requireDoubleClickCheckbox.Location = new System.Drawing.Point(9, 492);
            this.requireDoubleClickCheckbox.Name = "requireDoubleClickCheckbox";
            this.requireDoubleClickCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.requireDoubleClickCheckbox.Size = new System.Drawing.Size(299, 40);
            this.requireDoubleClickCheckbox.TabIndex = 59;
            this.requireDoubleClickCheckbox.Text = "Require Double Click To Close Notification";
            this.requireDoubleClickCheckbox.UseVisualStyleBackColor = false;
            this.requireDoubleClickCheckbox.CheckedChanged += new System.EventHandler(this.requireDoubleClickCheckbox_CheckedChanged);
            // 
            // NotificationsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.requireDoubleClickCheckbox);
            this.Controls.Add(this.onlyShowWhenTibiaIsActiveCheckbox);
            this.Controls.Add(this.monitorAnchorHeader);
            this.Controls.Add(this.monitorAnchorDropdown);
            this.Controls.Add(this.selectedWindowHeader);
            this.Controls.Add(this.notificationLengthHeader);
            this.Controls.Add(this.notificationDurationBox);
            this.Controls.Add(this.notificationTypeListHeader);
            this.Controls.Add(this.displayGroupHeader);
            this.Controls.Add(this.overwriteSettingsHeader);
            this.Controls.Add(this.applyTheseSettingsToAllButton);
            this.Controls.Add(this.clearDisplayButton);
            this.Controls.Add(this.displayNotificationButton);
            this.Controls.Add(this.testingHeader);
            this.Controls.Add(this.yOffsetLabel);
            this.Controls.Add(this.xOffsetLabel);
            this.Controls.Add(this.positionOffsetHeader);
            this.Controls.Add(this.anchorHeader);
            this.Controls.Add(this.notificationTypeList);
            this.Controls.Add(this.displayGroupDropDownList);
            this.Controls.Add(this.notificationYOffsetBox);
            this.Controls.Add(this.notificationXOffsetBox);
            this.Controls.Add(this.notificationAnchorDropDownList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NotificationsTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.notificationDurationBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.TrackBar notificationDurationBox;
        private System.Windows.Forms.Label yOffsetLabel;
        private System.Windows.Forms.Label xOffsetLabel;
        private PrettyListBox notificationTypeList;
        private PrettyDropDownList displayGroupDropDownList;
        private EnterTextBox notificationYOffsetBox;
        private EnterTextBox notificationXOffsetBox;
        private PrettyDropDownList notificationAnchorDropDownList;
        #endregion
        private PrettyDropDownList monitorAnchorDropdown;
        private PrettyCheckBox onlyShowWhenTibiaIsActiveCheckbox;
        private PrettyHeader monitorAnchorHeader;
        private PrettyHeader anchorHeader;
        private PrettyHeader positionOffsetHeader;
        private PrettyHeader testingHeader;
        private PrettyButton displayNotificationButton;
        private PrettyButton clearDisplayButton;
        private PrettyButton applyTheseSettingsToAllButton;
        private PrettyHeader overwriteSettingsHeader;
        private PrettyHeader displayGroupHeader;
        private PrettyHeader notificationTypeListHeader;
        private PrettyHeader notificationLengthHeader;
        private PrettyHeader selectedWindowHeader;
        private PrettyCheckBox requireDoubleClickCheckbox;
    }
}