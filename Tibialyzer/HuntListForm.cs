using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tibialyzer
{
    class HuntListForm : NotificationForm
    {
        private TransparentLabel levelLabel;
        private TransparentLabel expLabel;
        private TransparentLabel lootLabel;
        private TransparentLabel cityLabel;
        private TransparentLabel nameLabel;

        public string header = null;
        public List<HuntingPlace> hunting_places;

        private static Font text_font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        private TransparentLabel headerLabel;
        private static Dictionary<int, Color> rating_colors;
    
        public HuntListForm()
        {
            InitializeComponent();
        }

        public new static void Initialize()
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
            this.headerLabel = new Tibialyzer.TransparentLabel();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.nameLabel.Location = new System.Drawing.Point(29, 30);
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
            this.levelLabel.Location = new System.Drawing.Point(178, 30);
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
            this.expLabel.Location = new System.Drawing.Point(222, 30);
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
            this.lootLabel.Location = new System.Drawing.Point(256, 30);
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
            this.cityLabel.Location = new System.Drawing.Point(294, 30);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(28, 13);
            this.cityLabel.TabIndex = 6;
            this.cityLabel.Text = "City";
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.headerLabel.Location = new System.Drawing.Point(87, 10);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(60, 16);
            this.headerLabel.TabIndex = 7;
            this.headerLabel.Text = "Header";
            // 
            // HuntListForm
            // 
            this.ClientSize = new System.Drawing.Size(378, 165);
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.lootLabel);
            this.Controls.Add(this.expLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.nameLabel);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HuntListForm";
            this.Load += new System.EventHandler(this.CreatureHuntForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CreatureHuntForm_Load(object sender, EventArgs e)
        {
            this.NotificationInitialize();

            if (header != null)
            {
                headerLabel.Text = header;
            }

            this.SuspendLayout();
            int offset = 0;
            int base_offset = 20;
            int size = 24;
            foreach(HuntingPlace h in hunting_places)
            {
                TransparentPictureBox picture = new TransparentPictureBox();
                picture.Image = h.image;
                picture.Size = new Size(size, size);
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.Location = new Point(nameLabel.Location.X - size, this.nameLabel.Location.Y + size * offset + base_offset - 4);
                picture.Click += openHuntingPlace;
                picture.Name = h.name.ToString();
                this.Controls.Add(picture);


                TransparentLabel name = new TransparentLabel();
                name.Text = h.name;
                name.Location = new Point(nameLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                name.ForeColor = MainForm.label_text_color;
                name.Size = new Size(this.levelLabel.Location.X - this.nameLabel.Location.X, 20);
                name.Font = text_font;
                name.Click += openHuntingPlace;
                name.Name = h.name.ToString();
                this.Controls.Add(name);

                List<Control> hcontrols = new List<Control>();
                TransparentLabel level = new TransparentLabel();
                level.Text = h.level == -127 ? "-" : h.level.ToString();
                level.Location = new Point(levelLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                level.ForeColor = Color.SeaGreen;
                hcontrols.Add(level);

                TransparentLabel exp = new TransparentLabel();
                exp.Text = h.exp_quality.ToString(); ;
                exp.Location = new Point(expLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                if (rating_colors.ContainsKey(h.exp_quality))
                    exp.ForeColor = rating_colors[h.exp_quality];
                hcontrols.Add(exp);

                TransparentLabel loot = new TransparentLabel();
                loot.Text = h.loot_quality.ToString();
                loot.Location = new Point(lootLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                if (rating_colors.ContainsKey(h.loot_quality))
                    loot.ForeColor = rating_colors[h.loot_quality];
                hcontrols.Add(loot);

                TransparentLabel city = new TransparentLabel();
                city.Text = h.city;
                city.Location = new Point(cityLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
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
            this.ResumeLayout(false);
        }

        
        private bool clicked = false;
        protected void openHuntingPlace(object sender, EventArgs e)
        {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            string name = (sender as Control).Name;
            MainForm.mainForm.priority_command = "hunt@" + name.ToLower();
        }

    }
}
