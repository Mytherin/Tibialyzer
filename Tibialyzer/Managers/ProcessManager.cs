using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Tibialyzer {
    class ProcessManager {
        public static string TibiaClientName = "Tibia";
        public static string TibiaClientType = "Classic";
        private static SafeTimer detectTibiaActive = new SafeTimer(250, DetectIfTibiaActive);
        public static EventHandler<bool> TibiaVisibilityChanged;
        public static IntPtr TibialyzerProcessHandle;


        public static void Initialize() {
            TibiaClientName = SettingsManager.settingExists("TibiaClientName") ? SettingsManager.getSettingString("TibiaClientName") : TibiaClientName;
            detectTibiaActive.Start();
        }

        public static Process GetTibiaProcess() {
            Process[] p = GetTibiaProcesses();
            if (p == null || p.Length == 0) return null;
            return p[0];
        }

        public static Process[] GetTibiaProcesses() {
            if (TibiaClientName == null) {
                Dictionary<int, string> candidateProcess = new Dictionary<int, string>();
                foreach (Process proc in Process.GetProcesses()) {
                    string name = proc.ProcessName.ToLower();
                    if (TibiaClientType == "Flash-Firefox") {
                        if (proc.ProcessName.Contains("flashplayerplugin", StringComparison.InvariantCultureIgnoreCase)) {
                            if (!candidateProcess.ContainsKey(3))
                                candidateProcess.Add(3, proc.ProcessName);
                        } else if (proc.ProcessName.Contains("flashplayer", StringComparison.InvariantCultureIgnoreCase)) {
                            if (!candidateProcess.ContainsKey(2))
                                candidateProcess.Add(2, proc.ProcessName);
                        } else if (proc.ProcessName.Contains("flash", StringComparison.InvariantCultureIgnoreCase)) {
                            if (!candidateProcess.ContainsKey(1))
                                candidateProcess.Add(1, proc.ProcessName);
                        }
                    } else if (TibiaClientType == "Flash-Chrome") {
                        if (proc.ProcessName.Contains("chrome", StringComparison.InvariantCultureIgnoreCase)) {
                            if (!candidateProcess.ContainsKey(3))
                                candidateProcess.Add(3, proc.ProcessName);
                        }
                    }
                }
                if (candidateProcess.Count == 0) {
                    return null;
                }
                TibiaClientName = candidateProcess[candidateProcess.Keys.Max()];
            }
            Process[] p = Process.GetProcessesByName(TibiaClientName);
            if (p.Length > 0) {
                if (ReadMemoryManager.FlashClient) {
                    return p;
                }
                if (TibiaClientName.Contains("tibia", StringComparison.OrdinalIgnoreCase)) {
                    MemoryReader.OpenProcess(p[0]);
                }
                return new Process[1] { p[0] };
            }
            return null;
        }

        public static Screen GetScreen() {
            Process tibia_process = ProcessManager.GetTibiaProcess();
            if (tibia_process == null || SettingsManager.getSettingInt("MonitorAnchor") == 1) {
                return Screen.FromHandle(TibialyzerProcessHandle);
            } else {
                return Screen.FromHandle(tibia_process.MainWindowHandle);
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static bool isTibiaActive = false;
        public static bool IsTibiaActive() {
            return isTibiaActive;
        }

        private static void DetectIfTibiaActive() {
            bool oldTibiaActiveValue = isTibiaActive;
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            if (p.ProcessName.Contains("Tibialyzer", StringComparison.CurrentCultureIgnoreCase)) {
                isTibiaActive = true;
            } else {
                Process tibiaProcess = GetTibiaProcess();
                isTibiaActive = (tibiaProcess != null && tibiaProcess.Id == p.Id);
            }

            if (oldTibiaActiveValue != isTibiaActive && TibiaVisibilityChanged != null) {
                TibiaVisibilityChanged(null, isTibiaActive);
            }
        }

        public static void SelectProcess(Process process) {
            TibiaClientName = process.ProcessName;
            SettingsManager.setSetting("TibiaClientName", TibiaClientName);
        }
    }
}
