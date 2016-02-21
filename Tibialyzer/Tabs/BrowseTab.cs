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
    public partial class BrowseTab : Form, TabInterface {
        public BrowseTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            browseTypeBox.SelectedIndex = 0;
        }

        public void InitializeTooltips() {

        }

        private void browseSearch_TextChanged(object sender, EventArgs e) {
            refreshBrowseTimer();
        }

        private void browseSelectionBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (browseTextBox.Text == "") {
                return;
            }
            refreshBrowseTimer();
        }

        private List<TibiaObject> browseObjects = new List<TibiaObject>();
        private string browseSortedHeader = null;
        private bool browseDesc = false;

        object browseLock = new object();
        System.Timers.Timer browseTimer = null;
        protected void refreshBrowseTimer() {
            lock (browseLock) {
                if (browseTimer != null) {
                    browseTimer.Dispose();
                }
                browseTimer = new System.Timers.Timer(250);
                browseTimer.Elapsed += BrowseTimer_Elapsed;
                browseTimer.Enabled = true;
            }
        }


        private void BrowseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            lock (browseLock) {
                browseTimer.Dispose();
                browseTimer = null;
                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    string searchTerm = browseTextBox.Text;
                    switch (browseTypeBox.SelectedIndex) {
                        case 0:
                            browseObjects = StorageManager.searchCreature(searchTerm);
                            break;
                        case 1:
                            browseObjects = StorageManager.searchItem(searchTerm);
                            break;
                        case 2:
                            browseObjects = StorageManager.searchNPC(searchTerm);
                            break;
                        case 3:
                            browseObjects = StorageManager.searchHunt(searchTerm).ToList<TibiaObject>();
                            break;
                        case 4:
                            browseObjects = StorageManager.searchQuest(searchTerm);
                            break;
                        case 5:
                            browseObjects = StorageManager.searchMount(searchTerm);
                            break;
                        case 6:
                            browseObjects = StorageManager.searchOutfit(searchTerm);
                            break;
                    }
                    refreshItems(creaturePanel, creaturePanel.Controls, browseObjects, browseSortedHeader, browseDesc, sortBrowse);
                });
            }
        }

        private void sortBrowse(object sender, EventArgs e) {
            if (browseSortedHeader == (sender as Control).Name) {
                browseDesc = !browseDesc;
            } else {
                browseSortedHeader = (sender as Control).Name;
                browseDesc = false;
            }
            refreshItems(creaturePanel, creaturePanel.Controls, browseObjects, browseSortedHeader, browseDesc, sortBrowse);
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
