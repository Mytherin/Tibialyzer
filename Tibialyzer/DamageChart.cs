
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Tibialyzer {
    class DamageChart : NotificationForm {
        private TransparentChart mChart;
        public Dictionary<string, Tuple<int, int>> dps;
        public bool graph = true;
        private Label detailsButton;
        public string filter = "";

        public DamageChart() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DamageChart));
            this.mChart = new Tibialyzer.TransparentChart();
            this.detailsButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).BeginInit();
            this.SuspendLayout();
            // 
            // mChart
            // 
            this.mChart.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.mChart.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            legend1.Name = "Legend1";
            this.mChart.Legends.Add(legend1);
            this.mChart.Location = new System.Drawing.Point(0, -1);
            this.mChart.Name = "mChart";
            series1.BorderColor = System.Drawing.Color.Transparent;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.LabelBackColor = System.Drawing.Color.Transparent;
            series1.LabelBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            series1.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            series1.LabelBorderWidth = 0;
            series1.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.mChart.Series.Add(series1);
            this.mChart.Size = new System.Drawing.Size(448, 320);
            this.mChart.TabIndex = 0;
            this.mChart.Text = "Damage Chart";
            // 
            // detailsButton
            // 
            this.detailsButton.BackColor = System.Drawing.Color.Transparent;
            this.detailsButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detailsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.detailsButton.Location = new System.Drawing.Point(12, 291);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Padding = new System.Windows.Forms.Padding(2);
            this.detailsButton.Size = new System.Drawing.Size(96, 21);
            this.detailsButton.TabIndex = 3;
            this.detailsButton.Text = "Details";
            this.detailsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // DamageChart
            // 
            this.ClientSize = new System.Drawing.Size(450, 321);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.mChart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DamageChart";
            this.Text = "Damage Chart";
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).EndInit();
            this.ResumeLayout(false);

        }

        private List<Control> controlList = new List<Control>();
        public void refreshDamageChart() {
            foreach (Control c in controlList) {
                c.Dispose();
                Controls.Remove(c);
            }
            controlList.Clear();
            if (graph) {
                this.mChart.Visible = true;
                this.mChart.Series[0].Points.Clear();
                for (int i = 0; i < damageDealt.Count; i++) {
                    DamageObject p = damageDealt[i];
                    double percentage = p.percentage;
                    DataPoint point = new DataPoint();
                    point.XValue = percentage;
                    point.YValues = new double[1];
                    point.YValues[0] = p.totalDamage;
                    point.AxisLabel = p.name;
                    point.LegendText = p.name;
                    point.Label = Math.Round(percentage, 1).ToString() + "%";
                    this.mChart.Series[0].Points.Add(point);
                }
                this.mChart.ApplyPaletteColors();
                for (int i = 0; i < damageDealt.Count; i++) {
                    DamageObject p = damageDealt[i];
                    p.color = this.mChart.Series[0].Points[i].Color;
                }
                this.Size = new Size(startX, startY);
            } else {
                this.mChart.Series[0].Points.Clear();
                this.mChart.Visible = false;
                int newWidth = 0;
                int y = MainForm.DisplayCreatureAttributeList(Controls, damageDealt.ToList<TibiaObject>(), 5, 25, out newWidth, null, controlList, 0, 20, null, null, null, sortFunction, sortedHeader, desc);

                this.Size = new Size(Math.Max(startX, newWidth), Math.Max(startY, 25 + y));
            }
            refreshTimer();
        }

        private string sortedHeader = "Total Damage";
        private bool desc = true;
        public void sortFunction(object sender, EventArgs e) {
            if (sortedHeader == (sender as Control).Name) {
                desc = !desc;
            } else {
                sortedHeader = (sender as Control).Name;
                desc = false;
            }
            this.SuspendForm();
            refreshDamageChart();
            this.ResumeForm();
        }

        int startX, startY;

        private List<DamageObject> damageDealt = new List<DamageObject>();

        private void detailsButton_Click(object sender, EventArgs e) {
            graph = !graph;
            if (graph) {
                detailsButton.Text = "Details";
            } else {
                detailsButton.Text = "Graph";
            }
            this.SuspendForm();
            refreshDamageChart();
            this.ResumeForm();
        }

        public override void LoadForm() {
            this.SuspendForm();
            NotificationInitialize();
            detailsButton.Click -= c_Click;
            startX = this.Size.Width;
            startY = this.Size.Height;
            foreach (KeyValuePair<string, Tuple<int, int>> kvp in dps) {
                if (filter != "all" && filter != "creature" && char.IsLower(kvp.Key[0])) continue;
                if (filter == "creature" && char.IsUpper(kvp.Key[0])) continue;
                damageDealt.Add(new DamageObject() { name = kvp.Key.Replace(".", ""), totalDamage = kvp.Value.Item1, dps = (double)kvp.Value.Item1 / (double)kvp.Value.Item2 });
            }
            damageDealt.OrderByDescending(o => o.totalDamage);
            if (damageDealt.Count == 0) {
                damageDealt.Add(new DamageObject() { name = "Mytherin", dps = 50, totalDamage = 501 });
                damageDealt.Add(new DamageObject() { name = "Amel Cyrom", dps = 50, totalDamage = 250 });
                damageDealt.Add(new DamageObject() { name = "Martincc", dps = 50, totalDamage = 499 });
                damageDealt.Add(new DamageObject() { name = "You", dps = 50, totalDamage = 750 });
            }

            double total_damage = 0;
            foreach (DamageObject player in damageDealt) {
                total_damage = total_damage + player.totalDamage;
            }
            foreach (DamageObject p in damageDealt) {
                p.percentage = p.totalDamage / total_damage * 100;
            }

            refreshDamageChart();
            this.ResumeForm();
            NotificationFinalize();
        }
    }
}
