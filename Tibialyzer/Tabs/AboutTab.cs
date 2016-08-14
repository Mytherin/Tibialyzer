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
    public partial class AboutTab : Form, TabInterface {
        public AboutTab() {
            InitializeComponent();
            InitializeSettings();
            ApplyLocalization();
        }

        public void InitializeSettings() {

        }

        public void ApplyLocalization() {
            settingsGuideButton.Text = Tibialyzer.Translation.AboutTab.settingsGuideButton;
            updatingTibialyzerButton.Text = Tibialyzer.Translation.AboutTab.updatingTibialyzerButton;
            quickStartGuideButton.Text = Tibialyzer.Translation.AboutTab.quickStartGuideButton;
            damageMeterGuideButton.Text = Tibialyzer.Translation.AboutTab.damageMeterGuideButton;
            tibialyzerAboutText.Text = Tibialyzer.Translation.AboutTab.tibialyzerAboutText;
            additionalResourcesHeader.Text = Tibialyzer.Translation.AboutTab.additionalResourcesHeader;
            customCommandsGuideButton.Text = Tibialyzer.Translation.AboutTab.customCommandsGuideButton;
            informationGuideButton.Text = Tibialyzer.Translation.AboutTab.informationGuideButton;
            screenshotGuideButton.Text = Tibialyzer.Translation.AboutTab.screenshotGuideButton;
            tibialyzerVersionHeader.Text = Tibialyzer.Translation.AboutTab.tibialyzerVersionHeader;
            autoHotkeyGuideButton.Text = Tibialyzer.Translation.AboutTab.autoHotkeyGuideButton;
            tibiaUpdatedWhatNowButton.Text = Tibialyzer.Translation.AboutTab.tibiaUpdatedWhatNowButton;
        }

        private void autoHotkeyGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/AutoHotkey-Guide");
        }

        private void customCommandsGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Custom-Commands");
        }

        private void damageMeterGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Damage-Meter-Guide");
        }

        private void informationGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Information-Guide");
        }

        private void quickStartGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Information-Guide");
        }

        private void screenshotGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Screenshot-Guide");
        }

        private void settingsGuideButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Settings-Guide");
        }

        private void tibiaUpdatedWhatNowButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Tibia-Updated,-What-Now%3F");
        }

        private void updatingTibialyzerButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/wiki/Updating-Tibialyzer");
        }

        private void reportABugButton_Click(object sender, EventArgs e) {
            MainForm.OpenUrl("https://github.com/Mytherin/Tibialyzer/issues/new");
        }
    }
}
