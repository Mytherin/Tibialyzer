
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Tibialyzer {
    class NPCForm : NotificationForm {
        public NPC npc = null;
        private System.Windows.Forms.PictureBox mapUpLevel;
        private System.Windows.Forms.PictureBox mapDownLevel;
        private static Font text_font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);


        public NPCForm(int currentPage = 0, int currentDisplay = -1) {
            InitializeComponent();
            this.currentPage = Math.Max(currentPage, 0);
            this.currentControlList = Math.Min(currentDisplay, headers.Length);
        }

        private const int headerLength = 5;
        private string[] headers = { "Sell To", "Buy From", "Spells", "Transport", "Quests" };
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NPCForm));
            this.mapBox = new Tibialyzer.MapPictureBox();
            this.npcImage = new System.Windows.Forms.PictureBox();
            this.creatureName = new System.Windows.Forms.Label();
            this.mapUpLevel = new System.Windows.Forms.PictureBox();
            this.mapDownLevel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // mapBox
            // 
            this.mapBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapBox.Location = new System.Drawing.Point(121, 12);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(195, 190);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            // 
            // npcImage
            // 
            this.npcImage.BackColor = System.Drawing.Color.Transparent;
            this.npcImage.Location = new System.Drawing.Point(12, 45);
            this.npcImage.Name = "npcImage";
            this.npcImage.Size = new System.Drawing.Size(100, 98);
            this.npcImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.npcImage.TabIndex = 1;
            this.npcImage.TabStop = false;
            // 
            // creatureName
            // 
            this.creatureName.BackColor = System.Drawing.Color.Transparent;
            this.creatureName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureName.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.creatureName.Location = new System.Drawing.Point(11, 146);
            this.creatureName.MaximumSize = new System.Drawing.Size(100, 28);
            this.creatureName.Name = "creatureName";
            this.creatureName.Size = new System.Drawing.Size(100, 28);
            this.creatureName.TabIndex = 2;
            this.creatureName.Text = "Rashid";
            this.creatureName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.creatureName.Click += new System.EventHandler(this.creatureName_Click);
            // 
            // mapUpLevel
            // 
            this.mapUpLevel.Location = new System.Drawing.Point(121, 13);
            this.mapUpLevel.Name = "mapUpLevel";
            this.mapUpLevel.Size = new System.Drawing.Size(21, 21);
            this.mapUpLevel.TabIndex = 3;
            this.mapUpLevel.TabStop = false;
            // 
            // mapDownLevel
            // 
            this.mapDownLevel.Location = new System.Drawing.Point(121, 34);
            this.mapDownLevel.Name = "mapDownLevel";
            this.mapDownLevel.Size = new System.Drawing.Size(21, 21);
            this.mapDownLevel.TabIndex = 4;
            this.mapDownLevel.TabStop = false;
            // 
            // NPCForm
            // 
            this.ClientSize = new System.Drawing.Size(328, 209);
            this.Controls.Add(this.mapDownLevel);
            this.Controls.Add(this.mapUpLevel);
            this.Controls.Add(this.creatureName);
            this.Controls.Add(this.npcImage);
            this.Controls.Add(this.mapBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NPCForm";
            this.Text = "NPC Form";
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.npcImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapUpLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapDownLevel)).EndInit();
            this.ResumeLayout(false);

        }

        private MapPictureBox mapBox;
        private System.Windows.Forms.PictureBox npcImage;
        private System.Windows.Forms.Label creatureName;

        protected override bool ShowWithoutActivation {
            get { return true; }
        }

        private List<string>[] removedLists = new List<string>[headerLength];
        private List<TibiaObject>[] objectList = new List<TibiaObject>[headerLength];
        private Control[] objectControls = new Control[headerLength];
        private int currentControlList = -1;
        private int base_y;
        private int currentPage;
        private string[] extraAttributes = new string[headerLength];
        private Func<TibiaObject, Attribute>[] attributeFunctions = new Func<TibiaObject, Attribute>[headerLength];
        private Func<TibiaObject, IComparable>[] attributeSortFunctions = new Func<TibiaObject, IComparable>[headerLength];

        public override void LoadForm() {
            if (npc == null) return;

            this.SuspendForm();
            NotificationInitialize();
            npcImage.Image = npc.GetImage();
            creatureName.Text = MainForm.ToTitle(npc.city);
            Font f = MainForm.fontList[0];
            for (int i = 0; i < MainForm.fontList.Count; i++) {
                Font font = MainForm.fontList[i];
                Size size = TextRenderer.MeasureText(this.creatureName.Text, font);
                if (size.Width < creatureName.MaximumSize.Width && size.Height < creatureName.MaximumSize.Height) {
                    f = font;
                } else {
                    break;
                }
            }
            this.creatureName.Font = f;

            for (int i = 0; i < headers.Length; i++) {
                objectList[i] = new List<TibiaObject>();
            }
            extraAttributes[0] = "Value";
            attributeFunctions[0] = SellPrice;
            attributeSortFunctions[0] = SellSort;
            removedLists[0] = new List<string> { "Value" };
            foreach (ItemSold itemSold in npc.sellItems) {
                objectList[0].Add(new LazyTibiaObject { id = itemSold.itemid, type = TibiaObjectType.Item });
            }
            extraAttributes[1] = "Price";
            attributeFunctions[1] = BuyPrice;
            attributeSortFunctions[1] = BuySort;
            removedLists[1] = new List<string> { "Value" };
            foreach (ItemSold itemSold in npc.buyItems) {
                objectList[1].Add(new LazyTibiaObject { id = itemSold.itemid, type = TibiaObjectType.Item });
            }
            extraAttributes[2] = "Vocation";
            attributeFunctions[2] = SpellVoc;
            attributeSortFunctions[2] = SpellSort;
            removedLists[2] = new List<string> { "Words" };
            foreach (SpellTaught spellTaught in npc.spellsTaught) {
                objectList[2].Add(new LazyTibiaObject { id = spellTaught.spellid, type = TibiaObjectType.Spell });
            }
            // Transport
            foreach (Transport transport in npc.transportOffered) {
                objectList[3].Add(transport);
            }
            // Quests Involved In
            foreach (Quest q in npc.involvedQuests) {
                objectList[4].Add(q);
            }

            base_y = this.Size.Height;
            int x = 5;
            for (int i = 0; i < headers.Length; i++) {
                if (objectList[i].Count > 0) {
                    Label label = new Label();
                    label.Text = headers[i];
                    label.Location = new Point(x, base_y);
                    label.ForeColor = MainForm.label_text_color;
                    label.BackColor = Color.Transparent;
                    label.Font = MainForm.text_font;
                    label.Size = new Size(90, 25);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Name = i.ToString();
                    label.Click += toggleObjectDisplay;
                    objectControls[i] = label;
                    this.Controls.Add(label);
                    if (currentControlList < 0 || currentControlList > headers.Length) {
                        currentControlList = i;
                    }
                    x += 90;
                } else {
                    objectControls[i] = null;
                }
            }
            base_y += 25;

            Map m = MainForm.getMap(npc.pos.z);

            mapBox.map = m;
            mapBox.mapImage = null;

            Target t = new Target();
            t.coordinate = new Coordinate(npc.pos);
            t.image = npc.GetImage();
            t.size = 20;

            mapBox.targets.Add(t);
            mapBox.sourceWidth = mapBox.Width;
            mapBox.mapCoordinate = new Coordinate(npc.pos);
            mapBox.zCoordinate = npc.pos.z;
            mapBox.UpdateMap();

            mapBox.Click -= c_Click;

            this.mapUpLevel.Image = MainForm.mapup_image;
            this.mapUpLevel.Click -= c_Click;
            this.mapUpLevel.Click += mapUpLevel_Click;
            this.mapDownLevel.Image = MainForm.mapdown_image;
            this.mapDownLevel.Click -= c_Click;
            this.mapDownLevel.Click += mapDownLevel_Click;

            refresh();
            base.NotificationFinalize();
            this.ResumeForm();
        }
        private Attribute SellPrice(TibiaObject obj) {
            int npcValue = npc.sellItems.Find(o => o.itemid == (obj as LazyTibiaObject).id).price;
            return new StringAttribute(npcValue.ToString(), 60, npcValue >= obj.AsItem().vendor_value ? Item.GoldColor : Creature.BossColor);
        }
        private IComparable SellSort(TibiaObject obj) {
            return npc.sellItems.Find(o => o.itemid == (obj as LazyTibiaObject).id).price;
        }
        private Attribute BuyPrice(TibiaObject obj) {
            return new StringAttribute(String.Format("{0}", npc.buyItems.Find(o => o.itemid == (obj as LazyTibiaObject).id).price), 60, Item.GoldColor);
        }
        private IComparable BuySort(TibiaObject obj) {
            return npc.buyItems.Find(o => o.itemid == (obj as LazyTibiaObject).id).price;
        }
        private Attribute SpellVoc(TibiaObject obj) {
            SpellTaught spell = npc.spellsTaught.Find(o => o.spellid == (obj as LazyTibiaObject).id);
            string voc = (spell.knight ? "Kn+" : "") + (spell.paladin ? "Pa+" : "") + (spell.druid ? "Dr+" : "") + (spell.sorcerer ? "So+" : "");
            if (voc.Length == 3) {
                voc = (spell.knight ? "Knight" : "") + (spell.paladin ? "Paladin" : "") + (spell.druid ? "Druid" : "") + (spell.sorcerer ? "Sorcerer" : "");
            } else {
                voc = voc.Substring(0, voc.Length - 1);
            }
            return new StringAttribute(voc, 80, Item.GoldColor);
        }
        private IComparable SpellSort(TibiaObject obj) {
            return (SpellVoc(obj) as StringAttribute).value;
        }

        void updateCommand() {
            string[] split = command.command.Split(MainForm.commandSymbol);
            command.command = split[0] + MainForm.commandSymbol + split[1] + MainForm.commandSymbol + currentPage.ToString() + MainForm.commandSymbol + currentControlList.ToString();
        }

        private List<Control> controlList = new List<Control>();
        private void refresh() {
            foreach (Control c in controlList) {
                this.Controls.Remove(c);
                c.Dispose();
            }
            if (currentControlList == -1) {
                return;
            }
            updateCommand();
            for (int i = 0; i < headers.Length; i++) {
                if (objectControls[i] != null) {
                    objectControls[i].Enabled = i != currentControlList;
                    if (i == currentControlList) {
                        (objectControls[i] as Label).BorderStyle = BorderStyle.Fixed3D;
                    } else {
                        (objectControls[i] as Label).BorderStyle = BorderStyle.FixedSingle;
                    }
                }
            }
            controlList.Clear();
            int newwidth;
            MainForm.PageInfo pageInfo = new MainForm.PageInfo(false, false);
            int y = base_y + MainForm.DisplayCreatureAttributeList(this.Controls, objectList[currentControlList], 10, base_y, out newwidth, null, controlList, currentPage, 10, pageInfo, extraAttributes[currentControlList], attributeFunctions[currentControlList], sortHeader, sortedHeader, desc, attributeSortFunctions[currentControlList], removedLists[currentControlList]);
            newwidth = Math.Max(newwidth, this.Size.Width);
            if (pageInfo.prevPage || pageInfo.nextPage) {
                if (pageInfo.prevPage) {
                    PictureBox prevpage = new PictureBox();
                    prevpage.Location = new Point(10, y);
                    prevpage.Size = new Size(97, 23);
                    prevpage.Image = MainForm.prevpage_image;
                    prevpage.BackColor = Color.Transparent;
                    prevpage.SizeMode = PictureBoxSizeMode.Zoom;
                    prevpage.Click += Prevpage_Click; ;
                    this.Controls.Add(prevpage);
                    controlList.Add(prevpage);

                }
                if (pageInfo.nextPage) {
                    PictureBox nextpage = new PictureBox();
                    nextpage.Location = new Point(newwidth - 108, y);
                    nextpage.Size = new Size(98, 23);
                    nextpage.BackColor = Color.Transparent;
                    nextpage.Image = MainForm.nextpage_image;
                    nextpage.SizeMode = PictureBoxSizeMode.Zoom;
                    nextpage.Click += Nextpage_Click; ;
                    this.Controls.Add(nextpage);
                    controlList.Add(nextpage);
                }
                y += 25;
            }
            this.Size = new Size(newwidth, y + 10);
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
        private void toggleObjectDisplay(object sender, EventArgs e) {
            this.currentControlList = int.Parse((sender as Control).Name);
            currentPage = 0;
            this.SuspendForm();
            refresh();
            this.ResumeForm();
        }

        private string sortedHeader = null;
        private bool desc = false;
        public void sortHeader(object sender, EventArgs e) {
            if (sortedHeader == (sender as Control).Name) {
                desc = !desc;
            } else {
                sortedHeader = (sender as Control).Name;
                desc = false;
            }
            this.SuspendForm();
            refresh();
            this.ResumeForm();
        }

        void mapUpLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z--;
            mapBox.UpdateMap();
            base.ResetTimer();
        }

        void mapDownLevel_Click(object sender, EventArgs e) {
            mapBox.mapCoordinate.z++;
            mapBox.UpdateMap();
            base.ResetTimer();
        }

        private void creatureName_Click(object sender, EventArgs e) {
            string city = npc.city;
            MainForm.mainForm.ExecuteCommand("city" + MainForm.commandSymbol + city);
        }
    }
}
