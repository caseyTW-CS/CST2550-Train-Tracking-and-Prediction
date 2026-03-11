using System;

namespace TestReposit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Server connection string - used when importing data from SQL
            string connectionString =
            "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            Console.WriteLine("Train Prediction System Initialised.");

            // data loading from SQL will be implemented here later
            // once stations, trains and schedules are imported,
            // prediction and journey calculations will run

            Console.ReadLine();
        }
    }
}