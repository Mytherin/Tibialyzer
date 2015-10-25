using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer
{
    public partial class LootDropForm : NotificationForm
    {
        public List<Item> items = new List<Item>();
        public List<Creature> creatures = new List<Creature>();
        List<Image> images = new List<Image>();

        public static Font loot_font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
        private string screenshot_path;
        public LootDropForm(string screenshot_path = "")
        {
            this.screenshot_path = screenshot_path;
            if (screenshot_path != "") this.Visible = false;
            InitializeComponent();
        }

        Image[] GetFrames(Image originalImg)
        {
            int numberOfFrames = originalImg.GetFrameCount(FrameDimension.Time);

            Image[] frames = new Image[numberOfFrames];
            for (int i = 0; i < numberOfFrames; i++)
            {
                originalImg.SelectActiveFrame(FrameDimension.Time, i);
                frames[i] = ((Image)originalImg.Clone());
            }

            return frames;
        }

        private Image GetStackImage(Image[] stack, int count, Item item)
        {
            if (stack == null) return item.image;
            int max = stack.Length;
            int index = 0;

            if (count <= 5) index = count - 1;
            else if (count <= 10) index = 5;
            else if (count <= 25) index = 6;
            else if (count <= 50) index = 7;
            else index = 8;

            if (index >= max) index = max - 1;
            return stack[index];
        }

        private void SaveScreenshot()
        {
            Bitmap bitmap = new Bitmap(Size.Width, Size.Height);
            DrawToBitmap(bitmap, new Rectangle(0, 0, Size.Width, Size.Height));
            bitmap.Save(screenshot_path);
            bitmap.Dispose();
        }

        private void LootDropForm_Load(object sender, EventArgs e)
        {
            int base_x = 20, base_y = 30;
            int x = 0, y = 0;
            int item_spacing = 4;
            Size item_size = new Size(32, 32);
            int max_x = 275;
            int max_creature_y = 500;
            this.SuspendLayout();
            this.NotificationInitialize();

            // add a tooltip that displays the actual droprate when you mouseover
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            int total_value = 0;
            foreach(Item item in items)
            {
                Image[] stacks = null;
                if (item.stackable) {
                    stacks = GetFrames(item.image);
                }
                while (item.drops > 0)
                {
                    if (x >= (max_x - item_size.Width - item_spacing))
                    {
                        x = 0;
                        y = y + item_size.Height + item_spacing;
                    }
                    int mitems = 1;
                    if (item.stackable) mitems = Math.Min(item.drops, 100);
                    item.drops -= mitems;

                    PictureBox picture_box = new PictureBox();
                    picture_box.Location = new System.Drawing.Point(base_x + x, base_y + y);
                    picture_box.Name = item.name;
                    picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
                    picture_box.TabIndex = 1;
                    picture_box.TabStop = false;
                    if (item.stackable) {
                        Bitmap image = new Bitmap(GetStackImage(stacks, mitems, item));
                        Graphics gr = Graphics.FromImage(image);
                        int numbers = (int)Math.Floor(Math.Log(mitems,10)) + 1;
                        int xoffset = 1, logamount = mitems;
                        for(int i = 0; i < numbers; i++) {
                            int imagenr = logamount % 10;
                            xoffset = xoffset + MainForm.image_numbers[imagenr].Width + 1;
                            gr.DrawImage(MainForm.image_numbers[imagenr], 
                                new Point(image.Width - xoffset, image.Height - MainForm.image_numbers[imagenr].Height - 3));
                            logamount /= 10;
                        }
                        picture_box.Image = image;
                    } else {
                        picture_box.Image = item.image;
                    }
                    
                    picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box.BackgroundImage = MainForm.item_background;
                    picture_box.Click += openItemBox;
                    value_tooltip.SetToolTip(picture_box, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.name) + " value: " + Math.Max(item.actual_value, item.vendor_value) * mitems);
                    total_value += Math.Max(item.actual_value, item.vendor_value) * mitems;
                    this.Controls.Add(picture_box);

                    x += item_size.Width + item_spacing;
                }
            }
            Graphics graphics = Graphics.FromImage(MainForm.image_numbers[0]);
            this.totalValueLabel.Text = "Total Value: " + total_value.ToString();
            this.totalValueLabel.Location = new Point(this.Width / 2 - ((int)graphics.MeasureString(totalValueLabel.Text, totalValueLabel.Font).Width) / 2, totalValueLabel.Location.Y);
            x = 0;
            y = y + item_size.Height + item_spacing;
            if (y < max_creature_y)
            {
                base_x = 5;
                Size creature_size = new Size(1, 1);
                Size labelSize = new Size(1, 1);

                foreach (Creature creature in creatures)
                {
                    creature_size.Width = Math.Max(creature_size.Width, creature.image.Width);
                    creature_size.Height = Math.Max(creature_size.Height, creature.image.Height);
                }
                int i = 0;
                foreach (Creature creature in creatures)
                {
                    if (x >= max_x - creature_size.Width - item_spacing * 2)
                    {
                        x = 0;
                        y = y + creature_size.Height + 20;
                    }
                    int xoffset = (creature_size.Width - creature.image.Width) / 2;
                    int yoffset = (creature_size.Height - creature.image.Height) / 2;
                
                    TransparentLabel count = new TransparentLabel();
                    count.Text = creature.kills.ToString() + "x";
                    count.Font = loot_font;
                    count.Size = new Size(1, 10);
                    count.Location = new Point(base_x + x + xoffset, base_y + y + creature_size.Height);
                    count.AutoSize = true;
                    count.TextAlign = ContentAlignment.MiddleCenter;
                    count.ForeColor = Color.FromArgb(191, 191, 191);

                    int measured_size = (int)count.CreateGraphics().MeasureString(count.Text, count.Font).Width;
                    int width = Math.Max(measured_size, creature.image.Width);
                    TransparentPictureBox picture_box = new TransparentPictureBox();
                    picture_box.Location = new System.Drawing.Point(base_x + x + xoffset, base_y + y + yoffset + (creature_size.Height - creature.image.Height) / 2);
                    picture_box.Name = creature.name;
                    picture_box.Size = new System.Drawing.Size(creature.image.Width, creature.image.Height);
                    picture_box.TabIndex = 1;
                    picture_box.TabStop = false;
                    picture_box.Image = creature.image;
                    picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box.Click += openCreatureDrops;
                                        
                    if (width > creature.image.Width)
                    {
                        picture_box.Location = new Point(picture_box.Location.X + (width - creature.image.Width) / 2, picture_box.Location.Y);
                    }
                    else
                    {
                        count.Location = new Point(count.Location.X + (width - measured_size) / 2, count.Location.Y);
                    }

                    labelSize = count.Size;

                    i++;
                    x += width + xoffset;
                    this.Controls.Add(picture_box);
                    this.Controls.Add(count);
                }
                y = y + creature_size.Height + labelSize.Height * 2;
            }

            this.ResumeLayout(false);
            this.PerformLayout();
            this.Size = new Size(max_x + item_spacing * 2, base_y + y + item_spacing + 10);
            if (screenshot_path != "")
                SaveScreenshot();
            else base.NotificationFinalize();
        }

        private bool clicked = false;
        void openItemBox(object sender, EventArgs e)
        {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.priority_command = "item@" + (sender as Control).Name;
        }

        void openCreatureDrops(object sender, EventArgs e)
        {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.priority_command = "loot@" + (sender as Control).Name;
        }


    }
}
