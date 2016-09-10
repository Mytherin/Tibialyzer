
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class OutfiterForm : NotificationForm {
        private PictureBox outfiterImageBox;
        private PrettyButton rotateRightButton;
        private PrettyButton rotateLeftButton;
        private PrettyButton secondaryColorButton;
        private PrettyButton detailColorButton;
        private PrettyButton primaryColorButton;
        private PrettyButton headColorButton;
        private PrettyButton rotateLabel;
        private PrettyButton outfitLabel;
        private PrettyButton outfitRightButton;
        private PrettyButton outfitLeftButton;
        private PrettyButton mountLabel;
        private PrettyButton mountRightButton;
        private PrettyButton mountLeftButton;
        private PrettyButton copyTextButton;
        private PrettyButton addon1Toggle;
        private PrettyButton addon2Toggle;
        private PrettyButton genderToggle;
        public OutfiterOutfit outfit;
        public OutfiterForm(OutfiterOutfit outfit) {
            this.outfit = outfit;
            this.InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutfiterForm));
            this.outfiterImageBox = new System.Windows.Forms.PictureBox();
            this.rotateRightButton = new Tibialyzer.PrettyButton();
            this.rotateLeftButton = new Tibialyzer.PrettyButton();
            this.secondaryColorButton = new Tibialyzer.PrettyButton();
            this.detailColorButton = new Tibialyzer.PrettyButton();
            this.primaryColorButton = new Tibialyzer.PrettyButton();
            this.headColorButton = new Tibialyzer.PrettyButton();
            this.rotateLabel = new Tibialyzer.PrettyButton();
            this.outfitLabel = new Tibialyzer.PrettyButton();
            this.outfitRightButton = new Tibialyzer.PrettyButton();
            this.outfitLeftButton = new Tibialyzer.PrettyButton();
            this.mountLabel = new Tibialyzer.PrettyButton();
            this.mountRightButton = new Tibialyzer.PrettyButton();
            this.mountLeftButton = new Tibialyzer.PrettyButton();
            this.copyTextButton = new Tibialyzer.PrettyButton();
            this.addon1Toggle = new Tibialyzer.PrettyButton();
            this.addon2Toggle = new Tibialyzer.PrettyButton();
            this.genderToggle = new Tibialyzer.PrettyButton();
            ((System.ComponentModel.ISupportInitialize)(this.outfiterImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // outfiterImageBox
            // 
            this.outfiterImageBox.BackColor = System.Drawing.Color.Transparent;
            this.outfiterImageBox.Location = new System.Drawing.Point(125, 142);
            this.outfiterImageBox.Name = "outfiterImageBox";
            this.outfiterImageBox.Size = new System.Drawing.Size(128, 128);
            this.outfiterImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.outfiterImageBox.TabIndex = 4;
            this.outfiterImageBox.TabStop = false;
            // 
            // rotateRightButton
            // 
            this.rotateRightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.rotateRightButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotateRightButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.rotateRightButton.Image = global::Tibialyzer.Properties.Resources.rightarrow;
            this.rotateRightButton.Location = new System.Drawing.Point(362, 142);
            this.rotateRightButton.Name = "rotateRightButton";
            this.rotateRightButton.Padding = new System.Windows.Forms.Padding(3);
            this.rotateRightButton.Size = new System.Drawing.Size(24, 24);
            this.rotateRightButton.TabIndex = 91;
            this.rotateRightButton.Click += new System.EventHandler(this.rotateRightButton_Click);
            // 
            // rotateLeftButton
            // 
            this.rotateLeftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.rotateLeftButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotateLeftButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.rotateLeftButton.Image = global::Tibialyzer.Properties.Resources.leftarrow;
            this.rotateLeftButton.Location = new System.Drawing.Point(258, 142);
            this.rotateLeftButton.Name = "rotateLeftButton";
            this.rotateLeftButton.Padding = new System.Windows.Forms.Padding(3);
            this.rotateLeftButton.Size = new System.Drawing.Size(24, 24);
            this.rotateLeftButton.TabIndex = 90;
            this.rotateLeftButton.Click += new System.EventHandler(this.rotateLeftButton_Click);
            // 
            // secondaryColorButton
            // 
            this.secondaryColorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.secondaryColorButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondaryColorButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.secondaryColorButton.Location = new System.Drawing.Point(12, 192);
            this.secondaryColorButton.Name = "secondaryColorButton";
            this.secondaryColorButton.Padding = new System.Windows.Forms.Padding(3);
            this.secondaryColorButton.Size = new System.Drawing.Size(107, 25);
            this.secondaryColorButton.TabIndex = 89;
            this.secondaryColorButton.Text = "Secondary";
            this.secondaryColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.secondaryColorButton.Click += new System.EventHandler(this.secondaryColorButton_Click);
            // 
            // detailColorButton
            // 
            this.detailColorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.detailColorButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailColorButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.detailColorButton.Location = new System.Drawing.Point(12, 217);
            this.detailColorButton.Name = "detailColorButton";
            this.detailColorButton.Padding = new System.Windows.Forms.Padding(3);
            this.detailColorButton.Size = new System.Drawing.Size(107, 25);
            this.detailColorButton.TabIndex = 88;
            this.detailColorButton.Text = "Detail";
            this.detailColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.detailColorButton.Click += new System.EventHandler(this.detailColorButton_Click);
            // 
            // primaryColorButton
            // 
            this.primaryColorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.primaryColorButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.primaryColorButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.primaryColorButton.Location = new System.Drawing.Point(12, 167);
            this.primaryColorButton.Name = "primaryColorButton";
            this.primaryColorButton.Padding = new System.Windows.Forms.Padding(3);
            this.primaryColorButton.Size = new System.Drawing.Size(107, 25);
            this.primaryColorButton.TabIndex = 87;
            this.primaryColorButton.Text = "Primary";
            this.primaryColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.primaryColorButton.Click += new System.EventHandler(this.primaryColorButton_Click);
            // 
            // headColorButton
            // 
            this.headColorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.headColorButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headColorButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.headColorButton.Location = new System.Drawing.Point(12, 142);
            this.headColorButton.Name = "headColorButton";
            this.headColorButton.Padding = new System.Windows.Forms.Padding(3);
            this.headColorButton.Size = new System.Drawing.Size(107, 25);
            this.headColorButton.TabIndex = 86;
            this.headColorButton.Text = "Head";
            this.headColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.headColorButton.Click += new System.EventHandler(this.headColorButton_Click);
            // 
            // rotateLabel
            // 
            this.rotateLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.rotateLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.rotateLabel.Location = new System.Drawing.Point(279, 142);
            this.rotateLabel.Name = "rotateLabel";
            this.rotateLabel.Padding = new System.Windows.Forms.Padding(3);
            this.rotateLabel.Size = new System.Drawing.Size(83, 24);
            this.rotateLabel.TabIndex = 92;
            this.rotateLabel.Text = "Rotate";
            this.rotateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // outfitLabel
            // 
            this.outfitLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.outfitLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outfitLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.outfitLabel.Location = new System.Drawing.Point(279, 167);
            this.outfitLabel.Name = "outfitLabel";
            this.outfitLabel.Padding = new System.Windows.Forms.Padding(3);
            this.outfitLabel.Size = new System.Drawing.Size(83, 24);
            this.outfitLabel.TabIndex = 95;
            this.outfitLabel.Text = "Outfit";
            this.outfitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // outfitRightButton
            // 
            this.outfitRightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.outfitRightButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outfitRightButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.outfitRightButton.Image = global::Tibialyzer.Properties.Resources.rightarrow;
            this.outfitRightButton.Location = new System.Drawing.Point(362, 167);
            this.outfitRightButton.Name = "outfitRightButton";
            this.outfitRightButton.Padding = new System.Windows.Forms.Padding(3);
            this.outfitRightButton.Size = new System.Drawing.Size(24, 24);
            this.outfitRightButton.TabIndex = 94;
            this.outfitRightButton.Click += new System.EventHandler(this.outfitRightButton_Click);
            // 
            // outfitLeftButton
            // 
            this.outfitLeftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.outfitLeftButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outfitLeftButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.outfitLeftButton.Image = global::Tibialyzer.Properties.Resources.leftarrow;
            this.outfitLeftButton.Location = new System.Drawing.Point(258, 167);
            this.outfitLeftButton.Name = "outfitLeftButton";
            this.outfitLeftButton.Padding = new System.Windows.Forms.Padding(3);
            this.outfitLeftButton.Size = new System.Drawing.Size(24, 24);
            this.outfitLeftButton.TabIndex = 93;
            this.outfitLeftButton.Click += new System.EventHandler(this.outfitLeftButton_Click);
            // 
            // mountLabel
            // 
            this.mountLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mountLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.mountLabel.Location = new System.Drawing.Point(279, 192);
            this.mountLabel.Name = "mountLabel";
            this.mountLabel.Padding = new System.Windows.Forms.Padding(3);
            this.mountLabel.Size = new System.Drawing.Size(83, 24);
            this.mountLabel.TabIndex = 98;
            this.mountLabel.Text = "Mount";
            this.mountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mountRightButton
            // 
            this.mountRightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mountRightButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mountRightButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.mountRightButton.Image = global::Tibialyzer.Properties.Resources.rightarrow;
            this.mountRightButton.Location = new System.Drawing.Point(362, 192);
            this.mountRightButton.Name = "mountRightButton";
            this.mountRightButton.Padding = new System.Windows.Forms.Padding(3);
            this.mountRightButton.Size = new System.Drawing.Size(24, 24);
            this.mountRightButton.TabIndex = 97;
            this.mountRightButton.Click += new System.EventHandler(this.mountRightButton_Click);
            // 
            // mountLeftButton
            // 
            this.mountLeftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.mountLeftButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mountLeftButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.mountLeftButton.Image = global::Tibialyzer.Properties.Resources.leftarrow;
            this.mountLeftButton.Location = new System.Drawing.Point(258, 192);
            this.mountLeftButton.Name = "mountLeftButton";
            this.mountLeftButton.Padding = new System.Windows.Forms.Padding(3);
            this.mountLeftButton.Size = new System.Drawing.Size(24, 24);
            this.mountLeftButton.TabIndex = 96;
            this.mountLeftButton.Click += new System.EventHandler(this.mountLeftButton_Click);
            // 
            // copyTextButton
            // 
            this.copyTextButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.copyTextButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyTextButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.copyTextButton.Location = new System.Drawing.Point(258, 245);
            this.copyTextButton.Name = "copyTextButton";
            this.copyTextButton.Padding = new System.Windows.Forms.Padding(3);
            this.copyTextButton.Size = new System.Drawing.Size(128, 25);
            this.copyTextButton.TabIndex = 99;
            this.copyTextButton.Text = "Copy Text";
            this.copyTextButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.copyTextButton.Click += new System.EventHandler(this.copyTextButton_Click);
            // 
            // addon1Toggle
            // 
            this.addon1Toggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.addon1Toggle.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addon1Toggle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.addon1Toggle.Location = new System.Drawing.Point(258, 217);
            this.addon1Toggle.Name = "addon1Toggle";
            this.addon1Toggle.Padding = new System.Windows.Forms.Padding(3);
            this.addon1Toggle.Size = new System.Drawing.Size(40, 25);
            this.addon1Toggle.TabIndex = 100;
            this.addon1Toggle.Text = "A1";
            this.addon1Toggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addon1Toggle.Click += new System.EventHandler(this.addon1Toggle_Click);
            // 
            // addon2Toggle
            // 
            this.addon2Toggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.addon2Toggle.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addon2Toggle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.addon2Toggle.Location = new System.Drawing.Point(304, 217);
            this.addon2Toggle.Name = "addon2Toggle";
            this.addon2Toggle.Padding = new System.Windows.Forms.Padding(3);
            this.addon2Toggle.Size = new System.Drawing.Size(40, 25);
            this.addon2Toggle.TabIndex = 101;
            this.addon2Toggle.Text = "A2";
            this.addon2Toggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addon2Toggle.Click += new System.EventHandler(this.addon2Toggle_Click);
            // 
            // genderToggle
            // 
            this.genderToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.genderToggle.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genderToggle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(133)))), ((int)(((byte)(142)))));
            this.genderToggle.Location = new System.Drawing.Point(346, 217);
            this.genderToggle.Name = "genderToggle";
            this.genderToggle.Padding = new System.Windows.Forms.Padding(3);
            this.genderToggle.Size = new System.Drawing.Size(40, 25);
            this.genderToggle.TabIndex = 102;
            this.genderToggle.Text = "F";
            this.genderToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genderToggle.Click += new System.EventHandler(this.genderToggle_Click);
            // 
            // OutfiterForm
            // 
            this.ClientSize = new System.Drawing.Size(392, 279);
            this.Controls.Add(this.genderToggle);
            this.Controls.Add(this.addon2Toggle);
            this.Controls.Add(this.addon1Toggle);
            this.Controls.Add(this.copyTextButton);
            this.Controls.Add(this.mountLabel);
            this.Controls.Add(this.mountRightButton);
            this.Controls.Add(this.mountLeftButton);
            this.Controls.Add(this.outfitLabel);
            this.Controls.Add(this.outfitRightButton);
            this.Controls.Add(this.outfitLeftButton);
            this.Controls.Add(this.rotateLabel);
            this.Controls.Add(this.rotateRightButton);
            this.Controls.Add(this.rotateLeftButton);
            this.Controls.Add(this.secondaryColorButton);
            this.Controls.Add(this.detailColorButton);
            this.Controls.Add(this.primaryColorButton);
            this.Controls.Add(this.headColorButton);
            this.Controls.Add(this.outfiterImageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OutfiterForm";
            ((System.ComponentModel.ISupportInitialize)(this.outfiterImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        PictureBox colorPicker;
        int colorIndex;
        public override void LoadForm() {
            if (outfit == null) return;
            this.SuspendLayout();
            NotificationInitialize();
            
            colorPicker = new PictureBox();
            colorPicker.Location = new Point(50, 25);
            colorPicker.Size = new Size(OutfiterManager.OutfitColorBoxSize * OutfiterManager.OutfitColorsPerRow, OutfiterManager.OutfitColorBoxSize * 7);
            colorPicker.Image = null;
            colorPicker.MouseDown += ChangeOutfitColor;
            this.Controls.Add(colorPicker);
            RefreshColorPicker(0);
            this.RefreshImage();
            ToggleActivation(addon1Toggle, outfit.addon1);
            ToggleActivation(addon2Toggle, outfit.addon2);
            ToggleActivation(genderToggle, outfit.gender == Gender.Female);

            base.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private void RefreshColorPicker(int colorIndex) {
            this.colorIndex = colorIndex;
            headColorButton.Enabled = true;
            primaryColorButton.Enabled = true;
            secondaryColorButton.Enabled = true;
            detailColorButton.Enabled = true;
            switch (colorIndex) {
                case 0: headColorButton.Enabled = false; break;
                case 1: primaryColorButton.Enabled = false; break;
                case 2: secondaryColorButton.Enabled = false; break;
                case 3: detailColorButton.Enabled = false; break;
            }
            Image oldImage = colorPicker.Image;
            colorPicker.Image = OutfiterManager.GenerateColorImage(outfit.colors[colorIndex]);
            if (oldImage != null) {
                oldImage.Dispose();
            }
        }
        private void ChangeOutfitColor(object sender, MouseEventArgs e) {
            int index = OutfiterManager.ColorIndex(e.X, e.Y);
            if (index < 0) return;
            outfit.colors[colorIndex] = index;
            RefreshColorPicker(colorIndex);
            RefreshImage();
        }

        public void RefreshImage() {
            Image oldImage = outfiterImageBox.Image;
            outfiterImageBox.Image = outfit.GetImage();
            if (oldImage != null) {
                oldImage.Dispose();
            }
        }

        public override string FormName() {
            return "OutfiterForm";
        }

        private void headColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(0);
        }

        private void primaryColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(1);
        }

        private void secondaryColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(2);
        }

        private void detailColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(3);
        }

        private void rotateLeftButton_Click(object sender, EventArgs e) {
            outfit.Rotate(-1);
            RefreshImage();
        }

        private void rotateRightButton_Click(object sender, EventArgs e) {
            outfit.Rotate(1);
            RefreshImage();
        }

        private void outfitLeftButton_Click(object sender, EventArgs e) {
            outfit.SwitchOutfit(-1);
            RefreshImage();
        }

        private void outfitRightButton_Click(object sender, EventArgs e) {
            outfit.SwitchOutfit(1);
            RefreshImage();
        }

        private void mountLeftButton_Click(object sender, EventArgs e) {
            outfit.SwitchMount(-1);
            RefreshImage();
        }

        private void mountRightButton_Click(object sender, EventArgs e) {
            outfit.SwitchMount(1);
            RefreshImage();
        }

        private void copyTextButton_Click(object sender, EventArgs e) {
            MainForm.mainForm.Invoke((MethodInvoker)delegate {
                Clipboard.SetText(outfit.ToString());
            });
        }

        public void ToggleActivation(Control control, bool activated) {
            if (!activated) {
                control.ForeColor = StyleManager.HealthDanger;
            } else {
                control.ForeColor = StyleManager.HealthHealthy;
            }
        }

        private void addon1Toggle_Click(object sender, EventArgs e) {
            outfit.addon1 = !outfit.addon1;
            ToggleActivation(sender as Control, outfit.addon1);
            RefreshImage();
        }

        private void addon2Toggle_Click(object sender, EventArgs e) {
            outfit.addon2 = !outfit.addon2;
            ToggleActivation(sender as Control, outfit.addon2);
            RefreshImage();
        }

        private void genderToggle_Click(object sender, EventArgs e) {
            outfit.gender = outfit.gender == Gender.Male ? Gender.Female : Gender.Male;
            ToggleActivation(sender as Control, outfit.gender == Gender.Female);
            RefreshImage();
        }
    }
}
