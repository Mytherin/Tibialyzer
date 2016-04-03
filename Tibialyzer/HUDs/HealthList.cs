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
    class PlayerEntry {
        public string name = null;
        public int playerid = -1;
        public Image playerImage = null;
        public ProgressBarLabel healthBar = null;
        public Label playerNameLabel = null;
        public PictureBox playerIconLabel = null;

        ~PlayerEntry() {
            if (playerImage != null) {
                playerImage.Dispose();
            }
        }
    }

    public partial class HealthList : BaseHUD {
        private bool displayText;
        private bool displayNames;
        private bool displayIcons;
        private int playerBarHeight;
        private int healthBarHeight;
        private Font baseFont;
        private List<PlayerEntry> players = new List<PlayerEntry>();

        public HealthList() {
            InitializeComponent();

            this.BackgroundImage = StyleManager.GetImage("background_image.png");

            displayNames = SettingsManager.getSettingBool(GetHUD() + "DisplayNames");
            displayIcons = SettingsManager.getSettingBool(GetHUD() + "DisplayIcons");
            displayText = SettingsManager.getSettingBool(GetHUD() + "DisplayText");
            healthBarHeight = SettingsManager.getSettingInt(GetHUD() + "Height");
            playerBarHeight = displayNames ? healthBarHeight * 5 / 3 : healthBarHeight;

            List<string> names = SettingsManager.getSetting(GetHUD() + "PlayerNames");
            int index = 0;
            foreach(string name in names) {
                if (name.Trim() == "") continue;
                string imagePath = SettingsManager.getSettingString(GetHUD() + "Image" + index.ToString());
                Image image = null;
                if (imagePath != null) {
                    try {
                        image = Image.FromFile(imagePath);
                    } catch { }
                }
                this.players.Add(new PlayerEntry { name = name, playerImage = image });
                index++;
            }

            double opacity = SettingsManager.getSettingDouble(GetHUD() + "Opacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;
        }

        public override void LoadHUD() {
            double fontSize = SettingsManager.getSettingDouble(GetHUD() + "FontSize");
            fontSize = fontSize < 0 ? 20 : fontSize;
            baseFont = new System.Drawing.Font("Verdana", (float)fontSize, System.Drawing.FontStyle.Bold);
            this.RefreshHUD();
            this.Load += HealthList_Load;
        }

        private System.Timers.Timer timer;
        private void HealthList_Load(object sender, EventArgs e) {
            timer = new System.Timers.Timer(10);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            RefreshHealth();
        }
        
        public void RefreshHUD() {
            lock(players) {
                int playerIndex = 0;
                for (int index = 0; index < players.Count; index++) {
                    PlayerEntry player = players[index];
                    if (player.playerid < 0) {
                        player.playerid = MemoryReader.GetPlayerID(player.name);
                    }
                    if (player.playerid >= 0) {
                        if (player.healthBar == null) {
                            int basePositionX = 0;
                            int basePositionY = playerIndex * playerBarHeight;
                            if (displayNames) {
                                player.playerNameLabel = new Label();
                                player.playerNameLabel.Text = player.name;
                                player.playerNameLabel.Location = new Point(basePositionX, basePositionY);
                                player.playerNameLabel.Size = new Size(this.Size.Width, healthBarHeight * 2 / 3);
                                player.playerNameLabel.ForeColor = StyleManager.MainFormButtonForeColor;
                                player.playerNameLabel.BackColor = Color.Transparent;
                                player.playerNameLabel.Font = baseFont;
                                this.Controls.Add(player.playerNameLabel);
                                basePositionY += healthBarHeight * 2 / 3;
                            }
                            if (displayIcons && player.playerImage != null) {
                                player.playerIconLabel = new PictureBox();
                                player.playerIconLabel.Size = new Size(healthBarHeight, healthBarHeight);
                                player.playerIconLabel.Location = new Point(basePositionX, basePositionY);
                                player.playerIconLabel.BackColor = Color.Transparent;
                                player.playerIconLabel.SizeMode = PictureBoxSizeMode.Zoom;
                                player.playerIconLabel.Image = player.playerImage;
                                this.Controls.Add(player.playerIconLabel);
                                basePositionX += healthBarHeight;
                            }
                            player.healthBar = new ProgressBarLabel();
                            player.healthBar.Location = new Point(basePositionX, basePositionY);
                            player.healthBar.Size = new Size(this.Size.Width - basePositionX, healthBarHeight);
                            player.healthBar.percentage = 100;
                            player.healthBar.Font = baseFont;
                            player.healthBar.Text = "";
                            this.Controls.Add(player.healthBar);

                            for (int j = index + 1; j < players.Count; j++) {
                                if (players[j].healthBar != null) {
                                    players[j].healthBar.Location = new Point(players[j].healthBar.Location.X, players[j].healthBar.Location.Y + playerBarHeight);
                                }
                                if (players[j].playerIconLabel != null) {
                                    players[j].playerIconLabel.Location = new Point(players[j].playerIconLabel.Location.X, players[j].playerIconLabel.Location.Y + playerBarHeight);
                                }
                                if (players[j].playerNameLabel != null) {
                                    players[j].playerNameLabel.Location = new Point(players[j].playerNameLabel.Location.X, players[j].playerNameLabel.Location.Y + playerBarHeight);
                                }
                            }
                            this.Size = new Size(this.Size.Width, playerBarHeight * players.Count);
                        }
                        int percentage = MemoryReader.GetHealthPercentage(player.playerid);
                        if (displayText) {
                            player.healthBar.Text = String.Format("{0}%", percentage);
                        }
                        player.healthBar.percentage = percentage;
                        player.healthBar.BackColor = StyleManager.GetHealthColor(percentage / 100.0);
                        playerIndex++;
                    }
                }
            }
        }

        public void RefreshHealth() {
            timer.Enabled = false;
            try {
                bool visible = ProcessManager.IsTibiaActive();
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD();
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
            return "HealthList";
        }
    }
}
