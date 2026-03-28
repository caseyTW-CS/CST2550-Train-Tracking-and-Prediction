using System;

namespace TrainApp.Models
{
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
}