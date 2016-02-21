using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class DatabaseTab : Form, TabInterface {
        public DatabaseTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            CreateRatioDisplay(Constants.DisplayItemList, discardItemsHeader.Location.X + 10, discardItemsHeader.Location.Y + discardItemsHeader.Size.Height + 8, UpdateDiscardRatio, discardLabels);
            UpdateDiscardDisplay();
            CreateRatioDisplay(Constants.ConvertUnstackableItemList, convertUnstackableHeader.Location.X + 10, convertUnstackableHeader.Location.Y + convertUnstackableHeader.Size.Height + 8, UpdateConvertRatio, convertLabels);
            CreateRatioDisplay(Constants.ConvertStackableItemList, convertStackableHeader.Location.X + 10, convertStackableHeader.Location.Y + convertStackableHeader.Size.Height + 8, UpdateConvertRatio, convertLabels);
            UpdateConvertDisplay();
        }

        public void InitializeTooltips() {

        }

        private void CreateRatioDisplay(List<string> itemList, int baseX, int baseY, EventHandler itemClick, List<Control> labelControls) {
            int it = 0;
            foreach (string itemName in itemList) {
                Item item = StorageManager.getItem(itemName);
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = item.image;
                pictureBox.Location = new Point(baseX + it * 52, baseY);
                pictureBox.BackgroundImage = StyleManager.GetImage("item_background.png");
                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Size = new Size(48, 48);
                pictureBox.Name = itemName;
                pictureBox.Click += itemClick;

                double goldRatio = item.GetMaxValue() / item.capacity;
                Label label = new Label();
                label.Text = String.Format(goldRatio < 100 ? "{0:0.#}" : "{0:0.}", goldRatio);
                label.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + pictureBox.Size.Height);
                label.Font = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);
                label.Size = new Size(48, 24);
                label.ForeColor = StyleManager.MainFormButtonColor;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Name = itemName;
                labelControls.Add(label);

                this.Controls.Add(pictureBox);
                this.Controls.Add(label);
                it++;
            }
        }

        private void UpdateDiscardRatio(object sender, EventArgs e) {
            string itemName = (sender as Control).Name;
            Item item = StorageManager.getItem(itemName);
            double ratio = item.GetMaxValue() / item.capacity;
            CommandManager.ExecuteCommand("setdiscardgoldratio" + Constants.CommandSymbol + Math.Floor(ratio));
            UpdateDiscardDisplay();
        }

        private List<Control> discardLabels = new List<Control>();
        private void UpdateDiscardDisplay() {
            foreach (Control c in discardLabels) {
                string itemName = c.Name;
                Item item = StorageManager.getItem(itemName);
                if (item.discard) {
                    c.BackColor = StyleManager.DatabaseDiscardColor;
                } else {
                    c.BackColor = StyleManager.DatabaseNoDiscardColor;
                }
            }
        }

        private void UpdateConvertRatio(object sender, EventArgs e) {
            string itemName = (sender as Control).Name;
            Item item = StorageManager.getItem(itemName);
            double ratio = item.GetMaxValue() / item.capacity;
            CommandManager.ExecuteCommand("setconvertgoldratio" + Constants.CommandSymbol + (item.stackable ? "1-" : "0-") + Math.Ceiling(ratio + 0.01));
            UpdateConvertDisplay();
        }

        private List<Control> convertLabels = new List<Control>();
        private void UpdateConvertDisplay() {
            foreach (Control c in convertLabels) {
                string itemName = c.Name;
                Item item = StorageManager.getItem(itemName);
                if (item.convert_to_gold) {
                    c.BackColor = StyleManager.ItemGoldColor;
                } else {
                    c.BackColor = StyleManager.DatabaseNoConvertColor;
                }
            }
        }
        private void applyDiscardRatioButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customDiscardRatioBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                CommandManager.ExecuteCommand("setdiscardgoldratio" + Constants.CommandSymbol + Math.Floor(ratio));
                UpdateDiscardDisplay();
            }
        }

        private void customDiscardRatioBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                applyDiscardRatioButton_Click(null, null);
                e.Handled = true;
            }
        }

        private void applyConvertRatioButton_Click(object sender, EventArgs e) {
            double ratio;
            if (double.TryParse(customConvertRatioBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ratio)) {
                CommandManager.ExecuteCommand("setconvertgoldratio" + Constants.CommandSymbol + "0-" + Math.Floor(ratio));
                CommandManager.ExecuteCommand("setconvertgoldratio" + Constants.CommandSymbol + "1-" + Math.Floor(ratio));
                UpdateConvertDisplay();
            }
        }

        private void customConvertRatioBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                applyConvertRatioButton_Click(null, null);
                e.Handled = true;
            }
        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormHoverColor;
            (sender as Control).ForeColor = StyleManager.MainFormHoverForeColor;
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = StyleManager.MainFormButtonColor;
            (sender as Control).ForeColor = StyleManager.MainFormButtonForeColor;
        }
    }
}
