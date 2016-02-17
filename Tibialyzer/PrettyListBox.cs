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

        private object timerLock = new object();
        private System.Timers.Timer flickerTimer = null;
        private bool caret_flicker = true;
        private int caretOffset = 0;
        public PrettyListBox() {
            this.MeasureItem += PrettyListBox_MeasureItem;
            this.DrawItem += PrettyListBox_DrawItem;
            this.KeyDown += PrettyListBox_KeyDown;
            this.KeyPress += PrettyListBox_KeyPress;
            this.ItemsChanged += PrettyListBox_ItemsChanged;
            this.GotFocus += PrettyListBox_GotFocus;

            flickerTimer = new System.Timers.Timer(250);
            flickerTimer.Elapsed += FlickerTimer_Elapsed;
        }


        private void PrettyListBox_GotFocus(object sender, EventArgs e) {
            if (ReadOnly) return;
            lock (timerLock) {
                flickerTimer.Start();
            }
        }

        private void FlickerTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            try {
                this.Invoke((MethodInvoker)delegate {
                    lock (timerLock) {
                        if (this.Focused) {
                            this.Refresh();
                            flickerTimer.Start();
                        } else {
                            flickerTimer.Stop();
                        }
                    }
                });
            } catch {

            }
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
                    string currentString = this.Items[this.SelectedIndex].ToString();
                    int caretPosition = CaretPosition(currentString);
                    this.Items[this.SelectedIndex] = currentString.Substring(0, caretPosition) + e.KeyChar + currentString.Substring(caretPosition, currentString.Length - caretPosition);
                    e.Handled = true;
                    if (ItemsChanged != null) {
                        ItemsChanged.Invoke(this, null);
                    }
                }
            }
        }

        private int CaretPosition(string currentString) {
            caretOffset = Math.Min(Math.Max(caretOffset, 0), currentString.Length);
            return currentString.Length - caretOffset;
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
            } else if (e.KeyCode == Keys.Back && !ReadOnly) {
                if (e.Modifiers == Keys.Control) {
                    this.Items[this.SelectedIndex] = "";
                } else {
                    string itemString = this.Items[this.SelectedIndex].ToString();
                    if (itemString.Length > 0) {
                        int caretPosition = CaretPosition(itemString);
                        this.Items[this.SelectedIndex] = itemString.Substring(0, caretPosition - 1) + itemString.Substring(caretPosition, itemString.Length - caretPosition);
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
            } else if (e.KeyCode == Keys.Left) {
                caretOffset++;
                e.Handled = true;
            } else if (e.KeyCode == Keys.Right) {
                caretOffset--;
                e.Handled = true;
            } else if (e.KeyCode == Keys.Home) {
                caretOffset = int.MaxValue;
                e.Handled = true;
            } else if (e.KeyCode == Keys.End) {
                caretOffset = 0;
                e.Handled = true;
            }
        }

        private void PrettyListBox_MeasureItem(object sender, MeasureItemEventArgs e) {
            PrettyListBox self = sender as PrettyListBox;
            string str = self.Items[e.Index].ToString();
            e.ItemWidth = self.Width;
            e.ItemHeight = (int)e.Graphics.MeasureString(str, self.Font, e.ItemWidth, StringFormat.GenericDefault).Height;
        }

        private static Brush HoverBrush = new SolidBrush(StyleManager.MainFormHoverColor);
        private static Brush ColorBrush = new SolidBrush(StyleManager.MainFormButtonForeColor);
        private static Brush HoverForeBrush = new SolidBrush(StyleManager.MainFormHoverForeColor);
        private static Brush ErrorBrush = new SolidBrush(StyleManager.MainFormErrorColor);
        private void PrettyListBox_DrawItem(object sender, DrawItemEventArgs e) {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            if (e.Index < 0) {
                return;
            }
            Brush brush = ColorBrush;
            string itemString = (sender as ListBox).Items[e.Index].ToString();
            string displayString = itemString;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                e.Graphics.FillRectangle(HoverBrush, e.Bounds);
                brush = HoverForeBrush;
                if (!ReadOnly) {
                    if (this.Focused) {
                        int caretPosition = CaretPosition(itemString);
                        string caret = caret_flicker ? "/" : "\\";
                        displayString = displayString.Substring(0, caretPosition) + caret + displayString.Substring(caretPosition, displayString.Length - caretPosition);
                        caret_flicker = !caret_flicker;
                    }
                }
            }
            // Draw the current item text based on the current Font  
            // and the custom brush settings.
            if (verifyItem != null && !verifyItem(itemString)) {
                brush = ErrorBrush;
            }
            SizeF size = e.Graphics.MeasureString(displayString, e.Font, e.Bounds.Width, StringFormat.GenericDefault);
            Rectangle newBounds = e.Bounds;
            if (TextAlign == HorizontalAlignment.Center) {
                int halfWidth = (int)Math.Max(0, (e.Bounds.Width - size.Width) / 2.0);
                int halfHeight = (int)Math.Max(0, (e.Bounds.Height - size.Height) / 2.0);
                newBounds = new Rectangle(e.Bounds.X + halfWidth, e.Bounds.Y + halfHeight, e.Bounds.Width - halfWidth, e.Bounds.Height - halfHeight);
            }
            e.Graphics.DrawString(displayString, e.Font, brush, newBounds, StringFormat.GenericDefault);
        }
    }
}
