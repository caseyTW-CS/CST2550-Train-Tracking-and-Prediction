using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace TrainApp.Pages
{
    public class TrackTrainModel : PageModel
    {
        public string trainName;
        public string start;
        public string end;

        public List<string> route = new List<string>();

        public string currentStation;
        public string nextStation;
        public string status;

        // Live API data
        public List<Arrival> arrivals = new List<Arrival>();

        private List<string> stations = new List<string>
        {
            "Reading",
            "Paddington",
            "Bond Street",
            "Tottenham Court Road",
            "Liverpool Street",
            "Whitechapel",
            "Abbey Wood"
        };

        public async Task OnGet(int id)
        {
            // Train selection
            if (id == 1)
            {
                trainName = "Train A";
                start = "Paddington";
                end = "Abbey Wood";
            }
            else if (id == 2)
            {
                trainName = "Train B";
                start = "Reading";
                end = "Liverpool Street";
            }
            else
            {
                trainName = "Train C";
                start = "Paddington";
                end = "Whitechapel";
            }

            // Build route
            int startIndex = stations.IndexOf(start);
            int endIndex = stations.IndexOf(end);

            for (int i = startIndex; i <= endIndex; i++)
            {
                route.Add(stations[i]);
            }

            // Simulate movement
            int position = DateTime.Now.Second % route.Count;

            currentStation = route[position];

            if (position < route.Count - 1)
            {
                nextStation = route[position + 1];
                status = "Between stations";
            }
            else
            {
                nextStation = "Final Stop";
                status = "Arrived";
            }

            // Load real TfL data
            await LoadLiveArrivals();
        }

        private async Task LoadLiveArrivals()
        {
            using (var client = new HttpClient())
            {
                var url = "https://api.tfl.gov.uk/Line/elizabeth/Arrivals";

                var response = await client.GetStringAsync(url);

                var data = JsonSerializer.Deserialize<List<Arrival>>(response);

                if (data != null)
                {
                    // take only first 5 trains
                    arrivals = data.GetRange(0, Math.Min(5, data.Count));
                }
            }
        }

        public class Arrival
        {
            public string destinationName { get; set; }
            public int timeToStation { get; set; }
            public string stationName { get; set; }
        }
    }
}