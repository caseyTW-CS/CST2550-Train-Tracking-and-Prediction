using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainApp.Pages
{
    public class AlertsModel : PageModel
    {
        // used to call TfL API
        private readonly HttpClient _httpClient;

        public List<AlertDisplay> ActiveAlerts { get; set; } = new();
        public bool HasActiveDelays { get; set; }
        public DateTime LastUpdated { get; set; }
        public string ApiStatus { get; set; } = "";

        public AlertsModel()
        {
            _httpClient = new HttpClient();
        }

        // runs when page loads
        public async Task OnGetAsync()
        {
            await LoadTfLAlerts();
        }

        // get live alerts from TfL
        private async Task LoadTfLAlerts()
        {
            try
            {
                string url = "https://api.tfl.gov.uk/Line/elizabeth";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    ApiStatus = "TfL API unavailable";
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();

                var lines = JsonSerializer.Deserialize<List<TfLLine>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                ActiveAlerts.Clear();

                var elizabeth = lines?.FirstOrDefault();

                if (elizabeth?.LineStatuses != null)
                {
                    foreach (var status in elizabeth.LineStatuses)
                    {
                        // only add if there's actually a reason (delay info)
                        if (!string.IsNullOrWhiteSpace(status.Reason))
                        {
                            var alert = new AlertDisplay
                            {
                                Id = Guid.NewGuid().ToString(),
                                Description = status.Reason,
                                ReportedAt = DateTime.Now,
                                AffectedStations = ExtractStations(status.Reason)
                            };

                            ApplySeverity(alert);
                            ActiveAlerts.Add(alert);
                        }
                    }
                }

                HasActiveDelays = ActiveAlerts.Any();
                ApiStatus = HasActiveDelays
                    ? $"Live disruptions: {ActiveAlerts.Count}"
                    : "Good Service";
            }
            catch
            {
                ApiStatus = "Error fetching live data";
            }

            LastUpdated = DateTime.Now;
        }

        // decide how serious the alert is
        private void ApplySeverity(AlertDisplay alert)
        {
            var text = (alert.Description ?? "").ToLower();

            if (text.Contains("suspended") || text.Contains("closed"))
            {
                alert.Severity = "Major";
            }
            else if (text.Contains("delay"))
            {
                alert.Severity = "Moderate";
            }
            else
            {
                alert.Severity = "Info";
            }
        }

        // try to find station names from text
        private List<string> ExtractStations(string text)
        {
            var stations = new List<string>();

            string[] knownStations =
            {
                "Paddington", "Bond Street", "Farringdon",
                "Liverpool Street", "Stratford", "Canary Wharf",
                "Woolwich", "Abbey Wood", "Reading", "Heathrow"
            };

            foreach (var station in knownStations)
            {
                if (text.Contains(station, StringComparison.OrdinalIgnoreCase))
                {
                    stations.Add(station);
                }
            }

            return stations;
        }

        // used by JS to refresh alerts
        public async Task<IActionResult> OnGetGetActiveAlerts()
        {
            await LoadTfLAlerts();

            return new JsonResult(new
            {
                success = true,
                alerts = ActiveAlerts,
                hasActiveDelays = HasActiveDelays,
                alertCount = ActiveAlerts.Count,
                lastUpdated = LastUpdated.ToString("HH:mm:ss")
            });
        }
    }

    // TfL response model
    public class TfLLine
    {
        public List<LineStatus>? LineStatuses { get; set; }
    }

    public class LineStatus
    {
        public string? StatusSeverityDescription { get; set; }
        public string? Reason { get; set; }
    }

    // what we show on UI
    public class AlertDisplay
    {
        public string Id { get; set; } = "";
        public string Description { get; set; } = "";
        public string Severity { get; set; } = "";
        public DateTime ReportedAt { get; set; }
        public List<string> AffectedStations { get; set; } = new();

        public string TimeFormatted => ReportedAt.ToString("HH:mm");
    }
}