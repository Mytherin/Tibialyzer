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
    public partial class HelpTab : Form, TabInterface {
        public HelpTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }
        
        public void InitializeSettings() {

        }

        public void InitializeTooltips() {

        }

        public void LoadHelpTab() {
            HelpTimer_Elapsed(null, null);
        }
        
        private void refreshItems(Control suspend, Control.ControlCollection controls, List<TibiaObject> tibiaObjects, string sortedHeader, bool desc, EventHandler eventHandler, int maxItems = 20) {
            int maxWidth = 0;

            this.SuspendLayout();
            NotificationForm.SuspendDrawing(suspend);
            foreach (Control c in controls) {
                c.Dispose();
            }
            controls.Clear();
            MainForm.DisplayCreatureAttributeList(controls, tibiaObjects, 0, 10, out maxWidth, null, null, 0, maxItems, null, null, null, eventHandler, sortedHeader, desc);
            NotificationForm.ResumeDrawing(suspend);
            this.ResumeLayout(false);
        }

        object helpLock = new object();
        System.Timers.Timer helpTimer = null;
        protected void refreshHelpTimer() {
            lock (helpLock) {
                if (helpTimer != null) {
                    helpTimer.Dispose();
                }
                helpTimer = new System.Timers.Timer(250);
                helpTimer.Elapsed += HelpTimer_Elapsed;
                helpTimer.Enabled = true;
            }
        }

        List<TibiaObject> commands = new List<TibiaObject>();

        private void HelpTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            lock (helpLock) {
                if (helpTimer != null)
                    helpTimer.Dispose();
                helpTimer = null;
                MainForm.mainForm.BeginInvoke((MethodInvoker)delegate {
                    string helpText = searchCommandHelpBox.Text.ToLower();
                    commands.Clear();
                    foreach (HelpCommand command in StorageManager.helpCommands) {
                        if (helpText == "" || command.command.ToLower().Contains(helpText) || command.description.ToLower().Contains(helpText)) {
                            commands.Add(command);
                        }
                    }
                    refreshItems(helpPanel, helpPanel.Controls, commands, helpSortedHeader, helpDesc, sortHelp, 100);
                });
            }
        }

        private string helpSortedHeader = null;
        private bool helpDesc = false;

        private void helpSearchBox_TextChanged(object sender, EventArgs e) {
            refreshHelpTimer();
        }
        private void sortHelp(object sender, EventArgs e) {
            if (helpSortedHeader == (sender as Control).Name) {
                helpDesc = !helpDesc;
            } else {
                helpSortedHeader = (sender as Control).Name;
                helpDesc = false;
            }
            refreshItems(helpPanel, helpPanel.Controls, commands, helpSortedHeader, helpDesc, sortHelp, 100);
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
