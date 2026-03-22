using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestReposit.Models.WebsitePage.Pages
{
    public class DeparturesModel : PageModel
    {
        // stores the search term the user typed
        public string searchedStation { get; set; }
        // stores the departures to display on the page
        public List<string> departures { get; set; } = new List<string>();
        // stores the arrivals to display on the page
        public List<string> arrivals { get; set; } = new List<string>();

        // this runs when the page first loads
        public void OnGet(string station)
        {
            searchedStation = station;
            // if the user searched for a station load the data
            if (!string.IsNullOrEmpty(station))
            {
                loadDepartures(station);
                loadArrivals(station);
            }
        }

        // queries the database for departures from the searched station
        private void loadDepartures(string stationName)
        {
            string connectionString =
                "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // get all journeys departing from the searched station
                    string query = @"SELECT j.scheduledDeparture, j.scheduledArrival, 
                                    j.currentDelayMinutes, j.journeyStatus, 
                                    s.stationName as destination, s.stationPlatform
                                    FROM Journey j
                                    JOIN Station s ON j.arrivalStationID = s.stationID
                                    JOIN Station ds ON j.departureStationID = ds.stationID
                                    WHERE ds.stationName = @stationName";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@stationName", stationName);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // format each departure as a readable string for now
                        departures.Add($"{reader["destination"]} | " +
                                      $"{reader["scheduledDeparture"]} | " +
                                      $"{reader["journeyStatus"]} | " +
                                      $"Platform {reader["stationPlatform"]}");
                    }
                }
            }
            catch (Exception e)
            {
                // if something goes wrong show an error message
                departures.Add($"Error loading departures: {e.Message}");
            }
        }

        // queries the database for arrivals at the searched station
        private void loadArrivals(string stationName)
        {
            string connectionString =
                "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // get all journeys arriving at the searched station
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
                        // format each arrival as a readable string for now
                        arrivals.Add($"{reader["origin"]} | " +
                                    $"{reader["scheduledArrival"]} | " +
                                    $"{reader["journeyStatus"]} | " +
                                    $"Platform {reader["stationPlatform"]}");
                    }
                }
            }
            catch (Exception e)
            {
                // if something goes wrong show an error message
                arrivals.Add($"Error loading arrivals: {e.Message}");
            }
        }
    }
}
