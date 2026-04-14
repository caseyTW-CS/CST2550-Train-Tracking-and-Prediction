using System;
using System.Collections.Generic;

namespace TrainApp.Models
{
    // Represents a disruption affecting one or more journeys
    // Groups delayed journeys together and provides a summary
    public class Disruption
    {
        // unique ID for the disruption
        public string disruptionId { get; private set; }

        // reason for the disruption
        public string reason { get; private set; }

        // estimated delay duration in minutes
        public int estimatedDurationMinutes { get; private set; }

        // when the disruption was reported
        public DateTime reportedAt { get; private set; }

        // list of affected journeys
        public List<Journey> affectedJourneys { get; private set; }

        // severity level (Minor / Moderate / Severe)
        public string severity { get; private set; }

        // constructor
        public Disruption(string id, string reason, int duration, string severity)
        {
            disruptionId = id;
            this.reason = reason;
            estimatedDurationMinutes = duration;
            reportedAt = DateTime.Now;
            this.severity = severity;
            affectedJourneys = new List<Journey>();
        }

        // adds a journey to the disruption
        public void addAffectedJourney(Journey journey)
        {
            affectedJourneys.Add(journey);
        }

        // checks if a journey is delayed and includes it if needed
        public void detectAndFlagDelay(Journey journey)
        {
            if (journey.currentDelayMinutes > 0)
            {
                if (!affectedJourneys.Contains(journey))
                    affectedJourneys.Add(journey);
            }
        }

        // determines severity based on delay time
        public static string calculateSeverity(int delayMinutes)
        {
            if (delayMinutes <= 5)
                return "Minor";
            else if (delayMinutes <= 20)
                return "Moderate";
            else
                return "Severe";
        }

        // scans a list of journeys and flags delayed ones
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

        // returns a readable message for display
        public string getAlertMessage()
        {
            return $"[{severity}] Disruption: {reason} - Estimated delay: {estimatedDurationMinutes} mins. " +
                   $"{affectedJourneys.Count} journey(s) affected. Reported at {reportedAt:HH:mm}";
        }
    }
}