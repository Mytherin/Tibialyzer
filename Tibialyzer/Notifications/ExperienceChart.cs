
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
    class ExperienceChart : NotificationForm {
        private TransparentChart mChart;
        public string filter = "";

        public ExperienceChart() {
            InitializeComponent();
            RefreshForm();
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExperienceChart));
            this.mChart = new Tibialyzer.TransparentChart();
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
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
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
            // ExperienceChart
            //
            this.ClientSize = new System.Drawing.Size(450, 321);
            this.Controls.Add(this.mChart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExperienceChart";
            this.Text = "Damage Chart";
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).EndInit();
            this.ResumeLayout(false);

        }

        public void refreshExperienceChart() {
            this.mChart.Visible = true;
            this.mChart.Series[0].Points.Clear();
            this.mChart.ChartAreas[0].AxisX.CustomLabels.Clear();
            Dictionary<string, int> experienceCopy = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> kvp in GlobalDataManager.GetExperience()) {
                experienceCopy.Add(kvp.Key, kvp.Value);
            }

            List<string> stamps = TimestampManager.getLatestTimes(60);
            stamps.Reverse();
            int index = 60;
            foreach (string stamp in stamps) {
                if (experienceCopy.ContainsKey(stamp)) {
                    break;
                }
                index--;
            }
            index = Math.Max(index, 5);
            int interval = Math.Max(1, index / 12);
            this.mChart.ChartAreas[0].AxisX.Interval = interval;
            this.mChart.ChartAreas[0].AxisX.Minimum = 1;
            this.mChart.ChartAreas[0].AxisX.Maximum = index;

            stamps = TimestampManager.getLatestTimes(index);
            stamps.Reverse();
            int i = 0;
            long average = 0;
            foreach (string stamp in stamps) {
                if (i % interval == 0) {
                    this.mChart.ChartAreas[0].AxisX.CustomLabels.Add(i, i + interval, stamp);
                }
                int exp = experienceCopy.ContainsKey(stamp) ? experienceCopy[stamp] : 0;
                this.mChart.Series[0].Points.AddXY(++i, exp * 60);
                average += exp;
            }
            stripLine.IntervalOffset = 60 * (average / i);
            mChart.ChartAreas[0].AxisY.StripLines.Clear();
            mChart.ChartAreas[0].AxisY.StripLines.Add(stripLine);

            refreshTimer();
        }

        public void UpdateExperience() {
            try {
                this.Invoke((MethodInvoker)delegate {
                    refreshExperienceChart();
                });
            } catch {

            }
        }

        StripLine stripLine = new StripLine();
        public override void LoadForm() {
            this.SuspendForm();
            NotificationInitialize();

            mChart.ForeColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisX.LineColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisY.LineColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisX.InterlacedColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisY.InterlacedColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisX.MajorGrid.LineColor = StyleManager.NotificationTextColor;
            mChart.ChartAreas[0].AxisY.MajorGrid.LineColor = StyleManager.NotificationTextColor;
            mChart.Series[0].IsVisibleInLegend = false;
            stripLine.Interval = 0;
            stripLine.IntervalOffset = 100;
            stripLine.StripWidth = 2;
            stripLine.BackColor = StyleManager.SummaryExperienceColor;
            mChart.ChartAreas[0].AxisY.StripLines.Add(stripLine);

            refreshExperienceChart();
            this.ResumeForm();
            NotificationFinalize();
        }

        public override string FormName() {
            return "ExperienceChart";
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

        public override void RefreshForm() {
            this.SuspendForm();
            this.Size = new Size(GetWidth(), (int)(GetWidth() * 0.9));
            mChart.Size = new Size(this.Size.Width, this.Size.Height);
            this.ResumeForm();
        }
    }
}
