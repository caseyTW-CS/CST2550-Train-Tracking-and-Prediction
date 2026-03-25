using System;
using System.Collections.Generic;

namespace TrainProject
{
    public class AlertService
    {
        //PROPERTIES
        // stores all active alerts
        public List<string> activeAlerts { get; private set; }

        //METHODS
        public AlertService()
        {
            activeAlerts = new List<string>();
        }

        // triggers an alert when a train is approaching a station
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

        // triggers an alert when a delay is detected on a journey
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

        // clears all active alerts
        public void clearAlerts()
        {
            activeAlerts.Clear();
        }

        // prints all active alerts
        public void displayAllAlerts()
        {
            if (activeAlerts.Count == 0)
            {
                Console.WriteLine("No active alerts.");
                return;
            }
            foreach (string alert in activeAlerts)
            {
                Console.WriteLine(alert);
            }
        }
    }
}