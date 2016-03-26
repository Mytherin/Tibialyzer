
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class AutoHotkeySuspendedMode : SimpleNotification {
        private System.Windows.Forms.Label typeModeLabel;
        System.Timers.Timer showTimer = null;
        private bool alwaysShow = false;

        public AutoHotkeySuspendedMode(bool alwaysShow) {
            this.InitializeComponent();

            this.ShowInTaskbar = false;

            this.alwaysShow = alwaysShow;
            this.InitializeSimpleNotification(false, false);

            showTimer = new System.Timers.Timer(50);
            showTimer.Elapsed += ShowTimer_Elapsed;
            showTimer.Enabled = true;
        }

        private void ShowTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (alwaysShow) return;
            if (ProcessManager.IsFlashClient()) {
                return;
            }
            try {
                if (showTimer == null || this.IsDisposed || this.Disposing) {
                    return;
                }
                // only show the suspended window when tibia is active
                bool visible = ProcessManager.IsTibiaActive();
                this.BeginInvoke((MethodInvoker)delegate {
                    this.Visible = visible;
                });
            } catch {

            }
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
            this.typeModeLabel.Size = new System.Drawing.Size(218, 42);
            this.typeModeLabel.TabIndex = 0;
            this.typeModeLabel.Text = "Suspended";
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
