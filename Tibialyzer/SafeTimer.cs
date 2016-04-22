using System;
using System.Timers;

namespace Tibialyzer
{
    public sealed class SafeTimer : IDisposable
    {
        private Timer timer;

        public SafeTimer(double interval, Action action)
        {
            timer = new Timer(interval);
            timer.AutoReset = false;
            timer.Elapsed += (s, e) =>
            {
                action();
                if (timer != null) {
                    timer.Start();
                }
            };
        }

        ~SafeTimer()
        {
            Dispose(false);
        }

        public void Start() {
            timer.Start();
        }

        public void Stop() {
            timer.Stop();
        }

        public double Interval
        {
            get {
                if (timer == null) {
                    return -1;
                }
                return timer.Interval;
            }
            set {
                if (timer != null) {
                    timer.Interval = value;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
            }
        }
    }
}
