using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Tibialyzer {
    class TaskHUD : BaseHUD {
        private ActualTaskHUD actualHUD = null;
        public TaskHUD() {
            this.TransparencyKey = StyleManager.TransparencyKey;
            ProcessManager.TibiaVisibilityChanged += (o, e) => UpdateVisibility(e);

            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;

            actualHUD = new ActualTaskHUD(this);
            actualHUD.ShowInTaskbar = false;
            actualHUD.FormBorderStyle = FormBorderStyle.None;
            actualHUD.Size = this.Size;
            actualHUD.StartPosition = FormStartPosition.Manual;
            actualHUD.Location = this.PointToScreen(new Point(0, 0));

            double opacity = SettingsManager.getSettingDouble(GetHUD() + "Opacity");
            opacity = Math.Min(1, Math.Max(0, opacity));
            this.Opacity = opacity;

            this.FormClosed += CloseActualHUD;
        }

        protected override void OnPaint(PaintEventArgs e) {
            using (Brush brush = new SolidBrush(this.TransparencyKey)) {
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, this.Width, this.Height));
            }
            using (Brush brush = new SolidBrush(Color.Black)) {
                for (int y = 0; y < this.Height; y += actualHUD.TaskSize + actualHUD.VerticalPadding) {
                    e.Graphics.FillRectangle(brush, new Rectangle(0, y, this.Width, actualHUD.TaskSize));
                }
            }
        }

        private void CloseActualHUD(object sender, FormClosedEventArgs e) {
            actualHUD.Close();
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            actualHUD.Visible = this.Visible;
        }

        protected override void OnMove(EventArgs e) {
            base.OnMove(e);
            actualHUD.Location = this.PointToScreen(new Point(0, 0));
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            actualHUD.Size = this.Size;
        }

        public override void ShowHUD() {
            actualHUD.Show(this);
            UpdateVisibility(true);
        }
        
        public override string GetHUD() { return actualHUD.GetHUD(); }
        public override void LoadHUD() { actualHUD.LoadHUD(); }
    }

    class ActualTaskHUD : BaseHUD {
        List<Task> trackedTasks = new List<Task>();
        Dictionary<Task, List<Control>> taskControls = new Dictionary<Task, List<Control>>();
        private object controlLock = new object();
        Font font;
        TaskHUD parentHUD;

        public ActualTaskHUD(TaskHUD parentHUD) {
            this.parentHUD = parentHUD;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            BackColor = StyleManager.BlendTransparencyKey;
            TransparencyKey = StyleManager.BlendTransparencyKey;

            double fontSize = SettingsManager.getSettingDouble(GetHUD() + "FontSize");
            fontSize = fontSize < 0 ? 20 : fontSize;
            font = new System.Drawing.Font("Verdana", (float)fontSize, System.Drawing.FontStyle.Bold);

            TaskSize = SettingsManager.getSettingInt(GetHUD() + "Height");
        }
        
        public override string GetHUD() { return "TaskHUD"; }
        public override void LoadHUD() {
            foreach (Task task in TaskManager.GetTrackedTasks()) {
                CreateTask(task);
            }

            TaskManager.TrackTask += AddTask;
            TaskManager.UntrackTask += RemoveTask;
            TaskManager.TaskUpdated += UpdateTask;
            this.FormClosed += UnregisterEvents;
        }

        private void UnregisterEvents(object sender, FormClosedEventArgs e) {
            TaskManager.TrackTask -= AddTask;
            TaskManager.UntrackTask -= RemoveTask;
            TaskManager.TaskUpdated -= UpdateTask;
        }

        public int TaskSize = 32;
        public int VerticalPadding = 4;
        private void CreateTask(Task task) {
            lock (controlLock) {
                int y = taskControls.Count * (TaskSize + VerticalPadding);
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(TaskSize, TaskSize);
                pictureBox.Image = task.GetImage();
                if (pictureBox.Image.Width < pictureBox.Width && pictureBox.Image.Height < pictureBox.Height) {
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                } else {
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
                pictureBox.Location = new Point(2, y);
                pictureBox.BackColor = Color.Transparent;
                int killCount = TaskManager.GetKillCount(task.id);
                Label label = new Label();
                label.Text = killCount.ToString();
                label.Location = new Point(TaskSize + 6, y);
                label.Size = new Size(this.Width - TaskSize - 4, TaskSize);
                label.BackColor = Color.Transparent;
                label.Font = font;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.ForeColor = killCount >= task.count ? StyleManager.TaskTrackerForeColorCompleted : StyleManager.TaskTrackerForeColor;

                this.Controls.Add(label);
                this.Controls.Add(pictureBox);

                trackedTasks.Add(task);
                taskControls.Add(task, new List<Control> { label, pictureBox });
                this.parentHUD.Size = new Size(this.Size.Width, y + TaskSize);
            }
        }

        private void DeleteTask(Task task) {
            lock (controlLock) {
                int index = trackedTasks.IndexOf(task);
                for (int i = index + 1; i < trackedTasks.Count; i++) {
                    foreach (Control control in taskControls[trackedTasks[i]]) {
                        control.Location = new Point(control.Location.X, control.Location.Y - (TaskSize + VerticalPadding));
                    }
                }
                trackedTasks.Remove(task);
                foreach (Control control in taskControls[task]) {
                    this.Controls.Remove(control);
                    control.Dispose();
                }
                taskControls.Remove(task);
                this.parentHUD.Size = new Size(this.Size.Width, taskControls.Count * (TaskSize + VerticalPadding));
            }
        }

        private void AddTask(Task task) {
            try {
                this.Invoke((MethodInvoker)delegate {
                    if (!trackedTasks.Contains(task)) {
                        CreateTask(task);
                    }
                });
            } catch {

            }
        }
        private void RemoveTask(Task task) {
            try {
                this.Invoke((MethodInvoker)delegate {
                    if (trackedTasks.Contains(task)) {
                        DeleteTask(task);
                    }
                });
            } catch {

            }
        }
        private void UpdateTask(Task task, int newKills) {
            try {
                lock (controlLock) {
                    this.Invoke((MethodInvoker)delegate {
                        if (taskControls.ContainsKey(task)) {
                            taskControls[task][0].Text = newKills.ToString();
                            taskControls[task][0].ForeColor = newKills >= task.count ? StyleManager.TaskTrackerForeColorCompleted : StyleManager.TaskTrackerForeColor;
                        }
                    });
                }
            } catch {

            }
        }
    }
}
