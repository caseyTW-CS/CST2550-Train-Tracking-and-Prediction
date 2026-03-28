using System;
using System.Collections.Generic;

namespace TrainApp.Models
{
    public class Disruption
    {
        //PROPERTIES
        public string disruptionId { get; private set; }

        public string reason { get; private set; }

        public int estimatedDurationMinutes { get; private set; }

        public DateTime reportedAt { get; private set; }

        public List<Journey> affectedJourneys { get; private set; }

        public string severity { get; private set; }

        //METHODS
        public Disruption(string id, string reason, int duration, string severity)
        {
            disruptionId = id;
            this.reason = reason;
            estimatedDurationMinutes = duration;
            reportedAt = DateTime.Now;
            this.severity = severity;
            affectedJourneys = new List<Journey>();
        }

        public void addAffectedJourney(Journey journey)
        {
            affectedJourneys.Add(journey);
        }

        // checks if a journey is delayed and adds it to affected journeys automatically
        public void detectAndFlagDelay(Journey journey)
        {
            if (journey.currentDelayMinutes > 0)
            {
                if (!affectedJourneys.Contains(journey))
                    affectedJourneys.Add(journey);
            }
        }

        // automatically works out severity based on delay minutes
        public static string calculateSeverity(int delayMinutes)
        {
            if (delayMinutes <= 5)
                return "Minor";
            else if (delayMinutes <= 20)
                return "Moderate";
            else
                return "Severe";
        }

        // checks a list of journeys and flags any that are delayed
        public void scanJourneysForDelays(List<Journey> journeys)
        {
            foreach (Journey journey in journeys)
            {
                if (journey.currentDelayMinutes > 0)
                {
                    journey.updateDelay(journey.currentDelayMinutes);
                    detectAndFlagDelay(journey);
                }
            }
        }

        // returns a readable alert message for this disruption
        public string getAlertMessage()
        {
            return $"[{severity}] Disruption: {reason} - Estimated delay: {estimatedDurationMinutes} mins. " +
                   $"{affectedJourneys.Count} journey(s) affected. Reported at {reportedAt:HH:mm}";
        }
    }
}