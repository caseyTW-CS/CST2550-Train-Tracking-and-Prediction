using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainApp.Models; 

namespace TrainApp.Pages
{
    public class AlertsModel : PageModel
    {
        private readonly AlertService _alertService;

        public List<AlertResult> ActiveAlerts { get; set; } = new();
        public bool HasActiveDelays { get; set; }
        public DateTime LastUpdated { get; set; }

        // 🔥 inject service
        public AlertsModel()
        {
            _alertService = new AlertService();
        }

        // page load
        public async Task OnGetAsync()
        {
            ActiveAlerts = await _alertService.GetAlertsAsync();
            HasActiveDelays = ActiveAlerts.Any(a => a.Severity != "Info");
            LastUpdated = DateTime.Now;
        }

        // 🔥 used by JS refresh
        public async Task<IActionResult> OnGetGetActiveAlerts()
        {
            var alerts = await _alertService.GetAlertsAsync();

            return new JsonResult(new
            {
                success = true,
                alerts = alerts,
                alertCount = alerts.Count,
                hasActiveDelays = alerts.Count > 0,
                lastUpdated = DateTime.Now.ToString("HH:mm:ss")
            });
        }
    }
}