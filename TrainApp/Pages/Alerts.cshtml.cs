using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
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
        private readonly HttpClient _httpClient;

        public List<AlertDisplay> ActiveAlerts { get; set; } = new();
        public bool HasActiveDelays { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public string ApiStatus { get; set; } = "";

        public AlertsModel()
        {
            _httpClient = new HttpClient();
        }

        public async Task OnGetAsync()
        {
            await LoadTfLAlerts();
            LoadEmailPref();
        }

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
                var lines = JsonSerializer.Deserialize<List<TfLLine>>(json);

                ActiveAlerts.Clear();

                var elizabeth = lines?.FirstOrDefault();

                if (elizabeth?.LineStatuses != null)
                {
                    foreach (var status in elizabeth.LineStatuses)
                    {
                        if (!string.IsNullOrEmpty(status.Reason))
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

                    HasActiveDelays = ActiveAlerts.Any();
                    ApiStatus = HasActiveDelays
                        ? $"Live disruptions: {ActiveAlerts.Count}"
                        : "Good Service";
                }
            }
            catch
            {
                ApiStatus = "Error fetching live data";
            }

            LastUpdated = DateTime.Now;
        }

        private void ApplySeverity(AlertDisplay alert)
        {
            var text = alert.Description.ToLower();

            if (text.Contains("suspended") || text.Contains("closed"))
            {
                alert.Severity = "Major";
                alert.Icon = "fas fa-exclamation-triangle";
                alert.SeverityClass = "severity-major";
                alert.SeverityBadgeClass = "bg-danger";
            }
            else if (text.Contains("delay"))
            {
                alert.Severity = "Moderate";
                alert.Icon = "fas fa-exclamation-circle";
                alert.SeverityClass = "severity-moderate";
                alert.SeverityBadgeClass = "bg-warning text-dark";
            }
            else
            {
                alert.Severity = "Info";
                alert.Icon = "fas fa-info-circle";
                alert.SeverityClass = "severity-info";
                alert.SeverityBadgeClass = "bg-secondary";
            }

            alert.Type = alert.Severity == "Info" ? "Info" : "Delay";
        }

        private List<string> ExtractStations(string text)
        {
            var stations = new List<string>();

            string[] knownStations = {
                "Paddington", "Bond Street", "Tottenham Court Road", "Farringdon",
                "Liverpool Street", "Whitechapel", "Stratford", "Canary Wharf",
                "Woolwich", "Abbey Wood", "Reading", "Heathrow", "Shenfield"
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

        public async Task<IActionResult> OnPostRefreshAlerts()
        {
            await LoadTfLAlerts();
            return RedirectToPage();
        }

        public IActionResult OnPostClearAlerts()
        {
            ActiveAlerts.Clear();
            return RedirectToPage();
        }

        public IActionResult OnPostToggleEmail([FromBody] EmailToggleRequest request)
        {
            HttpContext.Session.SetString("EmailNotifications", request.Enabled.ToString());
            return new JsonResult(new { success = true });
        }

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

        private void LoadEmailPref()
        {
            var pref = HttpContext.Session.GetString("EmailNotifications");
            EmailNotificationsEnabled = pref == "True";
        }
    }

    // ✅ REAL TfL API model
    public class TfLLine
    {
        public List<LineStatus>? LineStatuses { get; set; }
    }

    public class LineStatus
    {
        public string? StatusSeverityDescription { get; set; }
        public string? Reason { get; set; }
    }

    public class AlertDisplay
    {
        public string Id { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";
        public string Severity { get; set; } = "";
        public string Icon { get; set; } = "";
        public string SeverityClass { get; set; } = "";
        public string SeverityBadgeClass { get; set; } = "";
        public DateTime ReportedAt { get; set; }
        public List<string> AffectedStations { get; set; } = new();

        public string TimeFormatted => ReportedAt.ToString("HH:mm");
        public string DateFormatted => ReportedAt.ToString("dd MMM");
    }

    public class EmailToggleRequest
    {
        public bool Enabled { get; set; }
    }
}