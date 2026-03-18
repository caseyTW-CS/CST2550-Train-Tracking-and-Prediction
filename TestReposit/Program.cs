using System;
using System.Data.SqlClient;

namespace TestReposit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Server connection string - used when importing data from SQL
            string connectionString =
            "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT userID, userName FROM userInfo";

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["userID"]}, Name: {reader["userName"]}");
                }

                // data loading from SQL will be implemented here later
                // once stations, trains and schedules are imported,
                // prediction and journey calculations will run

                Console.ReadLine();
            }
        }
    }
}