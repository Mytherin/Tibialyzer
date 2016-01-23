
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tibialyzer {
    class SimpleNotification : Form {
        System.Timers.Timer moveTimer = null;
        public int targetPositionX = 0, targetPositionY = 0;
        System.Timers.Timer closeTimer = null;
        
        protected void InitializeSimpleNotification(bool movement = true, bool destroy = true) {
            this.Click += c_Click;
            foreach (Control c in this.Controls) {
                c.Click += c_Click;
            }

            if (movement) {
                moveTimer = new System.Timers.Timer(5);
                moveTimer.Elapsed += MoveTimer_Elapsed;
                moveTimer.Enabled = true;
            }
            if (destroy) {
                closeTimer = new System.Timers.Timer(8000);
                closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(CloseNotification);
                closeTimer.Enabled = true;
            }
        }
        

        private void MoveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            int desktopX = this.DesktopLocation.X;
            int desktopY = this.DesktopLocation.Y;
            if (this.Visible && (desktopX != targetPositionX || desktopY != targetPositionY)) {
                int updatedX = Math.Abs(desktopX - targetPositionX) < 3 ? targetPositionX : (int)(desktopX + (targetPositionX - desktopX) / 5.0);
                int updatedY = Math.Abs(desktopY - targetPositionY) < 3 ? targetPositionY : (int)(desktopY + (targetPositionY - desktopY) / 5.0);
                try {
                    this.Invoke((MethodInvoker)delegate {
                        if (!this.IsDisposed) {
                            this.SetDesktopLocation(updatedX, updatedY);
                        }
                    });
                } catch {

                }
            }
        }

        public void ClearTimers() {
            if (moveTimer != null) {
                moveTimer.Dispose();
                moveTimer = null;
            }

            if (closeTimer != null) {
                closeTimer.Dispose();
                closeTimer = null;
            }
        }

        public void CloseNotification(object sender, EventArgs e) {
            if (moveTimer != null) {
                moveTimer.Dispose();
                moveTimer = null;
            }
            if (this.Opacity <= 0) {
                closeTimer.Close();

                try {
                    this.Invoke((MethodInvoker)delegate {
                        close();
                    });
                } catch {

                }
            } else {
                if (this.IsHandleCreated && !this.IsDisposed) {
                    try {
                        this.Invoke((MethodInvoker)delegate {
                            this.Opacity -= 0.03;
                        });
                        closeTimer.Interval = 20;
                        closeTimer.Start();
                    } catch {

                    }
                }

            }
        }
        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        static int WS_EX_NOACTIVATE = 0x08000000;
        static int WS_EX_TOOLWINDOW = 0x00000080;

        protected override CreateParams CreateParams {
            get {
                CreateParams baseParams = base.CreateParams;

                baseParams.ExStyle |= (int)(
                  WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);

                return baseParams;
            }
        }

        public void close() {
            try {
                closeTimer.Dispose();
                closeTimer = null;
                this.Close();
            } catch {

            }
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleNotification));
            this.SuspendLayout();
            // 
            // SimpleNotification
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimpleNotification";
            this.ResumeLayout(false);

        }

        protected void c_Click(object sender, EventArgs e) {
            close();
        }
    }
}
