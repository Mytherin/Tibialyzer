namespace Tibialyzer {
    partial class HuntsTab {
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
            this.lootDisplayHeader = new Tibialyzer.PrettyHeader();
            this.showHuntLootButton = new Tibialyzer.PrettyButton();
            this.creatureListHeader = new Tibialyzer.PrettyHeader();
            this.creatureImagePanel = new System.Windows.Forms.Panel();
            this.trackedCreaturesHeader = new Tibialyzer.PrettyHeader();
            this.setAsActiveHuntButton = new Tibialyzer.PrettyButton();
            this.huntOptionsHeader = new Tibialyzer.PrettyHeader();
            this.listOfHuntsHeader = new Tibialyzer.PrettyHeader();
            this.trackedCreatureList = new Tibialyzer.PrettyListBox();
            this.gatherTrackedKillsCheckbox = new Tibialyzer.PrettyCheckBox();
            this.switchOnKillCheckbox = new Tibialyzer.PrettyCheckBox();
            this.displayAllCreaturesCheckbox = new Tibialyzer.PrettyCheckBox();
            this.clearHuntOnStartupCheckbox = new Tibialyzer.PrettyCheckBox();
            this.huntList = new Tibialyzer.PrettyListBox();
            this.lootDisplayOptionsHeader = new Tibialyzer.PrettyHeader();
            this.automaticallyWriteLootToFileCheckbox = new Tibialyzer.PrettyCheckBox();
            this.ignoreLowExperienceCheckbox = new Tibialyzer.PrettyCheckBox();
            this.displayAllItemsAsStackableCheckbox = new Tibialyzer.PrettyCheckBox();
            this.expValueLabel = new System.Windows.Forms.Label();
            this.ignoreLowExperienceBox = new Tibialyzer.EnterTextBox();
            this.SuspendLayout();
            // 
            // lootDisplayHeader
            // 
            this.lootDisplayHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lootDisplayHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lootDisplayHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lootDisplayHeader.Location = new System.Drawing.Point(327, 126);
            this.lootDisplayHeader.Name = "lootDisplayHeader";
            this.lootDisplayHeader.Size = new System.Drawing.Size(299, 30);
            this.lootDisplayHeader.TabIndex = 42;
            this.lootDisplayHeader.Text = "Loot Display";
            this.lootDisplayHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showHuntLootButton
            // 
            this.showHuntLootButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.showHuntLootButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showHuntLootButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.showHuntLootButton.Location = new System.Drawing.Point(327, 156);
            this.showHuntLootButton.Name = "showHuntLootButton";
            this.showHuntLootButton.Padding = new System.Windows.Forms.Padding(10);
            this.showHuntLootButton.Size = new System.Drawing.Size(299, 38);
            this.showHuntLootButton.TabIndex = 41;
            this.showHuntLootButton.Text = "Show Loot";
            this.showHuntLootButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showHuntLootButton.Click += new System.EventHandler(this.showLootButton_Click);
            // 
            // creatureListHeader
            // 
            this.creatureListHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.creatureListHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureListHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.creatureListHeader.Location = new System.Drawing.Point(327, 202);
            this.creatureListHeader.Name = "creatureListHeader";
            this.creatureListHeader.Size = new System.Drawing.Size(299, 30);
            this.creatureListHeader.TabIndex = 39;
            this.creatureListHeader.Text = "Creature List";
            this.creatureListHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // creatureImagePanel
            // 
            this.creatureImagePanel.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.creatureImagePanel.Location = new System.Drawing.Point(327, 337);
            this.creatureImagePanel.Name = "creatureImagePanel";
            this.creatureImagePanel.Size = new System.Drawing.Size(299, 200);
            this.creatureImagePanel.TabIndex = 16;
            // 
            // trackedCreaturesHeader
            // 
            this.trackedCreaturesHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.trackedCreaturesHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackedCreaturesHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.trackedCreaturesHeader.Location = new System.Drawing.Point(9, 202);
            this.trackedCreaturesHeader.Name = "trackedCreaturesHeader";
            this.trackedCreaturesHeader.Size = new System.Drawing.Size(299, 30);
            this.trackedCreaturesHeader.TabIndex = 35;
            this.trackedCreaturesHeader.Text = "Tracked Creatures";
            this.trackedCreaturesHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // setAsActiveHuntButton
            // 
            this.setAsActiveHuntButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.setAsActiveHuntButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setAsActiveHuntButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.setAsActiveHuntButton.Location = new System.Drawing.Point(327, 39);
            this.setAsActiveHuntButton.Name = "setAsActiveHuntButton";
            this.setAsActiveHuntButton.Padding = new System.Windows.Forms.Padding(10);
            this.setAsActiveHuntButton.Size = new System.Drawing.Size(299, 38);
            this.setAsActiveHuntButton.TabIndex = 34;
            this.setAsActiveHuntButton.Text = "Set As Active Hunt";
            this.setAsActiveHuntButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.setAsActiveHuntButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.activeHuntButton_Click);
            // 
            // huntOptionsHeader
            // 
            this.huntOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.huntOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.huntOptionsHeader.Location = new System.Drawing.Point(327, 9);
            this.huntOptionsHeader.Name = "huntOptionsHeader";
            this.huntOptionsHeader.Size = new System.Drawing.Size(299, 30);
            this.huntOptionsHeader.TabIndex = 32;
            this.huntOptionsHeader.Text = "Options";
            this.huntOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listOfHuntsHeader
            // 
            this.listOfHuntsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.listOfHuntsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listOfHuntsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.listOfHuntsHeader.Location = new System.Drawing.Point(9, 9);
            this.listOfHuntsHeader.Name = "listOfHuntsHeader";
            this.listOfHuntsHeader.Size = new System.Drawing.Size(299, 30);
            this.listOfHuntsHeader.TabIndex = 3;
            this.listOfHuntsHeader.Text = "List of Hunts";
            this.listOfHuntsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackedCreatureList
            // 
            this.trackedCreatureList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.trackedCreatureList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trackedCreatureList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.trackedCreatureList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackedCreatureList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.trackedCreatureList.FormattingEnabled = true;
            this.trackedCreatureList.ItemHeight = 20;
            this.trackedCreatureList.Items.AddRange(new object[] {
            ""});
            this.trackedCreatureList.Location = new System.Drawing.Point(327, 232);
            this.trackedCreatureList.Name = "trackedCreatureList";
            this.trackedCreatureList.Size = new System.Drawing.Size(299, 95);
            this.trackedCreatureList.TabIndex = 40;
            // 
            // gatherTrackedKillsCheckbox
            // 
            this.gatherTrackedKillsCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.gatherTrackedKillsCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gatherTrackedKillsCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.gatherTrackedKillsCheckbox.Location = new System.Drawing.Point(9, 312);
            this.gatherTrackedKillsCheckbox.Name = "gatherTrackedKillsCheckbox";
            this.gatherTrackedKillsCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.gatherTrackedKillsCheckbox.Size = new System.Drawing.Size(299, 40);
            this.gatherTrackedKillsCheckbox.TabIndex = 38;
            this.gatherTrackedKillsCheckbox.Text = "Always add tracked creature to hunt";
            this.gatherTrackedKillsCheckbox.UseVisualStyleBackColor = false;
            this.gatherTrackedKillsCheckbox.CheckedChanged += new System.EventHandler(this.aggregateHuntBox_CheckedChanged);
            // 
            // switchOnKillCheckbox
            // 
            this.switchOnKillCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.switchOnKillCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.switchOnKillCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.switchOnKillCheckbox.Location = new System.Drawing.Point(9, 272);
            this.switchOnKillCheckbox.Name = "switchOnKillCheckbox";
            this.switchOnKillCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.switchOnKillCheckbox.Size = new System.Drawing.Size(299, 40);
            this.switchOnKillCheckbox.TabIndex = 37;
            this.switchOnKillCheckbox.Text = "Activate if tracked creature is killed";
            this.switchOnKillCheckbox.UseVisualStyleBackColor = false;
            this.switchOnKillCheckbox.CheckedChanged += new System.EventHandler(this.sideHuntBox_CheckedChanged);
            // 
            // displayAllCreaturesCheckbox
            // 
            this.displayAllCreaturesCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayAllCreaturesCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayAllCreaturesCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayAllCreaturesCheckbox.Location = new System.Drawing.Point(9, 232);
            this.displayAllCreaturesCheckbox.Name = "displayAllCreaturesCheckbox";
            this.displayAllCreaturesCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.displayAllCreaturesCheckbox.Size = new System.Drawing.Size(299, 40);
            this.displayAllCreaturesCheckbox.TabIndex = 36;
            this.displayAllCreaturesCheckbox.Text = "Display All Creatures";
            this.displayAllCreaturesCheckbox.UseVisualStyleBackColor = false;
            this.displayAllCreaturesCheckbox.CheckedChanged += new System.EventHandler(this.trackCreaturesCheckbox_CheckedChanged);
            // 
            // clearHuntOnStartupCheckbox
            // 
            this.clearHuntOnStartupCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.clearHuntOnStartupCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearHuntOnStartupCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.clearHuntOnStartupCheckbox.Location = new System.Drawing.Point(327, 77);
            this.clearHuntOnStartupCheckbox.Name = "clearHuntOnStartupCheckbox";
            this.clearHuntOnStartupCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.clearHuntOnStartupCheckbox.Size = new System.Drawing.Size(299, 40);
            this.clearHuntOnStartupCheckbox.TabIndex = 31;
            this.clearHuntOnStartupCheckbox.Text = "Clear Hunt On Startup";
            this.clearHuntOnStartupCheckbox.UseVisualStyleBackColor = false;
            this.clearHuntOnStartupCheckbox.CheckedChanged += new System.EventHandler(this.startupHuntCheckbox_CheckedChanged);
            // 
            // huntList
            // 
            this.huntList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.huntList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.huntList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.huntList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.huntList.FormattingEnabled = true;
            this.huntList.ItemHeight = 20;
            this.huntList.Items.AddRange(new object[] {
            "Main Hunt"});
            this.huntList.Location = new System.Drawing.Point(9, 39);
            this.huntList.Name = "huntList";
            this.huntList.Size = new System.Drawing.Size(299, 155);
            this.huntList.TabIndex = 2;
            this.huntList.SelectedIndexChanged += new System.EventHandler(this.huntBox_SelectedIndexChanged);
            // 
            // lootDisplayOptionsHeader
            // 
            this.lootDisplayOptionsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lootDisplayOptionsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lootDisplayOptionsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lootDisplayOptionsHeader.Location = new System.Drawing.Point(9, 359);
            this.lootDisplayOptionsHeader.Name = "lootDisplayOptionsHeader";
            this.lootDisplayOptionsHeader.Size = new System.Drawing.Size(299, 30);
            this.lootDisplayOptionsHeader.TabIndex = 43;
            this.lootDisplayOptionsHeader.Text = "Loot Display Options";
            this.lootDisplayOptionsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // automaticallyWriteLootToFileCheckbox
            // 
            this.automaticallyWriteLootToFileCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.automaticallyWriteLootToFileCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.automaticallyWriteLootToFileCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.automaticallyWriteLootToFileCheckbox.Location = new System.Drawing.Point(9, 488);
            this.automaticallyWriteLootToFileCheckbox.Name = "automaticallyWriteLootToFileCheckbox";
            this.automaticallyWriteLootToFileCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.automaticallyWriteLootToFileCheckbox.Size = new System.Drawing.Size(299, 38);
            this.automaticallyWriteLootToFileCheckbox.TabIndex = 46;
            this.automaticallyWriteLootToFileCheckbox.Text = "Automatically Write Loot To File";
            this.automaticallyWriteLootToFileCheckbox.UseVisualStyleBackColor = false;
            this.automaticallyWriteLootToFileCheckbox.CheckedChanged += new System.EventHandler(this.automaticallyWriteLootToFileCheckbox_CheckedChanged);
            // 
            // ignoreLowExperienceCheckbox
            // 
            this.ignoreLowExperienceCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.ignoreLowExperienceCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ignoreLowExperienceCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.ignoreLowExperienceCheckbox.Location = new System.Drawing.Point(9, 427);
            this.ignoreLowExperienceCheckbox.Name = "ignoreLowExperienceCheckbox";
            this.ignoreLowExperienceCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.ignoreLowExperienceCheckbox.Size = new System.Drawing.Size(299, 38);
            this.ignoreLowExperienceCheckbox.TabIndex = 45;
            this.ignoreLowExperienceCheckbox.Text = "Ignore Low Experience Creatures";
            this.ignoreLowExperienceCheckbox.UseVisualStyleBackColor = false;
            this.ignoreLowExperienceCheckbox.CheckedChanged += new System.EventHandler(this.ignoreLowExperienceCheckbox_CheckedChanged);
            // 
            // displayAllItemsAsStackableCheckbox
            // 
            this.displayAllItemsAsStackableCheckbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayAllItemsAsStackableCheckbox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayAllItemsAsStackableCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayAllItemsAsStackableCheckbox.Location = new System.Drawing.Point(9, 389);
            this.displayAllItemsAsStackableCheckbox.Name = "displayAllItemsAsStackableCheckbox";
            this.displayAllItemsAsStackableCheckbox.Padding = new System.Windows.Forms.Padding(10);
            this.displayAllItemsAsStackableCheckbox.Size = new System.Drawing.Size(299, 38);
            this.displayAllItemsAsStackableCheckbox.TabIndex = 44;
            this.displayAllItemsAsStackableCheckbox.Text = "Display All Items As Stackable";
            this.displayAllItemsAsStackableCheckbox.UseVisualStyleBackColor = false;
            this.displayAllItemsAsStackableCheckbox.CheckedChanged += new System.EventHandler(this.displayAllItemsAsStackableCheckbox_CheckedChanged);
            // 
            // expValueLabel
            // 
            this.expValueLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.expValueLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.expValueLabel.Location = new System.Drawing.Point(9, 465);
            this.expValueLabel.Name = "expValueLabel";
            this.expValueLabel.Size = new System.Drawing.Size(109, 23);
            this.expValueLabel.TabIndex = 64;
            this.expValueLabel.Text = "Exp Value";
            this.expValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ignoreLowExperienceBox
            // 
            this.ignoreLowExperienceBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.ignoreLowExperienceBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ignoreLowExperienceBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.ignoreLowExperienceBox.Location = new System.Drawing.Point(117, 465);
            this.ignoreLowExperienceBox.Name = "ignoreLowExperienceBox";
            this.ignoreLowExperienceBox.Size = new System.Drawing.Size(191, 23);
            this.ignoreLowExperienceBox.TabIndex = 63;
            this.ignoreLowExperienceBox.Text = "50000";
            this.ignoreLowExperienceBox.TextChanged += new System.EventHandler(this.ignoreLowExperienceBox_TextChanged);
            // 
            // HuntsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.expValueLabel);
            this.Controls.Add(this.ignoreLowExperienceBox);
            this.Controls.Add(this.automaticallyWriteLootToFileCheckbox);
            this.Controls.Add(this.ignoreLowExperienceCheckbox);
            this.Controls.Add(this.displayAllItemsAsStackableCheckbox);
            this.Controls.Add(this.lootDisplayOptionsHeader);
            this.Controls.Add(this.lootDisplayHeader);
            this.Controls.Add(this.showHuntLootButton);
            this.Controls.Add(this.creatureListHeader);
            this.Controls.Add(this.creatureImagePanel);
            this.Controls.Add(this.trackedCreaturesHeader);
            this.Controls.Add(this.setAsActiveHuntButton);
            this.Controls.Add(this.huntOptionsHeader);
            this.Controls.Add(this.listOfHuntsHeader);
            this.Controls.Add(this.trackedCreatureList);
            this.Controls.Add(this.gatherTrackedKillsCheckbox);
            this.Controls.Add(this.switchOnKillCheckbox);
            this.Controls.Add(this.displayAllCreaturesCheckbox);
            this.Controls.Add(this.clearHuntOnStartupCheckbox);
            this.Controls.Add(this.huntList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HuntsTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Panel creatureImagePanel;
        private PrettyListBox trackedCreatureList;
        private PrettyCheckBox gatherTrackedKillsCheckbox;
        private PrettyCheckBox switchOnKillCheckbox;
        private PrettyCheckBox displayAllCreaturesCheckbox;
        private PrettyCheckBox clearHuntOnStartupCheckbox;
        private PrettyListBox huntList;
        #endregion
        private PrettyCheckBox automaticallyWriteLootToFileCheckbox;
        private PrettyCheckBox ignoreLowExperienceCheckbox;
        private PrettyCheckBox displayAllItemsAsStackableCheckbox;
        private System.Windows.Forms.Label expValueLabel;
        private EnterTextBox ignoreLowExperienceBox;
        private PrettyHeader lootDisplayOptionsHeader;
        private PrettyHeader listOfHuntsHeader;
        private PrettyHeader huntOptionsHeader;
        private PrettyButton setAsActiveHuntButton;
        private PrettyHeader trackedCreaturesHeader;
        private PrettyHeader creatureListHeader;
        private PrettyButton showHuntLootButton;
        private PrettyHeader lootDisplayHeader;
    }
}