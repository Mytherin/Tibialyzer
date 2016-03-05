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
    public partial class WasteForm : NotificationForm {
        public Hunt hunt;
        private List<Control> wasteControls = new List<Control>();
        public WasteForm() {
            InitializeComponent();
        }

        public override void LoadForm() {
            this.SuspendForm();
            this.NotificationInitialize();
            this.RefreshWaste();
            this.NotificationFinalize();
            this.ResumeForm();
        }

        public void UpdateWaste() {
            try {
                this.Invoke((MethodInvoker)delegate {
                    this.SuspendForm();
                    this.RefreshWaste();
                    this.ResumeForm();
                });
            } catch {

            }
        }

        private void RefreshWaste() {
            foreach(Control c in wasteControls) {
                Controls.Remove(c);
                c.Dispose();
            }
            wasteControls.Clear();

            int base_x = 5, x = 0;
            int base_y = 32, y = 0;
            int max_x = this.Size.Width - 5;
            Size item_size = new Size(32, 32);
            int item_spacing = 4;
            foreach (var tpl in HuntManager.GetUsedItems(hunt)) {
                Item item = tpl.Item1;
                int count = tpl.Item2;
                while (count > 0) {
                    if (x >= (max_x - item_size.Width - item_spacing)) {
                        x = 0;
                        y += item_size.Height + item_spacing;
                    }
                    int mitems = 1;
                    if (item.stackable) mitems = Math.Min(count, 100);
                    count -= mitems;

                    PictureBox picture_box = new PictureBox();
                    picture_box.Location = new System.Drawing.Point(base_x + x, base_y + y);
                    picture_box.Name = item.GetName();
                    picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
                    picture_box.TabIndex = 1;
                    picture_box.TabStop = false;
                    picture_box.Click += openItem_Click;
                    if (item.stackable) {
                        picture_box.Image = LootDropForm.DrawCountOnItem(item, mitems);
                    } else {
                        picture_box.Image = item.GetImage();
                    }

                    picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box.BackgroundImage = StyleManager.GetImage("item_background.png");
                    this.Controls.Add(picture_box);
                    wasteControls.Add(picture_box);

                    x += item_size.Width + item_spacing;
                }
            }

            this.Size = new Size(this.Size.Width, base_y + y + item_size.Height + item_spacing * 2);
        }

        private void openItem_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("item" + Constants.CommandSymbol + (sender as Control).Name);
        }

        public override string FormName() {
            return "WasteForm";
        }
    }
}
