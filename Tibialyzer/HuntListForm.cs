using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tibialyzer {
    class HuntListForm : NotificationForm {
        private Label levelLabel;
        private Label expLabel;
        private Label lootLabel;
        private Label cityLabel;
        private Label nameLabel;

        public string header = null;
        public List<HuntingPlace> hunting_places;
        public List<Quest> quests;

        public static Font text_font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        private Label headerLabel;
        public static Dictionary<int, Color> rating_colors;

        public int initialPage;
        private int start_index = 0;
        private int max_hunts = 15;

        public HuntListForm() {
            InitializeComponent();
        }

        public new static void Initialize() {
            rating_colors = new Dictionary<int, Color>();
            rating_colors.Add(1, Color.DarkRed);
            rating_colors.Add(2, Color.Firebrick);
            rating_colors.Add(3, Color.FromArgb(22, 125, 190));
            rating_colors.Add(4, Color.Green);
            rating_colors.Add(5, Color.Gold);
            rating_colors.Add(-127, Color.DarkGray);
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HuntListForm));
            this.nameLabel = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.expLabel = new System.Windows.Forms.Label();
            this.lootLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.headerLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
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
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
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
            this.expLabel.BackColor = System.Drawing.Color.Transparent;
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
            this.lootLabel.BackColor = System.Drawing.Color.Transparent;
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
            this.cityLabel.BackColor = System.Drawing.Color.Transparent;
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
            this.headerLabel.BackColor = System.Drawing.Color.Transparent;
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HuntListForm";
            this.Text = "Hunt List";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private int RefreshHuntingPlaces() {
            this.Controls.Clear();
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.lootLabel);
            this.Controls.Add(this.expLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.nameLabel);
            int offset = 0;
            int base_offset = 20;
            int size = 24;
            int current_index = -1;

            int huntCount;
            if (hunting_places != null) {
                huntCount = hunting_places.Count;
                initialCommand = "hunt";
                foreach (HuntingPlace h in hunting_places) {
                    if (++current_index < start_index) continue;
                    if (current_index > start_index + max_hunts) break;
                    PictureBox picture = new PictureBox();
                    picture.Image = h.image;
                    picture.Size = new Size(size, size);
                    picture.SizeMode = PictureBoxSizeMode.Zoom;
                    picture.Location = new Point(nameLabel.Location.X - size, this.nameLabel.Location.Y + size * offset + base_offset - 4);
                    picture.Click += openHuntingPlace;
                    picture.Name = h.name.ToString();
                    picture.BackColor = Color.Transparent;
                    this.Controls.Add(picture);

                    Label name = new Label();
                    name.Text = h.name;
                    name.Location = new Point(nameLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    name.ForeColor = MainForm.label_text_color;
                    name.Size = new Size(this.levelLabel.Location.X - this.nameLabel.Location.X, 20);
                    name.Font = text_font;
                    name.Click += openHuntingPlace;
                    name.Name = h.name.ToString();
                    name.BackColor = Color.Transparent;
                    this.Controls.Add(name);

                    List<Control> hcontrols = new List<Control>();
                    Label level = new Label();
                    level.Text = h.level == -127 ? "-" : h.level.ToString();
                    level.Location = new Point(levelLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    level.ForeColor = Color.SeaGreen;
                    level.BackColor = Color.Transparent;
                    hcontrols.Add(level);

                    PictureBox exp = new PictureBox();
                    exp.Image = MainForm.star_image_text[h.exp_quality < 0 ? 5 : h.exp_quality - 1];
                    exp.BackColor = Color.Transparent;
                    exp.Location = new Point(expLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    exp.Size = new Size(24, 16);
                    exp.SizeMode = PictureBoxSizeMode.StretchImage;
                    exp.Name = h.name.ToString();
                    exp.Click += openHuntingPlace;
                    this.Controls.Add(exp);

                    PictureBox loot = new PictureBox();
                    loot.Image = MainForm.star_image_text[h.loot_quality < 0 ? 5 : h.loot_quality - 1];
                    loot.BackColor = Color.Transparent;
                    loot.Location = new Point(lootLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    loot.Size = new Size(24, 16);
                    loot.SizeMode = PictureBoxSizeMode.StretchImage;
                    loot.Name = h.name.ToString();
                    loot.Click += openHuntingPlace;
                    this.Controls.Add(loot);

                    Label city = new Label();
                    city.Text = h.city;
                    city.Location = new Point(cityLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    city.ForeColor = MainForm.label_text_color;
                    city.BackColor = Color.Transparent;
                    hcontrols.Add(city);

                    foreach (Control c in hcontrols) {
                        c.Name = h.name.ToString();
                        c.Size = new Size(1, 1);
                        c.AutoSize = true;
                        c.Font = text_font;
                        c.Click += openHuntingPlace;
                        this.Controls.Add(c);
                    }

                    offset++;
                }
            } else {
                huntCount = quests.Count;
                initialCommand = "quest";
                //expLabel.Visible = false;
                lootLabel.Visible = false;
                expLabel.Text = "Premium";
                foreach (Quest q in quests) {
                    if (++current_index < start_index) continue;
                    if (current_index > start_index + max_hunts) break;
                    Image image = q.GetImage();
                    

                    if (image != null) {
                        PictureBox picture = new PictureBox();
                        picture.Image = image;
                        picture.Size = new Size(size, size);
                        picture.SizeMode = PictureBoxSizeMode.Zoom;
                        picture.Location = new Point(nameLabel.Location.X - size, this.nameLabel.Location.Y + size * offset + base_offset - 4);
                        picture.Click += openHuntingPlace;
                        picture.Name = q.name.ToString();
                        picture.BackColor = Color.Transparent;
                        this.Controls.Add(picture);
                    }
                    
                    Label name = new Label();
                    name.Text = q.name;
                    name.Location = new Point(nameLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    name.ForeColor = MainForm.label_text_color;
                    name.Size = new Size(this.levelLabel.Location.X - this.nameLabel.Location.X, 20);
                    name.Font = text_font;
                    name.Click += openHuntingPlace;
                    name.Name = q.name.ToString();
                    name.BackColor = Color.Transparent;
                    this.Controls.Add(name);
                    
                    Label level = new Label();
                    level.Text = q.minlevel == -127 ? "-" : q.minlevel.ToString();
                    level.Location = new Point(levelLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    level.ForeColor = Color.SeaGreen;
                    level.BackColor = Color.Transparent;
                    level.Name = q.name.ToString();
                    level.Size = new Size(1, 1);
                    level.AutoSize = true;
                    level.Font = text_font;
                    level.Click += openHuntingPlace;
                    this.Controls.Add(level);

                    Label premium = new Label();
                    premium.Text = q.premium ? "Yes" : "No";
                    premium.Location = new Point(expLabel.Location.X, this.expLabel.Location.Y + size * offset + base_offset);
                    premium.ForeColor = q.premium ? Color.SeaGreen : Color.RoyalBlue;
                    premium.BackColor = Color.Transparent;
                    premium.Name = q.name.ToString();
                    premium.Size = new Size(1, 1);
                    premium.AutoSize = true;
                    premium.Font = text_font;
                    premium.Click += openHuntingPlace;
                    this.Controls.Add(premium);

                    Label city = new Label();
                    city.Text = MainForm.ToTitle(q.city);
                    city.Location = new Point(cityLabel.Location.X, this.nameLabel.Location.Y + size * offset + base_offset);
                    city.ForeColor = MainForm.label_text_color;
                    city.BackColor = Color.Transparent;
                    city.Name = q.name.ToString();
                    city.Size = new Size(1, 1);
                    city.AutoSize = true;
                    city.Font = text_font;
                    city.Click += openHuntingPlace;
                    this.Controls.Add(city);


                    offset++;
                }
            }
            int total_yoffset = this.nameLabel.Location.Y + this.nameLabel.Size.Height + offset * size + base_offset;
            // if there are too many hunts to be displayed on one page, add 'prev' and 'next' buttons
            if (huntCount > max_hunts) {
                if (start_index > 0) {
                    PictureBox prevpage = new PictureBox();
                    prevpage.Location = new Point(10, total_yoffset);
                    prevpage.Size = new Size(97, 23);
                    prevpage.Image = MainForm.prevpage_image;
                    prevpage.BackColor = Color.Transparent;
                    prevpage.SizeMode = PictureBoxSizeMode.StretchImage;
                    prevpage.Click += prevpage_Click;
                    this.Controls.Add(prevpage);
                }
                if (start_index + max_hunts < huntCount) {
                    PictureBox nextpage = new PictureBox();
                    nextpage.Location = new Point(this.Size.Width - 108, total_yoffset);
                    nextpage.Size = new Size(98, 23);
                    nextpage.BackColor = Color.Transparent;
                    nextpage.Image = MainForm.nextpage_image;
                    nextpage.SizeMode = PictureBoxSizeMode.StretchImage;
                    nextpage.Click += nextpage_Click;
                    this.Controls.Add(nextpage);
                }

                total_yoffset += 23 + 10;
            }
            return total_yoffset;
        }

        public override void LoadForm() {
            this.SuspendForm();
            this.NotificationInitialize();
            
            if (header != null) {
                headerLabel.Text = header;
            }
            int huntCount = hunting_places != null ? hunting_places.Count : quests.Count;
            start_index = Math.Min(Math.Max(initialPage * max_hunts, 0), (huntCount / max_hunts) * max_hunts);

            int total_yoffset = RefreshHuntingPlaces();

            this.Size = new Size(this.Size.Width, total_yoffset);
            base.NotificationFinalize();
            this.ResumeForm();
        }

        void updateCommand() {
            string[] split = command.command.Split(MainForm.commandSymbol);
            command.command = split[0] + MainForm.commandSymbol + split[1] + MainForm.commandSymbol + (start_index / max_hunts);
        }

        void prevpage_Click(object sender, EventArgs e) {
            start_index = Math.Max(start_index - max_hunts, 0);
            updateCommand();
            this.SuspendForm();
            int total_yoffset = RefreshHuntingPlaces();
            this.Size = new Size(this.Size.Width, total_yoffset);
            this.ResumeForm();
            base.ResetTimer();
        }

        void nextpage_Click(object sender, EventArgs e) {
            start_index += max_hunts;
            updateCommand();
            this.SuspendForm();
            int total_yoffset = RefreshHuntingPlaces();
            this.Size = new Size(this.Size.Width, total_yoffset);
            this.ResumeForm();
            base.ResetTimer();
        }
        
        private string initialCommand = "hunt";
        private bool clicked = false;
        protected void openHuntingPlace(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            string name = (sender as Control).Name;
            MainForm.mainForm.ExecuteCommand(initialCommand + MainForm.commandSymbol + name.ToLower());
        }

    }
}
