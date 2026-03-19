using System.Collections.Generic;

namespace TestReposit
{
    public class Train
    {
        //PROPERTIES
        public string trainNumber { get; private set; }

        //e.g. thameslink:
        public string trainCompany { get; private set; }

        public int trainCarriages { get; private set; }

        // average speed in mph, used for journey time calculations:
        public int averageSpeed { get; private set; }

        //will be used to store the names of each stop on the journey in a list:
        public List<Station> trainStops { get; private set; }

        //METHODS
        public Train(string number, string company, int carriages, int avgSpeed)
        {
            trainNumber = number;
            trainCompany = company;
            trainCarriages = carriages;
            averageSpeed = avgSpeed;
            trainStops = new List<Station>();
        }
    }
}