using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer {
    class CreatureList : NotificationForm {
        public List<TibiaObject> objects;
        public string title = "List";
        public string prefix = "creature" + MainForm.commandSymbol;

        public CreatureList() {
            objects = null;
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatureList));
            this.listTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listTitle
            // 
            this.listTitle.AutoSize = true;
            this.listTitle.BackColor = System.Drawing.Color.Transparent;
            this.listTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.listTitle.Location = new System.Drawing.Point(152, 9);
            this.listTitle.Name = "listTitle";
            this.listTitle.Size = new System.Drawing.Size(32, 16);
            this.listTitle.TabIndex = 14;
            this.listTitle.Text = "List";
            // 
            // CreatureList
            // 
            this.ClientSize = new System.Drawing.Size(352, 76);
            this.Controls.Add(this.listTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreatureList";
            this.Text = "Tibia Object List";
            this.Load += new System.EventHandler(this.CreatureList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CreatureList_Load(object sender, EventArgs e) {
            this.SuspendForm();
            int base_y =  this.listTitle.Location.Y + this.listTitle.Height + 10;
            int y = MainForm.DisplayCreatureList(this.Controls, objects, 10, base_y, 344, 4, true);

            this.listTitle.Text = title;
            this.listTitle.Location = new Point(this.Size.Width / 2 - this.listTitle.Width / 2, this.listTitle.Location.Y);
            
            this.Size = new Size(this.Size.Width, base_y + y + 10);

            foreach (Control control in this.Controls)
                if (control is PictureBox)
                    control.Click += openItemBox;
            this.NotificationInitialize();
            this.NotificationFinalize();
            this.ResumeForm();
        }

        private Label listTitle;

        private bool clicked = false;
        void openItemBox(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand(prefix + (sender as Control).Name);
        }
    }
}
