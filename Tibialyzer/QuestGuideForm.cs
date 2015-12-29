using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tibialyzer {
    class QuestGuideForm : NotificationForm {
        private Label questTitle;
        private static Font requirementFont = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


        public HuntingPlace hunt;
        public Directions direction;
        public Quest quest;
        public QuestInstruction questInstruction;
        public int instructionIndex = 0;
        public int minInstructions = 0;
        public int maxInstructions = 0;
        private Label nextButton;
        private Label prevButton;
        public List<Control> addedControls = new List<Control>();
        public QuestGuideForm(Quest q) {
            this.quest = q;
            this.questInstruction = null;
            this.hunt = null;
            this.direction = null;
            maxInstructions = quest.questInstructions.Count;
            if (quest.questInstructions.Count > 0) {
                int ordering = quest.questInstructions[quest.questInstructions.Count - 1].ordering;
                for (int i = quest.questInstructions.Count - 1; i >= 0; i--) {
                    if (quest.questInstructions[i].ordering < ordering) {
                        maxInstructions = i + 2;
                        break;
                    }
                }
            }
            this.InitializeComponent();
        }
        public QuestGuideForm(HuntingPlace h) {
            this.hunt = h;
            this.direction = h.directions[0];
            this.quest = null;
            this.questInstruction = null;
            instructionIndex = 1;
            minInstructions = 1;
            maxInstructions = hunt.directions.Count;
            if (hunt.directions.Count > 0) {
                int ordering = hunt.directions[hunt.directions.Count - 1].ordering;
                for (int i = hunt.directions.Count - 1; i >= 0; i--) {
                    if (hunt.directions[i].ordering < ordering) {
                        maxInstructions = i + 2;
                        break;
                    }
                }
            }
            this.InitializeComponent();
        }

        private void InitializeComponent() {
            this.questTitle = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Label();
            this.prevButton = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // questTitle
            // 
            this.questTitle.BackColor = System.Drawing.Color.Transparent;
            this.questTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.questTitle.Location = new System.Drawing.Point(12, 30);
            this.questTitle.Name = "questTitle";
            this.questTitle.Size = new System.Drawing.Size(300, 16);
            this.questTitle.TabIndex = 5;
            this.questTitle.Text = "Quest Name";
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.Transparent;
            this.nextButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.nextButton.Location = new System.Drawing.Point(216, 46);
            this.nextButton.Name = "nextButton";
            this.nextButton.Padding = new System.Windows.Forms.Padding(2);
            this.nextButton.Size = new System.Drawing.Size(96, 21);
            this.nextButton.TabIndex = 27;
            this.nextButton.Text = "Next Step";
            this.nextButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.nextButton.Visible = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.BackColor = System.Drawing.Color.Transparent;
            this.prevButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.prevButton.Location = new System.Drawing.Point(114, 46);
            this.prevButton.Name = "prevButton";
            this.prevButton.Padding = new System.Windows.Forms.Padding(2);
            this.prevButton.Size = new System.Drawing.Size(96, 21);
            this.prevButton.TabIndex = 28;
            this.prevButton.Text = "Prev Step";
            this.prevButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.prevButton.Visible = false;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // QuestGuideForm
            // 
            this.ClientSize = new System.Drawing.Size(392, 75);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.questTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QuestGuideForm";
            this.Load += new System.EventHandler(this.QuestGuideForm_Load);
            this.ResumeLayout(false);

        }

        private void setupGuide() {
            foreach (Control c in addedControls) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            addedControls.Clear();

            int y = this.questTitle.Location.Y + 40;
            if (questInstruction == null && hunt == null) {
                if (this.quest.additionalRequirements.Count > 0 || this.quest.questRequirements.Count > 0) {
                    Label label = new Label();
                    label.Text = "Requirements";
                    label.Location = new Point(5, y);
                    label.ForeColor = MainForm.label_text_color;
                    label.BackColor = Color.Transparent;
                    label.Font = questTitle.Font;
                    label.Size = new Size(this.Size.Width - 10, label.Height);
                    addedControls.Add(label);
                    this.Controls.Add(label);
                    y += 25;

                    // Item requirements
                    if (this.quest.questRequirements.Count > 0) {

                        List<TibiaObject> rewards = this.quest.questRequirements.OrderBy(o => o.Item1 * o.Item2.GetMaxValue()).Select(o => o.Item2).ToList<TibiaObject>();

                        List<Control> itemControls = new List<Control>();
                        y = y + MainForm.DisplayCreatureList(this.Controls, rewards, 10, y, this.Size.Width - 10, 1, false, null, 1, itemControls);
                        foreach (Control control in itemControls) {
                            addedControls.Add(control);
                            control.BackgroundImage = MainForm.item_background;
                            control.Click += itemClick;
                        }
                    }

                    // Text requirements
                    if (this.quest.additionalRequirements.Count > 0) {
                        List<string> requirementStrings = this.quest.additionalRequirements.ToArray().ToList();
                        if (this.quest.minlevel > 0) {
                            requirementStrings.Add(String.Format("You must be at least level {0}.", this.quest.minlevel));
                        }

                        y += 5;
                        foreach (string text in requirementStrings) {
                            label = new Label();
                            label.Text = text == "" ? "" : "- " + text;
                            label.Location = new Point(5, y);
                            label.ForeColor = MainForm.label_text_color;
                            label.BackColor = Color.Transparent;
                            label.Font = requirementFont;
                            Size size;
                            using (Graphics gr = Graphics.FromHwnd(label.Handle)) {
                                size = gr.MeasureString(label.Text, label.Font, this.Size.Width - 10).ToSize();
                                label.Size = new Size(this.Size.Width - 10, size.Height + 2);
                            }
                            addedControls.Add(label);
                            this.Controls.Add(label);
                            y += size.Height + 2;
                        }
                    }
                }
            } else {
                List<Coordinate> begin = new List<Coordinate>();
                List<Coordinate> end = new List<Coordinate>();
                List<string> description = new List<string>();
                if (questInstruction != null) {
                    int ordering = questInstruction.ordering;
                    int currentIndex = instructionIndex - 1;
                    while (currentIndex < quest.questInstructions.Count && quest.questInstructions[currentIndex].ordering == ordering) {
                        begin.Add(quest.questInstructions[currentIndex].begin);
                        end.Add(quest.questInstructions[currentIndex].end);
                        description.Add(quest.questInstructions[currentIndex].description);
                        currentIndex++;
                    }
                } else {
                    int ordering = direction.ordering;
                    int currentIndex = instructionIndex - 1;
                    while (currentIndex < hunt.directions.Count && hunt.directions[currentIndex].ordering == ordering) {
                        begin.Add(hunt.directions[currentIndex].begin);
                        end.Add(hunt.directions[currentIndex].end);
                        description.Add(hunt.directions[currentIndex].description);
                        currentIndex++;
                    }
                }
                bool noText = true;
                if (description.Count > 1) {
                    for (int i = 1; i < description.Count; i++) {
                        string str = description[i];
                        if (str != "") {
                            noText = false;
                            continue;
                        }
                    }
                    if (noText && description[0] != "") {
                        Label label = new Label();
                        label.Text = description[0];
                        label.Location = new Point(5, y);
                        label.ForeColor = MainForm.label_text_color;
                        label.BackColor = Color.Transparent;
                        label.Font = requirementFont;
                        label.AutoSize = true;
                        label.MaximumSize = new Size(this.Size.Width - 10, 0);
                        int labelHeight = 0;
                        using (Graphics gr = Graphics.FromHwnd(label.Handle)) {
                            labelHeight = (int)(gr.MeasureString(label.Text, label.Font, this.Size.Width - 10).Height * 1.2);
                        }
                        addedControls.Add(label);
                        this.Controls.Add(label);
                        y += labelHeight;
                    }
                } else {
                    noText = false;
                }
                int startX = 5;
                int maxY = 0;
                for (int i = 0; i < begin.Count; i++) {
                    int xOffset;
                    int newY = drawDirections(begin[i], end[i], description[i], startX, y, begin.Count > 1, begin.Count, noText, out xOffset);
                    if (noText) {
                        startX += xOffset;
                        if (newY > maxY) {
                            maxY = newY;
                        }
                        if (startX + 120 > this.Size.Width) {
                            startX = 5;
                            y = maxY;
                        }
                        if (i == begin.Count - 1) {
                            y = maxY;
                        }
                    } else {
                        y = newY;
                    }
                }
            }

            if (instructionIndex > minInstructions || maxInstructions > instructionIndex) {
                y += 5;
                if (maxInstructions > instructionIndex) {
                    nextButton.Location = new Point(this.Size.Width - 105, y);
                    nextButton.Visible = true;
                } else {
                    nextButton.Visible = false;
                }
                if (instructionIndex > minInstructions) {
                    prevButton.Location = new Point(5, y);
                    prevButton.Visible = true;
                } else {
                    prevButton.Visible = false;
                }
                y += 20;
            }

            this.Size = new Size(this.Size.Width, y + 20);
            refreshTimer();
        }

        private int drawDirections(Coordinate begin, Coordinate end, string description, int start_x, int y, bool variableSize, int imageCount, bool noText, out int width) {
            const int mapSize = 180;
            Size minSize = new Size(120, 120);

            PictureBox map = MainForm.DrawRoute(begin, end, variableSize ? new Size(0, 0) : new Size(mapSize, mapSize), minSize, new Size(mapSize, mapSize));
            width = map.Width + 5;
            if (!noText) {
                map.Location = new Point(this.Size.Width - (map.Width + 5), y);
            } else {
                map.Location = new Point(start_x, y);
            }

            this.Controls.Add(map);
            addedControls.Add(map);
            if (noText) {
                return y + map.Height + 5;
            }
            if (description.Contains("@")) {
                int x = 5;
                int minheightoffset = 20;
                string[] questStrings = description.Split('@');
                int minY = y + map.Size.Height + 10;
                foreach (string instruction in questStrings) {
                    if (instruction.Contains("=")) {
                        string[] splits = instruction.Split('=');
                        if (splits[0].ToLower() == "cr" || splits[0].ToLower() == "npc" || splits[0].ToLower() == "item") {
                            bool blockWidth = true;
                            string imageString = splits[1];
                            if (splits[1].Contains(';')) {
                                string[] options = splits[1].Split(';');
                                imageString = options[0];
                                for (int i = 1; i < options.Length; i++) {
                                    if (options[i].ToLower() == "blockheight") {
                                        blockWidth = false;
                                    }
                                }
                            }
                            string command = "";
                            Image image = null;
                            if (splits[0].ToLower() == "cr") {
                                Creature cr = MainForm.getCreature(imageString);
                                image = cr.image;
                                command = "creature" + MainForm.commandSymbol + cr.name.ToLower();
                            } else if (splits[0].ToLower() == "npc") {
                                NPC npc = MainForm.getNPC(imageString);
                                image = npc.image;
                                command = "npc" + MainForm.commandSymbol + npc.name.ToLower();
                            } else if (splits[0].ToLower() == "item") {
                                Item item = MainForm.getItem(imageString);
                                image = item.image;
                                command = "item" + MainForm.commandSymbol + item.name.ToLower();
                            }
                            PictureBox pictureBox = new PictureBox();
                            pictureBox.Location = new Point(x, y);
                            pictureBox.Image = image;
                            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox.Size = new Size(image.Width, image.Height);
                            pictureBox.BackColor = Color.Transparent;
                            pictureBox.Name = command;
                            pictureBox.Click += QuestTitle_Click;
                            if (blockWidth) {
                                x += pictureBox.Size.Width;
                                minheightoffset = pictureBox.Size.Height + 5;
                            } else {
                                y += pictureBox.Size.Height;
                            }

                            addedControls.Add(pictureBox);
                            this.Controls.Add(pictureBox);
                            continue;
                        }
                    }
                    Label label = new Label();
                    label.Text = instruction == "" ? "" : "- " + instruction;
                    label.Location = new Point(x, y);
                    label.ForeColor = MainForm.label_text_color;
                    label.BackColor = Color.Transparent;
                    label.Font = requirementFont;
                    label.AutoSize = true;
                    label.MaximumSize = new Size(this.Size.Width - (map.Size.Width) - x, 0);
                    int labelHeight = 0;
                    using (Graphics gr = Graphics.FromHwnd(label.Handle)) {
                        labelHeight = (int)(gr.MeasureString(label.Text, label.Font, this.Size.Width - (map.Size.Width + 10) - x).Height * 1.2);
                    }
                    addedControls.Add(label);
                    this.Controls.Add(label);
                    y += Math.Max(labelHeight, minheightoffset);
                    minheightoffset = 0;
                    x = 5;
                }
                if (y < minY) y = minY;
            } else {
                Label label = new Label();
                label.Text = description == "" ? "" : "- " + description;
                label.Location = new Point(5, y);
                label.ForeColor = MainForm.label_text_color;
                label.BackColor = Color.Transparent;
                label.Font = requirementFont;
                Size size;
                using (Graphics gr = Graphics.FromHwnd(label.Handle)) {
                    size = gr.MeasureString(label.Text, label.Font, this.Size.Width - (map.Size.Width + 10)).ToSize();
                    label.Size = new Size(this.Size.Width - (map.Size.Width + 5), Math.Max((int)(size.Height * 1.3), map.Size.Height));
                }
                addedControls.Add(label);
                this.Controls.Add(label);
                y += Math.Max(label.Size.Height, map.Size.Height) + 10;
            }
            return y;
        }

        private bool clicked = false;
        private void itemClick(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand("item" + MainForm.commandSymbol + (sender as Control).Name);
        }
        private void QuestTitle_Click(object sender, EventArgs e) {
            if (clicked) return;
            clicked = true;
            this.ReturnFocusToTibia();
            MainForm.mainForm.ExecuteCommand((sender as Control).Name);
        }


        private void QuestGuideForm_Load(object sender, EventArgs e) {
            if (quest == null && hunt == null) return;
            this.SuspendLayout();
            NotificationInitialize();

            this.nextButton.Click -= c_Click;
            this.prevButton.Click -= c_Click;

            if (quest != null) {
                this.questTitle.Text = quest.title;
                this.questTitle.Name = "quest" + MainForm.commandSymbol + quest.name;
            } else {
                this.questTitle.Text = hunt.name;
                this.questTitle.Name = "hunt" + MainForm.commandSymbol + hunt.name;
            }
            this.questTitle.Click += QuestTitle_Click;
            setupGuide();

            base.NotificationFinalize();
            this.ResumeLayout(false);
        }

        private void nextButton_Click(object sender, EventArgs e) {
            if (maxInstructions > instructionIndex) {
                if (quest != null) {
                    if (questInstruction == null) {
                        this.questInstruction = this.quest.questInstructions[instructionIndex++];
                    } else {
                        int ordering = this.questInstruction.ordering;
                        while ((this.questInstruction = quest.questInstructions[instructionIndex++]).ordering == ordering) ;
                    }
                } else {
                    int ordering = this.direction.ordering;
                    while ((this.direction = this.hunt.directions[instructionIndex++]).ordering == ordering) ;
                }
                this.SuspendLayout();
                setupGuide();
                this.ResumeLayout(true);
                this.Refresh();
            }
        }

        private void prevButton_Click(object sender, EventArgs e) {
            if (instructionIndex > minInstructions) {
                instructionIndex--;
                if (instructionIndex == 0) {
                    this.questInstruction = null;
                } else {
                    if (this.quest != null) {
                        this.questInstruction = this.quest.questInstructions[instructionIndex - 1];
                        int ordering = questInstruction.ordering;
                        while (instructionIndex - 2 >= 0 && this.quest.questInstructions[instructionIndex - 2].ordering == ordering) {
                            instructionIndex--;
                            this.questInstruction = this.quest.questInstructions[instructionIndex - 1];
                        }
                    } else {
                        this.direction = this.hunt.directions[instructionIndex - 1];
                        int ordering = direction.ordering;
                        while (instructionIndex - 2 >= 0 && this.hunt.directions[instructionIndex - 2].ordering == ordering) {
                            instructionIndex--;
                            direction = this.hunt.directions[instructionIndex - 1];
                        }
                    }
                }
                this.SuspendLayout();
                setupGuide();
                this.ResumeLayout(true);
                this.Refresh();
            }
        }
    }
}
