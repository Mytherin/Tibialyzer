using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Tibialyzer
{
    class DamageMeter : NotificationForm
    {
        private TransparentChart damageChart;
        public Dictionary<string, int> dps = new Dictionary<string, int>();
        struct Player
        {
            public string name;
            public int damage;
        };
        public string filter = "";
        private string screenshot_path;
        public DamageMeter(string screenshot_path = "")
        {
            this.screenshot_path = screenshot_path;
            if (screenshot_path != "") this.Visible = false;
            InitializeComponent();
            NotificationInitialize();
        }

        private void SaveScreenshot()
        {
            Bitmap bitmap = new Bitmap(Size.Width, Size.Height);
            DrawToBitmap(bitmap, new Rectangle(0, 0, Size.Width, Size.Height));
            bitmap.Save(screenshot_path);
            bitmap.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Cleanup();
                if (damageChart != null) damageChart.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.damageChart = new Tibialyzer.TransparentChart();
            ((System.ComponentModel.ISupportInitialize)(this.damageChart)).BeginInit();
            this.SuspendLayout();
            // 
            // damageChart
            // 
            this.damageChart.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.damageChart.ChartAreas.Add(chartArea1);
            this.damageChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.damageChart.Legends.Add(legend1);
            this.damageChart.Location = new System.Drawing.Point(0, 0);
            this.damageChart.Name = "damageChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.CustomProperties = "PieLineColor=191\\, 191\\, 191, PieLabelStyle=Outside";
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.Yes;
            this.damageChart.Series.Add(series1);
            this.damageChart.Size = new System.Drawing.Size(374, 311);
            this.damageChart.TabIndex = 0;
            this.damageChart.Text = "Damage";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            title1.Name = "Title1";
            title1.Text = "Damage Dealt";
            this.damageChart.Titles.Add(title1);
            // 
            // DamageMeter
            // 
            this.ClientSize = new System.Drawing.Size(374, 311);
            this.Controls.Add(this.damageChart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DamageMeter";
            this.Load += new System.EventHandler(this.DamageMeter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.damageChart)).EndInit();
            this.ResumeLayout(false);

        }

        void DamageMeter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.damageChart.ChartAreas.Clear();
            this.damageChart.Legends.Clear();
            this.damageChart.Series.Clear();
            this.Controls.Remove(damageChart);
        }

        private void DamageMeter_Load(object sender, EventArgs e)
        {
            this.SuspendForm();
            int maxdeeps = -1;
            foreach (int deeps in dps.Values)
            {
                if (deeps > maxdeeps) maxdeeps = deeps;
            }

            List<Player> players = new List<Player>();

            foreach (KeyValuePair<string, int> kvp in dps)
            {
                if (filter != "all" && filter != "creature" && char.IsLower(kvp.Key[0])) continue;
                if (filter == "creature" && char.IsUpper(kvp.Key[0])) continue;
                Player p = new Player();
                p.name = kvp.Key;
                p.damage = kvp.Value;
                players.Add(p);
            }
            players.OrderByDescending(o => o.damage);
            if (players.Count == 0)
            {
                players.Add(new Player() { damage = 40, name = "You" });
                players.Add(new Player() { damage = 30, name = "Mytherin" });
                players.Add(new Player() { damage = 35, name = "Martincc" });
                players.Add(new Player() { damage = 15, name = "Amel Cyrom" });
                players.Add(new Player() { damage = 20, name = "Sample" });
            }

            double total_damage = 0;
            foreach(Player player in players)
            {
                total_damage = total_damage + player.damage;
            }

            int i = 0;
            this.damageChart.Series[0].Points.Clear();
            foreach (Player p in players)
            {
                double percentage = p.damage / total_damage * 100;
                DataPoint point = new DataPoint();
                point.XValue = percentage;
                point.YValues = new double[1];
                point.YValues[0] = p.damage;
                point.AxisLabel = p.name;
                point.LegendText = p.name;
                point.Label = Math.Round(percentage, 1).ToString() + "%";
                this.damageChart.Series[0].Points.Add(point);
                i++;
            }
            if (screenshot_path == "")
                base.NotificationFinalize();
            this.ResumeForm();
            if (screenshot_path != "")
                SaveScreenshot();
        }

    }
}
