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
    public partial class SummaryTab : Form, TabInterface {
        public SummaryTab() {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
        }

        public void InitializeSettings() {
            maxItemsDisplayedTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("SummaryMaxItemDrops"), maxItemsDisplayedTrack.Minimum), maxUsedItemsTrack.Maximum);
            maxCreatureKillsTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("SummaryMaxCreatures"), maxCreatureKillsTrack.Minimum), maxCreatureKillsTrack.Maximum);
            maxRecentDropsTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("SummaryMaxRecentDrops"), maxRecentDropsTrack.Minimum), maxRecentDropsTrack.Maximum);
            maxDamageEntryTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("SummaryMaxDamagePlayers"), maxDamageEntryTrack.Minimum), maxDamageEntryTrack.Maximum);
            maxUsedItemsTrack.Value = Math.Min(Math.Max(SettingsManager.getSettingInt("SummaryMaxUsedItems"), maxUsedItemsTrack.Minimum), maxUsedItemsTrack.Maximum);

            maxItemsDisplayedTrack_Scroll(maxItemsDisplayedTrack, null);
            maxCreatureKillsTrack_Scroll(maxCreatureKillsTrack, null);
            maxRecentDropsTrack_Scroll(maxRecentDropsTrack, null);
            maxDamageEntryTrack_Scroll(maxDamageEntryTrack, null);
            maxUsedItemsTrack_Scroll(maxUsedItemsTrack, null);
        }

        public void InitializeTooltips() {

        }

        private void maxItemsDisplayedTrack_Scroll(object sender, EventArgs e) {
            SettingsManager.setSetting("SummaryMaxItemDrops", (sender as TrackBar).Value);
            maxItemsDisplayedHeader.Text = String.Format("Max # of Item Drops: {0}", (sender as TrackBar).Value);
        }

        private void maxCreatureKillsTrack_Scroll(object sender, EventArgs e) {
            SettingsManager.setSetting("SummaryMaxCreatures", (sender as TrackBar).Value);
            maxCreatureKillsHeader.Text = String.Format("Max # of Kills: {0}", (sender as TrackBar).Value);
        }

        private void maxRecentDropsTrack_Scroll(object sender, EventArgs e) {
            SettingsManager.setSetting("SummaryMaxRecentDrops", (sender as TrackBar).Value);
            maxRecentDropsHeader.Text = String.Format("Max # of Recent Drops: {0}", (sender as TrackBar).Value);
        }

        private void maxDamageEntryTrack_Scroll(object sender, EventArgs e) {
            SettingsManager.setSetting("SummaryMaxDamagePlayers", (sender as TrackBar).Value);
            maxDamageEntryHeader.Text = String.Format("Max # of Damage Entries: {0}", (sender as TrackBar).Value);
        }

        private void maxUsedItemsTrack_Scroll(object sender, EventArgs e) {
            SettingsManager.setSetting("SummaryMaxUsedItems", (sender as TrackBar).Value);
            maxUsedItemsHeader.Text = String.Format("Max # of Used Items: {0}", (sender as TrackBar).Value);
        }
    }
}
