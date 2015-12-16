using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Tibialyzer {

    public partial class CreatureDropsForm : NotificationForm {
        public Creature creature;
        private List<Image> images = new List<Image>();

        public CreatureDropsForm() {
            InitializeComponent();
        }

        private void DisplayItem(ItemDrop drop, int base_x, int base_y, int x, int y, Size item_size, ToolTip droprate_tooltip, int dropbar_height, string prefix = "Drop rate of ") {

            // the main picture of the item
            PictureBox picture_box = new PictureBox();
            picture_box.Location = new System.Drawing.Point(base_x + x, base_y + y);
            picture_box.Name = drop.item.name;
            picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
            picture_box.TabIndex = 1;
            picture_box.TabStop = false;
            picture_box.Image = drop.item.image;
            picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
            picture_box.BackgroundImage = MainForm.item_background;
            picture_box.Click += openItemBox;
            droprate_tooltip.SetToolTip(picture_box, prefix + drop.item.name + " is " + (drop.percentage >= 0 ? Math.Round(drop.percentage, 1).ToString() + "%." : "unknown."));
            this.Controls.Add(picture_box);

            // the 'dropbar' that roughly displays the droprate of the item
            PictureBox dropbar_box = new PictureBox();
            Brush brush;
            dropbar_box.Location = new System.Drawing.Point(base_x + x, base_y + y + item_size.Height);
            dropbar_box.Name = "dropbar_image";
            dropbar_box.Size = new System.Drawing.Size(item_size.Width, dropbar_height);
            dropbar_box.TabIndex = 1;
            dropbar_box.TabStop = false;
            Image image = new Bitmap(dropbar_box.Width, dropbar_box.Height);
            Graphics gr = Graphics.FromImage(image);
            gr.FillRectangle(Brushes.DarkGray, new Rectangle(0, 0, item_size.Width, dropbar_height)); //dropbar base bar
            if (drop.percentage < 1) {
                brush = Brushes.DarkRed; // <1% is red
            } else if (drop.percentage < 15) {
                brush = Brushes.Yellow; //<15% is yellow
            } else {
                brush = Brushes.ForestGreen; //everything else is green
            }
            gr.FillRectangle(brush, new Rectangle(0, 0, (int)(Math.Ceiling(item_size.Width * drop.percentage / 100) + 1), dropbar_height));
            dropbar_box.Image = image;
            this.Controls.Add(dropbar_box);
        }

        private void CombineItems() {
            Size item_size = new Size(32, 32); //size of item image
            int dropbar_height = 6; //height of dropbar
            int item_spacing = 6; //spacing between items
            int base_x = 110;
            int base_y = this.mainImage.Location.Y;
            int max_x = 250;
            int max_y = base_y + 134;

            // add a tooltip that displays the actual droprate when you mouseover
            ToolTip droprate_tooltip = new ToolTip();
            droprate_tooltip.AutoPopDelay = 60000;
            droprate_tooltip.InitialDelay = 500;
            droprate_tooltip.ReshowDelay = 0;
            droprate_tooltip.ShowAlways = true;
            droprate_tooltip.UseFading = true;

            int x = item_spacing, y = item_spacing;
            List<ItemDrop> sorted_items = creature.itemdrops.OrderByDescending(o => o.percentage).ToList();
            foreach (ItemDrop drop in sorted_items) {
                if (x > (max_x - item_size.Width - item_spacing)) {
                    x = item_spacing;
                    y += item_size.Height + item_spacing;
                }
                DisplayItem(drop, base_x, base_y, x, y, item_size, droprate_tooltip, dropbar_height);
                x += item_size.Width + item_spacing;
            }

            if (creature.skin != null) {
                ItemDrop skinDrop = new ItemDrop();
                PictureBox picture_box = new PictureBox();
                picture_box.Location = new System.Drawing.Point(20, this.statsButton.Location.Y + this.statsButton.Size.Height + 10);
                picture_box.Name = creature.skin.skin_item.name;
                picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
                picture_box.TabIndex = 1;
                picture_box.TabStop = false;
                picture_box.Image = creature.skin.skin_item.image;
                picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                picture_box.BackgroundImage = MainForm.item_background;
                picture_box.Click += openItemBox; droprate_tooltip.SetToolTip(picture_box, "You can skin this creature with the item " + creature.skin.skin_item.name + ".");
                this.Controls.Add(picture_box);

                skinDrop.item = creature.skin.drop_item;
                skinDrop.percentage = creature.skin.percentage;
                DisplayItem(skinDrop, 20 + item_size.Width + item_spacing, this.statsButton.Location.Y + this.statsButton.Size.Height + 10, 0, 0, item_size, droprate_tooltip, dropbar_height, "Skin rate of ");
                if (y < this.statsButton.Location.Y + this.statsButton.Size.Height) y = this.statsButton.Location.Y + this.statsButton.Size.Height;
            }

            if (this.Height < (y + item_size.Height * 2 + item_spacing)) {
                this.Height = y + item_size.Height * 2 + item_spacing;
            }
            this.Refresh();
        }

        private bool clicked = false;
        void openItemBox(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("item@" + (sender as Control).Name);
        }

        private void CreatureDropsForm_Load(object sender, EventArgs e) {
            this.SuspendForm();
            base.NotificationInitialize();
            // load image from the creature
            this.mainImage.Image = this.creature.image;
            this.statsButton.Name = this.creature.name.ToLower();
            // set background of actual form to transparent
            this.BackColor = MainForm.background_color;
            this.Opacity = MainForm.opacity;
            if (MainForm.transparent) {
                this.TransparencyKey = MainForm.background_color;
                this.Opacity = 1;
            }
            CombineItems();
            this.nameLabel.Text = this.creature.name;
            Font f = this.nameLabel.Font;
            while (TextRenderer.MeasureText(this.creature.name, f).Width < this.mainImage.Size.Width && TextRenderer.MeasureText(this.creature.name, f).Height < 26) {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size + 1.0f);
            }
            while (TextRenderer.MeasureText(this.creature.name, f).Width > this.mainImage.Size.Width && f.Size > 1) {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size - 1.0f);
            }
            this.nameLabel.Font = f;
            this.nameLabel.Left = this.mainImage.Left + (mainImage.Width - this.nameLabel.Size.Width) / 2;
            this.NotificationFinalize();
            this.ResumeForm();
        }

        private void statsButton_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("stats@" + (sender as Control).Name);
        }
    }
}
