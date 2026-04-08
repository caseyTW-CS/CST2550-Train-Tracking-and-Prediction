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
                string url = "https://api.tfl.gov.uk/Line/elizabeth/Status";
                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return alerts;

                var json = await response.Content.ReadAsStringAsync();

                var lines = JsonSerializer.Deserialize<List<TfLLine>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var elizabeth = lines?.FirstOrDefault();

                if (elizabeth?.LineStatuses != null)
                {
                    foreach (var status in elizabeth.LineStatuses)
                    {
                        // 🔥 PRIORITY: Use Reason FIRST if exists
                        string description = !string.IsNullOrWhiteSpace(status.Reason)
                            ? status.Reason
                            : status.StatusSeverityDescription ?? "";

                        // ❌ Ignore empty
                        if (string.IsNullOrWhiteSpace(description))
                            continue;

                        // ❌ Only skip TRUE clean "Good Service"
                        if (status.StatusSeverityDescription != null &&
                            status.StatusSeverityDescription.Equals("Good Service", StringComparison.OrdinalIgnoreCase) &&
                            string.IsNullOrWhiteSpace(status.Reason))
                        {
                            continue;
                        }

                        alerts.Add(new AlertResult
                        {
                            Description = description.Trim(),
                            Severity = MapSeverity(status.StatusSeverityDescription, description),
                            ReportedAt = DateTime.Now,
                            Stations = ExtractStations(description)
                        });
                    }
                }

                // ✅ fallback if nothing found
                if (!alerts.Any())
                {
                    alerts.Add(new AlertResult
                    {
                        Description = "All trains running normally on the Elizabeth Line",
                        Severity = "Info",
                        ReportedAt = DateTime.Now,
                        Stations = new List<string>()
                    });
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

        // 🔍 Improved station extractor
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

            // arrows
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

        private string MapSeverity(string severityText, string description)
        {
            var text = (severityText + " " + description).ToLower();

            if (text.Contains("minor"))
                return "Minor";

            if (text.Contains("severe") || text.Contains("suspended") || text.Contains("closed"))
                return "Major";

            if (text.Contains("good"))
                return "Info";

            return "Moderate";
        }
    }

    public class TfLLine
    {
        public List<LineStatus>? LineStatuses { get; set; }
    }

    public class LineStatus
    {
        public string? StatusSeverityDescription { get; set; }
        public string? Reason { get; set; }
    }

    public class AlertResult
    {
        public string Description { get; set; } = "";
        public string Severity { get; set; } = "";
        public DateTime ReportedAt { get; set; }
        public List<string> Stations { get; set; } = new();
    }
}