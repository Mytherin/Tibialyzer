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
        ToolTip tooltip = UIManager.CreateTooltip();


        public LogsTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            logMessageCollection.ReadOnly = true;
            logMessageCollection.TextAlign = HorizontalAlignment.Left;
            logMessageCollection.AttemptDeleteItem += LogMessageCollection_AttemptDeleteItem;
            logMessageCollection.DrawMode = DrawMode.OwnerDrawVariable;
        }
        public void ApplyLocalization() {
            tooltip.RemoveAll();

            saveLogToFileButton.Text = Tibialyzer.Translation.LogsTab.saveLogToFileButton;
            exportHeader.Text = Tibialyzer.Translation.LogsTab.exportHeader;
            loadLogFromFileButton.Text = Tibialyzer.Translation.LogsTab.loadLogFromFileButton;
            logMessagesHeader.Text = Tibialyzer.Translation.LogsTab.logMessagesHeader;
            popupSelectedMessageButton.Text = Tibialyzer.Translation.LogsTab.popupSelectedMessageButton;
            showAllLootButton.Text = Tibialyzer.Translation.LogsTab.showAllLootButton;
            deleteHeader.Text = Tibialyzer.Translation.LogsTab.deleteHeader;
            clearLogButton.Text = Tibialyzer.Translation.LogsTab.clearLogButton;
            tooltip.SetToolTip(saveLogToFileButton, Tibialyzer.Translation.LogsTab.saveLogToFileButtonTooltip);
            tooltip.SetToolTip(clearLogButton, Tibialyzer.Translation.LogsTab.clearLogButtonTooltip);
            tooltip.SetToolTip(loadLogFromFileButton, Tibialyzer.Translation.LogsTab.loadLogFromFileButtonTooltip);
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
                    PopupManager.ShowSimpleNotification(new SimpleLootNotification(result.Item1, result.Item2, message));
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
        
        private void loadLogFromFileButton_Click(object sender, EventArgs e) {
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

        private void saveLogToFileButton_Click(object sender, EventArgs e) {
            try {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Export Log File";
                if (File.Exists("exported_log.txt")) {
                    int i = 1;
                    while (File.Exists("exported_log (" + i.ToString() + ").txt")) i++;
                    dialog.FileName = "exported_log (" + i.ToString() + ").txt";
                } else {
                    dialog.FileName = "exported_log.txt";
                }
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    HuntManager.SaveLog(MainForm.mainForm.getSelectedHunt(), dialog.FileName);
                }
            } catch (Exception ex) {
                MainForm.mainForm.DisplayWarning(ex.Message);
            }
        }
    }
}
