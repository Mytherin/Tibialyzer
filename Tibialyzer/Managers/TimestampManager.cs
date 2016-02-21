using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class TimestampManager {
        public static int createStamp() {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            return getStamp(hour, minute);

        }

        public static int getStamp(int hour, int minute) { return hour * 60 + minute; }

        public static List<string> getLatestTimes(int count, int ignoreStamp = -1) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            return getLatestTimes(hour, minute, count, ignoreStamp);
        }

        public static List<string> getLatestTimes(int hour, int minute, int count, int ignoreStamp = -1) {
            List<string> stamps = new List<string>();
            for (int i = 0; i < count; i++) {
                if (getStamp(hour, minute) == ignoreStamp) return stamps;

                stamps.Add(string.Format("{0}:{1}", (hour < 10 ? "0" + hour.ToString() : hour.ToString()), (minute < 10 ? "0" + minute.ToString() : minute.ToString())));

                if (minute == 0) {
                    hour = hour > 0 ? hour - 1 : 23;
                    minute = 59;
                } else {
                    minute = minute - 1;
                }
            }
            return stamps;
        }

        public static List<int> getLatestStamps(int count, int ignoreStamp = -1) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            return getLatestStamps(hour, minute, count, ignoreStamp);
        }

        public static List<int> getLatestStamps(int hour, int minute, int count, int ignoreStamp = -1) {
            List<int> stamps = new List<int>();
            for (int i = 0; i < count; i++) {
                int stamp = getStamp(hour, minute);
                stamps.Add(stamp);
                if (stamp == ignoreStamp) return stamps;

                if (minute == 0) {
                    hour = hour > 0 ? hour - 1 : 23;
                    minute = 59;
                } else {
                    minute = minute - 1;
                }
            }
            return stamps;
        }

        public static int getDayStamp() {
            var t = DateTime.Now;
            return t.Year * 400 + t.Month * 40 + t.Day;
        }

        public static string createTime(int hour, int minute) {
            while (hour < 0) {
                hour += 24;
            }
            while (minute < 0) {
                minute += 60;
            }
            return String.Format("{0:00}:{1:00}", hour, minute);
        }

        public static int distance(int hour, int minute, int hour2, int minute2) {
            int v1 = hour * 60 + minute;
            int v2 = hour2 * 60 + minute2;
            return Math.Min(Math.Abs(v2 - v1), Math.Abs((Math.Max(v1, v2) - 60 * 24) - Math.Min(v1, v2)));
        }
    }
}
