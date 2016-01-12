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

namespace Tibialyzer {
    public partial class CreatureStatsForm : NotificationForm {
        struct Resistance {
            public string name;
            public int resistance;
            public Resistance(string name, int resistance) {
                this.name = name;
                this.resistance = resistance;
            }
        }

        private ToolTip resistance_tooltip = new ToolTip();
        public static Dictionary<string, Color> resistance_colors = new Dictionary<string, Color>();
        public static Dictionary<string, Image> resistance_images = new Dictionary<string, Image>();
        private System.Windows.Forms.PictureBox[] resistance_controls = new PictureBox[7];
        public Creature creature;
        public CreatureStatsForm() {
            InitializeComponent();

            resistance_tooltip.AutoPopDelay = 60000;
            resistance_tooltip.InitialDelay = 500;
            resistance_tooltip.ReshowDelay = 500;
            resistance_tooltip.ShowAlways = true;
            resistance_tooltip.UseFading = true;
            
            // add colors for every resistance
            resistance_controls[0] = resistanceLabel1;
            resistance_controls[1] = resistanceLabel2;
            resistance_controls[2] = resistanceLabel3;
            resistance_controls[3] = resistanceLabel4;
            resistance_controls[4] = resistanceLabel5;
            resistance_controls[5] = resistanceLabel6;
            resistance_controls[6] = resistanceLabel7;
        }

        public static void InitializeCreatureStats() {
            resistance_colors.Add("Ice", Color.DodgerBlue);
            resistance_colors.Add("Fire", Color.FromArgb(255,64,64));
            resistance_colors.Add("Holy", Color.DarkOrange);
            resistance_colors.Add("Phys", Color.DimGray);
            resistance_colors.Add("Earth", Color.ForestGreen);
            resistance_colors.Add("Death", Color.FromArgb(32, 32, 32));
            resistance_colors.Add("Energy", Color.MidnightBlue);
            //and images
            foreach (string str in resistance_colors.Keys) {
                resistance_images.Add(str, System.Drawing.Image.FromFile(@"Images/" + str.ToLower() + ".png"));
            }
        }

        private void AddResistances(List<Resistance> resistances) {
            List<Resistance> sorted_list = resistances.OrderByDescending(o => o.resistance).ToList();
            int i = 0;
            foreach (Resistance resistance in sorted_list) {
                resistance_tooltip.SetToolTip(resistance_controls[i], "Damage taken from " + resistance.name + ": " + resistance.resistance.ToString() + "%");

                // add a tooltip that displays the actual resistance when you mouseover
                Bitmap bitmap = new Bitmap(19 + resistance.resistance, 19);
                Graphics gr = Graphics.FromImage(bitmap);
                using (Brush brush = new SolidBrush(resistance_colors[resistance.name])) {
                    gr.FillRectangle(brush, new Rectangle(19, 0, bitmap.Width - 19, bitmap.Height));
                }
                gr.DrawRectangle(Pens.Black, new Rectangle(19, 0, bitmap.Width - 20, bitmap.Height - 1));
                gr.DrawImage(resistance_images[resistance.name], new Point(2, 2));
                resistance_controls[i].Width = bitmap.Width;
                resistance_controls[i].Height = bitmap.Height;
                resistance_controls[i].Image = bitmap;
                i++;
            }
        }

        public override void LoadForm() {
            this.SuspendForm();
            int horizontal, left, right;
            this.statsButton.Name = creature.GetName().ToLower();
            this.huntButton.Name = creature.GetName().ToLower();
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
            this.healthLabel.Text = (health > 0 ? health.ToString() + " Health" : "Unknown");
            horizontal = 96 - healthLabel.Size.Width;
            left = horizontal / 2;
            right = horizontal - left;
            this.healthLabel.Padding = new Padding(left, 2, right, 2);
            // set exp of creature
            this.expLabel.Text = (experience > 0 ? experience.ToString() : "Unknown") + " Exp";
            horizontal = 96 - expLabel.Size.Width;
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
            this.nameLabel.Text = MainForm.ToTitle(this.creature.name);
            Font f = this.nameLabel.Font;
            while (TextRenderer.MeasureText(this.creature.name, f).Width < this.mainImage.Size.Width && TextRenderer.MeasureText(this.creature.name, f).Height < 26) {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size + 1.0f);
            }
            while (TextRenderer.MeasureText(this.creature.name, f).Width > this.mainImage.Size.Width && f.Size > 1) {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size - 1.0f);
            }

            this.maxDamageLabel.Text = "Max Damage: " + (this.creature.maxdamage >= 0 ? this.creature.maxdamage.ToString() : "-");
            this.abilitiesLabel.Text = RemoveTextInBrackets(this.creature.abilities.Replace(", ", "\n"));
            this.abilitiesLabel.BorderStyle = BorderStyle.FixedSingle;

            string tooltip;
            this.illusionableBox.Image = creature.illusionable ? MainForm.checkmark_yes : MainForm.checkmark_no;
            tooltip = creature.illusionable ? "Creature illusion works for this creature." : "Creature illusion does not work for this creature.";
            resistance_tooltip.SetToolTip(illusionableBox, tooltip);
            resistance_tooltip.SetToolTip(illusionableLabel, tooltip);
            this.summonableBox.Image = creature.summoncost > 0 ? MainForm.checkmark_yes : MainForm.checkmark_no;
            tooltip = creature.summoncost > 0 ? "This creature can be summoned for " + creature.summoncost + " mana." : "This creature cannot be summoned.";
            resistance_tooltip.SetToolTip(summonableBox, tooltip);
            resistance_tooltip.SetToolTip(summonableLabel, tooltip);
            this.invisibleBox.Image = !creature.senseinvis ? MainForm.checkmark_yes : MainForm.checkmark_no;
            tooltip = !creature.senseinvis ? "This creature does not detect invisibility." : "This creature detects invisibility.";
            resistance_tooltip.SetToolTip(invisibleBox, tooltip);
            resistance_tooltip.SetToolTip(invisibleLabel, tooltip);
            this.paralysableBox.Image = creature.paralysable ? MainForm.checkmark_yes : MainForm.checkmark_no;
            tooltip = creature.paralysable ? "This creature can be paralysed." : "This creature cannot be paralysed.";
            resistance_tooltip.SetToolTip(paralysableBox, tooltip);
            resistance_tooltip.SetToolTip(paralysableLabel, tooltip);
            this.pushableBox.Image = creature.pushable ? MainForm.checkmark_yes : MainForm.checkmark_no;
            tooltip = creature.pushable ? "This creature can be pushed." : "This creature cannot be pushed.";
            resistance_tooltip.SetToolTip(pushableBox, tooltip);
            resistance_tooltip.SetToolTip(pushableLabel, tooltip);
            this.pushesBox.Image = creature.pushes ? MainForm.checkmark_yes : MainForm.checkmark_no;
            tooltip = creature.pushes ? "This creature pushes smaller creatures." : "This creature cannot push smaller creatures.";
            resistance_tooltip.SetToolTip(pushesBox, tooltip);
            resistance_tooltip.SetToolTip(pushesLabel, tooltip);

            this.Size = new Size(this.Size.Width, (int)Math.Max(this.abilitiesLabel.Location.Y + this.abilitiesLabel.Size.Height + 10, this.expLabel.Location.Y + this.expLabel.Height + 10));
            this.nameLabel.Font = f;
            this.nameLabel.Left = this.mainImage.Left + (mainImage.Width - this.nameLabel.Size.Width) / 2;
            base.NotificationInitialize();
            base.NotificationFinalize();
            this.ResumeForm();
        }

        public string RemoveTextInBrackets(string str) {
            string ss = "";
            int kk;
            int j = 0;
            bool bracket = false;
            int items = 0;
            for (int i = 0; i < str.Length; i++) {
                if (bracket) {
                    if (str[i] == '-' || int.TryParse(str[i].ToString(), out kk)) {
                        ss = ss + str[i];
                        items++;
                    } else if (str[i] == ')') {
                        if (items == 0) {
                            ss = ss.Substring(0, ss.Length - 1);
                        } else {
                            ss = ss + str[i];
                        }
                        bracket = false;
                    }
                } else {
                    ss = ss + str[i];
                    if (str[i] == '(') {
                        bracket = true;
                        items = 0;
                    }
                }
            }
            return ss;
        }

        private bool clicked = false;
        private void statsButton_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("creature" + MainForm.commandSymbol + (sender as Control).Name);
        }

        private void huntButton_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("hunt" + MainForm.commandSymbol + (sender as Control).Name);
        }
    }
}
