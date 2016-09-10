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
    public partial class ExperienceBar : BaseHUD {
        private bool displayText;

        public ExperienceBar() {
            InitializeComponent();
            
            BackColor = StyleManager.BlendTransparencyKey;
            TransparencyKey = StyleManager.BlendTransparencyKey;

            displayText = SettingsManager.getSettingBool(GetHUD() + "DisplayText");
            double opacity = SettingsManager.getSettingDouble(GetHUD() + "Opacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;

            MemoryReader.ExperienceChanged += (o, e) => RefreshHUD(e);
            ProcessManager.TibiaVisibilityChanged += (o, e) => UpdateVisibility(e);
        }

        public override void LoadHUD() {
            double fontSize = SettingsManager.getSettingDouble(GetHUD() + "FontSize");
            fontSize = fontSize < 0 ? 20 : fontSize;
            this.experienceBarLabel.Font = new System.Drawing.Font("Verdana", (float)fontSize, System.Drawing.FontStyle.Bold);
            this.RefreshHUD(100, 100, 1);
        }

        public static long GetExperience(long lvl) {
            return (50 * lvl * lvl * lvl - 150 * lvl * lvl + 400 * lvl) / 3;
        }

        private void RefreshHUD(long value, long max, int level) {
            double percentage = ((double) value) / ((double) max);
            if (displayText) {
                experienceBarLabel.Text = String.Format("Lvl {0}: {1}%", level, (int)(percentage * 100));
            } else {
                experienceBarLabel.Text = "";
            }

            experienceBarLabel.percentage = percentage;
            experienceBarLabel.Size = this.Size;
            experienceBarLabel.BackColor = StyleManager.ExperienceColor;
        }

        private void RefreshHUD(PlayerExperience playerExp) {
            int level = playerExp.Level;
            long baseExperience = GetExperience(level - 1);
            long exp = playerExp.Experience - baseExperience;
            long maxExp = GetExperience(level) - baseExperience;

            if (maxExp == 0) {
                exp = 1;
                maxExp = 1;
            }

            try
            {
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD(exp, maxExp, level);
                });
            }
            catch
            {
            }
        }
        
        public override string GetHUD() {
            return "ExperienceBar";
        }
    }
}
