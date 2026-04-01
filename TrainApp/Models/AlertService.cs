using System;
using System.Collections.Generic;

namespace TrainApp.Models
{
    public class AlertService
    {
        public List<string> activeAlerts { get; private set; }

        public AlertService()
        {
            activeAlerts = new List<string>();
        }

        public string trainApproachingAlert(TrainPosition trainPosition)
        {
            double minutesLeft = trainPosition.minutesUntilNextStation();

            if (minutesLeft <= 2)
            {
                string alert = $"[APPROACHING] Train approaching {trainPosition.nextStation.stationName} " +
                               $"- arriving in {minutesLeft:F0} minute(s)";
                activeAlerts.Add(alert);
                return alert;
            }
            return null;
        }

        public string delayAlert(Journey journey)
        {
            if (journey.currentDelayMinutes > 0)
            {
                string severity = Disruption.calculateSeverity(journey.currentDelayMinutes);
                string alert = $"[{severity} DELAY] {journey.journeyTrain.trainNumber} " +
                               $"from {journey.departureStation.stationName} to {journey.arrivalStation.stationName} " +
                               $"is delayed by {journey.currentDelayMinutes} minute(s). " +
                               $"New arrival time: {journey.getPredictedArrival():HH:mm}";
                activeAlerts.Add(alert);
                return alert;
            }
            return null;
        }

        public void clearAlerts()
        {
            activeAlerts.Clear();
        }

        public void displayAllAlerts()
        {
            if (activeAlerts.Count == 0)
            {
                return;
            }
            foreach (string alert in activeAlerts)
            {
                //Console.WriteLine(alert);
            }
        }
    }
}