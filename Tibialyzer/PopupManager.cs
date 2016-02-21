using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class PopupManager {
        private static bool clearSimpleNotifications = false;
        private static int notificationSpacing = 5;
        private static List<SimpleNotification> notificationStack = new List<SimpleNotification>();
        public static NotifyIcon notifyIcon;

        public static void Initialize(NotifyIcon simplePopupControl) {
            notifyIcon = simplePopupControl;
        }

        public static void ShowSimpleNotification(string title, string text, Image image) {
            notifyIcon.BalloonTipText = text;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.Icon = Icon.FromHandle(((Bitmap)image).GetHicon());
            notifyIcon.ShowBalloonTip(5000);
        }

        public static void ShowSimpleNotification(SimpleNotification f) {
            int position_x = 0, position_y = 0;
            Screen screen;
            Process tibia_process = ProcessManager.GetTibiaProcess();
            if (tibia_process == null) {
                screen = Screen.FromControl(MainForm.mainForm);
            } else {
                screen = Screen.FromHandle(tibia_process.MainWindowHandle);
            }
            int simpleX = SettingsManager.getSettingInt("SimpleNotificationXOffset");
            int simpleY = SettingsManager.getSettingInt("SimpleNotificationYOffset");

            int xOffset = simpleX < 0 ? 30 : simpleX;
            int yOffset = simpleY < 0 ? 30 : simpleY;
            int anchor = SettingsManager.getSettingInt("SimpleNotificationAnchor");
            int sign = 1;
            int basePosition = screen.WorkingArea.Bottom - yOffset;
            int startX = 0;
            switch (anchor) {
                case 0:
                case 1:
                    // Top
                    sign = -1;
                    basePosition = screen.WorkingArea.Top + yOffset;
                    break;
                case 2:
                default:
                    // Bottom
                    break;
            }
            switch (anchor) {
                case 0:
                case 2:
                    // Left
                    position_x = screen.WorkingArea.Left + xOffset;
                    startX = position_x - (f.Width + notificationSpacing);
                    break;
                case 1:
                default:
                    // Right
                    position_x = screen.WorkingArea.Right - f.Width - notificationSpacing - xOffset;
                    startX = position_x + f.Width + notificationSpacing;
                    break;
            }

            foreach (SimpleNotification notification in notificationStack) {
                basePosition -= sign * (notification.Height + notificationSpacing);
            }
            position_y = basePosition - sign * f.Height;
            f.StartPosition = FormStartPosition.Manual;
            if (!SettingsManager.getSettingBool("EnableSimpleNotificationAnimation")) {
                startX = position_x;
            }

            f.SetDesktopLocation(startX, position_y);
            f.targetPositionX = position_x;
            f.targetPositionY = position_y;
            f.FormClosed += simpleNotificationClosed;

            notificationStack.Add(f);

            f.TopMost = true;
            f.Show();
        }

        public static void ClearSimpleNotifications() {
            clearSimpleNotifications = true;
            foreach (SimpleNotification f in notificationStack) {
                f.ClearTimers();
                f.Close();
            }
            notificationStack.Clear();
            clearSimpleNotifications = false;
        }

        public static void simpleNotificationClosed(object sender, FormClosedEventArgs e) {
            if (clearSimpleNotifications) return;
            SimpleNotification notification = sender as SimpleNotification;
            if (notification == null) return;
            bool moveDown = false;
            int positionModification = 0;
            int anchor = SettingsManager.getSettingInt("SimpleNotificationAnchor");
            int sign = 1;
            switch (anchor) {
                case 0:
                case 1:
                    sign = -1;
                    break;
            }
            foreach (SimpleNotification f in notificationStack) {
                if (f == notification) {
                    positionModification = sign * (f.Height + notificationSpacing);
                    moveDown = true;
                } else if (moveDown) {
                    f.targetPositionY += positionModification;
                }
            }
            notificationStack.Remove(notification);
        }

    }
}
