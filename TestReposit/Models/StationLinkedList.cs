using System;

namespace TestReposit
{
    // this is a single node in our linked list
    // think of it like one carriage in a train - it holds data and points to the next one
    public class StationNode
    {
        //PROPERTIES
        // the station this node holds
        public Station station { get; private set; }
        // points to the next station in the line
        // null if this is the last station
        public StationNode next { get; set; }

        //METHODS
        public StationNode(Station station)
        {
            this.station = station;
            next = null; // starts with no next station
        }
    }

    // this is our custom linked list for storing stations
    // we built this ourselves instead of using List<> to meet the brief requirements
    // time complexity:
    // - addStation (at end): O(n) - has to walk to the end of the list
    // - addAtFront: O(1) - just updates the head pointer
    // - findStation: O(n) - has to walk through the list to find it
    // - removeStation: O(n) - has to walk through the list to find and remove it
    // - getCount: O(1) - we keep track of the count as we go
    public class StationLinkedList
    {
        //PROPERTIES
        // the first station in the list
        public StationNode head { get; private set; }
        // how many stations are in the list
        public int count { get; private set; }

        //METHODS
        public StationLinkedList()
        {
            head = null; // starts empty
            count = 0;
        }

        // adds a station to the end of the list
        // O(n) because we have to walk to the end first
        public void addStation(Station station)
        {
            StationNode newNode = new StationNode(station);

            // if the list is empty just set it as the head
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                // walk to the end of the list
                StationNode current = head;
                while (current.next != null)
                {
                    current = current.next;
                }
                // add the new station at the end
                current.next = newNode;
            }
            count++;
        }

        // adds a station to the front of the list
        // O(1) because we just update the head pointer
        public void addAtFront(Station station)
        {
            StationNode newNode = new StationNode(station);
            newNode.next = head;
            head = newNode;
            count++;
        }

        // finds a station by name and returns it
        // O(n) because we have to walk through the list
        // ignores capitals so "paddington" and "Paddington" both work
        public Station findStation(string stationName)
        {
            StationNode current = head;
            while (current != null)
            {
                if (current.station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase))
                    return current.station; // found it
                current = current.next;
            }
            return null; // station not found
        }

        // removes a station from the list by name
        // O(n) because we have to walk through the list to find it
        public void removeStation(string stationName)
        {
            if (head == null) return; // list is empty

            // if its the first station just update the head
            if (head.station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase))
            {
                head = head.next;
                count--;
                return;
            }

            // walk through the list looking for it
            StationNode current = head;
            while (current.next != null)
            {
                if (current.next.station.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase))
                {
                    // skip over the node we want to remove
                    current.next = current.next.next;
                    count--;
                    return;
                }
                current = current.next;
            }
        }

        // gets the station at a specific position in the list
        // O(n) because we have to walk to that position
        public Station getAt(int index)
        {
            if (index < 0 || index >= count)
                return null; // index out of range

            StationNode current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.next;
            }
            return current.station;
        }

        // checks if a station is in the list
        // O(n) because we have to walk through the list
        public bool contains(string stationName)
        {
            return findStation(stationName) != null;
        }

        // prints out all stations in the list in order
        public void displayAll()
        {
            StationNode current = head;
            while (current != null)
            {
                if (current.next != null)
                    Console.WriteLine($"  {current.station.stationName} -->");
                else
                    Console.WriteLine($"  {current.station.stationName}");
                current = current.next;
            }
        }
    }
}