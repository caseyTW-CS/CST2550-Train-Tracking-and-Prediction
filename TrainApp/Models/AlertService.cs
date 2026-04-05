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

        // get alerts from TfL and process them
        public async Task<List<AlertResult>> GetAlertsAsync()
        {
            var alerts = new List<AlertResult>();

            try
            {
                string url = "https://api.tfl.gov.uk/Line/elizabeth";
                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return alerts;
                }

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
                        // ?? use reason if available, otherwise fallback
                        string description = status.Reason;

                        if (string.IsNullOrWhiteSpace(description))
                            description = status.StatusSeverityDescription;

                        if (string.IsNullOrWhiteSpace(description))
                            continue;

                        // skip good service
                        if (description.Contains("Good Service", StringComparison.OrdinalIgnoreCase))
                            continue;

                        var alert = new AlertResult
                        {
                            Description = description,
                            Severity = MapSeverity(description),
                            ReportedAt = DateTime.Now
                        };

                        alerts.Add(alert);
                    }
                }

                // if no alerts found ? show good service
                if (!alerts.Any())
                {
                    alerts.Add(new AlertResult
                    {
                        Description = "All trains running normally on the Elizabeth Line",
                        Severity = "Info",
                        ReportedAt = DateTime.Now
                    });
                }
            }
            catch
            {
                alerts.Add(new AlertResult
                {
                    Description = "Unable to fetch live service data",
                    Severity = "Info",
                    ReportedAt = DateTime.Now
                });
            }

            return alerts;
        }

        // convert TfL text into severity
        private string MapSeverity(string text)
        {
            text = text.ToLower();

            if (text.Contains("good"))
                return "Info";

            if (text.Contains("minor"))
                return "Minor";

            if (text.Contains("severe") || text.Contains("suspended"))
                return "Major";

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

    // clean alert format for UI
    public class AlertResult
    {
        public string Description { get; set; } = "";
        public string Severity { get; set; } = "";
        public DateTime ReportedAt { get; set; }
    }
}