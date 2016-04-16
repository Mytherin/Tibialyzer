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
        private Image backgroundImage, centerImage;
        private Point backgroundOffset, centerOffset;
        private int backgroundScale, centerScale;

        private const int WS_EX_Transparent = 0x20;
        private const int WS_EX_Layered = 0x80000;
        private const int WS_EX_Composited = 0x02000000;

        public Portrait() {
            InitializeComponent();

            BackColor = StyleManager.BlendTransparencyKey;
            TransparencyKey = StyleManager.BlendTransparencyKey;
        }

        ~Portrait() {
            if (backgroundImage != null) {
                backgroundImage.Dispose();
            }

            if (timer != null) {
                timer.Stop();
                timer.Dispose();
            }
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_Composited | WS_EX_Transparent | WS_EX_Layered;
                return cp;
            }
        }

        public override void LoadHUD() {
            pictureBox = new PictureBox();
            pictureBox.Location = new Point(0, 0);
            pictureBox.Size = new Size(this.Width, this.Height);
            pictureBox.BackColor = StyleManager.TransparencyKey;
            this.Controls.Add(pictureBox);

            try {
                string backgroundLocation = SettingsManager.getSettingString("PortraitBackgroundImage");
                if (backgroundLocation != null) {
                    backgroundImage = Image.FromFile(backgroundLocation);
                } else {
                    backgroundImage = StyleManager.GetImage("defaultportrait-blue.png").Clone() as Image;
                }
            } catch {
                backgroundImage = StyleManager.GetImage("defaultportrait-blue.png").Clone() as Image;
            }

            try {
                string centerLocation = SettingsManager.getSettingString("PortraitCenterImage");
                if (centerLocation != null) {
                    centerImage = Image.FromFile(centerLocation);
                } else {
                    centerImage = null;
                }
            } catch {
                centerImage = null;
            }


            backgroundOffset = new Point(SettingsManager.getSettingInt("PortraitBackgroundXOffset"), SettingsManager.getSettingInt("PortraitBackgroundYOffset"));
            centerOffset = new Point(SettingsManager.getSettingInt("PortraitCenterXOffset"), SettingsManager.getSettingInt("PortraitCenterYOffset"));

            backgroundScale = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitBackgroundScale")));
            centerScale = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitCenterScale")));

            RefreshHUD(1, 1);

            this.Load += Portrait_Load;
        }

        private Timer timer;
        private void Portrait_Load(object sender, EventArgs e) {
            timer = new Timer();
            timer.Interval = 25;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        
        private void Timer_Tick(object sender, EventArgs e) {
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

                gr.Clear(StyleManager.BlendTransparencyKey);
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

                int backgroundSize = (int)(height * backgroundScale / 100.0);
                int backgroundBaseOffset = (height - backgroundSize) / 2;
                int centerSize = (int)(height * centerScale / 100.0);
                int centerBaseOffset = (height - centerSize) / 2;

                SummaryForm.RenderImageResized(gr, backgroundImage, new Rectangle(backgroundBaseOffset + backgroundOffset.X, backgroundBaseOffset + backgroundOffset.Y, backgroundSize, backgroundSize));
                SummaryForm.RenderImageResized(gr, centerImage, new Rectangle(centerBaseOffset + centerOffset.X, centerBaseOffset + centerOffset.Y, centerSize, centerSize));

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
            } catch { }
        }
        
        public override string GetHUD() {
            return "Portrait";
        }
    }
}
