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
        ToolTip tooltip = UIManager.CreateTooltip();
        public HuntsTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {
            trackedCreatureList.ItemsChanged += TrackedCreatureList_ItemsChanged;
            trackedCreatureList.verifyItem = StorageManager.creatureExists;

            this.displayAllItemsAsStackableCheckbox.Checked = SettingsManager.getSettingBool("StackAllItems");
            this.ignoreLowExperienceCheckbox.Checked = SettingsManager.getSettingBool("IgnoreLowExperience");
            this.ignoreLowExperienceBox.Enabled = this.ignoreLowExperienceCheckbox.Checked;
            this.ignoreLowExperienceBox.Text = SettingsManager.getSettingInt("IgnoreLowExperienceValue").ToString();
            this.automaticallyWriteLootToFileCheckbox.Checked = SettingsManager.getSettingBool("AutomaticallyWriteLootToFile");
        }


        public void ApplyLocalization() {
            tooltip.RemoveAll();
            trackedCreaturesHeader.Text = Tibialyzer.Translation.HuntsTab.trackedCreaturesHeader;
            listOfHuntsHeader.Text = Tibialyzer.Translation.HuntsTab.listOfHuntsHeader;
            expValueLabel.Text = Tibialyzer.Translation.HuntsTab.expValueLabel;
            setAsActiveHuntButton.Text = Tibialyzer.Translation.HuntsTab.setAsActiveHuntButton;
            creatureListHeader.Text = Tibialyzer.Translation.HuntsTab.creatureListHeader;
            ignoreLowExperienceCheckbox.Text = Tibialyzer.Translation.HuntsTab.ignoreLowExperienceCheckbox;
            displayAllCreaturesCheckbox.Text = Tibialyzer.Translation.HuntsTab.displayAllCreaturesCheckbox;
            automaticallyWriteLootToFileCheckbox.Text = Tibialyzer.Translation.HuntsTab.automaticallyWriteLootToFileCheckbox;
            clearHuntOnStartupCheckbox.Text = Tibialyzer.Translation.HuntsTab.clearHuntOnStartupCheckbox;
            lootDisplayOptionsHeader.Text = Tibialyzer.Translation.HuntsTab.lootDisplayOptionsHeader;
            switchOnKillCheckbox.Text = Tibialyzer.Translation.HuntsTab.switchOnKillCheckbox;
            gatherTrackedKillsCheckbox.Text = Tibialyzer.Translation.HuntsTab.gatherTrackedKillsCheckbox;
            lootDisplayHeader.Text = Tibialyzer.Translation.HuntsTab.lootDisplayHeader;
            displayAllItemsAsStackableCheckbox.Text = Tibialyzer.Translation.HuntsTab.displayAllItemsAsStackableCheckbox;
            huntOptionsHeader.Text = Tibialyzer.Translation.HuntsTab.huntOptionsHeader;
            tooltip.SetToolTip(clearHuntOnStartupCheckbox, Tibialyzer.Translation.HuntsTab.clearHuntOnStartupCheckboxTooltip);
            tooltip.SetToolTip(ignoreLowExperienceCheckbox, Tibialyzer.Translation.HuntsTab.ignoreLowExperienceCheckboxTooltip);
            tooltip.SetToolTip(gatherTrackedKillsCheckbox, Tibialyzer.Translation.HuntsTab.gatherTrackedKillsCheckboxTooltip);
            tooltip.SetToolTip(setAsActiveHuntButton, Tibialyzer.Translation.HuntsTab.setAsActiveHuntButtonTooltip);
            tooltip.SetToolTip(displayAllItemsAsStackableCheckbox, Tibialyzer.Translation.HuntsTab.displayAllItemsAsStackableCheckboxTooltip);
            tooltip.SetToolTip(switchOnKillCheckbox, Tibialyzer.Translation.HuntsTab.switchOnKillCheckboxTooltip);
            tooltip.SetToolTip(displayAllCreaturesCheckbox, Tibialyzer.Translation.HuntsTab.displayAllCreaturesCheckboxTooltip);
            tooltip.SetToolTip(automaticallyWriteLootToFileCheckbox, Tibialyzer.Translation.HuntsTab.automaticallyWriteLootToFileCheckboxTooltip);
        }
        public void InitializeHuntDisplay(int activeHuntIndex) {
            MainForm.mainForm.skip_hunt_refresh = true;

            huntList.Items.Clear();
            foreach (Hunt h in HuntManager.IterateHunts()) {
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
            if (HuntManager.HuntCount() <= 1) return;
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
            return listOfHuntsHeader;
        }

        public void refreshHunts(bool refreshSelection = false) {
            Hunt h = getSelectedHunt();
            int currentHunt = 0;
            MainForm.mainForm.skip_hunt_refresh = true;

            huntList.Items.Clear();
            foreach (Hunt hunt in HuntManager.IterateHunts()) {
                huntList.Items.Add(hunt.name);
                if (hunt == h) currentHunt = huntList.Items.Count - 1;
            }
            huntList.SelectedIndex = refreshSelection ? 0 : currentHunt;

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
            UIManager.DisplayCreatureList(creatureImagePanel.Controls, creatureObjects, 0, 0, creatureImagePanel.Width, spacing, null, magnification);
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
            setAsActiveHuntButton.Text = "Currently Active";
            setAsActiveHuntButton.Enabled = false;
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
            displayAllCreaturesCheckbox.Checked = h.trackAllCreatures;
            if (h == HuntManager.activeHunt) {
                setAsActiveHuntButton.Text = "Currently Active";
                setAsActiveHuntButton.Enabled = false;
            } else {
                setAsActiveHuntButton.Text = "Set As Active Hunt";
                setAsActiveHuntButton.Enabled = true;
            }
            string[] split = h.trackedCreatures.Split('\n');
            trackedCreatureList.Items.Clear();
            foreach (string str in split) {
                trackedCreatureList.Items.Add(str);
            }
            clearHuntOnStartupCheckbox.Checked = h.clearOnStartup;
            switchOnKillCheckbox.Checked = h.sideHunt;
            gatherTrackedKillsCheckbox.Checked = h.aggregateHunt;
            refreshHuntImages(h);
            MainForm.mainForm.refreshHuntLog(h);
            MainForm.mainForm.switch_hunt = false;
        }

        private void displayAllItemsAsStackableCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("StackAllItems", (sender as CheckBox).Checked);
        }

        
        private void automaticallyWriteLootToFileCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("AutomaticallyWriteLootToFile", (sender as CheckBox).Checked);
        }

        private void ignoreLowExperienceCheckbox_CheckedChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;

            SettingsManager.setSetting("IgnoreLowExperience", (sender as CheckBox).Checked);
            ignoreLowExperienceBox.Enabled = (sender as CheckBox).Checked;
        }

        private void ignoreLowExperienceBox_TextChanged(object sender, EventArgs e) {
            if (MainForm.prevent_settings_update) return;
            int value;
            if (int.TryParse(ignoreLowExperienceBox.Text, out value)) {
                SettingsManager.setSetting("IgnoreLowExperienceValue", value);
            }
        }
    }
}
