namespace Tibialyzer {
    partial class DatabaseTab {
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
            this.databaseItemsHeader = new Tibialyzer.PrettyHeader();
            this.conversionPresetsHeader = new Tibialyzer.PrettyHeader();
            this.defaultSettingsButton = new Tibialyzer.PrettyButton();
            this.mageCreatureProductsButton = new Tibialyzer.PrettyButton();
            this.valuablesOnly1KButton = new Tibialyzer.PrettyButton();
            this.mageNoCreatureProductsButton = new Tibialyzer.PrettyButton();
            this.noGoldCoinButton = new Tibialyzer.PrettyButton();
            this.valuablesOnly5KButton = new Tibialyzer.PrettyButton();
            this.SuspendLayout();
            // 
            // databaseItemsHeader
            // 
            this.databaseItemsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.databaseItemsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseItemsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.databaseItemsHeader.Location = new System.Drawing.Point(9, 9);
            this.databaseItemsHeader.Name = "databaseItemsHeader";
            this.databaseItemsHeader.Size = new System.Drawing.Size(617, 30);
            this.databaseItemsHeader.TabIndex = 51;
            this.databaseItemsHeader.Text = "Database Item Conversion";
            this.databaseItemsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // conversionPresetsHeader
            // 
            this.conversionPresetsHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.conversionPresetsHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conversionPresetsHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.conversionPresetsHeader.Location = new System.Drawing.Point(12, 186);
            this.conversionPresetsHeader.Name = "conversionPresetsHeader";
            this.conversionPresetsHeader.Size = new System.Drawing.Size(617, 30);
            this.conversionPresetsHeader.TabIndex = 52;
            this.conversionPresetsHeader.Text = "Conversion Presets";
            this.conversionPresetsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defaultSettingsButton
            // 
            this.defaultSettingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.defaultSettingsButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultSettingsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.defaultSettingsButton.Image = global::Tibialyzer.Properties.Resources.outfit_noob;
            this.defaultSettingsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.defaultSettingsButton.Location = new System.Drawing.Point(12, 219);
            this.defaultSettingsButton.Name = "defaultSettingsButton";
            this.defaultSettingsButton.Padding = new System.Windows.Forms.Padding(10);
            this.defaultSettingsButton.Size = new System.Drawing.Size(299, 80);
            this.defaultSettingsButton.TabIndex = 53;
            this.defaultSettingsButton.Text = "Default Settings";
            this.defaultSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.defaultSettingsButton.Click += new System.EventHandler(this.defaultSettingsButton_Click);
            // 
            // mageCreatureProductsButton
            // 
            this.mageCreatureProductsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mageCreatureProductsButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mageCreatureProductsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.mageCreatureProductsButton.Image = global::Tibialyzer.Properties.Resources.outfit_sorcerer;
            this.mageCreatureProductsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mageCreatureProductsButton.Location = new System.Drawing.Point(12, 305);
            this.mageCreatureProductsButton.Name = "mageCreatureProductsButton";
            this.mageCreatureProductsButton.Padding = new System.Windows.Forms.Padding(10);
            this.mageCreatureProductsButton.Size = new System.Drawing.Size(299, 80);
            this.mageCreatureProductsButton.TabIndex = 54;
            this.mageCreatureProductsButton.Text = "Mage (Creature Products)";
            this.mageCreatureProductsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mageCreatureProductsButton.Click += new System.EventHandler(this.mageCreatureProductsButton_Click);
            // 
            // valuablesOnly1KButton
            // 
            this.valuablesOnly1KButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.valuablesOnly1KButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valuablesOnly1KButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.valuablesOnly1KButton.Image = global::Tibialyzer.Properties.Resources.outfit_nobleman_poor;
            this.valuablesOnly1KButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.valuablesOnly1KButton.Location = new System.Drawing.Point(12, 391);
            this.valuablesOnly1KButton.Name = "valuablesOnly1KButton";
            this.valuablesOnly1KButton.Padding = new System.Windows.Forms.Padding(10);
            this.valuablesOnly1KButton.Size = new System.Drawing.Size(299, 80);
            this.valuablesOnly1KButton.TabIndex = 55;
            this.valuablesOnly1KButton.Text = "Valuables Only (>1000G)";
            this.valuablesOnly1KButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.valuablesOnly1KButton.Click += new System.EventHandler(this.valuablesOnly1KButton_Click);
            // 
            // mageNoCreatureProductsButton
            // 
            this.mageNoCreatureProductsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mageNoCreatureProductsButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mageNoCreatureProductsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.mageNoCreatureProductsButton.Image = global::Tibialyzer.Properties.Resources.outfit_druid;
            this.mageNoCreatureProductsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mageNoCreatureProductsButton.Location = new System.Drawing.Point(330, 305);
            this.mageNoCreatureProductsButton.Name = "mageNoCreatureProductsButton";
            this.mageNoCreatureProductsButton.Padding = new System.Windows.Forms.Padding(10);
            this.mageNoCreatureProductsButton.Size = new System.Drawing.Size(299, 80);
            this.mageNoCreatureProductsButton.TabIndex = 56;
            this.mageNoCreatureProductsButton.Text = "Mage (No Creature Products)";
            this.mageNoCreatureProductsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mageNoCreatureProductsButton.Click += new System.EventHandler(this.mageNoCreatureProductsButton_Click);
            // 
            // noGoldCoinButton
            // 
            this.noGoldCoinButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.noGoldCoinButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noGoldCoinButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.noGoldCoinButton.Image = global::Tibialyzer.Properties.Resources.outfit_knight;
            this.noGoldCoinButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.noGoldCoinButton.Location = new System.Drawing.Point(327, 219);
            this.noGoldCoinButton.Name = "noGoldCoinButton";
            this.noGoldCoinButton.Padding = new System.Windows.Forms.Padding(10);
            this.noGoldCoinButton.Size = new System.Drawing.Size(299, 80);
            this.noGoldCoinButton.TabIndex = 57;
            this.noGoldCoinButton.Text = "No Gold Coins";
            this.noGoldCoinButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.noGoldCoinButton.Click += new System.EventHandler(this.noGoldCoinButton_Click);
            // 
            // valuablesOnly5KButton
            // 
            this.valuablesOnly5KButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.valuablesOnly5KButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valuablesOnly5KButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.valuablesOnly5KButton.Image = global::Tibialyzer.Properties.Resources.outfit_nobleman_fancy;
            this.valuablesOnly5KButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.valuablesOnly5KButton.Location = new System.Drawing.Point(330, 391);
            this.valuablesOnly5KButton.Name = "valuablesOnly5KButton";
            this.valuablesOnly5KButton.Padding = new System.Windows.Forms.Padding(10);
            this.valuablesOnly5KButton.Size = new System.Drawing.Size(299, 80);
            this.valuablesOnly5KButton.TabIndex = 58;
            this.valuablesOnly5KButton.Text = "Valuables Only (>5000G)";
            this.valuablesOnly5KButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.valuablesOnly5KButton.Click += new System.EventHandler(this.valuablesOnly5KButton_Click);
            // 
            // DatabaseTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.valuablesOnly5KButton);
            this.Controls.Add(this.noGoldCoinButton);
            this.Controls.Add(this.mageNoCreatureProductsButton);
            this.Controls.Add(this.valuablesOnly1KButton);
            this.Controls.Add(this.mageCreatureProductsButton);
            this.Controls.Add(this.defaultSettingsButton);
            this.Controls.Add(this.conversionPresetsHeader);
            this.Controls.Add(this.databaseItemsHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DatabaseTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);

        }
        #endregion

        private PrettyButton valuablesOnly5KButton;
        private PrettyButton noGoldCoinButton;
        private PrettyButton mageNoCreatureProductsButton;
        private PrettyButton valuablesOnly1KButton;
        private PrettyButton mageCreatureProductsButton;
        private PrettyButton defaultSettingsButton;
        private PrettyHeader conversionPresetsHeader;
        private PrettyHeader databaseItemsHeader;
    }
}