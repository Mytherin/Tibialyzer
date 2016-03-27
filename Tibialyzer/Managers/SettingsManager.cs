// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace Tibialyzer {
    public class SettingsManager {
        public static Dictionary<string, List<string>> settings = new Dictionary<string, List<string>>();
        public static void LoadSettings(string settingsFile) {
            string line;
            string currentSetting = null;
            lock(settings) {
                settings.Clear();

                if (!File.Exists(settingsFile)) {
                    ResetSettingsToDefault();
                    SaveSettings();
                } else {
                    StreamReader file = new StreamReader(settingsFile);
                    while ((line = file.ReadLine()) != null) {
                        if (line.Length == 0) continue;
                        if (line[0] == '@') {
                            currentSetting = line.Substring(1, line.Length - 1);
                            if (!settings.ContainsKey(currentSetting))
                                settings.Add(currentSetting, new List<string>());
                        } else if (currentSetting != null) {
                            settings[currentSetting].Add(line);
                        }
                    }
                    file.Close();
                }
            }
        }

        public static void SaveSettings() {
            try {
                lock (settings) {
                    using (StreamWriter file = new StreamWriter(Constants.SettingsFile)) {
                        foreach (KeyValuePair<string, List<string>> pair in settings) {
                            file.WriteLine("@" + pair.Key);
                            foreach (string str in pair.Value) {
                                file.WriteLine(str);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    MainForm.mainForm.DisplayWarning(String.Format("Failed to save settings: {0}", ex.Message));
                });
            }
        }

        public static void removeSetting(string key) {
            lock(settings) {
                if (settings.ContainsKey(key)) {
                    settings.Remove(key);
                }
            }
            SaveSettings();
        }

        public static bool getSettingBool(string key) {
            string str = getSettingString(key);
            if (str == null) return false;
            return str == "True";
        }

        public static int getSettingInt(string key) {
            string str = getSettingString(key);
            if (str == null) return -1;
            int v;
            if (int.TryParse(str, out v)) {
                return v;
            }
            return -1;
        }
        public static double getSettingDouble(string key) {
            string str = getSettingString(key);
            if (str == null) return -1;
            double v;
            if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) {
                return v;
            }
            return -1;
        }

        public static string getSettingString(string key) {
            var settings = getSetting(key);
            if (settings.Count == 0) return null;
            return settings[0];
        }

        public static List<string> getSetting(string key) {
            lock(settings) {
                if (!settings.ContainsKey(key)) return new List<string>();
                return settings[key];
            }
        }

        public static void setSetting(string key, List<string> value) {
            lock(settings) {
                if (!settings.ContainsKey(key)) settings.Add(key, value);
                else settings[key] = value;
            }
            SaveSettings();
        }

        public static void setSetting(string key, string value) {
            setSetting(key, new List<string> { value });
        }

        public static void setSetting(string key, int value) {
            setSetting(key, value.ToString());
        }
        public static void setSetting(string key, bool value) {
            setSetting(key, value.ToString());
        }
        public static void setSetting(string key, double value) {
            setSetting(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public static void setSettingIfNotSet(string key, List<string> value) {
            if (!settings.ContainsKey(key)) settings.Add(key, value);
        }

        public static void setSettingIfNotSet(string key, string value) {
            if (!settings.ContainsKey(key)) setSetting(key, new List<string> { value });
        }

        public static void setSettingIfNotSet(string key, int value) {
            if (!settings.ContainsKey(key)) setSetting(key, value.ToString());
        }
        public static void setSettingIfNotSet(string key, bool value) {
            if (!settings.ContainsKey(key)) setSetting(key, value.ToString());
        }
        public static void setSettingIfNotSet(string key, double value) {
            if (!settings.ContainsKey(key)) setSetting(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public static bool settingExists(string key) {
            return settings.ContainsKey(key) && settings[key].Count > 0;
        }

        public static string defaultWASDSettings = @"; Suspend autohotkey mode with Ctrl+Enter
Ctrl+Enter::Suspend
; Enable WASD Movement
W::Up
A::Left
S::Down
D::Right
; Enable diagonal movement with QEZC
Q::NumpadHome
E::NumpadPgUp
Z::NumpadEnd
C::NumpadPgDn
; Hotkey Tibialyzer commands
; Open loot window with the [ key
[::Command=loot@
; Show exp with ] key
]::Command=exp@
; Close all windows when = key is pressed
=::Command=close@
; Open last window with - key
-::Command=refresh@
";

        public static void ApplyDefaultSettings() {
            setSettingIfNotSet("EnableEventNotifications", true);
            setSettingIfNotSet("EnableUnrecognizedNotifications", true);
            setSettingIfNotSet("CopyAdvances", true);
            setSettingIfNotSet("UseRichNotificationType", true);
            setSettingIfNotSet("LookMode", true);
            setSettingIfNotSet("StartAutohotkeyAutomatically", false);
            setSettingIfNotSet("ShutdownAutohotkeyOnExit", false);
            setSettingIfNotSet("NotificationItems", "");
            setSettingIfNotSet("AutoHotkeySettings", defaultWASDSettings.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList());
            setSettingIfNotSet("AutoScreenshotAdvance", false);
            setSettingIfNotSet("AutoScreenshotItemDrop", false);
            setSettingIfNotSet("AutoScreenshotDeath", false);
            setSettingIfNotSet("EnableScreenshots", false);
            setSettingIfNotSet("Names", "Mytherin");
            setSettingIfNotSet("ScanSpeed", "50");
            setSettingIfNotSet("OutfitGenderMale", true);
            setSettingIfNotSet("RichNotificationXOffset", 30);
            setSettingIfNotSet("RichNotificationYOffset", 30);
            setSettingIfNotSet("RichNotificationAnchor", 0);
            setSettingIfNotSet("SimpleNotificationXOffset", 5);
            setSettingIfNotSet("SimpleNotificationYOffset", 10);
            setSettingIfNotSet("SimpleNotificationAnchor", 3);
            setSettingIfNotSet("EnableSimpleNotificationAnimation", true);
            setSettingIfNotSet("SuspendedNotificationXOffset", 10);
            setSettingIfNotSet("SuspendedNotificationYOffset", 10);
            setSettingIfNotSet("SuspendedNotificationAnchor", 1);
            setSettingIfNotSet("TibiaClientName", "Tibia");
            foreach (string obj in Constants.NotificationTypes) {
                string settingObject = obj.Replace(" ", "");
                setSettingIfNotSet(settingObject + "Anchor", 0);
                setSettingIfNotSet(settingObject + "XOffset", 30);
                setSettingIfNotSet(settingObject + "YOffset", 30);
                setSettingIfNotSet(settingObject + "Duration", 30);
                setSettingIfNotSet(settingObject + "Group", 0);
            }
            setSettingIfNotSet("StackAllItems", false);
            setSettingIfNotSet("IgnoreLowExperience", false);
            setSettingIfNotSet("IgnoreLowExperienceValue", 250);
            setSettingIfNotSet("AutomaticallyWriteLootToFile", true);
            setSettingIfNotSet("NotificationConditions", "item.value >= 2000");
            setSettingIfNotSet("CityDisplayFormWidth", 396);
            setSettingIfNotSet("CreatureDropsFormWidth", 358);
            setSettingIfNotSet("CreatureStatsFormWidth", 378);
            setSettingIfNotSet("DamageChartWidth", 450);
            setSettingIfNotSet("ExperienceChartWidth", 450);
            setSettingIfNotSet("SummaryFormWidth", 210);
            setSettingIfNotSet("ItemViewFormWidth", 378);
            setSettingIfNotSet("PopupDuration", 8);
            setSettingIfNotSet("SummaryMaxItemDrops", 5);
            setSettingIfNotSet("SummaryMaxCreatures", 5);
            setSettingIfNotSet("SummaryMaxRecentDrops", 5);
            setSettingIfNotSet("SummaryMaxDamagePlayers", 5);
            setSettingIfNotSet("SummaryMaxUsedItems", 5);
            setSettingIfNotSet("ExperiencePerHourCalculation", "TibiaStyle");
            setSettingIfNotSet("SimpleNotificationWidth", 354);
            setSettingIfNotSet("SimpleNotificationCopyButton", true);
            setSettingIfNotSet("XORAddress", "0x534658");
            setSettingIfNotSet("HealthAddress", "0x6d2030");
            setSettingIfNotSet("MaxHealthAddress", "0x6D2024");
            setSettingIfNotSet("ManaAddress", "0x534688");
            setSettingIfNotSet("MaxManaAddress", "0x53465C");
            setSettingIfNotSet("PlayerIDAddress", "0x6D202C");
            setSettingIfNotSet("BattleListAddress", "0x72DE20");
            setSettingIfNotSet("ExperienceAddress", "0x534660");
            setSettingIfNotSet("LevelAddress", "0x534670");
            setSettingIfNotSet("MagicLevelAddress", "0x534678");
            setSettingIfNotSet("TabsBaseAddress", "0x534970");
            setSettingIfNotSet("ScanInternalTabStructure", true);
            setSettingIfNotSet("SkipDuplicateLoot", false);
                        
            setSettingIfNotSet("ManaBarYOffset", 95);
            setSettingIfNotSet("ExperienceBarYOffset", 160);

            setSettingIfNotSet("CurvedBarsWidth", 300);
            setSettingIfNotSet("CurvedBarsHeight", 300);
            setSettingIfNotSet("CurvedBarsAnchor", 4);
            setSettingIfNotSet("CurvedBarsXOffset", -100);
            setSettingIfNotSet("CurvedBarsYOffset", -100);

            foreach (string obj in Constants.HudTypes) {
                string settingObject = obj.Replace(" ", "");
                setSettingIfNotSet(settingObject + "Anchor", 1);
                setSettingIfNotSet(settingObject + "XOffset", 180);
                setSettingIfNotSet(settingObject + "YOffset", 30);
                setSettingIfNotSet(settingObject + "FontSize", 20);
                setSettingIfNotSet(settingObject + "Width", 200);
                setSettingIfNotSet(settingObject + "Height", 65);
                setSettingIfNotSet(settingObject + "ShowOnStartup", false);
                setSettingIfNotSet(settingObject + "Opacity", 0.8);
            }

            setSettingIfNotSet("MonitorAnchor", 0);
            setSettingIfNotSet("SummaryLootItemSize", 25);
            setSettingIfNotSet("SummaryRecentDropsItemSize", 25);
            setSettingIfNotSet("SummaryWasteItemSize", 25);
        }

        public static void ResetSettingsToDefault() {
            settings = new Dictionary<string, List<string>>();
            ApplyDefaultSettings();
            SaveSettings();
        }
    }
}
