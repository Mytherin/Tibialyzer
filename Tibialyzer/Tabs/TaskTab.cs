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
    public partial class TaskTab : Form, TabInterface {
        private Dictionary<int, Task> buttonTasks = new Dictionary<int, Task>();
        private Dictionary<int, TextBox> textBoxes = new Dictionary<int, TextBox>();
        public TaskTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }
        
        public void InitializeSettings() {
            int x = 9, y = 9;
            int index = 0;
            foreach(var taskPair in StorageManager.taskList) {
                foreach(Task task in taskPair.Value) {
                    index = task.id;

                    PictureBox image = new PictureBox();
                    image.Size = new Size(48, 48);
                    if (task.GetImage().Size.Width < 48 && task.GetImage().Size.Height < 48) {
                        image.SizeMode = PictureBoxSizeMode.CenterImage;
                    } else {
                        image.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    image.Location = new Point(x, y);
                    image.BackColor = Color.Transparent;
                    image.Name = index.ToString();
                    image.Click += ToggleTrack;

                    TextBox textBox = new TextBox();
                    textBox.BackColor = StyleManager.MainFormButtonColor;
                    textBox.Font = StyleManager.MainFormLabelFontSmall;
                    textBox.ForeColor = StyleManager.MainFormButtonForeColor;
                    textBox.Location = new System.Drawing.Point(x + 50, y + 12);
                    textBox.Size = new System.Drawing.Size(60, 23);
                    textBox.TabIndex = index;
                    textBox.Name = index.ToString();
                    textBox.Text = TaskManager.GetKillCount(task.id).ToString();
                    textBox.TextAlign = HorizontalAlignment.Right;
                    textBox.TextChanged += ChangeTrackedCount;

                    if (TaskManager.IsTracked(index)) {
                        textBox.Enabled = true;
                        image.Image = task.GetImage();
                    } else {
                        textBox.Enabled = false;
                        image.Image = task.GetImage().ToGrayscale();
                    }

                    textBoxes.Add(index, textBox);
                    this.Controls.Add(textBox);
                    this.Controls.Add(image);
                    buttonTasks.Add(index, task);
                    y += 48;
                    if (y > this.Height - 60) {
                        y = 9;
                        x += 120;
                    }
                }
            }
        }
        
        private void ChangeTrackedCount(object sender, EventArgs e) {
            if (!(sender is TextBox)) return;
            int index = 0;
            if (!int.TryParse((sender as Control).Name, out index)) return;

            int trackedValue = 0;
            if (int.TryParse((sender as TextBox).Text, out trackedValue)) {
                TaskManager.ChangeKillCount(index, trackedValue);
            }
        }

        private void ToggleTrack(object sender, EventArgs e) {
            if (!(sender is PictureBox)) return;
            int index = 0;
            if (!int.TryParse((sender as Control).Name, out index)) return;

            Task task = buttonTasks[index];
            if (!TaskManager.IsTracked(index)) {
                (sender as PictureBox).Image = task.GetImage();
                textBoxes[index].Enabled = true;
                TaskManager.ChangeTracked(index, true);
            } else {
                (sender as PictureBox).Image = task.GetImage().ToGrayscale();
                textBoxes[index].Enabled = false;
                TaskManager.ChangeTracked(index, false);
            }
        }

        public void ApplyLocalization() {
        }
    }
}
