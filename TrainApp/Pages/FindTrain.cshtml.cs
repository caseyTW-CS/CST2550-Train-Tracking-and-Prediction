using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace TrainApp.Pages
{
    public class FindTrainModel : PageModel
    {
        public List<Station> Stations = new List<Station>();

        [BindProperty]
        public string StartId { get; set; }

        [BindProperty]
        public string Destination { get; set; }

        public List<Arrival> Results = new List<Arrival>();

        public string errorMessage;

        private List<string> stationOrder = new List<string>
        {
            "reading",
            "twyford",
            "maidenhead",
            "taplow",
            "burnham",
            "slough",
            "langley",
            "iver",
            "west drayton",
            "hayes & harlington",
            "southall",
            "hanwell",
            "west ealing",
            "ealing broadway",
            "acton main line",
            "paddington",
            "bond street",
            "tottenham court road",
            "farringdon",
            "liverpool street",
            "whitechapel",
            "canary wharf",
            "custom house",
            "woolwich",
            "abbey wood"
        };

        public async Task OnGet()
        {
            await LoadStations();
        }

        public async Task OnPost()
        {
            await LoadStations();
            Results.Clear();

            if (string.IsNullOrEmpty(StartId) || string.IsNullOrEmpty(Destination))
            {
                errorMessage = "Please select a start station and destination.";
                return;
            }

            string startName = GetStationName(StartId).Trim().ToLower();
            string destinationName = Destination.Trim().ToLower();

            int startIndex = stationOrder.IndexOf(startName);
            int endIndex = stationOrder.IndexOf(destinationName);

            if (startIndex == -1 || endIndex == -1)
            {
                errorMessage = "Invalid stations selected.";
                return;
            }

            if (startIndex >= endIndex)
            {
                errorMessage = "Invalid route direction.";
                return;
            }

            using (var client = new HttpClient())
            {
                var url = $"https://api.tfl.gov.uk/StopPoint/{StartId}/Arrivals";
                var response = await client.GetStringAsync(url);

                var data = JsonSerializer.Deserialize<List<Arrival>>(response);

                if (data == null)
                {
                    errorMessage = "No data found.";
                    return;
                }

                foreach (var train in data)
                {
                    if (!string.IsNullOrEmpty(train.destinationName))
                    {
                        string dest = train.destinationName.Trim().ToLower();

                        int trainIndex = stationOrder.FindIndex(s => dest.Contains(s));

                        if (trainIndex != -1 && trainIndex >= endIndex)
                        {
                            Results.Add(train);
                        }
                    }
                }

                Results = Results.OrderBy(t => t.timeToStation).ToList();
                Results = Results.Take(10).ToList();

                if (Results.Count == 0)
                {
                    errorMessage = "No trains found for this route.";
                }
            }
        }

        private async Task LoadStations()
        {
            using (var client = new HttpClient())
            {
                var url = "https://api.tfl.gov.uk/Line/elizabeth/StopPoints";
                var response = await client.GetStringAsync(url);

                Stations = JsonSerializer.Deserialize<List<Station>>(response);
            }
        }

        public string GetStationName(string id)
        {
            foreach (var station in Stations)
            {
                if (station.id == id)
                {
                    return station.commonName;
                }
            }
            return "";
        }

        public string GetArrivalTime(int seconds)
        {
            if (seconds <= 0)
                return "At station now";

            DateTime arrival = DateTime.Now.AddSeconds(seconds);
            return arrival.ToString("HH:mm");
        }

        public class Station
        {
            public string id { get; set; }
            public string commonName { get; set; }
        }

        public class Arrival
        {
            public string destinationName { get; set; }
            public int timeToStation { get; set; }
        }
    }
}