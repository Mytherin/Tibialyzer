using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tibialyzer
{
    class TransparentPictureBox : PictureBox
    {
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            Graphics g = e.Graphics;

            Control p = Parent;
            while (p != null)
            {
                // draw the background of the parent as background onto this 
                if (p is NotificationForm)
                {
                    NotificationForm c = (NotificationForm)p;
                    Bitmap bmp = new Bitmap(c.Width, c.Height, g);
                    c.PaintBackground(Graphics.FromImage(bmp));
                    Bitmap background = bmp.Clone(new Rectangle(new Point(Math.Max(Left, 0), Math.Max(Top, 0)), new Size(Math.Min(Size.Width, c.Width - Math.Max(Left, 0)), Math.Min(Size.Height, c.Height - Math.Max(Top, 0)))), bmp.PixelFormat);
                    e.Graphics.DrawImage(background, new Point(0, 0));
                    bmp.Dispose();
                    background.Dispose();
                    break;
                }
                if (p.Parent == p) break;
                p = p.Parent;
            }
        }
    }
}
