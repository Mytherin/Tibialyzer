using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tibialyzer {
    class AutoHotkeyManager {
        private static AutoHotkeySuspendedMode window = null;
        private static string autoHotkeyWarning = "Warning: Modified AutoHotkey settings have not taken effect. Restart AutoHotkey to apply changes.";

        private static string modifyKeyString(string value) {
            if (value.Contains("alt+")) {
                value = value.Replace("alt+", "!");
            }
            if (value.Contains("ctrl+")) {
                value = value.Replace("ctrl+", "^");
            }
            if (value.Contains("shift+")) {
                value = value.Replace("shift+", "+");
            }
            if (value.Contains("command=")) {
                string[] split = value.Split(new string[] { "command=" }, StringSplitOptions.None);
                value = split[0] + "SendMessage, 0xC, 0, \"" + split[1] + "\",,Tibialyzer"; //command is send through the WM_SETTEXT message
            }

            return value;
        }

        private static void writeToAutoHotkeyFile() {
            if (!SettingsManager.settingExists("AutoHotkeySettings")) return;
            using (StreamWriter writer = new StreamWriter(Constants.AutohotkeyFile)) {
                writer.WriteLine("#SingleInstance force");
                if (ProcessManager.IsFlashClient()) {
                    Process p = ProcessManager.GetTibiaProcess();
                    writer.WriteLine("SetTitleMatchMode 2");
                    writer.WriteLine(String.Format("#IfWinActive Tibia Flash Client", p == null ? 0 : p.Id));
                } else {
                    writer.WriteLine("#IfWinActive ahk_class TibiaClient");
                }
                foreach (string l in SettingsManager.getSetting("AutoHotkeySettings")) {
                    string line = l.ToLower();
                    if (line.Length == 0 || line[0] == ';') continue;
                    if (line.Contains("suspend")) {
                        // if the key is set to suspend the hotkey layout, we set it up so it sends a message to us
                        writer.WriteLine(modifyKeyString(line.ToLower().Split(new string[] { "suspend" }, StringSplitOptions.None)[0]));
                        writer.WriteLine("suspend");
                        writer.WriteLine("if (A_IsSuspended)");
                        // message 32 is suspend
                        writer.WriteLine("PostMessage, 0x317,32,32,,Tibialyzer");
                        writer.WriteLine("else");
                        // message 33 is not suspended
                        writer.WriteLine("PostMessage, 0x317,33,33,,Tibialyzer");
                        writer.WriteLine("return");
                    } else {
                        writer.WriteLine(modifyKeyString(line));
                    }
                }
            }
        }

        public static void UpdateSettings(List<string> settings) {
            SettingsManager.setSetting("AutoHotkeySettings", settings);
            MainForm.mainForm.DisplayWarning(autoHotkeyWarning);
        }

        public static void StartAutohotkey() {
            MainForm.mainForm.ClearWarning(autoHotkeyWarning);
            writeToAutoHotkeyFile();
            System.Diagnostics.Process.Start(Constants.AutohotkeyFile);
        }

        public static void ShutdownAutohotkey() {
            foreach (var process in Process.GetProcessesByName("AutoHotkey")) {
                process.Kill();
            }
            CloseSuspendedWindow();
        }


        private static object suspendedLock = new object();
        public static void ShowSuspendedWindow(bool alwaysShow = false) {
            lock (suspendedLock) {
                if (window != null) {
                    window.Close();
                    window = null;
                }
                Screen screen;
                Process tibia_process = ProcessManager.GetTibiaProcess();
                if (tibia_process == null) {
                    screen = Screen.FromControl(MainForm.mainForm);
                } else {
                    screen = Screen.FromHandle(tibia_process.MainWindowHandle);
                }
                window = new AutoHotkeySuspendedMode(alwaysShow);
                int position_x = 0, position_y = 0;

                int suspendedX = SettingsManager.getSettingInt("SuspendedNotificationXOffset");
                int suspendedY = SettingsManager.getSettingInt("SuspendedNotificationYOffset");

                int xOffset = suspendedX < 0 ? 10 : suspendedX;
                int yOffset = suspendedY < 0 ? 10 : suspendedY;
                int anchor = SettingsManager.getSettingInt("SuspendedNotificationAnchor");
                switch (anchor) {
                    case 3:
                        position_x = screen.WorkingArea.Right - xOffset - window.Width;
                        position_y = screen.WorkingArea.Bottom - yOffset - window.Height;
                        break;
                    case 2:
                        position_x = screen.WorkingArea.Left + xOffset;
                        position_y = screen.WorkingArea.Bottom - yOffset - window.Height;
                        break;
                    case 0:
                        position_x = screen.WorkingArea.Left + xOffset;
                        position_y = screen.WorkingArea.Top + yOffset;
                        break;
                    default:
                        position_x = screen.WorkingArea.Right - xOffset - window.Width;
                        position_y = screen.WorkingArea.Top + yOffset;
                        break;
                }

                window.StartPosition = FormStartPosition.Manual;
                window.SetDesktopLocation(position_x, position_y);
                window.TopMost = true;
                window.Show();
            }
        }
        public static void CloseSuspendedWindow() {
            lock (suspendedLock) {
                if (window != null) {
                    window.Close();
                    window = null;
                }
            }
        }
    }
}
