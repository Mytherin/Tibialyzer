
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer {
    public enum DisplayType { Details = 0, Images = 1 }
    class CreatureList : NotificationForm {
        public List<TibiaObject> objects;
        public string title = "List";
        private Label toggleButton;
        public DisplayType displayType = DisplayType.Details;
        public bool addConditionalAttributes = false;
        private string sortedHeader = null;
        private bool desc;

        public CreatureList(int initialPage, DisplayType displayType, string sortedHeader, bool desc) {
            this.currentPage = initialPage;
            this.displayType = displayType;
            this.sortedHeader = sortedHeader;
            this.desc = desc;
            objects = null;
            InitializeComponent();
        }
        void updateCommand() {
            string[] split = command.command.Split(MainForm.commandSymbol);
            command.command = split[0] + MainForm.commandSymbol + split[1] + MainForm.commandSymbol + currentPage.ToString() + MainForm.commandSymbol + ((int)displayType).ToString() + MainForm.commandSymbol + (desc ? 1 : 0) + (sortedHeader != null ? MainForm.commandSymbol + sortedHeader : "");
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatureList));
            this.listTitle = new System.Windows.Forms.Label();
            this.toggleButton = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listTitle
            // 
            this.listTitle.AutoSize = true;
            this.listTitle.BackColor = System.Drawing.Color.Transparent;
            this.listTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.listTitle.Location = new System.Drawing.Point(152, 9);
            this.listTitle.Name = "listTitle";
            this.listTitle.Size = new System.Drawing.Size(32, 16);
            this.listTitle.TabIndex = 14;
            this.listTitle.Text = "List";
            // 
            // toggleButton
            // 
            this.toggleButton.BackColor = System.Drawing.Color.Transparent;
            this.toggleButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toggleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.toggleButton.Location = new System.Drawing.Point(244, 7);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Padding = new System.Windows.Forms.Padding(2);
            this.toggleButton.Size = new System.Drawing.Size(96, 21);
            this.toggleButton.TabIndex = 15;
            this.toggleButton.Text = "Icons";
            this.toggleButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toggleButton.Click += new System.EventHandler(this.toggleButton_Click);
            // 
            // CreatureList
            // 
            this.ClientSize = new System.Drawing.Size(352, 76);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.listTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreatureList";
            this.Text = "Tibia Object List";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        public void sortHeader(object sender, EventArgs e) {
            if (sortedHeader == (sender as Control).Name) {
                desc = !desc;
            } else {
                sortedHeader = (sender as Control).Name;
                desc = false;
            }
            this.SuspendForm();
            refresh();
            this.ResumeForm();
        }
        private int currentPage = 0;
        private List<Control> createdControls = new List<Control>();
        private int startDisplay = 0;
        private int currentDisplay = -1;
        private void refresh() {
            foreach(Control c in createdControls) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            int base_y = this.listTitle.Location.Y + this.listTitle.Height + 10;
            int newWidth = 352;
            MainForm.PageInfo pageInfo = new MainForm.PageInfo(false, false);

            int y;
            if (displayType == DisplayType.Details) {
                y = MainForm.DisplayCreatureAttributeList(this.Controls, objects, 10, base_y, out newWidth, null, createdControls, currentPage, 20, pageInfo, null, null, sortHeader, sortedHeader, desc, null, null, addConditionalAttributes);
            } else {
                y = MainForm.DisplayCreatureList(this.Controls, objects, 10, base_y, 344, 4, null, 1, createdControls, currentPage, 600, pageInfo, currentDisplay);
                if (currentDisplay >= 0) {
                    currentDisplay = -1;
                    currentPage = pageInfo.currentPage;
                }
            }
            startDisplay = pageInfo.startDisplay;
            updateCommand();

            newWidth = Math.Max(newWidth, 275);
            if (pageInfo.prevPage) {
                PictureBox prevpage = new PictureBox();
                prevpage.Location = new Point(10, base_y + y);
                prevpage.Size = new Size(97, 23);
                prevpage.Image = MainForm.prevpage_image;
                prevpage.BackColor = Color.Transparent;
                prevpage.SizeMode = PictureBoxSizeMode.StretchImage;
                prevpage.Click += Prevpage_Click;
                this.Controls.Add(prevpage);
                createdControls.Add(prevpage);

            }
            if (pageInfo.nextPage) {
                PictureBox nextpage = new PictureBox();
                nextpage.Location = new Point(10 + newWidth - 108, base_y + y);
                nextpage.Size = new Size(98, 23);
                nextpage.BackColor = Color.Transparent;
                nextpage.Image = MainForm.nextpage_image;
                nextpage.SizeMode = PictureBoxSizeMode.StretchImage;
                nextpage.Click += Nextpage_Click;
                this.Controls.Add(nextpage);
                createdControls.Add(nextpage);
            }
            toggleButton.Location = new Point(newWidth - toggleButton.Size.Width, toggleButton.Location.Y);
            if (pageInfo.prevPage || pageInfo.nextPage) {
                y += 23;
            }
            this.Size = new Size(10 + newWidth, base_y + y + 10);
            this.refreshTimer();
        }

        private void Nextpage_Click(object sender, EventArgs e) {
            currentPage++;
            this.SuspendForm();
            refresh();
            this.ResumeForm();
        }

        private void Prevpage_Click(object sender, EventArgs e) {
            currentPage--;
            this.SuspendForm();
            refresh();
            this.ResumeForm();
        }

        public override void LoadForm() {
            this.SuspendForm();

            this.NotificationInitialize();
            toggleButton.Click -= c_Click;

            refresh();

            this.listTitle.Text = title;
            this.listTitle.Location = new Point(this.Size.Width / 2 - this.listTitle.Width / 2, this.listTitle.Location.Y);
            
            this.NotificationFinalize();
            this.ResumeForm();
        }

        private Label listTitle;

        private void toggleButton_Click(object sender, EventArgs e) {
            if (displayType == DisplayType.Details) {
                displayType = DisplayType.Images;
                (sender as Label).Text = "Details";
                currentDisplay = startDisplay;
            } else {
                displayType = DisplayType.Details;
                (sender as Label).Text = "Icons";
                currentPage = (int)Math.Floor(startDisplay / 20.0);
            }
            this.SuspendForm();
            refresh();
            this.ResumeForm();
            currentDisplay = -1;
        }
    }
}
