using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


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
                List<Process> ids = Process.GetProcesses().Where(x => x.Id == TibiaProcessId).ToList();
                if (ids.Count > 0) {
                    return new Process[1] { ids[0] };
                }
                TibiaProcessId = -1;
            }
            Process[] p = Process.GetProcessesByName(TibiaClientName);
            if (p.Length > 0) {
                if (TibiaClientName.ToLower().Contains("flash")) {
                    return p;
                }
                return new Process[1] { p[0] };
            }
            return null;
        }

        public static void DetectFlashClient() {
            foreach (Process p in Process.GetProcesses()) {
                if (p.ProcessName.ToLower().Contains("flash")) {
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
            return TibiaClientName.ToLower().Contains("flash") || TibiaClientName.ToLower().Contains("chrome");
        }
    }
}
