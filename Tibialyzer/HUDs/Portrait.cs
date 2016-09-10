using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tibialyzer.Structures;

namespace Tibialyzer {
    public partial class Portrait : BaseHUD {
        PictureBox pictureBox;
        private Image backgroundImage, centerImage;
        private Point backgroundOffset, centerOffset;
        private int backgroundScale, centerScale;

        private float lifePercentage;
        private float manaPercentage;
        private int level;

        private const int WS_EX_Transparent = 0x20;
        private const int WS_EX_Layered = 0x80000;
        private const int WS_EX_Composited = 0x02000000;

        public Portrait() {
            InitializeComponent();

            lifePercentage = 1;
            manaPercentage = 1;
            level = 1;

            BackColor = StyleManager.BlendTransparencyKey;
            TransparencyKey = StyleManager.BlendTransparencyKey;
            MemoryReader.HealthChanged += (o, e) => RefreshHealth(e);
            MemoryReader.ManaChanged += (o, e) => RefreshMana(e);
            MemoryReader.ExperienceChanged += (o, e) => RefreshExp(e);
            ProcessManager.TibiaVisibilityChanged += (o, e) => UpdateVisibility(e);
        }

        ~Portrait() {
            if (backgroundImage != null) {
                backgroundImage.Dispose();
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
                        
            string centerLocation = SettingsManager.getSettingString("PortraitCenterImage");
            if (centerLocation != null) {
                try {
                    centerImage = Image.FromFile(centerLocation);
                } catch {
                    OutfiterOutfit outfit = new OutfiterOutfit();
                    outfit.FromString(centerLocation);
                    using(Image image = outfit.GetImage()) {
                        using(Bitmap bitmap = new Bitmap(image)) {
                            centerImage = bitmap.Clamp();
                        }
                    }
                }
            } else {
                centerImage = null;
            }


            backgroundOffset = new Point(SettingsManager.getSettingInt("PortraitBackgroundXOffset"), SettingsManager.getSettingInt("PortraitBackgroundYOffset"));
            centerOffset = new Point(SettingsManager.getSettingInt("PortraitCenterXOffset"), SettingsManager.getSettingInt("PortraitCenterYOffset"));

            backgroundScale = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitBackgroundScale")));
            centerScale = Math.Min(100, Math.Max(0, SettingsManager.getSettingInt("PortraitCenterScale")));

            RefreshHUD();
        }

        private void RefreshHUD() {
            float life = lifePercentage.ClampPercentage();
            float mana = manaPercentage.ClampPercentage();
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
                lifeRectangle.Width = (int)(lifeRectangle.Width * life) - 2;
                using (Brush brush = new SolidBrush(StyleManager.GetHealthColor(life))) {
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
                manaRectangle.Width = (int)(manaRectangle.Width * mana) - 2;
                using (Brush brush = new SolidBrush(StyleManager.ManaColor)) {
                    gr.FillRectangle(brush, manaRectangle);
                }

                int backgroundSize = (int)(height * backgroundScale / 100.0);
                int backgroundBaseOffset = (height - backgroundSize) / 2;
                int centerSize = (int)(height * centerScale / 100.0);
                int centerBaseOffset = (height - centerSize) / 2;

                using(Brush brush = new SolidBrush(this.TransparencyKey)) {
                    gr.FillEllipse(brush, new Rectangle(backgroundBaseOffset + backgroundOffset.X, backgroundBaseOffset + backgroundOffset.Y, backgroundSize, backgroundSize));
                }
                using (Pen pen = new Pen(Color.Black, 5)) {
                    gr.DrawEllipse(pen, new Rectangle(backgroundBaseOffset + backgroundOffset.X, backgroundBaseOffset + backgroundOffset.Y, backgroundSize, backgroundSize));
                }

                //SummaryForm.RenderImageResized(gr, backgroundImage, new Rectangle(backgroundBaseOffset + backgroundOffset.X, backgroundBaseOffset + backgroundOffset.Y, backgroundSize, backgroundSize));
                SummaryForm.RenderImageResized(gr, centerImage, new Rectangle(centerBaseOffset + centerOffset.X, centerBaseOffset + centerOffset.Y, centerSize, centerSize));
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

        private void RefreshHealth(PlayerHealth playerHp) {
            lifePercentage = (float)playerHp.Health / playerHp.MaxHealth;

            try
            {
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD();
                });
            }
            catch { }
        }

        private void RefreshMana(PlayerMana playerMp) {
            manaPercentage = (float)playerMp.Mana / playerMp.MaxMana;

            try
            {
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD();
                });
            }
            catch { }
        }

        private void RefreshExp(PlayerExperience playerExp) {
            level = playerExp.Level;

            try
            {
                this.Invoke((MethodInvoker)delegate {
                    RefreshHUD();
                });
            }
            catch { }
        }
        
        public override string GetHUD() {
            return "Portrait";
        }
    }
}
