using System;
using BCrypt.Net;

namespace TrainProject
{
    public class Users
    {
        //Attributes:
        public string userName { get; set; }
        public string userPass { get; set; }
        public int userAge { get; set; }
        public string userEmail { get; set; }
        public string userPhone { get; set; }

        public Users() 
        { 
        } 

        //Validate login method
        public bool validateLogin(string enteredPass)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPass, userPass);
        }
    }
}