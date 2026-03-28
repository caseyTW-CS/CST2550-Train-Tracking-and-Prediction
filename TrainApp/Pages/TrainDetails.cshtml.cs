using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace TrainApp.Pages
{
    public class TrainDetailsModel : PageModel
    {
        public string Start { get; set; } = "";
        public string Destination { get; set; } = "";

        public int TimeToStation { get; set; }

        public string ArrivalTime { get; set; } = "";
        public string DepartureTime { get; set; } = "";

        public int DurationMinutes { get; set; }

        public List<string> Route = new List<string>();

        private List<string> stationOrder = new List<string>
        {
            "Reading","Twyford","Maidenhead","Slough","Burnham","Taplow","Iver","Langley",
            "West Drayton","Hayes & Harlington","Southall","Hanwell","West Ealing",
            "Ealing Broadway","Acton Main Line","Paddington","Bond Street",
            "Tottenham Court Road","Farringdon","Liverpool Street","Whitechapel",
            "Stratford","Maryland","Forest Gate","Manor Park","Ilford","Seven Kings",
            "Goodmayes","Chadwell Heath","Romford","Gidea Park","Harold Wood",
            "Brentwood","Shenfield","Canary Wharf","Custom House","Woolwich","Abbey Wood"
        };

        public void OnGet(string start, string dest, int time)
        {
            // ✅ SAFETY CHECK (prevents crash)
            if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(dest))
            {
                Start = "Unknown";
                Destination = "Unknown";
                return;
            }

            Start = start;
            Destination = dest;
            TimeToStation = time;

            DepartureTime = DateTime.Now.ToString("HH:mm");

            if (time <= 0)
            {
                ArrivalTime = "Now";
                DurationMinutes = 0;
            }
            else
            {
                DateTime arrival = DateTime.Now.AddSeconds(time);
                ArrivalTime = arrival.ToString("HH:mm");
                DurationMinutes = time / 60;
            }

            BuildRoute();
        }

        private void BuildRoute()
        {
            int startIndex = FindStationIndex(Start);
            int endIndex = FindStationIndex(Destination);

            if (startIndex == -1 || endIndex == -1)
                return;

            if (startIndex <= endIndex)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Route.Add(stationOrder[i]);
                }
            }
            else
            {
                for (int i = startIndex; i >= endIndex; i--)
                {
                    Route.Add(stationOrder[i]);
                }
            }
        }

        private int FindStationIndex(string name)
        {
            for (int i = 0; i < stationOrder.Count; i++)
            {
                if (name.ToLower().Contains(stationOrder[i].ToLower()))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}