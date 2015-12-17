using System;
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

        protected void InitializeSimpleNotification() {
            this.Click += c_Click;
            foreach (Control c in this.Controls) {
                c.Click += c_Click;
            }

            moveTimer = new System.Timers.Timer(10);
            moveTimer.Elapsed += MoveTimer_Elapsed;
            moveTimer.Enabled = true;


            closeTimer = new System.Timers.Timer(8000);
            closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(CloseNotification);
            closeTimer.Enabled = true;
        }
        

        private void MoveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            int desktopX = this.DesktopLocation.X;
            int desktopY = this.DesktopLocation.Y;
            if (this.Visible && (desktopX != targetPositionX || desktopY != targetPositionY)) {
                int updatedX = Math.Abs(desktopX - targetPositionX) < 3 ? targetPositionX : (int)(desktopX + (targetPositionX - desktopX) / 10.0);
                int updatedY = Math.Abs(desktopY - targetPositionY) < 3 ? targetPositionY : (int)(desktopY + (targetPositionY - desktopY) / 10.0);
                
                this.Invoke((MethodInvoker)delegate {
                    if (!this.IsDisposed) {
                        this.SetDesktopLocation(updatedX, updatedY);
                    }
                });
            }
        }
        public void CloseNotification(object sender, EventArgs e) {
            if (moveTimer != null) {
                moveTimer.Dispose();
                moveTimer = null;
            }
            if (this.Opacity <= 0) {
                closeTimer.Close();

                this.Invoke((MethodInvoker)delegate {
                    close();
                });
            } else {
                if (this.IsHandleCreated && !this.IsDisposed) {
                    this.Invoke((MethodInvoker)delegate {
                        this.Opacity -= 0.03;
                    });
                    closeTimer.Interval = 20;
                    closeTimer.Start();
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

        protected void c_Click(object sender, EventArgs e) {
            close();
        }
    }
}
