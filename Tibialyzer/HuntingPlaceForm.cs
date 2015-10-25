using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer
{
    class HuntingPlaceForm : NotificationForm
    {
        public HuntingPlace hunting_place = null;
        private Coordinate target = null;
        private Coordinate map_coordinates;
        private TransparentLabel huntingPlaceName;
        private TransparentLabel creatureLabel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private TransparentLabel experienceLabel;
        private TransparentLabel label1;
        private TransparentPictureBox experienceStarBox;
        private TransparentPictureBox lootStarBox;
        private TransparentLabel levelLabel;
        private TransparentLabel cityLabel;
        private TransparentLabel requirementLabel;
        private Bitmap map_image = null;
        private static Font requirement_font = new Font(FontFamily.GenericSansSerif, 7.5f, FontStyle.Bold);

        public HuntingPlaceForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.requirementLabel = new Tibialyzer.TransparentLabel();
            this.cityLabel = new Tibialyzer.TransparentLabel();
            this.levelLabel = new Tibialyzer.TransparentLabel();
            this.lootStarBox = new Tibialyzer.TransparentPictureBox();
            this.experienceStarBox = new Tibialyzer.TransparentPictureBox();
            this.label1 = new Tibialyzer.TransparentLabel();
            this.experienceLabel = new Tibialyzer.TransparentLabel();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.creatureLabel = new Tibialyzer.TransparentLabel();
            this.huntingPlaceName = new Tibialyzer.TransparentLabel();
            this.mapBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.lootStarBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.experienceStarBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            this.SuspendLayout();
            // 
            // requirementLabel
            // 
            this.requirementLabel.AutoSize = true;
            this.requirementLabel.BackColor = System.Drawing.Color.Transparent;
            this.requirementLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requirementLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.requirementLabel.Location = new System.Drawing.Point(37, 111);
            this.requirementLabel.Name = "requirementLabel";
            this.requirementLabel.Size = new System.Drawing.Size(104, 16);
            this.requirementLabel.TabIndex = 13;
            this.requirementLabel.Text = "Requirements";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.BackColor = System.Drawing.Color.Transparent;
            this.cityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cityLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.cityLabel.Location = new System.Drawing.Point(7, 48);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(34, 16);
            this.cityLabel.TabIndex = 12;
            this.cityLabel.Text = "City";
            this.cityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // levelLabel
            // 
            this.levelLabel.BackColor = System.Drawing.Color.Transparent;
            this.levelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.levelLabel.Location = new System.Drawing.Point(111, 48);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(54, 16);
            this.levelLabel.TabIndex = 11;
            this.levelLabel.Text = "Level: ";
            this.levelLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lootStarBox
            // 
            this.lootStarBox.Location = new System.Drawing.Point(100, 86);
            this.lootStarBox.Name = "lootStarBox";
            this.lootStarBox.Size = new System.Drawing.Size(61, 20);
            this.lootStarBox.TabIndex = 10;
            this.lootStarBox.TabStop = false;
            // 
            // experienceStarBox
            // 
            this.experienceStarBox.Location = new System.Drawing.Point(8, 86);
            this.experienceStarBox.Name = "experienceStarBox";
            this.experienceStarBox.Size = new System.Drawing.Size(61, 20);
            this.experienceStarBox.TabIndex = 9;
            this.experienceStarBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label1.Location = new System.Drawing.Point(111, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Loot";
            // 
            // experienceLabel
            // 
            this.experienceLabel.AutoSize = true;
            this.experienceLabel.BackColor = System.Drawing.Color.Transparent;
            this.experienceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.experienceLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.experienceLabel.Location = new System.Drawing.Point(23, 67);
            this.experienceLabel.Name = "experienceLabel";
            this.experienceLabel.Size = new System.Drawing.Size(34, 16);
            this.experienceLabel.TabIndex = 7;
            this.experienceLabel.Text = "Exp";
            // 
            // mapDownLevel
            // 
            this.mapDownLevel.Location = new System.Drawing.Point(170, 33);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 6;
            this.mapDownLevel.TabStop = false;
            // 
            // mapUpLevel
            // 
            this.mapUpLevel.Location = new System.Drawing.Point(170, 12);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 5;
            this.mapUpLevel.TabStop = false;
            // 
            // creatureLabel
            // 
            this.creatureLabel.AutoSize = true;
            this.creatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.creatureLabel.Location = new System.Drawing.Point(39, 211);
            this.creatureLabel.Name = "creatureLabel";
            this.creatureLabel.Size = new System.Drawing.Size(75, 16);
            this.creatureLabel.TabIndex = 4;
            this.creatureLabel.Text = "Creatures";
            // 
            // huntingPlaceName
            // 
            this.huntingPlaceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntingPlaceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.huntingPlaceName.Location = new System.Drawing.Point(6, 14);
            this.huntingPlaceName.Name = "huntingPlaceName";
            this.huntingPlaceName.Size = new System.Drawing.Size(158, 34);
            this.huntingPlaceName.TabIndex = 3;
            this.huntingPlaceName.Text = "Brimstone Cave";
            this.huntingPlaceName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // mapBox
            // 
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapBox.Location = new System.Drawing.Point(170, 12);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(195, 190);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            // 
            // HuntingPlaceForm
            // 
            this.ClientSize = new System.Drawing.Size(382, 273);
            this.Controls.Add(this.requirementLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.lootStarBox);
            this.Controls.Add(this.experienceStarBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.experienceLabel);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.creatureLabel);
            this.Controls.Add(this.huntingPlaceName);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HuntingPlaceForm";
            this.Load += new System.EventHandler(this.HuntingPlaceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lootStarBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.experienceStarBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox mapBox;

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Cleanup();
                if (hunting_place != null) hunting_place.Dispose();
                if (map_image != null) map_image.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HuntingPlaceForm_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            NotificationInitialize();
            if (hunting_place == null) return;
            this.cityLabel.Text = hunting_place.city;
            this.huntingPlaceName.Text = MainForm.ToTitle(hunting_place.name);
            this.levelLabel.Text = hunting_place.level.ToString();

            ToolTip tooltip = new ToolTip();
            tooltip.AutoPopDelay = 60000;
            tooltip.InitialDelay = 500;
            tooltip.ReshowDelay = 0;
            tooltip.ShowAlways = true;
            tooltip.UseFading = true;

            if (this.hunting_place.coordinates != null && this.hunting_place.coordinates.Count > 0)
            {
                int count = 1;
                foreach (Coordinate coordinate in this.hunting_place.coordinates)
                {
                    TransparentLabel label = new TransparentLabel();
                    label.ForeColor = MainForm.label_text_color;
                    label.Name = (count - 1).ToString();
                    label.Font = LootDropForm.loot_font;
                    label.Text = count.ToString();
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Size = new Size(1, 1);
                    label.AutoSize = true;
                    label.Location = new Point(mapBox.Location.X + (count - 1) * 25, mapBox.Location.Y + mapBox.Size.Height + 5);
                    label.Click += label_Click;
                    this.Controls.Add(label);
                    count++;
                }
                int offset = (count - 1) * 25;
                int dircount = 1;
                foreach (Directions dir in this.hunting_place.directions)
                {
                    TransparentLabel label = new TransparentLabel();
                    label.ForeColor = MainForm.label_text_color;
                    label.Name = (dircount - 1).ToString();
                    label.Font = requirement_font;
                    label.Text = dir.name;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Location = new Point(mapBox.Location.X + offset, mapBox.Location.Y + mapBox.Size.Height + 5);
                    label.Click += direction_Click;
                    this.Controls.Add(label);
                    int width = TextRenderer.MeasureText(label.Text, label.Font).Width + 5;
                    label.Size = new Size(width, 26);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    tooltip.SetToolTip(label, dir.notes);
                    offset += TextRenderer.MeasureText(label.Text, label.Font).Width + 5;
                    dircount++;
                }
                target = new Coordinate(this.hunting_place.coordinates[0]);
                map_coordinates = new Coordinate(this.hunting_place.coordinates[0]);
            }
            else
            {
                map_coordinates = new Coordinate(0.5f, 0.5f, 7);
            }

            if (hunting_place.requirements != null && hunting_place.requirements.Count > 0)
            {
                int count = 0;
                foreach(Requirements requirement in hunting_place.requirements)
                {
                    TransparentLabel label = new TransparentLabel();
                    label.ForeColor = Color.Firebrick;
                    label.Font = requirement_font;
                    label.Text = "- " + requirement.notes;
                    label.Location = new Point(3, requirementLabel.Location.Y + requirementLabel.Size.Height + count * 20 + 3);
                    label.AutoSize = true;
                    label.MaximumSize = new Size(170, 0);
                    label.Name = requirement.questid.ToString();
                    this.Controls.Add(label);
                    count++;
                }
            }
            else
            {
                this.requirementLabel.Hide();
            }

            int base_y = this.creatureLabel.Location.Y + this.creatureLabel.Height + 5;
            int y = MainForm.DisplayCreatureList(this.Controls, (hunting_place.creatures as IEnumerable<TibiaObject>).ToList(), 10, base_y, this.Size.Width, 4, true, null, 0.8f);
            foreach(Control c in this.Controls)
            {
                if (c is TransparentPictureBox)
                {
                    c.Click += openCreatureMenu;
                }
            }

            Font f = this.huntingPlaceName.Font;
            while (TextRenderer.MeasureText(this.huntingPlaceName.Text, f).Width < this.huntingPlaceName.Size.Width && TextRenderer.MeasureText(this.huntingPlaceName.Text, f).Height < 26)
            {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size + 1.0f);
            }
            while (TextRenderer.MeasureText(this.huntingPlaceName.Text, f).Width > this.huntingPlaceName.Size.Width && f.Size > 1)
            {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size - 1.0f);
            }


            Bitmap bitmap = new Bitmap(experienceStarBox.Size.Width, experienceStarBox.Size.Height);
            Graphics gr = Graphics.FromImage(bitmap);
            for(int i = 0; i < Math.Min(this.hunting_place.exp_quality, 5); i++)
            {
                gr.DrawImage(MainForm.star_image, new Rectangle(i * experienceStarBox.Size.Width / 5, 0, experienceStarBox.Size.Width / 5, experienceStarBox.Size.Width / 5));
            }
            experienceStarBox.Image = bitmap;

            bitmap = new Bitmap(lootStarBox.Size.Width, lootStarBox.Size.Height);
            gr = Graphics.FromImage(bitmap);
            for (int i = 0; i < Math.Min(this.hunting_place.loot_quality, 5); i++)
            {
                gr.DrawImage(MainForm.star_image, new Rectangle(i * lootStarBox.Size.Width / 5, 0, lootStarBox.Size.Width / 5, lootStarBox.Size.Width / 5));
            }
            lootStarBox.Image = bitmap;

            this.huntingPlaceName.Font = f;
            this.Size = new Size(this.Size.Width, base_y + y + 10);
            UpdateMap();

            mapBox.Click -= c_Click;
            mapBox.MouseDown += mapBox_MouseDown;
            mapBox.MouseUp += mapBox_MouseUp;
            mapBox.MouseMove += mapBox_MouseMove;
            mapBox.MouseWheel += mapBox_MouseWheel;

            this.mapUpLevel.Image = MainForm.mapup_image;
            this.mapUpLevel.Click -= c_Click;
            this.mapUpLevel.Click += mapUpLevel_Click;
            this.mapDownLevel.Image = MainForm.mapdown_image;
            this.mapDownLevel.Click -= c_Click;
            this.mapDownLevel.Click += mapDownLevel_Click;
            this.ResumeLayout(false);
        }


        private bool clicked = false;
        protected void openCreatureMenu(object sender, EventArgs e)
        {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            string name = (sender as Control).Name;
            MainForm.mainForm.priority_command = "creature@" + name.ToLower();
        }

        void label_Click(object sender, EventArgs e)
        {
            Coordinate new_coordinate = hunting_place.coordinates[int.Parse((sender as Control).Name)];
            this.target = new Coordinate(new_coordinate);
            this.map_coordinates = new Coordinate(new_coordinate);
            UpdateMap();
        }

        void direction_Click(object sender, EventArgs e)
        {
            Directions direction = hunting_place.directions[int.Parse((sender as Control).Name)];
            this.target = new Coordinate(direction.x, direction.y, direction.z);
            this.map_coordinates = new Coordinate(direction.x, direction.y, direction.z);
            UpdateMap();
        }

        float current_zoom = 1.0f;
        private void UpdateMap()
        {
            if (map_image != null) map_image.Dispose();
            map_image = new Bitmap(mapBox.Width, mapBox.Height);
            Graphics gr = Graphics.FromImage(map_image);

            Image big_map = MainForm.map_files[map_coordinates.z].image;
            Point point = new Point((int)(map_coordinates.x * (7f / 8f) * big_map.Width), (int)(map_coordinates.y * big_map.Height));
            Rectangle sourceRectangle = new Rectangle(
                point.X - (int)(mapBox.Width / 2 * current_zoom),
                point.Y - (int)(mapBox.Height / 2 * current_zoom),
                (int)(mapBox.Width * current_zoom),
                (int)(mapBox.Height * current_zoom));

            gr.DrawImage(big_map, new Rectangle(0, 0, mapBox.Width, mapBox.Height), sourceRectangle, GraphicsUnit.Pixel);

            if (target != null)
            {
                if (target.z == this.map_coordinates.z)
                {
                    point = new Point((int)(target.x * (7f / 8f) * big_map.Width), (int)(target.y * big_map.Height));
                    int width = (int)(20 / this.current_zoom);
                    Rectangle cross_rectangle = new Rectangle(point.X - width / 2, point.Y - width / 2, width, width);
                    if (sourceRectangle.IntersectsWith(cross_rectangle))
                    {
                        int x = (int)(mapBox.Width * ((float)point.X - sourceRectangle.X) / sourceRectangle.Width);
                        int y = (int)(mapBox.Height * ((float)point.Y - sourceRectangle.Y) / sourceRectangle.Height);
                        gr.DrawImage(MainForm.cross_image, new Rectangle(x - width / 2, y - width / 2, width, width));
                    }
                }

            }

            mapBox.Image = map_image;
        }

        void mapUpLevel_Click(object sender, EventArgs e)
        {
            this.map_coordinates.z -= 1;
            if (this.map_coordinates.z < 0) this.map_coordinates.z = 0;
            UpdateMap();
            base.ResetTimer();
        }

        void mapDownLevel_Click(object sender, EventArgs e)
        {
            this.map_coordinates.z += 1;
            if (this.map_coordinates.z >= MainForm.map_files.Count) this.map_coordinates.z = MainForm.map_files.Count - 1;
            UpdateMap();
            base.ResetTimer();
        }


        bool drag_map = false;
        Point center_point;
        Point screen_center;
        void mapBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (drag_map)
                {
                    drag_map = false;
                    System.Windows.Forms.Cursor.Show();
                    base.ResetTimer();
                    System.Windows.Forms.Cursor.Position = screen_center;
                }
            }
        }

        void mapBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mapBox.Focus();
                screen_center = this.PointToScreen(new Point(
                    mapBox.Location.X + mapBox.Size.Width / 2,
                    mapBox.Location.Y + mapBox.Size.Height / 2));
                System.Windows.Forms.Cursor.Position = screen_center;
                center_point = new Point(mapBox.Size.Width / 2, mapBox.Size.Height / 2);
                System.Windows.Forms.Cursor.Hide();
                drag_map = true;
                base.ResetTimer();
            }

        }

        bool disable_move = false;
        float mouse_factor = 0.0005f;
        void mapBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (disable_move) return;
            if (drag_map)
            {
                map_coordinates.x = Math.Max(Math.Min(map_coordinates.x + mouse_factor * (e.X - center_point.X), 1), 0);
                map_coordinates.y = Math.Max(Math.Min(map_coordinates.y + mouse_factor * (e.Y - center_point.Y), 1), 0);
                UpdateMap();
                center_point.X = e.X;
                center_point.Y = e.Y;
                base.ResetTimer();
            }
        }

        void mapBox_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            current_zoom -= 0.0005f * e.Delta;
            current_zoom = Math.Min(Math.Max(current_zoom, 0.2f), 2.15f);
            UpdateMap();
            base.ResetTimer();
        }
    }
}
