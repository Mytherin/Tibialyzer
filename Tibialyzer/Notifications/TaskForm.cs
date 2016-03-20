using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class TaskForm : NotificationForm {
        public Task task;

        private System.Windows.Forms.PictureBox mapDownLevel;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private System.Windows.Forms.Label taskName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label bossInfoLabel;
        private System.Windows.Forms.Label pointsLabel;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.PictureBox bossPictureBox;
        private System.Windows.Forms.Label bossNameLabel;
        private System.Windows.Forms.Label taskGroupLabel;
        private System.Windows.Forms.Label huntLabel;
        private System.Windows.Forms.Label creatureLabel;
        private Label routeButton;
        private MapPictureBox mapBox;

        public TaskForm(Task t) {
            this.task = t;
            this.InitializeComponent();
        }

        private void InitializeComponent() {
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.taskName = new System.Windows.Forms.Label();
            this.mapBox = new Tibialyzer.MapPictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bossInfoLabel = new System.Windows.Forms.Label();
            this.pointsLabel = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            this.bossPictureBox = new System.Windows.Forms.PictureBox();
            this.bossNameLabel = new System.Windows.Forms.Label();
            this.taskGroupLabel = new System.Windows.Forms.Label();
            this.creatureLabel = new System.Windows.Forms.Label();
            this.huntLabel = new System.Windows.Forms.Label();
            this.routeButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bossPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mapDownLevel
            // 
            this.mapDownLevel.Location = new System.Drawing.Point(168, 33);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 11;
            this.mapDownLevel.TabStop = false;
            this.mapDownLevel.Click += new System.EventHandler(this.mapDownLevel_Click);
            // 
            // mapUpLevel
            // 
            this.mapUpLevel.Location = new System.Drawing.Point(168, 12);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 10;
            this.mapUpLevel.TabStop = false;
            this.mapUpLevel.Click += new System.EventHandler(this.mapUpLevel_Click);
            // 
            // taskName
            // 
            this.taskName.BackColor = System.Drawing.Color.Transparent;
            this.taskName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.taskName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.taskName.Location = new System.Drawing.Point(4, 27);
            this.taskName.Name = "taskName";
            this.taskName.Size = new System.Drawing.Size(158, 34);
            this.taskName.TabIndex = 8;
            this.taskName.Text = "Crystal Spiders";
            this.taskName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // mapBox
            // 
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapBox.Location = new System.Drawing.Point(168, 12);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(195, 190);
            this.mapBox.TabIndex = 7;
            this.mapBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label1.Location = new System.Drawing.Point(7, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Points";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.label2.Location = new System.Drawing.Point(7, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Count";
            // 
            // bossInfoLabel
            // 
            this.bossInfoLabel.AutoSize = true;
            this.bossInfoLabel.BackColor = System.Drawing.Color.Transparent;
            this.bossInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bossInfoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.bossInfoLabel.Location = new System.Drawing.Point(8, 114);
            this.bossInfoLabel.Name = "bossInfoLabel";
            this.bossInfoLabel.Size = new System.Drawing.Size(38, 15);
            this.bossInfoLabel.TabIndex = 14;
            this.bossInfoLabel.Text = "Boss";
            // 
            // pointsLabel
            // 
            this.pointsLabel.BackColor = System.Drawing.Color.Transparent;
            this.pointsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.pointsLabel.Location = new System.Drawing.Point(92, 79);
            this.pointsLabel.Name = "pointsLabel";
            this.pointsLabel.Size = new System.Drawing.Size(70, 15);
            this.pointsLabel.TabIndex = 15;
            this.pointsLabel.Text = "1";
            this.pointsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // countLabel
            // 
            this.countLabel.BackColor = System.Drawing.Color.Transparent;
            this.countLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.countLabel.Location = new System.Drawing.Point(92, 96);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(70, 15);
            this.countLabel.TabIndex = 16;
            this.countLabel.Text = "1";
            this.countLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bossPictureBox
            // 
            this.bossPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.bossPictureBox.Location = new System.Drawing.Point(12, 133);
            this.bossPictureBox.Name = "bossPictureBox";
            this.bossPictureBox.Size = new System.Drawing.Size(48, 48);
            this.bossPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bossPictureBox.TabIndex = 18;
            this.bossPictureBox.TabStop = false;
            this.bossPictureBox.Click += new System.EventHandler(this.bossPictureBox_Click);
            // 
            // bossNameLabel
            // 
            this.bossNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.bossNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bossNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.bossNameLabel.Location = new System.Drawing.Point(62, 133);
            this.bossNameLabel.MaximumSize = new System.Drawing.Size(100, 48);
            this.bossNameLabel.Name = "bossNameLabel";
            this.bossNameLabel.Size = new System.Drawing.Size(100, 48);
            this.bossNameLabel.TabIndex = 19;
            this.bossNameLabel.Text = "Hide";
            this.bossNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bossNameLabel.Click += new System.EventHandler(this.bossNameLabel_Click);
            // 
            // taskGroupLabel
            // 
            this.taskGroupLabel.BackColor = System.Drawing.Color.Transparent;
            this.taskGroupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.taskGroupLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.taskGroupLabel.Location = new System.Drawing.Point(10, 53);
            this.taskGroupLabel.Name = "taskGroupLabel";
            this.taskGroupLabel.Size = new System.Drawing.Size(150, 15);
            this.taskGroupLabel.TabIndex = 20;
            this.taskGroupLabel.Text = "Points";
            this.taskGroupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.taskGroupLabel.Click += new System.EventHandler(this.taskGroupLabel_Click);
            // 
            // creatureLabel
            // 
            this.creatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.creatureLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.creatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.creatureLabel.Location = new System.Drawing.Point(13, 205);
            this.creatureLabel.Name = "creatureLabel";
            this.creatureLabel.Size = new System.Drawing.Size(96, 25);
            this.creatureLabel.TabIndex = 21;
            this.creatureLabel.Text = "Creatures";
            this.creatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.creatureLabel.Click += new System.EventHandler(this.creatureLabel_Click);
            // 
            // huntLabel
            // 
            this.huntLabel.BackColor = System.Drawing.Color.Transparent;
            this.huntLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.huntLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.huntLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.huntLabel.Location = new System.Drawing.Point(115, 205);
            this.huntLabel.Name = "huntLabel";
            this.huntLabel.Size = new System.Drawing.Size(96, 25);
            this.huntLabel.TabIndex = 22;
            this.huntLabel.Text = "Hunts";
            this.huntLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.huntLabel.Click += new System.EventHandler(this.huntLabel_Click);
            // 
            // routeButton
            // 
            this.routeButton.BackColor = System.Drawing.Color.Transparent;
            this.routeButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.routeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.routeButton.Location = new System.Drawing.Point(216, 205);
            this.routeButton.Name = "routeButton";
            this.routeButton.Padding = new System.Windows.Forms.Padding(2);
            this.routeButton.Size = new System.Drawing.Size(96, 25);
            this.routeButton.TabIndex = 23;
            this.routeButton.Text = "Route";
            this.routeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.routeButton.Click += new System.EventHandler(this.routeButton_Click);
            // 
            // TaskForm
            // 
            this.ClientSize = new System.Drawing.Size(378, 233);
            this.Controls.Add(this.routeButton);
            this.Controls.Add(this.huntLabel);
            this.Controls.Add(this.creatureLabel);
            this.Controls.Add(this.taskGroupLabel);
            this.Controls.Add(this.bossNameLabel);
            this.Controls.Add(this.bossPictureBox);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.pointsLabel);
            this.Controls.Add(this.bossInfoLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.taskName);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TaskForm";
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bossPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        int baseWidth, baseHeight;
        List<Control> createdControls = new List<Control>();
        public override void LoadForm() {
            this.SuspendForm();
            this.NotificationInitialize();
            huntLabel.Click -= c_Click;
            creatureLabel.Click -= c_Click;
            bossNameLabel.Click -= c_Click;
            bossPictureBox.Click -= c_Click;
            mapUpLevel.Click -= c_Click;
            mapDownLevel.Click -= c_Click;
            mapUpLevel.Image = StyleManager.GetImage("mapup.png");
            mapDownLevel.Image = StyleManager.GetImage("mapdown.png");
            mapBox.MapUpdated += MapBox_MapUpdated;

            pointsLabel.Text = task.taskpoints > 0 ? task.taskpoints.ToString() : "-";
            countLabel.Text = task.count.ToString();
            taskName.Text = task.GetName();
            Creature boss = task.GetBoss();
            if (boss != null) {
                bossNameLabel.Text = boss.GetName();
                bossPictureBox.Image = boss.GetImage();
                mapBox.targets.Add(new Target { coordinate = new Coordinate(task.bossposition), image = boss.GetImage(), size = 24 });
            } else {
                bossNameLabel.Visible = false;
                bossPictureBox.Visible = false;
                bossInfoLabel.Visible = false;
            }
            mapBox.mapCoordinate = new Coordinate(task.bossposition);
            mapBox.map = StorageManager.getMap(task.bossposition.z);
            mapBox.sourceWidth = mapBox.Width;
            mapBox.Click -= c_Click;
            mapBox.UpdateMap();
            taskGroupLabel.Text = task.groupname;
            baseWidth = this.Size.Width;
            baseHeight = this.Size.Height;
            refreshAttributes();

            this.NotificationFinalize();
            this.ResumeForm();
        }

        private bool viewCreatures = true;
        private void refreshAttributes() {
            foreach(Control c in createdControls) {
                c.Dispose();
                this.Controls.Remove(c);
            }
            createdControls.Clear();
            List<TibiaObject> taskCreatures = new List<TibiaObject>();
            if (viewCreatures) {
                foreach (int creatureid in task.creatures) {
                    taskCreatures.Add(StorageManager.getCreature(creatureid));
                }
            } else {
                foreach (int huntid in task.hunts) {
                    taskCreatures.Add(StorageManager.getHunt(huntid));
                }
            }
            int newWidth;
            int y = UIManager.DisplayCreatureAttributeList(this.Controls, taskCreatures, 5, baseHeight, out newWidth, null, createdControls);
            this.Size = new System.Drawing.Size(Math.Max(newWidth, baseWidth), baseHeight + y);
        }

        private void MapBox_MapUpdated() {
            refreshTimer();
        }

        private void bossNameLabel_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("creature" + Constants.CommandSymbol + task.GetBoss().GetName());
        }

        private void bossPictureBox_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("creature" + Constants.CommandSymbol + task.GetBoss().GetName());
        }

        private void mapUpLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z--;
            mapBox.UpdateMap();
        }

        private void mapDownLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z++;
            mapBox.UpdateMap();
        }

        private void taskGroupLabel_Click(object sender, EventArgs e) {
            CommandManager.ExecuteCommand("task" + Constants.CommandSymbol + task.groupname);
        }

        private void creatureLabel_Click(object sender, EventArgs e) {
            viewCreatures = true;
            this.SuspendForm();
            refreshAttributes();
            this.ResumeForm();
        }

        private void huntLabel_Click(object sender, EventArgs e) {
            viewCreatures = false;
            this.SuspendForm();
            refreshAttributes();
            this.ResumeForm();
        }

        private void routeButton_Click(object sender, EventArgs e) {
            if (task.bossposition != null) {
                CommandManager.ExecuteCommand(String.Format("route{0}{1},{2},{3}{0}{4}", Constants.CommandSymbol, task.bossposition.x, task.bossposition.y, task.bossposition.z, task.GetBoss() == null ? "" : task.GetBoss().title));
            }
        }

        public override string FormName() {
            return "TaskForm";
        }
    }
}
