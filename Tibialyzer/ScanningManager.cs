using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    enum ScanningState { Scanning, NoTibia, Stuck };
    class ScanningManager {
        private static ScanningState currentState;
        private static System.Timers.Timer scanTimer = null;

        public static void StartScanning() {
            BackgroundWorker mainScanner = new BackgroundWorker();
            mainScanner.DoWork += ScanMemory;
            mainScanner.RunWorkerAsync();

            BackgroundWorker missingChunkScanner = new BackgroundWorker();
            missingChunkScanner.DoWork += ScanMissingChunks;
            missingChunkScanner.RunWorkerAsync();

            currentState = ScanningState.NoTibia;
        }

        private static void ScanMissingChunks(object sender, DoWorkEventArgs e) {
            while (true) {
                MainForm.ScanMissingChunks();
            }
        }

        private static void ScanMemory(object sender, DoWorkEventArgs e) {
            while (true) {
                if (scanTimer == null) {
                    scanTimer = new System.Timers.Timer(10000);
                    scanTimer.Elapsed += StuckScanning;
                    scanTimer.Enabled = true;
                }
                bool success = false;
                try {
                    success = MainForm.mainForm.ScanMemory();
                } catch (Exception ex) {
                    MainForm.mainForm.BeginInvoke((MethodInvoker)delegate {
                        MainForm.mainForm.DisplayWarning(String.Format("Database Scan Error (Non-Fatal): {0}", ex.Message));
                        Console.WriteLine(ex.Message);
                    });
                }
                scanTimer.Dispose();
                scanTimer = null;
                if (success) {
                    if (currentState != ScanningState.Scanning) {
                        currentState = ScanningState.Scanning;
                        MainForm.mainForm.BeginInvoke((MethodInvoker)delegate {
                            MainForm.mainForm.SetScanningImage("scanningbar.gif", "Scanning Memory...", true);
                        });
                    }
                } else {
                    if (currentState != ScanningState.NoTibia) {
                        currentState = ScanningState.NoTibia;
                        MainForm.mainForm.BeginInvoke((MethodInvoker)delegate {
                            MainForm.mainForm.SetScanningImage("scanningbar-red.gif", "No Tibia Client Found...", true);
                        });
                    }
                }
            }
        }

        private static void StuckScanning(object sender, System.Timers.ElapsedEventArgs e) {
            if (currentState != ScanningState.Stuck) {
                currentState = ScanningState.Stuck;
                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    MainForm.mainForm.SetScanningImage("scanningbar-gray.gif", "Waiting, possibly stuck...", false);
                });
            }
        }

    }
}
