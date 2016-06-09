
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace Tibialyzer {
    public partial class PlayerForm : NotificationForm {
        public Player player;
        public List<Control> extraControls = new List<Control>();
        public PlayerForm() {
            InitializeComponent();
        }
        
        public void RefreshPlayer() {
            foreach (Control c in extraControls) this.Controls.Remove(c);
            extraControls.Clear();
            mainImage.Image = player.GetImage();
            nameLabel.Text = player.name.ToTitle();
            levelLabel.Text = String.Format("Level {0}", player.level);
            hpLabel.Text = String.Format("{0} Life", player.MaxLife().ToString("N0"));
            manaLabel.Text = String.Format("{0} Mana", player.MaxMana().ToString("N0"));
            capLabel.Text = String.Format("{0} Cap", player.Capacity());
            sharedLabel.Text = String.Format("Shared Range\n{0} - {1}", player.SharedLevelMin(), player.SharedLevelMax());
            vocationLabel.Text = player.GetVocation();
            
            Font f = StyleManager.FontList[0];
            Font prevFont = f;
            for (int i = 0; i < StyleManager.FontList.Count; i++) {
                Font font = StyleManager.FontList[i];
                int width = TextRenderer.MeasureText(this.nameLabel.Text, font).Width;
                if (width < this.mainImage.Size.Width) {
                    f = prevFont;
                } else {
                    break;
                }
                prevFont = font;
            }
            nameLabel.Font = f;

            accountLabel.Visible = player.additionalInfo;
            houseLabel.Visible = player.additionalInfo;
            guildLabel.Visible = player.additionalInfo;
            marriageLabel.Visible = player.additionalInfo;
            worldLabel.Visible = player.additionalInfo;

            if (player.additionalInfo) {
                accountLabel.Text = player.premium ? "Premium Account" : "Free Account";
                accountLabel.ForeColor = player.premium ? StyleManager.HealthHealthy : StyleManager.ElementFireColor;
                if (player.house != null) houseLabel.Text = String.Format("House: {0}", player.house);
                else houseLabel.Visible = false;
                if (player.guild != null) guildLabel.Text = String.Format("Guild: {0}", player.guild);
                else guildLabel.Visible = false;
                if (player.marriage != null) marriageLabel.Text = String.Format("Marriage: {0}", player.marriage);
                else marriageLabel.Visible = false;
                worldLabel.Text = String.Format("World: {0}", player.world);
                int i = 0;
                foreach(string death in player.recentDeaths) {
                    Label label = new Label();
                    label.Text = death;
                    label.Font = hpLabel.Font;
                    label.BackColor = Color.Transparent;
                    label.ForeColor = StyleManager.ElementFireColor;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.AutoSize = false;
                    int height = label.Text.Length < 50 ? 17 : 35;
                    label.Size = new Size(262, height);
                    label.Location = new Point(110, 106 + i);
                    this.Controls.Add(label);
                    extraControls.Add(label);
                    i += height;
                    if (i + 35 > 500) break;
                }
                this.Size = new Size(378, Math.Max(220, 106 + i + 10));
            } else {
                this.Size = new Size(200, 220);
            }
        }

        public override void LoadForm() {
            this.SuspendForm();
            base.NotificationInitialize();

            RefreshPlayer();

            base.NotificationFinalize();
            this.ResumeForm();
        }
                
        public override string FormName() {
            return "PlayerForm";
        }

        public override int MinWidth() {
            return 378;
        }

        public override int MaxWidth() {
            return 378;
        }

        public override int WidthInterval() {
            return 200;
        }
    }
}
