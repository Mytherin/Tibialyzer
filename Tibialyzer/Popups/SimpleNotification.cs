
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tibialyzer {
    public class SimpleNotification : Form {
        System.Timers.Timer moveTimer = null;
        public int targetPositionX = 0, targetPositionY = 0;
        public double targetOpacity;
        System.Timers.Timer closeTimer = null;
        private bool animations = true;

        public SimpleNotification() {
            this.ShowInTaskbar = false;
            targetOpacity = 1.0;
        }

        protected void InitializeSimpleNotification(bool movement = true, bool destroy = true, double extraTime = 0) {
            this.Click += c_Click;
            foreach (Control c in this.Controls) {
                c.Click += c_Click;
            }

            this.animations = SettingsManager.getSettingBool("EnableSimpleNotificationAnimation");

            if (movement) {
                moveTimer = new System.Timers.Timer(5);
                moveTimer.Elapsed += MoveTimer_Elapsed;
                moveTimer.Start();
            }
            if (destroy) {
                closeTimer = new System.Timers.Timer((Math.Max(SettingsManager.getSettingInt("PopupDuration"), 1) + extraTime) * 1000);
                closeTimer.Elapsed += CloseTimer_Elapsed;
                closeTimer.Start();
            }
        }
        
        private int moveLock = 0;
        private void MoveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (moveLock > 0) return;
            moveLock++;
            int desktopX = this.DesktopLocation.X;
            int desktopY = this.DesktopLocation.Y;
            if (this.Visible && (desktopX != targetPositionX || desktopY != targetPositionY || targetOpacity != Opacity)) {
                int updatedX, updatedY;
                double updatedOpacity;
                if (animations) {
                    updatedX = Math.Abs(desktopX - targetPositionX) < 3 ? targetPositionX : (int)(desktopX + (targetPositionX - desktopX) / 5.0);
                    updatedY = Math.Abs(desktopY - targetPositionY) < 3 ? targetPositionY : (int)(desktopY + (targetPositionY - desktopY) / 5.0);
                    updatedOpacity = Math.Abs(targetOpacity - Opacity) < 0.1 ? targetOpacity : Opacity + (targetOpacity - Opacity) / 10.0;
                } else {
                    updatedX = targetPositionX;
                    updatedY = targetPositionY;
                    updatedOpacity = targetOpacity;
                }
                try {
                    this.Invoke((MethodInvoker)delegate {
                        if (!this.IsDisposed) {
                            this.SetDesktopLocation(updatedX, updatedY);
                            this.Opacity = updatedOpacity;
                            if (updatedOpacity <= 0 || (targetOpacity == 0 && !animations)) {
                                moveTimer.Dispose();
                                close();
                            }
                        }
                    });
                } catch {
                }
                moveTimer.Interval = 5;
            } else {
                moveTimer.Interval = 100;
            }
            moveLock = 0;
        }

        private void CloseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            closeTimer.Dispose();
            closeTimer = null;
            if (this.animations) {
                targetOpacity = 0;
            } else {
                try {
                    this.Invoke((MethodInvoker)delegate {
                        close();
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
                if (closeTimer != null) {
                    closeTimer.Dispose();
                    closeTimer = null;
                }
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
