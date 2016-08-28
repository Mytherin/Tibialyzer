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
            this.popupSelectedMessageButton = new Tibialyzer.PrettyButton();
            this.showAllLootButton = new Tibialyzer.PrettyButton();
            this.deleteHeader = new Tibialyzer.PrettyHeader();
            this.logMessagesHeader = new Tibialyzer.PrettyHeader();
            this.exportHeader = new Tibialyzer.PrettyHeader();
            this.clearLogButton = new Tibialyzer.PrettyButton();
            this.loadLogFromFileButton = new Tibialyzer.PrettyButton();
            this.saveLogToFileButton = new Tibialyzer.PrettyButton();
            this.logMessageCollection = new Tibialyzer.PrettyListBox();
            this.SuspendLayout();
            // 
            // popupSelectedMessageButton
            // 
            this.popupSelectedMessageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.popupSelectedMessageButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.popupSelectedMessageButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.popupSelectedMessageButton.Location = new System.Drawing.Point(388, 509);
            this.popupSelectedMessageButton.Name = "popupSelectedMessageButton";
            this.popupSelectedMessageButton.Padding = new System.Windows.Forms.Padding(10);
            this.popupSelectedMessageButton.Size = new System.Drawing.Size(238, 38);
            this.popupSelectedMessageButton.TabIndex = 49;
            this.popupSelectedMessageButton.Text = "Popup Selected Message";
            this.popupSelectedMessageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.popupSelectedMessageButton.Click += new System.EventHandler(this.showPopupButton_Click);
            // 
            // showAllLootButton
            // 
            this.showAllLootButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.showAllLootButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showAllLootButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.showAllLootButton.Location = new System.Drawing.Point(9, 509);
            this.showAllLootButton.Name = "showAllLootButton";
            this.showAllLootButton.Padding = new System.Windows.Forms.Padding(10);
            this.showAllLootButton.Size = new System.Drawing.Size(238, 38);
            this.showAllLootButton.TabIndex = 48;
            this.showAllLootButton.Text = "Show All Loot";
            this.showAllLootButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showAllLootButton.Click += new System.EventHandler(this.showAllLootButton_Click);
            // 
            // deleteHeader
            // 
            this.deleteHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(56)))), ((int)(((byte)(65)))));
            this.deleteHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.deleteHeader.Location = new System.Drawing.Point(327, 85);
            this.deleteHeader.Name = "deleteHeader";
            this.deleteHeader.Size = new System.Drawing.Size(299, 30);
            this.deleteHeader.TabIndex = 47;
            this.deleteHeader.Text = "Delete";
            this.deleteHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logMessagesHeader
            // 
            this.logMessagesHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.logMessagesHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logMessagesHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.logMessagesHeader.Location = new System.Drawing.Point(9, 200);
            this.logMessagesHeader.Name = "logMessagesHeader";
            this.logMessagesHeader.Size = new System.Drawing.Size(617, 30);
            this.logMessagesHeader.TabIndex = 45;
            this.logMessagesHeader.Text = "Log Messages";
            this.logMessagesHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exportHeader
            // 
            this.exportHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.exportHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.exportHeader.Location = new System.Drawing.Point(327, 9);
            this.exportHeader.Name = "exportHeader";
            this.exportHeader.Size = new System.Drawing.Size(299, 30);
            this.exportHeader.TabIndex = 44;
            this.exportHeader.Text = "Export";
            this.exportHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clearLogButton
            // 
            this.clearLogButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.clearLogButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearLogButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.clearLogButton.Image = global::Tibialyzer.Properties.Resources.icon_warning;
            this.clearLogButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearLogButton.Location = new System.Drawing.Point(327, 153);
            this.clearLogButton.Name = "clearLogButton";
            this.clearLogButton.Padding = new System.Windows.Forms.Padding(10);
            this.clearLogButton.Size = new System.Drawing.Size(299, 41);
            this.clearLogButton.TabIndex = 43;
            this.clearLogButton.Text = "Clear Log";
            this.clearLogButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.clearLogButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.resetButton_Click);
            // 
            // loadLogFromFileButton
            // 
            this.loadLogFromFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.loadLogFromFileButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadLogFromFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.loadLogFromFileButton.Image = global::Tibialyzer.Properties.Resources.icon_warning;
            this.loadLogFromFileButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.loadLogFromFileButton.Location = new System.Drawing.Point(327, 115);
            this.loadLogFromFileButton.Name = "loadLogFromFileButton";
            this.loadLogFromFileButton.Padding = new System.Windows.Forms.Padding(10);
            this.loadLogFromFileButton.Size = new System.Drawing.Size(299, 38);
            this.loadLogFromFileButton.TabIndex = 42;
            this.loadLogFromFileButton.Text = "Load Log From File";
            this.loadLogFromFileButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.loadLogFromFileButton.Click += new System.EventHandler(this.loadLogFromFileButton_Click);
            // 
            // saveLogToFileButton
            // 
            this.saveLogToFileButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.saveLogToFileButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLogToFileButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.saveLogToFileButton.Location = new System.Drawing.Point(327, 39);
            this.saveLogToFileButton.Name = "saveLogToFileButton";
            this.saveLogToFileButton.Padding = new System.Windows.Forms.Padding(10);
            this.saveLogToFileButton.Size = new System.Drawing.Size(299, 38);
            this.saveLogToFileButton.TabIndex = 41;
            this.saveLogToFileButton.Text = "Save Log To File";
            this.saveLogToFileButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveLogToFileButton.Click += new System.EventHandler(this.saveLogToFileButton_Click);
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
            this.logMessageCollection.Location = new System.Drawing.Point(9, 230);
            this.logMessageCollection.Name = "logMessageCollection";
            this.logMessageCollection.Size = new System.Drawing.Size(617, 277);
            this.logMessageCollection.TabIndex = 46;
            // 
            // LogsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(638, 549);
            this.Controls.Add(this.popupSelectedMessageButton);
            this.Controls.Add(this.showAllLootButton);
            this.Controls.Add(this.deleteHeader);
            this.Controls.Add(this.logMessagesHeader);
            this.Controls.Add(this.exportHeader);
            this.Controls.Add(this.clearLogButton);
            this.Controls.Add(this.loadLogFromFileButton);
            this.Controls.Add(this.saveLogToFileButton);
            this.Controls.Add(this.logMessageCollection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LogsTab";
            this.Text = "TabBase";
            this.ResumeLayout(false);

        }
        private PrettyListBox logMessageCollection;

        #endregion

        private PrettyButton saveLogToFileButton;
        private PrettyButton loadLogFromFileButton;
        private PrettyButton clearLogButton;
        private PrettyHeader exportHeader;
        private PrettyHeader logMessagesHeader;
        private PrettyHeader deleteHeader;
        private PrettyButton showAllLootButton;
        private PrettyButton popupSelectedMessageButton;
    }
}