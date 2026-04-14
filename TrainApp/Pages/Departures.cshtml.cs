using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TrainApp.Models;

namespace TrainApp.Pages
{
    public class DeparturesModel : PageModel
    {
        private readonly HttpClient _http = new();

        // Simple model used to display train data on the page
        public class Train
        {
            public string? destination { get; set; }
            public string? departureTime { get; set; }
            public string? platform { get; set; }
        }

        // Lists used to store trains for display
        public List<Train> results { get; set; } = new();
        public List<Train> arrivals { get; set; } = new();

        // Stores any disruptions detected from delays
        public List<Disruption> Disruptions { get; set; } = new();

        // Dictionary allows quick lookup of disruptions by ID
        public Dictionary<string, Disruption> DisruptionMap { get; set; } = new();

        public string? searchedStation { get; set; }

        [BindProperty]
        public string? stationName { get; set; }

        public async Task OnGet(string? stationName)
        {
            // If the user comes from favourites, automatically run a search
            if (!string.IsNullOrWhiteSpace(stationName))
            {
                this.stationName = stationName;
                await OnPostAsync();
            }
        }

        public async Task OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(stationName))
                return;

            searchedStation = stationName.Trim();

            try
            {
                var stopId = await FindElizabethStopIdAsync(searchedStation);

                if (string.IsNullOrWhiteSpace(stopId))
                    return;

                var arrivalsUrl = $"https://api.tfl.gov.uk/StopPoint/{stopId}/Arrivals";
                var json = await _http.GetStringAsync(arrivalsUrl);

                var data = JsonSerializer.Deserialize<List<TflArrival>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (data == null)
                    return;

                var trains = data
                    .Where(x => x.lineId == "elizabeth")
                    .Where(x => !string.IsNullOrWhiteSpace(x.destinationName))
                    .OrderBy(x => x.expectedArrival)
                    .Take(8)
                    .ToList();

                // Convert API data into something the UI can use
                foreach (var t in trains)
                {
                    var timeText = FormatTime(t.expectedArrival);

                    results.Add(new Train
                    {
                        destination = t.destinationName,
                        departureTime = timeText,
                        platform = CleanPlatform(t.platformName)
                    });

                    arrivals.Add(new Train
                    {
                        destination = t.destinationName,
                        departureTime = timeText,
                        platform = CleanPlatform(t.platformName)
                    });
                }

                // Create journey objects so we can apply disruption logic
                var journeys = new List<Journey>();

                foreach (var t in trains)
                {
                    int delay = t.expectedArrival > DateTime.UtcNow
                        ? (int)(t.expectedArrival - DateTime.UtcNow).TotalMinutes
                        : 0;

                    // Minimal objects just to connect into the backend logic
                    var trainObj = new TrainApp.Models.Train(
                    "Elizabeth Line",
                    "TfL",
                    1000,
                    90
                    );

                    var departureStationObj = new Station(
                        searchedStation,
                        "Elizabeth Line",
                        5,
                        new GeoCoords(0, 0)
                    );

                    var arrivalStationObj = new Station(
                        t.destinationName ?? "Unknown",
                        "Elizabeth Line",
                        5,
                        new GeoCoords(0, 0)
                    );

                    var journey = new Journey(
                        Guid.NewGuid().ToString(),
                        trainObj,
                        departureStationObj,
                        arrivalStationObj,
                        DateTime.Now,
                        t.expectedArrival
                    );

                    journey.updateDelay(delay);

                    journeys.Add(journey);
                }

                // If any journeys are delayed, create a disruption
                if (journeys.Any(j => j.currentDelayMinutes > 0))
                {
                    int maxDelay = journeys.Max(j => j.currentDelayMinutes);

                    var disruption = new Disruption(
                        Guid.NewGuid().ToString(),
                        $"Delays at {searchedStation}",
                        maxDelay,
                        Disruption.calculateSeverity(maxDelay)
                    );

                    // Apply logic from the Disruption class
                    disruption.scanJourneysForDelays(journeys);

                    // Store results in both list and dictionary
                    Disruptions.Add(disruption);
                    DisruptionMap[disruption.disruptionId] = disruption;
                }
            }
            catch
            {
                // If the API fails, just keep the page running
            }
        }

        // Used by JavaScript to refresh live data
        public async Task<IActionResult> OnGetDataAsync(string station)
        {
            if (string.IsNullOrWhiteSpace(station))
                return new JsonResult(new List<Train>());

            try
            {
                var stopId = await FindElizabethStopIdAsync(station);

                if (string.IsNullOrWhiteSpace(stopId))
                    return new JsonResult(new List<Train>());

                var url = $"https://api.tfl.gov.uk/StopPoint/{stopId}/Arrivals";
                var json = await _http.GetStringAsync(url);

                var data = JsonSerializer.Deserialize<List<TflArrival>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var trains = data?
                    .Where(x => x.lineId == "elizabeth")
                    .Where(x => !string.IsNullOrWhiteSpace(x.destinationName))
                    .OrderBy(x => x.expectedArrival)
                    .Take(8)
                    .Select(x => new Train
                    {
                        destination = x.destinationName,
                        departureTime = FormatTime(x.expectedArrival),
                        platform = CleanPlatform(x.platformName)
                    })
                    .ToList() ?? new List<Train>();

                return new JsonResult(trains);
            }
            catch
            {
                return new JsonResult(new List<Train>());
            }
        }

        // Finds the correct stop ID from TfL using the station name
        private async Task<string?> FindElizabethStopIdAsync(string station)
        {
            var searchUrl =
                $"https://api.tfl.gov.uk/StopPoint/Search?query={Uri.EscapeDataString(station)}";

            var json = await _http.GetStringAsync(searchUrl);

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("matches", out var matches))
                return null;

            foreach (var match in matches.EnumerateArray())
            {
                var id = match.TryGetProperty("id", out var idProp) ? idProp.GetString() : null;
                var name = match.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;

                if (string.IsNullOrWhiteSpace(id))
                    continue;

                // Elizabeth line stations usually start with this prefix
                if (id.StartsWith("910G", StringComparison.OrdinalIgnoreCase))
                    return id;

                if (!string.IsNullOrWhiteSpace(name) &&
                    name.Contains(station, StringComparison.OrdinalIgnoreCase))
                    return id;
            }

            return null;
        }

        // Converts API time into something readable
        private string FormatTime(DateTime expectedArrival)
        {
            var diff = expectedArrival - DateTime.UtcNow;

            if (diff.TotalMinutes <= 1)
                return "Due";

            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} min";

            return expectedArrival.ToLocalTime().ToString("HH:mm");
        }

        // Cleans platform text (e.g. removes "Platform ")
        private string CleanPlatform(string? platformName)
        {
            if (string.IsNullOrWhiteSpace(platformName))
                return "-";

            return platformName.Replace("Platform ", "", StringComparison.OrdinalIgnoreCase).Trim();
        }

        // Model for TfL API response
        public class TflArrival
        {
            public string? destinationName { get; set; }
            public string? platformName { get; set; }
            public DateTime expectedArrival { get; set; }
            public string? lineId { get; set; }
            public string? lineName { get; set; }
        }
    }
}