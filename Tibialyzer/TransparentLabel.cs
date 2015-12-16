using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public class TransparentLabel : RichTextBox {
        public TransparentLabel() {
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            this.TextChanged += TransparentLabel_TextChanged;
            this.MouseDown += TransparentLabel_MouseDown;
            this.MouseUp += TransparentLabel_MouseUp;
            this.KeyDown += TransparentLabel_KeyDown;
            this.VScroll += TransparentLabel_TextChanged;
            this.HScroll += TransparentLabel_TextChanged;
        }

        void TransparentLabel_KeyDown(object sender, KeyEventArgs e) {
            this.ForceRefresh();
        }

        void TransparentLabel_MouseUp(object sender, MouseEventArgs e) {
            this.ForceRefresh();
        }

        void TransparentLabel_MouseDown(object sender, MouseEventArgs e) {
            this.ForceRefresh();
        }

        void TransparentLabel_TextChanged(object sender, System.EventArgs e) {
            this.ForceRefresh();
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams parms = base.CreateParams;
                parms.ExStyle |= 0x20;  // Turn on WS_EX_TRANSPARENT
                return parms;
            }
        }
        public void ForceRefresh() {
            this.UpdateStyles();
        }
    }
}
