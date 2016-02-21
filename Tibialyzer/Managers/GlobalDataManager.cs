using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class GlobalDataManager {
        public class DamageData {
            public Dictionary<string, int> damagePerMinute;
            public int totalDamage;
        }

        public delegate void DataChangedHandler();
        public static event DataChangedHandler ExperienceChanged;
        public static event DataChangedHandler DamageChanged;

        private static Dictionary<string, List<string>> totalItemDrops = new Dictionary<string, List<string>>();
        private static Dictionary<string, List<Tuple<string, string>>> totalCommands = new Dictionary<string, List<Tuple<string, string>>>();
        private static Dictionary<string, DamageData> totalDamageResults = new Dictionary<string, DamageData>();
        private static Dictionary<string, List<Tuple<string, string>>> totalURLs = new Dictionary<string, List<Tuple<string, string>>>();
        private static Dictionary<string, int> totalExperienceResults = new Dictionary<string, int>();
        private static Dictionary<string, bool> totalDeaths = new Dictionary<string, bool>();
        private static HashSet<string> eventMessages = new HashSet<string>();
        private static HashSet<string> levelAdvances = new HashSet<string>();
        private static Dictionary<string, List<string>> totalLooks = new Dictionary<string, List<string>>();

        public static void UpdateDamage() {
            if (DamageChanged != null) {
                DamageChanged();
            }
        }

        public static int UpdateExperience(Dictionary<string, int> newExperience) {
            int exp = 0;
            lock (totalExperienceResults) {
                foreach (KeyValuePair<string, int> kvp in newExperience) {
                    string time = kvp.Key;
                    int experience = kvp.Value;
                    if (!totalExperienceResults.ContainsKey(time)) {
                        totalExperienceResults.Add(time, experience);
                        exp += experience;
                    } else if (totalExperienceResults[time] < experience) {
                        exp += experience - totalExperienceResults[time];
                        totalExperienceResults[time] = experience;
                    }
                }
            }
            if (exp > 0 && ExperienceChanged != null) {
                ExperienceChanged();
            }
            return exp;
        }

        public static Tuple<int, int> GetTotalExperience(List<string> times) {
            int experience = 0;
            int minutes = 0;
            lock (totalExperienceResults) {
                foreach (string t in times) {
                    if (totalExperienceResults.ContainsKey(t)) {
                        experience += totalExperienceResults[t];
                        minutes++;
                    }
                }
            }
            return new Tuple<int, int>(experience, minutes);
        }

        public static bool UpdateDeaths(Dictionary<string, bool> newDeaths) {
            bool newDeath = false;
            lock (totalDeaths) {
                foreach (KeyValuePair<string, bool> kvp in newDeaths) {
                    if (!totalDeaths.ContainsKey(kvp.Key)) {
                        totalDeaths.Add(kvp.Key, false);
                    }
                    if (kvp.Value && !totalDeaths[kvp.Key]) {
                        newDeath = true;
                        totalDeaths[kvp.Key] = true;
                    }
                }
            }
            return newDeath;
        }

        public static bool UpdateDamageInformation(Dictionary<string, Dictionary<string, int>> damageDealt) {
            bool newDamage = false;
            lock (totalDamageResults) {
                foreach (KeyValuePair<string, Dictionary<string, int>> kvp in damageDealt) {
                    string player = kvp.Key;
                    Dictionary<string, int> playerDamage = kvp.Value;
                    if (!totalDamageResults.ContainsKey(player)) totalDamageResults.Add(player, new DamageData { damagePerMinute = new Dictionary<string, int>(), totalDamage = 0 });
                    DamageData data = totalDamageResults[player];
                    foreach (KeyValuePair<string, int> kvp2 in playerDamage) {
                        string timestamp = kvp2.Key;
                        int damage = kvp2.Value;
                        // if the damage for the given timestamp does not exist yet, add it
                        if (!data.damagePerMinute.ContainsKey(timestamp)) {
                            data.damagePerMinute.Add(timestamp, damage);
                            data.totalDamage += damage;
                            newDamage = true;
                        }
                        // if it does exist, select the biggest of the two
                        // the reason we select the biggest of the two is:
                        // - if the timestamp is 'the current time', totalDamageResults may hold an old value, so we update it
                        // - if timestamp is old, a part of the log for the time could have already been removed (because the log was full)
                        //    so the 'new' damage is only part of the damage for this timestamp
                        else if (data.damagePerMinute[timestamp] < damage) {
                            data.totalDamage += damage - data.damagePerMinute[timestamp];
                            data.damagePerMinute[timestamp] = damage;
                            newDamage = true;
                        }
                    }
                }
            }
            return newDamage;
        }

        public static void GenerateDamageResults(Dictionary<string, DamageResult> damageResults, List<string> times) {
            lock (totalDamageResults) {
                foreach (KeyValuePair<string, DamageData> kvp in totalDamageResults) {
                    string player = kvp.Key;
                    DamageData data = kvp.Value;
                    int damage = 0;
                    int minutes = 0;
                    foreach (string t in times) {
                        if (data.damagePerMinute.ContainsKey(t)) {
                            damage += data.damagePerMinute[t];
                            minutes++;
                        }
                    }
                    if (damage > 0) {
                        damageResults.Add(player, new DamageResult { damagePerSecond = damage / (minutes * 60.0), totalDamage = data.totalDamage });
                    }
                }
            }
        }

        public static IEnumerable<string> UpdateCommands(Dictionary<string, List<Tuple<string, string>>> newCommands) {
            lock (totalCommands) {
                foreach (KeyValuePair<string, List<Tuple<string, string>>> kvp in newCommands) {
                    string t = kvp.Key;
                    List<Tuple<string, string>> currentCommands = kvp.Value;
                    if (!totalCommands.ContainsKey(t)) totalCommands[t] = new List<Tuple<string, string>>();
                    if (currentCommands.Count > totalCommands[t].Count) {
                        List<Tuple<string, string>> unseenCommands = new List<Tuple<string, string>>();
                        List<Tuple<string, string>> commandsList = totalCommands[t].ToArray().ToList(); // create a copy of the list
                        foreach (Tuple<string, string> command in currentCommands) {
                            if (!totalCommands[t].Contains(command)) {
                                unseenCommands.Add(command);
                                string player = command.Item1;
                                string cmd = command.Item2;
                                if (SettingsManager.getSetting("Names").Contains(player)) {
                                    yield return cmd;
                                }
                            } else {
                                totalCommands[t].Remove(command);
                            }
                        }
                        commandsList.AddRange(unseenCommands);
                        totalCommands[t] = commandsList;
                    }
                }
            }
            yield break;
        }

        public static void UpdateURLs(Dictionary<string, List<Tuple<string, string>>> newURLs) {
            lock (totalURLs) {
                foreach (KeyValuePair<string, List<Tuple<string, string>>> kvp in newURLs) {
                    string t = kvp.Key;
                    List<Tuple<string, string>> currentURLs = kvp.Value;
                    if (!totalURLs.ContainsKey(t)) {
                        totalURLs.Add(t, currentURLs);
                    } else if (currentURLs.Count > totalURLs[t].Count) {
                        totalURLs[t] = currentURLs;
                    }
                }
            }
        }

        public static IEnumerable<string> UpdateLookInformation(Dictionary<string, List<string>> lookMessages) {
            lock (totalLooks) {
                foreach (KeyValuePair<string, List<string>> kvp in lookMessages) {
                    string t = kvp.Key;
                    List<string> currentMessages = kvp.Value;
                    if (!totalLooks.ContainsKey(t)) totalLooks[t] = new List<string>();
                    if (currentMessages.Count > totalLooks[t].Count) {
                        List<string> unseenLooks = new List<string>();
                        List<string> lookList = totalLooks[t].ToArray().ToList();
                        foreach (string lookMessage in currentMessages) {
                            if (!totalLooks[t].Contains(lookMessage)) {
                                unseenLooks.Add(lookMessage);
                                yield return lookMessage;
                            } else {
                                totalLooks[t].Remove(lookMessage);
                            }
                        }
                        lookList.AddRange(unseenLooks);
                        totalLooks[t] = lookList;
                    }
                }
            }
            yield break;
        }

        public static IEnumerable<Tuple<Event, string>> UpdateEventInformation(List<Tuple<Event, string>> newEvents) {
            lock (eventMessages) {
                foreach (Tuple<Event, string> tpl in newEvents) {
                    if (!eventMessages.Contains(tpl.Item2)) {
                        eventMessages.Add(tpl.Item2);
                        yield return tpl;
                    }
                }
            }
            yield break;
        }

        public static bool AddLevelAdvance(string logMessage) {
            lock (levelAdvances) {
                if (!levelAdvances.Contains(logMessage)) {
                    levelAdvances.Add(logMessage);
                    return true;
                }
            }
            return false;
        }

        public static List<Tuple<string, string>> GetRecentCommands(int type, int max_entries = 15) {
            List<string> times = TimestampManager.getLatestTimes(5);
            times.Reverse();

            Dictionary<string, List<Tuple<string, string>>> dict = type == 0 ? GlobalDataManager.totalCommands : GlobalDataManager.totalURLs;

            List<Tuple<string, string>> results = new List<Tuple<string, string>>();
            lock (dict) {
                foreach (string t in times) {
                    if (dict.ContainsKey(t)) {
                        foreach (Tuple<string, string> tpl in dict[t]) {
                            if (tpl.Item2.ToLower().Contains("recent") || tpl.Item2.ToLower().Contains("last")) continue;
                            results.Add(tpl);
                            if (results.Count >= max_entries) return results;
                        }
                    }
                }
            }
            return results;
        }

        public static IEnumerable<string> GetLookInformation(List<string> times) {
            lock (totalLooks) {
                foreach (string t in times) {
                    if (!totalLooks.ContainsKey(t)) continue;
                    foreach (string message in totalLooks[t]) {
                        yield return message;
                    }
                }
            }
            yield break;
        }

        public static IEnumerable<KeyValuePair<string, int>> GetExperience() {
            lock(totalExperienceResults) {
                foreach(KeyValuePair<string, int> kvp in totalExperienceResults) {
                    yield return kvp;
                }
            }
            yield break;
        }
    }
}
