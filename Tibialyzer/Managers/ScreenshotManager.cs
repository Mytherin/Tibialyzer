using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tibialyzer {
    class ScreenshotManager {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

        public static Bitmap takeScreenshot() {
            try {
                RECT Rect = new RECT();
                if (ReadMemoryManager.FlashClient) {
                    Screen screen = Screen.FromControl(MainForm.mainForm);

                    Rect.left = screen.Bounds.Left;
                    Rect.right = screen.Bounds.Right;
                    Rect.top = screen.Bounds.Top;
                    Rect.bottom = screen.Bounds.Bottom;
                } else {
                    Process tibia_process = ProcessManager.GetTibiaProcess();
                    if (tibia_process == null) return null; //no tibia to take screenshot of
                    
                    if (!GetWindowRect(tibia_process.MainWindowHandle, ref Rect)) return null;
                }

                Bitmap bitmap = new Bitmap(Rect.right - Rect.left, Rect.bottom - Rect.top);
                using (Graphics gr = Graphics.FromImage(bitmap)) {
                    gr.CopyFromScreen(new Point(Rect.left, Rect.top), Point.Empty, bitmap.Size);
                }
                return bitmap;
            } catch(Exception ex) {
                MainForm.mainForm.DisplayWarning("Failed to take screenshot: " + ex.Message);
                return null;
            }
        }

        public static void saveScreenshot(string name, Bitmap bitmap) {
            if (bitmap == null) return;
            string path = SettingsManager.getSettingString("ScreenshotPath");
            if (path == null) return;
            try {
                DateTime dt = DateTime.Now;
                name = String.Format("{0}-{1}-{2} {3}h{4}m{5}s{6}ms {7}.png", dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"), dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"), dt.Millisecond.ToString("D4"), name);
                path = Path.Combine(path, name);
                bitmap.Save(path, ImageFormat.Png);
                bitmap.Dispose();
                MainForm.mainForm.refreshScreenshots();
            } catch(Exception ex) {
                MainForm.mainForm.DisplayWarning("Failed to save screenshot: " + ex.Message);
            }
        }

    }
}
