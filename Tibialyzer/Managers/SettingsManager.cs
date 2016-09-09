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
using System.Text;

namespace Tibialyzer {
    public static class SettingsManager {
        private static object lockObject = new object();
        public static Dictionary<string, List<string>> settings = new Dictionary<string, List<string>>();
        private static System.Timers.Timer flushTimer;

        public static void Initialize() {
            flushTimer = new System.Timers.Timer(1000);
            flushTimer.AutoReset = false;
            flushTimer.Elapsed += (s, e) => {
                ActuallySaveSettings();
            };
        }


        public static void LoadSettings(string settingsFile) {
            string currentSetting = null;
            lock(lockObject) {
                settings.Clear();
               
                if (!File.Exists(settingsFile)) {
                    ResetSettingsToDefault();
                    SaveSettings();
                } else {
                    var lines = File.ReadAllLines(settingsFile);
                    foreach (string line in lines) {
                        if (line.Length == 0) continue;
                        if (line[0] == '@') {
                            currentSetting = line.Substring(1, line.Length - 1);
                            if (!settings.ContainsKey(currentSetting))
                                settings.Add(currentSetting, new List<string>());
                        } else if (currentSetting != null) {
                            settings[currentSetting].Add(line);
                        }
                    }
                }
            }
        }
        
        private static void ActuallySaveSettings() {
            try {
                lock (lockObject) {
                    flushTimer.Stop();
                    flushTimer.Enabled = false;
                    // First check if a temporary file exists, if it does delete it
                    try {
                        if (File.Exists(Constants.SettingsTemporaryFile)) {
                            File.Delete(Constants.SettingsTemporaryFile);
                        }
                    } catch(Exception ex) {
                        // could not delete old temporary file
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            MainForm.mainForm.DisplayWarning(String.Format("Failed to remove old temporary file: {0}", ex.Message));
                        });
                        return;
                    }
                    
                    // We initially write the settings to the temporary file
                    var sb = new StringBuilder();
                    foreach (KeyValuePair<string, List<string>> pair in settings) {
                        sb.Append("@").Append(pair.Key).AppendLine();
                        foreach (string str in pair.Value) {
                            sb.AppendLine(str);
                        }
                    }
                    try {
                        File.WriteAllText(Constants.SettingsTemporaryFile, sb.ToString());
                    } catch(Exception ex) {
                        // could not write settings, abort
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            MainForm.mainForm.DisplayWarning(String.Format("Failed to write settings: {0}", ex.Message));
                        });
                        return;
                    }
                    // Now that the settings have been successfully written, move the temporary file to the temporary backup
                    // "Temporary backup" indicates that it is filled with working settings
                    try {
                        if (File.Exists(Constants.SettingsTemporaryBackup)) {
                            File.Delete(Constants.SettingsTemporaryBackup);
                        }
                        File.Move(Constants.SettingsTemporaryFile, Constants.SettingsTemporaryBackup);
                    } catch(Exception ex) {
                        // could not create temporary backup of settings, abort
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            MainForm.mainForm.DisplayWarning(String.Format("Failed to create backup of settings: {0}", ex.Message));
                        });
                        return;
                    }
                    // Now replace the settings file with the temporary backup, first delete the old settings file
                    try {
                        File.Delete(Constants.SettingsFile);
                    } catch(Exception ex) {
                        // failed to delete old settings
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            MainForm.mainForm.DisplayWarning(String.Format("Failed to delete old settings: {0}", ex.Message));
                        });
                        return;
                    }
                    // Then move the temporary backup to the settings file location
                    try {
                        File.Move(Constants.SettingsTemporaryBackup, Constants.SettingsFile);
                    } catch(Exception ex) {
                        // failed to move settings
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            MainForm.mainForm.DisplayWarning(String.Format("Failed to move settings: {0}", ex.Message));
                        });
                        return;
                    }
                }
            } catch (Exception ex) {
                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    MainForm.mainForm.DisplayWarning(String.Format("Failed to save settings: {0}", ex.Message));
                });
            }
        }

        public static void CreateBackup() {
            lock(lockObject) {
                try {
                    // create a backup of the settings file, first copy the settings to a temporary file
                    if (File.Exists(Constants.SettingsTemporaryFile)) {
                        File.Delete(Constants.SettingsTemporaryFile);
                    }
                    File.Copy(Constants.SettingsFile, Constants.SettingsTemporaryFile);
                    if (File.Exists(Constants.SettingsBackupFile)) {
                        File.Delete(Constants.SettingsBackupFile);
                    }
                    // then if the copy was successful, move it to the backup file
                    File.Move(Constants.SettingsTemporaryFile, Constants.SettingsBackupFile);
                } catch(Exception ex) {
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        MainForm.mainForm.DisplayWarning(String.Format("Failed to create backup: {0}", ex.Message));
                    });
                }
            }
        }

        public static void RestoreBackup() {
            lock(lockObject) {
                // restore from a backup, if one exists
                if (!File.Exists(Constants.SettingsBackupFile)) {
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        MainForm.mainForm.DisplayWarning(String.Format("Failed to restore backup: no backup file found"));
                    });
                    return;
                }
                // first try to load from the backup; if that is not successful the backup is corrupted
                var oldSettings = settings;
                try {
                    LoadSettings(Constants.SettingsBackupFile);
                    if (settings.Count == 0) {
                        throw new Exception("No settings found in file.");
                    }
                } catch(Exception ex) {
                    settings = oldSettings;
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        MainForm.mainForm.DisplayWarning(String.Format("Failed to restore backup: {0}", ex.Message));
                    });
                    return;
                }
                settings = oldSettings;
                // for safety, first move the current settings to the Temporary Backup location
                try {
                    if (File.Exists(Constants.SettingsTemporaryBackup)) {
                        File.Delete(Constants.SettingsTemporaryBackup);
                    }
                    File.Copy(Constants.SettingsFile, Constants.SettingsTemporaryBackup);
                } catch(Exception ex) {
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        MainForm.mainForm.DisplayWarning(String.Format("Failed to create a copy of the current settings file: {0}", ex.Message));
                    });
                    return;
                }
                // now restore the backup, delete the current settings file and move the backup to the settings file location
                try {
                    File.Delete(Constants.SettingsFile);
                    File.Copy(Constants.SettingsBackupFile, Constants.SettingsFile);
                    LoadSettings(Constants.SettingsFile);
                    File.Delete(Constants.SettingsTemporaryBackup);
                } catch(Exception ex) {
                    string message = ex.Message;
                    try {
                        // failed to restore backup, restore old settings file
                        File.Move(Constants.SettingsTemporaryBackup, Constants.SettingsFile);
                    } catch(Exception ex2) {
                        Console.WriteLine(ex2.Message);
                    }
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        MainForm.mainForm.DisplayWarning(String.Format("Failed to restore backup: {0}", ex.Message));
                    });
                    return;
                }
                // restart after a successful backup restore
                Application.Restart();
            }
        }
        
        public static void SaveSettings() {
            lock(lockObject) {
                if (!flushTimer.Enabled) {
                    flushTimer.Start();
                }
            }
        }

        public static void removeSetting(string key) {
            lock(lockObject) {
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
            lock(lockObject) {
                if (!settings.ContainsKey(key)) return new List<string>();
                return settings[key];
            }
        }

        public static void setSetting(string key, List<string> value) {
            lock(lockObject) {
                if (!settings.ContainsKey(key)) {
                    settings.Add(key, value);
                } else {
                    settings[key] = value;
                }
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
            lock (lockObject) {
                if (!settings.ContainsKey(key)) {
                    setSetting(key, value);
                }
            }
        }

        public static void setSettingIfNotSet(string key, string value) {
            lock (lockObject) {
                if (!settings.ContainsKey(key)) setSetting(key, new List<string> { value });
            }
        }

        public static void setSettingIfNotSet(string key, int value) {
            lock (lockObject) {
                if (!settings.ContainsKey(key)) setSetting(key, value.ToString());
            }
        }
        public static void setSettingIfNotSet(string key, bool value) {
            lock (lockObject) {
                if (!settings.ContainsKey(key)) setSetting(key, value.ToString());
            }
        }
        public static void setSettingIfNotSet(string key, double value) {
            lock (lockObject) {
                if (!settings.ContainsKey(key)) setSetting(key, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool settingExists(string key) {
            lock (lockObject) {
                return settings.ContainsKey(key) && settings[key].Count > 0;
            }
        }

        public const string defaultWASDSettings = @"; Suspend autohotkey mode with Ctrl+Enter
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
            setSettingIfNotSet("EnableUnrecognizedNotifications", false);
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
            setSettingIfNotSet("RouteFormDuration", Constants.MaximumNotificationDuration);
            setSettingIfNotSet("OutfiterFormDuration", Constants.MaximumNotificationDuration);
            setSettingIfNotSet("LootDropFormDuration", Constants.MaximumNotificationDuration);
            setSettingIfNotSet("LootDropFormGroup", 1);
            setSettingIfNotSet("SummaryFormDuration", Constants.MaximumNotificationDuration);
            setSettingIfNotSet("SummaryFormGroup", 1);
            foreach (string obj in Constants.NotificationTypes) {
                string settingObject = obj.Replace(" ", "");
                setSettingIfNotSet(settingObject + "Anchor", 0);
                setSettingIfNotSet(settingObject + "XOffset", 30);
                setSettingIfNotSet(settingObject + "YOffset", 30);
                setSettingIfNotSet(settingObject + "Duration", 30);
                setSettingIfNotSet(settingObject + "Group", 0);
            }
            setSettingIfNotSet("StackAllItems", true);
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
            setSettingIfNotSet("ScanInternalTabStructure", true);
            setSettingIfNotSet("SkipDuplicateLoot", false);
                        
            setSettingIfNotSet("ManaBarYOffset", 95);
            setSettingIfNotSet("ExperienceBarYOffset", 160);

            setSettingIfNotSet("CurvedBarsWidth", 300);
            setSettingIfNotSet("CurvedBarsHeight", 300);
            setSettingIfNotSet("CurvedBarsAnchor", 4);
            setSettingIfNotSet("CurvedBarsXOffset", -100);
            setSettingIfNotSet("CurvedBarsYOffset", -100);
            setSettingIfNotSet("PortraitAnchor", 0);
            setSettingIfNotSet("PortraitXOffset", 300);
            setSettingIfNotSet("PortraitYOffset", 20);
            setSettingIfNotSet("PortraitWidth", 270);
            setSettingIfNotSet("PortraitHeight", 90);
            setSettingIfNotSet("PortraitCenterImage", "http://tibia.wikia.com/wiki/Outfiter?o=41&a1&a2&fm&f=2&c1=114&c2=41&c3=116&c4=4&m=25");

            setSettingIfNotSet("TaskHUDWidth", 120);
            setSettingIfNotSet("TaskHUDHeight", 35);
            
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
                setSettingIfNotSet(settingObject + "DisplayText", true);
            }
            setSettingIfNotSet("HealthListDisplayNames", true);
            setSettingIfNotSet("HealthListDisplayIcons", false);
            setSettingIfNotSet("HealthListPlayerList", "");

            setSettingIfNotSet("NotificationShowTibiaActive", false);
            setSettingIfNotSet("MonitorAnchor", 0);
            setSettingIfNotSet("SummaryLootItemSize", 25);
            setSettingIfNotSet("SummaryRecentDropsItemSize", 25);
            setSettingIfNotSet("SummaryWasteItemSize", 25);
            setSettingIfNotSet("MaxDamageChartPlayers", 0);

            setSettingIfNotSet("PortraitBackgroundScale", 100);
            setSettingIfNotSet("PortraitBackgroundXOffset", 0);
            setSettingIfNotSet("PortraitBackgroundYOffset", 0);
            setSettingIfNotSet("PortraitCenterScale", 80);
            setSettingIfNotSet("PortraitCenterXOffset", 0);
            setSettingIfNotSet("PortraitCenterYOffset", 0);

            setSettingIfNotSet("GatherExtraPlayerInformation", false);

            setSettingIfNotSet("TibialyzerLanguage", "en-US");
            setSettingIfNotSet("AutomaticallyDetectCharacter", true);
            setSettingIfNotSet("AutomaticallyDownloadAddresses", true);
            setSettingIfNotSet("TibiaClient", "Classic");
            setSettingIfNotSet("NotificationDoubleClick", true);
        }

        public static void ResetSettingsToDefault() {
            lock (lockObject) {
                settings = new Dictionary<string, List<string>>();
            }
            
            ApplyDefaultSettings();
            SaveSettings();
        }
    }
}
