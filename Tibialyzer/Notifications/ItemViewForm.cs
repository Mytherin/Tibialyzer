
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
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;


namespace Tibialyzer {
    class ItemViewForm : NotificationForm {
        private Label itemName;
        private Label itemCategory;
        private Label lookText;
        private System.Windows.Forms.PictureBox itemPictureBox;

        public Item item;
        private System.Windows.Forms.CheckBox pickupBox;
        private System.Windows.Forms.CheckBox convertBox;
        private PictureBox increaseValue1;
        private PictureBox decreaseValue1;
        private Label valueDigit1;
        private Label valueDigit10;
        private PictureBox decreaseValue10;
        private PictureBox increaseValue10;
        private Label valueDigit100;
        private PictureBox decreaseValue100;
        private PictureBox increaseValue100;
        private Label valueDigit1000;
        private PictureBox decreaseValue1000;
        private PictureBox increaseValue1000;
        private Label valueDigit10000;
        private PictureBox decreaseValue10000;
        private PictureBox increaseValue10000;
        private Label valueDigit100000;
        private PictureBox decreaseValue100000;
        private PictureBox increaseValue100000;
        private Label valueDigit1000000;
        private PictureBox decreaseValue1000000;
        private PictureBox increaseValue1000000;
        private Label valueDigit10000000;
        private PictureBox decreaseValue10000000;
        private PictureBox increaseValue10000000;
        private Label valueDigit100000000;
        private PictureBox decreaseValue100000000;
        private PictureBox increaseValue100000000;
        private Label valueDigit1000000000;
        private PictureBox decreaseValue1000000000;
        private PictureBox increaseValue1000000000;
        private Label valueDigit10000000000;
        private PictureBox decreaseValue10000000000;
        private PictureBox increaseValue10000000000;

        private List<PictureBox> increaseBoxes = new List<PictureBox>();
        private List<PictureBox> decreaseBoxes = new List<PictureBox>();
        private List<Label> valueLabels = new List<Label>();
        private Label goldPerCapLabel;
        private Label capacityLabel;
        private string[] headers = { "Sell To", "Buy From", "Dropped", "Rewarded" };
        public ItemViewForm(int currentPage, int currentDisplay) {
            skip_event = true;
            InitializeComponent();
            skip_event = false;
            this.currentPage = Math.Max(currentPage, 0);
            this.currentControlList = Math.Min(currentDisplay, headers.Length);

            valueLabels.Add(valueDigit1);
            valueLabels.Add(valueDigit10);
            valueLabels.Add(valueDigit100);
            valueLabels.Add(valueDigit1000);
            valueLabels.Add(valueDigit10000);
            valueLabels.Add(valueDigit100000);
            valueLabels.Add(valueDigit1000000);
            valueLabels.Add(valueDigit10000000);
            valueLabels.Add(valueDigit100000000);
            valueLabels.Add(valueDigit1000000000);
            valueLabels.Add(valueDigit10000000000);
            decreaseBoxes.Add(decreaseValue1);
            decreaseBoxes.Add(decreaseValue10);
            decreaseBoxes.Add(decreaseValue100);
            decreaseBoxes.Add(decreaseValue1000);
            decreaseBoxes.Add(decreaseValue10000);
            decreaseBoxes.Add(decreaseValue100000);
            decreaseBoxes.Add(decreaseValue1000000);
            decreaseBoxes.Add(decreaseValue10000000);
            decreaseBoxes.Add(decreaseValue100000000);
            decreaseBoxes.Add(decreaseValue1000000000);
            decreaseBoxes.Add(decreaseValue10000000000);
            increaseBoxes.Add(increaseValue1);
            increaseBoxes.Add(increaseValue10);
            increaseBoxes.Add(increaseValue100);
            increaseBoxes.Add(increaseValue1000);
            increaseBoxes.Add(increaseValue10000);
            increaseBoxes.Add(increaseValue100000);
            increaseBoxes.Add(increaseValue1000000);
            increaseBoxes.Add(increaseValue10000000);
            increaseBoxes.Add(increaseValue100000000);
            increaseBoxes.Add(increaseValue1000000000);
            increaseBoxes.Add(increaseValue10000000000);

            for (int i = 0; i < decreaseBoxes.Count; i++) {
                PictureBox box = decreaseBoxes[i];
                box.Image = StyleManager.GetImage("mapdown.png");
                box.Name = (-Math.Pow(10, i)).ToString();
                box.Click += changeClick;
            }

            for (int i = 0; i < increaseBoxes.Count; i++) {
                PictureBox box = increaseBoxes[i];
                box.Image = StyleManager.GetImage("mapup.png");
                box.Name = (Math.Pow(10, i)).ToString();
                box.Click += changeClick;
            }
        }

        private void updateValue() {
            long value = Math.Max(item.actual_value, 0);
            for (int i = 0; i < valueLabels.Count; i++) {
                long next = value / 10;
                long remainder = value - (next * 10);

                if (next == 0 && remainder == 0 && i > 0) {
                    valueLabels[i].Visible = false;
                } else {
                    valueLabels[i].Text = remainder.ToString();
                    valueLabels[i].Visible = true;
                }

                value = next;
            }
        }

        private void changeClick(object sender, EventArgs e) {
            long change = long.Parse((sender as Control).Name);
            long modifiedValue = Math.Max(Math.Max(item.actual_value, 0) + change, 0);
            CommandManager.ExecuteCommand("setval" + Constants.CommandSymbol + item.GetName() + "=" + modifiedValue);
            updateValue();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemViewForm));
            this.valueDigit10000000000 = new System.Windows.Forms.Label();
            this.decreaseValue10000000000 = new System.Windows.Forms.PictureBox();
            this.increaseValue10000000000 = new System.Windows.Forms.PictureBox();
            this.valueDigit1000000000 = new System.Windows.Forms.Label();
            this.decreaseValue1000000000 = new System.Windows.Forms.PictureBox();
            this.increaseValue1000000000 = new System.Windows.Forms.PictureBox();
            this.valueDigit100000000 = new System.Windows.Forms.Label();
            this.decreaseValue100000000 = new System.Windows.Forms.PictureBox();
            this.increaseValue100000000 = new System.Windows.Forms.PictureBox();
            this.valueDigit10000000 = new System.Windows.Forms.Label();
            this.decreaseValue10000000 = new System.Windows.Forms.PictureBox();
            this.increaseValue10000000 = new System.Windows.Forms.PictureBox();
            this.valueDigit1000000 = new System.Windows.Forms.Label();
            this.decreaseValue1000000 = new System.Windows.Forms.PictureBox();
            this.increaseValue1000000 = new System.Windows.Forms.PictureBox();
            this.valueDigit100000 = new System.Windows.Forms.Label();
            this.decreaseValue100000 = new System.Windows.Forms.PictureBox();
            this.increaseValue100000 = new System.Windows.Forms.PictureBox();
            this.valueDigit10000 = new System.Windows.Forms.Label();
            this.decreaseValue10000 = new System.Windows.Forms.PictureBox();
            this.increaseValue10000 = new System.Windows.Forms.PictureBox();
            this.valueDigit1000 = new System.Windows.Forms.Label();
            this.decreaseValue1000 = new System.Windows.Forms.PictureBox();
            this.increaseValue1000 = new System.Windows.Forms.PictureBox();
            this.valueDigit100 = new System.Windows.Forms.Label();
            this.decreaseValue100 = new System.Windows.Forms.PictureBox();
            this.increaseValue100 = new System.Windows.Forms.PictureBox();
            this.valueDigit10 = new System.Windows.Forms.Label();
            this.decreaseValue10 = new System.Windows.Forms.PictureBox();
            this.increaseValue10 = new System.Windows.Forms.PictureBox();
            this.valueDigit1 = new System.Windows.Forms.Label();
            this.decreaseValue1 = new System.Windows.Forms.PictureBox();
            this.increaseValue1 = new System.Windows.Forms.PictureBox();
            this.convertBox = new System.Windows.Forms.CheckBox();
            this.pickupBox = new System.Windows.Forms.CheckBox();
            this.lookText = new System.Windows.Forms.Label();
            this.itemCategory = new System.Windows.Forms.Label();
            this.itemName = new System.Windows.Forms.Label();
            this.itemPictureBox = new System.Windows.Forms.PictureBox();
            this.goldPerCapLabel = new System.Windows.Forms.Label();
            this.capacityLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10000000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10000000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1000000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1000000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue100000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue100000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1000000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue100000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue100000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1000)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue100)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue100)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // valueDigit10000000000
            // 
            this.valueDigit10000000000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit10000000000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit10000000000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit10000000000.Location = new System.Drawing.Point(154, 42);
            this.valueDigit10000000000.Name = "valueDigit10000000000";
            this.valueDigit10000000000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit10000000000.TabIndex = 60;
            this.valueDigit10000000000.Text = "0";
            this.valueDigit10000000000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue10000000000
            // 
            this.decreaseValue10000000000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue10000000000.Location = new System.Drawing.Point(153, 58);
            this.decreaseValue10000000000.Name = "decreaseValue10000000000";
            this.decreaseValue10000000000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue10000000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue10000000000.TabIndex = 59;
            this.decreaseValue10000000000.TabStop = false;
            // 
            // increaseValue10000000000
            // 
            this.increaseValue10000000000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue10000000000.Location = new System.Drawing.Point(153, 26);
            this.increaseValue10000000000.Name = "increaseValue10000000000";
            this.increaseValue10000000000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue10000000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue10000000000.TabIndex = 58;
            this.increaseValue10000000000.TabStop = false;
            // 
            // valueDigit1000000000
            // 
            this.valueDigit1000000000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit1000000000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit1000000000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit1000000000.Location = new System.Drawing.Point(170, 42);
            this.valueDigit1000000000.Name = "valueDigit1000000000";
            this.valueDigit1000000000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit1000000000.TabIndex = 57;
            this.valueDigit1000000000.Text = "0";
            this.valueDigit1000000000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue1000000000
            // 
            this.decreaseValue1000000000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue1000000000.Location = new System.Drawing.Point(169, 58);
            this.decreaseValue1000000000.Name = "decreaseValue1000000000";
            this.decreaseValue1000000000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue1000000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue1000000000.TabIndex = 56;
            this.decreaseValue1000000000.TabStop = false;
            // 
            // increaseValue1000000000
            // 
            this.increaseValue1000000000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue1000000000.Location = new System.Drawing.Point(169, 26);
            this.increaseValue1000000000.Name = "increaseValue1000000000";
            this.increaseValue1000000000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue1000000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue1000000000.TabIndex = 55;
            this.increaseValue1000000000.TabStop = false;
            // 
            // valueDigit100000000
            // 
            this.valueDigit100000000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit100000000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit100000000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit100000000.Location = new System.Drawing.Point(186, 42);
            this.valueDigit100000000.Name = "valueDigit100000000";
            this.valueDigit100000000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit100000000.TabIndex = 54;
            this.valueDigit100000000.Text = "0";
            this.valueDigit100000000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue100000000
            // 
            this.decreaseValue100000000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue100000000.Location = new System.Drawing.Point(185, 58);
            this.decreaseValue100000000.Name = "decreaseValue100000000";
            this.decreaseValue100000000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue100000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue100000000.TabIndex = 53;
            this.decreaseValue100000000.TabStop = false;
            // 
            // increaseValue100000000
            // 
            this.increaseValue100000000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue100000000.Location = new System.Drawing.Point(185, 26);
            this.increaseValue100000000.Name = "increaseValue100000000";
            this.increaseValue100000000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue100000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue100000000.TabIndex = 52;
            this.increaseValue100000000.TabStop = false;
            // 
            // valueDigit10000000
            // 
            this.valueDigit10000000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit10000000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit10000000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit10000000.Location = new System.Drawing.Point(202, 42);
            this.valueDigit10000000.Name = "valueDigit10000000";
            this.valueDigit10000000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit10000000.TabIndex = 51;
            this.valueDigit10000000.Text = "0";
            this.valueDigit10000000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue10000000
            // 
            this.decreaseValue10000000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue10000000.Location = new System.Drawing.Point(201, 58);
            this.decreaseValue10000000.Name = "decreaseValue10000000";
            this.decreaseValue10000000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue10000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue10000000.TabIndex = 50;
            this.decreaseValue10000000.TabStop = false;
            // 
            // increaseValue10000000
            // 
            this.increaseValue10000000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue10000000.Location = new System.Drawing.Point(201, 26);
            this.increaseValue10000000.Name = "increaseValue10000000";
            this.increaseValue10000000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue10000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue10000000.TabIndex = 49;
            this.increaseValue10000000.TabStop = false;
            // 
            // valueDigit1000000
            // 
            this.valueDigit1000000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit1000000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit1000000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit1000000.Location = new System.Drawing.Point(218, 42);
            this.valueDigit1000000.Name = "valueDigit1000000";
            this.valueDigit1000000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit1000000.TabIndex = 48;
            this.valueDigit1000000.Text = "0";
            this.valueDigit1000000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue1000000
            // 
            this.decreaseValue1000000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue1000000.Location = new System.Drawing.Point(217, 58);
            this.decreaseValue1000000.Name = "decreaseValue1000000";
            this.decreaseValue1000000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue1000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue1000000.TabIndex = 47;
            this.decreaseValue1000000.TabStop = false;
            // 
            // increaseValue1000000
            // 
            this.increaseValue1000000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue1000000.Location = new System.Drawing.Point(217, 26);
            this.increaseValue1000000.Name = "increaseValue1000000";
            this.increaseValue1000000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue1000000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue1000000.TabIndex = 46;
            this.increaseValue1000000.TabStop = false;
            // 
            // valueDigit100000
            // 
            this.valueDigit100000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit100000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit100000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit100000.Location = new System.Drawing.Point(234, 42);
            this.valueDigit100000.Name = "valueDigit100000";
            this.valueDigit100000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit100000.TabIndex = 45;
            this.valueDigit100000.Text = "0";
            this.valueDigit100000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue100000
            // 
            this.decreaseValue100000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue100000.Location = new System.Drawing.Point(233, 58);
            this.decreaseValue100000.Name = "decreaseValue100000";
            this.decreaseValue100000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue100000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue100000.TabIndex = 44;
            this.decreaseValue100000.TabStop = false;
            // 
            // increaseValue100000
            // 
            this.increaseValue100000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue100000.Location = new System.Drawing.Point(233, 26);
            this.increaseValue100000.Name = "increaseValue100000";
            this.increaseValue100000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue100000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue100000.TabIndex = 43;
            this.increaseValue100000.TabStop = false;
            // 
            // valueDigit10000
            // 
            this.valueDigit10000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit10000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit10000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit10000.Location = new System.Drawing.Point(250, 42);
            this.valueDigit10000.Name = "valueDigit10000";
            this.valueDigit10000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit10000.TabIndex = 42;
            this.valueDigit10000.Text = "0";
            this.valueDigit10000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue10000
            // 
            this.decreaseValue10000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue10000.Location = new System.Drawing.Point(249, 58);
            this.decreaseValue10000.Name = "decreaseValue10000";
            this.decreaseValue10000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue10000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue10000.TabIndex = 41;
            this.decreaseValue10000.TabStop = false;
            // 
            // increaseValue10000
            // 
            this.increaseValue10000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue10000.Location = new System.Drawing.Point(249, 26);
            this.increaseValue10000.Name = "increaseValue10000";
            this.increaseValue10000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue10000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue10000.TabIndex = 40;
            this.increaseValue10000.TabStop = false;
            // 
            // valueDigit1000
            // 
            this.valueDigit1000.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit1000.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit1000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit1000.Location = new System.Drawing.Point(266, 42);
            this.valueDigit1000.Name = "valueDigit1000";
            this.valueDigit1000.Size = new System.Drawing.Size(16, 16);
            this.valueDigit1000.TabIndex = 39;
            this.valueDigit1000.Text = "0";
            this.valueDigit1000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue1000
            // 
            this.decreaseValue1000.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue1000.Location = new System.Drawing.Point(265, 58);
            this.decreaseValue1000.Name = "decreaseValue1000";
            this.decreaseValue1000.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue1000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue1000.TabIndex = 38;
            this.decreaseValue1000.TabStop = false;
            // 
            // increaseValue1000
            // 
            this.increaseValue1000.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue1000.Location = new System.Drawing.Point(265, 26);
            this.increaseValue1000.Name = "increaseValue1000";
            this.increaseValue1000.Size = new System.Drawing.Size(16, 16);
            this.increaseValue1000.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue1000.TabIndex = 37;
            this.increaseValue1000.TabStop = false;
            // 
            // valueDigit100
            // 
            this.valueDigit100.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit100.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit100.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit100.Location = new System.Drawing.Point(282, 42);
            this.valueDigit100.Name = "valueDigit100";
            this.valueDigit100.Size = new System.Drawing.Size(16, 16);
            this.valueDigit100.TabIndex = 36;
            this.valueDigit100.Text = "0";
            this.valueDigit100.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue100
            // 
            this.decreaseValue100.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue100.Location = new System.Drawing.Point(281, 58);
            this.decreaseValue100.Name = "decreaseValue100";
            this.decreaseValue100.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue100.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue100.TabIndex = 35;
            this.decreaseValue100.TabStop = false;
            // 
            // increaseValue100
            // 
            this.increaseValue100.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue100.Location = new System.Drawing.Point(281, 26);
            this.increaseValue100.Name = "increaseValue100";
            this.increaseValue100.Size = new System.Drawing.Size(16, 16);
            this.increaseValue100.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue100.TabIndex = 34;
            this.increaseValue100.TabStop = false;
            // 
            // valueDigit10
            // 
            this.valueDigit10.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit10.Location = new System.Drawing.Point(298, 42);
            this.valueDigit10.Name = "valueDigit10";
            this.valueDigit10.Size = new System.Drawing.Size(16, 16);
            this.valueDigit10.TabIndex = 33;
            this.valueDigit10.Text = "0";
            this.valueDigit10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue10
            // 
            this.decreaseValue10.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue10.Location = new System.Drawing.Point(297, 58);
            this.decreaseValue10.Name = "decreaseValue10";
            this.decreaseValue10.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue10.TabIndex = 32;
            this.decreaseValue10.TabStop = false;
            // 
            // increaseValue10
            // 
            this.increaseValue10.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue10.Location = new System.Drawing.Point(297, 26);
            this.increaseValue10.Name = "increaseValue10";
            this.increaseValue10.Size = new System.Drawing.Size(16, 16);
            this.increaseValue10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue10.TabIndex = 31;
            this.increaseValue10.TabStop = false;
            // 
            // valueDigit1
            // 
            this.valueDigit1.BackColor = System.Drawing.Color.Transparent;
            this.valueDigit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueDigit1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueDigit1.Location = new System.Drawing.Point(314, 42);
            this.valueDigit1.Name = "valueDigit1";
            this.valueDigit1.Size = new System.Drawing.Size(16, 16);
            this.valueDigit1.TabIndex = 30;
            this.valueDigit1.Text = "0";
            this.valueDigit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decreaseValue1
            // 
            this.decreaseValue1.BackColor = System.Drawing.Color.Transparent;
            this.decreaseValue1.Location = new System.Drawing.Point(313, 58);
            this.decreaseValue1.Name = "decreaseValue1";
            this.decreaseValue1.Size = new System.Drawing.Size(16, 16);
            this.decreaseValue1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.decreaseValue1.TabIndex = 29;
            this.decreaseValue1.TabStop = false;
            // 
            // increaseValue1
            // 
            this.increaseValue1.BackColor = System.Drawing.Color.Transparent;
            this.increaseValue1.Location = new System.Drawing.Point(313, 26);
            this.increaseValue1.Name = "increaseValue1";
            this.increaseValue1.Size = new System.Drawing.Size(16, 16);
            this.increaseValue1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.increaseValue1.TabIndex = 28;
            this.increaseValue1.TabStop = false;
            // 
            // convertBox
            // 
            this.convertBox.AutoSize = true;
            this.convertBox.BackColor = System.Drawing.Color.Transparent;
            this.convertBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.convertBox.Location = new System.Drawing.Point(239, 75);
            this.convertBox.Name = "convertBox";
            this.convertBox.Size = new System.Drawing.Size(104, 17);
            this.convertBox.TabIndex = 6;
            this.convertBox.Text = "Convert To Gold";
            this.convertBox.UseVisualStyleBackColor = false;
            this.convertBox.CheckedChanged += new System.EventHandler(this.convertBox_CheckedChanged);
            // 
            // pickupBox
            // 
            this.pickupBox.AutoSize = true;
            this.pickupBox.BackColor = System.Drawing.Color.Transparent;
            this.pickupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.pickupBox.Location = new System.Drawing.Point(133, 76);
            this.pickupBox.Name = "pickupBox";
            this.pickupBox.Size = new System.Drawing.Size(87, 17);
            this.pickupBox.TabIndex = 5;
            this.pickupBox.Text = "Pick Up Item";
            this.pickupBox.UseVisualStyleBackColor = false;
            this.pickupBox.CheckedChanged += new System.EventHandler(this.pickupBox_CheckedChanged);
            // 
            // lookText
            // 
            this.lookText.AutoSize = true;
            this.lookText.BackColor = System.Drawing.Color.Transparent;
            this.lookText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.lookText.Location = new System.Drawing.Point(141, 95);
            this.lookText.MaximumSize = new System.Drawing.Size(210, 0);
            this.lookText.Name = "lookText";
            this.lookText.Size = new System.Drawing.Size(64, 13);
            this.lookText.TabIndex = 3;
            this.lookText.Text = "You see a...";
            // 
            // itemCategory
            // 
            this.itemCategory.BackColor = System.Drawing.Color.Transparent;
            this.itemCategory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.itemCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.itemCategory.Location = new System.Drawing.Point(34, 27);
            this.itemCategory.MaximumSize = new System.Drawing.Size(96, 28);
            this.itemCategory.Name = "itemCategory";
            this.itemCategory.Size = new System.Drawing.Size(96, 28);
            this.itemCategory.TabIndex = 2;
            this.itemCategory.Text = "category";
            this.itemCategory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.itemCategory.Click += new System.EventHandler(this.itemCategory_Click);
            // 
            // itemName
            // 
            this.itemName.BackColor = System.Drawing.Color.Transparent;
            this.itemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.itemName.Location = new System.Drawing.Point(34, 89);
            this.itemName.MaximumSize = new System.Drawing.Size(96, 28);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(96, 28);
            this.itemName.TabIndex = 1;
            this.itemName.Text = "name";
            this.itemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // itemPictureBox
            // 
            this.itemPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.itemPictureBox.Location = new System.Drawing.Point(66, 56);
            this.itemPictureBox.Name = "itemPictureBox";
            this.itemPictureBox.Size = new System.Drawing.Size(32, 32);
            this.itemPictureBox.TabIndex = 0;
            this.itemPictureBox.TabStop = false;
            // 
            // goldPerCapLabel
            // 
            this.goldPerCapLabel.BackColor = System.Drawing.Color.Transparent;
            this.goldPerCapLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.goldPerCapLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goldPerCapLabel.ForeColor = System.Drawing.Color.Gold;
            this.goldPerCapLabel.Location = new System.Drawing.Point(34, 137);
            this.goldPerCapLabel.Name = "goldPerCapLabel";
            this.goldPerCapLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.goldPerCapLabel.Size = new System.Drawing.Size(96, 19);
            this.goldPerCapLabel.TabIndex = 62;
            this.goldPerCapLabel.Text = "21K/oz.";
            this.goldPerCapLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // capacityLabel
            // 
            this.capacityLabel.BackColor = System.Drawing.Color.Transparent;
            this.capacityLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.capacityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.capacityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.capacityLabel.Location = new System.Drawing.Point(34, 119);
            this.capacityLabel.Name = "capacityLabel";
            this.capacityLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.capacityLabel.Size = new System.Drawing.Size(96, 19);
            this.capacityLabel.TabIndex = 61;
            this.capacityLabel.Text = "35.0 oz.";
            this.capacityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ItemViewForm
            // 
            this.ClientSize = new System.Drawing.Size(378, 165);
            this.Controls.Add(this.goldPerCapLabel);
            this.Controls.Add(this.capacityLabel);
            this.Controls.Add(this.valueDigit10000000000);
            this.Controls.Add(this.decreaseValue10000000000);
            this.Controls.Add(this.increaseValue10000000000);
            this.Controls.Add(this.valueDigit1000000000);
            this.Controls.Add(this.decreaseValue1000000000);
            this.Controls.Add(this.increaseValue1000000000);
            this.Controls.Add(this.valueDigit100000000);
            this.Controls.Add(this.decreaseValue100000000);
            this.Controls.Add(this.increaseValue100000000);
            this.Controls.Add(this.valueDigit10000000);
            this.Controls.Add(this.decreaseValue10000000);
            this.Controls.Add(this.increaseValue10000000);
            this.Controls.Add(this.valueDigit1000000);
            this.Controls.Add(this.decreaseValue1000000);
            this.Controls.Add(this.increaseValue1000000);
            this.Controls.Add(this.valueDigit100000);
            this.Controls.Add(this.decreaseValue100000);
            this.Controls.Add(this.increaseValue100000);
            this.Controls.Add(this.valueDigit10000);
            this.Controls.Add(this.decreaseValue10000);
            this.Controls.Add(this.increaseValue10000);
            this.Controls.Add(this.valueDigit1000);
            this.Controls.Add(this.decreaseValue1000);
            this.Controls.Add(this.increaseValue1000);
            this.Controls.Add(this.valueDigit100);
            this.Controls.Add(this.decreaseValue100);
            this.Controls.Add(this.increaseValue100);
            this.Controls.Add(this.valueDigit10);
            this.Controls.Add(this.decreaseValue10);
            this.Controls.Add(this.increaseValue10);
            this.Controls.Add(this.valueDigit1);
            this.Controls.Add(this.decreaseValue1);
            this.Controls.Add(this.increaseValue1);
            this.Controls.Add(this.convertBox);
            this.Controls.Add(this.pickupBox);
            this.Controls.Add(this.lookText);
            this.Controls.Add(this.itemCategory);
            this.Controls.Add(this.itemName);
            this.Controls.Add(this.itemPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ItemViewForm";
            this.Text = "Item View";
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10000000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10000000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1000000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1000000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue100000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue100000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1000000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue100000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue100000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1000)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue100)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue100)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decreaseValue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.increaseValue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void DestroyForm() {

        }

        private List<TibiaObject>[] objectList = new List<TibiaObject>[4];
        private Control[] objectControls = new Control[4];
        private int currentControlList = -1;
        private int base_y;
        private int initialOffset;
        private string[] extraAttributes = new string[4];
        private Func<TibiaObject, Attribute>[] attributeFunctions = new Func<TibiaObject, Attribute>[4];
        private Func<TibiaObject, IComparable>[] attributeSortFunctions = new Func<TibiaObject, IComparable>[4];
        public override void LoadForm() {
            skip_event = true;
            this.SuspendForm();
            this.NotificationInitialize();
            CultureInfo c = System.Threading.Thread.CurrentThread.CurrentCulture;

            for (int i = 0; i < decreaseBoxes.Count; i++) {
                decreaseBoxes[i].Click -= c_Click;
            }
            for (int i = 0; i < increaseBoxes.Count; i++) {
                increaseBoxes[i].Click -= c_Click;
            }

            this.itemName.Text = c.TextInfo.ToTitleCase(item.displayname);
            Font f = StyleManager.FontList[0];
            for (int i = 0; i < StyleManager.FontList.Count; i++) {
                Font font = StyleManager.FontList[i];
                int width = TextRenderer.MeasureText(this.itemName.Text, font).Width;
                if (width < itemName.MaximumSize.Width) {
                    f = font;
                } else {
                    break;
                }
            }
            this.itemName.Font = f;

            this.capacityLabel.Text = String.Format("{0:0.0} oz.", item.capacity);
            this.goldPerCapLabel.Text = String.Format("{0}/oz.", StyleManager.GoldToText(item.GetMaxValue() / item.capacity));

            this.itemCategory.Text = item.category;
            f = StyleManager.FontList[0];
            for (int i = 0; i < StyleManager.FontList.Count; i++) {
                Font font = StyleManager.FontList[i];
                Size size = TextRenderer.MeasureText(this.itemCategory.Text, font);
                if (size.Width < itemCategory.MaximumSize.Width && size.Height < itemCategory.MaximumSize.Height) {
                    f = font;
                } else {
                    break;
                }
            }
            this.itemCategory.Font = f;


            this.itemPictureBox.BackgroundImage = StyleManager.GetImage("item_background.png");
            this.lookText.Text = item.look_text;
            this.pickupBox.Checked = !item.discard;
            this.convertBox.Checked = item.convert_to_gold;
            this.itemPictureBox.Image = item.image;
            this.updateValue();

            // add a tooltip that displays the actual droprate when you mouseover
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;

            for (int i = 0; i < headers.Length; i++) {
                objectList[i] = new List<TibiaObject>();
            }
            extraAttributes[0] = "Value";
            attributeFunctions[0] = SellPrice;
            attributeSortFunctions[0] = SellSort;
            foreach (ItemSold sold in item.sellItems) {
                objectList[0].Add(new LazyTibiaObject { id = sold.npcid, type = TibiaObjectType.NPC });
            }
            extraAttributes[1] = "Price";
            attributeFunctions[1] = BuyPrice;
            attributeSortFunctions[1] = BuySort;
            foreach (ItemSold sold in item.buyItems) {
                objectList[1].Add(new LazyTibiaObject { id = sold.npcid, type = TibiaObjectType.NPC });
            }
            extraAttributes[2] = "Drop";
            attributeFunctions[2] = DropChance;
            attributeSortFunctions[2] = DropSort;
            foreach (ItemDrop itemDrop in item.itemdrops) {
                objectList[2].Add(new LazyTibiaObject { id = itemDrop.creatureid, type = TibiaObjectType.Creature });
            }
            extraAttributes[3] = null;
            attributeFunctions[3] = null;
            attributeSortFunctions[3] = null;
            foreach (Quest q in item.rewardedBy) {
                objectList[3].Add(q);
            }
            if (currentControlList >= 0 && objectList[currentControlList].Count == 0) currentControlList = -1;
            base_y = this.Size.Height;
            SizeF measuredSize = lookText.CreateGraphics().MeasureString(item.look_text, lookText.Font, lookText.MaximumSize.Width, StringFormat.GenericTypographic);
            if ((int)(lookText.Location.Y + measuredSize.Height) >= base_y) {
                base_y = (int)(lookText.Location.Y + measuredSize.Height);
                this.Size = new Size(this.Size.Width, base_y + 5);
            }
            base_y += 5;
            initialOffset = base_y;
            RefreshForm();
            base.NotificationFinalize();
            this.ResumeForm();
            skip_event = false;
        }

        private List<Control> headerControls = new List<Control>();
        private void refreshHeaders() {
            foreach(Control c in headerControls) {
                Controls.Remove(c);
            }
            headerControls.Clear();
            int x = 5;
            for (int i = 0; i < headers.Length; i++) {
                if (objectList[i].Count > 0) {
                    if (x + 90 > this.Size.Width) {
                        x = 5;
                        base_y += 25;
                    }
                    Label label = new Label();
                    label.Text = headers[i];
                    label.Location = new Point(x, base_y);
                    label.ForeColor = StyleManager.NotificationTextColor;
                    label.BackColor = Color.Transparent;
                    label.Font = StyleManager.TextFont;
                    label.Size = new Size(90, 25);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Name = i.ToString();
                    label.Click += toggleObjectDisplay;
                    objectControls[i] = label;
                    this.Controls.Add(label);
                    headerControls.Add(label);
                    if (currentControlList < 0 || currentControlList > headers.Length) {
                        currentControlList = i;
                    }
                    x += 90;
                } else {
                    objectControls[i] = null;
                }
            }
        }

        private Attribute DropChance(TibiaObject obj) {
            float percentage = item.itemdrops.Find(o => o.creatureid == (obj as LazyTibiaObject).id).percentage;
            return new StringAttribute(percentage > 0 ? String.Format("{0:0.0}%", percentage) : "-", 60);
        }
        private Attribute SellPrice(TibiaObject obj) {
            int npcValue = item.sellItems.Find(o => o.npcid == (obj as LazyTibiaObject).id).price;

            return new StringAttribute(String.Format("{0}", npcValue), 60, npcValue >= item.vendor_value ? StyleManager.ItemGoldColor : StyleManager.CreatureBossColor);
        }
        private Attribute BuyPrice(TibiaObject obj) {
            return new StringAttribute(String.Format("{0}", item.buyItems.Find(o => o.npcid == (obj as LazyTibiaObject).id).price), 60, StyleManager.ItemGoldColor);
        }
        private IComparable DropSort(TibiaObject obj) {
            float percentage = item.itemdrops.Find(o => o.creatureid == (obj as LazyTibiaObject).id).percentage;
            return percentage;
        }
        private IComparable SellSort(TibiaObject obj) {
            return item.sellItems.Find(o => o.npcid == (obj as LazyTibiaObject).id).price;
        }
        private IComparable BuySort(TibiaObject obj) {
            return item.buyItems.Find(o => o.npcid == (obj as LazyTibiaObject).id).price;
        }

        private void toggleObjectDisplay(object sender, EventArgs e) {
            this.currentControlList = int.Parse((sender as Control).Name);
            currentPage = 0;
            this.SuspendForm();
            refreshObjectList();
            this.ResumeForm();
        }

        void updateCommand() {
            string[] split = command.command.Split(Constants.CommandSymbol);
            command.command = split[0] + Constants.CommandSymbol + split[1] + Constants.CommandSymbol + currentPage.ToString() + Constants.CommandSymbol + currentControlList.ToString();
        }

        private string sortedHeader = null;
        private bool desc = false;
        public void sortHeader(object sender, EventArgs e) {
            if (sortedHeader == (sender as Control).Name) {
                desc = !desc;
            } else {
                sortedHeader = (sender as Control).Name;
                desc = false;
            }
            this.SuspendForm();
            refreshObjectList();
            this.ResumeForm();
        }

        private int currentPage = 0;
        private List<Control> controlList = new List<Control>();
        private void refreshObjectList() {
            foreach(Control c in controlList) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            if (currentControlList == -1) {
                return;
            }
            updateCommand();
            for (int i = 0; i < headers.Length; i++) {
                if (objectControls[i] != null) {
                    objectControls[i].Enabled = i != currentControlList;
                    if (i == currentControlList) {
                        (objectControls[i] as Label).BorderStyle = BorderStyle.Fixed3D;
                    } else {
                        (objectControls[i] as Label).BorderStyle = BorderStyle.FixedSingle;
                    }
                }
            }
            controlList.Clear();
            int newwidth;
            PageInfo pageInfo = new PageInfo(false, false);
            int y = base_y + UIManager.DisplayCreatureAttributeList(this.Controls, objectList[currentControlList], 10, base_y, out newwidth, null, controlList, currentPage, 10, pageInfo, extraAttributes[currentControlList], attributeFunctions[currentControlList], sortHeader, sortedHeader, desc, attributeSortFunctions[currentControlList], null, false, this.Size.Width - 14);
            newwidth = this.Size.Width;
            if (pageInfo.prevPage || pageInfo.nextPage) {
                if (pageInfo.prevPage) {
                    PictureBox prevpage = new PictureBox();
                    prevpage.Location = new Point(10, y);
                    prevpage.Size = new Size(97, 23);
                    prevpage.Image = StyleManager.GetImage("prevpage.png");
                    prevpage.BackColor = Color.Transparent;
                    prevpage.SizeMode = PictureBoxSizeMode.Zoom;
                    prevpage.Click += Prevpage_Click; ;
                    this.Controls.Add(prevpage);
                    controlList.Add(prevpage);

                }
                if (pageInfo.nextPage) {
                    PictureBox nextpage = new PictureBox();
                    nextpage.Location = new Point(newwidth - 108, y);
                    nextpage.Size = new Size(98, 23);
                    nextpage.BackColor = Color.Transparent;
                    nextpage.Image = StyleManager.GetImage("nextpage.png");
                    nextpage.SizeMode = PictureBoxSizeMode.Zoom;
                    nextpage.Click += Nextpage_Click; ;
                    this.Controls.Add(nextpage);
                    controlList.Add(nextpage);
                }
                y += 25;
            }
            this.Size = new Size(newwidth, y + 10);
        }

        private void Nextpage_Click(object sender, EventArgs e) {
            currentPage++;
            this.SuspendForm();
            refreshObjectList();
            this.ResumeForm();
        }

        private void Prevpage_Click(object sender, EventArgs e) {
            currentPage--;
            this.SuspendForm();
            refreshObjectList();
            this.ResumeForm();
        }

        private string command_start = "npc" + Constants.CommandSymbol;
        void openItemBox(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            CommandManager.ExecuteCommand(command_start + (sender as Control).Name);
        }

        private string switch_start = "drop" + Constants.CommandSymbol;
        private void statsButton_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            CommandManager.ExecuteCommand(switch_start + (sender as Control).Name);
        }

        private bool skip_event = false;
        private void pickupBox_CheckedChanged(object sender, EventArgs e) {
            if (skip_event) return;
            bool is_checked = (sender as CheckBox).Checked;
            this.ReturnFocusToTibia();
            if (is_checked) CommandManager.ExecuteCommand("pickup" + Constants.CommandSymbol + item.GetName());
            else CommandManager.ExecuteCommand("nopickup" + Constants.CommandSymbol + item.GetName());
        }

        private void convertBox_CheckedChanged(object sender, EventArgs e) {
            if (skip_event) return;
            bool is_checked = (sender as CheckBox).Checked;
            this.ReturnFocusToTibia();
            if (is_checked) CommandManager.ExecuteCommand("convert" + Constants.CommandSymbol + item.GetName());
            else CommandManager.ExecuteCommand("noconvert" + Constants.CommandSymbol + item.GetName());
        }

        private void itemCategory_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("category" + Constants.CommandSymbol + item.category);
        }

        public override string FormName() {
            return "ItemViewForm";
        }

        public override int MinWidth() {
            return 196;
        }

        public override int MaxWidth() {
            return 378;
        }

        public override int WidthInterval() {
            return 200;
        }

        public override void RefreshForm() {
            this.SuspendForm();
            this.Size = new Size(GetWidth(), this.Size.Height);
            if (this.Size.Width == MinWidth()) {
                lookText.Visible = false;
                for(int i = valueLabels.Count - 1; i >= 0; i--) {
                    increaseBoxes[i].Location = new Point(8 + 16 * (valueLabels.Count - (i + 1)), 160);
                    valueLabels[i].Location = new Point(increaseBoxes[i].Location.X, increaseBoxes[i].Location.Y + 16);
                    decreaseBoxes[i].Location = new Point(valueLabels[i].Location.X, valueLabels[i].Location.Y + 16);
                }
                pickupBox.Location = new Point(4, 208);
                convertBox.Location = new Point(90, 208);
                itemPictureBox.Location = new Point(this.Size.Width / 2 - itemPictureBox.Width / 2, itemPictureBox.Location.Y);
                itemCategory.Location = new Point(this.Size.Width / 2 - itemCategory.Width / 2, itemCategory.Location.Y);
                itemName.Location = new Point(this.Size.Width / 2 - itemName.Width / 2, itemName.Location.Y);
                capacityLabel.Location = new Point(this.Size.Width / 2 - capacityLabel.Width / 2, capacityLabel.Location.Y);
                goldPerCapLabel.Location = new Point(this.Size.Width / 2 - goldPerCapLabel.Width / 2, goldPerCapLabel.Location.Y);
                base_y = 228;
                refreshHeaders();
                base_y += 25;
                refreshObjectList();
                int max_y = 0;
                foreach(Control c in this.Controls) {
                    if (c.Location.Y + c.Size.Height > max_y) {
                        max_y = c.Location.Y + c.Size.Height;
                    }
                }
                this.Size = new Size(this.Size.Width, Math.Max(max_y + 5, this.Size.Height));
            } else {
                lookText.Visible = true;
                for (int i = valueLabels.Count - 1; i >= 0; i--) {
                    increaseBoxes[i].Location = new Point(153 + 16 * (valueLabels.Count - (i + 1)), 26);
                    valueLabels[i].Location = new Point(increaseBoxes[i].Location.X, increaseBoxes[i].Location.Y + 16);
                    decreaseBoxes[i].Location = new Point(valueLabels[i].Location.X, valueLabels[i].Location.Y + 16);
                }
                pickupBox.Location = new Point(133, 76);
                convertBox.Location = new Point(239, 75);
                this.itemCategory.Location = new Point(34, 27);
                this.itemName.Location = new Point(34, 89);
                this.itemPictureBox.Location = new Point(66, 56);
                this.capacityLabel.Location = new Point(34, 119);
                this.goldPerCapLabel.Location = new Point(34, 137);
                base_y = initialOffset;
                refreshHeaders();
                base_y += 25;
                refreshObjectList();
            }

            this.ResumeForm();
        }
    }
}
