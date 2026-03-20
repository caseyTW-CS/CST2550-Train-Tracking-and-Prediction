using System;
using System.Collections.Generic;

namespace TestReposit
{
    public class User
    {
        //PROPERTIES
        public int userID { get; set; }
        public string userName { get; private set; }
        public string userPass { get; private set; }

        //can be used for recommending tickets types/railcards:
        public int userAge { get; private set; }

        public string userEmail { get; private set; }

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
            userRailcard = railcard;
            userTickets = new List<string>();
        }

        // checks if the password the user typed matches their account
        public bool validateLogin(string enteredPass)
        {
            return userPass == enteredPass;
        }
    }
}using System;
using System.Collections.Generic;
using BCrypt.Net;

namespace TestReposit
{
    public class User
    {
        //PROPERTIES
        public int userID { get; set; }
        public string userName { get; private set; }
        public string userPass { get; private set; }

        //can be used for recommending tickets types/railcards:
        public int userAge { get; private set; }

        public string userEmail { get; private set; }

        //user phone number:
        public string userPhone { get; private set; }

        //for storing 
        public List<string> userTickets { get; private set; }

        public string userRailcard { get; private set; }

        //METHODS
        public User(string name, string pass, int age, string email, string phone, string railcard)
        {
            userName = name;
            userPass = BCrypt.HashPassword(pass);
            userAge = age;
            userEmail = email;
            userPhone = phone;
            userRailcard = railcard;
            userTickets = new List<string>();
        }

        // checks if the password the user typed matches their account
        public bool validateLogin(string enteredPass)
        {
            return BCrypt.Verify(enteredPass, userPass);
        }
    }
}