using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestReposit;

namespace TestReposit
{
    public class Class1
    {
        
    }

    public class User
    {
        //PROPERTIES
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
        public List<string> trainStops { get; private set; }

        //METHODS
        public Train(string number, string type, string company, int carriages)
        {
            trainNumber = number;
            trainType = type;
            trainCompany = company;
            trainCarriages = carriages;
            trainStops = new List<string>();
        }
    }
    public class Station
    {
        //PROPERTIES
        public string stationName { get; private set;  }
        //for storing the size of the platforms:
        public string stationSize { get; private set; }
        //for the platform the train will be stopping on:
        public int stationPlatform { get; private set; }
        
        //METHODS
        public Station (string name, string size, int platform)
        {
            stationName = name;
            stationSize = size;
            stationPlatform = platform;
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
        currentDelayMinutes = 0; // starts with no delay obviously
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
            return $"Slight Delay ({currentDelayMinutes} mins)"; // within 5 mins is pretty normal tbh
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
// useful for showing users why their train is late lol
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
}
