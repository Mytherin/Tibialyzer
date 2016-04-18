using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class PopupContainer : Form {
        public static Color ContainerTransparentColor = Color.Fuchsia;

        private object notificationLock = new object();
        private List<List<Control>> notifications = new List<List<Control>>();

        public bool UpsideDown = true;

        public PopupContainer() {
            InitializeComponent();

            this.Size = new Size(SettingsManager.getSettingInt("SimpleNotificationWidth"), this.Size.Height);

            this.BackColor = ContainerTransparentColor;
            this.TransparencyKey = ContainerTransparentColor;

            this.ShowInTaskbar = false;
        }

        public int TranslateY(int y, int height) {
            return UpsideDown ? this.Size.Height - y - 60 + height : y;
        }

        public void ShowNotification(SimpleNotification notification) {
            lock (notificationLock) {
                List<Control> controls = new List<Control>();
                int x = 0, y = notifications.Count * 65;

                // background label
                Label background = new Label();
                background.Size = notification.Size;
                background.BackColor = notification.BackColor;
                background.Location = new Point(x, UpsideDown ? this.Size.Height - y - background.Height : y);
                background.Click += Background_Click;
                this.Controls.Add(background);

                foreach (Control c in notification.Controls) {
                    controls.Add(c);
                }

                foreach (Control c in controls) {
                    c.Location = new Point(x + c.Location.X, background.Location.Y + c.Location.Y);
                    this.Controls.Add(c);
                    if (c.BackgroundImage == null) {
                        c.BackColor = notification.BackColor;
                    }
                    if (c.Name.Length == 0) {
                        c.Click += Background_Click;
                    }
                    c.BringToFront();
                }

                notifications.Add(controls);
                controls.Add(background);

                System.Threading.Tasks.Task.Delay(Math.Max(SettingsManager.getSettingInt("PopupDuration"), 1) * 1000).ContinueWith(t => ClosePopup()).Start();
            }
        }

        private void Background_Click(object sender, EventArgs e) {
            lock (notificationLock) {
                for(int i = 0; i < notifications.Count; i++) {
                    if (notifications[i].Contains(sender)) {
                        foreach(Control c in notifications[i]) {
                            this.Controls.Remove(c);
                        }
                        notifications.RemoveAt(i);
                        for(int j = i; j < notifications.Count; j++) {
                            foreach (Control c in notifications[j]) {
                                c.Location = new Point(c.Location.X, UpsideDown ? c.Location.Y + 65 : c.Location.Y - 65);
                            }
                        }
                        break;
                    }
                }
            }
        }

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        static int WS_EX_NOACTIVATE = 0x08000000;
        static int WS_EX_TOOLWINDOW = 0x00000080;
        static int WS_EX_COMPOSITED = 0x02000000;

        protected override CreateParams CreateParams {
            get {
                CreateParams baseParams = base.CreateParams;

                baseParams.ExStyle |= (int)(
                  WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_COMPOSITED);

                return baseParams;
            }
        }

        private void ClosePopup() {
            this.Invoke((MethodInvoker)delegate {
                lock (notificationLock) {
                    if (notifications.Count == 0) return;
                    // remove the first notification
                    List<Control> controls = notifications[0];
                    foreach (Control c in controls) {
                        this.Controls.Remove(c);
                    }
                    notifications.RemoveAt(0);
                    foreach (List<Control> notification in notifications) {
                        foreach (Control c in notification) {
                            c.Location = new Point(c.Location.X, UpsideDown ? c.Location.Y + 65 : c.Location.Y - 65);
                        }
                    }
                }
            });
        }
    }
}
