using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace Tibialyzer {
    class PrettyDropDownList : ComboBox {
        public PrettyDropDownList() {
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(this.Location, this.Size));
            e.Graphics.DrawRectangle(new Pen(StyleManager.BorderColor, 1), new Rectangle(0, 0, this.Width - 1, this.Height - 1));

            SizeF size = e.Graphics.MeasureString(this.Text, this.Font);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), new Point(10, 4));
        }
    }
}
