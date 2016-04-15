using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class ProgressBarLabel : Label {
        public double percentage;
        public Color backColor = StyleManager.BlendTransparencyKey;

        public ProgressBarLabel() {
            this.percentage = 1;
        }
        
        protected override void OnPaint(PaintEventArgs e) {
            SummaryForm.RenderText(e.Graphics, this.Text, 5, Color.Empty, StyleManager.NotificationTextColor, Color.FromArgb(64, 64, 64), this.Height, 4, this.Font, true, System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor, backColor == StyleManager.TransparencyKey ? System.Drawing.Drawing2D.SmoothingMode.None : System.Drawing.Drawing2D.SmoothingMode.HighQuality);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            e.Graphics.Clear(backColor);
            using (Brush brush = new SolidBrush(this.BackColor)) {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, (int)(this.Width * percentage), this.Height));
            }
        }
    }
}
