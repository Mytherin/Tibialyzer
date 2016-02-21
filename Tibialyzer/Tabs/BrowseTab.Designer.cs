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
            this.SuspendLayout();
            ///
            /// browseTextBox
            ///
            this.browseTextBox = new Tibialyzer.EnterTextBox();
            this.browseTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.browseTextBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.browseTextBox.Location = new System.Drawing.Point(3, 49);
            this.browseTextBox.Name = "browseTextBox";
            this.browseTextBox.Size = new System.Drawing.Size(390, 23);
            this.browseTextBox.TabIndex = 55;
            this.browseTextBox.TextChanged += new System.EventHandler(this.browseSearch_TextChanged);
            ///
            /// creaturePanel
            ///
            this.creaturePanel = new System.Windows.Forms.Panel();
            this.creaturePanel.AutoScroll = true;
            this.creaturePanel.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.creaturePanel.Location = new System.Drawing.Point(3, 73);
            this.creaturePanel.Name = "creaturePanel";
            this.creaturePanel.Size = new System.Drawing.Size(534, 421);
            this.creaturePanel.TabIndex = 2;
            this.creaturePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(MainForm.mainForm.draggable_MouseDown);
            ///
            /// searchDatabaseHeader
            ///
            this.searchDatabaseHeader = new System.Windows.Forms.Label();
            this.searchDatabaseHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.searchDatabaseHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchDatabaseHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.searchDatabaseHeader.Location = new System.Drawing.Point(3, 19);
            this.searchDatabaseHeader.Name = "searchDatabaseHeader";
            this.searchDatabaseHeader.Size = new System.Drawing.Size(534, 30);
            this.searchDatabaseHeader.TabIndex = 53;
            this.searchDatabaseHeader.Text = "Search Database";
            this.searchDatabaseHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// browseTypeBox
            ///
            this.browseTypeBox = new Tibialyzer.PrettyDropDownList();
            this.browseTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.browseTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.browseTypeBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseTypeBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.browseTypeBox.FormattingEnabled = true;
            this.browseTypeBox.Items.AddRange(new object[] {
            "Creatures",
            "Items",
            "NPCs",
            "Hunting Places",
            "Quests",
            "Mounts",
            "Outfits"});
            this.browseTypeBox.Location = new System.Drawing.Point(393, 49);
            this.browseTypeBox.Name = "browseTypeBox";
            this.browseTypeBox.Size = new System.Drawing.Size(144, 24);
            this.browseTypeBox.TabIndex = 54;
            this.browseTypeBox.SelectedIndexChanged += new System.EventHandler(this.browseSelectionBox_SelectedIndexChanged);
            // 
            // TabBase
            // 
            this.Controls.Add(browseTextBox);
            this.Controls.Add(creaturePanel);
            this.Controls.Add(searchDatabaseHeader);
            this.Controls.Add(browseTypeBox);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabBase";
            this.Text = "TabBase";
            this.ResumeLayout(false);
        }

        private EnterTextBox browseTextBox;
        private System.Windows.Forms.Panel creaturePanel;
        private System.Windows.Forms.Label searchDatabaseHeader;
        private PrettyDropDownList browseTypeBox;
        #endregion
    }
}