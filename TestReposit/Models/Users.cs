using System;
using BCrypt.Net;

namespace TestReposit
{
    public class Users
    {
        public string userName { get; set; }
        public string userPass { get; set; }
        public int userAge { get; set; }
        public string userEmail { get; set; }
        public string userPhone { get; set; }

        public Users() 
        { 
        } 

        // Optional: validate login method
        public bool validateLogin(string enteredPass)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPass, userPass);
        }
    }
}