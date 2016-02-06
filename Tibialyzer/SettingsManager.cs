using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace Tibialyzer {
    public class SettingsManager {
        private static string settingsFile = @"Database\settings.txt";
        public static Dictionary<string, List<string>> settings = new Dictionary<string, List<string>>();
        public static void LoadSettings() {
            string line;
            string currentSetting = null;

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

        public static void SaveSettings() {
            try {
                lock (settings) {
                    using (StreamWriter file = new StreamWriter(settingsFile)) {
                        foreach (KeyValuePair<string, List<string>> pair in settings) {
                            file.WriteLine("@" + pair.Key);
                            foreach (string str in pair.Value) {
                                file.WriteLine(str);
                            }
                        }
                    }
                }
            } catch {
            }
        }

        public static void removeSetting(string key) {
            if (settings.ContainsKey(key)) {
                settings.Remove(key);
                SaveSettings();
            }
        }
        
        public static bool getSettingBool(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return false;
            return settings[key][0] == "True";
        }

        public static int getSettingInt(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return -1;
            int v;
            if (int.TryParse(settings[key][0], out v)) {
                return v;
            }
            return -1;
        }
        public static double getSettingDouble(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return -1;
            double v;
            if (double.TryParse(settings[key][0], NumberStyles.Any, CultureInfo.InvariantCulture, out v)) {
                return v;
            }
            return -1;
        }

        public static string getSettingString(string key) {
            if (!settings.ContainsKey(key) || settings[key].Count == 0) return null;
            return settings[key][0];
        }

        public static List<string> getSetting(string key) {
            if (!settings.ContainsKey(key)) return new List<string>();
            return settings[key];
        }

        public static void setSetting(string key, List<string> value) {
            if (!settings.ContainsKey(key)) settings.Add(key, value);
            else settings[key] = value;
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

        public static bool settingExists(string key) {
            return settings.ContainsKey(key) && settings[key].Count > 0;
        }

        public static string defaultWASDSettings = @"# Suspend autohotkey mode with Ctrl+Enter
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

        public static void ResetSettingsToDefault() {
            settings = new Dictionary<string, List<string>>();
            setSetting("NotificationDuration", 30);
            setSetting("EnableSimpleNotifications", true);
            setSetting("EnableEventNotifications", true);
            setSetting("EnableUnrecognizedNotifications", true);
            setSetting("EnableRichNotifications", true);
            setSetting("CopyAdvances", true);
            setSetting("ShowNotifications", true);
            setSetting("UseRichNotificationType", true);
            setSetting("ShowNotificationsValue", true);
            setSetting("NotificationValue", 2000);
            setSetting("ShowNotificationsRatio", false);
            setSetting("NotificationGoldRatio", 100);
            setSetting("ShowNotificationsSpecific", true);
            setSetting("LookMode", true);
            setSetting("AlwaysShowLoot", false);
            setSetting("StartAutohotkeyAutomatically", false);
            setSetting("ShutdownAutohotkeyOnExit", false);
            setSetting("NotificationItems", "");
            setSetting("AutoHotkeySettings", defaultWASDSettings.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList());
            setSetting("AutoScreenshotAdvance", false);
            setSetting("AutoScreenshotItemDrop", false);
            setSetting("AutoScreenshotDeath", false);
            setSetting("EnableScreenshots", false);
            setSetting("Names", "Mytherin");
            setSetting("ScanSpeed", "0");
            setSetting("OutfitGenderMale", true);
            setSetting("RichNotificationXOffset", 30);
            setSetting("RichNotificationYOffset", 30);
            setSetting("RichNotificationAnchor", 0);
            setSetting("SimpleNotificationXOffset", 5);
            setSetting("SimpleNotificationYOffset", 10);
            setSetting("SimpleNotificationAnchor", 3);
            setSetting("EnableSimpleNotificationAnimation", true);
            setSetting("SuspendedNotificationXOffset", 10);
            setSetting("SuspendedNotificationYOffset", 10);
            setSetting("SuspendedNotificationAnchor", 1);
            setSetting("TibiaClientName", "Tibia");

            SaveSettings();
        }
    }
}
