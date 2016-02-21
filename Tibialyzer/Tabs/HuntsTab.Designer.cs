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
            this.SuspendLayout();

            ///
            /// lootDisplayHeader
            ///
            this.lootDisplayHeader = new System.Windows.Forms.Label();
            this.lootDisplayHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.lootDisplayHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lootDisplayHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.lootDisplayHeader.Location = new System.Drawing.Point(300, 136);
            this.lootDisplayHeader.Name = "lootDisplayHeader";
            this.lootDisplayHeader.Size = new System.Drawing.Size(238, 30);
            this.lootDisplayHeader.TabIndex = 42;
            this.lootDisplayHeader.Text = "Loot Display";
            this.lootDisplayHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// showHuntLootButton
            ///
            this.showHuntLootButton = new System.Windows.Forms.Label();
            this.showHuntLootButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.showHuntLootButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showHuntLootButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.showHuntLootButton.Location = new System.Drawing.Point(300, 166);
            this.showHuntLootButton.Name = "showHuntLootButton";
            this.showHuntLootButton.Padding = new System.Windows.Forms.Padding(10);
            this.showHuntLootButton.Size = new System.Drawing.Size(238, 38);
            this.showHuntLootButton.TabIndex = 41;
            this.showHuntLootButton.Text = "Show Loot";
            this.showHuntLootButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showHuntLootButton.Click += new System.EventHandler(this.showLootButton_Click);
            this.showHuntLootButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.showHuntLootButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// label35
            ///
            this.label35 = new System.Windows.Forms.Label();
            this.label35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label35.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label35.Location = new System.Drawing.Point(299, 353);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(238, 30);
            this.label35.TabIndex = 39;
            this.label35.Text = "Creature List";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// creatureImagePanel
            ///
            this.creatureImagePanel = new System.Windows.Forms.Panel();
            this.creatureImagePanel.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.creatureImagePanel.Location = new System.Drawing.Point(6, 384);
            this.creatureImagePanel.Name = "creatureImagePanel";
            this.creatureImagePanel.Size = new System.Drawing.Size(287, 94);
            this.creatureImagePanel.TabIndex = 16;
            ///
            /// label34
            ///
            this.label34 = new System.Windows.Forms.Label();
            this.label34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label34.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label34.Location = new System.Drawing.Point(3, 228);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(290, 30);
            this.label34.TabIndex = 35;
            this.label34.Text = "Tracked Creatures";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// setActiveHuntButton
            ///
            this.setActiveHuntButton = new System.Windows.Forms.Label();
            this.setActiveHuntButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.setActiveHuntButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setActiveHuntButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.setActiveHuntButton.Location = new System.Drawing.Point(299, 49);
            this.setActiveHuntButton.Name = "setActiveHuntButton";
            this.setActiveHuntButton.Padding = new System.Windows.Forms.Padding(10);
            this.setActiveHuntButton.Size = new System.Drawing.Size(238, 38);
            this.setActiveHuntButton.TabIndex = 34;
            this.setActiveHuntButton.Text = "Set As Active Hunt";
            this.setActiveHuntButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.setActiveHuntButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.activeHuntButton_Click);
            this.setActiveHuntButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.setActiveHuntButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// label32
            ///
            this.label32 = new System.Windows.Forms.Label();
            this.label32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label32.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label32.Location = new System.Drawing.Point(299, 19);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(238, 30);
            this.label32.TabIndex = 32;
            this.label32.Text = "Options";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// huntListLabel
            ///
            this.huntListLabel = new System.Windows.Forms.Label();
            this.huntListLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.huntListLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.huntListLabel.Location = new System.Drawing.Point(3, 19);
            this.huntListLabel.Name = "huntListLabel";
            this.huntListLabel.Size = new System.Drawing.Size(290, 30);
            this.huntListLabel.TabIndex = 3;
            this.huntListLabel.Text = "List of Hunts";
            this.huntListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// trackedCreatureList
            ///
            this.trackedCreatureList = new Tibialyzer.PrettyListBox();
            this.trackedCreatureList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.trackedCreatureList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trackedCreatureList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.trackedCreatureList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackedCreatureList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.trackedCreatureList.FormattingEnabled = true;
            this.trackedCreatureList.ItemHeight = 20;
            this.trackedCreatureList.Items.AddRange(new object[] {
            ""});
            this.trackedCreatureList.Location = new System.Drawing.Point(299, 383);
            this.trackedCreatureList.Name = "trackedCreatureList";
            this.trackedCreatureList.Size = new System.Drawing.Size(238, 95);
            this.trackedCreatureList.TabIndex = 40;
            ///
            /// gatherTrackedKillsBox
            ///
            this.gatherTrackedKillsBox = new Tibialyzer.PrettyCheckBox();
            this.gatherTrackedKillsBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.gatherTrackedKillsBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gatherTrackedKillsBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.gatherTrackedKillsBox.Location = new System.Drawing.Point(3, 338);
            this.gatherTrackedKillsBox.Name = "gatherTrackedKillsBox";
            this.gatherTrackedKillsBox.Padding = new System.Windows.Forms.Padding(10);
            this.gatherTrackedKillsBox.Size = new System.Drawing.Size(290, 40);
            this.gatherTrackedKillsBox.TabIndex = 38;
            this.gatherTrackedKillsBox.Text = "Always add tracked creature to hunt";
            this.gatherTrackedKillsBox.UseVisualStyleBackColor = false;
            this.gatherTrackedKillsBox.CheckedChanged += new System.EventHandler(this.aggregateHuntBox_CheckedChanged);
            this.gatherTrackedKillsBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.gatherTrackedKillsBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// switchOnKillBox
            ///
            this.switchOnKillBox = new Tibialyzer.PrettyCheckBox();
            this.switchOnKillBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.switchOnKillBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.switchOnKillBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.switchOnKillBox.Location = new System.Drawing.Point(3, 298);
            this.switchOnKillBox.Name = "switchOnKillBox";
            this.switchOnKillBox.Padding = new System.Windows.Forms.Padding(10);
            this.switchOnKillBox.Size = new System.Drawing.Size(290, 40);
            this.switchOnKillBox.TabIndex = 37;
            this.switchOnKillBox.Text = "Activate if tracked creature is killed";
            this.switchOnKillBox.UseVisualStyleBackColor = false;
            this.switchOnKillBox.CheckedChanged += new System.EventHandler(this.sideHuntBox_CheckedChanged);
            this.switchOnKillBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.switchOnKillBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// displayAllCreaturesBox
            ///
            this.displayAllCreaturesBox = new Tibialyzer.PrettyCheckBox();
            this.displayAllCreaturesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.displayAllCreaturesBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayAllCreaturesBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.displayAllCreaturesBox.Location = new System.Drawing.Point(3, 258);
            this.displayAllCreaturesBox.Name = "displayAllCreaturesBox";
            this.displayAllCreaturesBox.Padding = new System.Windows.Forms.Padding(10);
            this.displayAllCreaturesBox.Size = new System.Drawing.Size(290, 40);
            this.displayAllCreaturesBox.TabIndex = 36;
            this.displayAllCreaturesBox.Text = "Display All Creatures";
            this.displayAllCreaturesBox.UseVisualStyleBackColor = false;
            this.displayAllCreaturesBox.CheckedChanged += new System.EventHandler(this.trackCreaturesCheckbox_CheckedChanged);
            this.displayAllCreaturesBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.displayAllCreaturesBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// clearHuntOnStartupBox
            ///
            this.clearHuntOnStartupBox = new Tibialyzer.PrettyCheckBox();
            this.clearHuntOnStartupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.clearHuntOnStartupBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearHuntOnStartupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.clearHuntOnStartupBox.Location = new System.Drawing.Point(299, 87);
            this.clearHuntOnStartupBox.Name = "clearHuntOnStartupBox";
            this.clearHuntOnStartupBox.Padding = new System.Windows.Forms.Padding(10);
            this.clearHuntOnStartupBox.Size = new System.Drawing.Size(238, 40);
            this.clearHuntOnStartupBox.TabIndex = 31;
            this.clearHuntOnStartupBox.Text = "Clear Hunt On Startup";
            this.clearHuntOnStartupBox.UseVisualStyleBackColor = false;
            this.clearHuntOnStartupBox.CheckedChanged += new System.EventHandler(this.startupHuntCheckbox_CheckedChanged);
            this.clearHuntOnStartupBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.clearHuntOnStartupBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// huntList
            ///
            this.huntList = new Tibialyzer.PrettyListBox();
            this.huntList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.huntList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.huntList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.huntList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.huntList.FormattingEnabled = true;
            this.huntList.ItemHeight = 20;
            this.huntList.Items.AddRange(new object[] {
            "Main Hunt"});
            this.huntList.Location = new System.Drawing.Point(3, 49);
            this.huntList.Name = "huntList";
            this.huntList.Size = new System.Drawing.Size(290, 155);
            this.huntList.TabIndex = 2;
            this.huntList.SelectedIndexChanged += new System.EventHandler(this.huntBox_SelectedIndexChanged);
            //
            // TabBase
            //
            this.Controls.Add(lootDisplayHeader);
            this.Controls.Add(showHuntLootButton);
            this.Controls.Add(label35);
            this.Controls.Add(creatureImagePanel);
            this.Controls.Add(label34);
            this.Controls.Add(setActiveHuntButton);
            this.Controls.Add(label32);
            this.Controls.Add(huntListLabel);
            this.Controls.Add(trackedCreatureList);
            this.Controls.Add(gatherTrackedKillsBox);
            this.Controls.Add(switchOnKillBox);
            this.Controls.Add(displayAllCreaturesBox);
            this.Controls.Add(clearHuntOnStartupBox);
            this.Controls.Add(huntList);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabBase";
            this.Text = "TabBase";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lootDisplayHeader;
        private System.Windows.Forms.Label showHuntLootButton;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Panel creatureImagePanel;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label setActiveHuntButton;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label huntListLabel;
        private PrettyListBox trackedCreatureList;
        private PrettyCheckBox gatherTrackedKillsBox;
        private PrettyCheckBox switchOnKillBox;
        private PrettyCheckBox displayAllCreaturesBox;
        private PrettyCheckBox clearHuntOnStartupBox;
        private PrettyListBox huntList;
        #endregion
    }
}