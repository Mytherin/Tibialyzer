using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class ProgressBarLabel : Label {
        public double percentage;
        public Color backColor = StyleManager.BlendTransparencyKey;
        public bool reverse;
        public bool centerText;

        public ProgressBarLabel() {
            this.percentage = 1;
        }
        
        protected override void OnPaint(PaintEventArgs e) {
            int x = 5;
            if (!reverse) {
                x = (int)(this.Width - 10 - this.Text.Length * this.Font.Size);
            }
            if (centerText) {
                x = (int)(this.Width / 2);
            }
            SummaryForm.RenderText(e.Graphics, this.Text, x, Color.Empty, StyleManager.NotificationTextColor, Color.FromArgb(0, 0, 0), this.Height, 4, this.Font, true, System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor, backColor == StyleManager.TransparencyKey ? System.Drawing.Drawing2D.SmoothingMode.None : System.Drawing.Drawing2D.SmoothingMode.HighQuality);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            e.Graphics.Clear(backColor);
            using (Brush brush = new SolidBrush(this.BackColor)) {
                int x = 0;
                if (reverse) {
                    int offset = (int)(this.Width - this.Width * percentage);
                    x = offset;
                }
                e.Graphics.FillRectangle(brush, new Rectangle(x, 0, (int)(this.Width * percentage), this.Height));
            }
            using (Pen pen = new Pen(Color.Black, 2)) {
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, this.Width, this.Height));
            }
        }
    }
}
