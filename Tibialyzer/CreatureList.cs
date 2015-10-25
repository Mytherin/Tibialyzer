using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer
{
    class CreatureList : NotificationForm
    {
        public List<Creature> creatures;

        public CreatureList()
        {
            creatures = null;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CreatureList
            // 
            this.ClientSize = new System.Drawing.Size(344, 52);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreatureList";
            this.Load += new System.EventHandler(this.CreatureList_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Cleanup();
                if (creatures != null)
                {
                    foreach (Creature creature in creatures)
                        creature.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void CreatureList_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            int y = MainForm.DisplayCreatureList(this.Controls, (creatures as IEnumerable<TibiaObject>).ToList(), 10, 10, 344, 4, true);

            this.Size = new Size(this.Size.Width, this.Size.Height + y);
            
             foreach (Control control in this.Controls)
                        if (control is TransparentPictureBox)
                            control.Click += openItemBox;
            this.NotificationInitialize();
            this.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private bool clicked = false;
        void openItemBox(object sender, EventArgs e)
        {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.priority_command = "creature@" + (sender as Control).Name;
        }
    }
}
