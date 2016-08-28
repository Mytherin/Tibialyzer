using System;
using System.Data.SQLite;
using System.IO;

namespace Tibialyzer {
    class LootDatabaseManager {
        private static SQLiteConnection lootConn;
        public delegate void LootChangedHandler();
        public static event LootChangedHandler LootChanged;
        private static object lootLock = new object();
        private static bool LootUpdated;


        public static void Initialize() {
            lock (lootLock) {
                OpenConnection();
            }
            LootUpdated = false;
        }

        public static void LootUpdatedEvent() {
            if (LootUpdated) {
                if (LootChanged != null) {
                    LootChanged();
                }
                LootUpdated = false;
            }
        }

        public static void Close() {
            lock(lootLock) {
                lootConn.Close();
                lootConn.Dispose();
                lootConn = null;
            }
        }

        public static void ReplaceDatabase(string otherDatabase) {
            lock(lootLock) {
                lootConn.Close();
                lootConn.Dispose();
                lootConn = null;

                File.Delete(Constants.LootDatabaseFile);
                File.Copy(otherDatabase, Constants.LootDatabaseFile);

                OpenConnection();
            }
        }

        private static void OpenConnection() {
            lootConn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", Constants.LootDatabaseFile));
            lootConn.Open();
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS TrackedTasks(taskid INTEGER PRIMARY KEY, tracked BOOLEAN, kills INTEGER);", lootConn);
            command.ExecuteNonQuery();
        }

        public static void UpdateLoot() {
            LootUpdated = true;
        }

        public static void ExecuteNonQuery(string query) {
            lock(lootLock) {
                if (lootConn == null) return;
                SQLiteCommand command = new SQLiteCommand(query, lootConn);
                command.ExecuteNonQuery();
            }
        }

        public static SQLiteDataReader ExecuteReaderQuery(string query) {
            lock (lootLock) {
                if (lootConn == null) return null;
                SQLiteCommand command = new SQLiteCommand(query, lootConn);
                return command.ExecuteReader();
            }
        }
        public static object ExecuteScalar(string query) {
            lock(lootLock) {
                if (lootConn == null) return null;
                SQLiteCommand command = new SQLiteCommand(query, lootConn);
                return command.ExecuteScalar();
            }
        }
        
        public static void CreateHuntTable(Hunt hunt) {
            ExecuteNonQuery(String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"(day INTEGER, hour INTEGER, minute INTEGER, message STRING);", hunt.GetTableName()));
            ExecuteNonQuery(String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"(itemid INTEGER, amount INTEGER);", hunt.GetWasteTableName()));
        }

        public static void DeleteHuntTable(Hunt hunt) {
            ExecuteNonQuery(String.Format("DROP TABLE IF EXISTS \"{0}\";", hunt.GetTableName()));
            ExecuteNonQuery(String.Format("DROP TABLE IF EXISTS \"{0}\";", hunt.GetWasteTableName()));
        }

        public static SQLiteDataReader GetHuntMessages(Hunt hunt) {
            return ExecuteReaderQuery(String.Format("SELECT message FROM \"{0}\" ORDER BY day, hour, minute;", hunt.GetTableName()));
        }


        public static bool HuntTableExists(Hunt h) {
            int value = 0;
            object result = ExecuteScalar(String.Format("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{0}';", h.GetTableName()));
            if (result != null && int.TryParse(result.ToString(), out value)) {
                return value != 0;
            }
            return false;
        }

        public static SQLiteTransaction BeginTransaction() {
            lock(lootLock) {
                if (lootLock == null) return null;
                return lootConn.BeginTransaction();
            }
        }

        public static void InsertMessage(Hunt hunt, int stamp, int hour, int minute, string message) {
            ExecuteNonQuery(String.Format("INSERT INTO \"{4}\" VALUES({0}, {1}, {2}, \"{3}\");", stamp, hour, minute, message.Replace("\"", "\\\""), hunt.GetTableName()));
        }

        public static void DeleteMessage(Hunt hunt, string msg, SQLiteTransaction transaction) {
            ExecuteNonQuery(String.Format("DELETE FROM \"{0}\" WHERE message=\"{1}\"", hunt.GetTableName(), msg.Replace("\"", "\\\"")));
        }

        public static void DeleteMessagesBefore(Hunt h, int stamp, int hour, int minute) {
            ExecuteNonQuery(String.Format("DELETE FROM \"{0}\" WHERE day < {1} OR hour < {2} OR (hour == {2} AND minute < {3})", h.GetTableName(), stamp, hour, minute));
        }

        public static void UpdateUsedItems(Hunt h) {
            var usedItems = h.GetUsedItems();
            lock(lootLock) {
                foreach (var item in usedItems) {
                    int itemid = item.Item1.id;
                    int amount = item.Item2;

                    int value = 0;
                    object result = ExecuteScalar(String.Format("SELECT itemid FROM {0} WHERE itemid={1}", h.GetWasteTableName(), itemid));
                    if (result != null && int.TryParse(result.ToString(), out value)) {
                        ExecuteNonQuery(String.Format("UPDATE {0} SET amount={1} WHERE itemid={2}", h.GetWasteTableName(), amount, itemid));
                    } else {
                        ExecuteNonQuery(String.Format("INSERT INTO {0} (itemid, amount) VALUES ({1}, {2})", h.GetWasteTableName(), itemid, amount));
                    }
                }
            }
        }

        public static SQLiteDataReader GetUsedItems(Hunt h) {
            return ExecuteReaderQuery(String.Format("SELECT itemid, amount FROM {0}", h.GetWasteTableName()));
        }
    }
}
