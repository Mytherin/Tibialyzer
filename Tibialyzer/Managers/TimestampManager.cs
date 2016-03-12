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

        public static Tuple<int, int> ParseTimestamp(string timestamp) {
            if (timestamp.Length < 5)
                return null;
            return new Tuple<int, int>(int.Parse(timestamp.Substring(0, 2)), int.Parse(timestamp.Substring(3, 2)));
        }

        public static int getStamp(int hour, int minute) { return hour * 60 + minute; }

        public static List<string> getLatestTimes(int count, int ignoreStamp = -1) {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            return getLatestTimes(hour, minute, count, ignoreStamp);
        }

        public static string FormatTimestamp(int hour, int minute) {
            while (hour < 0) {
                hour += 24;
            }
            while (minute < 0) {
                minute += 60;
            }
            return String.Format("{0:00}:{1:00}", hour, minute);
        }

        public static string getCurrentTime() {
            var time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;
            return FormatTimestamp(hour, minute);
        }

        public static List<string> getLatestTimes(int hour, int minute, int count, int ignoreStamp = -1) {
            List<string> stamps = new List<string>();
            for (int i = 0; i < count; i++) {
                if (getStamp(hour, minute) == ignoreStamp) return stamps;

                stamps.Add(FormatTimestamp(hour, minute));

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

        public static int Distance(string timestamp, string timestamp2) {
            var one = ParseTimestamp(timestamp);
            var two = ParseTimestamp(timestamp2);
            return Distance(one.Item1, one.Item2, two.Item1, two.Item2);
        }

        public static int Distance(int hour, int minute, int hour2, int minute2) {
            int v1 = hour * 60 + minute;
            int v2 = hour2 * 60 + minute2;
            return Math.Min(Math.Abs(v2 - v1), Math.Abs((Math.Max(v1, v2) - 60 * 24) - Math.Min(v1, v2)));
        }
    }
}
