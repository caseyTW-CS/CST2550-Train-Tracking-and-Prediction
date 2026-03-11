using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TestReposit
{

    public class User
    {
        //PROPERTIES
        public int userID { get; set; }
        public string userName { get; private set; }
        public string userPass { get; private set; }

        //can be used for recommending tickets types/railcards:
        public int userAge { get; private set; }

        public string userEmail { get; private set; }

        //user phone number:
        public string userPhone { get; private set; }

        //for storing 
        public List<string> userTickets { get; private set; }

        public string userRailcard { get; private set; }

        //METHODS
        public User(string name, string pass, int age, string email, string phone, string railcard)
        {
            userName = name;
            userPass = pass;
            userAge = age;
            userEmail = email;
            userPhone = phone;
            userRailcard = railcard;
            userTickets = new List<string>();
        }

        // checks if the password the user typed matches their account
        public bool validateLogin(string enteredPass)
        {
            return userPass == enteredPass;
        }
    }

    public struct GeoCoords
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoCoords(double newLatitude, double newLongitude)
        {
            Latitude = newLatitude;
            Longitude = newLongitude;
        }

        // Haversine formula - calculates straight-line distance between
        // two sets of coordinates on the Earth's surface, returns miles
        public double distanceTo(GeoCoords other)
        {
            const double EarthRadiusMiles = 3958.8;

            double lat1 = toRadians(this.Latitude);
            double lat2 = toRadians(other.Latitude);
            double deltaLat = toRadians(other.Latitude - this.Latitude);
            double deltaLon = toRadians(other.Longitude - this.Longitude);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusMiles * c;
        }

        // helper to convert degrees to radians
        private double toRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }

    public class Station
    {
        //PROPERTIES
        public string stationName { get; private set; }

        //for storing the size of the platforms:
        public string stationSize { get; private set; }

        //for the platform the train will be stopping on:
        public int stationPlatform { get; private set; }

        public GeoCoords stationLocation { get; private set; }

        //METHODS
        public Station(string name, string size, int platform, GeoCoords location)
        {
            stationName = name;
            stationSize = size;
            stationPlatform = platform;
            stationLocation = location;
        }
    }

    public class Train
    {
        //PROPERTIES
        public string trainNumber { get; private set; }

        //e.g. thameslink:
        public string trainCompany { get; private set; }

        public int trainCarriages { get; private set; }

        //will be used to store the names of each stop on the journey in a list:
        public List<Station> trainStops { get; private set; }

        //METHODS
        public Train(string number, string company, int carriages)
        {
            trainNumber = number;
            trainCompany = company;
            trainCarriages = carriages;
            trainStops = new List<Station>();
        }
    }


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

        // this is the main prediction method - takes the current delay and works out
        // when the train will arrive at each remaining stop
        public void updateDelay(int delayMinutes)
        {
            currentDelayMinutes = delayMinutes;

            foreach (var stop in stops)
            {
                stop.updatePredictedArrival(delayMinutes);
            }
        }

        // works out if the train is on time, delayed etc
        public string getStatus()
        {
            if (currentDelayMinutes == 0)
                return "On Time";
            else if (currentDelayMinutes <= 5)
                return $"Slight Delay ({currentDelayMinutes} mins)";
            else
                return $"Delayed ({currentDelayMinutes} mins)";
        }

        // returns predicted arrival at the final destination
        public DateTime getPredictedArrival()
        {
            return scheduledArrival.AddMinutes(currentDelayMinutes);
        }
    }


    // represents one stop along a journey
    // needed this separate from Station because the same station
    // can appear in loads of journeys
    public class JourneyStop
    {
        //PROPERTIES
        public Station stopStation { get; private set; }

        public DateTime scheduledArrival { get; private set; }

        public DateTime scheduledDeparture { get; private set; }

        public DateTime predictedArrival { get; private set; }

        public DateTime predictedDeparture { get; private set; }

        // has the train already been through this stop?
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

        // called when we get a delay update - shifts predicted times forward
        public void updatePredictedArrival(int delayMinutes)
        {
            if (!hasPassed)
            {
                predictedArrival = scheduledArrival.AddMinutes(delayMinutes);
                predictedDeparture = scheduledDeparture.AddMinutes(delayMinutes);
            }
        }

        // call this when the train actually passes through
        public void markAsPassed()
        {
            hasPassed = true;
        }
    }


    // stores info about a disruption or delay and why it happened
    // useful for showing users why their train is late
    public class Disruption
    {
        //PROPERTIES
        public string disruptionId { get; private set; }

        // e.g. "signal failure", "person on track"
        public string reason { get; private set; }

        public int estimatedDurationMinutes { get; private set; }

        public DateTime reportedAt { get; private set; }

        // which journeys does this affect
        public List<Journey> affectedJourneys { get; private set; }

        // how bad is it - low, medium, high
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

        // add a journey to the affected list
        public void addAffectedJourney(Journey journey)
        {
            affectedJourneys.Add(journey);
        }
    }


    // stores average speeds for different train types
    // useful for estimating journey times and predicting delays
    public class TrainSpeed
    {
        //PROPERTIES
        public string trainType { get; private set; }

        // average speed in mph
        public int averageSpeed { get; private set; }

        // top speed the train can do in mph
        public int topSpeed { get; private set; }

        //METHODS
        public TrainSpeed(string type, int avgSpeed, int maxSpeed)
        {
            trainType = type;
            averageSpeed = avgSpeed;
            topSpeed = maxSpeed;
        }

        // works out how long a journey will take based on distance in miles
        public double estimateJourneyTime(double distanceMiles)
        {
            // time = distance / speed, gives us hours so we times by 60 for minutes
            return (distanceMiles / averageSpeed) * 60;
        }
    }


    // manages all the different train type speeds in one place
    public class TrainSpeedManager
    {
        //PROPERTIES
        public List<TrainSpeed> trainSpeeds { get; private set; }

        //METHODS
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

        // calculates how long a train will take between two stations
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


    // program entry point
    public class Program
    {
        public static void Main(string[] args)
        {
            // Server connection string - used when importing data from SQL
            string connectionString =
            "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            Console.WriteLine("Train Prediction System Initialised.");

            // data loading from SQL will be implemented here later
            // once stations, trains and schedules are imported,
            // prediction and journey calculations will run

            Console.ReadLine();
        }
    }

}