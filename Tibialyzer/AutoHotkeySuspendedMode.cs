using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class AutoHotkeySuspendedMode : SimpleNotification {
        private System.Windows.Forms.Label typeModeLabel;

        public AutoHotkeySuspendedMode() {
            this.InitializeComponent();

            this.InitializeSimpleNotification(false, false);
        }

        private void InitializeComponent() {
            this.typeModeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // typeModeLabel
            // 
            this.typeModeLabel.AutoSize = true;
            this.typeModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeModeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.typeModeLabel.Location = new System.Drawing.Point(12, 9);
            this.typeModeLabel.Name = "typeModeLabel";
            this.typeModeLabel.Size = new System.Drawing.Size(215, 42);
            this.typeModeLabel.TabIndex = 0;
            this.typeModeLabel.Text = "Type Mode";
            // 
            // AutoHotkeySuspendedMode
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(236, 61);
            this.Controls.Add(this.typeModeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoHotkeySuspendedMode";
            this.Text = "AutoHotkey Suspended";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
