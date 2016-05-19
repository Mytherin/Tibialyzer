using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class HUDManager {
        private static Dictionary<string, BaseHUD> huds = new Dictionary<string, BaseHUD>();

        public static void Initialize() {
            foreach(string hudName in Constants.HudTypes) {
                if (SettingsManager.getSettingBool(hudName.Replace(" ", "") + "ShowOnStartup")) {
                    ShowHUD(hudName);
                }
            }
        }
        
        private static string AlternateHUDNames(string name) {
            switch (name) {
                case "health":
                case "life":
                case "hp":
                case "healthbar":
                    return "health bar";
                case "mana":
                case "mp":
                case "manabar":
                    return "mana bar";
                case "exp":
                case "experience":
                case "experiencebar":
                    return "experience bar";
                case "curvedbars":
                case "curvedbar":
                case "curved":
                    return "curved bar";
                case "healthlist":
                case "hudlist":
                case "hplist":
                    return "health list";
                case "portrait":
                    return "portrait";
            }
            return name;
        }

        public static void ShowHUD(string param) {
            BaseHUD hud = null;
            param = filterName(param);
            switch(param) {
                case "healthbar":
                    hud = new StatusBar(StatusType.Health);
                    break;
                case "manabar":
                    hud = new StatusBar(StatusType.Mana);
                    break;
                case "experiencebar":
                    hud = new StatusBar(StatusType.Experience);
                    break;
                case "curvedbar":
                case "curvedbars":
                    hud = new CurvedHUD();
                    break;
                case "healthlist":
                    hud = new HealthList();
                    break;
                case "portrait":
                    hud = new Portrait();
                    break;
            }
            if (hud == null) return;
            string hudName = hud.GetHUD();
            string entryName = filterName(hud.GetHUD());
            lock(huds) {
                if (huds.ContainsKey(entryName)) {
                    huds[entryName].Close();
                    huds.Remove(entryName);
                }
                huds.Add(entryName, hud);
                int x = SettingsManager.getSettingInt(hudName + "XOffset");
                int y = SettingsManager.getSettingInt(hudName + "YOffset");
                int width = SettingsManager.getSettingInt(hudName + "Width");
                int height = SettingsManager.getSettingInt(hudName + "Height");
                int anchor = SettingsManager.getSettingInt(hudName + "Anchor");

                width = width < 0 ? 280 : width;
                height = height < 0 ? 60 : height;

                int position_x = 0, position_y = 0;
                Screen screen = ProcessManager.GetScreen();
                int xOffset = x == -1 ? 30 : x;
                int yOffset = y == -1 ? 30 : y;
                switch (anchor) {
                    case 4:
                        position_x = (screen.WorkingArea.Left + screen.WorkingArea.Width / 2) + xOffset - width / 2;
                        position_y = (screen.WorkingArea.Top + screen.WorkingArea.Height / 2) + yOffset - height / 2;
                        break;
                    case 3:
                        position_x = screen.WorkingArea.Right - xOffset - width;
                        position_y = screen.WorkingArea.Bottom - yOffset - height;
                        break;
                    case 2:
                        position_x = screen.WorkingArea.Left + xOffset;
                        position_y = screen.WorkingArea.Bottom - yOffset - height;
                        break;
                    case 1:
                        position_x = screen.WorkingArea.Right - xOffset - width;
                        position_y = screen.WorkingArea.Top + yOffset;
                        break;
                    default:
                        position_x = screen.WorkingArea.Left + xOffset;
                        position_y = screen.WorkingArea.Top + yOffset;
                        break;
                }
                hud.Width = width;
                hud.Height = height;

                hud.LoadHUD();

                hud.StartPosition = FormStartPosition.Manual;
                hud.SetDesktopLocation(position_x, position_y);
                hud.TopMost = true;
                hud.Show();
            }
        }

        public static void CloseHUD(string argument) {
            string hudName = filterName(argument);
            lock(huds) {
                if (hudName.Length == 0) {
                    foreach(var tpl in huds) {
                        tpl.Value.Close();
                    }
                    huds.Clear();
                } else {
                    if (huds.ContainsKey(hudName)) {
                        huds[hudName].Close();
                        huds.Remove(hudName);
                    }
                }
            }
        }

        public static string filterName(string name) {
            return AlternateHUDNames(name.ToLower()).Replace(" ", "");
        }
    }
}
