using System;
using System.Collections.Generic;

namespace TrainProject
{ 
  // stores a users personal settings and preferences
  // covers "basic user settings" from the brief
  public class UserSettings
{
  //PROPERTIES
  // which user these settings belong to
  public User user { get; private set; }
  // their favourite stations so they dont have to search every time
  public List<string> favouriteStations { get; private set; }
  // the last few stations they searched for
  public List<string> recentSearches { get; private set; }
  // how many recent searches to keep before removing old ones
  // we set it to 5 by default but it can be changed
  public int maxRecentSearches { get; private set; }

  //METHODS
  public UserSettings(User user)
{
  this.user = user;
  favouriteStations = new List<string>();
  recentSearches = new List<string>();
  maxRecentSearches = 5; // only keep the last 5 searches by default
}

  // adds a station to the users favourites
  public void AddFavouriteStation(string stationName)
{
  // dont add it if its already in the list
   if (!favouriteStations.Contains(stationName))
       favouriteStations.Add(stationName);
}

  // removes a station from the users favourites
  public void removeFavourite(string stationName)
{
  favouriteStations.Remove(stationName);
}

  // checks if a station is in the users favourites
  public bool isFavourite(string stationName)
{
  return favouriteStations.Contains(stationName);
}

  // adds a station to recent searches
  // automatically removes the oldest search if the list gets too long
  public void addRecentSearch(string stationName)
{
  // if its already in recent searches remove it first
  // so it gets moved to the top as the most recent
  if (recentSearches.Contains(stationName))
      recentSearches.Remove(stationName);

  // add it to the top of the list
  recentSearches.Insert(0, stationName);

  // if the list is too long remove the oldest search at the bottom
  if (recentSearches.Count > maxRecentSearches)
      recentSearches.RemoveAt(recentSearches.Count - 1);
}

  // clears all recent searches
  public void clearRecentSearches()
{
  recentSearches.Clear();
}

  // prints out the users settings so they can see them
  public void displaySettings()
{
        Console.WriteLine($"-- Settings for {user.userName} --");
        Console.WriteLine("Favourite Stations:");
        if (favouriteStations.Count == 0)
            Console.WriteLine("  No favourites yet");
        else
            foreach (string station in favouriteStations)
                Console.WriteLine($"  {station}");

        Console.WriteLine("Recent Searches:");
        if (recentSearches.Count == 0)
            Console.WriteLine("  No recent searches");
        else
            foreach (string search in recentSearches)
                Console.WriteLine($"  {search}");
        }
    }
}