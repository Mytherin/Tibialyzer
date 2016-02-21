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
    public partial class HuntsTab : Form, TabInterface {
        public HuntsTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            trackedCreatureList.ItemsChanged += TrackedCreatureList_ItemsChanged;
            trackedCreatureList.verifyItem = StorageManager.creatureExists;
        }


        public void InitializeTooltips() {
            ToolTip tooltip = MainForm.CreateTooltip();

            tooltip.SetToolTip(setActiveHuntButton, "Sets the currently selected hunt as the active hunt. Any creatures killed will be added to the currently active hunt. ");
            tooltip.SetToolTip(displayAllCreaturesBox, "In the loot@ command, only creatures specified in the box below are shown if this is selected.");
            tooltip.SetToolTip(switchOnKillBox, "When a creature specified in the box below is killed, this hunt is made the currently active hunt.");
            tooltip.SetToolTip(gatherTrackedKillsBox, "When a creature specified in the box below is killed, the loot of that creature is always added to this hunt (in addition to the active hunt).");
            tooltip.SetToolTip(clearHuntOnStartupBox, "If this is checked, this hunt will be automatically cleared when Tibialyzer is restarted.");
        }
        public void InitializeHuntDisplay(int activeHuntIndex) {
            MainForm.mainForm.skip_hunt_refresh = true;

            huntList.Items.Clear();
            foreach (Hunt h in HuntManager.hunts) {
                huntList.Items.Add(h.name);
            }
            MainForm.mainForm.skip_hunt_refresh = false;

            huntList.SelectedIndex = activeHuntIndex;
            huntList.ItemsChanged += HuntList_ItemsChanged;
            huntList.ChangeTextOnly = true;
            huntList.AttemptDeleteItem += HuntList_AttemptDeleteItem;
            huntList.AttemptNewItem += HuntList_AttemptNewItem;
        }

        private void HuntList_AttemptNewItem(object sender, EventArgs e) {
            HuntManager.CreateNewHunt();
            refreshHunts();
        }

        private void HuntList_AttemptDeleteItem(object sender, EventArgs e) {
            if (HuntManager.hunts.Count <= 1) return;
            Hunt h = getSelectedHunt();
            HuntManager.DeleteHunt(h);
            HuntManager.SaveHunts();
            refreshHunts(true);
        }

        private void HuntList_ItemsChanged(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null) {
                h.name = (sender as PrettyListBox).Items[(sender as PrettyListBox).SelectedIndex].ToString();
            }
        }

        public PrettyListBox GetHuntList() {
            return huntList;
        }

        public Label GetHuntLabel() {
            return huntListLabel;
        }

        public void refreshHunts(bool refreshSelection = false) {
            Hunt h = getSelectedHunt();
            int currentHunt = 0;
            MainForm.mainForm.skip_hunt_refresh = true;

            lock (HuntManager.hunts) {
                huntList.Items.Clear();
                foreach (Hunt hunt in HuntManager.hunts) {
                    huntList.Items.Add(hunt.name);
                    if (hunt == h) currentHunt = huntList.Items.Count - 1;
                }
                huntList.SelectedIndex = refreshSelection ? 0 : currentHunt;
            }

            MainForm.mainForm.skip_hunt_refresh = false;
            huntBox_SelectedIndexChanged(huntList, null);
        }

        private void refreshHuntImages(Hunt h) {
            int spacing = 4;
            int totalWidth = spacing + spacing;
            int maxHeight = -1;
            float magnification = 1.0f;
            List<TibiaObject> creatureObjects = HuntManager.refreshLootCreatures(h);
            foreach (TibiaObject obj in creatureObjects) {
                Creature cc = obj as Creature;
                totalWidth += cc.image.Width + spacing;
                maxHeight = Math.Max(maxHeight, cc.image.Height);
            }

            if (totalWidth < creatureImagePanel.Width) {
                // fits on one line
                magnification = ((float)creatureImagePanel.Width) / totalWidth;
                //also consider the height
                float maxMagnification = ((float)creatureImagePanel.Height) / maxHeight;
                if (magnification > maxMagnification) magnification = maxMagnification;
            } else if (totalWidth < creatureImagePanel.Width * 2) {
                // make it fit on two lines
                magnification = (creatureImagePanel.Width * 1.7f) / totalWidth;
                //also consider the height
                float maxMagnification = creatureImagePanel.Height / (maxHeight * 2.0f);
                if (magnification > maxMagnification) magnification = maxMagnification;
            } else {
                // make it fit on three lines
                magnification = (creatureImagePanel.Width * 2.7f) / totalWidth;
                //also consider the height
                float maxMagnification = creatureImagePanel.Height / (maxHeight * 3.0f);
                if (magnification > maxMagnification) magnification = maxMagnification;
            }
            creatureImagePanel.Controls.Clear();
            MainForm.DisplayCreatureList(creatureImagePanel.Controls, creatureObjects, 0, 0, creatureImagePanel.Width, spacing, null, magnification);
        }
        private Hunt getSelectedHunt() {
            if (huntList.SelectedIndex < 0) return null;
            return HuntManager.GetHunt(huntList.SelectedIndex);
        }

        private void showLootButton_Click(object sender, EventArgs e) {
            Hunt h = getSelectedHunt();
            if (h != null) {
                CommandManager.ExecuteCommand("loot" + Constants.CommandSymbol + h.name);
            }
        }

        private void activeHuntButton_Click(object sender, MouseEventArgs e) {
            if (MainForm.mainForm.switch_hunt) return;
            Hunt h = getSelectedHunt();
            HuntManager.SetActiveHunt(h);
            setActiveHuntButton.Text = "Currently Active";
            setActiveHuntButton.Enabled = false;
            HuntManager.SaveHunts();
        }

        private void aggregateHuntBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.mainForm.switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.aggregateHunt = (sender as CheckBox).Checked;
            HuntManager.SaveHunts();
        }

        private void sideHuntBox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.mainForm.switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.sideHunt = (sender as CheckBox).Checked;
            HuntManager.SaveHunts();
        }

        private void trackCreaturesCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.mainForm.switch_hunt) return;
            bool chk = (sender as CheckBox).Checked;

            Hunt h = getSelectedHunt();
            h.trackAllCreatures = chk;

            HuntManager.SaveHunts();
        }

        private void TrackedCreatureList_ItemsChanged(object sender, EventArgs e) {
            if (MainForm.mainForm.switch_hunt) return;
            Hunt h = getSelectedHunt();
            string str = "";
            foreach (object obj in (sender as PrettyListBox).Items) {
                str += obj.ToString() + "\n";
            }
            h.trackedCreatures = str.Trim();

            HuntManager.SaveHunts();
            refreshHuntImages(h);
        }

        private void startupHuntCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.mainForm.switch_hunt) return;
            Hunt h = getSelectedHunt();
            h.clearOnStartup = (sender as CheckBox).Checked;
            HuntManager.SaveHunts();
        }

        private void huntBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MainForm.mainForm.skip_hunt_refresh) return;
            if (huntList.SelectedIndex < 0) return;
            MainForm.mainForm.switch_hunt = true;
            Hunt h = getSelectedHunt();
            displayAllCreaturesBox.Checked = h.trackAllCreatures;
            if (h == HuntManager.activeHunt) {
                setActiveHuntButton.Text = "Currently Active";
                setActiveHuntButton.Enabled = false;
            } else {
                setActiveHuntButton.Text = "Set As Active Hunt";
                setActiveHuntButton.Enabled = true;
            }
            string[] split = h.trackedCreatures.Split('\n');
            trackedCreatureList.Items.Clear();
            foreach (string str in split) {
                trackedCreatureList.Items.Add(str);
            }
            clearHuntOnStartupBox.Checked = h.clearOnStartup;
            switchOnKillBox.Checked = h.sideHunt;
            gatherTrackedKillsBox.Checked = h.aggregateHunt;
            refreshHuntImages(h);
            MainForm.mainForm.refreshHuntLog(h);
            MainForm.mainForm.switch_hunt = false;
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
