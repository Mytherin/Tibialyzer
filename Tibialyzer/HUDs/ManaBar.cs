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
    public partial class ManaBar : BaseHUD {
        private bool displayText;

        public ManaBar() {
            InitializeComponent();
            
            BackColor = StyleManager.BlendTransparencyKey;
            TransparencyKey = StyleManager.BlendTransparencyKey;

            displayText = SettingsManager.getSettingBool(GetHUD() + "DisplayText");
            double opacity = SettingsManager.getSettingDouble(GetHUD() + "Opacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;

            MemoryReader.ManaChanged += (o, e) => RefreshHUD(e);
            ProcessManager.TibiaVisibilityChanged += (o, e) => UpdateVisibility(e);
        }

        public override void LoadHUD() {
            double fontSize = SettingsManager.getSettingDouble(GetHUD() + "FontSize");
            fontSize = fontSize < 0 ? 20 : fontSize;
            this.manaBarLabel.Font = new System.Drawing.Font("Verdana", (float)fontSize, System.Drawing.FontStyle.Bold);
            this.RefreshHUD(100, 100);
        }

        private void RefreshHUD(long value, long max) {
            double percentage = ((double) value) / ((double) max);
            if (displayText) {
                manaBarLabel.Text = String.Format("{0}/{1}", value, max);
            } else {
                manaBarLabel.Text = "";
            }

            manaBarLabel.percentage = percentage;
            manaBarLabel.Size = this.Size;
            manaBarLabel.BackColor = StyleManager.ManaColor;
        }

        private void RefreshHUD(PlayerMana playerMp) {
            long mana = playerMp.Mana;
            long maxMana = playerMp.MaxMana;

            if (maxMana == 0) {
                mana = 1;
                maxMana = 1;
            }

            try
            {
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD(mana, maxMana);
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
            return "ManaBar";
        }
    }
}
