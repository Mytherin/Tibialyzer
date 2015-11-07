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
        System.Timers.Timer closeTimer = null;
        static Bitmap background_image = null;
        protected PictureBox back_button;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public void SuspendDrawing() {
            SendMessage(this.Handle, WM_SETREDRAW, false, 0);
        }

        public void ResumeDrawing() {
            SendMessage(this.Handle, WM_SETREDRAW, true, 0);
            this.Refresh();
        }

        public void SuspendForm() {
            this.SuspendLayout();
            this.SuspendDrawing();
        }

        public void ResumeForm() {
            this.ResumeLayout(false);
            this.ResumeDrawing();
        }

        Region fill_region = null;
        protected void NotificationInitialize() {
            this.BackgroundImage = background_image;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            closeTimer = new System.Timers.Timer(1000 * MainForm.mainForm.notification_seconds);
            closeTimer.Elapsed += new System.Timers.ElapsedEventHandler(CloseNotification);
            closeTimer.Enabled = true;

            foreach (Control c in this.Controls) {
                if (c is TextBox || c is CheckBox || c is System.Windows.Forms.DataVisualization.Charting.Chart) continue;
                c.Click += c_Click;
            }
        }

        protected void RegisterForClose(Control c) {
            c.Click += c_Click;
        }

        protected void NotificationFinalize() {
            if (MainForm.mainForm.HasBack()) {
                back_button = new PictureBox();
                back_button.Location = new Point(5, 5);
                back_button.Image = MainForm.back_image;
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
            MainForm.mainForm.Back();
            this.ReturnFocusToTibia();
        }

        public static void Initialize() {
            background_image = new Bitmap(@"Images\background_image.png");
        }

        protected void Cleanup() {
            if (closeTimer != null) closeTimer.Dispose();
        }

        public void close() {
            try {
                this.Invoke((MethodInvoker)delegate {
                    this.Close();
                });
            } catch {

            }
        }

        protected void c_Click(object sender, EventArgs e) {
            close();
        }

        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            close();
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        public void ReturnFocusToTibia() {
            Process[] tibia_process = Process.GetProcessesByName("Tibia");
            if (tibia_process.Length != 0) {
                SetForegroundWindow(tibia_process[0].MainWindowHandle);
                tibia_process[0].Dispose();
            }
        }

        protected void CloseForm(object sender, EventArgs e) {
            close();
        }

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        protected void ResetTimer() {
            this.closeTimer.Stop();
            this.closeTimer.Start();
        }

        public void CloseNotification(object sender, EventArgs e) {
            if (this.Opacity <= 0) {
                closeTimer.Close();
                close();
            } else {
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

        public void PaintBackground(Graphics e) {
            Rectangle r = new Rectangle((int)e.ClipBounds.Left, (int)e.ClipBounds.Top, (int)e.ClipBounds.Width, (int)e.ClipBounds.Height);
            OnPaintBackground(new PaintEventArgs(e, r));
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            if (fill_region != null) e.Graphics.FillRegion(Brushes.Black, fill_region);
            base.OnPaintBackground(e);
            using (Pen p = new Pen(Brushes.Black, 5)) {
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 2, Height - 2));
                e.Graphics.DrawLine(p, new Point(14, 0), new Point(0, 14));
                e.Graphics.DrawLine(p, new Point(16, Height), new Point(0, Height - 16));
                e.Graphics.DrawLine(p, new Point(Width - 14, 0), new Point(Width, 14));
                e.Graphics.DrawLine(p, new Point(Width - 16, Height), new Point(Width, Height - 16));
            }
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // NotificationForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "NotificationForm";
            this.ResumeLayout(false);

        }

    }
}
