using System;
using System.Collections.Generic;

namespace TrainProject
{
    public class TrainSpeedManager
    {
        public List<TrainSpeed> trainSpeeds { get; private set; }

        public TrainSpeedManager()
        {
            trainSpeeds = new List<TrainSpeed>()
            {
                new TrainSpeed("Commuter", 50, 100),
                new TrainSpeed("Express", 90, 125),
                new TrainSpeed("Freight", 40, 75),
                new TrainSpeed("High Speed", 150, 200),
                new TrainSpeed("Regional", 60, 90)
            };
        }

        public TrainSpeed getSpeedForType(string trainType)
        {
            foreach (var speed in trainSpeeds)
            {
                if (speed.trainType == trainType)
                    return speed;
            }

            return null;
        }

        public double estimateJourneyTime(string trainType, double distanceMiles)
        {
            var speed = getSpeedForType(trainType);

            if (speed == null)
                return -1;

            return speed.estimateJourneyTime(distanceMiles);
        }

        public string calculateTimeBetweenStations(Station stationA, Station stationB, string trainType)
        {
            double distanceMiles =
                stationA.stationLocation.distanceTo(stationB.stationLocation);

            TrainSpeed speed = getSpeedForType(trainType);

            if (speed == null)
                return "Train type not found";

            double totalMinutes = speed.estimateJourneyTime(distanceMiles);

            int hours = (int)totalMinutes / 60;
            int minutes = (int)totalMinutes % 60;

            if (hours > 0)
                return $"{hours}h {minutes}m (approx {Math.Round(distanceMiles, 1)} miles)";
            else
                return $"{minutes}m (approx {Math.Round(distanceMiles, 1)} miles)";
        }
    }
}