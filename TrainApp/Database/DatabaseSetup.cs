using Microsoft.Data.Sqlite;
using System.IO;

namespace TrainApp.Database
{
    public static class DatabaseSetup
    {
        public static void Initialise()
        {
            string dbPath = "TrainsDatabase.db";

            if (!File.Exists(dbPath))
            {
                using var connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                string sql = File.ReadAllText("Database/TrainsDatabase.sql");

                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }
}