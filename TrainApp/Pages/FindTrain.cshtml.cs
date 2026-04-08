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
        public string StartName { get; set; }

        [BindProperty]
        public string StartIcs { get; set; }

        [BindProperty]
        public string Destination { get; set; }

        [BindProperty]
        public string LeaveOption { get; set; } = "now";

        [BindProperty]
        public string LeaveTime { get; set; }

        [BindProperty]
        public string LeaveDate { get; set; }

        public List<JourneyResult> JourneyResults = new List<JourneyResult>();
        public bool IsScheduled { get; set; } = false;

        public List<Arrival> Results = new List<Arrival>();

        public string errorMessage;

        private List<string> stationOrder = new List<string>
{
    "reading","twyford","maidenhead","slough","burnham","taplow","iver","langley",
    "west drayton","hayes & harlington","southall","hanwell","west ealing",
    "ealing broadway","acton main line","paddington","bond street",
    "tottenham court road","farringdon","liverpool street","whitechapel",
    "stratford","maryland","forest gate","manor park","ilford","seven kings",
    "goodmayes","chadwell heath","romford","gidea park","harold wood",
    "brentwood","shenfield","canary wharf","custom house","woolwich","abbey wood"
};

        public async Task OnGet()
        {
            await LoadStations();
        }

        public async Task OnPost()
        {
            await LoadStations();
            Results.Clear();
            JourneyResults = new List<JourneyResult>();

            if (string.IsNullOrEmpty(StartId) || string.IsNullOrEmpty(Destination))
            {
                errorMessage = "Please select a start station and destination.";
                return;
            }

            string startName = !string.IsNullOrEmpty(StartName)
                ? StartName.Trim().ToLower()
                : (GetStationName(StartId) is var n && !string.IsNullOrEmpty(n) ? n : StartId).Trim().ToLower();

            string destinationName = Destination.Trim().ToLower();

            int startIndex = stationOrder.FindIndex(s =>
    startName.Contains(s) || s.Contains(startName) ||
    startName.Replace(" rail station", "").Replace(" underground station", "").Contains(s));
            int endIndex = stationOrder.FindIndex(s =>
                destinationName.Contains(s) || s.Contains(destinationName) ||
                destinationName.Replace(" rail station", "").Replace(" underground station", "").Contains(s));

            if (startIndex == -1 || endIndex == -1)
            {
                errorMessage = $"Could not match stations. Got: '{startName}' and '{destinationName}'";
                return;
            }

            if (startIndex == endIndex)
            {
                errorMessage = "Start and destination cannot be the same.";
                return;
            }

            if (LeaveOption == "time" || LeaveOption == "datetime")
            {
                IsScheduled = true;
                await LoadJourneyPlanner();
            }
            else
            {
                IsScheduled = false;
                await LoadLiveArrivals(endIndex);
            }
        }

        private async Task LoadLiveArrivals(int endIndex)
        {
            int startIndex = stationOrder.FindIndex(s =>
            {
                string startName = (!string.IsNullOrEmpty(StartName)
                    ? StartName : GetStationName(StartId)).Trim().ToLower();
                return startName.Contains(s) || s.Contains(startName);
            });

            bool isEastbound = endIndex > startIndex;

            using (var client = new HttpClient())
            {
                var url = $"https://api.tfl.gov.uk/StopPoint/{StartId}/Arrivals";
                var response = await client.GetStringAsync(url);
                var data = JsonSerializer.Deserialize<List<Arrival>>(response);

                if (data == null) { errorMessage = "No data found."; return; }

                foreach (var train in data)
                {
                    if (string.IsNullOrEmpty(train.destinationName)) continue;

                    string dest = train.destinationName.Trim().ToLower();
                    int trainDestIndex = stationOrder.FindIndex(s => dest.Contains(s) || s.Contains(dest));

                    // If train destination isn't in our list, skip it
                    if (trainDestIndex == -1) continue;

                    // Train must be going in the right direction and far enough
                    if (isEastbound && trainDestIndex >= endIndex)
                        Results.Add(train);
                    else if (!isEastbound && trainDestIndex <= endIndex)
                        Results.Add(train);
                }

                Results = Results.OrderBy(t => t.timeToStation).ToList();
                Results = Results.Take(10).ToList();

                if (Results.Count == 0)
                    errorMessage = "No trains found for this route.";
            }
        }
        private async Task LoadJourneyPlanner()
        {
            try
            {
                // Build ICS code for destination by looking it up from stations list
                string destIcs = "";
                string destName = Destination.Trim().ToLower();
                foreach (var station in Stations)
                {
                    if (station.commonName.Trim().ToLower().Contains(destName) ||
                        destName.Contains(station.commonName.Trim().ToLower()))
                    {
                        destIcs = station.icsCode ?? "";
                        break;
                    }
                }

                string fromCode = !string.IsNullOrEmpty(StartIcs) ? StartIcs : StartId;
                string toCode = !string.IsNullOrEmpty(destIcs) ? destIcs : Uri.EscapeDataString(Destination.Trim());

                string dateStr = "";
                string timeStr = "";

                if (!string.IsNullOrEmpty(LeaveDate) && !string.IsNullOrEmpty(LeaveTime))
                {
                    var now = DateTime.Now;
                    var dateParts = LeaveDate.Split('/');
                    if (dateParts.Length == 2)
                    {
                        int month = int.Parse(dateParts[0]);
                        int day = int.Parse(dateParts[1]);
                        dateStr = $"{now.Year}{month:D2}{day:D2}";
                    }
                    timeStr = LeaveTime.Replace(":", "");
                }
                else if (!string.IsNullOrEmpty(LeaveTime))
                {
                    dateStr = DateTime.Now.ToString("yyyyMMdd");
                    timeStr = LeaveTime.Replace(":", "");
                }

                using (var client = new HttpClient())
                {
                    var url = $"https://api.tfl.gov.uk/Journey/JourneyResults/{fromCode}/to/{toCode}?mode=elizabeth-line&date={dateStr}&time={timeStr}&timeIs=Departing";
                    var response = await client.GetStringAsync(url);
                    var doc = JsonDocument.Parse(response);

                    if (!doc.RootElement.TryGetProperty("journeys", out var journeys))
                    {
                        errorMessage = "No journeys found.";
                        return;
                    }

                    foreach (var journey in journeys.EnumerateArray())
                    {
                        string depTime = "";
                        string arrTime = "";
                        int duration = 0;

                        if (journey.TryGetProperty("startDateTime", out var startDt))
                            depTime = DateTime.Parse(startDt.GetString()).ToString("HH:mm");
                        if (journey.TryGetProperty("arrivalDateTime", out var arrDt))
                            arrTime = DateTime.Parse(arrDt.GetString()).ToString("HH:mm");
                        if (journey.TryGetProperty("duration", out var dur))
                            duration = dur.GetInt32();

                        JourneyResults.Add(new JourneyResult
                        {
                            DepartureTime = depTime,
                            ArrivalTime = arrTime,
                            Duration = duration,
                            Destination = destName
                        });

                        if (JourneyResults.Count >= 8) break;
                    }
                    if (JourneyResults.Count == 0)
                        errorMessage = "No scheduled trains found for this time.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
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
                    return station.commonName;
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
            public string icsCode { get; set; }
        }

        public class Arrival
        {
            public string destinationName { get; set; }
            public int timeToStation { get; set; }
            public string currentLocation { get; set; }
            public string towards { get; set; }
            public string vehicleId { get; set; }
        }

        public class JourneyResult
        {
            public string DepartureTime { get; set; }
            public string ArrivalTime { get; set; }
            public int Duration { get; set; }
            public string Destination { get; set; }
        }
    }
}