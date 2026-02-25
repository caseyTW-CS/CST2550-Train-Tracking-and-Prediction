using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
