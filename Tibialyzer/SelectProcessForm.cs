using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace Tibialyzer {
    class SelectProcessForm : Form {
        private ListBox processList;
        private Button selectButton;
        private Button cancelButton;
        private Label label1;
        private Button refreshButton;
        private List<Process> processes;
        public SelectProcessForm() {
            this.InitializeComponent();
            refreshProcesses();
        }

        private void InitializeComponent() {
            this.processList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // processList
            // 
            this.processList.FormattingEnabled = true;
            this.processList.Location = new System.Drawing.Point(258, 34);
            this.processList.Name = "processList";
            this.processList.Size = new System.Drawing.Size(195, 251);
            this.processList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Tibia Application.";
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(258, 291);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 2;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(378, 289);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(378, 5);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // SelectProcessForm
            // 
            this.ClientSize = new System.Drawing.Size(456, 313);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SelectProcessForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        List<Control> removedControls = new List<Control>();
        private void refreshProcesses() {
            foreach(Control c in removedControls) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            removedControls.Clear();

            this.processList.Items.Clear();
            Process[] currentProcesses = Process.GetProcesses();
            processes = new List<Process>();

            foreach (Process p in currentProcesses) {
                if (p.ProcessName.ToLower().Contains("tibia") || p.ProcessName.ToLower().Contains("flash")) {
                    processes.Add(p);
                }
            }

            int x = label1.Location.X;
            int y = label1.Location.Y + label1.Size.Height;
            int index = 0;
            foreach(Process p in processes) {
                Image image = null;
                try {
                    Icon ico = Icon.ExtractAssociatedIcon(p.MainModule.FileName);
                    image = ico.ToBitmap();
                } catch {
                }
                PictureBox box = new PictureBox();
                box.Image = image;
                box.Location = new Point(x, y);
                box.Size = new Size(32, 32);
                box.SizeMode = PictureBoxSizeMode.Zoom;
                box.Name = index.ToString();
                box.Click += SelectProcess;
                Controls.Add(box);
                removedControls.Add(box);

                Button label = new Button();
                label.Text = p.ProcessName;
                label.Location = new Point(x + 32, y);
                label.Size = new Size(200, 32);
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Name = index.ToString();
                label.Click += SelectProcess;
                Controls.Add(label);
                removedControls.Add(label);
                index++;

                y += 36;
            }

            processes.AddRange(currentProcesses.ToList().OrderBy(o => o.ProcessName));
            foreach(Process p in processes) {
                processList.Items.Add(p.ProcessName);
            }
        }

        private void SelectProcess(object sender, EventArgs e) {
            int index = int.Parse((sender as Control).Name);

            MainForm.TibiaClientName = this.processes[index].ProcessName;
            MainForm.TibiaProcessId = this.processes[index].Id;
            SettingsManager.setSetting("TibiaClientName", MainForm.TibiaClientName);

            this.Close();
        }

        private void selectButton_Click(object sender, EventArgs e) {
            if (this.processList.SelectedIndex < processes.Count) {
                MainForm.TibiaClientName = this.processes[processList.SelectedIndex].ProcessName;
                MainForm.TibiaProcessId = this.processes[processList.SelectedIndex].Id;
                SettingsManager.setSetting("TibiaClientName", MainForm.TibiaClientName);
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void refreshButton_Click(object sender, EventArgs e) {
            refreshProcesses();
        }
    }
}
