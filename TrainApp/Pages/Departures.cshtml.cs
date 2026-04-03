using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace TrainApp.Pages
{
    public class DeparturesModel : PageModel
    {
        private readonly HttpClient _http = new();

        public class Train
        {
            public string? destination { get; set; }
            public string? departureTime { get; set; }
            public string? platform { get; set; }
        }

        public List<Train> results { get; set; } = new();
        public List<Train> arrivals { get; set; } = new();

        public string? searchedStation { get; set; }

        [BindProperty]
        public string? stationName { get; set; }

        public void OnGet()
        {
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
            }
            catch
            {
                // keep page alive if TfL is unavailable
            }
        }

        // 🔥 ADD THIS METHOD (this is what fixes everything)
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

                if (id.StartsWith("910G", StringComparison.OrdinalIgnoreCase))
                    return id;

                if (!string.IsNullOrWhiteSpace(name) &&
                    name.Contains(station, StringComparison.OrdinalIgnoreCase))
                    return id;
            }

            return null;
        }

        private string FormatTime(DateTime expectedArrival)
        {
            var diff = expectedArrival - DateTime.UtcNow;

            if (diff.TotalMinutes <= 1)
                return "Due";

            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} min";

            return expectedArrival.ToLocalTime().ToString("HH:mm");
        }

        private string CleanPlatform(string? platformName)
        {
            if (string.IsNullOrWhiteSpace(platformName))
                return "-";

            return platformName.Replace("Platform ", "", StringComparison.OrdinalIgnoreCase).Trim();
        }

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