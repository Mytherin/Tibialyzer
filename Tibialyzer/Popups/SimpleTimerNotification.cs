using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tibialyzer {
    class SimpleTimerNotification : SimpleTextNotification {
        private double remainingTime = 0;
        private System.Timers.Timer timer;
        private Label timeLabel;
        private Stopwatch elapsedWatch;

        public SimpleTimerNotification(Image image, string title, string text, double remainingSeconds) :
            base(image, title, text, remainingSeconds, remainingSeconds > 0 ? 60 : 0) {
            this.remainingTime = remainingSeconds;
            if (remainingTime > 0) {
                timeLabel = new Label();
                timeLabel.Location = new Point(this.Size.Width - 60, (this.Size.Height - 40) / 2);
                timeLabel.Text = String.Format("{0:0.0}", remainingTime);
                timeLabel.Size = new Size(60, 40);
                timeLabel.Font = StyleManager.MainFormLabelFontBig;
                timeLabel.BackColor = Color.Transparent;
                timeLabel.ForeColor = StyleManager.MainFormButtonForeColor;
                timeLabel.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(timeLabel);

                timer = new System.Timers.Timer(10);
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;

                elapsedWatch = new Stopwatch();
                elapsedWatch.Start();
            }
        }


        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            elapsedWatch.Stop();
            remainingTime -= elapsedWatch.Elapsed.TotalSeconds;

            if (remainingTime > 0) {
                try {
                    this.Invoke((MethodInvoker)delegate {
                        timeLabel.Text = String.Format("{0:0.0}", remainingTime);
                    });
                } catch {

                }
                elapsedWatch.Restart();
            } else {
                try {
                    this.Invoke((MethodInvoker)delegate {
                        timeLabel.Visible = false;
                    });
                } catch {

                }
                timer.Enabled = false;
                timer.Dispose();
            }
        }
    }
}
