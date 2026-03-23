using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TestReposit;

namespace TestReposit.Models.WebsitePage.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Users newUser { get; set; }

        // for the login form
        [BindProperty]
        public string loginUsername { get; set; }

        [BindProperty]
        public string loginPassword { get; set; }

        // shows a message if login fails
        public string message { get; set; }

        public void OnGet()
        {
        }

        // handles the register form - your friends original code untouched
        public IActionResult OnPostRegister()
        {
            string connectionStringUser = "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            using (SqlConnection conn = new SqlConnection(connectionStringUser))
            {
                conn.Open();

                string query = @"INSERT INTO userInfo (userName, userPass, userAge, userEmail, userPhone)
                                 VALUES (@newUsername, @newPassword, @newAge, @newEmail, @newPhone)";

                SqlCommand cmd = new SqlCommand(query, conn);

                // hash the password before storing it so its not plain text
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.userPass);

                cmd.Parameters.AddWithValue("@newUsername", newUser.userName);
                cmd.Parameters.AddWithValue("@newPassword", hashedPassword);
                cmd.Parameters.AddWithValue("@newAge", newUser.userAge);
                cmd.Parameters.AddWithValue("@newEmail", newUser.userEmail);
                cmd.Parameters.AddWithValue("@newPhone", newUser.userPhone);

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("Index");
        }

        // handles the login form - checks username and password against the database
        public IActionResult OnPostLogin()
        {
            string connectionStringUser = "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            using (SqlConnection conn = new SqlConnection(connectionStringUser))
            {
                conn.Open();

                // get the hashed password for this username from the database
                string query = @"SELECT userPass FROM userInfo 
                                 WHERE userName = @userName";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userName", loginUsername);

                string storedHash = cmd.ExecuteScalar() as string;

                // check if the username exists and the password matches the hash
                if (storedHash != null && BCrypt.Net.BCrypt.Verify(loginPassword, storedHash))
                {
                    // login successful - redirect to home page
                    return RedirectToPage("Index");
                }
                else
                {
                    // login failed - show error message
                    message = "Incorrect username or password";
                    return Page();
                }
            }
        }
    }
}