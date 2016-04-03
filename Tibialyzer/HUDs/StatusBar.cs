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
    public enum StatusType { Health, Mana, Experience, ExpPerHour };

    public partial class StatusBar : BaseHUD {
        private StatusType statusType;
        private bool displayText;

        public StatusBar(StatusType statusType) {
            this.statusType = statusType;
            InitializeComponent();

            BackColor = StyleManager.TransparencyKey;
            TransparencyKey = StyleManager.TransparencyKey;

            displayText = SettingsManager.getSettingBool(GetHUD() + "DisplayText");
            double opacity = SettingsManager.getSettingDouble(GetHUD() + "Opacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;
        }

        public override void LoadHUD() {
            double fontSize = SettingsManager.getSettingDouble(GetHUD() + "FontSize");
            fontSize = fontSize < 0 ? 20 : fontSize;
            this.healthBarLabel.Font = new System.Drawing.Font("Verdana", (float)fontSize, System.Drawing.FontStyle.Bold);
            this.RefreshHUD(100, 100, 1);
            this.Load += StatusBar_Load;
        }

        private System.Timers.Timer timer;
        private void StatusBar_Load(object sender, EventArgs e) {
            timer = new System.Timers.Timer(10);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            RefreshHealth();
        }

        public static int GetExperience(int lvl) {
            return (50 * lvl * lvl * lvl - 150 * lvl * lvl + 400 * lvl) / 3;
        }

        public void RefreshHUD(int min, int max, double percentage) {
            if (displayText) {
                if (statusType == StatusType.Experience) {
                    healthBarLabel.Text = String.Format("Lvl {0}: {1}%", MemoryReader.Level, (int)(percentage * 100));
                } else {
                    healthBarLabel.Text = String.Format("{0}/{1}", min, max);
                }
            } else {
                healthBarLabel.Text = "";
            }
            healthBarLabel.percentage = percentage;
            if (statusType == StatusType.Health) {
                healthBarLabel.BackColor = StyleManager.GetHealthColor(percentage);
            } else if (statusType == StatusType.Mana) {
                healthBarLabel.BackColor = StyleManager.ManaColor;
            } else if (statusType == StatusType.Experience) {
                healthBarLabel.BackColor = StyleManager.ExperienceColor;
            }
        }

        public void RefreshHealth() {
            timer.Enabled = false;
            int life = 0, maxlife = 1;
            if (statusType == StatusType.Health) {
                life = MemoryReader.Health;
                maxlife = MemoryReader.MaxHealth;
            } else if (statusType == StatusType.Mana) {
                life = MemoryReader.Mana;
                maxlife = MemoryReader.MaxMana;
            } else if (statusType == StatusType.Experience) {
                int level = MemoryReader.Level;
                int baseExperience = GetExperience(level - 1);
                life = MemoryReader.Experience - baseExperience;
                maxlife = GetExperience(level) - baseExperience;
            }
            if (maxlife == 0) {
                life = 1;
                maxlife = 1;
            }
            double percentage = (double)life / maxlife;

            try {
                bool visible = ProcessManager.IsTibiaActive();
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD(life, maxlife, percentage);
                    this.Visible = visible;
                });
                timer.Enabled = true;
            } catch {
                if (timer != null) {
                    timer.Dispose();
                    timer = null;
                }
            }
        }

        public override string GetHUD() {
            switch (statusType) {
                case StatusType.Health:
                    return "HealthBar";
                case StatusType.Mana:
                    return "ManaBar";
                case StatusType.Experience:
                    return "ExperienceBar";
            }
            return "StatusBar";
        }
    }
}
