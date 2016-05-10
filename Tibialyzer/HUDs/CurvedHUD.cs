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
        public CurvedHUD() {
            InitializeComponent();

            BackColor = StyleManager.TransparencyKey;
            TransparencyKey = StyleManager.TransparencyKey;

            double opacity = SettingsManager.getSettingDouble("CurvedBarsOpacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;
        }
        
        public override string GetHUD() {
            return "CurvedBars";
        }
    }
}
