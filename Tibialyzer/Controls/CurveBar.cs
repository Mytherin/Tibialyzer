/*
The functions in this file are based on the work by Viktor Gustavsson (https://github.com/villor/TibiaHUD), which has the following license.

The MIT License (MIT)

Copyright (c) 2016 Viktor Gustavsson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using System;
using System.Drawing;
using System.Windows.Forms;
using Tibialyzer.Structures;

namespace Tibialyzer {
    public class CurveBar : Control {

        private float health = 1;
        private float mana = 1;
        private bool alwaysShow = false;

        public CurveBar() {
            MemoryReader.AttributesChanged += (o, e) => UpdateHud(e);
            ProcessManager.TibiaVisibilityChanged += (o, e) => UpdateHudVisibility(e);
            this.DoubleBuffered = true;
            alwaysShow = SettingsManager.getSettingBool("AlwaysShowHUD");
        }

        private void UpdateHud(PlayerAttributes attributes) {
            health = (float)attributes.Health / attributes.MaxHealth;
            mana = (float)attributes.Mana / attributes.MaxMana;
            this.Invalidate();
        }

        private void UpdateHudVisibility(bool visible) {
            if (!alwaysShow) {
                try {
                    this.Invoke((MethodInvoker)delegate {
                        this.Visible = alwaysShow || visible;
                    });

                }
                catch {
                }
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            float thickness = (float)SettingsManager.getSettingDouble("CurvedBarsFontSize");

            Rectangle rect = this.DisplayRectangle;
            rect.Y -= (rect.Width - rect.Height) / 2;
            rect.Height = rect.Width;
            rect.Width -= (int) (thickness + 2);
            rect.X += (int) (thickness / 2 + 1);

            using (Pen pen = new Pen(Color.Black)) {
                pen.Width = thickness + 2;
                e.Graphics.DrawArc(pen, rect, 135, 90);
                e.Graphics.DrawArc(pen, rect, 45, -90);

                pen.Width = thickness;

                health = health.ClampPercentage();
                mana = mana.ClampPercentage();
                
                pen.Color = StyleManager.GetHealthColor(health);
                e.Graphics.DrawArc(pen, rect, 135, 90 * health);

                pen.Color = StyleManager.ManaColor;
                e.Graphics.DrawArc(pen, rect, 45, -90 * mana);
            }
        }
    }
}
