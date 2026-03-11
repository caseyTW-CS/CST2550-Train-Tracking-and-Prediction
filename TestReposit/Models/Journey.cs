using System;
using System.Collections.Generic;

namespace TestReposit
{
    // this is probably the most important class for the project
    // it links a train to its stops and keeps track of delays
    public class Journey
    {
        //PROPERTIES
        public string journeyId { get; private set; }

        public Train journeyTrain { get; private set; }

        public Station departureStation { get; private set; }

        public Station arrivalStation { get; private set; }

        // scheduled times - what the timetable says
        public DateTime scheduledDeparture { get; private set; }

        public DateTime scheduledArrival { get; private set; }

        // actual times - what actually happened or our prediction
        public DateTime? actualDeparture { get; private set; }

        public DateTime? actualArrival { get; private set; }

        // how many minutes delayed the train currently is
        public int currentDelayMinutes { get; private set; }

        // list of all stops with their own scheduled/actual times
        public List<JourneyStop> stops { get; private set; }

        //METHODS
        public Journey(string id, Train train, Station departure, Station arrival,
                       DateTime scheduledDep, DateTime scheduledArr)
        {
            journeyId = id;
            journeyTrain = train;
            departureStation = departure;
            arrivalStation = arrival;
            scheduledDeparture = scheduledDep;
            scheduledArrival = scheduledArr;
            currentDelayMinutes = 0;
            stops = new List<JourneyStop>();
        }

        public void updateDelay(int delayMinutes)
        {
            currentDelayMinutes = delayMinutes;

            foreach (var stop in stops)
            {
                stop.updatePredictedArrival(delayMinutes);
            }
        }

        public string getStatus()
        {
            if (currentDelayMinutes == 0)
                return "On Time";
            else if (currentDelayMinutes <= 5)
                return $"Slight Delay ({currentDelayMinutes} mins)";
            else
                return $"Delayed ({currentDelayMinutes} mins)";
        }

        public DateTime getPredictedArrival()
        {
            return scheduledArrival.AddMinutes(currentDelayMinutes);
        }
    }
}