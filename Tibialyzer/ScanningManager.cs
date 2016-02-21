using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    enum ScanningState { Scanning, NoTibia, Stuck };
    class ScanningManager {
        private static ScanningState currentState;
        private static System.Timers.Timer scanTimer = null;
        public static ParseMemoryResults lastResults;
        public static bool shownException = false;


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
                ReadMemoryManager.ScanMissingChunks();
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
                    success = ScanMemory();
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

        public static bool ScanMemory() {
            ReadMemoryResults readMemoryResults = ReadMemoryManager.ReadMemory();
            ParseMemoryResults parseMemoryResults = Parser.ParseLogResults(readMemoryResults);

            if (parseMemoryResults != null) {
                lastResults = parseMemoryResults;
            }
            if (readMemoryResults != null && readMemoryResults.newAdvances.Count > 0) {
                if (SettingsManager.getSettingBool("AutoScreenshotAdvance")) {
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        ScreenshotManager.saveScreenshot("Advance", ScreenshotManager.takeScreenshot());
                    });
                }
                if (SettingsManager.getSettingBool("CopyAdvances")) {
                    foreach (object obj in readMemoryResults.newAdvances) {
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            Clipboard.SetText(obj.ToString());
                        });
                    }
                }
                readMemoryResults.newAdvances.Clear();
            }

            if (parseMemoryResults != null && parseMemoryResults.death) {
                if (SettingsManager.getSettingBool("AutoScreenshotDeath")) {
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        ScreenshotManager.saveScreenshot("Death", ScreenshotManager.takeScreenshot());
                    });
                }
                parseMemoryResults.death = false;
            }

            if (parseMemoryResults != null) {
                if (parseMemoryResults.newEventMessages.Count > 0) {
                    if (SettingsManager.getSettingBool("EnableEventNotifications")) {
                        foreach (Tuple<Event, string> tpl in parseMemoryResults.newEventMessages) {
                            Event ev = tpl.Item1;
                            Creature cr = StorageManager.getCreature(ev.creatureid);
                            MainForm.mainForm.Invoke((MethodInvoker)delegate {
                                if (!SettingsManager.getSettingBool("UseRichNotificationType")) {
                                    PopupManager.ShowSimpleNotification("Event in " + ev.location, tpl.Item2, cr.image);
                                } else {
                                    PopupManager.ShowSimpleNotification(new SimpleTextNotification(cr.image, "Event in " + ev.location, tpl.Item2));
                                }
                            });
                        }
                    }
                    parseMemoryResults.newEventMessages.Clear();
                }
            }

            if (SettingsManager.getSettingBool("LookMode") && readMemoryResults != null) {
                foreach (string msg in parseMemoryResults.newLooks) {
                    string itemName = Parser.parseLookItem(msg).ToLower();
                    if (StorageManager.itemExists(itemName)) {
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            CommandManager.ExecuteCommand("item@" + itemName);
                        });
                    } else if (StorageManager.creatureExists(itemName) ||
                        (itemName.Contains("dead ") && (itemName = itemName.Replace("dead ", "")) != null && StorageManager.creatureExists(itemName)) ||
                        (itemName.Contains("slain ") && (itemName = itemName.Replace("slain ", "")) != null && StorageManager.creatureExists(itemName))) {
                        MainForm.mainForm.Invoke((MethodInvoker)delegate {
                            CommandManager.ExecuteCommand("creature@" + itemName);
                        });
                    } else {
                        NPC npc = StorageManager.getNPC(itemName);
                        if (npc != null) {
                            MainForm.mainForm.Invoke((MethodInvoker)delegate {
                                CommandManager.ExecuteCommand("npc@" + itemName);
                            });
                        }
                    }
                }
                parseMemoryResults.newLooks.Clear();
            }

            List<string> commands = parseMemoryResults == null ? new List<string>() : parseMemoryResults.newCommands.ToArray().ToList();
            commands.Reverse();

            foreach (string command in commands) {
                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    if (!CommandManager.ExecuteCommand(command, parseMemoryResults) && SettingsManager.getSettingBool("EnableUnrecognizedNotifications")) {
                        if (!SettingsManager.getSettingBool("UseRichNotificationType")) {
                            PopupManager.ShowSimpleNotification("Unrecognized command", "Unrecognized command: " + command, StyleManager.GetImage("tibia.png"));
                        } else {
                            PopupManager.ShowSimpleNotification(new SimpleTextNotification(null, "Unrecognized command", "Unrecognized command: " + command));
                        }
                    }
                });
            }
            if (parseMemoryResults != null) {
                if (parseMemoryResults.newItems.Count > 0) {
                    MainForm.mainForm.Invoke((MethodInvoker)delegate {
                        LootDatabaseManager.UpdateLoot();
                    });
                }
                foreach (Tuple<Creature, List<Tuple<Item, int>>> tpl in parseMemoryResults.newItems) {
                    Creature cr = tpl.Item1;
                    List<Tuple<Item, int>> items = tpl.Item2;
                    bool showNotification = PopupManager.ShowDropNotification(tpl);
                    if (showNotification) {
                        if (!SettingsManager.getSettingBool("UseRichNotificationType")) {
                            Console.WriteLine("Rich Notification");
                            PopupManager.ShowSimpleNotification(cr.displayname, cr.displayname + " dropped a valuable item.", cr.image);
                        } else {
                            MainForm.mainForm.Invoke((MethodInvoker)delegate {
                                PopupManager.ShowSimpleNotification(new SimpleLootNotification(cr, items));
                            });
                        }

                        if (SettingsManager.getSettingBool("AutoScreenshotItemDrop")) {
                            // Take a screenshot if Tibialyzer is set to take screenshots of valuable loot
                            Bitmap screenshot = ScreenshotManager.takeScreenshot();
                            if (screenshot == null) continue;
                            // Add a notification to the screenshot
                            SimpleLootNotification screenshotNotification = new SimpleLootNotification(cr, items);
                            Bitmap notification = new Bitmap(screenshotNotification.Width, screenshotNotification.Height);
                            screenshotNotification.DrawToBitmap(notification, new Rectangle(0, 0, screenshotNotification.Width, screenshotNotification.Height));
                            foreach (Control c in screenshotNotification.Controls) {
                                c.DrawToBitmap(notification, new Rectangle(c.Location, c.Size));
                            }
                            screenshotNotification.Dispose();
                            int widthOffset = notification.Width + 10;
                            int heightOffset = notification.Height + 10;
                            if (screenshot.Width > widthOffset && screenshot.Height > heightOffset) {
                                using (Graphics gr = Graphics.FromImage(screenshot)) {
                                    gr.DrawImage(notification, new Point(screenshot.Width - widthOffset, screenshot.Height - heightOffset));
                                }
                            }
                            notification.Dispose();
                            MainForm.mainForm.Invoke((MethodInvoker)delegate {
                                ScreenshotManager.saveScreenshot("Loot", screenshot);
                            });
                        }
                    }
                }
            }
            return readMemoryResults != null;
        }
    }
}
