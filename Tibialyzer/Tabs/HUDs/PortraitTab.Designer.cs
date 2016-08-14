namespace Tibialyzer {
    partial class PortraitTab {
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
            this.changeBackgroundImageButton = new PrettyButton();
            this.backgroundImageHeader = new PrettyHeader();
            this.backgroundImageBox = new System.Windows.Forms.PictureBox();
            this.changeCenterImageButton = new PrettyButton();
            this.centerImageHeader = new PrettyHeader();
            this.centerImageBox = new System.Windows.Forms.PictureBox();
            this.backgroundScaleHeader = new PrettyHeader();
            this.backgroundImageScale = new System.Windows.Forms.TrackBar();
            this.scaleHeader = new PrettyHeader();
            this.centerImageScale = new System.Windows.Forms.TrackBar();
            this.xOffsetLabel = new System.Windows.Forms.Label();
            this.xOffsetBox = new Tibialyzer.EnterTextBox();
            this.yOffsetLabel = new System.Windows.Forms.Label();
            this.yOffsetBox = new Tibialyzer.EnterTextBox();
            this.yOffsetCenterLabel = new System.Windows.Forms.Label();
            this.yOffsetBoxCenter = new Tibialyzer.EnterTextBox();
            this.xOffsetCenterLabel = new System.Windows.Forms.Label();
            this.xOffsetBoxCenter = new Tibialyzer.EnterTextBox();
            this.refreshButton = new PrettyButton();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImageScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerImageScale)).BeginInit();
            this.SuspendLayout();
            // 
            // changeBackgroundImageButton
            // 
            this.changeBackgroundImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.changeBackgroundImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeBackgroundImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.changeBackgroundImageButton.Location = new System.Drawing.Point(129, 42);
            this.changeBackgroundImageButton.Name = "changeBackgroundImageButton";
            this.changeBackgroundImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.changeBackgroundImageButton.Size = new System.Drawing.Size(106, 38);
            this.changeBackgroundImageButton.TabIndex = 55;
            this.changeBackgroundImageButton.Text = "Browse";
            this.changeBackgroundImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.changeBackgroundImageButton.Click += new System.EventHandler(this.changeBackgroundImageButton_Click);
            // 
            // backgroundImageHeader
            // 
            this.backgroundImageHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.backgroundImageHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backgroundImageHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.backgroundImageHeader.Location = new System.Drawing.Point(9, 9);
            this.backgroundImageHeader.Name = "backgroundImageHeader";
            this.backgroundImageHeader.Size = new System.Drawing.Size(299, 30);
            this.backgroundImageHeader.TabIndex = 54;
            this.backgroundImageHeader.Text = "Background Image";
            this.backgroundImageHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundImageBox
            // 
            this.backgroundImageBox.BackColor = System.Drawing.Color.Transparent;
            this.backgroundImageBox.Location = new System.Drawing.Point(9, 42);
            this.backgroundImageBox.Name = "backgroundImageBox";
            this.backgroundImageBox.Size = new System.Drawing.Size(100, 100);
            this.backgroundImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.backgroundImageBox.TabIndex = 53;
            this.backgroundImageBox.TabStop = false;
            // 
            // changeCenterImageButton
            // 
            this.changeCenterImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.changeCenterImageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeCenterImageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.changeCenterImageButton.Location = new System.Drawing.Point(129, 207);
            this.changeCenterImageButton.Name = "changeCenterImageButton";
            this.changeCenterImageButton.Padding = new System.Windows.Forms.Padding(10);
            this.changeCenterImageButton.Size = new System.Drawing.Size(106, 38);
            this.changeCenterImageButton.TabIndex = 58;
            this.changeCenterImageButton.Text = "Browse";
            this.changeCenterImageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.changeCenterImageButton.Click += new System.EventHandler(this.changeCenterImageButton_Click);
            // 
            // centerImageHeader
            // 
            this.centerImageHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.centerImageHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.centerImageHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.centerImageHeader.Location = new System.Drawing.Point(12, 174);
            this.centerImageHeader.Name = "centerImageHeader";
            this.centerImageHeader.Size = new System.Drawing.Size(299, 30);
            this.centerImageHeader.TabIndex = 57;
            this.centerImageHeader.Text = "Center Image";
            this.centerImageHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // centerImageBox
            // 
            this.centerImageBox.BackColor = System.Drawing.Color.Transparent;
            this.centerImageBox.Location = new System.Drawing.Point(9, 207);
            this.centerImageBox.Name = "centerImageBox";
            this.centerImageBox.Size = new System.Drawing.Size(100, 100);
            this.centerImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.centerImageBox.TabIndex = 56;
            this.centerImageBox.TabStop = false;
            // 
            // backgroundScaleHeader
            // 
            this.backgroundScaleHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.backgroundScaleHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backgroundScaleHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.backgroundScaleHeader.Location = new System.Drawing.Point(327, 9);
            this.backgroundScaleHeader.Name = "backgroundScaleHeader";
            this.backgroundScaleHeader.Size = new System.Drawing.Size(299, 30);
            this.backgroundScaleHeader.TabIndex = 60;
            this.backgroundScaleHeader.Text = "Scale";
            this.backgroundScaleHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundImageScale
            // 
            this.backgroundImageScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.backgroundImageScale.Location = new System.Drawing.Point(327, 39);
            this.backgroundImageScale.Maximum = 100;
            this.backgroundImageScale.Name = "backgroundImageScale";
            this.backgroundImageScale.Size = new System.Drawing.Size(299, 45);
            this.backgroundImageScale.TabIndex = 59;
            this.backgroundImageScale.Value = 100;
            this.backgroundImageScale.Scroll += new System.EventHandler(this.backgroundImageScale_Scroll);
            // 
            // scaleHeader
            // 
            this.scaleHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.scaleHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.scaleHeader.Location = new System.Drawing.Point(327, 174);
            this.scaleHeader.Name = "scaleHeader";
            this.scaleHeader.Size = new System.Drawing.Size(299, 30);
            this.scaleHeader.TabIndex = 62;
            this.scaleHeader.Text = "Scale";
            this.scaleHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // centerImageScale
            // 
            this.centerImageScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.centerImageScale.Location = new System.Drawing.Point(327, 204);
            this.centerImageScale.Maximum = 100;
            this.centerImageScale.Name = "centerImageScale";
            this.centerImageScale.Size = new System.Drawing.Size(299, 45);
            this.centerImageScale.TabIndex = 61;
            this.centerImageScale.Value = 100;
            this.centerImageScale.Scroll += new System.EventHandler(this.centerImageScale_Scroll);
            // 
            // xOffsetLabel
            // 
            this.xOffsetLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.xOffsetLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.xOffsetLabel.Location = new System.Drawing.Point(321, 96);
            this.xOffsetLabel.Name = "xOffsetLabel";
            this.xOffsetLabel.Size = new System.Drawing.Size(179, 23);
            this.xOffsetLabel.TabIndex = 64;
            this.xOffsetLabel.Text = "X Offset (From Center)";
            this.xOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xOffsetBox
            // 
            this.xOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.xOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.xOffsetBox.Location = new System.Drawing.Point(500, 96);
            this.xOffsetBox.Name = "xOffsetBox";
            this.xOffsetBox.Size = new System.Drawing.Size(126, 23);
            this.xOffsetBox.TabIndex = 63;
            this.xOffsetBox.TextChanged += new System.EventHandler(this.xOffsetBox_TextChanged);
            // 
            // yOffsetLabel
            // 
            this.yOffsetLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.yOffsetLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.yOffsetLabel.Location = new System.Drawing.Point(321, 119);
            this.yOffsetLabel.Name = "yOffsetLabel";
            this.yOffsetLabel.Size = new System.Drawing.Size(179, 23);
            this.yOffsetLabel.TabIndex = 66;
            this.yOffsetLabel.Text = "Y Offset (From Center)";
            this.yOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // yOffsetBox
            // 
            this.yOffsetBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.yOffsetBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yOffsetBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.yOffsetBox.Location = new System.Drawing.Point(500, 119);
            this.yOffsetBox.Name = "yOffsetBox";
            this.yOffsetBox.Size = new System.Drawing.Size(126, 23);
            this.yOffsetBox.TabIndex = 65;
            this.yOffsetBox.TextChanged += new System.EventHandler(this.yOffsetBox_TextChanged);
            // 
            // yOffsetCenterLabel
            // 
            this.yOffsetCenterLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.yOffsetCenterLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yOffsetCenterLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.yOffsetCenterLabel.Location = new System.Drawing.Point(324, 278);
            this.yOffsetCenterLabel.Name = "yOffsetCenterLabel";
            this.yOffsetCenterLabel.Size = new System.Drawing.Size(179, 23);
            this.yOffsetCenterLabel.TabIndex = 70;
            this.yOffsetCenterLabel.Text = "Y Offset (From Center)";
            this.yOffsetCenterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // yOffsetBoxCenter
            // 
            this.yOffsetBoxCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.yOffsetBoxCenter.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yOffsetBoxCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.yOffsetBoxCenter.Location = new System.Drawing.Point(503, 278);
            this.yOffsetBoxCenter.Name = "yOffsetBoxCenter";
            this.yOffsetBoxCenter.Size = new System.Drawing.Size(126, 23);
            this.yOffsetBoxCenter.TabIndex = 69;
            this.yOffsetBoxCenter.TextChanged += new System.EventHandler(this.yOffsetBoxCenter_TextChanged);
            // 
            // xOffsetCenterLabel
            // 
            this.xOffsetCenterLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.xOffsetCenterLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xOffsetCenterLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.xOffsetCenterLabel.Location = new System.Drawing.Point(324, 255);
            this.xOffsetCenterLabel.Name = "xOffsetCenterLabel";
            this.xOffsetCenterLabel.Size = new System.Drawing.Size(179, 23);
            this.xOffsetCenterLabel.TabIndex = 68;
            this.xOffsetCenterLabel.Text = "X Offset (From Center)";
            this.xOffsetCenterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // xOffsetBoxCenter
            // 
            this.xOffsetBoxCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.xOffsetBoxCenter.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xOffsetBoxCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.xOffsetBoxCenter.Location = new System.Drawing.Point(503, 255);
            this.xOffsetBoxCenter.Name = "xOffsetBoxCenter";
            this.xOffsetBoxCenter.Size = new System.Drawing.Size(126, 23);
            this.xOffsetBoxCenter.TabIndex = 67;
            this.xOffsetBoxCenter.TextChanged += new System.EventHandler(this.xOffsetBoxCenter_TextChanged);
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.refreshButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.refreshButton.Location = new System.Drawing.Point(235, 316);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Padding = new System.Windows.Forms.Padding(10);
            this.refreshButton.Size = new System.Drawing.Size(172, 38);
            this.refreshButton.TabIndex = 71;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // PortraitTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.yOffsetCenterLabel);
            this.Controls.Add(this.yOffsetBoxCenter);
            this.Controls.Add(this.xOffsetCenterLabel);
            this.Controls.Add(this.xOffsetBoxCenter);
            this.Controls.Add(this.yOffsetLabel);
            this.Controls.Add(this.yOffsetBox);
            this.Controls.Add(this.xOffsetLabel);
            this.Controls.Add(this.xOffsetBox);
            this.Controls.Add(this.scaleHeader);
            this.Controls.Add(this.centerImageScale);
            this.Controls.Add(this.backgroundScaleHeader);
            this.Controls.Add(this.backgroundImageScale);
            this.Controls.Add(this.changeCenterImageButton);
            this.Controls.Add(this.centerImageHeader);
            this.Controls.Add(this.centerImageBox);
            this.Controls.Add(this.changeBackgroundImageButton);
            this.Controls.Add(this.backgroundImageHeader);
            this.Controls.Add(this.backgroundImageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PortraitTab";
            this.Text = "TabBase";
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImageScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerImageScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label changeBackgroundImageButton;
        private System.Windows.Forms.Label backgroundImageHeader;
        private System.Windows.Forms.PictureBox backgroundImageBox;
        private System.Windows.Forms.Label changeCenterImageButton;
        private System.Windows.Forms.Label centerImageHeader;
        private System.Windows.Forms.PictureBox centerImageBox;
        private System.Windows.Forms.Label backgroundScaleHeader;
        private System.Windows.Forms.TrackBar backgroundImageScale;
        private System.Windows.Forms.Label scaleHeader;
        private System.Windows.Forms.TrackBar centerImageScale;
        private System.Windows.Forms.Label xOffsetLabel;
        private EnterTextBox xOffsetBox;
        private System.Windows.Forms.Label yOffsetLabel;
        private EnterTextBox yOffsetBox;
        private System.Windows.Forms.Label yOffsetCenterLabel;
        private EnterTextBox yOffsetBoxCenter;
        private System.Windows.Forms.Label xOffsetCenterLabel;
        private EnterTextBox xOffsetBoxCenter;
        private System.Windows.Forms.Label refreshButton;
    }
}