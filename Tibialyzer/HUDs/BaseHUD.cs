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
        public virtual void ShowHUD() { this.Show(); }

        protected bool alwaysShow = false;

        static int WS_EX_TRANSPARENT = 0x20;
        static int WS_EX_LAYERED = 0x80000;
        static int WS_EX_NOACTIVATE = 0x08000000;
        static int WS_EX_TOOLWINDOW = 0x00000080;
        static int WS_EX_COMPOSITED = 0x02000000;

        public BaseHUD() {
            this.ShowInTaskbar = Constants.OBSEnableWindowCapture;
            this.alwaysShow = SettingsManager.getSettingBool("AlwaysShowHUD");
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams baseParams = base.CreateParams;
                
                baseParams.ExStyle |= WS_EX_NOACTIVATE | WS_EX_COMPOSITED | WS_EX_LAYERED | WS_EX_TRANSPARENT;
                if (!Constants.OBSEnableWindowCapture) {
                    baseParams.ExStyle |= WS_EX_TOOLWINDOW;
                }

                return baseParams;
            }
        }
    }
}
