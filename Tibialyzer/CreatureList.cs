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

        public CreatureList(int initialPage = 0) {
            this.currentPage = initialPage;
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private int currentPage = 0;
        private List<Control> createdControls = new List<Control>();
        private void refresh() {
            foreach(Control c in createdControls) {
                this.Controls.Remove(c);
                c.Dispose();
            }

            int base_y = this.listTitle.Location.Y + this.listTitle.Height + 10;
            MainForm.PageInfo pageInfo = new MainForm.PageInfo(false, false);
            int y = MainForm.DisplayCreatureList(this.Controls, objects, 10, base_y, 344, 4, true, null, 1, createdControls, currentPage, 600, pageInfo);
            foreach (Control control in createdControls)
                control.Click += openItemBox;

            if (pageInfo.prevPage) {
                PictureBox prevpage = new PictureBox();
                prevpage.Location = new Point(10, base_y + y);
                prevpage.Size = new Size(97, 23);
                prevpage.Image = MainForm.prevpage_image;
                prevpage.BackColor = Color.Transparent;
                prevpage.SizeMode = PictureBoxSizeMode.StretchImage;
                prevpage.Click += Prevpage_Click;
                this.Controls.Add(prevpage);
                createdControls.Add(prevpage);

            }
            if (pageInfo.nextPage) {
                PictureBox nextpage = new PictureBox();
                nextpage.Location = new Point(this.Size.Width - 108, base_y + y);
                nextpage.Size = new Size(98, 23);
                nextpage.BackColor = Color.Transparent;
                nextpage.Image = MainForm.nextpage_image;
                nextpage.SizeMode = PictureBoxSizeMode.StretchImage;
                nextpage.Click += Nextpage_Click;
                this.Controls.Add(nextpage);
                createdControls.Add(nextpage);
            }
            if (pageInfo.prevPage || pageInfo.nextPage) {
                y += 23;
            }

            this.Size = new Size(this.Size.Width, base_y + y + 10);
        }

        private void Nextpage_Click(object sender, EventArgs e) {
            currentPage++;
            this.SuspendForm();
            refresh();
            this.ResumeForm();
        }

        private void Prevpage_Click(object sender, EventArgs e) {
            currentPage--;
            this.SuspendForm();
            refresh();
            this.ResumeForm();
        }

        public override void LoadForm() {
            this.SuspendForm();

            this.NotificationInitialize();

            refresh();

            this.listTitle.Text = title;
            this.listTitle.Location = new Point(this.Size.Width / 2 - this.listTitle.Width / 2, this.listTitle.Location.Y);
            
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
