using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class CurvedHUD : BaseHUD {
        private const int WS_EX_Transparent = 0x20;
        private const int WS_EX_Layered = 0x80000;
        private const int WS_EX_Composited = 0x02000000;

        public CurvedHUD() {
            InitializeComponent();

            BackColor = StyleManager.TransparencyKey;
            TransparencyKey = StyleManager.TransparencyKey;

            double opacity = SettingsManager.getSettingDouble("CurvedBarsOpacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_Composited | WS_EX_Transparent | WS_EX_Layered;
                return cp;
            }
        }

        public override string GetHUD() {
            return "CurvedBars";
        }
    }
}
