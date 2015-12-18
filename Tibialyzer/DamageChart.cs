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
        public Dictionary<string, int> dps;
        struct Player {
            public string name;
            public int damage;
        };
        public string filter = "";

        private string screenshot_path;
        public DamageChart(string screenshot_path = "") {
            this.screenshot_path = screenshot_path;
            if (screenshot_path != "") this.Visible = false;
            InitializeComponent();
            NotificationInitialize();
        }
        private void SaveScreenshot() {
            Bitmap bitmap = new Bitmap(Size.Width, Size.Height);
            DrawToBitmap(bitmap, new Rectangle(0, 0, Size.Width, Size.Height));
            bitmap.Save(screenshot_path);
            bitmap.Dispose();
        }

        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DamageChart));
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
            // DamageChart
            // 
            this.ClientSize = new System.Drawing.Size(450, 321);
            this.Controls.Add(this.mChart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DamageChart";
            this.Text = "Damage Chart";
            this.Load += new System.EventHandler(this.DamageChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).EndInit();
            this.ResumeLayout(false);

        }

        private void DamageChart_Load(object sender, EventArgs e) {
            this.SuspendForm();
            int maxdeeps = -1;
            foreach (int deeps in dps.Values) {
                if (deeps > maxdeeps) maxdeeps = deeps;
            }

            List<Player> players = new List<Player>();

            foreach (KeyValuePair<string, int> kvp in dps) {
                if (filter != "all" && filter != "creature" && char.IsLower(kvp.Key[0])) continue;
                if (filter == "creature" && char.IsUpper(kvp.Key[0])) continue;
                Player p = new Player();
                p.name = kvp.Key;
                p.damage = kvp.Value;
                players.Add(p);
            }
            players.OrderByDescending(o => o.damage);
            if (players.Count == 0) {
                players.Add(new Player() { damage = 40, name = "You" });
                players.Add(new Player() { damage = 30, name = "Mytherin" });
                players.Add(new Player() { damage = 35, name = "Martincc" });
                players.Add(new Player() { damage = 15, name = "Amel Cyrom" });
                players.Add(new Player() { damage = 20, name = "Sample" });
            }

            double total_damage = 0;
            foreach (Player player in players) {
                total_damage = total_damage + player.damage;
            }

            int i = 0;
            this.mChart.Series[0].Points.Clear();
            foreach (Player p in players) {
                double percentage = p.damage / total_damage * 100;
                DataPoint point = new DataPoint();
                point.XValue = percentage;
                point.YValues = new double[1];
                point.YValues[0] = p.damage;
                point.AxisLabel = p.name;
                point.LegendText = p.name;
                point.Label = Math.Round(percentage, 1).ToString() + "%";
                this.mChart.Series[0].Points.Add(point);
                i++;
            }
            /*
            Bitmap bitmap = new Bitmap(this.mChart.Width, this.mChart.Height);
            this.mChart.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

            PictureBox box = new PictureBox();
            box.Location = new Point(mChart.Location.X, mChart.Location.Y);
            box.Size = new Size(mChart.Width, mChart.Height);
            box.Image = bitmap;
            box.BackColor = Color.Transparent;
            this.Controls.Remove(mChart);
            this.Controls.Add(box);
            mChart.Dispose();
            RegisterForClose(box);*/
            if (screenshot_path == "")
                base.NotificationFinalize();
            this.ResumeForm();
            if (screenshot_path != "")
                SaveScreenshot();

        }
    }
}
