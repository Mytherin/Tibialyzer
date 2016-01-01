using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tibialyzer {
    public class Target {
        public Image image;
        public int size;
        public Coordinate coordinate;
    }

    public class MapPictureBox : PictureBox {
        public Image mapImage;
        public Coordinate mapCoordinate;
        public int sourceWidth;
        public int zCoordinate;
        public Coordinate beginCoordinate;
        public List<Target> targets;
        public int beginWidth;
        public const int minWidth = 40;
        public const int maxWidth = 400;
        public delegate void MapUpdatedHandler();
        public event MapUpdatedHandler MapUpdated;

        public MapPictureBox() {
            mapImage = null;
            mapCoordinate = null;
            beginCoordinate = null;
            sourceWidth = 0;
            beginWidth = 0;
            targets = new List<Target>();
        }

        protected override void Dispose(bool disposing) {
            if (mapImage != null) {
                mapImage.Dispose();
            }
            base.Dispose(disposing);
        }

        public void UpdateMap() {
            if (beginCoordinate == null) {
                beginCoordinate = new Coordinate(mapCoordinate);
                beginWidth = sourceWidth;
            }
            if (mapCoordinate.z < 0) {
                mapCoordinate.z = 0;
            } else if (mapCoordinate.z >= MainForm.map_files.Count) {
                mapCoordinate.z = MainForm.map_files.Count - 1;
            }

            sourceWidth = Math.Min(Math.Max(sourceWidth, minWidth), maxWidth);
            Rectangle sourceRectangle = new Rectangle(mapCoordinate.x - sourceWidth / 2, mapCoordinate.y - sourceWidth / 2, sourceWidth, sourceWidth);
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                if (mapCoordinate.z == zCoordinate) {
                    gr.DrawImage(mapImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height), sourceRectangle, GraphicsUnit.Pixel);
                } else {
                    gr.DrawImage(MainForm.map_files[mapCoordinate.z].image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), sourceRectangle, GraphicsUnit.Pixel);
                }
                foreach (Target target in targets) {
                    if (target.coordinate.z == mapCoordinate.z) {
                        int x = target.coordinate.x - (mapCoordinate.x - sourceWidth / 2);
                        int y = target.coordinate.y - (mapCoordinate.y - sourceWidth / 2);
                        if (x >= 0 && y >= 0 && x < sourceWidth && y < sourceWidth) {
                            x = (int)((double)x / sourceWidth * bitmap.Width);
                            y = (int)((double)y / sourceWidth * bitmap.Height);
                            int targetWidth = (int)((double)target.size / target.image.Height * target.image.Width);
                            gr.DrawImage(target.image, new Rectangle(x - targetWidth, y - target.size, targetWidth * 2, target.size * 2));
                        }
                    }
                }
            }
            if (this.Image != null) {
                this.Image.Dispose();
            }
            this.Image = bitmap;
            if (MapUpdated != null)
                MapUpdated();
        }

        bool drag_map = false;
        Point center_point;
        Point screen_center;
        Point initial_position;
        protected override void OnMouseUp(MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                if (drag_map) {
                    drag_map = false;
                    System.Windows.Forms.Cursor.Show();
                    System.Windows.Forms.Cursor.Position = initial_position;
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                this.Focus();
                initial_position = System.Windows.Forms.Cursor.Position;
                screen_center = this.PointToScreen(new Point(
                    this.Size.Width / 2,
                    this.Size.Height / 2));
                System.Windows.Forms.Cursor.Position = screen_center;
                center_point = new Point(this.Size.Width / 2, this.Size.Height / 2);
                System.Windows.Forms.Cursor.Hide();
                drag_map = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (drag_map) {
                mapCoordinate.x = (int)Math.Max(Math.Min(mapCoordinate.x + (e.X - center_point.X), Coordinate.MaxWidth), 0);
                mapCoordinate.y = (int)Math.Max(Math.Min(mapCoordinate.y + (e.Y - center_point.Y), Coordinate.MaxHeight), 0);
                this.UpdateMap();
                center_point.X = e.X;
                center_point.Y = e.Y;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            sourceWidth -= e.Delta / 3;
            sourceWidth = Math.Min(Math.Max(sourceWidth, minWidth), maxWidth);
            UpdateMap();
            base.OnMouseWheel(e);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyData == Keys.Subtract || e.KeyData == Keys.OemMinus) {
                mapCoordinate.z--;
                UpdateMap();
            } else if (e.KeyData == Keys.Add || e.KeyData == Keys.Oemplus) {
                mapCoordinate.z++;
                UpdateMap();
            } else if (e.KeyData == Keys.Space) {
                mapCoordinate = new Coordinate(beginCoordinate);
                sourceWidth = beginWidth;
                UpdateMap();
            } else {
                base.OnKeyDown(e);
            }
        }
    }
}
