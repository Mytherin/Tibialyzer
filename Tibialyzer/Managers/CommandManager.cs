using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Tibialyzer {
    class CommandManager {
        private static Coordinate teleportCoordinatePrev = null;
        public static bool ExecuteCommand(string command, ParseMemoryResults parseMemoryResults = null) {
            try {
                if (parseMemoryResults == null) {
                    parseMemoryResults = ScanningManager.lastResults;
                }
                string comp = command.Trim().ToLower();
                Console.WriteLine(command);
                if (comp.StartsWith("creature" + Constants.CommandSymbol)) { //creature@
                    string[] split = command.Split(Constants.CommandSymbol);
                    string parameter = split[1].Trim().ToLower();
                    Creature cr = StorageManager.getCreature(parameter);
                    if (cr != null) {
                        NotificationManager.ShowCreatureDrops(cr, command);
                    } else {
                        List<TibiaObject> creatures = StorageManager.searchCreature(parameter);
                        if (creatures.Count == 1) {
                            NotificationManager.ShowCreatureDrops(creatures[0].AsCreature(), command);
                        } else if (creatures.Count > 1) {
                            NotificationManager.ShowCreatureList(creatures, "Creature List", command);
                        }
                    }
                } else if (comp.StartsWith("look" + Constants.CommandSymbol)) { //look@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    if (parameter == "on") {
                        SettingsManager.setSetting("LookMode", "True");
                    } else if (parameter == "off") {
                        SettingsManager.setSetting("LookMode", "False");
                    } else {
                        List<string> times = TimestampManager.getLatestTimes(5);
                        List<TibiaObject> items = new List<TibiaObject>();
                        foreach (string message in GlobalDataManager.GetLookInformation(times)) {
                            string itemName = Parser.parseLookItem(message).Item1.ToLower();
                            Item item = StorageManager.getItem(itemName);

                            if (item != null) {
                                items.Add(item);
                            } else {
                                Creature cr = StorageManager.getCreature(itemName);
                                if (cr != null) {
                                    items.Add(cr);
                                }
                            }
                        }
                        if (items.Count == 1) {
                            if (items[0] is Item) {
                                NotificationManager.ShowItemNotification("item" + Constants.CommandSymbol + items[0].GetName().ToLower());
                            } else if (items[0] is Creature) {
                                NotificationManager.ShowCreatureDrops(items[0].AsCreature(), command);
                            }
                        } else if (items.Count > 1) {
                            NotificationManager.ShowCreatureList(items, "Looked At Items", command);
                        }
                    }
                } else if (comp.StartsWith("stats" + Constants.CommandSymbol)) { //stats@
                    string name = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Creature cr = StorageManager.getCreature(name);
                    if (cr != null) {
                        NotificationManager.ShowCreatureStats(cr, command);
                    }
                } else if (comp.StartsWith("close" + Constants.CommandSymbol)) { //close@
                                                                                 // close all notifications
                    NotificationManager.ClearNotifications();
                    PopupManager.ClearSimpleNotifications();
                } else if (comp.StartsWith("delete" + Constants.CommandSymbol)) { //delete@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    int killCount;
                    if (int.TryParse(parameter, out killCount)) {
                        HuntManager.deleteCreatureWithThreshold(killCount);
                    } else {
                        Creature cr = StorageManager.getCreature(parameter);
                        if (cr != null) {
                            HuntManager.deleteCreatureFromLog(cr);
                        }
                    }
                } else if (comp.StartsWith("skin" + Constants.CommandSymbol)) { //skin@
                    string[] split = command.Split(Constants.CommandSymbol);
                    string parameter = split[1].Trim().ToLower();
                    int count = 1;
                    Creature cr = StorageManager.getCreature(parameter);
                    if (cr != null) {
                        if (split.Length > 2)
                            int.TryParse(split[2], out count);
                        HuntManager.InsertSkin(cr, count);
                    } else {
                        int.TryParse(parameter, out count);
                        // find creature with highest killcount with a skin and skin that
                        cr = HuntManager.GetHighestKillCreature(HuntManager.activeHunt);
                        if (cr != null) {
                            HuntManager.InsertSkin(cr, count);
                        }
                    }
                } else if (comp.StartsWith("city" + Constants.CommandSymbol)) { //city@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    if (StorageManager.cityNameMap.ContainsKey(parameter)) {
                        City city = StorageManager.cityNameMap[parameter];
                        NotificationManager.ShowCityDisplayForm(city, command);
                    }
                } else if (comp.StartsWith("damage" + Constants.CommandSymbol)) { //damage@
                    if (parseMemoryResults != null) {
                        string[] splits = command.Split(Constants.CommandSymbol);
                        string screenshot_path = "";
                        string parameter = splits[1].Trim().ToLower();
                        if (parameter == "screenshot" && splits.Length > 2) {
                            parameter = "";
                            screenshot_path = splits[2];
                        }
                        NotificationManager.ShowDamageMeter(parseMemoryResults.damagePerSecond, command, parameter, screenshot_path);
                    }
                } else if (comp.StartsWith("experience" + Constants.CommandSymbol)) { //experience@
                    if (parseMemoryResults != null) {
                        NotificationManager.ShowExperienceChartNotification(command);
                    }
                } else if (comp.StartsWith("remindme" + Constants.CommandSymbol)) { //remindme@
                    string[] splits = command.Split(Constants.CommandSymbol);
                    string time = splits[1].ToLower().Replace(" ", "").Replace("\t", "").Replace("\n", "") + 's'; //remove all whitespace
                    int timeInSeconds = 0;
                    int startIndex = 0, endIndex = 0;
                    for (int i = 0; i < time.Length; i++) {
                        if (time[i].isDigit()) {
                            endIndex = i + 1;
                        } else if (endIndex > startIndex) {
                            int value = int.Parse(time.Substring(startIndex, endIndex - startIndex));
                            if (time[i] == 'm') {
                                value *= 60;
                            } else if (time[i] == 'h') {
                                value *= 3600;
                            }
                            timeInSeconds += value;
                            startIndex = i + 1;
                            endIndex = startIndex;
                        } else {
                            startIndex = i + 1;
                        }
                    }
                    if (timeInSeconds > 0) {
                        Image iconImage = null;
                        if (splits.Length > 4) {
                            string icon = splits[4];
                            TibiaObject[] objects = new TibiaObject[] { StorageManager.getItem(icon), StorageManager.getCreature(icon), StorageManager.getNPC(icon), StorageManager.getMount(icon), StorageManager.getSpell(icon), StorageManager.getOutfit(icon) };
                            foreach (var obj in objects) {
                                if (obj != null) {
                                    iconImage = obj.GetImage();
                                    if (iconImage != null) {
                                        break;
                                    }
                                }
                            }
                        }
                        string title = splits.Length > 2 ? splits[2] : "Reminder!";
                        string message = splits.Length > 3 ? splits[3] : String.Format("Reminder from {0} seconds ago!", timeInSeconds);

                        const int notificationWarningTime = 5;

                        if (timeInSeconds <= notificationWarningTime) {
                            PopupManager.ShowSimpleNotification(new SimpleTimerNotification(iconImage, title, message, timeInSeconds));
                        } else {
                            System.Timers.Timer timer = new System.Timers.Timer(1000 * (timeInSeconds - notificationWarningTime));
                            timer.Elapsed += (sender, e) => {
                                timer.Enabled = false;
                                timer.Dispose();

                                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                                    PopupManager.ShowSimpleNotification(new SimpleTimerNotification(iconImage, title, message, notificationWarningTime));
                                });
                            };
                            timer.Enabled = true;
                        }
                    }
                } else if (comp.StartsWith("exp" + Constants.CommandSymbol)) { //exp@
                    string title = "Experience";
                    string text = "Currently gaining " + (parseMemoryResults == null ? "unknown" : ((int)parseMemoryResults.expPerHour).ToString()) + " experience an hour.";
                    Image image = StyleManager.GetImage("tibia.png");
                    if (!SettingsManager.getSettingBool("UseRichNotificationType")) {
                        PopupManager.ShowSimpleNotification(title, text, image);
                    } else {
                        PopupManager.ShowSimpleNotification(new SimpleTextNotification(null, title, text));
                    }
                } else if (comp.StartsWith("waste" + Constants.CommandSymbol)) { //waste@
                    NotificationManager.ShowWasteForm(HuntManager.activeHunt, command);
                } else if (comp.StartsWith("summary" + Constants.CommandSymbol)) { //summary@
                    NotificationManager.ShowSummaryForm(command);
                } else if (comp.StartsWith("loot" + Constants.CommandSymbol)) { //loot@
                    string[] splits = command.Split(Constants.CommandSymbol);
                    string screenshot_path = "";
                    string parameter = splits[1].Trim().ToLower();
                    if (parameter == "screenshot" && splits.Length > 2) {
                        parameter = "";
                        screenshot_path = splits[2];
                    }

                    Hunt currentHunt = HuntManager.activeHunt;
                    if (splits.Length >= 2 && splits[1] != "") {
                        Hunt h = HuntManager.GetHunt(splits[1]);
                        if (h != null) {
                            currentHunt = h;
                        }
                    }
                    // display loot notification
                    NotificationManager.ShowLootDrops(currentHunt, command, screenshot_path);
                } else if (comp.StartsWith("clipboard" + Constants.CommandSymbol)) { //clipboard@
                    // Copy loot message to the clipboard
                    // clipboard@damage copies the damage information to the clipboard
                    // clipboard@<creature> copies the loot of a specific creature to the clipboard
                    // clipboard@ copies all loot to the clipboard
                    string creatureName = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Creature lootCreature = null;
                    if (creatureName == "damage" && parseMemoryResults != null) {
                        var damageInformation = DamageChart.GenerateDamageInformation(parseMemoryResults.damagePerSecond, "");
                        string damageString = "Damage Dealt: ";
                        foreach (var damage in damageInformation) {
                            damageString += String.Format("{0}: {1:N1}%; ", damage.name, damage.percentage);
                        }
                        Clipboard.SetText(damageString.Substring(0, damageString.Length - 2));
                        return true;
                    } else if (creatureName != "") {
                        lootCreature = StorageManager.getCreature(creatureName);
                    }

                    var tpl = LootDropForm.GenerateLootInformation(HuntManager.activeHunt, "", lootCreature);
                    var creatureKills = tpl.Item1;
                    var itemDrops = tpl.Item2;

                    string lootString = "";
                    if (creatureKills.Count == 1) {
                        foreach (KeyValuePair<Creature, int> kvp in creatureKills) {
                            lootString = "Total Loot of " + kvp.Value.ToString() + " " + kvp.Key.GetName() + (kvp.Value > 1 ? "s" : "") + ": ";
                        }
                    } else {
                        int totalKills = 0;
                        foreach (KeyValuePair<Creature, int> kvp in creatureKills) {
                            totalKills += kvp.Value;
                        }
                        lootString = "Total Loot of " + totalKills + " Kills: ";
                    }
                    foreach (Tuple<Item, int> kvp in itemDrops) {
                        lootString += kvp.Item2 + " " + kvp.Item1.displayname + (kvp.Item2 > 1 ? "s" : "") + ", ";
                    }
                    lootString = lootString.Substring(0, lootString.Length - 2) + ".";
                    Clipboard.SetText(lootString);
                } else if (comp.StartsWith("reset" + Constants.CommandSymbol)) { //reset@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    int time = 0;
                    if (parameter == "old") {
                        HuntManager.clearOldLog(HuntManager.activeHunt);
                    } else if (int.TryParse(parameter, out time) && time > 0) {
                        HuntManager.clearOldLog(HuntManager.activeHunt, time);
                    } else {
                        // reset@<hunt> resets the specified hunt
                        if (parameter.Length > 0 && HuntManager.resetHunt(parameter)) {
                            return true;
                        } else {
                            //reset@ deletes all loot from the currently active hunt
                            HuntManager.resetHunt(HuntManager.activeHunt);
                        }
                    }
                    MainForm.mainForm.refreshHunts();
                    ReadMemoryManager.ignoreStamp = TimestampManager.createStamp();
                } else if (comp.StartsWith("refresh" + Constants.CommandSymbol)) { //refresh@
                                                                                   // refresh: refresh duration on current form, or if no current form, repeat last command without removing it from stack
                                                                                   /*if (tooltipForm != null && !tooltipForm.IsDisposed) {
                                                                                       try {
                                                                                           (tooltipForm as NotificationForm).ResetTimer();
                                                                                       } catch {
                                                                                       }
                                                                                   } else if (command_stack.Count > 0) {*/
                    ExecuteCommand(NotificationManager.LastCommand().command);
                    //}
                    return true;
                } else if (comp.StartsWith("switch" + Constants.CommandSymbol)) { //switch@
                                                                                  // switch: switch to hunt
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    HuntManager.SwitchHunt(parameter);
                    HuntManager.SaveHunts();
                } else if (comp.StartsWith("item" + Constants.CommandSymbol)) { //item@
                                                                                //show the item with all the NPCs that sell it
                    NotificationManager.ShowItemNotification(command);
                } else if (comp.StartsWith("task" + Constants.CommandSymbol)) { //task@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    if (StorageManager.taskList.Keys.Contains(parameter)) {
                        NotificationManager.ShowCreatureList(StorageManager.taskList[parameter].ToList<TibiaObject>(), StorageManager.taskList[parameter][0].groupname, command);
                    } else {
                        int id = -1;
                        int.TryParse(parameter, out id);
                        List<TibiaObject> tasks = new List<TibiaObject>();
                        foreach (KeyValuePair<string, List<Task>> kvp in StorageManager.taskList) {
                            foreach (Task t in kvp.Value) {
                                if (id >= 0 && t.id == id) {
                                    NotificationManager.ShowTaskNotification(t, command);
                                    return true;
                                } else {
                                    if (t.GetName().Contains(parameter, StringComparison.OrdinalIgnoreCase)) {
                                        tasks.Add(t);
                                    }
                                }
                            }
                        }
                        if (tasks.Count == 1) {
                            NotificationManager.ShowTaskNotification(tasks[0] as Task, command);
                        } else {
                            NotificationManager.ShowCreatureList(tasks, String.Format("Tasks Containing \"{0}\"", parameter), command);
                        }

                    }
                } else if (comp.StartsWith("map" + Constants.CommandSymbol)) { //map@
                    NotificationManager.ShowMapForm(command);
                } else if (comp.StartsWith("route" + Constants.CommandSymbol)) { //route@
                    string[] splits = comp.Split(Constants.CommandSymbol);
                    Coordinate targetCoordinate = new Coordinate();
                    TibiaObject imageObject = null;
                    if (splits.Length > 1) {
                        string[] coords = splits[1].Split(',');
                        if (coords.Length >= 3) {
                            if (int.TryParse(coords[0], out targetCoordinate.x) && int.TryParse(coords[1], out targetCoordinate.y) && int.TryParse(coords[2], out targetCoordinate.z)) {
                                if (splits.Length > 2) {
                                    imageObject = StorageManager.getItem(splits[2]);
                                    if (imageObject == null) {
                                        imageObject = StorageManager.getCreature(splits[2]);
                                        if (imageObject == null) {
                                            imageObject = StorageManager.getNPC(splits[2]);
                                            if (imageObject == null) {
                                                imageObject = StorageManager.getMount(splits[2]);
                                                if (imageObject == null) {
                                                    imageObject = StorageManager.getOutfit(splits[2]);
                                                    if (imageObject == null) {
                                                        imageObject = StorageManager.getSpell(splits[2]);
                                                        if (imageObject == null) {
                                                            imageObject = StorageManager.getHunt(splits[2]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                NotificationManager.ShowRoute(targetCoordinate, imageObject, command);
                            }
                        }
                    }
                } else if (comp.StartsWith("registerdoor" + Constants.CommandSymbol)) { //registerdoor@
                    using (StreamWriter writer = new StreamWriter("doors", true)) {
                        writer.WriteLine(String.Format("{0},{1},{2}", MemoryReader.X, MemoryReader.Y, MemoryReader.Z));
                    }
                } else if (comp.StartsWith("registerteleport" + Constants.CommandSymbol)) { //registerteleport@
                    if (teleportCoordinatePrev == null) {
                        teleportCoordinatePrev = new Coordinate(MemoryReader.X, MemoryReader.Y, MemoryReader.Z);
                    } else {
                        string teleportName = "Stairs";
                        if (comp.Split('@')[1].Length > 0) {
                            teleportName = comp.Split('@')[1];
                        }
                        using (StreamWriter writer = new StreamWriter("teleports", true)) {
                            writer.WriteLine(String.Format("{0},{1},{2}-{3},{4},{5}-{6}", teleportCoordinatePrev.x, teleportCoordinatePrev.y, teleportCoordinatePrev.z, MemoryReader.X, MemoryReader.Y, MemoryReader.Z, teleportName));
                        }
                        teleportCoordinatePrev = null;
                    }
                } else if (comp.StartsWith("category" + Constants.CommandSymbol)) { //category@
                                                                                    // list all items with the specified category
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    List<TibiaObject> items = StorageManager.getItemsByCategory(parameter);
                    if (items.Count == 1) {
                        NotificationManager.ShowItemNotification("item" + Constants.CommandSymbol + items[0].GetName().ToLower());
                    } else if (items.Count > 1) {
                        NotificationManager.ShowCreatureList(items, "Category: " + parameter, command, true);
                    }
                } else if (comp.StartsWith("hunt" + Constants.CommandSymbol)) { //hunt@
                    string[] splits = command.Split(Constants.CommandSymbol);
                    string parameter = splits[1].Trim().ToLower();
                    int page = 0;
                    if (splits.Length > 2 && int.TryParse(splits[2], out page)) { }
                    if (Constants.cities.Contains(parameter)) {
                        List<HuntingPlace> huntingPlaces = StorageManager.getHuntsInCity(parameter);
                        NotificationManager.ShowCreatureList(huntingPlaces.ToList<TibiaObject>(), "Hunts in " + parameter, command);
                        return true;
                    }
                    HuntingPlace h = StorageManager.getHunt(parameter);
                    if (h != null) {
                        NotificationManager.ShowHuntingPlace(h, command);
                        return true;
                    }
                    Creature cr = StorageManager.getCreature(parameter);
                    if (cr != null) {
                        List<HuntingPlace> huntingPlaces = StorageManager.getHuntsForCreature(cr.id);
                        NotificationManager.ShowCreatureList(huntingPlaces.ToList<TibiaObject>(), "Hunts containing creature " + parameter.ToTitle(), command);
                        return true;
                    }
                    int minlevel = -1, maxlevel = -1;
                    int level;
                    if (int.TryParse(parameter, out level)) {
                        minlevel = (int)(level * 0.8);
                        maxlevel = (int)(level * 1.2);
                    } else if (parameter.Contains('-')) {
                        string[] split = parameter.Split('-');
                        int.TryParse(split[0].Trim(), out minlevel);
                        int.TryParse(split[1].Trim(), out maxlevel);
                    }
                    if (minlevel >= 0 && maxlevel >= 0) {
                        List<HuntingPlace> huntingPlaces = StorageManager.getHuntsForLevels(minlevel, maxlevel);
                        huntingPlaces = huntingPlaces.OrderBy(o => o.level).ToList();
                        NotificationManager.ShowCreatureList(huntingPlaces.ToList<TibiaObject>(), "Hunts between levels " + minlevel.ToString() + "-" + maxlevel.ToString(), command);
                        return true;
                    } else {
                        string title;
                        List<HuntingPlace> huntList = StorageManager.searchHunt(parameter);
                        title = "Hunts Containing \"" + parameter + "\"";
                        if (huntList.Count == 1) {
                            NotificationManager.ShowHuntingPlace(huntList[0], command);
                        } else if (huntList.Count > 1) {
                            NotificationManager.ShowCreatureList(huntList.ToList<TibiaObject>(), title, command);
                        }
                    }
                } else if (comp.StartsWith("npc" + Constants.CommandSymbol)) { //npc@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    NPC npc = StorageManager.getNPC(parameter);
                    if (npc != null) {
                        NotificationManager.ShowNPCForm(npc, command);
                    } else if (Constants.cities.Contains(parameter)) {
                        NotificationManager.ShowCreatureList(StorageManager.getNPCWithCity(parameter), "NPC List", command);
                    } else {
                        NotificationManager.ShowCreatureList(StorageManager.searchNPC(parameter), "NPC List", command);
                    }
                } else if (comp.StartsWith("savelog" + Constants.CommandSymbol)) {
                    HuntManager.SaveLog(HuntManager.activeHunt, command.Split(Constants.CommandSymbol)[1].Trim().Replace("'", "\\'"));
                } else if (comp.StartsWith("loadlog" + Constants.CommandSymbol)) {
                    HuntManager.LoadLog(HuntManager.activeHunt, command.Split(Constants.CommandSymbol)[1].Trim().Replace("'", "\\'"));
                } else if (comp.StartsWith("setdiscardgoldratio" + Constants.CommandSymbol)) {
                    double val;
                    if (double.TryParse(command.Split(Constants.CommandSymbol)[1].Trim(), out val)) {
                        StorageManager.setGoldRatio(val);
                    }
                } else if (comp.StartsWith("wiki" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim();
                    string response = "";
                    using (WebClient client = new WebClient()) {
                        response = client.DownloadString(String.Format("http://tibia.wikia.com/api/v1/Search/List?query={0}&limit=1&minArticleQuality=10&batch=1&namespaces=0", parameter));
                    }
                    Regex regex = new Regex("\"url\":\"([^\"]+)\"");
                    Match m = regex.Match(response);
                    var gr = m.Groups[1];
                    MainForm.OpenUrl(gr.Value.Replace("\\/", "/"));
                } else if (comp.StartsWith("char" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim();
                    MainForm.OpenUrl("https://secure.tibia.com/community/?subtopic=characters&name=" + parameter);
                } else if (comp.StartsWith("setconvertgoldratio" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim();
                    string[] split = parameter.Split('-');
                    if (split.Length < 2) return true;
                    int stackable = 0;
                    if (split[0] == "1") stackable = 1;
                    double val;
                    if (double.TryParse(split[1], out val)) {
                        StorageManager.setConvertRatio(val, stackable == 1);
                    }
                } else if (comp.StartsWith("recent" + Constants.CommandSymbol) || comp.StartsWith("url" + Constants.CommandSymbol) || comp.StartsWith("last" + Constants.CommandSymbol)) {
                    bool url = comp.StartsWith("url" + Constants.CommandSymbol);
                    int type = url ? 1 : 0;
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    if (comp.StartsWith("last" + Constants.CommandSymbol)) parameter = "1";
                    List<Command> command_list = GlobalDataManager.GetRecentCommands(type).Select(o => new Command() { player = o.Item1, command = o.Item2 }).ToList();
                    command_list.Reverse();
                    int number;
                    //recent@<number> opens the last <number> command, so recent@1 opens the last command
                    if (int.TryParse(parameter, out number)) {
                        if (number > 0 && number <= command_list.Count) {
                            ListNotification.OpenCommand(command_list[number - 1].command, type); ;
                            return true;
                        }
                    } else {
                        //recent@<player> opens the last
                        bool found = false;
                        foreach (Command comm in command_list) {
                            if (comm.player.ToLower() == parameter) {
                                ListNotification.OpenCommand(command_list[number].command, type);
                                found = true;
                                break;
                            }
                        }
                        if (found) return true;
                    }
                    NotificationManager.ShowListNotification(command_list, type, command);
                } else if (comp.StartsWith("spell" + Constants.CommandSymbol)) { // spell@
                    string[] splits = command.Split(Constants.CommandSymbol);
                    string parameter = splits[1].Trim().ToLower();
                    int initialVocation = -1;
                    if (splits.Length > 2 && int.TryParse(splits[2], out initialVocation)) { }
                    Spell spell = StorageManager.getSpell(parameter);
                    if (spell != null) {
                        NotificationManager.ShowSpellNotification(spell, initialVocation, command);
                    } else {
                        List<TibiaObject> spellList = new List<TibiaObject>();
                        string title;
                        if (Constants.vocations.Contains(parameter)) {
                            spellList = StorageManager.getSpellsForVocation(parameter);
                            title = parameter.ToTitle() + " Spells";
                        } else {
                            spellList = StorageManager.searchSpell(parameter);
                            if (spellList.Count == 0) {
                                spellList = StorageManager.searchSpellWords(parameter);
                            }
                            title = "Spells Containing \"" + parameter + "\"";
                        }
                        if (spellList.Count == 1) {
                            NotificationManager.ShowSpellNotification(spellList[0].AsSpell(), initialVocation, command);
                        } else if (spellList.Count > 1) {
                            NotificationManager.ShowCreatureList(spellList, title, command);
                        }
                    }
                } else if (comp.StartsWith("outfit" + Constants.CommandSymbol)) { // outfit@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Outfit outfit = StorageManager.getOutfit(parameter);
                    if (outfit != null) {
                        NotificationManager.ShowOutfitNotification(outfit, command);
                    } else {
                        string title;
                        List<TibiaObject> outfitList = StorageManager.searchOutfit(parameter);
                        title = "Outfits Containing \"" + parameter + "\"";
                        if (outfitList.Count == 1) {
                            NotificationManager.ShowOutfitNotification(outfitList[0].AsOutfit(), command);
                        } else if (outfitList.Count > 1) {
                            NotificationManager.ShowCreatureList(outfitList, title, command);
                        }
                    }
                } else if (comp.StartsWith("quest" + Constants.CommandSymbol)) { // quest@
                    string[] splits = command.Split(Constants.CommandSymbol);
                    string parameter = splits[1].Trim().ToLower();
                    int page = 0;
                    if (splits.Length > 2 && int.TryParse(splits[2], out page)) { }
                    List<Quest> questList = new List<Quest>();
                    if (StorageManager.questNameMap.ContainsKey(parameter)) {
                        NotificationManager.ShowQuestNotification(StorageManager.questNameMap[parameter], command);
                    } else {
                        string title;
                        if (Constants.cities.Contains(parameter)) {
                            title = "Quests In " + parameter;
                            foreach (Quest q in StorageManager.questIdMap.Values) {
                                if (q.city.ToLower() == parameter) {
                                    questList.Add(q);
                                }
                            }
                        } else {
                            title = "Quests Containing \"" + parameter + "\"";
                            string[] splitStrings = parameter.Split(' ');
                            foreach (Quest quest in StorageManager.questIdMap.Values) {
                                bool found = true;
                                foreach (string str in splitStrings) {
                                    if (!quest.name.Contains(str, StringComparison.OrdinalIgnoreCase)) {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found) {
                                    questList.Add(quest);
                                }
                            }
                        }
                        if (questList.Count == 1) {
                            NotificationManager.ShowQuestNotification(questList[0], command);
                        } else if (questList.Count > 1) {
                            NotificationManager.ShowCreatureList(questList.ToList<TibiaObject>(), title, command);
                            //ShowQuestList(questList, title, command, page);
                        }
                    }
                } else if (comp.StartsWith("guide" + Constants.CommandSymbol)) { // guide@
                    string[] splits = command.Split(Constants.CommandSymbol);
                    string parameter = splits[1].Trim().ToLower();
                    int page = 0;
                    string mission = "";
                    if (splits.Length > 2 && int.TryParse(splits[2], out page)) { }
                    if (splits.Length > 3) { mission = splits[3]; }
                    List<Quest> questList = new List<Quest>();
                    if (StorageManager.questNameMap.ContainsKey(parameter)) {
                        NotificationManager.ShowQuestGuideNotification(StorageManager.questNameMap[parameter], command, page, mission);
                    } else {
                        string title;
                        foreach (Quest quest in StorageManager.questIdMap.Values) {
                            if (quest.name.Contains(parameter, StringComparison.OrdinalIgnoreCase)) {
                                questList.Add(quest);
                            }
                        }
                        title = "Quests Containing \"" + parameter + "\"";
                        if (questList.Count == 1) {
                            NotificationManager.ShowQuestGuideNotification(questList[0], command, page, mission);
                        } else if (questList.Count > 1) {
                            NotificationManager.ShowCreatureList(questList.ToList<TibiaObject>(), title, command);
                        }
                    }
                } else if (comp.StartsWith("direction" + Constants.CommandSymbol)) { // direction@
                    string[] splits = command.Split(Constants.CommandSymbol);
                    string parameter = splits[1].Trim().ToLower();
                    int page = 0;
                    if (splits.Length > 2 && int.TryParse(splits[2], out page)) { }
                    List<HuntingPlace> huntList = new List<HuntingPlace>();
                    HuntingPlace h = StorageManager.getHunt(parameter);
                    if (h != null) {
                        NotificationManager.ShowHuntGuideNotification(h, command, page);
                    } else {
                        string title;
                        huntList = StorageManager.searchHunt(parameter);
                        title = "Hunts Containing \"" + parameter + "\"";
                        if (huntList.Count == 1) {
                            NotificationManager.ShowHuntGuideNotification(huntList[0], command, page);
                        } else if (huntList.Count > 1) {
                            NotificationManager.ShowCreatureList(huntList.ToList<TibiaObject>(), title, command);
                        }
                    }
                } else if (comp.StartsWith("mount" + Constants.CommandSymbol)) { // mount@
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Mount m = StorageManager.getMount(parameter);
                    if (m != null) {
                        NotificationManager.ShowMountNotification(m, command);
                    } else {
                        string title;
                        List<TibiaObject> mountList = StorageManager.searchMount(parameter);
                        title = "Mounts Containing \"" + parameter + "\"";
                        if (mountList.Count == 1) {
                            NotificationManager.ShowMountNotification(mountList[0].AsMount(), command);
                        } else if (mountList.Count > 1) {
                            NotificationManager.ShowCreatureList(mountList, title, command);
                        }
                    }
                } else if (comp.StartsWith("pickup" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Item item = StorageManager.getItem(parameter);
                    if (item != null) {
                        StorageManager.setItemDiscard(item, false);
                    }
                } else if (comp.StartsWith("nopickup" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Item item = StorageManager.getItem(parameter);
                    if (item != null) {
                        StorageManager.setItemDiscard(item, true);
                    }
                } else if (comp.StartsWith("convert" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Item item = StorageManager.getItem(parameter);
                    if (item != null) {
                        StorageManager.setItemConvert(item, true);
                    }
                } else if (comp.StartsWith("noconvert" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                    Item item = StorageManager.getItem(parameter);
                    if (item != null) {
                        StorageManager.setItemConvert(item, false);
                    }
                } else if (comp.StartsWith("setval" + Constants.CommandSymbol)) {
                    string parameter = command.Split(Constants.CommandSymbol)[1].Trim();
                    if (!parameter.Contains('=')) return true;
                    string[] split = parameter.Split('=');
                    string item = split[0].Trim().ToLower().Replace("'", "\\'");
                    long value = 0;
                    if (long.TryParse(split[1].Trim(), out value)) {
                        Item it = StorageManager.getItem(split[0]);
                        if (it != null) {
                            StorageManager.setItemValue(it, value);
                        }
                    }
                } else if (comp.StartsWith("screenshot" + Constants.CommandSymbol)) {
                    ScreenshotManager.saveScreenshot("Screenshot", ScreenshotManager.takeScreenshot());
                } else if (comp.StartsWith("lootcount" + Constants.CommandSymbol)) {
                    var sum = GlobalDataManager.GetLootValue();
                    string title = "Loot Value";
                    string text = "Loot value is currently: " + sum;
                    Image image = StyleManager.GetImage("tibia.png");
                    if (!SettingsManager.getSettingBool("UseRichNotificationType")) {
                        PopupManager.ShowSimpleNotification(title, text, image);
                    }
                    else {
                        PopupManager.ShowSimpleNotification(new SimpleTextNotification(null, title, text));
                    }
                } else if (comp.StartsWith("lootcountclear" + Constants.CommandSymbol)) {
                    GlobalDataManager.ClearLootValue();
                }
                else {
                    bool found = false;
                    foreach (string city in Constants.cities) {
                        if (comp.StartsWith(city + Constants.CommandSymbol)) {
                            string itemName = command.Split(Constants.CommandSymbol)[1].Trim().ToLower();
                            Item item = StorageManager.getItem(itemName);
                            if (item != null) {
                                NPC npc = StorageManager.getNPCSellingItemInCity(item.id, city);
                                if (npc != null) {
                                    NotificationManager.ShowNPCForm(npc, command);
                                }
                            } else {
                                Spell spell = StorageManager.getSpell(itemName);
                                if (spell != null) {
                                    NPC npc = StorageManager.getNPCTeachingSpellInCity(spell.id, city);
                                    if (npc != null) {
                                        NotificationManager.ShowNPCForm(npc, command);
                                    }
                                }
                            }

                            found = true;
                        }
                    }
                    // else try custom commands
                    foreach (SystemCommand c in MainForm.mainForm.GetCustomCommands()) {
                        if (c.tibialyzer_command.Trim().Length > 0 && comp.StartsWith(c.tibialyzer_command + Constants.CommandSymbol)) {
                            string[] parameters = command.Split(Constants.CommandSymbol);
                            string systemCallParameters = c.parameters;
                            int i = 0;
                            while (true) {
                                if (systemCallParameters.Contains("{" + i.ToString() + "}")) {
                                    systemCallParameters = systemCallParameters.Replace("{" + i.ToString() + "}", parameters.Length > i + 1 ? parameters[i + 1].Trim() : "");
                                } else {
                                    break;
                                }
                                i++;
                            }
                            ProcessStartInfo procStartInfo = new ProcessStartInfo(c.command, systemCallParameters);

                            procStartInfo.UseShellExecute = true;

                            // Do not show the cmd window to the user.
                            procStartInfo.CreateNoWindow = true;
                            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            Process.Start(procStartInfo);
                            return true;
                        }
                    }
                    if (found) return true;
                    //if we get here we didn't find any command
                    return false;
                }
                return true;
            } catch (Exception e) {
                MainForm.mainForm.DisplayWarning(String.Format("Tibialyzer Exception While Processing Command \"{0}\".\nMessage: {1} ", command, e.Message));
                Console.WriteLine(e.Message);
                return true;
            }
        }
    }
}
