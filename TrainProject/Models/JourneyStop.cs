using System;

namespace TrainProject
{
    public class JourneyStop
    {
        //PROPERTIES
        public Station stopStation { get; private set; }

        public DateTime scheduledArrival { get; private set; }

        public DateTime scheduledDeparture { get; private set; }

        public DateTime predictedArrival { get; private set; }

        public DateTime predictedDeparture { get; private set; }

        public bool hasPassed { get; private set; }

        //METHODS
        public JourneyStop(Station station, DateTime scheduledArr, DateTime scheduledDep)
        {
            stopStation = station;
            scheduledArrival = scheduledArr;
            scheduledDeparture = scheduledDep;

            predictedArrival = scheduledArr;
            predictedDeparture = scheduledDep;

            hasPassed = false;
        }

        public void updatePredictedArrival(int delayMinutes)
        {
            if (!hasPassed)
            {
                predictedArrival = scheduledArrival.AddMinutes(delayMinutes);
                predictedDeparture = scheduledDeparture.AddMinutes(delayMinutes);
            }
        }

        public void markAsPassed()
        {
            hasPassed = true;
        }
    }
}