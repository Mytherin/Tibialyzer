
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
    enum DamageChartType { DamageDealt, DamageTaken, HealingDone };

    class DamageChart : NotificationForm {
        private TransparentChart mChart;
        public Dictionary<string, DamageResult> dps;
        public bool graph = true;
        private Label detailsButton;
        private ComboBox targetBox;
        public string filter = "";
        private string target = null;
        private Label chartTitle;
        private DamageChartType chartType;

        public DamageChart(DamageChartType chartType) {
            InitializeComponent();
            this.chartType = chartType;
            switch (chartType) {
                case DamageChartType.DamageDealt:
                    this.mChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.BrightPastel;
                    chartTitle.Text = "Damage Dealt"; break;
                case DamageChartType.DamageTaken:
                    this.mChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Chocolate;
                    chartTitle.Text = "Damage Taken"; break;
                case DamageChartType.HealingDone:
                    this.mChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
                    chartTitle.Text = "Healing Done"; break;
            }
            this.Name = "Tibialyzer (Damage Form)";
            this.Text = "Tibialyzer (Damage Form)";
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DamageChart));
            this.targetBox = new System.Windows.Forms.ComboBox();
            this.detailsButton = new System.Windows.Forms.Label();
            this.mChart = new Tibialyzer.TransparentChart();
            this.chartTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).BeginInit();
            this.SuspendLayout();
            // 
            // targetBox
            // 
            this.targetBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetBox.FormattingEnabled = true;
            this.targetBox.Location = new System.Drawing.Point(75, 4);
            this.targetBox.Name = "targetBox";
            this.targetBox.Size = new System.Drawing.Size(96, 21);
            this.targetBox.TabIndex = 4;
            this.targetBox.SelectedIndexChanged += new System.EventHandler(this.targetBox_SelectedIndexChanged);
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
            // mChart
            // 
            this.mChart.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.mChart.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Far;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            legend1.Name = "Legend1";
            this.mChart.Legends.Add(legend1);
            this.mChart.Location = new System.Drawing.Point(0, -1);
            this.mChart.Name = "mChart";
            this.mChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
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
            // chartTitle
            // 
            this.chartTitle.AutoSize = true;
            this.chartTitle.BackColor = System.Drawing.Color.Transparent;
            this.chartTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.chartTitle.Location = new System.Drawing.Point(8, 30);
            this.chartTitle.Name = "chartTitle";
            this.chartTitle.Size = new System.Drawing.Size(32, 16);
            this.chartTitle.TabIndex = 15;
            this.chartTitle.Text = "List";
            // 
            // DamageChart
            // 
            this.ClientSize = new System.Drawing.Size(450, 321);
            this.Controls.Add(this.chartTitle);
            this.Controls.Add(this.targetBox);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.mChart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DamageChart";
            this.Text = "Damage Chart";
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
                int max = SettingsManager.getSettingInt("MaxDamageChartPlayers");
                for (int i = 0; i < damageDealt.Count; i++) {
                    if (max > 0 && i >= max) break;
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
                    if (max > 0 && i >= max) break;
                    DamageObject p = damageDealt[i];
                    p.color = this.mChart.Series[0].Points[i].Color;
                }
                this.Size = new Size(GetWidth(), (int)(GetWidth() * 0.9));
            } else {
                this.mChart.Series[0].Points.Clear();
                this.mChart.Visible = false;
                int newWidth = 0;
                int y = UIManager.DisplayCreatureAttributeList(Controls, damageDealt.ToList<TibiaObject>(), 5, 25, out newWidth, null, controlList, 0, 20, null, null, null, sortFunction, sortedHeader, desc, null, null, false, this.Size.Width - 20);

                this.Size = new Size(GetWidth(), Math.Max(startY, 25 + y));
            }
            refreshTimer();
        }

        public void UpdateDamage() {
            try {
                List<Color> colorList = null;
                if (chartType == DamageChartType.DamageDealt) {
                    this.dps = ScanningManager.lastResults.DamagePerSecond;
                    colorList = Constants.DamageChartColors;
                } else if (chartType == DamageChartType.HealingDone){
                    this.dps = ScanningManager.lastResults.HealingPerSecond;
                    colorList = Constants.HealingChartColors;
                } else if (chartType == DamageChartType.DamageTaken) {
                    this.dps = ScanningManager.lastResults.DamageTakenPerSecond;
                    colorList = Constants.DamageTakenChartColors;
                }

                var res = GenerateDamageInformation(dps, filter, target);
                damageDealt = res.Item2;
                for(int i = 0; i < damageDealt.Count; i++) {
                    damageDealt[i].color = colorList[i % colorList.Count];
                }
                this.Invoke((MethodInvoker)delegate {
                    try {
                        preventTargetUpdate = true;
                        string selected = targetBox.SelectedIndex < 0 ? "All" : targetBox.Items[targetBox.SelectedIndex].ToString();
                        targetBox.Items.Clear();
                        targetBox.Items.Add("All");
                        int index = 0;
                        foreach (var kvp in res.Item1.OrderByDescending(o => o.Value)) {
                            string target = kvp.Key;
                            if (target.Equals(selected, StringComparison.InvariantCultureIgnoreCase)) {
                                index = targetBox.Items.Count;
                            }
                            targetBox.Items.Add(target);
                        }
                        targetBox.SelectedIndex = index;
                        preventTargetUpdate = false;
                        refreshDamageChart();
                        RefreshForm();
                    } catch {

                    }
                });
            } catch {

            }
        }

        private void refreshTargets() {

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
            RefreshForm();
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
            RefreshForm();
            this.ResumeForm();
        }


        public static Tuple<Dictionary<string, int>, List<DamageObject>> GenerateDamageInformation(Dictionary<string, DamageResult> dps, string filter, string target = null) {
            List<DamageObject> damageDealt = new List<DamageObject>();
            Dictionary<string, int> targets = new Dictionary<string, int>();
            foreach (KeyValuePair<string, DamageResult> kvp in dps) {
                string name = kvp.Key.Replace(".", "");
                if (name.Substring(0, 2).Equals("a ", StringComparison.InvariantCultureIgnoreCase)) name = name.Substring(2, name.Length - 2);
                if (name.Substring(0, 3).Equals("an ", StringComparison.InvariantCultureIgnoreCase)) name = name.Substring(3, name.Length - 3);

                Creature cr = StorageManager.getCreature(name);
                if (filter != "all" && filter != "creature" && cr != null) continue;
                if (filter == "creature" && cr == null) continue;

                foreach (var creatureDamage in kvp.Value.damagePerCreature) {
                    if (!targets.ContainsKey(creatureDamage.Key)) {
                        targets.Add(creatureDamage.Key, creatureDamage.Value);
                    } else {
                        targets[creatureDamage.Key] += creatureDamage.Value;
                    }
                }
                int totalDamage = kvp.Value.totalDamage;
                if (target != null && !target.Equals("all", StringComparison.InvariantCultureIgnoreCase)) {
                    totalDamage = 0;
                    foreach(var v in kvp.Value.damagePerCreature) {
                        if (v.Key.Equals(target, StringComparison.InvariantCultureIgnoreCase)) {
                            totalDamage = v.Value;
                            break;
                        }
                    }
                }
                if (totalDamage > 0) {
                    damageDealt.Add(new DamageObject() { name = name, totalDamage = totalDamage, dps = kvp.Value.damagePerSecond });
                }
            }
            if (damageDealt.Count == 0) {
                damageDealt.Add(new DamageObject() { name = "Mytherin", dps = 50, totalDamage = 501 });
                damageDealt.Add(new DamageObject() { name = "Amel Cyrom", dps = 50, totalDamage = 250 });
                damageDealt.Add(new DamageObject() { name = "Martincc", dps = 50, totalDamage = 499 });
            }
            double total_damage = 0;
            foreach (DamageObject player in damageDealt) {
                total_damage = total_damage + player.totalDamage;
            }
            foreach (DamageObject p in damageDealt) {
                p.percentage = p.totalDamage / total_damage * 100;
            }
            damageDealt = damageDealt.OrderByDescending(o => o.totalDamage).ToList();
            return new Tuple<Dictionary<string, int>, List<DamageObject>>(targets, damageDealt);
        }

        public override void LoadForm() {
            this.SuspendForm();
            NotificationInitialize();
            detailsButton.Click -= c_Click;
            targetBox.Click -= c_Click;
            startX = this.Size.Width;
            startY = this.Size.Height;

            var res = GenerateDamageInformation(dps, filter, target);
            damageDealt = res.Item2;
            preventTargetUpdate = true;
            targetBox.Items.Clear();
            targetBox.Items.Add("All");
            foreach(var kvp in res.Item1.OrderByDescending(o => o.Value)) {
                string target = kvp.Key;
                targetBox.Items.Add(target);
            }
            targetBox.SelectedIndex = 0;
            preventTargetUpdate = false;

            refreshDamageChart();
            this.ResumeForm();
            NotificationFinalize();
            RefreshForm();
        }

        public override string FormName() {
            switch (chartType) {
                case DamageChartType.DamageDealt:
                    return "DamageChart";
                case DamageChartType.DamageTaken:
                    return "DamageTakenChart";
                case DamageChartType.HealingDone:
                    return "HealingChart";
            }
            return "UnknownChart";
        }

        public override int MinWidth() {
            return 200;
        }

        public override int MaxWidth() {
            return 800;
        }

        public override int WidthInterval() {
            return 70;
        }

        private bool preventTargetUpdate = false;
        private void targetBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (preventTargetUpdate) return;
            string newTarget;
            if (targetBox.SelectedIndex >= 0) {
                newTarget = targetBox.Items[targetBox.SelectedIndex].ToString();
            } else {
                newTarget = null;
            }
            if (newTarget != target) {
                target = newTarget;
                preventTargetUpdate = true;
                UpdateDamage();
                preventTargetUpdate = false;
            }
        }

        public override void RefreshForm() {
            this.SuspendForm();
            if (graph) {
                this.Size = new Size(GetWidth(), (int)(GetWidth() * 0.9));
            } else {
                this.Size = new Size(GetWidth(), 321);
            }
            mChart.Size = new Size(this.Size.Width, this.Size.Height);
            this.detailsButton.Location = new Point(3, this.Size.Height - detailsButton.Height - 5);
            this.refreshDamageChart();
            this.ResumeForm();
        }
    }
}
