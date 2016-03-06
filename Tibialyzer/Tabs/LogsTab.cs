using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    public partial class LogsTab : Form, TabInterface {
        public LogsTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            logMessageCollection.ReadOnly = true;
            logMessageCollection.TextAlign = HorizontalAlignment.Left;
            logMessageCollection.AttemptDeleteItem += LogMessageCollection_AttemptDeleteItem;
            logMessageCollection.DrawMode = DrawMode.OwnerDrawVariable;
        }
        public void InitializeTooltips() {
            ToolTip tooltip = UIManager.CreateTooltip();

            tooltip.SetToolTip(clearLog, "WARNING: Clears the active hunt, removing all loot from it.");
            tooltip.SetToolTip(saveLogToFileButton, "Saves all the log messages of the currently selected hunt to a file.");
            tooltip.SetToolTip(loadLogFromFileButton, "Loads a set of log messages from a file into the currently selected hunt. ");
        }


        public void refreshHuntLog(Hunt h) {
            if (h == null) return;
            const int maxLogLines = 250;
            int count = 0;
            logMessageCollection.Items.Clear();
            foreach (string message in h.IterateLogMessages()) {
                logMessageCollection.Items.Add(message);
                if (count++ > maxLogLines) break;
            }
        }
        private void LogMessageCollection_AttemptDeleteItem(object sender, EventArgs e) {
            Hunt h = MainForm.mainForm.getSelectedHunt();
            if (h != null && logMessageCollection.SelectedIndex >= 0) {
                string logMessage = logMessageCollection.Items[logMessageCollection.SelectedIndex].ToString();
                HuntManager.deleteLogMessage(h, logMessage);
                MainForm.mainForm.refreshHunts();
            }
        }

        private void showPopupButton_Click(object sender, EventArgs e) {
            if (logMessageCollection.SelectedIndex >= 0) {
                string message = logMessageCollection.Items[logMessageCollection.SelectedIndex].ToString();
                var result = Parser.ParseLootMessage(message);
                if (result != null) {
                    PopupManager.ShowSimpleNotification(new SimpleLootNotification(result.Item1, result.Item2));
                }
            }
        }

        private void showAllLootButton_Click(object sender, EventArgs e) {
            Hunt h = MainForm.mainForm.getSelectedHunt();
            CommandManager.ExecuteCommand("loot" + Constants.CommandSymbol + (h == null ? "" : h.name));
        }

        private void resetButton_Click(object sender, MouseEventArgs e) {
            Hunt h = MainForm.mainForm.getSelectedHunt();
            if (h != null) {
                CommandManager.ExecuteCommand("reset" + Constants.CommandSymbol + h.name);
            }
        }


        private void importLogFile_Click(object sender, MouseEventArgs e) {
            try {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Import Log File";
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    HuntManager.LoadLog(MainForm.mainForm.getSelectedHunt(), dialog.FileName);
                    MainForm.mainForm.refreshHunts();
                }
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }

        private void exportLogButton_Click(object sender, MouseEventArgs e) {
            try {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Export Log File";
                if (File.Exists("exported_log")) {
                    int i = 1;
                    while (File.Exists("exported_log (" + i.ToString() + ")")) i++;
                    dialog.FileName = "exported_log (" + i.ToString() + ")";
                } else {
                    dialog.FileName = "exported_log";
                }
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    HuntManager.SaveLog(MainForm.mainForm.getSelectedHunt(), dialog.FileName);
                }
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
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

        private void loadLogFromFileButton_Click(object sender, EventArgs e) {

        }
    }
}
