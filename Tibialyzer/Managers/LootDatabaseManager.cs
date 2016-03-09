using System;
using System.Data.SQLite;
using System.IO;

namespace Tibialyzer {
    class LootDatabaseManager {
        private static SQLiteConnection lootConn;
        public delegate void LootChangedHandler();
        public static event LootChangedHandler LootChanged;
        private static object lootLock = new object();

        public static void Initialize() {
            lock (lootLock) {
                lootConn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", Constants.LootDatabaseFile));
                lootConn.Open();
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

                lootConn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", Constants.LootDatabaseFile));
                lootConn.Open();
            }
        }

        public static void UpdateLoot() {
            if (LootChanged != null) {
                LootChanged();
            }
        }

        private static void ExecuteNonQuery(string query) {
            lock(lootLock) {
                if (lootConn == null) return;
                SQLiteCommand command = new SQLiteCommand(query, lootConn);
                command.ExecuteNonQuery();
            }
        }

        private static SQLiteDataReader ExecuteReaderQuery(string query) {
            lock (lootLock) {
                if (lootConn == null) return null;
                SQLiteCommand command = new SQLiteCommand(query, lootConn);
                return command.ExecuteReader();
            }
        }

        public static void CreateHuntTable(Hunt hunt) {
            ExecuteNonQuery(String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"(day INTEGER, hour INTEGER, minute INTEGER, message STRING);", hunt.GetTableName()));
        }

        public static void DeleteHuntTable(Hunt hunt) {
            ExecuteNonQuery(String.Format("DROP TABLE IF EXISTS \"{0}\";", hunt.GetTableName()));
        }

        public static SQLiteDataReader GetHuntMessages(Hunt hunt) {
            return ExecuteReaderQuery(String.Format("SELECT message FROM \"{0}\" ORDER BY day, hour, minute;", hunt.GetTableName()));
        }

        public static bool HuntTableExists(Hunt h) {
            lock(lootLock) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{0}';", h.GetTableName()), lootConn);
                int value = int.Parse(command.ExecuteScalar().ToString());
                return value != 0;
            }
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
    }
}
