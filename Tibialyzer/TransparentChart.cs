using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Tibialyzer {
    class TransparentChart : Chart {
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);

            Control p = Parent;
            while (p != null) {
                // draw the background of the parent as background onto this 
                if (p is NotificationForm) {
                    NotificationForm c = (NotificationForm)p;
                    c.Controls.Remove(this);
                    break;
                }
                if (p.Parent == p) break;
                p = p.Parent;
            }
            MainForm.mainForm.CloseNotification();
        }
    }
}
