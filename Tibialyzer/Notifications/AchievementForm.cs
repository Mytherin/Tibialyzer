
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
    public partial class AchievementForm : NotificationForm {
        public Achievement achievement;
        public AchievementForm() {
            InitializeComponent();
        }
        
        public override void LoadForm() {
            this.SuspendForm();
            base.NotificationInitialize();
            nameLabel.Text = achievement.GetName();
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
            gradeLabel.Text = String.Format("{0} Grade", achievement.grade);
            pointsLabel.Text = String.Format("{0} Points", achievement.points);
            mainImage.Image = achievement.GetImage();
            descriptionLabel.Text = String.Format("Description: {0}\n\nSpoiler: {1}", achievement.description, achievement.spoiler);

            base.NotificationFinalize();
            this.ResumeForm();
        }
        
        public override string FormName() {
            return "AchievementForm";
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

        private void AchievementForm_Load(object sender, EventArgs e) {

        }
    }
}
