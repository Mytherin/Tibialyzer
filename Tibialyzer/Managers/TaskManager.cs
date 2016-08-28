using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tibialyzer {
    class TaskManager {
        private static Dictionary<int, int> taskKills = new Dictionary<int, int>();
        private static Dictionary<int, bool> trackedTasks = new Dictionary<int, bool>();
        private static object killUpdate = new object();
        public delegate void TaskKillUpdateHandler(Task task, int newKills);
        public static event TaskKillUpdateHandler TaskUpdated;
        public delegate void TaskUpdateHandler(Task task);
        public static event TaskUpdateHandler UntrackTask;
        public static event TaskUpdateHandler TrackTask;

        public static void Initialize() {
            SQLiteDataReader reader = LootDatabaseManager.ExecuteReaderQuery("SELECT taskid, tracked, kills FROM TrackedTasks;");
            while(reader.Read()) {
                int taskid = reader.GetInt32(0);
                bool tracked = reader.GetBoolean(1);
                int killCount = reader.GetInt32(2);
                taskKills.Add(taskid, killCount);
                trackedTasks.Add(taskid, tracked);
            }
            
            HuntManager.NewCreatureKill += UpdateTaskKills;
        }

        private static void UpdateTaskKills(Creature creature) {
            lock(killUpdate) {
                foreach (var kvp in trackedTasks) {
                    if (kvp.Value) {
                        Task task = StorageManager.getTask(kvp.Key);
                        if (task.creatures.Contains(creature.id)) {
                            int newKills = taskKills[kvp.Key] + 1;
                            LootDatabaseManager.ExecuteNonQuery(String.Format("UPDATE TrackedTasks SET kills={0} WHERE taskid={1}", newKills, kvp.Key));
                            taskKills[kvp.Key] = newKills;
                            if (TaskUpdated != null) TaskUpdated(task, newKills);
                        }
                    }
                }
            }
        }

        public static List<Task> GetTrackedTasks() {
            List<Task> tasks = new List<Task>();
            foreach(var kvp in trackedTasks) {
                if (kvp.Value) {
                    tasks.Add(StorageManager.getTask(kvp.Key));
                }
            }
            return tasks;
        }

        public static void ChangeTracked(int taskid, bool tracked) {
            lock(killUpdate) {
                if (!trackedTasks.ContainsKey(taskid)) {
                    LootDatabaseManager.ExecuteNonQuery(String.Format("INSERT INTO TrackedTasks (taskid, tracked, kills) VALUES ({0},{1},{2});", taskid, tracked ? 1 : 0, 0));
                    trackedTasks.Add(taskid, tracked);
                    taskKills.Add(taskid, 0);
                } else {
                    LootDatabaseManager.ExecuteNonQuery(String.Format("UPDATE TrackedTasks SET tracked={0} WHERE taskid={1}", tracked ? 1 : 0, taskid));
                    trackedTasks[taskid] = tracked;
                }
                if (tracked) {
                    if (TrackTask != null) TrackTask(StorageManager.getTask(taskid));
                } else {
                    if (UntrackTask != null) UntrackTask(StorageManager.getTask(taskid));
                }
            }
        }

        public static bool IsTracked(int taskid) {
            if (!trackedTasks.ContainsKey(taskid)) return false;
            return trackedTasks[taskid];
        }

        public static void ChangeKillCount(int taskid, int value) {
            lock(killUpdate) {
                if (!taskKills.ContainsKey(taskid)) {
                    LootDatabaseManager.ExecuteNonQuery(String.Format("INSERT INTO TrackedTasks (taskid, tracked, kills) VALUES ({0},{1},{2});", taskid, 0, value));
                    trackedTasks.Add(taskid, false);
                    taskKills.Add(taskid, value);
                } else {
                    LootDatabaseManager.ExecuteNonQuery(String.Format("UPDATE TrackedTasks SET kills={0} WHERE taskid={1}", value, taskid));
                    taskKills[taskid] = value;
                    if (TaskUpdated != null) TaskUpdated(StorageManager.getTask(taskid), value);
                }
            }
        }

        public static int GetKillCount(int taskid) {
            if (!taskKills.ContainsKey(taskid)) return 0;
            return taskKills[taskid];
        }
    }
}
