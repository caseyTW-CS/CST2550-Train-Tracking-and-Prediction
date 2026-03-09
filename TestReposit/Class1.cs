using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestReposit;
using System.Data;
using System.Data.SqlClient;
    
namespace TestReposit
{
    public class Class1
    {
        
    }

    public class User
    {
        //PROPERTIES
        public int userID { get; set; }
        public string userName {  get; private set; }
        public string userPass { get; private set; }
        //can be used for recommending tickets types/railcards:
        public int userAge { get; private set; }
        public string userEmail { get; private set;  }
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
        public string trainNumber {  get; private set; }
        //referring to commuter, freight, etc:
        public string trainType { get; private set; }
        //e.g. thameslink:
        public string trainCompany { get; private set; }
        public int trainCarriages {  get; private set; }
        //will be used to store the names of each stop on the journey in a list:
        public List<Station> trainStops { get; private set; }

        //METHODS
        public Train(string number, string type, string company, int carriages)
        {
            trainNumber = number;
            trainType = type;
            trainCompany = company;
            trainCarriages = carriages;
            trainStops = new List<Station>();
        }
    }
    
}
// represents a single ticket purchased by a user
// i made this a class instead of just a string so we can actually store useful info
public class Ticket
{
    //PROPERTIES
    public string ticketId { get; private set; }
    public string ticketType { get; private set; } // e.g. single, return, season
    public decimal ticketPrice { get; private set; }
    public string ticketClass { get; private set; } // first or standard
    public string railcardApplied { get; private set; } // which railcard was used if any
    public DateTime purchaseDate { get; private set; }
    // the journey this ticket is for
    public Journey ticketJourney { get; private set; }

    //METHODS
    public Ticket(string id, string type, decimal price, string ticketClass, string railcard, Journey journey)
    {
        ticketId = id;
        ticketType = type;
        ticketPrice = price;
        this.ticketClass = ticketClass;
        railcardApplied = railcard;
        purchaseDate = DateTime.Now; // just set it to now when the ticket is created
        ticketJourney = journey;
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
    public DateTime? actualDeparture { get; private set; } // nullable because it might not have left yet
    public DateTime? actualArrival { get; private set; }
    // how many minutes delayed the train currently is
    public int currentDelayMinutes { get; private set; }
    // list of all stops with their own scheduled/actual times
    public List<JourneyStop> stops { get; private set; }

    //METHODS
    public Journey(string id, Train train, Station departure, Station arrival, DateTime scheduledDep, DateTime scheduledArr)
    {
        journeyId = id;
        journeyTrain = train;
        departureStation = departure;
        arrivalStation = arrival;
        scheduledDeparture = scheduledDep;
        scheduledArrival = scheduledArr;
        currentDelayMinutes = 0; // starts with no delay 
        stops = new List<JourneyStop>();
    }

    // this is the main prediction method - takes the current delay and works out
    // when the train will arrive at each remaining stop
    public void updateDelay(int delayMinutes)
    {
        currentDelayMinutes = delayMinutes;
        // go through each stop and push the predicted time forward by the delay
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
        else if (currentDelayMinutes > 0 && currentDelayMinutes <= 5)
            return $"Slight Delay ({currentDelayMinutes} mins)"; // within 5 mins
        else if (currentDelayMinutes > 5)
            return $"Delayed ({currentDelayMinutes} mins)";
        else
            return "Unknown";
    }

    // returns predicted arrival at the final destination
    public DateTime getPredictedArrival()
    {
        return scheduledArrival.AddMinutes(currentDelayMinutes);
    }
}

// represents one stop along a journey
// needed this separate from Station because the same station can appear in loads of journeys
public class JourneyStop
{
    //PROPERTIES
    public Station stopStation { get; private set; }
    public DateTime scheduledArrival { get; private set; }
    public DateTime scheduledDeparture { get; private set; }
    public DateTime predictedArrival { get; private set; }
    public DateTime predictedDeparture { get; private set; }
    public bool hasPassed { get; private set; } // has the train already been through this stop?

    //METHODS
    public JourneyStop(Station station, DateTime scheduledArr, DateTime scheduledDep)
    {
        stopStation = station;
        scheduledArrival = scheduledArr;
        scheduledDeparture = scheduledDep;
        predictedArrival = scheduledArr; // starts the same as scheduled
        predictedDeparture = scheduledDep;
        hasPassed = false;
    }

    // called when we get a delay update - shifts predicted times forward
    public void updatePredictedArrival(int delayMinutes)
    {
        if (!hasPassed) // no point updating stops the train has already been through
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
    public string reason { get; private set; } // e.g. "signal failure", "person on track"
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

// stores the timetable for a train service
// basically what times a train is supposed to be where
public class Schedule
{
    //PROPERTIES
    public string scheduleId { get; private set; }
    public Train scheduledTrain { get; private set; }
    // days this schedule runs on e.g. "Monday-Friday" or "Saturday"
    public string operatingDays { get; private set; }
    // the list of stops and times in order
    public List<JourneyStop> timetabledStops { get; private set; }
    public bool isActive { get; private set; } // is this schedule currently running?

    //METHODS
    public Schedule(string id, Train train, string operatingDays)
    {
        scheduleId = id;
        scheduledTrain = train;
        this.operatingDays = operatingDays;
        timetabledStops = new List<JourneyStop>();
        isActive = true;
    }

    // add a stop to the timetable
    public void addStop(JourneyStop stop)
    {
        timetabledStops.Add(stop);
    }

    // cancel the schedule e.g. if the service is not running today
    public void cancelSchedule()
    {
        isActive = false;
    }
    // handles all the user account stuff like logging in and registering
    // basically simulates what the sql database does on the c# side
    public class UserManager
    {
        //PROPERTIES
        // stores all the users in the system
        public List<User> allUsers { get; private set; }

        //METHODS
        public UserManager()
        {
            allUsers = new List<User>();
        }

        // adds a new user to the system
        public void registerUser(User user)
        {
            allUsers.Add(user);
        }

        // checks if the password the user typed matches their account
        public bool validateLogin(User user, string enteredPass)
        {
            return user.userPass == enteredPass; // simple check for now
        }

        // checks if a username and password match any user in the system
        // returns the user if found, null if not
        public User loginUser(string userName, string userPass)
        {
            // loop through all users and check if the username and password match
            foreach (var user in allUsers)
            {
                if (user.userName == userName && validateLogin(user, userPass))
                    return user; // found them
            }
            return null; // no match found
        }

        // finds a user by their username
        public User findUser(string userName)
        {
            foreach (var user in allUsers)
            {
                if (user.userName == userName)
                    return user;
            }
            return null; // user doesnt exist
        }

        // removes a user from the system
        public void deleteUser(string userName)
        {
            var user = findUser(userName);
            if (user != null)
                allUsers.Remove(user);
        }
    }

    // takes a disruption and works out which journeys are affected and how late theyll be
    public class DelayPredictor
    {
        //PROPERTIES
        // keeps track of all active journeys so we can update them
        public List<Journey> activeJourneys { get; private set; }

        //METHODS
        public DelayPredictor()
        {
            activeJourneys = new List<Journey>();
        }

        // add a journey to the tracker
        public void addJourney(Journey journey)
        {
            activeJourneys.Add(journey);
        }

        // this is the main one - takes a disruption and pushes the delay
        // through all the journeys it affects automatically
        public void applyDisruption(Disruption disruption)
        {
            foreach (var journey in disruption.affectedJourneys)
            {
                // work out delay from the disruption duration
                journey.updateDelay(disruption.estimatedDurationMinutes);
            }
        }

        // works out which journeys a disruption at a specific station will affect
        // e.g. if kings cross has a signal failure, find all trains going through it
        public List<Journey> getAffectedJourneys(Station station)
        {
            var affected = new List<Journey>();
            foreach (var journey in activeJourneys)
            {
                // check if any of the journeys stops include this station
                foreach (var stop in journey.stops)
                {
                    if (stop.stopStation.stationName == station.stationName && !stop.hasPassed)
                    {
                        affected.Add(journey);
                        break; // no need to check the rest of the stops for this journey
                    }
                }
            }
            return affected;
        }

        // returns all journeys that are currently delayed
        public List<Journey> getDelayedJourneys()
        {
            var delayed = new List<Journey>();
            foreach (var journey in activeJourneys)
            {
                if (journey.currentDelayMinutes > 0)
                    delayed.Add(journey);
            }
            return delayed;
        }

        // returns a predicted arrival time for a journey based on current delay
        public DateTime predictArrival(Journey journey)
        {
            return journey.getPredictedArrival();
        }
    }

    // handles everything to do with the live departure board at a station
    // basically what youd see on the screens at the station
    public class DepartureBoard
    {
        //PROPERTIES
        public Station boardStation { get; private set; }
        // all journeys passing through this station
        public List<Journey> stationJourneys { get; private set; }

        //METHODS
        public DepartureBoard(Station station)
        {
            boardStation = station;
            stationJourneys = new List<Journey>();
        }

        // add a journey to this stations board
        public void addJourney(Journey journey)
        {
            stationJourneys.Add(journey);
        }

        // returns all trains that havent departed yet, sorted by scheduled departure
        public List<Journey> getUpcomingDepartures()
        {
            var upcoming = new List<Journey>();
            foreach (var journey in stationJourneys)
            {
                // only show trains that havent left yet
                if (journey.departureStation.stationName == boardStation.stationName
                    && journey.actualDeparture == null)
                {
                    upcoming.Add(journey);
                }
            }
            // sort by scheduled departure time so earliest is first
            upcoming.Sort((a, b) => a.scheduledDeparture.CompareTo(b.scheduledDeparture));
            return upcoming;
        }

        // returns all trains arriving at this station that havent arrived yet
        public List<Journey> getUpcomingArrivals()
        {
            var arrivals = new List<Journey>();
            foreach (var journey in stationJourneys)
            {
                if (journey.arrivalStation.stationName == boardStation.stationName
                    && journey.actualArrival == null)
                {
                    arrivals.Add(journey);
                }
            }
            arrivals.Sort((a, b) => a.scheduledArrival.CompareTo(b.scheduledArrival));
            return arrivals;
        }

        // prints out the departure board like youd see at the station 
        public void displayBoard()
        {
            Console.WriteLine($"-- Departure Board: {boardStation.stationName} --");
            Console.WriteLine($"{"Destination",-25} {"Scheduled",-12} {"Predicted",-12} {"Status",-20} {"Platform",-10}");
            Console.WriteLine(new string('-', 80));

            foreach (var journey in getUpcomingDepartures())
            {
                Console.WriteLine(
                    $"{journey.arrivalStation.stationName,-25} " +
                    $"{journey.scheduledDeparture.ToString("HH:mm"),-12} " +
                    $"{journey.getPredictedArrival().ToString("HH:mm"),-12} " +
                    $"{journey.getStatus(),-20} " +
                    $"{journey.departureStation.stationPlatform,-10}"
                );
            }
        }
    }
}

public class Program
{
    public void Main(string[] args)
    {
        //Server connection string
        string connectionString = "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";


        Station example1 = new Station(
            "Example station 1",
            "Regular",
            2,
            new GeoCoords(50.0000, 100.0000)
        );

        Train train1 = new Train("T001", "Commuter", "FakeBrand", 8);

        train1.trainStops.Add(example1);
    }
}
