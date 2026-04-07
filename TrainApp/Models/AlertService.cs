using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainApp.Models
{
    public class AlertService
    {
        private readonly HttpClient _http;

        public AlertService()
        {
            _http = new HttpClient();
        }

        public async Task<List<AlertResult>> GetAlertsAsync()
        {
            var alerts = new List<AlertResult>();

            try
            {
                // SAME endpoint as rest of our app
                string url = "https://api.tfl.gov.uk/Line/elizabeth/Status";
                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return alerts;

                var json = await response.Content.ReadAsStringAsync();

                var lines = JsonSerializer.Deserialize<List<TfLLine>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                var elizabeth = lines?.FirstOrDefault();

                if (elizabeth?.LineStatuses != null)
                {
                    foreach (var status in elizabeth.LineStatuses)
                    {
                        string description = status.Reason;

                        if (string.IsNullOrWhiteSpace(description))
                            description = status.StatusSeverityDescription;

                        if (string.IsNullOrWhiteSpace(description))
                            continue;

                        // ❌ Skip "Good Service"
                        if (description.Contains("Good Service", StringComparison.OrdinalIgnoreCase))
                            continue;

                        alerts.Add(new AlertResult
                        {
                            Description = description,
                            Severity = MapSeverity(description),
                            ReportedAt = DateTime.Now,
                            Stations = ExtractStations(description)
                        });
                    }
                }
            }
            catch
            {
                alerts.Add(new AlertResult
                {
                    Description = "Unable to fetch live service data",
                    Severity = "Info",
                    ReportedAt = DateTime.Now,
                    Stations = new List<string>()
                });
            }

            return alerts;
        }

        // 🔍 Extract stations from text
        private List<string> ExtractStations(string text)
        {
            var stations = new List<string>();

            if (string.IsNullOrWhiteSpace(text))
                return stations;

            text = text.Replace("\n", " ").Trim();

            // between X and Y
            if (text.Contains("between") && text.Contains(" and "))
            {
                try
                {
                    var part = text.Split("between", StringSplitOptions.RemoveEmptyEntries)[1];
                    var split = part.Split(" and ");

                    if (split.Length >= 2)
                    {
                        stations.Add(split[0].Trim());
                        stations.Add(split[1].Split('.')[0].Trim());
                    }
                }
                catch { }
            }

            // X → Y or X ↔ Y
            else if (text.Contains("→") || text.Contains("↔"))
            {
                try
                {
                    var arrow = text.Contains("→") ? "→" : "↔";
                    var split = text.Split(arrow);

                    if (split.Length >= 2)
                    {
                        stations.Add(split[0].Trim());
                        stations.Add(split[1].Trim());
                    }
                }
                catch { }
            }

            return stations.Distinct().ToList();
        }

        private string MapSeverity(string text)
        {
            text = text.ToLower();

            if (text.Contains("minor"))
                return "Minor";

            if (text.Contains("severe") || text.Contains("suspended"))
                return "Major";

            if (text.Contains("good"))
                return "Info";

            return "Moderate";
        }
    }

    // TfL models
    public class TfLLine
    {
        public List<LineStatus>? LineStatuses { get; set; }
    }

    public class LineStatus
    {
        public string? StatusSeverityDescription { get; set; }
        public string? Reason { get; set; }
    }

    // UI model
    public class AlertResult
    {
        public string Description { get; set; } = "";
        public string Severity { get; set; } = "";
        public DateTime ReportedAt { get; set; }
        public List<string> Stations { get; set; } = new();
    }
}