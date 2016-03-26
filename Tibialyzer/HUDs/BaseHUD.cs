using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public class BaseHUD : Form {
        public virtual string GetHUD() { return ""; }
        public virtual void LoadHUD() { }

        static int WS_EX_NOACTIVATE = 0x08000000;
        static int WS_EX_TOOLWINDOW = 0x00000080;
        static int WS_EX_COMPOSITED = 0x02000000;

        public BaseHUD() {
            this.ShowInTaskbar = false;
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams baseParams = base.CreateParams;

                baseParams.ExStyle |= (int)(
                  WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_COMPOSITED);

                return baseParams;
            }
        }

    }
}
