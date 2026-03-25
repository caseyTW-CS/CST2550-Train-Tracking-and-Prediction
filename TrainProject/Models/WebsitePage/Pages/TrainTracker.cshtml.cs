using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrainProject.Models.WebsitePage.Pages
{
    public class TrainArrival
    {
        public string StationName { get; set; }
        public string Destination { get; set; }
        public string ExpectedArrival { get; set; }
        public string TimeToStation { get; set; }
        public string CurrentLocation { get; set; }
        public string VehicleId { get; set; }
        public string Platform { get; set; }
    }

    public class TrainTrackerModel : PageModel
    {
        private const string ApiKey = "bd620c86b82b487bb8d7b7f41c54d10b";
        private static readonly HttpClient client = new HttpClient();

        // connection string for the trains database
        private string connectionString =
            "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

        public string searchedTrain { get; set; }
        public List<TrainArrival> arrivals { get; set; } = new List<TrainArrival>();
        public string errorMessage { get; set; }
        // tells the page whether the data came from the database or TfL API
        public string dataSource { get; set; }

        public async Task OnGetAsync(string trainId)
        {
            searchedTrain = trainId;
            if (!string.IsNullOrEmpty(trainId))
            {
                // check the database first
                bool foundInDatabase = loadFromDatabase(trainId);

                // if nothing found in database fall back to TfL API
                if (!foundInDatabase)
                    await loadFromTflApi(trainId);
            }
        }

        // checks our own database for the train first
        // returns true if it found something, false if not
        private bool loadFromDatabase(string trainId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // look for the train in our database by train number
                    string query = @"SELECT t.trainNumber, j.scheduledArrival, 
                                    j.currentDelayMinutes, j.journeyStatus,
                                    j.actualArrival, ds.stationName as departureStation,
                                    ar.stationName as arrivalStation, ar.stationPlatform
                                    FROM Train t
                                    JOIN Journey j ON j.trainID = t.trainID
                                    JOIN Station ds ON j.departureStationID = ds.stationID
                                    JOIN Station ar ON j.arrivalStationID = ar.stationID
                                    WHERE t.trainNumber LIKE @trainId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@trainId", "%" + trainId + "%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // work out predicted arrival by adding delay to scheduled time
                        DateTime scheduledArrival = (DateTime)reader["scheduledArrival"];
                        int delayMins = (int)reader["currentDelayMinutes"];
                        DateTime predictedArrival = scheduledArrival.AddMinutes(delayMins);

                        arrivals.Add(new TrainArrival
                        {
                            VehicleId = reader["trainNumber"].ToString(),
                            StationName = reader["departureStation"].ToString(),
                            Destination = reader["arrivalStation"].ToString(),
                            ExpectedArrival = predictedArrival.ToString("HH:mm"),
                            TimeToStation = delayMins > 0
                                ? $"{delayMins} mins delay"
                                : "On Time",
                            CurrentLocation = $"Between {reader["departureStation"]} and {reader["arrivalStation"]}",
                            Platform = reader["stationPlatform"].ToString()
                        });
                    }
                }

                if (arrivals.Count > 0)
                {
                    // found in our database
                    dataSource = "database";
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                // database error - fall back to TfL API
                errorMessage = $"Database error: {e.Message}";
                return false;
            }
        }

        // falls back to TfL API if the train wasnt found in our database
        // your classmates original code kept exactly as they wrote it
        private async Task loadFromTflApi(string trainId)
        {
            try
            {
                dataSource = "tfl";
                string url = $"https://api.tfl.gov.uk/Line/elizabeth/Arrivals?app_key={ApiKey}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = "Could not connect to TfL API";
                    return;
                }

                string json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                foreach (JsonElement train in root.EnumerateArray())
                {
                    // filter by vehicle id if user searched for a specific train
                    string vehicleId = train.GetProperty("vehicleId").GetString();
                    if (!vehicleId.Contains(trainId, StringComparison.OrdinalIgnoreCase))
                        continue;

                    arrivals.Add(new TrainArrival
                    {
                        VehicleId = vehicleId,
                        StationName = train.GetProperty("stationName").GetString()
                            .Replace(" Underground Station", "")
                            .Replace(" Rail Station", ""),
                        Destination = train.TryGetProperty("destinationName", out var dest)
                            ? dest.GetString().Replace(" Rail Station", "") : "Unknown",
                        ExpectedArrival = DateTime.Parse(
                            train.GetProperty("expectedArrival").GetString()).ToString("HH:mm"),
                        TimeToStation = (train.GetProperty("timeToStation").GetInt32() / 60).ToString() + " mins",
                        CurrentLocation = train.TryGetProperty("currentLocation", out var loc)
                            ? loc.GetString() : "--",
                        Platform = train.TryGetProperty("platformName", out var plat)
                            ? plat.GetString() : "--"
                    });
                }

                if (arrivals.Count == 0)
                    errorMessage = $"No trains found matching '{trainId}' in our database or TfL live data";
            }
            catch (Exception e)
            {
                errorMessage = $"Error: {e.Message}";
            }
        }
    }
}