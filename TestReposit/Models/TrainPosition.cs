using System;

namespace TestReposit
{
    // tracks where a train actually is right now between two stations
    // this is the core of the position prediction part of the brief
    public class TrainPosition
    {
        //Properties
        //which station the train has left
        public Station lastStation { get; private set; }
        // which station the train is heading to next
        public Station nextStation { get; private set; }
        // how many minutes ago the train left the last station
        public double minutesSinceLastStation { get; private set; }
        // how fast its going right now in mph
        public double currentSpeed { get; private set; }
        // total journey time in minutes between the two stations
        public double totalJourneyMinutes { get; private set; }
        // when we last updated the position
        public DateTime lastUpdated { get; private set; }

        //METHODS
        public TrainPosition(Station last, Station next, double speed, double totalMinutes)
        {
            lastStation = last;
            nextStation = next;
            currentSpeed = speed;
            totalJourneyMinutes = totalMinutes;
            minutesSinceLastStation = 0; // starts at 0 when it leaves the last station
            lastUpdated = DateTime.Now;
        }

        // call this to update how long the train has been travelling since the last station
        public void updatePosition(double minutesTravelled)
        {
            minutesSinceLastStation = minutesTravelled;
            lastUpdated = DateTime.Now;
        }

        // updates the current speed of the train
        // useful when the train slows down approaching a station
        public void updateSpeed(double newSpeed)
        {
            currentSpeed = newSpeed;
            lastUpdated = DateTime.Now;
        }

        // works out how many minutes until the train reaches the next station
        public double minutesUntilNextStation()
        {
            double remaining = totalJourneyMinutes - minutesSinceLastStation;
            if (remaining < 0)
                return 0; // already there
            return remaining;
        }

        // returns a simple readable status of where the train is
        public string getPositionStatus()
        {
            double remaining = minutesUntilNextStation();
            if (minutesSinceLastStation == 0)
                return $"Departed {lastStation.stationName}";
            else if (remaining == 0)
                return $"Arrived at {nextStation.stationName}";
            else
                return $"Between {lastStation.stationName} and {nextStation.stationName} - {remaining:F0} minutes until {nextStation.stationName}";
            // F0 just rounds to a whole number so it shows 3 mins not 3.487 mins
        }
    }
}