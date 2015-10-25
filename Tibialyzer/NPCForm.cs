using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Tibialyzer
{
    class NPCForm : NotificationForm
    {
        public NPC npc = null;
        private Bitmap map_image = null;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private Coordinate map_coordinates;

        public NPCForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.mapBox = new System.Windows.Forms.PictureBox();
            this.npcImage = new Tibialyzer.TransparentPictureBox();
            this.creatureName = new Tibialyzer.TransparentLabel();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // mapBox
            // 
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapBox.Location = new System.Drawing.Point(121, 12);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(195, 190);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            // 
            // npcImage
            // 
            this.npcImage.BackColor = System.Drawing.Color.Transparent;
            this.npcImage.Location = new System.Drawing.Point(12, 45);
            this.npcImage.Name = "npcImage";
            this.npcImage.Size = new System.Drawing.Size(100, 98);
            this.npcImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.npcImage.TabIndex = 1;
            this.npcImage.TabStop = false;
            // 
            // creatureName
            // 
            this.creatureName.AutoSize = true;
            this.creatureName.BackColor = System.Drawing.Color.Transparent;
            this.creatureName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureName.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.creatureName.Location = new System.Drawing.Point(34, 146);
            this.creatureName.Name = "creatureName";
            this.creatureName.Size = new System.Drawing.Size(57, 16);
            this.creatureName.TabIndex = 2;
            this.creatureName.Text = "Rashid";
            // 
            // mapUpLevel
            // 
            this.mapUpLevel.Location = new System.Drawing.Point(121, 13);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 3;
            this.mapUpLevel.TabStop = false;
            // 
            // mapDownLevel
            // 
            this.mapDownLevel.Location = new System.Drawing.Point(121, 34);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 4;
            this.mapDownLevel.TabStop = false;
            // 
            // NPCForm
            // 
            this.ClientSize = new System.Drawing.Size(328, 259);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.creatureName);
            this.Controls.Add(this.npcImage);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NPCForm";
            this.Load += new System.EventHandler(this.NPCForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox mapBox;
        private TransparentPictureBox npcImage;
        private TransparentLabel creatureName;

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Cleanup();
                if (npc != null) npc.Dispose();
                if (map_image != null) map_image.Dispose();
            }
            base.Dispose(disposing);
        }

        private void NPCForm_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            NotificationInitialize();
            if (npc == null) return;
            npcImage.Image = npc.image;
            creatureName.Text = npc.city;

            map_coordinates = new Coordinate(npc.pos);
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
            base.NotificationFinalize();
            this.ResumeLayout(false);
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

            if (npc.pos.z == this.map_coordinates.z)
            {
                point = new Point((int)(npc.pos.x * (7f / 8f) * big_map.Width), (int)(npc.pos.y * big_map.Height));
                //int width = (int)(20 / this.current_zoom);
                int width = 20;
                Rectangle cross_rectangle = new Rectangle(point.X - width / 2, point.Y - width / 2, width, width);
                if (sourceRectangle.IntersectsWith(cross_rectangle))
                {
                    int x = (int)(mapBox.Width * ((float)point.X - sourceRectangle.X) / sourceRectangle.Width);
                    int y = (int)(mapBox.Height * ((float)point.Y - sourceRectangle.Y) / sourceRectangle.Height);
                    gr.DrawImage(MainForm.cross_image, new Rectangle(x - width / 2, y - width / 2, width, width));
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
