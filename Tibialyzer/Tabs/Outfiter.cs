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
    public partial class Outfiter : Form, TabInterface {
        PictureBox colorPicker;
        private OutfiterOutfit outfit = new OutfiterOutfit();
        private int colorIndex = 0;
        private Dictionary<string, int> outfitMap = new Dictionary<string, int>();
        private Dictionary<string, int> mountMap = new Dictionary<string, int>();

        public Outfiter() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();

            outfit.facing = Facing.Down;

            outfitList.Items.Clear();
            List<string> sortedItems = new List<string>();
            for(int i = 0; i < OutfiterManager.outfiterOutfitNames.Count; i++) {
                string outfit = OutfiterManager.outfiterOutfitNames[i].Replace('_', ' ');
                sortedItems.Add(outfit);
                outfitMap.Add(outfit, i);
            }
            sortedItems.Sort();
            outfitList.Items.AddRange(sortedItems.ToArray());
            sortedItems.Clear();

            mountsList.Items.Clear();
            for (int i = 0; i < OutfiterManager.outfiterMountNames.Count; i++) {
                string mount = OutfiterManager.outfiterMountNames[i].Replace('_', ' ');
                sortedItems.Add(mount);
                mountMap.Add(mount, i);
            }
            mountsList.Items.AddRange(sortedItems.OrderBy(o => o == "None" ? "--" : o).ToArray());
            sortedItems.Clear();


            outfitList.SelectedIndex = 0;
            mountsList.SelectedIndex = 0;

            outfitList.ReadOnly = true;
            mountsList.ReadOnly = true;

            colorPicker = new PictureBox();
            colorPicker.Location = new Point(headColorButton.Location.X + headColorButton.Width + 10, headColorButton.Location.Y);
            colorPicker.Size = new Size(OutfiterManager.OutfitColorBoxSize * OutfiterManager.OutfitColorsPerRow, OutfiterManager.OutfitColorBoxSize * 7);
            colorPicker.Image = null;
            colorPicker.MouseDown += ChangeOutfitColor;
            this.Controls.Add(colorPicker);
            RefreshColorPicker(0);
        }

        private void RefreshColorPicker(int colorIndex) {
            this.colorIndex = colorIndex;
            headColorButton.Enabled = true;
            primaryColorButton.Enabled = true;
            secondaryColorButton.Enabled = true;
            detailColorButton.Enabled = true;
            switch(colorIndex) {
                case 0: headColorButton.Enabled = false; break;
                case 1: primaryColorButton.Enabled = false; break;
                case 2: secondaryColorButton.Enabled = false; break;
                case 3: detailColorButton.Enabled = false; break;
            }
            Image oldImage = colorPicker.Image;
            colorPicker.Image = OutfiterManager.GenerateColorImage(outfit.colors[colorIndex]);
            if (oldImage != null) {
                oldImage.Dispose();
            }
        }

        private void ChangeOutfitColor(object sender, MouseEventArgs e) {
            int index = OutfiterManager.ColorIndex(e.X, e.Y);
            if (index < 0) return;
            outfit.colors[colorIndex] = index;
            RefreshColorPicker(colorIndex);
            RefreshImage();
        }

        public void InitializeSettings() {
        }

        public void ApplyLocalization() {
        }

        public void RefreshImage() {
            Image oldImage = outfiterImageBox.Image;
            outfiterImageBox.Image = outfit.GetImage();
            if (oldImage != null) {
                oldImage.Dispose();
            }
            outfiterCode.Text = outfit.ToString();
        }

        private void outfitList_SelectedIndexChanged(object sender, EventArgs e) {
            if ((sender as ListBox).SelectedIndex < 0) return;
            outfit.outfit = outfitMap[(sender as ListBox).Items[(sender as ListBox).SelectedIndex].ToString()];
            RefreshImage();
        }

        private void mountsList_SelectedIndexChanged(object sender, EventArgs e) {
            if ((sender as ListBox).SelectedIndex < 0) return;
            outfit.mount = mountMap[(sender as ListBox).Items[(sender as ListBox).SelectedIndex].ToString()];
            RefreshImage();
        }

        private void headColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(0);
        }

        private void primaryColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(1);
        }

        private void secondaryColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(2);
        }

        private void detailColorButton_Click(object sender, EventArgs e) {
            RefreshColorPicker(3);
        }

        private void femaleCheckbox_CheckedChanged(object sender, EventArgs e) {
            outfit.gender = (sender as CheckBox).Checked ? Gender.Female : Gender.Male;
            RefreshImage();
        }

        private void addon1Checkbox_CheckedChanged(object sender, EventArgs e) {
            outfit.addon1 = (sender as CheckBox).Checked;
            RefreshImage();
        }

        private void addon2Checkbox_CheckedChanged(object sender, EventArgs e) {
            outfit.addon2 = (sender as CheckBox).Checked;
            RefreshImage();
        }

        private void viewOnTibiaWikiButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("http://tibia.wikia.com/wiki/Outfiter?" + outfit.ToString());
        }

        private void saveOutfitImageButton_Click(object sender, EventArgs e) {

        }

        private void applyButton_Click(object sender, EventArgs e) {
            outfit.FromString(outfiterCode.Text);
            RefreshImage();
        }

        private void rotateLeftButton_Click(object sender, EventArgs e) {
            outfit.Rotate(-1);
            RefreshImage();
        }

        private void rotateRightButton_Click(object sender, EventArgs e) {
            outfit.Rotate(1);
            RefreshImage();
        }
    }
}
