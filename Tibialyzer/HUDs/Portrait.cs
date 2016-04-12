using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class Portrait : BaseHUD {
        PictureBox pictureBox;
        Image portrait;

        public Portrait() {
            InitializeComponent();

            BackColor = StyleManager.TransparencyKey;
            TransparencyKey = StyleManager.TransparencyKey;
        }

        public override void LoadHUD() {
            pictureBox = new PictureBox();
            pictureBox.Location = new Point(0, 0);
            pictureBox.Size = new Size(this.Width, this.Height);
            pictureBox.BackColor = StyleManager.TransparencyKey;
            this.Controls.Add(pictureBox);

            portrait = StyleManager.GetImage("defaultportrait-blue.png");

            RefreshHUD(1, 1);

            this.Load += Portrait_Load;
        }

        private System.Timers.Timer timer;
        private void Portrait_Load(object sender, EventArgs e) {
            timer = new System.Timers.Timer(10);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            RefreshStats();
        }

        private void RefreshHUD(double lifePercentage, double manaPercentage) {
            lifePercentage = lifePercentage.ClampPercentage();
            manaPercentage = manaPercentage.ClampPercentage();
            Bitmap bitmap = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);

            int width = bitmap.Width;
            int height = bitmap.Height;

            using (Graphics gr = Graphics.FromImage(bitmap)) {
                // we set the interpolation mode to nearest neighbor because other interpolation modes modify the colors
                // which causes the transparency key to appear around the edges of the object
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                gr.Clear(StyleManager.TransparencyKey);
                Rectangle lifeRectangle = new Rectangle();
                lifeRectangle.X = (int)(height * 0.7);
                lifeRectangle.Y = (int)(height * 0.15) - 2;
                lifeRectangle.Width = width - lifeRectangle.X;
                lifeRectangle.Height = (int)(height * 0.2) + 4;

                gr.FillRectangle(Brushes.Black, lifeRectangle);
                lifeRectangle.Y += 2;
                lifeRectangle.Height -= 4;
                lifeRectangle.Width = (int)(lifeRectangle.Width * lifePercentage) - 2;
                using (Brush brush = new SolidBrush(StyleManager.GetHealthColor(lifePercentage))) {
                    gr.FillRectangle(brush, lifeRectangle);
                }

                Rectangle manaRectangle = new Rectangle();
                manaRectangle.X = (int)(height * 0.7);
                manaRectangle.Y = (int)(height * 0.15) - 2 + lifeRectangle.Height + 2;
                manaRectangle.Width = (int)((width - lifeRectangle.X) * 0.95);
                manaRectangle.Height = (int)(height * 0.2) + 4;

                gr.FillRectangle(Brushes.Black, manaRectangle);
                manaRectangle.Y += 2;
                manaRectangle.Height -= 4;
                manaRectangle.Width = (int)(manaRectangle.Width * manaPercentage) - 2;
                using (Brush brush = new SolidBrush(StyleManager.ManaColor)) {
                    gr.FillRectangle(brush, manaRectangle);
                }

                SummaryForm.RenderImageResized(gr, portrait, new Rectangle(0, 0, height, height));

                Rectangle levelRect = new Rectangle((int)(height * 0.7), (int)(height * 0.6), (int)(height * 0.35), (int)(height * 0.35));
                using (Brush brush = new SolidBrush(StyleManager.MainFormButtonColor)) {
                    gr.FillEllipse(brush, levelRect);
                }
                using (Pen pen = new Pen(StyleManager.MainFormButtonForeColor, 2)) {
                    gr.DrawEllipse(pen, levelRect);
                }
                using (Brush brush = new SolidBrush(StyleManager.MainFormButtonForeColor)) {
                    string level = MemoryReader.Level.ToString();
                    gr.DrawString(level, StyleManager.MainFormLabelFont, brush, new PointF(height * (0.725f + (3 - level.Length) * 0.0375f), height * 0.7f));
                }
            }
            bitmap.MakeTransparent(StyleManager.TransparencyKey);
            Image oldImage = pictureBox.Image;
            pictureBox.Image = bitmap;
            if (oldImage != null) {
                lock (oldImage) {
                    oldImage.Dispose();
                }
            }
        }

        private void RefreshStats() {
            timer.Enabled = false;
            long life = 0, maxlife = 1, mana = 0, maxmana = 1;
            life = MemoryReader.Health;
            maxlife = MemoryReader.MaxHealth;
            mana = MemoryReader.Mana;
            maxmana = MemoryReader.MaxMana;
            double lifePercentage = (double)life / maxlife;
            double manaPercentage = (double)mana / maxmana;

            try {
                bool visible = ProcessManager.IsTibiaActive();
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD(lifePercentage, manaPercentage);
                    this.Visible = visible;
                });
                timer.Enabled = true;
            } catch {
                if (timer != null) {
                    timer.Dispose();
                    timer = null;
                }
            }
        }


        public override string GetHUD() {
            return "Portrait";
        }
    }
}
