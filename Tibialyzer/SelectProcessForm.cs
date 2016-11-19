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
        private Button selectButton;
        private Button cancelButton;
        private Label label1;
        private Button refreshButton;
        private DataGridView processView;
        private DataGridViewTextBoxColumn idColumn;
        private DataGridViewTextBoxColumn titleColumn;
        private DataGridViewTextBoxColumn nameColumn;
        private DataGridViewImageColumn iconColumn;
        public SelectProcessForm() {
            this.InitializeComponent();
            refreshProcesses();
        }

        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.selectButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.processView = new System.Windows.Forms.DataGridView();
            this.iconColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.processView)).BeginInit();
            this.SuspendLayout();
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
            this.selectButton.Location = new System.Drawing.Point(11, 416);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 2;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(378, 416);
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
            // processView
            // 
            this.processView.AllowUserToAddRows = false;
            this.processView.AllowUserToDeleteRows = false;
            this.processView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.processView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconColumn,
            this.nameColumn,
            this.titleColumn,
            this.idColumn});
            this.processView.Location = new System.Drawing.Point(11, 31);
            this.processView.Name = "processView";
            this.processView.ReadOnly = true;
            this.processView.RowHeadersVisible = false;
            this.processView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.processView.Size = new System.Drawing.Size(442, 379);
            this.processView.TabIndex = 5;
            this.processView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.processView_CellContentClick);
            // 
            // iconColumn
            // 
            this.iconColumn.HeaderText = "";
            this.iconColumn.Name = "iconColumn";
            this.iconColumn.ReadOnly = true;
            this.iconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iconColumn.Width = 48;
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            this.nameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.nameColumn.Width = 150;
            // 
            // titleColumn
            // 
            this.titleColumn.HeaderText = "Title";
            this.titleColumn.Name = "titleColumn";
            this.titleColumn.ReadOnly = true;
            this.titleColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.titleColumn.Width = 140;
            // 
            // idColumn
            // 
            this.idColumn.HeaderText = "ID";
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            this.idColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // SelectProcessForm
            // 
            this.ClientSize = new System.Drawing.Size(456, 445);
            this.Controls.Add(this.processView);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SelectProcessForm";
            ((System.ComponentModel.ISupportInitialize)(this.processView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        Dictionary<DataGridViewRow, Process> processMap = new Dictionary<DataGridViewRow, Process>();
        private void refreshProcesses() {
            processView.Rows.Clear();
            processMap.Clear();
            Process[] currentProcesses = Process.GetProcesses();
            foreach (Process p in currentProcesses) {
                Image image = null;
                if (p.MainWindowTitle != null && p.MainWindowTitle.Length > 0) {
                    try {
                        Icon ico = Icon.ExtractAssociatedIcon(p.MainModule.FileName);
                        image = ico.ToBitmap();
                    } catch {
                    }
                }

                var row = new DataGridViewRow();
                var processIcon = new DataGridViewImageCell();
                processIcon.Value = image != null ? image : StyleManager.GetImage("executable.png");
                var name = new DataGridViewTextBoxCell();
                name.Value = p.ProcessName;
                var title = new DataGridViewTextBoxCell();
                title.Value = p.MainWindowTitle;
                var id = new DataGridViewTextBoxCell();
                id.Value = p.Id;
                row.Cells.Add(processIcon);
                row.Cells.Add(name);
                row.Cells.Add(title);
                row.Cells.Add(id);
                row.Height = 48;
                row.ReadOnly = true;
                processMap.Add(row, p);
                processView.Rows.Add(row);
            }
        }
        
        private void selectButton_Click(object sender, EventArgs e) {
            if (processView.SelectedRows.Count == 1) {
                ProcessManager.SelectProcess(processMap[processView.SelectedRows[0]]);
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void refreshButton_Click(object sender, EventArgs e) {
            refreshProcesses();
        }

        private void processView_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }
    }
}
