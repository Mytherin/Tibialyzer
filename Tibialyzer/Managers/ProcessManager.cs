using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Tibialyzer {
    class ProcessManager {
        public static string TibiaClientName = "Tibia";
        public static int TibiaProcessId = -1;

        public static void Initialize() {
            TibiaClientName = SettingsManager.settingExists("TibiaClientName") ? SettingsManager.getSettingString("TibiaClientName") : TibiaClientName;
        }

        public static Process GetTibiaProcess() {
            Process[] p = GetTibiaProcesses();
            if (p == null || p.Length == 0) return null;
            return p[0];
        }

        public static Process[] GetTibiaProcesses() {
            if (TibiaProcessId >= 0) {
                Process[] ids = Process.GetProcesses();
                for (int i = 0; i < ids.Length; ++i) {
                    if (ids[i].Id == TibiaProcessId) {
                        MemoryReader.SetProcess(ids[i]);
                        return new Process[1] { ids[i] };
                    }
                }

                TibiaProcessId = -1;
            }
            Process[] p = Process.GetProcessesByName(TibiaClientName);
            if (p.Length > 0) {
                if (TibiaClientName.Contains("flash", StringComparison.OrdinalIgnoreCase)) {
                    return p;
                }
                if (TibiaClientName.Contains("tibia", StringComparison.OrdinalIgnoreCase)) {
                    MemoryReader.SetProcess(p[0]);
                }
                return new Process[1] { p[0] };
            }
            return null;
        }

        public static Screen GetScreen() {
            Process tibia_process = ProcessManager.GetTibiaProcess();
            if (tibia_process == null) {
                return Screen.FromControl(MainForm.mainForm);
            } else {
                return Screen.FromHandle(tibia_process.MainWindowHandle);
            }
        }

        public static void DetectFlashClient() {
            foreach (Process p in Process.GetProcesses()) {
                if (p.ProcessName.Contains("flash", StringComparison.OrdinalIgnoreCase)) {
                    TibiaClientName = p.ProcessName;
                    TibiaProcessId = -1;
                    break;
                }
            }
        }

        public static void SelectProcess(Process process) {
            TibiaClientName = process.ProcessName;
            TibiaProcessId = process.Id;
            SettingsManager.setSetting("TibiaClientName", TibiaClientName);
        }

        public static bool IsFlashClient() {
            return TibiaClientName.Contains("flash", StringComparison.OrdinalIgnoreCase) || TibiaClientName.Contains("chrome", StringComparison.OrdinalIgnoreCase);
        }
    }
}
