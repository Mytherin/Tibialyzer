namespace Tibialyzer {
    partial class SystemTab {
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
            /// customCommandParameterBox
            ///
            this.customCommandParameterBox = new Tibialyzer.EnterTextBox();
            this.customCommandParameterBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandParameterBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandParameterBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.customCommandParameterBox.Location = new System.Drawing.Point(239, 206);
            this.customCommandParameterBox.Name = "customCommandParameterBox";
            this.customCommandParameterBox.Size = new System.Drawing.Size(299, 23);
            this.customCommandParameterBox.TabIndex = 63;
            this.customCommandParameterBox.TextChanged += new System.EventHandler(this.customCommandParameterBox_TextChanged);
            this.customCommandParameterBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.customCommandParameterBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// label8
            ///
            this.label8 = new System.Windows.Forms.Label();
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label8.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label8.Location = new System.Drawing.Point(240, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(297, 30);
            this.label8.TabIndex = 62;
            this.label8.Text = "Parameters";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// customCommandBox
            ///
            this.customCommandBox = new Tibialyzer.EnterTextBox();
            this.customCommandBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(204)))), ((int)(((byte)(217)))));
            this.customCommandBox.Location = new System.Drawing.Point(239, 153);
            this.customCommandBox.Name = "customCommandBox";
            this.customCommandBox.Size = new System.Drawing.Size(299, 23);
            this.customCommandBox.TabIndex = 61;
            this.customCommandBox.TextChanged += new System.EventHandler(this.customCommandBox_TextChanged);
            this.customCommandBox.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.customCommandBox.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// customCommandName
            ///
            this.customCommandName = new System.Windows.Forms.Label();
            this.customCommandName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(128)))), ((int)(((byte)(176)))));
            this.customCommandName.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandName.Location = new System.Drawing.Point(240, 94);
            this.customCommandName.Name = "customCommandName";
            this.customCommandName.Size = new System.Drawing.Size(297, 30);
            this.customCommandName.TabIndex = 60;
            this.customCommandName.Text = "Command Information";
            this.customCommandName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// label7
            ///
            this.label7 = new System.Windows.Forms.Label();
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label7.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label7.Location = new System.Drawing.Point(240, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(297, 30);
            this.label7.TabIndex = 59;
            this.label7.Text = "System Command";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// label6
            ///
            this.label6 = new System.Windows.Forms.Label();
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label6.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label6.Location = new System.Drawing.Point(3, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(230, 30);
            this.label6.TabIndex = 58;
            this.label6.Text = "Custom Commands";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ///
            /// customCommandList
            ///
            this.customCommandList = new Tibialyzer.PrettyListBox();
            this.customCommandList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customCommandList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.customCommandList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCommandList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.customCommandList.FormattingEnabled = true;
            this.customCommandList.ItemHeight = 20;
            this.customCommandList.Items.AddRange(new object[] { "Unknown Command" });
            this.customCommandList.Location = new System.Drawing.Point(3, 124);
            this.customCommandList.Name = "customCommandList";
            this.customCommandList.Size = new System.Drawing.Size(230, 135);
            this.customCommandList.TabIndex = 57;
            this.customCommandList.SelectedIndexChanged += new System.EventHandler(this.customCommandList_SelectedIndexChanged);
            ///
            /// selectUpgradeTibialyzerButton
            ///
            this.selectUpgradeTibialyzerButton = new System.Windows.Forms.Label();
            this.selectUpgradeTibialyzerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.selectUpgradeTibialyzerButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectUpgradeTibialyzerButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.selectUpgradeTibialyzerButton.Location = new System.Drawing.Point(3, 49);
            this.selectUpgradeTibialyzerButton.Name = "selectUpgradeTibialyzerButton";
            this.selectUpgradeTibialyzerButton.Padding = new System.Windows.Forms.Padding(10);
            this.selectUpgradeTibialyzerButton.Size = new System.Drawing.Size(534, 38);
            this.selectUpgradeTibialyzerButton.TabIndex = 56;
            this.selectUpgradeTibialyzerButton.Text = "Select Tibialyzer";
            this.selectUpgradeTibialyzerButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.selectUpgradeTibialyzerButton.Click += new System.EventHandler(this.selectUpgradeTibialyzerButton_Click);
            this.selectUpgradeTibialyzerButton.MouseEnter += new System.EventHandler(ControlMouseEnter);
            this.selectUpgradeTibialyzerButton.MouseLeave += new System.EventHandler(ControlMouseLeave);
            ///
            /// label4
            ///
            this.label4 = new System.Windows.Forms.Label();
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(156)))), ((int)(((byte)(65)))));
            this.label4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.label4.Location = new System.Drawing.Point(3, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(534, 30);
            this.label4.TabIndex = 55;
            this.label4.Text = "Import Settings From Previous Tibialyzer";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TabBase
            // 
            this.Controls.Add(customCommandParameterBox);
            this.Controls.Add(label8);
            this.Controls.Add(customCommandBox);
            this.Controls.Add(customCommandName);
            this.Controls.Add(label7);
            this.Controls.Add(label6);
            this.Controls.Add(customCommandList);
            this.Controls.Add(selectUpgradeTibialyzerButton);
            this.Controls.Add(label4);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Tibialyzer.Properties.Resources.background_image;
            this.ClientSize = new System.Drawing.Size(538, 514);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabBase";
            this.Text = "TabBase";
            this.ResumeLayout(false);

        }

        private EnterTextBox customCommandParameterBox;
        private System.Windows.Forms.Label label8;
        private EnterTextBox customCommandBox;
        private System.Windows.Forms.Label customCommandName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private PrettyListBox customCommandList;
        private System.Windows.Forms.Label selectUpgradeTibialyzerButton;
        private System.Windows.Forms.Label label4;
        #endregion
    }
}