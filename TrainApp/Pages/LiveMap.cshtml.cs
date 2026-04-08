using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainApp.Pages
{
    public class LiveMapModel : PageModel
    {
        public List<LiveTrain> trains { get; set; } = new List<LiveTrain>();

        public async Task OnGetAsync()
        {
            using (var client = new HttpClient())
            {
                var url = "https://api.tfl.gov.uk/Line/elizabeth/Arrivals?app_key=bd620c86b82b487bb8d7b7f41c54d10b";
                var response = await client.GetStringAsync(url);
                var data = JsonSerializer.Deserialize<List<LiveTrain>>(response);

                if (data != null)
                {
                    var seen = new HashSet<string>();
                    foreach (var train in data)
                    {
                        if (!string.IsNullOrEmpty(train.vehicleId) && seen.Add(train.vehicleId))
                        {
                            trains.Add(train);
                        }
                    }
                }
            }
        }

        public async Task<JsonResult> OnGetTrainsAsync()
        {
            var result = new List<LiveTrain>();
            using (var client = new HttpClient())
            {
                var url = "https://api.tfl.gov.uk/Line/elizabeth/Arrivals?app_key=bd620c86b82b487bb8d7b7f41c54d10b";
                var response = await client.GetStringAsync(url);
                var data = JsonSerializer.Deserialize<List<LiveTrain>>(response);

                if (data != null)
                {
                    var seen = new HashSet<string>();
                    foreach (var train in data)
                    {
                        if (!string.IsNullOrEmpty(train.vehicleId) && seen.Add(train.vehicleId))
                        {
                            result.Add(train);
                        }
                    }
                }
            }
            return new JsonResult(result);
        }

        public class LiveTrain
        {
            public string vehicleId { get; set; }
            public string stationName { get; set; }
            public string destinationName { get; set; }
            public int timeToStation { get; set; }
            public string currentLocation { get; set; }
            public string platformName { get; set; }
        }
    }
}