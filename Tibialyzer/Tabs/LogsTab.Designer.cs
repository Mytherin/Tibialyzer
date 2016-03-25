namespace Tibialyzer {
    partial class LogsTab {
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
            this.showPopupButton = new System.Windows.Forms.Label();
            this.showAllLootButton = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.clearLog = new System.Windows.Forms.Label();
            this.loadLogFromFileButton = new System.Windows.Forms.Label();
            this.saveLogToFileButton = new System.Windows.Forms.Label();
            this.logMessageCollection = new Tibialyzer.PrettyListBox();
            this.SuspendLayout();
            // 
            // showPopupButton
            // 
            this.showPopupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.showPopupButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showPopupButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.showPopupButton.Location = new System.Drawing.Point(299, 463);
            this.showPopupButton.Name = "showPopupButton";
            this.showPopupButton.Padding = new System.Windows.Forms.Padding(10);
            this.showPopupButton.Size = new System.Drawing.Size(238, 38);
            this.showPopupButton.TabIndex = 49;
            this.showPopupButton.Text = "Popup Selected Message";
            this.showPopupButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showPopupButton.Click += new System.EventHandler(this.showPopupButton_Click);
            this.showPopupButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.showPopupButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // showAllLootButton
            // 
            this.showAllLootButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.showAllLootButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showAllLootButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.showAllLootButton.Location = new System.Drawing.Point(3, 463);
            this.showAllLootButton.Name = "showAllLootButton";
            this.showAllLootButton.Padding = new System.Windows.Forms.Padding(10);
            this.showAllLootButton.Size = new System.Drawing.Size(238, 38);
            this.showAllLootButton.TabIndex = 48;
            this.showAllLootButton.Text = "Show All Loot";
            this.showAllLootButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showAllLootButton.Click += new System.EventHandler(this.showAllLootButton_Click);
            this.showAllLootButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.showAllLootButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.label33.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label33.Location = new System.Drawing.Point(299, 95);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(238, 30);
            this.label33.TabIndex = 47;
            this.label33.Text = "Delete";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label31.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label31.Location = new System.Drawing.Point(3, 210);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(534, 30);
            this.label31.TabIndex = 45;
            this.label31.Text = "Log Messages";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label38
            // 
            this.label38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label38.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label38.Location = new System.Drawing.Point(299, 19);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(238, 30);
            this.label38.TabIndex = 44;
            this.label38.Text = "Export";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clearLog
            // 
            this.clearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.clearLog.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.clearLog.Location = new System.Drawing.Point(299, 163);
            this.clearLog.Name = "clearLog";
            this.clearLog.Padding = new System.Windows.Forms.Padding(10);
            this.clearLog.Size = new System.Drawing.Size(238, 41);
            this.clearLog.TabIndex = 43;
            this.clearLog.Text = "Clear Log";
            this.clearLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.clearLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.resetButton_Click);
            this.clearLog.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.clearLog.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // loadLogFromFileButton
            // 
            this.loadLogFromFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.loadLogFromFileButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadLogFromFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.loadLogFromFileButton.Location = new System.Drawing.Point(299, 125);
            this.loadLogFromFileButton.Name = "loadLogFromFileButton";
            this.loadLogFromFileButton.Padding = new System.Windows.Forms.Padding(10);
            this.loadLogFromFileButton.Size = new System.Drawing.Size(238, 38);
            this.loadLogFromFileButton.TabIndex = 42;
            this.loadLogFromFileButton.Text = "Load Log From File";
            this.loadLogFromFileButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.loadLogFromFileButton.Click += new System.EventHandler(this.loadLogFromFileButton_Click);
            this.loadLogFromFileButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.loadLogFromFileButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // saveLogToFileButton
            // 
            this.saveLogToFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveLogToFileButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLogToFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveLogToFileButton.Location = new System.Drawing.Point(299, 49);
            this.saveLogToFileButton.Name = "saveLogToFileButton";
            this.saveLogToFileButton.Padding = new System.Windows.Forms.Padding(10);
            this.saveLogToFileButton.Size = new System.Drawing.Size(238, 38);
            this.saveLogToFileButton.TabIndex = 41;
            this.saveLogToFileButton.Text = "Save Log To File";
            this.saveLogToFileButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveLogToFileButton.Click += new System.EventHandler(this.saveLogToFileButton_Click);
            this.saveLogToFileButton.MouseEnter += new System.EventHandler(this.ControlMouseEnter);
            this.saveLogToFileButton.MouseLeave += new System.EventHandler(this.ControlMouseLeave);
            // 
            // logMessageCollection
            // 
            this.logMessageCollection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.logMessageCollection.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logMessageCollection.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.logMessageCollection.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logMessageCollection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.logMessageCollection.FormattingEnabled = true;
            this.logMessageCollection.ItemHeight = 28;
            this.logMessageCollection.Items.AddRange(new object[] {
            ""});
            this.logMessageCollection.Location = new System.Drawing.Point(3, 240);
            this.logMessageCollection.Name = "logMessageCollection";
            this.logMessageCollection.Size = new System.Drawing.Size(534, 221);
            this.logMessageCollection.TabIndex = 46;
            // 
            // LogsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.Controls.Add(this.showPopupButton);
            this.Controls.Add(this.showAllLootButton);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.clearLog);
            this.Controls.Add(this.loadLogFromFileButton);
            this.Controls.Add(this.saveLogToFileButton);
            this.Controls.Add(this.logMessageCollection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LogsTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label showPopupButton;
        private System.Windows.Forms.Label showAllLootButton;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label clearLog;
        private System.Windows.Forms.Label loadLogFromFileButton;
        private System.Windows.Forms.Label saveLogToFileButton;
        private PrettyListBox logMessageCollection;

        #endregion
    }
}