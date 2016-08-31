
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Tibialyzer {

    public class NotificationForm : Form {
        object timerLock = new object();
        object closeLock = new object();
        System.Timers.Timer closeTimer = null;
        Timer checkTimer = null;
        public static Bitmap background_image = null;
        public TibialyzerCommand command;
        protected PictureBox back_button;
        protected Label decreaseWidthButton;
        protected Label increaseWidthButton;
        public int notificationDuration = 1;
        protected bool hideWhenTibiaInactive = false;
        protected bool requireDoubleClick = false;
        protected bool clicked = false;
        protected Stopwatch doubleClickWatch = new Stopwatch();

        public NotificationForm() {
            this.ShowInTaskbar = Constants.OBSEnableWindowCapture;
            this.Name = String.Format("Tibialyzer ({0})", FormName());

            requireDoubleClick = SettingsManager.getSettingBool("NotificationDoubleClick");
            hideWhenTibiaInactive = SettingsManager.getSettingBool("NotificationShowTibiaActive");
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        public const int WM_SETREDRAW = 11;

        public void SuspendDrawing() {
            SendMessage(this.Handle, WM_SETREDRAW, false, 0);
        }

        public void ResumeDrawing() {
            SendMessage(this.Handle, WM_SETREDRAW, true, 0);
            this.Refresh();
        }
        public static void SuspendDrawing(Control c) {
            SendMessage(c.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control c) {
            SendMessage(c.Handle, WM_SETREDRAW, true, 0);
            c.Refresh();
        }

        public void SuspendForm() {
            this.SuspendLayout();
            this.SuspendDrawing();
        }

        public void ResumeForm() {
            this.ResumeLayout(false);
            if (this.Visible) {
                this.ResumeDrawing();
            }
        }

        Region fill_region = null;
        protected void NotificationInitialize() {
            this.BackgroundImage = background_image;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            if (notificationDuration != Constants.MaximumNotificationDuration) {
                closeTimer = new System.Timers.Timer(1000 * notificationDuration);
                closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(CloseNotification);
                closeTimer.Enabled = true;
            }

            foreach (Control c in this.Controls) {
                if (c is TextBox || c is CheckBox || c is TransparentChart || c is PrettyButton) continue;
                c.Click += c_Click;
            }

            if (MinWidth() != MaxWidth()) {
                increaseWidthButton = new Label();
                increaseWidthButton.BackColor = System.Drawing.Color.Transparent;
                increaseWidthButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                increaseWidthButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                increaseWidthButton.ForeColor = StyleManager.NotificationTextColor;
                increaseWidthButton.Padding = new System.Windows.Forms.Padding(2);
                increaseWidthButton.Size = new System.Drawing.Size(40, 21);
                increaseWidthButton.Text = "+";
                increaseWidthButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                increaseWidthButton.Click += new System.EventHandler(IncreaseWidth_Click);

                decreaseWidthButton = new Label();
                decreaseWidthButton.BackColor = System.Drawing.Color.Transparent;
                decreaseWidthButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                decreaseWidthButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                decreaseWidthButton.ForeColor = StyleManager.NotificationTextColor;
                decreaseWidthButton.Padding = new System.Windows.Forms.Padding(2);
                decreaseWidthButton.Size = new System.Drawing.Size(40, 21);
                decreaseWidthButton.Text = "-";
                decreaseWidthButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                decreaseWidthButton.Click += new System.EventHandler(DecreaseWidth_Click);

                this.Controls.Add(increaseWidthButton);
                this.Controls.Add(decreaseWidthButton);
                increaseWidthButton.BringToFront();
                decreaseWidthButton.BringToFront();
                NotificationForm_SizeChanged(null, null);
                this.SizeChanged += NotificationForm_SizeChanged;
            }

            if (hideWhenTibiaInactive) {
                ProcessManager.TibiaVisibilityChanged += (o, e) => UpdateHudVisibility(e);
            }
        }

        private void UpdateHudVisibility(bool visible) {
            try {
                this.Invoke((MethodInvoker)delegate {
                    this.Visible = visible;
                });

            } catch {
            }

            this.Invalidate();
        }
        
        private void NotificationForm_SizeChanged(object sender, EventArgs e) {
            increaseWidthButton.Location = new System.Drawing.Point(this.Width - 45, 3);
            decreaseWidthButton.Location = new System.Drawing.Point(this.Width - 85, 3);
        }

        public virtual void LoadForm() {

        }

        protected void refreshTimer() {
            lock (timerLock) {
                if (closeTimer != null) {
                    closeTimer.Dispose();
                    closeTimer = new System.Timers.Timer(1000 * notificationDuration);
                    closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(CloseNotification);
                    closeTimer.Enabled = true;
                }
            }
        }

        protected void RegisterForClose(Control c) {
            c.Click += c_Click;
        }

        protected void NotificationFinalize() {
            if (NotificationManager.HasBack()) {
                back_button = new PictureBox();
                back_button.Location = new Point(5, 5);
                back_button.Image = StyleManager.GetImage("back.png");
                back_button.Size = new Size(63, 22);
                back_button.BackColor = Color.Transparent;
                back_button.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                back_button.Click += back_button_Click;
                this.Controls.Add(back_button);
                this.back_button.BringToFront();
            }
            this.ReturnFocusToTibia();
        }

        void back_button_Click(object sender, EventArgs e) {
            NotificationManager.Back();
            this.ReturnFocusToTibia();
        }

        public static void Initialize() {
            background_image = new Bitmap(@"Images\background_image.png");
        }

        protected void Cleanup() {
            lock (timerLock) {
                if (closeTimer != null) {
                    closeTimer.Dispose();
                    closeTimer = null;
                }
            }
        }

        public void close() {
            lock (closeLock) {
                this.Close();
            }
        }

        protected void c_Click(object sender, EventArgs e) {
            if (requireDoubleClick) {
                if (!clicked) {
                    clicked = true;
                    doubleClickWatch.Reset();
                    doubleClickWatch.Start();
                } else if (doubleClickWatch.ElapsedMilliseconds < 1000) {
                    close();
                } else {
                    doubleClickWatch.Stop();
                    doubleClickWatch.Reset();
                    doubleClickWatch.Restart();
                }
            } else {
                close();
            }
        }

        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            c_Click(this, e);
        }

        public void ReturnFocusToTibia() {
            return;
        }

        protected void CloseForm(object sender, EventArgs e) {
            close();
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

                baseParams.ExStyle |= (int)(WS_EX_NOACTIVATE | WS_EX_COMPOSITED);
                if (!Constants.OBSEnableWindowCapture) {
                    baseParams.ExStyle |= WS_EX_TOOLWINDOW;
                }

                return baseParams;
            }
        }

        public void ResetTimer() {
            lock (timerLock) {
                if (closeTimer != null) {
                    this.closeTimer.Stop();
                    this.closeTimer.Start();
                }
            }
        }

        public void CloseNotification(object sender, EventArgs e) {
            if (this.Opacity <= 0) {
                lock (timerLock) {
                    if (closeTimer != null) {
                        closeTimer.Close();
                        closeTimer = null;
                    }
                }
                lock (closeLock) {
                    if (this.IsHandleCreated && !this.IsDisposed) {
                        this.BeginInvoke((MethodInvoker)delegate {
                            close();
                        });
                    }
                }
            } else {
                lock (closeLock) {
                    if (this.IsHandleCreated && !this.IsDisposed) {
                        this.BeginInvoke((MethodInvoker)delegate {
                            this.Opacity -= 0.03;
                        });
                    }
                }
                lock (timerLock) {
                    if (closeTimer != null) {
                        closeTimer.Interval = 20;
                        closeTimer.Start();
                    }
                }
            }
        }

        public void PaintBackground(Graphics e) {
            Rectangle r = new Rectangle((int)e.ClipBounds.Left, (int)e.ClipBounds.Top, (int)e.ClipBounds.Width, (int)e.ClipBounds.Height);
            OnPaintBackground(new PaintEventArgs(e, r));
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            if (fill_region != null) e.Graphics.FillRegion(Brushes.Black, fill_region);
            base.OnPaintBackground(e);
            using (Pen p = new Pen(Brushes.Black, 5)) {
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 2, Height - 2));
                /*e.Graphics.DrawLine(p, new Point(14, 0), new Point(0, 14));
                e.Graphics.DrawLine(p, new Point(16, Height), new Point(0, Height - 16));
                e.Graphics.DrawLine(p, new Point(Width - 14, 0), new Point(Width, 14));
                e.Graphics.DrawLine(p, new Point(Width - 16, Height), new Point(Width, Height - 16));*/
            }
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationForm));
            this.SuspendLayout();
            //
            // NotificationForm
            //
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NotificationForm";
            this.ResumeLayout(false);
        }

        public virtual string FormName() {
            return "NotificationForm";
        }

        public virtual int MinWidth() {
            return 200;
        }

        public virtual int MaxWidth() {
            return 200;
        }

        public virtual int WidthInterval() {
            return 72;
        }

        public int GetWidth() {
            return Math.Min(Math.Max(SettingsManager.getSettingInt(FormName() + "Width"), MinWidth()), MaxWidth());
        }

        public void SetWidth(int width) {
            width = Math.Min(Math.Max(width, MinWidth()), MaxWidth());
            SettingsManager.setSetting(FormName() + "Width", width);
        }

        private void ChangeSize(int modification) {
            SetWidth(GetWidth() + modification);
            RefreshForm();
        }

        private void DecreaseWidth_Click(object sender, EventArgs e) {
            ChangeSize(-WidthInterval());
        }

        private void IncreaseWidth_Click(object sender, EventArgs e) {
            ChangeSize(WidthInterval());
        }

        public virtual void RefreshForm() {

        }
    }
}
