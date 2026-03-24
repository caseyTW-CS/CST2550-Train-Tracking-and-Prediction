using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestReposit.Models.WebsitePage.Pages
{
    public class DepartureEntry
    {
        public string Destination { get; set; }
        public string ScheduledDeparture { get; set; }
        public string Status { get; set; }
        public string Platform { get; set; }
        public int DelayMinutes { get; set; }
    }

    public class ArrivalEntry
    {
        public string Origin { get; set; }
        public string ScheduledArrival { get; set; }
        public string Status { get; set; }
        public string Platform { get; set; }
        public int DelayMinutes { get; set; }
    }

    public class DeparturesModel : PageModel
    {
        public string searchedStation { get; set; }
        public List<DepartureEntry> departures { get; set; } = new List<DepartureEntry>();
        public List<ArrivalEntry> arrivals { get; set; } = new List<ArrivalEntry>();

        public void OnGet(string station)
        {
            searchedStation = station;
            if (!string.IsNullOrEmpty(station))
            {
                loadDepartures(station);
                loadArrivals(station);
            }
        }

        private void loadDepartures(string stationName)
        {
            string connectionString =
                "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT j.scheduledDeparture, j.currentDelayMinutes, 
                                    j.journeyStatus, s.stationName as destination, 
                                    s.stationPlatform
                                    FROM Journey j
                                    JOIN Station s ON j.arrivalStationID = s.stationID
                                    JOIN Station ds ON j.departureStationID = ds.stationID
                                    WHERE ds.stationName = @stationName";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@stationName", stationName);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        departures.Add(new DepartureEntry
                        {
                            Destination = reader["destination"].ToString(),
                            ScheduledDeparture = Convert.ToDateTime(reader["scheduledDeparture"]).ToString("ddd dd MMM, HH:mm"),
                            Status = reader["journeyStatus"].ToString(),
                            Platform = reader["stationPlatform"].ToString(),
                            DelayMinutes = reader["currentDelayMinutes"] != DBNull.Value
                                ? Convert.ToInt32(reader["currentDelayMinutes"]) : 0
                        });
                    }
                }
            }
            catch (Exception e)
            {
                // if something goes wrong show an error message
                departures.Add(new DepartureEntry { Destination = $"Error: {e.Message}" });
            }
        }

        private void loadArrivals(string stationName)
        {
            string connectionString =
                "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT j.scheduledArrival, j.currentDelayMinutes,
                                    j.journeyStatus, s.stationName as origin,
                                    s.stationPlatform
                                    FROM Journey j
                                    JOIN Station s ON j.departureStationID = s.stationID
                                    JOIN Station ar ON j.arrivalStationID = ar.stationID
                                    WHERE ar.stationName = @stationName";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@stationName", stationName);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        arrivals.Add(new ArrivalEntry
                        {
                            Origin = reader["origin"].ToString(),
                            ScheduledArrival = Convert.ToDateTime(reader["scheduledArrival"]).ToString("ddd dd MMM, HH:mm"),
                            Status = reader["journeyStatus"].ToString(),
                            Platform = reader["stationPlatform"].ToString(),
                            DelayMinutes = reader["currentDelayMinutes"] != DBNull.Value
                                ? Convert.ToInt32(reader["currentDelayMinutes"]) : 0
                        });
                    }
                }
            }
            catch (Exception e)
            {
                // if something goes wrong show an error message
                arrivals.Add(new ArrivalEntry { Origin = $"Error: {e.Message}" });
            }
        }
    }
}