using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace TrainApp.Pages
{
    public class DeparturesModel : PageModel
    {
        public class Train
        {
            public string? destination { get; set; }
            public string? departureTime { get; set; }
            public string? platform { get; set; }
        }

        public class ArrivalTrain
        {
            public string? origin { get; set; }
            public string? arrivalTime { get; set; }
            public string? platform { get; set; }
        }

        public List<Train> results { get; set; } = new();
        public List<ArrivalTrain> arrivals { get; set; } = new();

        public string? searchedStation { get; set; }

        [BindProperty]
        public string? stationName { get; set; }

        private string connectionString = "Data Source=TrainApp.db";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (string.IsNullOrWhiteSpace(stationName))
                return;

            searchedStation = stationName.Trim();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                // ✅ DEPARTURES (case-insensitive + sorted)
                var departuresCmd = new SqliteCommand(
                    @"SELECT destination, departureTime, platform
                      FROM trainSchedule
                      WHERE LOWER(stationName) = LOWER(@station)
                      ORDER BY departureTime ASC", conn);

                departuresCmd.Parameters.AddWithValue("@station", searchedStation);

                using (var reader = departuresCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new Train
                        {
                            destination = reader["destination"]?.ToString(),
                            departureTime = reader["departureTime"]?.ToString(),
                            platform = reader["platform"]?.ToString()
                        });
                    }
                }

                // ✅ ARRIVALS (correct columns + case-insensitive + sorted)
                var arrivalsCmd = new SqliteCommand(
                    @"SELECT origin, arrivalTime, platform
                      FROM trainSchedule
                      WHERE LOWER(destination) = LOWER(@station)
                      ORDER BY arrivalTime ASC", conn);

                arrivalsCmd.Parameters.AddWithValue("@station", searchedStation);

                using (var reader = arrivalsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        arrivals.Add(new ArrivalTrain
                        {
                            origin = reader["origin"]?.ToString(),
                            arrivalTime = reader["arrivalTime"]?.ToString(),
                            platform = reader["platform"]?.ToString()
                        });
                    }
                }
            }
        }
    }
}