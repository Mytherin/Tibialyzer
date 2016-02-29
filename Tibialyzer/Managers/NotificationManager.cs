using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class NotificationManager {
        private static Stack<TibialyzerCommand> command_stack = new Stack<TibialyzerCommand>();
        private static NotificationForm[] NotificationFormGroups = new NotificationForm[10];
        public static void ShowNotification(NotificationForm f, string command, string screenshot_path = "") {
            if (f == null) return;

            if (screenshot_path == "") {
                TibialyzerCommand cmd = new TibialyzerCommand(command);
                command_stack.Push(cmd);
                f.command = cmd;
            }
            f.Visible = false;
            int richX = -1;
            int richY = -1;
            int anchor = 0;
            int duration = 5;
            int group = 0;
            for (int it = 0; it < Constants.NotificationTypeObjects.Count; it++) {
                if (f.GetType() == Constants.NotificationTypeObjects[it]) {
                    string settingObject = Constants.NotificationTypes[it].Replace(" ", "");
                    richX = SettingsManager.getSettingInt(settingObject + "XOffset");
                    richY = SettingsManager.getSettingInt(settingObject + "YOffset");
                    anchor = SettingsManager.getSettingInt(settingObject + "Anchor");
                    duration = SettingsManager.getSettingInt(settingObject + "Duration");
                    group = Math.Min(Math.Max(SettingsManager.getSettingInt(settingObject + "Group"), 0), 9);
                    break;
                }
            }
            f.notificationDuration = duration;
            f.LoadForm();
            if (screenshot_path != "") {
                Bitmap bitmap = new Bitmap(f.Width, f.Height);
                f.DrawToBitmap(bitmap, new Rectangle(0, 0, f.Width, f.Height));
                foreach (Control c in f.Controls) {
                    c.DrawToBitmap(bitmap, new Rectangle(new Point(Math.Min(Math.Max(c.Location.X, 0), f.Width), Math.Min(Math.Max(c.Location.Y, 0), f.Height)), c.Size));
                }
                bitmap.Save(screenshot_path);
                bitmap.Dispose();
                f.Dispose();
                return;
            }
            if (NotificationFormGroups[group] != null) {
                NotificationFormGroups[group].close();
            }
            int position_x = 0, position_y = 0;
            Screen screen;
            Process tibia_process = ProcessManager.GetTibiaProcess();
            if (tibia_process == null) {
                screen = Screen.FromControl(MainForm.mainForm);
            } else {
                screen = Screen.FromHandle(tibia_process.MainWindowHandle);
            }

            int xOffset = richX == -1 ? 30 : richX;
            int yOffset = richY == -1 ? 30 : richY;
            switch (anchor) {
                case 3:
                    position_x = screen.WorkingArea.Right - xOffset - f.Width;
                    position_y = screen.WorkingArea.Bottom - yOffset - f.Height;
                    break;
                case 2:
                    position_x = screen.WorkingArea.Left + xOffset;
                    position_y = screen.WorkingArea.Bottom - yOffset - f.Height;
                    break;
                case 1:
                    position_x = screen.WorkingArea.Right - xOffset - f.Width;
                    position_y = screen.WorkingArea.Top + yOffset;
                    break;
                default:
                    position_x = screen.WorkingArea.Left + xOffset;
                    position_y = screen.WorkingArea.Top + yOffset;
                    break;
            }

            f.StartPosition = FormStartPosition.Manual;
            f.SetDesktopLocation(position_x, position_y);
            f.TopMost = true;
            f.Show();
            NotificationFormGroups[group] = f;
        }

        public static void ShowItemNotification(string command) {
            string[] splits = command.Split(Constants.CommandSymbol);
            string parameter = splits[1].Trim().ToLower();
            int currentPage = 0;
            if (splits.Length > 2 && int.TryParse(splits[2], out currentPage)) { }
            int currentDisplay = -1;
            if (splits.Length > 3 && int.TryParse(splits[3], out currentDisplay)) { }
            Item item = StorageManager.getItem(parameter);
            if (item == null) {
                List<TibiaObject> items = StorageManager.searchItem(parameter);
                if (items.Count == 0) {
                    return;
                } else if (items.Count > 1) {
                    string category = null;
                    bool displayProperties = true;
                    foreach (TibiaObject obj in items) {
                        string cat = obj.AsItem().category;
                        if (category == null) category = cat;
                        else if (category != cat) {
                            displayProperties = false;
                            break;
                        }
                    }
                    ShowCreatureList(items, "Item List", command, displayProperties);
                    return;
                } else {
                    ShowItemView(items[0].AsItem(), currentPage, currentDisplay, command);
                }
            } else {
                ShowItemView(item, currentPage, currentDisplay, command);
            }
        }

        public static void ShowCreatureDrops(Creature c, string comm) {
            if (c == null) return;
            CreatureDropsForm f = new CreatureDropsForm();
            f.creature = c;

            ShowNotification(f, comm);
        }

        public static void ShowCreatureStats(Creature c, string comm) {
            if (c == null) return;
            CreatureStatsForm f = new CreatureStatsForm();
            f.creature = c;

            ShowNotification(f, comm);
        }
        public static void ShowCreatureList(List<TibiaObject> c, string title, string command, bool conditionalAttributes = false) {
            if (c == null) return;
            string[] split = command.Split(Constants.CommandSymbol);
            string parameter = split[1].Trim().ToLower();
            int page = 0;
            int displayType = 0;
            bool desc = false;
            string sortedHeader = null;

            if (split.Length > 2 && int.TryParse(split[2], out page)) { }
            if (split.Length > 3 && int.TryParse(split[3], out displayType)) { }
            if (split.Length > 4) { desc = split[4] == "1"; }
            if (split.Length > 5) { sortedHeader = split[5]; }
            CreatureList f = new CreatureList(page, displayType == 1 ? DisplayType.Images : DisplayType.Details, sortedHeader, desc);
            f.addConditionalAttributes = conditionalAttributes;
            f.objects = c;
            f.title = title;

            ShowNotification(f, command);
        }

        public static void ShowItemView(Item i, int currentPage, int currentDisplay, string comm) {
            if (i == null) return;
            ItemViewForm f = new ItemViewForm(currentPage, currentDisplay);
            f.item = i;

            ShowNotification(f, comm);
        }

        public static void ShowNPCForm(NPC c, string command) {
            if (c == null) return;
            string[] split = command.Split(Constants.CommandSymbol);
            int page = 0;
            int currentDisplay = -1;
            if (split.Length > 2 && int.TryParse(split[2], out page)) { }
            if (split.Length > 3 && int.TryParse(split[3], out currentDisplay)) { }
            NPCForm f = new NPCForm(page, currentDisplay);
            f.npc = c;

            ShowNotification(f, command);
        }

        public static void ShowWasteForm(Hunt hunt, string command) {
            if (hunt == null) return;
            WasteForm f = new WasteForm();
            f.hunt = hunt;

            ShowNotification(f, command);
        }

        public static void ShowDamageMeter(Dictionary<string, DamageResult> dps, string comm, string filter = "", string screenshot_path = "") {
            DamageChart f = new DamageChart();
            f.dps = dps;
            f.filter = filter;

            ShowNotification(f, comm, screenshot_path);
        }

        public static void ShowExperienceChartNotification(string comm) {
            ExperienceChart f = new ExperienceChart();

            ShowNotification(f, comm);
        }


        public static void ShowLootDrops(Hunt h, string comm, string screenshot_path) {
            LootDropForm ldf = new LootDropForm(comm);
            ldf.hunt = h;

            ShowNotification(ldf, comm, screenshot_path);
        }

        public static void ShowHuntingPlace(HuntingPlace h, string comm) {
            HuntingPlaceForm f = new HuntingPlaceForm();
            f.hunting_place = h;

            ShowNotification(f, comm);
        }

        public static void ShowSpellNotification(Spell spell, int initialVocation, string comm) {
            SpellForm f = new SpellForm(spell, initialVocation);

            ShowNotification(f, comm);
        }

        public static void ShowOutfitNotification(Outfit outfit, string comm) {
            OutfitForm f = new OutfitForm(outfit);

            ShowNotification(f, comm);
        }
        public static void ShowQuestNotification(Quest quest, string comm) {
            QuestForm f = new QuestForm(quest);

            ShowNotification(f, comm);
        }

        public static void ShowHuntGuideNotification(HuntingPlace hunt, string comm, int page) {
            if (hunt.directions.Count == 0) return;
            QuestGuideForm f = new QuestGuideForm(hunt);
            f.initialPage = page;

            ShowNotification(f, comm);
        }

        public static void ShowTaskNotification(Task task, string comm) {
            TaskForm f = new TaskForm(task);

            ShowNotification(f, comm);
        }

        public static void ShowQuestGuideNotification(Quest quest, string comm, int page, string mission) {
            if (quest.questInstructions.Count == 0) return;
            QuestGuideForm f = new QuestGuideForm(quest);
            f.initialPage = page;
            f.initialMission = mission;

            ShowNotification(f, comm);
        }
        public static void ShowMountNotification(Mount mount, string comm) {
            MountForm f = new MountForm(mount);

            ShowNotification(f, comm);
        }
        public static void ShowCityDisplayForm(City city, string comm) {
            CityDisplayForm f = new CityDisplayForm();
            f.city = city;

            ShowNotification(f, comm);
        }

        public static void ShowListNotification(List<Command> commands, int type, string comm) {
            ListNotification f = new ListNotification(commands);
            f.type = type;

            ShowNotification(f, comm);
        }

        public static void Back() {
            if (command_stack.Count <= 1) return;
            command_stack.Pop(); // remove the current command
            string command = command_stack.Pop().command;
            CommandManager.ExecuteCommand(command);
        }

        public static bool HasBack() {
            return command_stack.Count > 1;
        }

        public static void UpdateLootDisplay() {
            for (int i = 0; i < NotificationFormGroups.Length; i++) {
                if (NotificationFormGroups[i] != null && NotificationFormGroups[i] is LootDropForm) {
                    (NotificationFormGroups[i] as LootDropForm).UpdateLoot();
                }
            }
        }

        public static void UpdateExperienceDisplay() {
            for (int i = 0; i < NotificationFormGroups.Length; i++) {
                if (NotificationFormGroups[i] != null && NotificationFormGroups[i] is ExperienceChart) {
                    (NotificationFormGroups[i] as ExperienceChart).UpdateExperience();
                }
            }
        }

        public static void UpdateDamageDisplay() {
            for (int i = 0; i < NotificationFormGroups.Length; i++) {
                if (NotificationFormGroups[i] != null && NotificationFormGroups[i] is DamageChart) {
                    (NotificationFormGroups[i] as DamageChart).UpdateDamage();
                }
            }
        }

        public static void ClearNotifications() {
            for (int i = 0; i < NotificationFormGroups.Length; i++) {
                if (NotificationFormGroups[i] != null) {
                    NotificationFormGroups[i].close();
                }
            }
        }

        public static TibialyzerCommand LastCommand() {
            return command_stack.Peek();
        }

    }
}
