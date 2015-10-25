using System;
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

namespace Tibialyzer
{
    public partial class CreatureStatsForm : NotificationForm
    {
        struct Resistance
        {
            public string name;
            public int resistance;
            public Resistance(string name, int resistance) {
                this.name = name;
                this.resistance = resistance;
            }
        }

        private static Dictionary<string, Color> resistance_colors = new Dictionary<string, Color>();
        private static Dictionary<string, Image> resistance_images = new Dictionary<string, Image>();
        private PictureBox[] resistance_controls = new PictureBox[7];
        public Creature creature;
        public CreatureStatsForm()
        {
            InitializeComponent();
            // add colors for every resistance

            resistance_controls[0] = resistanceLabel1;
            resistance_controls[1] = resistanceLabel2;
            resistance_controls[2] = resistanceLabel3;
            resistance_controls[3] = resistanceLabel4;
            resistance_controls[4] = resistanceLabel5;
            resistance_controls[5] = resistanceLabel6;
            resistance_controls[6] = resistanceLabel7;
        }

        public static void InitializeCreatureStats()
        {
            resistance_colors.Add("Ice", Color.DodgerBlue);
            resistance_colors.Add("Fire", Color.Firebrick);
            resistance_colors.Add("Holy", Color.DarkOrange);
            resistance_colors.Add("Phys", Color.DimGray);
            resistance_colors.Add("Earth", Color.ForestGreen);
            resistance_colors.Add("Death", Color.FromArgb(32, 32, 32));
            resistance_colors.Add("Energy", Color.MidnightBlue);
            //and images
            foreach(string str in resistance_colors.Keys) {
                resistance_images.Add(str, System.Drawing.Image.FromFile(@"Images/" + str.ToLower() + ".png"));
            }
        }

        private void AddResistances(List<Resistance> resistances)
        {
            List<Resistance> sorted_list = resistances.OrderByDescending(o => o.resistance).ToList();
            int i = 0;
            foreach (Resistance resistance in sorted_list) {
                // add a tooltip that displays the actual resistance when you mouseover
                ToolTip resistance_tooltip = new ToolTip();
                resistance_tooltip.AutoPopDelay = 60000;
                resistance_tooltip.InitialDelay = 500;
                resistance_tooltip.ReshowDelay = 500;
                resistance_tooltip.ShowAlways = true;
                resistance_tooltip.UseFading = true;
                resistance_tooltip.SetToolTip(resistance_controls[i], "Damage taken from " + resistance.name + ": " + resistance.resistance.ToString() + "%");

                Bitmap bitmap = new Bitmap(19 + resistance.resistance, 19);
                Graphics gr = Graphics.FromImage(bitmap);
                using (Brush brush = new SolidBrush(resistance_colors[resistance.name]))
                {
                    gr.FillRectangle(brush, new Rectangle(19, 0, bitmap.Width - 19, bitmap.Height));
                }
                gr.DrawImage(resistance_images[resistance.name], new Point(2, 2));
                resistance_controls[i].Width = bitmap.Width;
                resistance_controls[i].Height = bitmap.Height;
                resistance_controls[i].Image = bitmap;
                i++;
            }
        }


        private void CreatureStatsForm_Load(object sender, EventArgs e)
        {
            int horizontal, left, right;
            int health = creature.health;
            int experience = creature.experience;
            List<Resistance> resistances = new List<Resistance>();
            resistances.Add(new Resistance("Ice", creature.res_ice));
            resistances.Add(new Resistance("Holy", creature.res_holy));
            resistances.Add(new Resistance("Death", creature.res_death));
            resistances.Add(new Resistance("Phys", creature.res_phys));
            resistances.Add(new Resistance("Earth", creature.res_earth));
            resistances.Add(new Resistance("Energy", creature.res_energy));
            resistances.Add(new Resistance("Fire", creature.res_fire));
            // load image from the creature
            this.mainImage.Image = creature.image;
            // set health of creature
            this.healthLabel.Text = health.ToString() + " Health";
            horizontal = 100 - healthLabel.Size.Width;
            left = horizontal / 2;
            right = horizontal - left;
            this.healthLabel.Padding = new Padding(left, 2, right, 2);
            // set exp of creature
            this.expLabel.Text = experience.ToString() + " Exp";
            horizontal = 100 - expLabel.Size.Width;
            left = horizontal / 2;
            right = horizontal - left;
            this.expLabel.Padding = new Padding(left, 2, right, 2);
            // add resistances of creature in order
            AddResistances(resistances);
            // set background of actual form to transparent
            this.BackColor = MainForm.background_color;
            this.Opacity = MainForm.opacity;
            if (MainForm.transparent) {
                this.TransparencyKey = MainForm.background_color;
                this.Opacity = 1;
            }
            this.nameLabel.Text = this.creature.name;
            Font f = this.nameLabel.Font;
            while (TextRenderer.MeasureText(this.creature.name, f).Width < this.mainImage.Size.Width && TextRenderer.MeasureText(this.creature.name, f).Height < 26)            {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size + 1.0f);
            }
            while (TextRenderer.MeasureText(this.creature.name, f).Width > this.mainImage.Size.Width && f.Size > 1)
            {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size - 1.0f);
            }
            this.nameLabel.Font = f;
            this.nameLabel.Left = this.mainImage.Left + (mainImage.Width - this.nameLabel.Size.Width) / 2;
            base.NotificationInitialize();
            base.NotificationFinalize();
        }

    }
}
