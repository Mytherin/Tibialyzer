using System;
using System.Runtime.InteropServices;

namespace Tibialyzer {
    public class WindowFocusWatcher : IDisposable {
        public delegate void WindowFocusWatcherEvent(uint processId);
        
        private readonly Native.WinEventDelegate _delegate;

        private readonly WindowFocusWatcherEvent _event;

        private readonly IntPtr _hook;

        private uint _lastPid;

        public WindowFocusWatcher(WindowFocusWatcherEvent e) {
            _event = e;
            _delegate = WinEventProc;
            _hook = Native.SetWinEventHook(Native.EVENT_OBJECT_FOCUS, Native.EVENT_OBJECT_FOCUS, IntPtr.Zero,
                _delegate, 0, 0, Native.WINEVENT_OUTOFCONTEXT);
        }

        ~WindowFocusWatcher() {
            Dispose(false);
        }

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
            uint dwEventThread, uint dwmsEventTime) {
            var hWnd = Native.GetForegroundWindow();
            uint pid;

            Native.GetWindowThreadProcessId(hWnd, out pid);

            if (_lastPid == pid)
                return;

            _lastPid = pid;
            _event(pid);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing) {
            if (disposing) {
                Native.UnhookWinEvent(_hook);
            }
        }

        private static class Native {
            #region Natives

            public delegate void WinEventDelegate(
                IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread,
                uint dwmsEventTime);

            [DllImport("user32.dll")]
            public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
                WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

            [DllImport("user32.dll")]
            public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();

            public const uint WINEVENT_OUTOFCONTEXT = 0;
            public const uint EVENT_OBJECT_FOCUS = 0x8005;

            #endregion Natives
        }
    }
}
