namespace Tibialyzer {
    partial class MainTab {
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
            this.detectFlashClientButton = new System.Windows.Forms.Label();
            this.saveAllLootCheckbox = new Tibialyzer.PrettyCheckBox();
            this.expValueLabel = new System.Windows.Forms.Label();
            this.ignoreLowExperienceBox = new Tibialyzer.EnterTextBox();
            this.ignoreLowExperienceButton = new Tibialyzer.PrettyCheckBox();
            this.stackAllItemsCheckbox = new Tibialyzer.PrettyCheckBox();
            this.lootOptionsHeaderLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.mainExecuteTibialyzerCommandLabel = new System.Windows.Forms.Label();
            this.saveDamageImageButton = new System.Windows.Forms.Label();
            this.saveLootImageButton = new System.Windows.Forms.Label();
            this.selectClientProgramButton = new System.Windows.Forms.Label();
            this.mainOptionsLabel = new System.Windows.Forms.Label();
            this.executeButton = new System.Windows.Forms.Label();
            this.issuesGuideButton = new System.Windows.Forms.Label();
            this.popupsGuideButton = new System.Windows.Forms.Label();
            this.lootGuideButton = new System.Windows.Forms.Label();
            this.mainGuidesLabel = new System.Windows.Forms.Label();
            this.gettingStartedGuideButton = new System.Windows.Forms.Label();
            this.nameListBox = new Tibialyzer.PrettyListBox();
            this.namesLabel = new System.Windows.Forms.Label();
            this.commandTextBox = new Tibialyzer.EnterTextBox();
            this.SuspendLayout();
            // 
            // detectFlashClientButton
            // 
            this.detectFlashClientButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.detectFlashClientButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detectFlashClientButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.detectFlashClientButton.Location = new System.Drawing.Point(305, 87);
            this.detectFlashClientButton.Name = "detectFlashClientButton";
            this.detectFlashClientButton.Padding = new System.Windows.Forms.Padding(10);
            this.detectFlashClientButton.Size = new System.Drawing.Size(226, 38);
            this.detectFlashClientButton.TabIndex = 46;
            this.detectFlashClientButton.Text = "Detect Flash Client";
            this.detectFlashClientButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.detectFlashClientButton.Click += new System.EventHandler(this.detectFlashClientButton_Click);
            this.detectFlashClientButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.detectFlashClientButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // saveAllLootCheckbox
            // 
            this.saveAllLootCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveAllLootCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveAllLootCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveAllLootCheckbox.Location = new System.Drawing.Point(3, 300);
            this.saveAllLootCheckbox.Name = "saveAllLootCheckbox";
            this.saveAllLootCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.saveAllLootCheckbox.Size = new System.Drawing.Size(296, 40);
            this.saveAllLootCheckbox.TabIndex = 45;
            this.saveAllLootCheckbox.Text = "Automatically Write Loot To File";
            this.saveAllLootCheckbox.UseVisualStyleBackColor = false;
            this.saveAllLootCheckbox.CheckedChanged += new System.EventHandler(this.saveAllLootCheckbox_CheckedChanged);
            this.saveAllLootCheckbox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.saveAllLootCheckbox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // expValueLabel
            // 
            this.expValueLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.expValueLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.expValueLabel.Location = new System.Drawing.Point(3, 277);
            this.expValueLabel.Name = "expValueLabel";
            this.expValueLabel.Size = new System.Drawing.Size(99, 23);
            this.expValueLabel.TabIndex = 44;
            this.expValueLabel.Text = "Exp Value";
            this.expValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ignoreLowExperienceBox
            // 
            this.ignoreLowExperienceBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.ignoreLowExperienceBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ignoreLowExperienceBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.ignoreLowExperienceBox.Location = new System.Drawing.Point(101, 277);
            this.ignoreLowExperienceBox.Name = "ignoreLowExperienceBox";
            this.ignoreLowExperienceBox.Size = new System.Drawing.Size(198, 23);
            this.ignoreLowExperienceBox.TabIndex = 43;
            this.ignoreLowExperienceBox.TextChanged += new System.EventHandler(this.ignoreLowExperienceBox_TextChanged);
            this.ignoreLowExperienceBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.ignoreLowExperienceBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // ignoreLowExperienceButton
            // 
            this.ignoreLowExperienceButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.ignoreLowExperienceButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ignoreLowExperienceButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.ignoreLowExperienceButton.Location = new System.Drawing.Point(3, 237);
            this.ignoreLowExperienceButton.Name = "ignoreLowExperienceButton";
            this.ignoreLowExperienceButton.Padding = new System.Windows.Forms.Padding(10);
            this.ignoreLowExperienceButton.Size = new System.Drawing.Size(296, 40);
            this.ignoreLowExperienceButton.TabIndex = 39;
            this.ignoreLowExperienceButton.Text = "Ignore Low Experience Creatures";
            this.ignoreLowExperienceButton.UseVisualStyleBackColor = false;
            this.ignoreLowExperienceButton.CheckedChanged += new System.EventHandler(this.ignoreLowExperienceButton_CheckedChanged);
            this.ignoreLowExperienceButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.ignoreLowExperienceButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // stackAllItemsCheckbox
            // 
            this.stackAllItemsCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.stackAllItemsCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stackAllItemsCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.stackAllItemsCheckbox.Location = new System.Drawing.Point(3, 197);
            this.stackAllItemsCheckbox.Name = "stackAllItemsCheckbox";
            this.stackAllItemsCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.stackAllItemsCheckbox.Size = new System.Drawing.Size(296, 40);
            this.stackAllItemsCheckbox.TabIndex = 38;
            this.stackAllItemsCheckbox.Text = "Display All Items As Stackable";
            this.stackAllItemsCheckbox.UseVisualStyleBackColor = false;
            this.stackAllItemsCheckbox.CheckedChanged += new System.EventHandler(this.stackAllItemsCheckbox_CheckedChanged);
            this.stackAllItemsCheckbox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.stackAllItemsCheckbox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // lootOptionsHeaderLabel
            // 
            this.lootOptionsHeaderLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.lootOptionsHeaderLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lootOptionsHeaderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.lootOptionsHeaderLabel.Location = new System.Drawing.Point(305, 158);
            this.lootOptionsHeaderLabel.Name = "lootOptionsHeaderLabel";
            this.lootOptionsHeaderLabel.Size = new System.Drawing.Size(226, 30);
            this.lootOptionsHeaderLabel.TabIndex = 37;
            this.lootOptionsHeaderLabel.Text = "Loot Options";
            this.lootOptionsHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label9.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label9.Location = new System.Drawing.Point(3, 167);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(296, 30);
            this.label9.TabIndex = 36;
            this.label9.Text = "Loot Display Options";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainExecuteTibialyzerCommandLabel
            // 
            this.mainExecuteTibialyzerCommandLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.mainExecuteTibialyzerCommandLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainExecuteTibialyzerCommandLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mainExecuteTibialyzerCommandLabel.Location = new System.Drawing.Point(9, 419);
            this.mainExecuteTibialyzerCommandLabel.Name = "mainExecuteTibialyzerCommandLabel";
            this.mainExecuteTibialyzerCommandLabel.Size = new System.Drawing.Size(523, 30);
            this.mainExecuteTibialyzerCommandLabel.TabIndex = 35;
            this.mainExecuteTibialyzerCommandLabel.Text = "Execute Tibialyzer Command";
            this.mainExecuteTibialyzerCommandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveDamageImageButton
            // 
            this.saveDamageImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveDamageImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveDamageImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveDamageImageButton.Location = new System.Drawing.Point(305, 224);
            this.saveDamageImageButton.Name = "saveDamageImageButton";
            this.saveDamageImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.saveDamageImageButton.Size = new System.Drawing.Size(226, 39);
            this.saveDamageImageButton.TabIndex = 31;
            this.saveDamageImageButton.Text = "Save Damage Image";
            this.saveDamageImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveDamageImageButton.Click += new System.EventHandler(this.damageButton_Click);
            this.saveDamageImageButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.saveDamageImageButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // saveLootImageButton
            // 
            this.saveLootImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveLootImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLootImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveLootImageButton.Location = new System.Drawing.Point(305, 186);
            this.saveLootImageButton.Name = "saveLootImageButton";
            this.saveLootImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.saveLootImageButton.Size = new System.Drawing.Size(226, 38);
            this.saveLootImageButton.TabIndex = 30;
            this.saveLootImageButton.Text = "Save Loot Image";
            this.saveLootImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveLootImageButton.Click += new System.EventHandler(this.saveLootImage_Click);
            this.saveLootImageButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.saveLootImageButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // selectClientProgramButton
            // 
            this.selectClientProgramButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.selectClientProgramButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectClientProgramButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.selectClientProgramButton.Location = new System.Drawing.Point(305, 49);
            this.selectClientProgramButton.Name = "selectClientProgramButton";
            this.selectClientProgramButton.Padding = new System.Windows.Forms.Padding(10);
            this.selectClientProgramButton.Size = new System.Drawing.Size(226, 38);
            this.selectClientProgramButton.TabIndex = 29;
            this.selectClientProgramButton.Text = "Select Tibia Client";
            this.selectClientProgramButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.selectClientProgramButton.Click += new System.EventHandler(this.selectClientButton_Click);
            this.selectClientProgramButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.selectClientProgramButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // mainOptionsLabel
            // 
            this.mainOptionsLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.mainOptionsLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainOptionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mainOptionsLabel.Location = new System.Drawing.Point(305, 19);
            this.mainOptionsLabel.Name = "mainOptionsLabel";
            this.mainOptionsLabel.Size = new System.Drawing.Size(226, 30);
            this.mainOptionsLabel.TabIndex = 28;
            this.mainOptionsLabel.Text = "Global Settings";
            this.mainOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // executeButton
            // 
            this.executeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.executeButton.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.executeButton.Location = new System.Drawing.Point(381, 453);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(150, 23);
            this.executeButton.TabIndex = 27;
            this.executeButton.Text = "Execute Command";
            this.executeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.executeButton.Click += new System.EventHandler(this.executeCommand_Click);
            this.executeButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.executeButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // issuesGuideButton
            // 
            this.issuesGuideButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.issuesGuideButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.issuesGuideButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.issuesGuideButton.Location = new System.Drawing.Point(402, 374);
            this.issuesGuideButton.Name = "issuesGuideButton";
            this.issuesGuideButton.Padding = new System.Windows.Forms.Padding(10);
            this.issuesGuideButton.Size = new System.Drawing.Size(130, 40);
            this.issuesGuideButton.TabIndex = 26;
            this.issuesGuideButton.Text = "Issues";
            this.issuesGuideButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.issuesGuideButton.Click += new System.EventHandler(this.issuesGuide_Click);
            this.issuesGuideButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.issuesGuideButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // popupsGuideButton
            // 
            this.popupsGuideButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.popupsGuideButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupsGuideButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.popupsGuideButton.Location = new System.Drawing.Point(272, 374);
            this.popupsGuideButton.Name = "popupsGuideButton";
            this.popupsGuideButton.Padding = new System.Windows.Forms.Padding(10);
            this.popupsGuideButton.Size = new System.Drawing.Size(130, 40);
            this.popupsGuideButton.TabIndex = 25;
            this.popupsGuideButton.Text = "Popups";
            this.popupsGuideButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.popupsGuideButton.Click += new System.EventHandler(this.popupsGuide_Click);
            this.popupsGuideButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.popupsGuideButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // lootGuideButton
            // 
            this.lootGuideButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.lootGuideButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lootGuideButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.lootGuideButton.Location = new System.Drawing.Point(142, 374);
            this.lootGuideButton.Name = "lootGuideButton";
            this.lootGuideButton.Padding = new System.Windows.Forms.Padding(10);
            this.lootGuideButton.Size = new System.Drawing.Size(130, 40);
            this.lootGuideButton.TabIndex = 24;
            this.lootGuideButton.Text = "Loot";
            this.lootGuideButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lootGuideButton.Click += new System.EventHandler(this.commandsGuide_Click);
            this.lootGuideButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.lootGuideButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // mainGuidesLabel
            // 
            this.mainGuidesLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.mainGuidesLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainGuidesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mainGuidesLabel.Location = new System.Drawing.Point(9, 344);
            this.mainGuidesLabel.Name = "mainGuidesLabel";
            this.mainGuidesLabel.Size = new System.Drawing.Size(523, 30);
            this.mainGuidesLabel.TabIndex = 23;
            this.mainGuidesLabel.Text = "Guides";
            this.mainGuidesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gettingStartedGuideButton
            // 
            this.gettingStartedGuideButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.gettingStartedGuideButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gettingStartedGuideButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.gettingStartedGuideButton.Location = new System.Drawing.Point(9, 374);
            this.gettingStartedGuideButton.Name = "gettingStartedGuideButton";
            this.gettingStartedGuideButton.Padding = new System.Windows.Forms.Padding(10);
            this.gettingStartedGuideButton.Size = new System.Drawing.Size(140, 40);
            this.gettingStartedGuideButton.TabIndex = 22;
            this.gettingStartedGuideButton.Text = "Getting Started";
            this.gettingStartedGuideButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gettingStartedGuideButton.Click += new System.EventHandler(this.gettingStartedGuide_Click);
            this.gettingStartedGuideButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.gettingStartedGuideButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // nameListBox
            // 
            this.nameListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.nameListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.nameListBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.nameListBox.FormattingEnabled = true;
            this.nameListBox.ItemHeight = 20;
            this.nameListBox.Items.AddRange(new object[] {
            "Mytherin",
            "Robin Razend",
            "Scheirke"});
            this.nameListBox.Location = new System.Drawing.Point(3, 49);
            this.nameListBox.Name = "nameListBox";
            this.nameListBox.Size = new System.Drawing.Size(296, 115);
            this.nameListBox.TabIndex = 0;
            // 
            // namesLabel
            // 
            this.namesLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.namesLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.namesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.namesLabel.Location = new System.Drawing.Point(3, 19);
            this.namesLabel.Name = "namesLabel";
            this.namesLabel.Size = new System.Drawing.Size(296, 30);
            this.namesLabel.TabIndex = 1;
            this.namesLabel.Text = "Character Names";
            this.namesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // commandTextBox
            // 
            this.commandTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.commandTextBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.commandTextBox.Location = new System.Drawing.Point(9, 453);
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.Size = new System.Drawing.Size(372, 23);
            this.commandTextBox.TabIndex = 9;
            this.commandTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.commandTextBox_KeyPress);
            this.commandTextBox.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.commandTextBox.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // MainTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.Controls.Add(this.detectFlashClientButton);
            this.Controls.Add(this.saveAllLootCheckbox);
            this.Controls.Add(this.expValueLabel);
            this.Controls.Add(this.ignoreLowExperienceBox);
            this.Controls.Add(this.ignoreLowExperienceButton);
            this.Controls.Add(this.stackAllItemsCheckbox);
            this.Controls.Add(this.lootOptionsHeaderLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.mainExecuteTibialyzerCommandLabel);
            this.Controls.Add(this.saveDamageImageButton);
            this.Controls.Add(this.saveLootImageButton);
            this.Controls.Add(this.selectClientProgramButton);
            this.Controls.Add(this.mainOptionsLabel);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.issuesGuideButton);
            this.Controls.Add(this.popupsGuideButton);
            this.Controls.Add(this.lootGuideButton);
            this.Controls.Add(this.mainGuidesLabel);
            this.Controls.Add(this.gettingStartedGuideButton);
            this.Controls.Add(this.nameListBox);
            this.Controls.Add(this.namesLabel);
            this.Controls.Add(this.commandTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label detectFlashClientButton;
        private PrettyCheckBox saveAllLootCheckbox;
        private System.Windows.Forms.Label expValueLabel;
        private EnterTextBox ignoreLowExperienceBox;
        private PrettyCheckBox ignoreLowExperienceButton;
        private PrettyCheckBox stackAllItemsCheckbox;
        private System.Windows.Forms.Label lootOptionsHeaderLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label mainExecuteTibialyzerCommandLabel;
        private System.Windows.Forms.Label saveDamageImageButton;
        private System.Windows.Forms.Label saveLootImageButton;
        private System.Windows.Forms.Label selectClientProgramButton;
        private System.Windows.Forms.Label mainOptionsLabel;
        private System.Windows.Forms.Label executeButton;
        private System.Windows.Forms.Label issuesGuideButton;
        private System.Windows.Forms.Label popupsGuideButton;
        private System.Windows.Forms.Label lootGuideButton;
        private System.Windows.Forms.Label mainGuidesLabel;
        private System.Windows.Forms.Label gettingStartedGuideButton;
        private PrettyListBox nameListBox;
        private System.Windows.Forms.Label namesLabel;
        private EnterTextBox commandTextBox;
        #endregion
    }
}