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
            this.selectedWindowLabel = new System.Windows.Forms.Label();
            this.notificationDurationLabel = new System.Windows.Forms.Label();
            this.notificationDurationBox = new System.Windows.Forms.TrackBar();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.applyNotificationSettingsToAllButton = new System.Windows.Forms.Label();
            this.clearNotificationDisplayButton = new System.Windows.Forms.Label();
            this.testNotificationDisplayButton = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.hideWhenTibiaMinimized = new Tibialyzer.PrettyCheckBox();
            this.monitorAnchorDropdown = new Tibialyzer.PrettyDropDownList();
            this.notificationTypeList = new Tibialyzer.PrettyListBox();
            this.notificationGroupBox = new Tibialyzer.PrettyDropDownList();
            this.notificationYOffsetBox = new Tibialyzer.EnterTextBox();
            this.notificationXOffsetBox = new Tibialyzer.EnterTextBox();
            this.notificationAnchorBox = new Tibialyzer.PrettyDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.notificationDurationBox)).BeginInit();
            this.SuspendLayout();
            // 
            // selectedWindowLabel
            // 
            this.selectedWindowLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(128)))), ((int)(((byte)(176)))));
            this.selectedWindowLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedWindowLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.selectedWindowLabel.Location = new System.Drawing.Point(3, 21);
            this.selectedWindowLabel.Name = "selectedWindowLabel";
            this.selectedWindowLabel.Size = new System.Drawing.Size(275, 30);
            this.selectedWindowLabel.TabIndex = 55;
            this.selectedWindowLabel.Text = "Loot Window";
            this.selectedWindowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // notificationDurationLabel
            // 
            this.notificationDurationLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.notificationDurationLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationDurationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationDurationLabel.Location = new System.Drawing.Point(284, 213);
            this.notificationDurationLabel.Name = "notificationDurationLabel";
            this.notificationDurationLabel.Size = new System.Drawing.Size(250, 30);
            this.notificationDurationLabel.TabIndex = 54;
            this.notificationDurationLabel.Text = "Notification Length";
            this.notificationDurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // notificationDurationBox
            // 
            this.notificationDurationBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationDurationBox.LargeChange = 50;
            this.notificationDurationBox.Location = new System.Drawing.Point(284, 243);
            this.notificationDurationBox.Maximum = 300;
            this.notificationDurationBox.Minimum = 1;
            this.notificationDurationBox.Name = "notificationDurationBox";
            this.notificationDurationBox.Size = new System.Drawing.Size(250, 45);
            this.notificationDurationBox.SmallChange = 5;
            this.notificationDurationBox.TabIndex = 53;
            this.notificationDurationBox.Value = 5;
            this.notificationDurationBox.Scroll += new System.EventHandler(this.notificationDurationBox_Scroll);
            this.notificationDurationBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.notificationDurationBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // label47
            // 
            this.label47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label47.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label47.Location = new System.Drawing.Point(3, 51);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(275, 30);
            this.label47.TabIndex = 52;
            this.label47.Text = "Notification Type List";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label46
            // 
            this.label46.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label46.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label46.Location = new System.Drawing.Point(284, 156);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(250, 30);
            this.label46.TabIndex = 50;
            this.label46.Text = "Display Group";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label45
            // 
            this.label45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.label45.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label45.Location = new System.Drawing.Point(284, 401);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(250, 30);
            this.label45.TabIndex = 48;
            this.label45.Text = "Overwrite Settings";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // applyNotificationSettingsToAllButton
            // 
            this.applyNotificationSettingsToAllButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.applyNotificationSettingsToAllButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyNotificationSettingsToAllButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.applyNotificationSettingsToAllButton.Location = new System.Drawing.Point(284, 431);
            this.applyNotificationSettingsToAllButton.Name = "applyNotificationSettingsToAllButton";
            this.applyNotificationSettingsToAllButton.Padding = new System.Windows.Forms.Padding(10);
            this.applyNotificationSettingsToAllButton.Size = new System.Drawing.Size(250, 45);
            this.applyNotificationSettingsToAllButton.TabIndex = 41;
            this.applyNotificationSettingsToAllButton.Text = "Apply These Settings To All";
            this.applyNotificationSettingsToAllButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.applyNotificationSettingsToAllButton.Click += new System.EventHandler(this.applyNotificationSettingsToAllButton_Click);
            this.applyNotificationSettingsToAllButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.applyNotificationSettingsToAllButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // clearNotificationDisplayButton
            // 
            this.clearNotificationDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.clearNotificationDisplayButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearNotificationDisplayButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.clearNotificationDisplayButton.Location = new System.Drawing.Point(284, 359);
            this.clearNotificationDisplayButton.Name = "clearNotificationDisplayButton";
            this.clearNotificationDisplayButton.Padding = new System.Windows.Forms.Padding(10);
            this.clearNotificationDisplayButton.Size = new System.Drawing.Size(250, 38);
            this.clearNotificationDisplayButton.TabIndex = 40;
            this.clearNotificationDisplayButton.Text = "Clear Display";
            this.clearNotificationDisplayButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.clearNotificationDisplayButton.Click += new System.EventHandler(this.clearNotificationDisplayButton_Click);
            this.clearNotificationDisplayButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.clearNotificationDisplayButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // testNotificationDisplayButton
            // 
            this.testNotificationDisplayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.testNotificationDisplayButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testNotificationDisplayButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.testNotificationDisplayButton.Location = new System.Drawing.Point(284, 321);
            this.testNotificationDisplayButton.Name = "testNotificationDisplayButton";
            this.testNotificationDisplayButton.Padding = new System.Windows.Forms.Padding(10);
            this.testNotificationDisplayButton.Size = new System.Drawing.Size(250, 38);
            this.testNotificationDisplayButton.TabIndex = 39;
            this.testNotificationDisplayButton.Text = "Display Notification";
            this.testNotificationDisplayButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testNotificationDisplayButton.Click += new System.EventHandler(this.testNotificationDisplayButton_Click);
            this.testNotificationDisplayButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.testNotificationDisplayButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // label41
            // 
            this.label41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label41.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label41.Location = new System.Drawing.Point(284, 291);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(250, 30);
            this.label41.TabIndex = 38;
            this.label41.Text = "Testing";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label40
            // 
            this.label40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label40.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.label40.Location = new System.Drawing.Point(284, 72);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(99, 23);
            this.label40.TabIndex = 37;
            this.label40.Text = "Y Offset";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label39
            // 
            this.label39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label39.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.label39.Location = new System.Drawing.Point(284, 49);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(99, 23);
            this.label39.TabIndex = 35;
            this.label39.Text = "X Offset";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label37.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label37.Location = new System.Drawing.Point(284, 19);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(250, 30);
            this.label37.TabIndex = 33;
            this.label37.Text = "Position (Offset)";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label36
            // 
            this.label36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label36.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label36.Location = new System.Drawing.Point(284, 98);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(250, 30);
            this.label36.TabIndex = 32;
            this.label36.Text = "Anchor";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label1.Location = new System.Drawing.Point(3, 401);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 30);
            this.label1.TabIndex = 57;
            this.label1.Text = "Monitor Anchor";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hideWhenTibiaMinimized
            // 
            this.hideWhenTibiaMinimized.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.hideWhenTibiaMinimized.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hideWhenTibiaMinimized.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.hideWhenTibiaMinimized.Location = new System.Drawing.Point(3, 458);
            this.hideWhenTibiaMinimized.Name = "hideWhenTibiaMinimized";
            this.hideWhenTibiaMinimized.Padding = new System.Windows.Forms.Padding(10);
            this.hideWhenTibiaMinimized.Size = new System.Drawing.Size(275, 40);
            this.hideWhenTibiaMinimized.TabIndex = 58;
            this.hideWhenTibiaMinimized.Text = "Only Show When Tibia is Active";
            this.hideWhenTibiaMinimized.UseVisualStyleBackColor = false;
            this.hideWhenTibiaMinimized.CheckedChanged += new System.EventHandler(this.hideWhenTibiaMinimized_CheckedChanged);
            this.hideWhenTibiaMinimized.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.hideWhenTibiaMinimized.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
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
            this.monitorAnchorDropdown.Location = new System.Drawing.Point(3, 431);
            this.monitorAnchorDropdown.Name = "monitorAnchorDropdown";
            this.monitorAnchorDropdown.Size = new System.Drawing.Size(275, 24);
            this.monitorAnchorDropdown.TabIndex = 56;
            this.monitorAnchorDropdown.SelectedIndexChanged += new System.EventHandler(this.monitorAnchorDropdown_SelectedIndexChanged);
            this.monitorAnchorDropdown.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.monitorAnchorDropdown.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
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
            this.notificationTypeList.Location = new System.Drawing.Point(3, 81);
            this.notificationTypeList.Name = "notificationTypeList";
            this.notificationTypeList.Size = new System.Drawing.Size(275, 315);
            this.notificationTypeList.TabIndex = 51;
            this.notificationTypeList.SelectedIndexChanged += new System.EventHandler(this.notificationTypeList_SelectedIndexChanged);
            // 
            // notificationGroupBox
            // 
            this.notificationGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationGroupBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.notificationGroupBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.notificationGroupBox.FormattingEnabled = true;
            this.notificationGroupBox.Items.AddRange(new object[] {
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
            this.notificationGroupBox.Location = new System.Drawing.Point(284, 186);
            this.notificationGroupBox.Name = "notificationGroupBox";
            this.notificationGroupBox.Size = new System.Drawing.Size(250, 24);
            this.notificationGroupBox.TabIndex = 49;
            this.notificationGroupBox.SelectedIndexChanged += new System.EventHandler(this.groupSelectionList_SelectedIndexChanged);
            this.notificationGroupBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.notificationGroupBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // notificationYOffsetBox
            // 
            this.notificationYOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationYOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationYOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.notificationYOffsetBox.Location = new System.Drawing.Point(382, 72);
            this.notificationYOffsetBox.Name = "notificationYOffsetBox";
            this.notificationYOffsetBox.Size = new System.Drawing.Size(152, 23);
            this.notificationYOffsetBox.TabIndex = 36;
            this.notificationYOffsetBox.TextChanged += new System.EventHandler(this.notificationYOffsetBox_TextChanged);
            this.notificationYOffsetBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.notificationYOffsetBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // notificationXOffsetBox
            // 
            this.notificationXOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationXOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationXOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.notificationXOffsetBox.Location = new System.Drawing.Point(382, 49);
            this.notificationXOffsetBox.Name = "notificationXOffsetBox";
            this.notificationXOffsetBox.Size = new System.Drawing.Size(152, 23);
            this.notificationXOffsetBox.TabIndex = 34;
            this.notificationXOffsetBox.TextChanged += new System.EventHandler(this.notificationXOffsetBox_TextChanged);
            this.notificationXOffsetBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.notificationXOffsetBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // notificationAnchorBox
            // 
            this.notificationAnchorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.notificationAnchorBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.notificationAnchorBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationAnchorBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.notificationAnchorBox.FormattingEnabled = true;
            this.notificationAnchorBox.Items.AddRange(new object[] {
            "Top Left",
            "Top Right",
            "Bottom Left",
            "Bottom Right"});
            this.notificationAnchorBox.Location = new System.Drawing.Point(284, 128);
            this.notificationAnchorBox.Name = "notificationAnchorBox";
            this.notificationAnchorBox.Size = new System.Drawing.Size(250, 24);
            this.notificationAnchorBox.TabIndex = 31;
            this.notificationAnchorBox.SelectedIndexChanged += new System.EventHandler(this.notificationAnchorBox_SelectedIndexChanged);
            this.notificationAnchorBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.notificationAnchorBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // NotificationsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.Controls.Add(this.hideWhenTibiaMinimized);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.monitorAnchorDropdown);
            this.Controls.Add(this.selectedWindowLabel);
            this.Controls.Add(this.notificationDurationLabel);
            this.Controls.Add(this.notificationDurationBox);
            this.Controls.Add(this.label47);
            this.Controls.Add(this.label46);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.applyNotificationSettingsToAllButton);
            this.Controls.Add(this.clearNotificationDisplayButton);
            this.Controls.Add(this.testNotificationDisplayButton);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.notificationTypeList);
            this.Controls.Add(this.notificationGroupBox);
            this.Controls.Add(this.notificationYOffsetBox);
            this.Controls.Add(this.notificationXOffsetBox);
            this.Controls.Add(this.notificationAnchorBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NotificationsTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.notificationDurationBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label selectedWindowLabel;
        private System.Windows.Forms.Label notificationDurationLabel;
        private System.Windows.Forms.TrackBar notificationDurationBox;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label applyNotificationSettingsToAllButton;
        private System.Windows.Forms.Label clearNotificationDisplayButton;
        private System.Windows.Forms.Label testNotificationDisplayButton;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private PrettyListBox notificationTypeList;
        private PrettyDropDownList notificationGroupBox;
        private EnterTextBox notificationYOffsetBox;
        private EnterTextBox notificationXOffsetBox;
        private PrettyDropDownList notificationAnchorBox;
        #endregion

        private System.Windows.Forms.Label label1;
        private PrettyDropDownList monitorAnchorDropdown;
        private PrettyCheckBox hideWhenTibiaMinimized;
    }
}