using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tibialyzer.Structures;

namespace Tibialyzer {
    public partial class HealthBar : BaseHUD {
        private bool displayText;

        public HealthBar() {
            InitializeComponent();
            
            BackColor = StyleManager.BlendTransparencyKey;
            TransparencyKey = StyleManager.BlendTransparencyKey;

            displayText = SettingsManager.getSettingBool(GetHUD() + "DisplayText");
            double opacity = SettingsManager.getSettingDouble(GetHUD() + "Opacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;

            MemoryReader.HealthChanged += (o, e) => RefreshHUD(e);
            ProcessManager.TibiaVisibilityChanged += (o, e) => UpdateVisibility(e);
        }

        public override void LoadHUD() {
            double fontSize = SettingsManager.getSettingDouble(GetHUD() + "FontSize");
            fontSize = fontSize < 0 ? 20 : fontSize;
            this.healthBarLabel.Font = new System.Drawing.Font("Verdana", (float)fontSize, System.Drawing.FontStyle.Bold);
            this.RefreshHUD(100, 100);
        }

        private void RefreshHUD(long value, long max) {
            double percentage = ((double) value) / ((double) max);
            if (displayText) {
                healthBarLabel.Text = String.Format("{0}/{1}", value, max);
            } else {
                healthBarLabel.Text = "";
            }

            healthBarLabel.percentage = percentage;
            healthBarLabel.Size = this.Size;
            healthBarLabel.BackColor = StyleManager.GetHealthColor(percentage);
        }

        private void RefreshHUD(PlayerHealth playerHp) {
            long life = playerHp.Health;
            long maxlife = playerHp.MaxHealth;

            if (maxlife == 0) {
                life = 1;
                maxlife = 1;
            }

            try
            {
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD(life, maxlife);
                });
            }
            catch
            {
            }
        }

        private void UpdateVisibility(bool visible) {
            try
            {
                this.Invoke((MethodInvoker)delegate {
                    this.Visible = alwaysShow || visible;
                });
            }
            catch
            {
            }
        }

        public override string GetHUD() {
            return "HealthBar";
        }
    }
}
