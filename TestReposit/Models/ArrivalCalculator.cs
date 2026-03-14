using System;

// works out when a train will arrive at the next station
// covers "train travel time calculation" and "train arrival calculation" from the brief
public class ArrivalCalculator
{
    //PROPERTIES
    // the current position of the train
    public TrainPosition trainPosition { get; private set; }
    // used to look up the average speed for the train type
    public TrainSpeedManager speedManager { get; private set; }

    //METHODS
    public ArrivalCalculator(TrainPosition position, TrainSpeedManager speeds)
    {
        trainPosition = position;
        speedManager = speeds;
    }

    // works out how many minutes until the train arrives at the next station
    // uses the TrainPosition we already built to get this
    public double getMinutesUntilArrival()
    {
        return trainPosition.minutesUntilNextStation();
    }

    // works out the actual clock time the train will arrive
    // so instead of "2 minutes" it gives you "14:32" which is more useful
    public DateTime getArrivalTime()
    {
        double minutesLeft = getMinutesUntilArrival();
        // add the minutes left onto the current time to get the arrival time
        return DateTime.Now.AddMinutes(minutesLeft);
    }

    // returns a nice readable string for displaying to users
    // e.g. "Train arriving at Whitechapel at 14:32"
    public string getArrivalMessage()
    {
        DateTime arrivalTime = getArrivalTime();
        double minutesLeft = getMinutesUntilArrival();

        if (minutesLeft == 0)
            return $"Train has arrived at {trainPosition.nextStation.stationName}";
        else if (minutesLeft <= 1)
            return $"Train arriving at {trainPosition.nextStation.stationName} in less than a minute";
        else
            // HH:mm just formats the time as 14:32 instead of 2:32:00 PM
            return $"Train arriving at {trainPosition.nextStation.stationName} in {minutesLeft:F0} minutes ({arrivalTime.ToString("HH:mm")})";
    }

    // works out the travel time between any two stations given a distance and train type
    // useful for planning journeys on the elizabeth line
    public double calculateTravelTime(string trainType, double distanceMiles)
    {
        return speedManager.estimateJourneyTime(trainType, distanceMiles);
    }
}