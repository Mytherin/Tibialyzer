using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Globalization;

namespace Tibialyzer {
    public class NotificationConditionManager {
        public static bool ResolveConditions(Tuple<Creature, List<Tuple<Item, int>>> dropInformation) {
            List<string> conditions = SettingsManager.getSetting("NotificationConditions");
            var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();

            SQLiteCommand command;
            command = new SQLiteCommand("CREATE TABLE item(name STRING, value INTEGER, capacity DOUBLE, count INTEGER)", connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE TABLE creature(name STRING, exp INTEGER, hp INTEGER)", connection);
            command.ExecuteNonQuery();

            if (dropInformation != null) {
                Creature cr = dropInformation.Item1;
                if (cr != null) {
                    command = new SQLiteCommand(String.Format("INSERT INTO creature (name, exp, hp) VALUES (\"{0}\",{1},{2})", cr.GetName().Replace("\"", "\\\""), cr.experience, cr.health), connection);
                    command.ExecuteNonQuery();
                    if (dropInformation.Item2 != null) {
                        foreach (Tuple<Item, int> tpl in dropInformation.Item2) {
                            Item it = tpl.Item1;
                            int count = tpl.Item2;
                            if (it == null) continue;
                            command = new SQLiteCommand(String.Format("INSERT INTO item (name, value, capacity, count) VALUES (\"{0}\",{1},{2},{3})", it.GetName().Replace("\"", "\\\""), it.GetMaxValue(), it.capacity.ToString(CultureInfo.InvariantCulture), count), connection);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            foreach (string condition in conditions) {
                try {
                    command = new SQLiteCommand(String.Format("SELECT {0} FROM item,creature", condition), connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        if (reader.GetInt32(0) > 0) {
                            return true;
                        }
                    }
                } catch {

                }
            }
            return false;
        }
    }
}
