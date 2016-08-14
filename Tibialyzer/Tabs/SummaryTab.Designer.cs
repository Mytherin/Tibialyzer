namespace Tibialyzer {
    partial class SummaryTab {
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
            this.maxItemsDisplayedHeader = new Tibialyzer.PrettyHeader();
            this.maxItemsDisplayedTrack = new System.Windows.Forms.TrackBar();
            this.maxCreatureKillsTrack = new System.Windows.Forms.TrackBar();
            this.maxCreatureKillsHeader = new Tibialyzer.PrettyHeader();
            this.maxRecentDropsTrack = new System.Windows.Forms.TrackBar();
            this.maxRecentDropsHeader = new Tibialyzer.PrettyHeader();
            this.maxDamageEntryTrack = new System.Windows.Forms.TrackBar();
            this.maxDamageEntryHeader = new Tibialyzer.PrettyHeader();
            this.maxUsedItemsTrack = new System.Windows.Forms.TrackBar();
            this.maxUsedItemsHeader = new Tibialyzer.PrettyHeader();
            this.itemDropsImageSizeHeader = new Tibialyzer.PrettyHeader();
            this.lootImageSizeBar = new System.Windows.Forms.TrackBar();
            this.recentDropsImageSizeBar = new System.Windows.Forms.TrackBar();
            this.recentDropsImageSizeHeader = new Tibialyzer.PrettyHeader();
            this.wasteImageSizeBar = new System.Windows.Forms.TrackBar();
            this.wasteImageSizeHeader = new Tibialyzer.PrettyHeader();
            ((System.ComponentModel.ISupportInitialize)(this.maxItemsDisplayedTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCreatureKillsTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRecentDropsTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxDamageEntryTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxUsedItemsTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lootImageSizeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recentDropsImageSizeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wasteImageSizeBar)).BeginInit();
            this.SuspendLayout();
            // 
            // maxItemsDisplayedHeader
            // 
            this.maxItemsDisplayedHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.maxItemsDisplayedHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxItemsDisplayedHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.maxItemsDisplayedHeader.Location = new System.Drawing.Point(9, 9);
            this.maxItemsDisplayedHeader.Name = "maxItemsDisplayedHeader";
            this.maxItemsDisplayedHeader.Size = new System.Drawing.Size(299, 30);
            this.maxItemsDisplayedHeader.TabIndex = 30;
            this.maxItemsDisplayedHeader.Text = "Max # of Item Drops";
            this.maxItemsDisplayedHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxItemsDisplayedTrack
            // 
            this.maxItemsDisplayedTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.maxItemsDisplayedTrack.Location = new System.Drawing.Point(9, 39);
            this.maxItemsDisplayedTrack.Maximum = 20;
            this.maxItemsDisplayedTrack.Name = "maxItemsDisplayedTrack";
            this.maxItemsDisplayedTrack.Size = new System.Drawing.Size(299, 45);
            this.maxItemsDisplayedTrack.TabIndex = 31;
            this.maxItemsDisplayedTrack.Value = 5;
            this.maxItemsDisplayedTrack.Scroll += new System.EventHandler(this.maxItemsDisplayedTrack_Scroll);
            // 
            // maxCreatureKillsTrack
            // 
            this.maxCreatureKillsTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.maxCreatureKillsTrack.Location = new System.Drawing.Point(9, 116);
            this.maxCreatureKillsTrack.Maximum = 20;
            this.maxCreatureKillsTrack.Name = "maxCreatureKillsTrack";
            this.maxCreatureKillsTrack.Size = new System.Drawing.Size(299, 45);
            this.maxCreatureKillsTrack.TabIndex = 33;
            this.maxCreatureKillsTrack.Value = 5;
            this.maxCreatureKillsTrack.Scroll += new System.EventHandler(this.maxCreatureKillsTrack_Scroll);
            // 
            // maxCreatureKillsHeader
            // 
            this.maxCreatureKillsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.maxCreatureKillsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxCreatureKillsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.maxCreatureKillsHeader.Location = new System.Drawing.Point(9, 86);
            this.maxCreatureKillsHeader.Name = "maxCreatureKillsHeader";
            this.maxCreatureKillsHeader.Size = new System.Drawing.Size(299, 30);
            this.maxCreatureKillsHeader.TabIndex = 32;
            this.maxCreatureKillsHeader.Text = "Max # of Creature Kills";
            this.maxCreatureKillsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxRecentDropsTrack
            // 
            this.maxRecentDropsTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.maxRecentDropsTrack.Location = new System.Drawing.Point(9, 193);
            this.maxRecentDropsTrack.Maximum = 20;
            this.maxRecentDropsTrack.Name = "maxRecentDropsTrack";
            this.maxRecentDropsTrack.Size = new System.Drawing.Size(299, 45);
            this.maxRecentDropsTrack.TabIndex = 35;
            this.maxRecentDropsTrack.Value = 5;
            this.maxRecentDropsTrack.Scroll += new System.EventHandler(this.maxRecentDropsTrack_Scroll);
            // 
            // maxRecentDropsHeader
            // 
            this.maxRecentDropsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.maxRecentDropsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxRecentDropsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.maxRecentDropsHeader.Location = new System.Drawing.Point(9, 163);
            this.maxRecentDropsHeader.Name = "maxRecentDropsHeader";
            this.maxRecentDropsHeader.Size = new System.Drawing.Size(299, 30);
            this.maxRecentDropsHeader.TabIndex = 34;
            this.maxRecentDropsHeader.Text = "Max # of Recent Drops";
            this.maxRecentDropsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxDamageEntryTrack
            // 
            this.maxDamageEntryTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.maxDamageEntryTrack.Location = new System.Drawing.Point(9, 270);
            this.maxDamageEntryTrack.Maximum = 20;
            this.maxDamageEntryTrack.Name = "maxDamageEntryTrack";
            this.maxDamageEntryTrack.Size = new System.Drawing.Size(299, 45);
            this.maxDamageEntryTrack.TabIndex = 37;
            this.maxDamageEntryTrack.Value = 5;
            this.maxDamageEntryTrack.Scroll += new System.EventHandler(this.maxDamageEntryTrack_Scroll);
            // 
            // maxDamageEntryHeader
            // 
            this.maxDamageEntryHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.maxDamageEntryHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxDamageEntryHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.maxDamageEntryHeader.Location = new System.Drawing.Point(9, 240);
            this.maxDamageEntryHeader.Name = "maxDamageEntryHeader";
            this.maxDamageEntryHeader.Size = new System.Drawing.Size(299, 30);
            this.maxDamageEntryHeader.TabIndex = 36;
            this.maxDamageEntryHeader.Text = "Max # of Damage Entries";
            this.maxDamageEntryHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // maxUsedItemsTrack
            // 
            this.maxUsedItemsTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.maxUsedItemsTrack.Location = new System.Drawing.Point(9, 347);
            this.maxUsedItemsTrack.Maximum = 20;
            this.maxUsedItemsTrack.Name = "maxUsedItemsTrack";
            this.maxUsedItemsTrack.Size = new System.Drawing.Size(299, 45);
            this.maxUsedItemsTrack.TabIndex = 39;
            this.maxUsedItemsTrack.Value = 5;
            this.maxUsedItemsTrack.Scroll += new System.EventHandler(this.maxUsedItemsTrack_Scroll);
            // 
            // maxUsedItemsHeader
            // 
            this.maxUsedItemsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.maxUsedItemsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxUsedItemsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.maxUsedItemsHeader.Location = new System.Drawing.Point(9, 317);
            this.maxUsedItemsHeader.Name = "maxUsedItemsHeader";
            this.maxUsedItemsHeader.Size = new System.Drawing.Size(299, 30);
            this.maxUsedItemsHeader.TabIndex = 38;
            this.maxUsedItemsHeader.Text = "Max # of Used Items";
            this.maxUsedItemsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // itemDropsImageSizeHeader
            // 
            this.itemDropsImageSizeHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.itemDropsImageSizeHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemDropsImageSizeHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.itemDropsImageSizeHeader.Location = new System.Drawing.Point(327, 9);
            this.itemDropsImageSizeHeader.Name = "itemDropsImageSizeHeader";
            this.itemDropsImageSizeHeader.Size = new System.Drawing.Size(299, 30);
            this.itemDropsImageSizeHeader.TabIndex = 40;
            this.itemDropsImageSizeHeader.Text = "Image Size";
            this.itemDropsImageSizeHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lootImageSizeBar
            // 
            this.lootImageSizeBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.lootImageSizeBar.LargeChange = 10;
            this.lootImageSizeBar.Location = new System.Drawing.Point(327, 38);
            this.lootImageSizeBar.Maximum = 32;
            this.lootImageSizeBar.Minimum = 16;
            this.lootImageSizeBar.Name = "lootImageSizeBar";
            this.lootImageSizeBar.Size = new System.Drawing.Size(299, 45);
            this.lootImageSizeBar.SmallChange = 2;
            this.lootImageSizeBar.TabIndex = 41;
            this.lootImageSizeBar.Value = 20;
            this.lootImageSizeBar.Scroll += new System.EventHandler(this.lootImageSizeBar_Scroll);
            // 
            // recentDropsImageSizeBar
            // 
            this.recentDropsImageSizeBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.recentDropsImageSizeBar.LargeChange = 10;
            this.recentDropsImageSizeBar.Location = new System.Drawing.Point(327, 192);
            this.recentDropsImageSizeBar.Maximum = 32;
            this.recentDropsImageSizeBar.Minimum = 16;
            this.recentDropsImageSizeBar.Name = "recentDropsImageSizeBar";
            this.recentDropsImageSizeBar.Size = new System.Drawing.Size(299, 45);
            this.recentDropsImageSizeBar.SmallChange = 2;
            this.recentDropsImageSizeBar.TabIndex = 43;
            this.recentDropsImageSizeBar.Value = 20;
            this.recentDropsImageSizeBar.Scroll += new System.EventHandler(this.recentDropsImageSizeBar_Scroll);
            // 
            // recentDropsImageSizeHeader
            // 
            this.recentDropsImageSizeHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.recentDropsImageSizeHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recentDropsImageSizeHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.recentDropsImageSizeHeader.Location = new System.Drawing.Point(327, 163);
            this.recentDropsImageSizeHeader.Name = "recentDropsImageSizeHeader";
            this.recentDropsImageSizeHeader.Size = new System.Drawing.Size(299, 30);
            this.recentDropsImageSizeHeader.TabIndex = 42;
            this.recentDropsImageSizeHeader.Text = "Image Size";
            this.recentDropsImageSizeHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wasteImageSizeBar
            // 
            this.wasteImageSizeBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.wasteImageSizeBar.LargeChange = 10;
            this.wasteImageSizeBar.Location = new System.Drawing.Point(327, 346);
            this.wasteImageSizeBar.Maximum = 32;
            this.wasteImageSizeBar.Minimum = 16;
            this.wasteImageSizeBar.Name = "wasteImageSizeBar";
            this.wasteImageSizeBar.Size = new System.Drawing.Size(299, 45);
            this.wasteImageSizeBar.SmallChange = 2;
            this.wasteImageSizeBar.TabIndex = 45;
            this.wasteImageSizeBar.Value = 20;
            this.wasteImageSizeBar.Scroll += new System.EventHandler(this.wasteImageSizeBar_Scroll);
            // 
            // wasteImageSizeHeader
            // 
            this.wasteImageSizeHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.wasteImageSizeHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wasteImageSizeHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.wasteImageSizeHeader.Location = new System.Drawing.Point(327, 317);
            this.wasteImageSizeHeader.Name = "wasteImageSizeHeader";
            this.wasteImageSizeHeader.Size = new System.Drawing.Size(299, 30);
            this.wasteImageSizeHeader.TabIndex = 44;
            this.wasteImageSizeHeader.Text = "Image Size";
            this.wasteImageSizeHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SummaryTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.wasteImageSizeBar);
            this.Controls.Add(this.wasteImageSizeHeader);
            this.Controls.Add(this.recentDropsImageSizeBar);
            this.Controls.Add(this.recentDropsImageSizeHeader);
            this.Controls.Add(this.lootImageSizeBar);
            this.Controls.Add(this.itemDropsImageSizeHeader);
            this.Controls.Add(this.maxUsedItemsTrack);
            this.Controls.Add(this.maxUsedItemsHeader);
            this.Controls.Add(this.maxDamageEntryTrack);
            this.Controls.Add(this.maxDamageEntryHeader);
            this.Controls.Add(this.maxRecentDropsTrack);
            this.Controls.Add(this.maxRecentDropsHeader);
            this.Controls.Add(this.maxCreatureKillsTrack);
            this.Controls.Add(this.maxCreatureKillsHeader);
            this.Controls.Add(this.maxItemsDisplayedTrack);
            this.Controls.Add(this.maxItemsDisplayedHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SummaryTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.maxItemsDisplayedTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCreatureKillsTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRecentDropsTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxDamageEntryTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxUsedItemsTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lootImageSizeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recentDropsImageSizeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wasteImageSizeBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar maxItemsDisplayedTrack;
        private System.Windows.Forms.TrackBar maxCreatureKillsTrack;
        private System.Windows.Forms.TrackBar maxRecentDropsTrack;
        private System.Windows.Forms.TrackBar maxDamageEntryTrack;
        private System.Windows.Forms.TrackBar maxUsedItemsTrack;
        private System.Windows.Forms.TrackBar lootImageSizeBar;
        private System.Windows.Forms.TrackBar recentDropsImageSizeBar;
        private System.Windows.Forms.TrackBar wasteImageSizeBar;
        private PrettyHeader wasteImageSizeHeader;
        private PrettyHeader recentDropsImageSizeHeader;
        private PrettyHeader itemDropsImageSizeHeader;
        private PrettyHeader maxUsedItemsHeader;
        private PrettyHeader maxDamageEntryHeader;
        private PrettyHeader maxRecentDropsHeader;
        private PrettyHeader maxCreatureKillsHeader;
        private PrettyHeader maxItemsDisplayedHeader;
    }
}