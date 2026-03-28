using System;
using BCrypt.Net;

namespace TrainApp.Models
{
    public class User
    {
        //Attributes:
        public string userName { get; set; }
        public string userPass { get; set; }
        public int userAge { get; set; }
        public string userEmail { get; set; }
        public string userPhone { get; set; }

        public User() 
        { 
        } 

        //Validate login method
        public bool validateLogin(string enteredPass)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPass, userPass);
        }
    }
}