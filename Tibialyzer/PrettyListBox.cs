using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Tibialyzer {
    public delegate bool VerifyItem(string item);
    class PrettyListBox : ListBox {
        public event EventHandler ItemsChanged;
        public event EventHandler AttemptDeleteItem;
        public event EventHandler AttemptNewItem;
        public VerifyItem verifyItem = null;
        
        public bool ReadOnly = false;
        public bool ChangeTextOnly = false;
        public HorizontalAlignment TextAlign = HorizontalAlignment.Center;

        public PrettyListBox() {

            this.DrawItem += PrettyListBox_DrawItem;
            this.KeyDown += PrettyListBox_KeyDown;
            this.KeyPress += PrettyListBox_KeyPress;
            this.ItemsChanged += PrettyListBox_ItemsChanged;
        }

        public void RefreshControl() {
            if (ReadOnly) return;
            if (ChangeTextOnly) return;
            if (this.Items.Count == 0 || (this.Items[this.Items.Count - 1].ToString() != "")) {
                this.Items.Add("");
            }
            while (this.Items.Count > 1) {
                if (this.Items[this.Items.Count - 1].ToString() == "" && this.Items[this.Items.Count - 2].ToString() == "") {
                    this.Items.RemoveAt(this.Items.Count - 1);
                } else {
                    break;
                }
            }
        }

        private void PrettyListBox_ItemsChanged(object sender, EventArgs e) {
            RefreshControl();
        }

        private void PrettyListBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (ReadOnly) return;
            if (this.SelectedIndex >= 0) {
                if (e.KeyChar >= 32 && e.KeyChar <= 126) {
                    this.Items[this.SelectedIndex] = this.Items[this.SelectedIndex].ToString() + e.KeyChar;
                    e.Handled = true;
                    if (ItemsChanged != null) {
                        ItemsChanged.Invoke(this, null);
                    }
                }
            }
        }
        
        private void PrettyListBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete && this.SelectedIndex >= 0) {
                if (AttemptDeleteItem != null) {
                    AttemptDeleteItem.Invoke(this, null);
                }
                if (!ChangeTextOnly && !ReadOnly) {
                    this.Items.RemoveAt(this.SelectedIndex);
                    this.Refresh();
                    if (ItemsChanged != null) {
                        ItemsChanged.Invoke(this, null);
                    }
                }
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Enter) {
                if (AttemptNewItem != null) {
                    AttemptNewItem.Invoke(this, null);
                }
                if (!ChangeTextOnly && !ReadOnly) {
                    if (ItemsChanged != null) {
                        ItemsChanged.Invoke(this, null);
                    }
                    this.SelectedIndex = this.Items.Count - 1;
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Back && !ReadOnly) {
                if (e.Modifiers == Keys.Control) {
                    this.Items[this.SelectedIndex] = "";
                } else {
                    string str = this.Items[this.SelectedIndex].ToString();
                    if (str.Length > 0) {
                        this.Items[this.SelectedIndex] = str.Substring(0, str.Length - 1);
                    }
                }
                e.Handled = true;
                if (ItemsChanged != null) {
                    ItemsChanged.Invoke(this, null);
                }
            } else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control && !ChangeTextOnly && !ReadOnly) {
                if (Clipboard.ContainsText()) {
                    string text = Clipboard.GetText();
                    this.Items[this.SelectedIndex] = text.Replace("\n", "");
                    if (ItemsChanged != null) {
                        ItemsChanged.Invoke(this, null);
                    }
                }
            } else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control) {
                Clipboard.SetText(this.Items[this.SelectedIndex].ToString());
            }
        }


        private static Brush HoverBrush = new SolidBrush(Color.FromArgb(43, 47, 51));
        private static Brush ColorBrush = new SolidBrush(MainForm.ButtonForeColor);
        private static Brush HoverForeBrush = new SolidBrush(MainForm.HoverForeColor);
        private static Brush ErrorBrush = new SolidBrush(Color.FromArgb(174, 33, 33));
        private void PrettyListBox_DrawItem(object sender, DrawItemEventArgs e) {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            if (e.Index < 0) {
                return;
            }
            Brush brush = ColorBrush;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                e.Graphics.FillRectangle(HoverBrush, e.Bounds);
                brush = HoverForeBrush;
            }
            // Draw the current item text based on the current Font  
            // and the custom brush settings.
            string str = (sender as ListBox).Items[e.Index].ToString();
            if (verifyItem != null && !verifyItem(str)) {
                brush = ErrorBrush;
            }
            SizeF size = e.Graphics.MeasureString(str, e.Font, e.Bounds.Width, StringFormat.GenericDefault);
            Rectangle newBounds = e.Bounds;
            if (TextAlign == HorizontalAlignment.Center) {
                int halfWidth = (int)Math.Max(0, (e.Bounds.Width - size.Width) / 2.0);
                int halfHeight = (int)Math.Max(0, (e.Bounds.Height - size.Height) / 2.0);
                newBounds = new Rectangle(e.Bounds.X + halfWidth, e.Bounds.Y + halfHeight, e.Bounds.Width - halfWidth, e.Bounds.Height - halfHeight);
            }
            e.Graphics.DrawString(str, e.Font, brush, newBounds, StringFormat.GenericDefault);
        }
    }
}
