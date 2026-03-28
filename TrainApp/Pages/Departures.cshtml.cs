using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace TrainApp.Pages
{
    public class DeparturesModel : PageModel
    {
        public class Train
        {
            public string destination { get; set; }
            public string departureTime { get; set; }
            public string platform { get; set; }
        }

        public List<Train> results { get; set; } = new List<Train>();

        public string searchedStation { get; set; }

        private string connectionString = "Data Source=TrainApp.db";

        public void OnGet()
        {
        }

        public void OnPost(string stationName)
        {
            searchedStation = stationName;

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqliteCommand(
                    @"SELECT destination, departureTime, platform 
                  FROM trainSchedule 
                  WHERE stationName = @station", conn);

                cmd.Parameters.AddWithValue("@station", stationName);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new Train
                        {
                            destination = reader["destination"].ToString(),
                            departureTime = reader["departureTime"].ToString(),
                            platform = reader["platform"].ToString()
                        });
                    }
                }
            }
        }
    }

}