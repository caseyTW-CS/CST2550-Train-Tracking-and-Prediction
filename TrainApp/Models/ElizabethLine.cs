/*using Syusing System;

namespace TrainApp.Models
{
    // this class represents the elizabeth line specifically
    // we built it for just this one line but it is to be generalised for other lines
    public class ElizabethLine
    {
        //PROPERTIES
        // the name of the line
        public string lineName { get; private set; }
        // main line stations from reading/heathrow through central london to whitechapel
        // now uses our custom linked list instead of List<> to meet the data structure requirement
        public StationLinkedList mainLine { get; private set; }
        // the branch line stations from shenfield from whitechapel (EAST)
        public StationLinkedList shenfieldBranch { get; private set; }
        // the branch line stations from abbey wood from whitechapel (SOUTH-EAST)
        public StationLinkedList abbeyWoodBranch { get; private set; }

        //METHODS
        public ElizabethLine()
        {
            lineName = "Elizabeth Line";
            mainLine = new StationLinkedList();
            shenfieldBranch = new StationLinkedList();
            abbeyWoodBranch = new StationLinkedList();
            setupMainLine();
            setupShenfieldBranch();
            setupAbbeyWoodBranch();
        }

        // sets up the main line from reading/heathrow through central london to whitechapel
        // whitechapel is the last stop before the line splits into two branches
        private void setupMainLine()
        {
            mainLine.addStation(new Station("Reading", "Large", 7, new GeoCoords(51.4587, -0.9719)));
            mainLine.addStation(new Station("Twyford", "Small", 2, new GeoCoords(51.4754, -0.8614)));
            mainLine.addStation(new Station("Maidenhead", "Medium", 2, new GeoCoords(51.5183, -0.7177)));
            mainLine.addStation(new Station("Taplow", "Small", 1, new GeoCoords(51.5238, -0.6882)));
            mainLine.addStation(new Station("Burnham", "Small", 1, new GeoCoords(51.5238, -0.6527)));
            mainLine.addStation(new Station("Slough", "Medium", 4, new GeoCoords(51.5113, -0.5950)));
            mainLine.addStation(new Station("Langley", "Small", 1, new GeoCoords(51.5057, -0.5541)));
            mainLine.addStation(new Station("Iver", "Small", 1, new GeoCoords(51.5100, -0.5050)));
            mainLine.addStation(new Station("West Drayton", "Small", 2, new GeoCoords(51.5096, -0.4472)));
            mainLine.addStation(new Station("Hayes & Harlington", "Small", 2, new GeoCoords(51.5069, -0.4225)));
            mainLine.addStation(new Station("Heathrow Terminal 2 & 3", "Large", 2, new GeoCoords(51.4713, -0.4524)));
            mainLine.addStation(new Station("Heathrow Terminal 4", "Medium", 2, new GeoCoords(51.4585, -0.4466)));
            mainLine.addStation(new Station("Heathrow Terminal 5", "Large", 2, new GeoCoords(51.4733, -0.4889)));
            mainLine.addStation(new Station("Southall", "Small", 2, new GeoCoords(51.5057, -0.3776)));
            mainLine.addStation(new Station("Hanwell", "Small", 1, new GeoCoords(51.5100, -0.3394)));
            mainLine.addStation(new Station("West Ealing", "Small", 2, new GeoCoords(51.5132, -0.3228)));
            mainLine.addStation(new Station("Ealing Broadway", "Medium", 3, new GeoCoords(51.5152, -0.3017)));
            mainLine.addStation(new Station("Paddington", "Large", 6, new GeoCoords(51.5154, -0.1755)));
            mainLine.addStation(new Station("Bond Street", "Large", 3, new GeoCoords(51.5142, -0.1494)));
            mainLine.addStation(new Station("Tottenham Court Road", "Large", 4, new GeoCoords(51.5165, -0.1299)));
            mainLine.addStation(new Station("Farringdon", "Medium", 2, new GeoCoords(51.5203, -0.1051)));
            mainLine.addStation(new Station("City Thameslink", "Small", 1, new GeoCoords(51.5141, -0.1028)));
            mainLine.addStation(new Station("Liverpool Street", "Large", 4, new GeoCoords(51.5183, -0.0823)));
            mainLine.addStation(new Station("Whitechapel", "Medium", 3, new GeoCoords(51.5197, -0.0594))); // line splits here
        }

        // sets up the shenfield branch going north east from whitechapel
        private void setupShenfieldBranch()
        {
            // starts at whitechapel where the line splits
            shenfieldBranch.addStation(new Station("Whitechapel", "Medium", 3, new GeoCoords(51.5197, -0.0594)));
            shenfieldBranch.addStation(new Station("Stratford", "Large", 6, new GeoCoords(51.5416, -0.0042)));
            shenfieldBranch.addStation(new Station("Maryland", "Small", 2, new GeoCoords(51.5461, 0.0039)));
            shenfieldBranch.addStation(new Station("Forest Gate", "Small", 2, new GeoCoords(51.5496, 0.0311)));
            shenfieldBranch.addStation(new Station("Manor Park", "Small", 2, new GeoCoords(51.5511, 0.0513)));
            shenfieldBranch.addStation(new Station("Ilford", "Medium", 3, new GeoCoords(51.5583, 0.0750)));
            shenfieldBranch.addStation(new Station("Seven Kings", "Small", 2, new GeoCoords(51.5641, 0.0894)));
            shenfieldBranch.addStation(new Station("Goodmayes", "Small", 2, new GeoCoords(51.5652, 0.1047)));
            shenfieldBranch.addStation(new Station("Chadwell Heath", "Small", 2, new GeoCoords(51.5657, 0.1267)));
            shenfieldBranch.addStation(new Station("Romford", "Medium", 3, new GeoCoords(51.5752, 0.1833)));
            shenfieldBranch.addStation(new Station("Gidea Park", "Small", 2, new GeoCoords(51.5816, 0.2122)));
            shenfieldBranch.addStation(new Station("Harold Wood", "Small", 2, new GeoCoords(51.5908, 0.2336)));
            shenfieldBranch.addStation(new Station("Brentwood", "Medium", 2, new GeoCoords(51.6185, 0.3005)));
            shenfieldBranch.addStation(new Station("Shenfield", "Medium", 2, new GeoCoords(51.6297, 0.3267)));
        }

        // sets up the abbey wood branch going south east from whitechapel
        private void setupAbbeyWoodBranch()
        {
            // also starts at whitechapel where the line splits
            abbeyWoodBranch.addStation(new Station("Whitechapel", "Medium", 3, new GeoCoords(51.5197, -0.0594)));
            abbeyWoodBranch.addStation(new Station("Shadwell", "Small", 2, new GeoCoords(51.5113, -0.0569)));
            abbeyWoodBranch.addStation(new Station("Wapping", "Small", 2, new GeoCoords(51.5044, -0.0553)));
            abbeyWoodBranch.addStation(new Station("Rotherhithe", "Small", 2, new GeoCoords(51.5006, -0.0522)));
            abbeyWoodBranch.addStation(new Station("Canada Water", "Medium", 3, new GeoCoords(51.4982, -0.0502)));
            abbeyWoodBranch.addStation(new Station("Surrey Quays", "Small", 2, new GeoCoords(51.4933, -0.0478)));
            abbeyWoodBranch.addStation(new Station("New Cross", "Small", 2, new GeoCoords(51.4760, -0.0325)));
            abbeyWoodBranch.addStation(new Station("New Cross Gate", "Small", 2, new GeoCoords(51.4757, -0.0402)));
            abbeyWoodBranch.addStation(new Station("Abbey Wood", "Medium", 2, new GeoCoords(51.4908, 0.1198)));
        }

        // finds a station by name, checks all three sections
        // ignores capital letters so "paddington" and "Paddington" both work
        public Station getStation(string stationName)
        {
            Station found = mainLine.findStation(stationName);
            if (found != null) return found;
            found = shenfieldBranch.findStation(stationName);
            if (found != null) return found;
            return abbeyWoodBranch.findStation(stationName);
        }

        // works out which branch a station is on
        // ignores capital letters so "paddington" and "Paddington" both work
        public string getBranch(string stationName)
        {
            if (mainLine.findStation(stationName) != null) return "Main Line";
            if (shenfieldBranch.findStation(stationName) != null) return "Shenfield Branch";
            if (abbeyWoodBranch.findStation(stationName) != null) return "Abbey Wood Branch";
            return "Not found"; // station not on the elizabeth line
        }

        // works out the distance between two stations using their GPS coordinates
        // no need to hardcode distances since GeoCoords does it for us
        public double getDistanceBetween(Station from, Station to)
        {
            return from.stationLocation.distanceTo(to.stationLocation);
        }

        // works out the total distance of a journey across multiple stops
        public double getTotalDistance(Station from, Station to)
        {
            // figure out which linked list the from station is in
            StationLinkedList line = getLineForStation(from.stationName);
            if (line == null) return -1; // station not found

            double total = 0;
            bool counting = false;
            StationNode current = line.head;

            // walk through the linked list adding up distances between stops
            while (current != null)
            {
                if (current.station.stationName.Equals(from.stationName, StringComparison.OrdinalIgnoreCase))
                    counting = true; // start counting from here

                if (counting && current.next != null)
                {
                    total += current.station.stationLocation.distanceTo(current.next.station.stationLocation);

                    if (current.next.station.stationName.Equals(to.stationName, StringComparison.OrdinalIgnoreCase))
                        break; // stop when we reach the destination
                }
                current = current.next;
            }
            return total;
        }

        // helper method to get the right linked list for a station
        private StationLinkedList getLineForStation(string stationName)
        {
            if (mainLine.findStation(stationName) != null) return mainLine;
            if (shenfieldBranch.findStation(stationName) != null) return shenfieldBranch;
            if (abbeyWoodBranch.findStation(stationName) != null) return abbeyWoodBranch;
            return null; // station not found on any branch
        }

        // prints out all stations on the full line
        public void displayRoute()
        {
            //Console.WriteLine($"-- {lineName} --");
            //Console.WriteLine("Main Line (Reading/Heathrow to Whitechapel):");
            for (int i = 0; i < mainLine.count; i++)
            {
                Station current = mainLine.getAt(i);
                Station next = mainLine.getAt(i + 1);
                if (next != null)
                {
                    double dist = current.stationLocation.distanceTo(next.stationLocation);
                    //Console.WriteLine($"  {current.stationName} --> ({dist:F1} miles)");
                }
                else
                    //Console.WriteLine($"  {current.stationName} (splits here)");
            }
            //Console.WriteLine("\nShenfield Branch:");
            for (int i = 0; i < shenfieldBranch.count; i++)
            {
                Station current = shenfieldBranch.getAt(i);
                Station next = shenfieldBranch.getAt(i + 1);
                if (next != null)
                {
                    double dist = current.stationLocation.distanceTo(next.stationLocation);
                    //Console.WriteLine($"  {current.stationName} --> ({dist:F1} miles)");
                }
                else
                    //Console.WriteLine($"  {current.stationName}");
            }
            //Console.WriteLine("\nAbbey Wood Branch:");
            for (int i = 0; i < abbeyWoodBranch.count; i++)
            {
                Station current = abbeyWoodBranch.getAt(i);
                Station next = abbeyWoodBranch.getAt(i + 1);
                if (next != null)
                {
                    double dist = current.stationLocation.distanceTo(next.stationLocation);
                    //Console.WriteLine($"  {current.stationName} --> ({dist:F1} miles)");
                }
                else
                    //Console.WriteLine($"  {current.stationName}");
            }
        }
    }
}*/