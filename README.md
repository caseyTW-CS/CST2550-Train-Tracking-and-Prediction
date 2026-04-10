# Where's the Train? 🚆

Where's the Train? is a web-based application for tracking Elizabeth Line services in real time using the Transport for London (TfL) API. The system allows users to check live departures, monitor service disruptions, and manage personalised travel preferences.

## Overview

This application is designed to provide a simple and accessible way for users to stay informed about train services. It integrates live data from TfL and presents it through a clean and responsive interface.

## Features

### Live Service Alerts
- Displays real-time disruption information
- Classifies severity levels (e.g. minor, moderate, major)
- Identifies affected sections of the line
- Includes a notification badge in the navigation bar
  

### Departures Board
- Allows users to search any Elizabeth Line station
- Displays upcoming trains with relevant details such as destination and arrival time



### Journey Search
- Enables users to search routes between stations
- Provides a straightforward and user-friendly interface



### User Accounts
- Supports user login functionality
- Provides a personalised experience



### Favourite Stations
- Users can save frequently used stations
- Favourites are accessible from the navigation bar



### Alert Notifications
- Displays a live alert counter
- Allows users to clear and restore alerts
- Maintains alert state using session storage



## Technologies Used

- ASP.NET Core Razor Pages
- C#
- JavaScript (Fetch API)
- Bootstrap
- HTML and CSS
- TfL Unified API



## API Integration

The application uses the Transport for London (TfL) API:

https://api.tfl.gov.uk/Line/elizabeth
https://api.tfl.gov.uk

This provides:
- Service status information
- Disruption descriptions
- Severity levels



## System Design

The application follows a structured approach using Razor Pages, separating concerns between:

- Models: handling data and API interaction (e.g. LiveMap, Train Track, Departures & Alerts)
- Pages: managing UI and user interaction
- JavaScript: handling dynamic updates and real-time behaviour


## How to Run

1. Clone the repository:
   
2. Open the project in Visual Studio

3. Build and run the application

4. Navigate to the local host address provided

## Authors

Casey Tyler Woods 

Rishon Ramdany

Ishaan Hari

Naga Arun Mahesh
