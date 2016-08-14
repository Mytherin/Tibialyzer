namespace Tibialyzer {
    partial class BrowseTab {
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
            this.browseTextBox = new Tibialyzer.EnterTextBox();
            this.creaturePanel = new System.Windows.Forms.Panel();
            this.searchDatabaseHeader = new PrettyHeader();
            this.browseTypeDropDownList = new Tibialyzer.PrettyDropDownList();
            this.SuspendLayout();
            // 
            // browseTextBox
            // 
            this.browseTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.browseTextBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.browseTextBox.Location = new System.Drawing.Point(9, 40);
            this.browseTextBox.Name = "browseTextBox";
            this.browseTextBox.Size = new System.Drawing.Size(466, 23);
            this.browseTextBox.TabIndex = 55;
            this.browseTextBox.TextChanged += new System.EventHandler(this.browseSearch_TextChanged);
            // 
            // creaturePanel
            // 
            this.creaturePanel.AutoScroll = true;
            this.creaturePanel.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.creaturePanel.Location = new System.Drawing.Point(9, 64);
            this.creaturePanel.Name = "creaturePanel";
            this.creaturePanel.Size = new System.Drawing.Size(617, 473);
            this.creaturePanel.TabIndex = 2;
            // 
            // searchDatabaseHeader
            // 
            this.searchDatabaseHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.searchDatabaseHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchDatabaseHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.searchDatabaseHeader.Location = new System.Drawing.Point(9, 10);
            this.searchDatabaseHeader.Name = "searchDatabaseHeader";
            this.searchDatabaseHeader.Size = new System.Drawing.Size(617, 30);
            this.searchDatabaseHeader.TabIndex = 53;
            this.searchDatabaseHeader.Text = "Search Database";
            this.searchDatabaseHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // browseTypeDropDownList
            // 
            this.browseTypeDropDownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.browseTypeDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.browseTypeDropDownList.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseTypeDropDownList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.browseTypeDropDownList.FormattingEnabled = true;
            this.browseTypeDropDownList.Items.AddRange(new object[] {
            "Creatures",
            "Items",
            "NPCs",
            "Hunting Places",
            "Quests",
            "Mounts",
            "Outfits"});
            this.browseTypeDropDownList.Location = new System.Drawing.Point(476, 40);
            this.browseTypeDropDownList.Name = "browseTypeDropDownList";
            this.browseTypeDropDownList.Size = new System.Drawing.Size(150, 24);
            this.browseTypeDropDownList.TabIndex = 54;
            this.browseTypeDropDownList.SelectedIndexChanged += new System.EventHandler(this.browseSelectionBox_SelectedIndexChanged);
            // 
            // BrowseTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.browseTextBox);
            this.Controls.Add(this.creaturePanel);
            this.Controls.Add(this.searchDatabaseHeader);
            this.Controls.Add(this.browseTypeDropDownList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BrowseTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private EnterTextBox browseTextBox;
        private System.Windows.Forms.Panel creaturePanel;
        private System.Windows.Forms.Label searchDatabaseHeader;
        private PrettyDropDownList browseTypeDropDownList;
        #endregion
    }
}