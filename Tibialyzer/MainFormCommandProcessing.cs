
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace Tibialyzer {
    public partial class MainForm : Form {
        public static ParseMemoryResults lastResults;
        public static bool shownException = false;
        
        public bool ScanMemory() {
            ReadMemoryResults readMemoryResults = ReadMemory();
            ParseMemoryResults parseMemoryResults = ParseLogResults(readMemoryResults);

            if (parseMemoryResults != null) {
                lastResults = parseMemoryResults;
            }
            if (readMemoryResults != null && readMemoryResults.newAdvances.Count > 0) {
                if (SettingsManager.getSettingBool("AutoScreenshotAdvance")) {
                    this.Invoke((MethodInvoker)delegate {
                        saveScreenshot("Advance", takeScreenshot());
                    });
                }
                if (SettingsManager.getSettingBool("CopyAdvances")) {
                    foreach (object obj in readMemoryResults.newAdvances) {
                        this.Invoke((MethodInvoker)delegate {
                            Clipboard.SetText(obj.ToString());
                        });
                    }
                }
                readMemoryResults.newAdvances.Clear();
            }

            if (parseMemoryResults != null && parseMemoryResults.death) {
                if (SettingsManager.getSettingBool("AutoScreenshotDeath")) {
                    this.Invoke((MethodInvoker)delegate {
                        saveScreenshot("Death", takeScreenshot());
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
                            this.Invoke((MethodInvoker)delegate {
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
                        this.Invoke((MethodInvoker)delegate {
                            CommandManager.ExecuteCommand("item@" + itemName);
                        });
                    } else if (StorageManager.creatureExists(itemName) ||
                        (itemName.Contains("dead ") && (itemName = itemName.Replace("dead ", "")) != null && StorageManager.creatureExists(itemName)) ||
                        (itemName.Contains("slain ") && (itemName = itemName.Replace("slain ", "")) != null && StorageManager.creatureExists(itemName))) {
                        this.Invoke((MethodInvoker)delegate {
                            CommandManager.ExecuteCommand("creature@" + itemName);
                        });
                    } else {
                        NPC npc = StorageManager.getNPC(itemName);
                        if (npc != null) {
                            this.Invoke((MethodInvoker)delegate {
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
                this.Invoke((MethodInvoker)delegate {
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
                    this.Invoke((MethodInvoker)delegate {
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
                            this.Invoke((MethodInvoker)delegate {
                                PopupManager.ShowSimpleNotification(new SimpleLootNotification(cr, items));
                            });
                        }

                        if (SettingsManager.getSettingBool("AutoScreenshotItemDrop")) {
                            // Take a screenshot if Tibialyzer is set to take screenshots of valuable loot
                            Bitmap screenshot = takeScreenshot();
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
                            this.Invoke((MethodInvoker)delegate {
                                saveScreenshot("Loot", screenshot);
                            });
                        }
                    }
                }
            }
            return readMemoryResults != null;
        }
    }
}
