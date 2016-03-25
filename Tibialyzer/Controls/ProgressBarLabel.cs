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

        public ProgressBarLabel() {
            this.percentage = 1;
        }

        private void TileImage(Graphics gr, Image image) {
            lock(image) {
                for (int y = 0; y < this.Size.Height; y += image.Height) {
                    for(int x = 0; x < this.Size.Width; x += image.Width) {
                        gr.DrawImage(image, new Point(x, y));
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            SummaryForm.RenderText(e.Graphics, this.Text, 5, Color.Empty, Color.FromArgb(191, 191, 191), Color.FromArgb(64, 64, 64), this.Height, 4, this.Font, true);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            TileImage(e.Graphics, this.Parent.BackgroundImage);
            using (Brush brush = new SolidBrush(this.BackColor)) {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, (int)(this.Width * percentage), this.Height));
            }
        }
    }
}
