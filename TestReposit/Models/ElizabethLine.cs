using System;
using System.Collections.Generic;

// this class represents the elizabeth line specifically
// we built it for just this one line but it is to be generalised for other lines
public class ElizabethLine
{
    // PROPERTIES
    // the name of the line
    public string lineName { get; private set; }
    // main line stations from reading/heathrow through central london to whitechapel
    public List<Station> mainLine { get; private set; }
    // the branch line stations from shenfield from whitechapel (EAST)
    public List<Station> shenfieldBranch { get; private set; }
    // the branch line stations from abbey wood from whitechapel (SOUTH-EAST)
    public List<Station> abbeyWoodBranch { get; private set; }

    // METHODS
    public ElizabethLine()
    {
        lineName = "Elizabeth Line";
        mainLine = new List<Station>();
        shenfieldBranch = new List<Station>();
        abbeyWoodBranch = new List<Station>();
        setupMainLine();
        setupShenfieldBranch();
        setupAbbeyWoodBranch();
    }

    // sets up the main line from reading/heathrow through central london to whitechapel
    // whitechapel is the last stop before the line splits into two branches
    private void setupMainLine()
    {
        mainLine.Add(new Station("Reading", "Large", 7, new GeoCoords(51.4587, -0.9719)));
        mainLine.Add(new Station("Twyford", "Small", 2, new GeoCoords(51.4754, -0.8614)));
        mainLine.Add(new Station("Maidenhead", "Medium", 2, new GeoCoords(51.5183, -0.7177)));
        mainLine.Add(new Station("Taplow", "Small", 1, new GeoCoords(51.5238, -0.6882)));
        mainLine.Add(new Station("Burnham", "Small", 1, new GeoCoords(51.5238, -0.6527)));
        mainLine.Add(new Station("Slough", "Medium", 4, new GeoCoords(51.5113, -0.5950)));
        mainLine.Add(new Station("Langley", "Small", 1, new GeoCoords(51.5057, -0.5541)));
        mainLine.Add(new Station("Iver", "Small", 1, new GeoCoords(51.5100, -0.5050)));
        mainLine.Add(new Station("West Drayton", "Small", 2, new GeoCoords(51.5096, -0.4472)));
        mainLine.Add(new Station("Hayes & Harlington", "Small", 2, new GeoCoords(51.5069, -0.4225)));
        mainLine.Add(new Station("Heathrow Terminal 2 & 3", "Large", 2, new GeoCoords(51.4713, -0.4524)));
        mainLine.Add(new Station("Heathrow Terminal 4", "Medium", 2, new GeoCoords(51.4585, -0.4466)));
        mainLine.Add(new Station("Heathrow Terminal 5", "Large", 2, new GeoCoords(51.4733, -0.4889)));
        mainLine.Add(new Station("Southall", "Small", 2, new GeoCoords(51.5057, -0.3776)));
        mainLine.Add(new Station("Hanwell", "Small", 1, new GeoCoords(51.5100, -0.3394)));
        mainLine.Add(new Station("West Ealing", "Small", 2, new GeoCoords(51.5132, -0.3228)));
        mainLine.Add(new Station("Ealing Broadway", "Medium", 3, new GeoCoords(51.5152, -0.3017)));
        mainLine.Add(new Station("Paddington", "Large", 6, new GeoCoords(51.5154, -0.1755)));
        mainLine.Add(new Station("Bond Street", "Large", 3, new GeoCoords(51.5142, -0.1494)));
        mainLine.Add(new Station("Tottenham Court Road", "Large", 4, new GeoCoords(51.5165, -0.1299)));
        mainLine.Add(new Station("Farringdon", "Medium", 2, new GeoCoords(51.5203, -0.1051)));
        mainLine.Add(new Station("City Thameslink", "Small", 1, new GeoCoords(51.5141, -0.1028)));
        mainLine.Add(new Station("Liverpool Street", "Large", 4, new GeoCoords(51.5183, -0.0823)));
        mainLine.Add(new Station("Whitechapel", "Medium", 3, new GeoCoords(51.5197, -0.0594))); // line splits here
    }

    // sets up the shenfield branch going north east from whitechapel
    private void setupShenfieldBranch()
    {
        // starts at whitechapel where the line splits
        shenfieldBranch.Add(new Station("Whitechapel", "Medium", 3, new GeoCoords(51.5197, -0.0594)));
        shenfieldBranch.Add(new Station("Stratford", "Large", 6, new GeoCoords(51.5416, -0.0042)));
        shenfieldBranch.Add(new Station("Maryland", "Small", 2, new GeoCoords(51.5461, 0.0039)));
        shenfieldBranch.Add(new Station("Forest Gate", "Small", 2, new GeoCoords(51.5496, 0.0311)));
        shenfieldBranch.Add(new Station("Manor Park", "Small", 2, new GeoCoords(51.5511, 0.0513)));
        shenfieldBranch.Add(new Station("Ilford", "Medium", 3, new GeoCoords(51.5583, 0.0750)));
        shenfieldBranch.Add(new Station("Seven Kings", "Small", 2, new GeoCoords(51.5641, 0.0894)));
        shenfieldBranch.Add(new Station("Goodmayes", "Small", 2, new GeoCoords(51.5652, 0.1047)));
        shenfieldBranch.Add(new Station("Chadwell Heath", "Small", 2, new GeoCoords(51.5657, 0.1267)));
        shenfieldBranch.Add(new Station("Romford", "Medium", 3, new GeoCoords(51.5752, 0.1833)));
        shenfieldBranch.Add(new Station("Gidea Park", "Small", 2, new GeoCoords(51.5816, 0.2122)));
        shenfieldBranch.Add(new Station("Harold Wood", "Small", 2, new GeoCoords(51.5908, 0.2336)));
        shenfieldBranch.Add(new Station("Brentwood", "Medium", 2, new GeoCoords(51.6185, 0.3005)));
        shenfieldBranch.Add(new Station("Shenfield", "Medium", 2, new GeoCoords(51.6297, 0.3267)));
    }

    // sets up the abbey wood branch going south east from whitechapel
    private void setupAbbeyWoodBranch()
    {
        // also starts at whitechapel where the line splits
        abbeyWoodBranch.Add(new Station("Whitechapel", "Medium", 3, new GeoCoords(51.5197, -0.0594)));
        abbeyWoodBranch.Add(new Station("Shadwell", "Small", 2, new GeoCoords(51.5113, -0.0569)));
        abbeyWoodBranch.Add(new Station("Wapping", "Small", 2, new GeoCoords(51.5044, -0.0553)));
        abbeyWoodBranch.Add(new Station("Rotherhithe", "Small", 2, new GeoCoords(51.5006, -0.0522)));
        abbeyWoodBranch.Add(new Station("Canada Water", "Medium", 3, new GeoCoords(51.4982, -0.0502)));
        abbeyWoodBranch.Add(new Station("Surrey Quays", "Small", 2, new GeoCoords(51.4933, -0.0478)));
        abbeyWoodBranch.Add(new Station("New Cross", "Small", 2, new GeoCoords(51.4760, -0.0325)));
        abbeyWoodBranch.Add(new Station("New Cross Gate", "Small", 2, new GeoCoords(51.4757, -0.0402)));
        abbeyWoodBranch.Add(new Station("Abbey Wood", "Medium", 2, new GeoCoords(51.4908, 0.1198)));
    }

    // works out the distance between two stations using their GPS coordinates
    public double getDistanceBetween(Station from, Station to)
    {
        return from.stationLocation.distanceTo(to.stationLocation);
    }

    // works out the total distance of a journey across multiple stops
    public double getTotalDistance(Station from, Station to)
    {
        // figures out which list the stations are in
        List<Station> line = getLineForStation(from.stationName);
        if (line == null)
            return -1; // station not found on any line

        int fromIndex = line.IndexOf(from);
        int toIndex = line.IndexOf(to);
        if (fromIndex == -1 || toIndex == -1)
            return -1; // one of the stations wasnt found

        double total = 0;
        int step = fromIndex < toIndex ? 1 : -1;
        // adds up the distances between each stop along the way
        for (int i = fromIndex; i != toIndex; i += step)
        {
            total += line[i].stationLocation.distanceTo(line[i + step].stationLocation);
        }
        return total;
    }

    // finds a station by name, checks all three sections
    // ignores capital letters so "paddington" and "Paddington" both work
    public Station getStation(string stationName)
    {
        foreach (Station station in mainLine)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return station;
        foreach (Station station in shenfieldBranch)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return station;
        foreach (Station station in abbeyWoodBranch)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return station;
        return null; // station not found
    }

    // works out which branch a station is on
    // ignores capital letters so "paddington" and "Paddington" both work
    public string getBranch(string stationName)
    {
        foreach (Station station in mainLine)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return "Main Line";
        foreach (Station station in shenfieldBranch)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return "Shenfield Branch";
        foreach (Station station in abbeyWoodBranch)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return "Abbey Wood Branch";
        return "Not found"; // station not on the elizabeth line
    }

    // helper method to get the right list for a station
    // used internally to figure out which branch a station is on
    private List<Station> getLineForStation(string stationName)
    {
        foreach (Station station in mainLine)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return mainLine;
        foreach (Station station in shenfieldBranch)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return shenfieldBranch;
        foreach (Station station in abbeyWoodBranch)
            if (station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase)) return abbeyWoodBranch;
        return null; // station not found on any branch
    }

    // prints out all stations on the full line with distances calculated from coordinates
    public void displayRoute()
    {
        Console.WriteLine($"-- {lineName} --");
        Console.WriteLine("Main Line (Reading/Heathrow to Whitechapel):");
        for (int i = 0; i < mainLine.Count; i++)
        {
            if (i < mainLine.Count - 1)
            {
                double dist = mainLine[i].stationLocation.distanceTo(mainLine[i + 1].stationLocation);
                Console.WriteLine($"  {mainLine[i].stationName} --> ({dist:F1} miles)");
            }
            else
                Console.WriteLine($"  {mainLine[i].stationName} (splits here)");
        }
        Console.WriteLine("\nShenfield Branch:");
        for (int i = 0; i < shenfieldBranch.Count; i++)
        {
            if (i < shenfieldBranch.Count - 1)
            {
                double dist = shenfieldBranch[i].stationLocation.distanceTo(shenfieldBranch[i + 1].stationLocation);
                Console.WriteLine($"  {shenfieldBranch[i].stationName} --> ({dist:F1} miles)");
            }
            else
                Console.WriteLine($"  {shenfieldBranch[i].stationName}");
        }
        Console.WriteLine("\nAbbey Wood Branch:");
        for (int i = 0; i < abbeyWoodBranch.Count; i++)
        {
            if (i < abbeyWoodBranch.Count - 1)
            {
                double dist = abbeyWoodBranch[i].stationLocation.distanceTo(abbeyWoodBranch[i + 1].stationLocation);
                Console.WriteLine($"  {abbeyWoodBranch[i].stationName} --> ({dist:F1} miles)");
            }
            else
                Console.WriteLine($"  {abbeyWoodBranch[i].stationName}");
        }
    }
}