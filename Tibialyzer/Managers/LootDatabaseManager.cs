using System;
using System.Data.SQLite;

namespace Tibialyzer {
    class LootDatabaseManager {
        private static SQLiteConnection lootConn;
        public delegate void LootChangedHandler();
        public static event LootChangedHandler LootChanged;

        public static void Initialize() {
            lootConn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", Constants.LootDatabaseFile));
            lootConn.Open();
        }

        public static void Close() {
            lootConn.Close();
            lootConn.Dispose();
        }

        public static void UpdateLoot() {
            LootChanged();
        }

        public static void CreateHuntTable(Hunt hunt) {
            SQLiteCommand command = new SQLiteCommand(String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"(day INTEGER, hour INTEGER, minute INTEGER, message STRING);", hunt.GetTableName()), lootConn);
            command.ExecuteNonQuery();
        }

        public static void DeleteHuntTable(Hunt hunt) {
            SQLiteCommand comm = new SQLiteCommand(String.Format("DROP TABLE IF EXISTS \"{0}\";", hunt.GetTableName()), lootConn);
            comm.ExecuteNonQuery();
        }

        public static SQLiteDataReader GetHuntMessages(Hunt hunt) {
            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT message FROM \"{0}\" ORDER BY day, hour, minute;", hunt.GetTableName()), lootConn);
            return command.ExecuteReader();
        }

        public static bool HuntTableExists(Hunt h) {
            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{0}';", h.GetTableName()), lootConn);
            int value = int.Parse(command.ExecuteScalar().ToString());
            return value != 0;
        }

        public static SQLiteTransaction BeginTransaction() {
            return lootConn.BeginTransaction();
        }

        public static void InsertMessage(Hunt hunt, int stamp, int hour, int minute, string message) {
            SQLiteCommand command = new SQLiteCommand(String.Format("INSERT INTO \"{4}\" VALUES({0}, {1}, {2}, \"{3}\");", stamp, hour, minute, message.Replace("\"", "\\\""), hunt.GetTableName()), lootConn);
            command.ExecuteNonQuery();
        }

        public static void DeleteMessage(Hunt hunt, string msg, SQLiteTransaction transaction) {
            SQLiteCommand command = new SQLiteCommand(String.Format("DELETE FROM \"{0}\" WHERE message=\"{1}\"", hunt.GetTableName(), msg.Replace("\"", "\\\"")), lootConn, transaction);
            command.ExecuteNonQuery();
        }

        public static void DeleteMessagesBefore(Hunt h, int stamp, int hour, int minute) {
            SQLiteCommand comm = new SQLiteCommand(String.Format("DELETE FROM \"{0}\" WHERE day < {1} OR hour < {2} OR (hour == {2} AND minute < {3})", h.GetTableName(), stamp, hour, minute), lootConn);
            comm.ExecuteNonQuery();
        }
    }
}
