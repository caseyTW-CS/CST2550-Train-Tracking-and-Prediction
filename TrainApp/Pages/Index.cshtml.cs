using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace TrainApp.Pages
{
    public class IndexModel : PageModel
    {
        public string ServiceStatus { get; set; } = "Loading...";

        public async Task OnGetAsync()
        {
            try
            {
                using var http = new HttpClient();

                var json = await http.GetStringAsync("https://api.tfl.gov.uk/Line/elizabeth");

                var data = JsonSerializer.Deserialize<List<TflLine>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                // safely extract status
                ServiceStatus = data?
                    .FirstOrDefault()?
                    .lineStatuses?
                    .FirstOrDefault()?
                    .statusSeverityDescription
                    ?? "Good Service";
            }
            catch
            {
                // fallback if API fails
                ServiceStatus = "Unavailable";
            }
        }

        // 🔹 models for TfL response
        public class TflLine
        {
            public List<LineStatus>? lineStatuses { get; set; }
        }

        public class LineStatus
        {
            public string? statusSeverityDescription { get; set; }
        }
    }
}
