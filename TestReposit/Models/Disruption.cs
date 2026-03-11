using System;
using System.Collections.Generic;

namespace TestReposit
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
    }
}