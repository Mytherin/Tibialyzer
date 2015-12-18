using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;


namespace Tibialyzer {
    class ItemViewForm : NotificationForm {
        private Label itemName;
        private Label itemCategory;
        private Label lookText;
        private Label valueLabel;
        private System.Windows.Forms.PictureBox itemPictureBox;

        public Item item;
        public Dictionary<NPC, int> buyNPCs = null;
        public Dictionary<NPC, int> sellNPCs = null;
        private System.Windows.Forms.CheckBox pickupBox;
        private System.Windows.Forms.CheckBox convertBox;
        private TextBox valueBox;
        public List<Creature> creatures = null;
        private Label statsButton;
        private string previous_value;

        public ItemViewForm() {
            skip_event = true;
            InitializeComponent();
            skip_event = false;
        }

        private void InitializeComponent() {
            this.statsButton = new System.Windows.Forms.Label();
            this.valueBox = new System.Windows.Forms.TextBox();
            this.convertBox = new System.Windows.Forms.CheckBox();
            this.pickupBox = new System.Windows.Forms.CheckBox();
            this.valueLabel = new System.Windows.Forms.Label();
            this.lookText = new System.Windows.Forms.Label();
            this.itemCategory = new System.Windows.Forms.Label();
            this.itemName = new System.Windows.Forms.Label();
            this.itemPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.itemPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statsButton
            // 
            this.statsButton.BackColor = System.Drawing.Color.Transparent;
            this.statsButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.statsButton.Location = new System.Drawing.Point(34, 115);
            this.statsButton.Name = "statsButton";
            this.statsButton.Padding = new System.Windows.Forms.Padding(2);
            this.statsButton.Size = new System.Drawing.Size(96, 21);
            this.statsButton.TabIndex = 27;
            this.statsButton.Text = "Drops";
            this.statsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.statsButton.Click += new System.EventHandler(this.statsButton_Click);
            // 
            // valueBox
            // 
            this.valueBox.Location = new System.Drawing.Point(195, 12);
            this.valueBox.Name = "valueBox";
            this.valueBox.Size = new System.Drawing.Size(148, 20);
            this.valueBox.TabIndex = 7;
            this.valueBox.TextChanged += new System.EventHandler(this.valueBox_TextChanged);
            // 
            // convertBox
            // 
            this.convertBox.AutoSize = true;
            this.convertBox.BackColor = System.Drawing.Color.Transparent;
            this.convertBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.convertBox.Location = new System.Drawing.Point(239, 41);
            this.convertBox.Name = "convertBox";
            this.convertBox.Size = new System.Drawing.Size(104, 17);
            this.convertBox.TabIndex = 6;
            this.convertBox.Text = "Convert To Gold";
            this.convertBox.UseVisualStyleBackColor = false;
            this.convertBox.CheckedChanged += new System.EventHandler(this.convertBox_CheckedChanged);
            // 
            // pickupBox
            // 
            this.pickupBox.AutoSize = true;
            this.pickupBox.BackColor = System.Drawing.Color.Transparent;
            this.pickupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.pickupBox.Location = new System.Drawing.Point(133, 41);
            this.pickupBox.Name = "pickupBox";
            this.pickupBox.Size = new System.Drawing.Size(87, 17);
            this.pickupBox.TabIndex = 5;
            this.pickupBox.Text = "Pick Up Item";
            this.pickupBox.UseVisualStyleBackColor = false;
            this.pickupBox.CheckedChanged += new System.EventHandler(this.pickupBox_CheckedChanged);
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.BackColor = System.Drawing.Color.Transparent;
            this.valueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.valueLabel.Location = new System.Drawing.Point(141, 13);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(56, 16);
            this.valueLabel.TabIndex = 4;
            this.valueLabel.Text = "Value: ";
            // 
            // lookText
            // 
            this.lookText.AutoSize = true;
            this.lookText.BackColor = System.Drawing.Color.Transparent;
            this.lookText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.lookText.Location = new System.Drawing.Point(141, 75);
            this.lookText.MaximumSize = new System.Drawing.Size(210, 0);
            this.lookText.Name = "lookText";
            this.lookText.Size = new System.Drawing.Size(64, 13);
            this.lookText.TabIndex = 3;
            this.lookText.Text = "You see a...";
            // 
            // itemCategory
            // 
            this.itemCategory.AutoSize = true;
            this.itemCategory.BackColor = System.Drawing.Color.Transparent;
            this.itemCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.itemCategory.Location = new System.Drawing.Point(50, 20);
            this.itemCategory.Name = "itemCategory";
            this.itemCategory.Size = new System.Drawing.Size(69, 16);
            this.itemCategory.TabIndex = 2;
            this.itemCategory.Text = "category";
            // 
            // itemName
            // 
            this.itemName.BackColor = System.Drawing.Color.Transparent;
            this.itemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.itemName.Location = new System.Drawing.Point(34, 84);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(96, 28);
            this.itemName.TabIndex = 1;
            this.itemName.Text = "name";
            this.itemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // itemPictureBox
            // 
            this.itemPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.itemPictureBox.Location = new System.Drawing.Point(66, 49);
            this.itemPictureBox.Name = "itemPictureBox";
            this.itemPictureBox.Size = new System.Drawing.Size(32, 32);
            this.itemPictureBox.TabIndex = 0;
            this.itemPictureBox.TabStop = false;
            // 
            // ItemViewForm
            // 
            this.ClientSize = new System.Drawing.Size(378, 151);
            this.Controls.Add(this.statsButton);
            this.Controls.Add(this.valueBox);
            this.Controls.Add(this.convertBox);
            this.Controls.Add(this.pickupBox);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.lookText);
            this.Controls.Add(this.itemCategory);
            this.Controls.Add(this.itemName);
            this.Controls.Add(this.itemPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ItemViewForm";
            this.Load += new System.EventHandler(this.ItemViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.itemPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void DestroyForm() {

        }

        string prefix;
        private string TooltipFunction(TibiaObject obj) {
            NPC npc = obj as NPC;
            return String.Format("{0} {1} for {2} gold.", prefix, item.name, prefix == "Sells" ? buyNPCs[npc] : sellNPCs[npc]);
        }

        private string CreatureTooltipFunction(TibiaObject obj) {
            Creature cr = obj as Creature;
            float percentage = item.itemdrops.Find(o => o.creature == cr).percentage;
            return String.Format("{0}: {1}%", cr.name, percentage < 0 ? "Unknown" : percentage.ToString());
        }

        private void ItemViewForm_Load(object sender, EventArgs e) {
            skip_event = true;
            this.SuspendForm();
            this.NotificationInitialize();
            CultureInfo c = System.Threading.Thread.CurrentThread.CurrentCulture;

            this.itemName.Text = c.TextInfo.ToTitleCase(item.name);
            Font f = this.itemName.Font;
            while (TextRenderer.MeasureText(this.itemName.Text, f).Width < this.itemName.Size.Width && TextRenderer.MeasureText(this.itemName.Text, f).Height < 26) {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size + 1.0f);
            }
            while (TextRenderer.MeasureText(this.itemName.Text, f).Width > this.itemName.Size.Width && f.Size > 1) {
                f.Dispose();
                f = new Font(f.FontFamily, f.Size - 1.0f);
            }
            this.itemName.Font = f;
            this.itemCategory.Text = item.category;
            //this.itemName.Location = new Point(this.itemPictureBox.Location.X + this.itemPictureBox.Width / 2 - this.itemName.Size.Width / 2, this.itemPictureBox.Location.Y + this.itemPictureBox.Height + 5);
            this.itemCategory.Location = new Point(this.itemPictureBox.Location.X + this.itemPictureBox.Width / 2 - this.itemCategory.Size.Width / 2, this.itemPictureBox.Location.Y - this.itemCategory.Height - 5);
            this.itemPictureBox.BackgroundImage = MainForm.item_background;
            this.lookText.Text = item.look_text;
            this.valueBox.Text = Math.Max(item.vendor_value, item.actual_value).ToString();
            this.pickupBox.Checked = !item.discard;
            this.convertBox.Checked = item.convert_to_gold;
            this.itemPictureBox.Image = item.image;

            int base_x = 20, base_y = this.statsButton.Location.Y + this.statsButton.Height + 5;
            int x = 0, y = 0;
            int max_x = 344;
            int spacing = 4;

            // add a tooltip that displays the actual droprate when you mouseover
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;

            if (creatures == null) {
                List<NPC> buyNPCList = buyNPCs.Keys.ToList().OrderBy(o => buyNPCs[o]).ToList();
                List<NPC> sellNPCList = sellNPCs.Keys.ToList().OrderBy(o => sellNPCs[o]).ToList();
                List<List<NPC>> npc_lists = new List<List<NPC>> ();
                npc_lists.Add(buyNPCList); npc_lists.Add(sellNPCList);
                string[] header_string = { "Buy From:", "Sell To:" };
                string[] info_string = { "Sells", "Buys" };
                for (int i = 0; i < npc_lists.Count; i++) {
                    prefix = info_string[i];
                    List<NPC> npc_list = npc_lists[i];
                    if (npc_list != null && npc_list.Count > 0) {
                        Label header = new Label();
                        header.ForeColor = MainForm.label_text_color;
                        header.BackColor = Color.Transparent;
                        header.Text = header_string[i];
                        header.Font = valueLabel.Font;
                        header.Location = new Point(base_x + x, base_y + y);
                        y = y + header.Size.Height;
                        this.Controls.Add(header);

                        y = y + MainForm.DisplayCreatureList(this.Controls, (npc_list as IEnumerable<TibiaObject>).ToList(), base_x, base_y + y, max_x, spacing, true, TooltipFunction);
                    }
                }
                command_start = "npc" + MainForm.commandSymbol;
                switch_start = "drop" + MainForm.commandSymbol;
                statsButton.Text = "Dropped By";
                statsButton.Name = item.name.ToLower();
                foreach (Control control in this.Controls)
                    if (control is PictureBox)
                        control.Click += openItemBox;
            } else {
                Label header = new Label();
                header.ForeColor = MainForm.label_text_color;
                header.BackColor = Color.Transparent;
                header.Text = "Dropped By";
                header.Font = valueLabel.Font;
                header.Location = new Point(base_x + x, base_y + y);
                y = y + header.Size.Height;
                y = y + MainForm.DisplayCreatureList(this.Controls, (creatures as IEnumerable<TibiaObject>).ToList(), base_x, base_y + y, max_x, spacing, true, CreatureTooltipFunction);

                command_start = "creature" + MainForm.commandSymbol;
                switch_start = "item" + MainForm.commandSymbol;
                statsButton.Text = "Sold By";
                statsButton.Name = item.name.ToLower();
                foreach (Control control in this.Controls)
                    if (control is PictureBox)
                        control.Click += openItemBox;
            }
            this.Size = new Size(this.Size.Width, base_y + y + 20);
            base.NotificationFinalize();
            this.ResumeForm();
            skip_event = false;
        }

        private string command_start = "npc" + MainForm.commandSymbol;
        private bool clicked = false;
        void openItemBox(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand(command_start + (sender as Control).Name);
            clicked = false;
        }

        private string switch_start = "drop" + MainForm.commandSymbol;
        private void statsButton_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand(switch_start + (sender as Control).Name);
            clicked = false;
        }

        private bool skip_event = false;
        private void pickupBox_CheckedChanged(object sender, EventArgs e) {
            if (skip_event) return;
            bool is_checked = (sender as CheckBox).Checked;
            this.ReturnFocusToTibia();
            if (is_checked) MainForm.mainForm.ExecuteCommand("pickup" + MainForm.commandSymbol + item.name);
            else MainForm.mainForm.ExecuteCommand("nopickup" + MainForm.commandSymbol + item.name);
        }

        private void convertBox_CheckedChanged(object sender, EventArgs e) {
            if (skip_event) return;
            bool is_checked = (sender as CheckBox).Checked;
            this.ReturnFocusToTibia();
            if (is_checked) MainForm.mainForm.ExecuteCommand("convert" + MainForm.commandSymbol + item.name);
            else MainForm.mainForm.ExecuteCommand("noconvert" + MainForm.commandSymbol + item.name);
        }

        private void valueBox_TextChanged(object sender, EventArgs e) {
            if (skip_event) return;

            string text = (sender as TextBox).Text;
            int new_value;
            if (int.TryParse(text, out new_value)) {
                MainForm.mainForm.ExecuteCommand("setval" + MainForm.commandSymbol + item.name + "=" + new_value);
                previous_value = (sender as TextBox).Text;
            }
        }
    }
}
