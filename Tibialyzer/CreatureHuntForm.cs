using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tibialyzer
{
    class CreatureHuntForm : NotificationForm
    {
        private TransparentLabel levelLabel;
        private TransparentLabel expLabel;
        private TransparentLabel lootLabel;
        private TransparentLabel cityLabel;
        private TransparentLabel nameLabel;

        public Creature creature;
        public List<HuntingPlace> hunting_places;

        private static Font text_font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        private static Dictionary<int, Color> rating_colors;
    
        public CreatureHuntForm()
        {
            InitializeComponent();
        }

        public static void Initialize()
        {
            rating_colors = new Dictionary<int, Color>();
            rating_colors.Add(1, Color.DarkRed);
            rating_colors.Add(2, Color.Firebrick);
            rating_colors.Add(3, Color.LightBlue);
            rating_colors.Add(4, Color.Green);
            rating_colors.Add(5, Color.Gold);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Cleanup();
                if (creature != null) creature.Dispose();
                if (hunting_places != null)
                {
                    foreach (HuntingPlace h in hunting_places)
                        h.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.nameLabel = new Tibialyzer.TransparentLabel();
            this.levelLabel = new Tibialyzer.TransparentLabel();
            this.expLabel = new Tibialyzer.TransparentLabel();
            this.lootLabel = new Tibialyzer.TransparentLabel();
            this.cityLabel = new Tibialyzer.TransparentLabel();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.nameLabel.Location = new System.Drawing.Point(29, 10);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(39, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name";
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.levelLabel.Location = new System.Drawing.Point(178, 10);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(38, 13);
            this.levelLabel.TabIndex = 3;
            this.levelLabel.Text = "Level";
            // 
            // expLabel
            // 
            this.expLabel.AutoSize = true;
            this.expLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.expLabel.Location = new System.Drawing.Point(222, 10);
            this.expLabel.Name = "expLabel";
            this.expLabel.Size = new System.Drawing.Size(28, 13);
            this.expLabel.TabIndex = 4;
            this.expLabel.Text = "Exp";
            // 
            // lootLabel
            // 
            this.lootLabel.AutoSize = true;
            this.lootLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lootLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.lootLabel.Location = new System.Drawing.Point(256, 10);
            this.lootLabel.Name = "lootLabel";
            this.lootLabel.Size = new System.Drawing.Size(32, 13);
            this.lootLabel.TabIndex = 5;
            this.lootLabel.Text = "Loot";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.cityLabel.Location = new System.Drawing.Point(294, 10);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(28, 13);
            this.cityLabel.TabIndex = 6;
            this.cityLabel.Text = "City";
            // 
            // CreatureHuntForm
            // 
            this.ClientSize = new System.Drawing.Size(388, 165);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.lootLabel);
            this.Controls.Add(this.expLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.nameLabel);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreatureHuntForm";
            this.Load += new System.EventHandler(this.CreatureHuntForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CreatureHuntForm_Load(object sender, EventArgs e)
        {
            this.NotificationInitialize();

            int offset = 0;
            int base_offset = 20;
            int size = 24;
            foreach(HuntingPlace h in hunting_places)
            {
                TransparentPictureBox picture = new TransparentPictureBox();
                picture.Image = h.image;
                picture.Size = new Size(size, size);
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.Location = new Point(nameLabel.Location.X - size, nameLabel.Location.Y + size * offset + base_offset - 4);
                picture.Click += openHuntingPlace;
                picture.Name = h.name.ToString();
                this.Controls.Add(picture);
                

                List<Control> hcontrols = new List<Control>();
                TransparentLabel name = new TransparentLabel();
                name.Text = h.name;
                name.Location = new Point(nameLabel.Location.X, nameLabel.Location.Y + size * offset + base_offset);
                name.ForeColor = MainForm.label_text_color;
                hcontrols.Add(name);

                TransparentLabel level = new TransparentLabel();
                level.Text = h.level == -127 ? "-" : h.level.ToString();
                level.Location = new Point(levelLabel.Location.X, levelLabel.Location.Y + size * offset + base_offset);
                level.ForeColor = Color.SeaGreen;
                hcontrols.Add(level);

                TransparentLabel exp = new TransparentLabel();
                exp.Text = h.exp_quality.ToString(); ;
                exp.Location = new Point(expLabel.Location.X, expLabel.Location.Y + size * offset + base_offset);
                if (rating_colors.ContainsKey(h.exp_quality))
                    exp.ForeColor = rating_colors[h.exp_quality];
                hcontrols.Add(exp);

                TransparentLabel loot = new TransparentLabel();
                loot.Text = h.loot_quality.ToString();
                loot.Location = new Point(lootLabel.Location.X, lootLabel.Location.Y + size * offset + base_offset);
                if (rating_colors.ContainsKey(h.loot_quality))
                    loot.ForeColor = rating_colors[h.loot_quality];
                hcontrols.Add(loot);

                TransparentLabel city = new TransparentLabel();
                city.Text = h.city;
                city.Location = new Point(cityLabel.Location.X, cityLabel.Location.Y + size * offset);
                city.ForeColor = MainForm.label_text_color;
                hcontrols.Add(city);

                foreach (Control c in hcontrols)
                {
                    c.Name = h.name.ToString();
                    c.Size = new Size(1, 1);
                    c.AutoSize = true;
                    c.Font = text_font;
                    c.Click += openHuntingPlace;   
                    this.Controls.Add(c);
                }

                offset++;
            }
            this.Size = new Size(this.Size.Width, this.nameLabel.Location.Y + this.nameLabel.Size.Height + offset * size + base_offset);
        }

        
        private bool clicked = false;
        protected void openHuntingPlace(object sender, EventArgs e)
        {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            string name = (sender as Control).Name;
            MainForm.mainForm.priority_command = "huntingplace@" + name.ToLower();
        }

    }
}
