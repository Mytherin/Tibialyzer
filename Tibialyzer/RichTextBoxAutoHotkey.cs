using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Tibialyzer {
    class RichTextBoxAutoHotkey : RichTextBox {
        List<string> keywordStrings = new List<string> { "up", "down", "left", "return", "right", "suspend", "numpad", "printscreen", "pause", "capslock", "space", "tab", "enter", "escape", "backspace", "delete", "insert", "home", "end", "pgup", "pgdn", "lbutton", "rbutton", "mbutton", "xbutton1", "xbutton2", "wheelup", "wheeldown" };
        List<string> modifierStrings = new List<string> { "ctrl+", "shift+", "alt+", "command=" };
        List<string> operatorStrings = new List<string> { "::" };
        List<Regex> keywordRegexes = new List<Regex> { new Regex("f[0-9]{1,2}") };
        List<Regex> commentRegexes = new List<Regex> { new Regex("#[^\n]*"), new Regex(";[^\n]*") };
        List<Regex> commandRegexes = new List<Regex> { new Regex("[a-z0-9]+@[a-z0-9]*") };
        Color keywordColor = Color.FromArgb(25, 25, 112);
        Color modifierColor = Color.FromArgb(178,34,34);
        Color operatorColor = Color.FromArgb(31, 31, 31);
        Color operatorBackColor = Color.FromArgb(191, 191, 191);
        Color commentColor = Color.FromArgb(34, 139, 34);
        Color commandColor = Color.FromArgb(140, 95, 20);


        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwndLock, Int32 wMsg, Int32 wParam, ref Point pt);
        
        // *** Get / Change Scroll Position ***
        private Point RTBScrollPos {
            get {
                const int EM_GETSCROLLPOS = 0x0400 + 221;
                Point pt = new Point();

                SendMessage(this.Handle, EM_GETSCROLLPOS, 0, ref pt);
                return pt;
            }
            set {
                const int EM_SETSCROLLPOS = 0x0400 + 222;

                SendMessage(this.Handle, EM_SETSCROLLPOS, 0, ref value);
            }
        }
        public RichTextBoxAutoHotkey() {
            this.TextChanged += RichTextBoxAutoHotkey_TextChanged;
        }

        private void RichTextBoxAutoHotkey_TextChanged(object sender, EventArgs e) {
        }

        bool highlighting = false;

        public void RefreshSyntax() {
            highlighted = false;
            OnTextChanged(null);
        }

        private void modifyText(string text, List<string> keywords, Color color, Color backColor, int start, int end) {
            foreach (string keyword in keywords) {
                int index = 0, baseIndex = 0;
                int length = keyword.Length;
                string copy = text;
                while ((index = copy.IndexOf(keyword)) >= 0) {
                    if (baseIndex + index + keyword.Length >= start && baseIndex + index <= end) {
                        this.SelectionStart = baseIndex + index;
                        this.SelectionLength = length;
                        if (backColor != Color.Empty) this.SelectionBackColor = backColor;
                        if (color != Color.Empty) this.SelectionColor = color;
                    }
                    copy = copy.Substring(index + length);
                    baseIndex += index + length;
                }
            }
        }

        private void modifyText(string text, List<Regex> keywords, Color color, Color backColor, int start, int end) {
            foreach (Regex regex in keywords) {
                MatchCollection mc = regex.Matches(text);
                foreach (Match m in mc) {
                    int startIndex = m.Index;
                    if (startIndex + m.Length >= start && startIndex <= end) {
                        int stopIndex = m.Length;
                        this.SelectionStart = startIndex;
                        this.SelectionLength = stopIndex;
                        if (backColor != Color.Empty) this.SelectionBackColor = backColor;
                        if (color != Color.Empty) this.SelectionColor = color;
                    }
                }
            }
        }

        bool highlighted = false;
        protected override void OnTextChanged(EventArgs e) {
            if (highlighting) return;
            base.OnTextChanged(e);
            this.SuspendLayout();
            highlighting = true;
            string text = this.Text.ToLower();

            int start = 0, end = text.Length - 1;

            if (highlighted) {
                start = Math.Max(this.SelectionStart - 25, start);
                end = Math.Min(this.SelectionStart + 25, end);
            } else {
                highlighted = true;
            }
            if (end <= start) {
                highlighting = false;
                this.ResumeLayout();
                return;
            }

            var initialScroll = RTBScrollPos;
            int initialCursorPosition = this.SelectionStart;
            int initialLength = this.SelectionLength;

            this.SelectionStart = start;
            this.SelectionLength = end - start;
            this.SelectionColor = Color.Black;

            modifyText(text, keywordStrings, keywordColor, Color.Empty, start, end);
            modifyText(text, modifierStrings, modifierColor, Color.Empty, start, end);
            modifyText(text, keywordRegexes, keywordColor, Color.Empty, start, end);
            modifyText(text, commentRegexes, commentColor, Color.Empty, start, end);
            modifyText(text, commandRegexes, commandColor, Color.Empty, start, end);

            this.SelectionStart = initialCursorPosition;
            this.SelectionLength = initialLength;
            this.SelectionColor = Color.Black;
            RTBScrollPos = initialScroll;
            highlighting = false;
            this.ResumeLayout();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m) {
            if (m.Msg == 0x00f) {
                if (!highlighting)
                    base.WndProc(ref m);
                else
                    m.Result = IntPtr.Zero;
            } else
                base.WndProc(ref m);
        }
    }
}
