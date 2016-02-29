namespace Tibialyzer {
    partial class WasteForm {
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
            this.wastedItemsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // wastedItemsLabel
            //
            this.wastedItemsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wastedItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.wastedItemsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wastedItemsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.wastedItemsLabel.Location = new System.Drawing.Point(0, 6);
            this.wastedItemsLabel.Name = "wastedItemsLabel";
            this.wastedItemsLabel.Size = new System.Drawing.Size(284, 20);
            this.wastedItemsLabel.TabIndex = 12;
            this.wastedItemsLabel.Text = "Used Items";
            this.wastedItemsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // WasteForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 85);
            this.Controls.Add(this.wastedItemsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WasteForm";
            this.Text = "WasteForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label wastedItemsLabel;
    }
}