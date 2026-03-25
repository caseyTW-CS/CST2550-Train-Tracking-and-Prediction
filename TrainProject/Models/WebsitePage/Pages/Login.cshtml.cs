using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TrainProject;

namespace WebsitePage.Pages 
{
    public class LoginModel : PageModel
    {
        //Attributes:
        [BindProperty]
        public Users newUser { get; set; }

        [BindProperty]
        public string loginUsername { get; set; }

        [BindProperty]
        public string loginPassword { get; set; }

        public string message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostRegister()
        {
            //Connection string to Users sql database
            string connectionStringUser = "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            using (SqlConnection conn = new SqlConnection(connectionStringUser))
            {
                conn.Open();

                string query = @"INSERT INTO userInfo (userName, userPass, userAge, userEmail, userPhone)
                                 VALUES (@newUsername, @newPassword, @newAge, @newEmail, @newPhone)";

                SqlCommand cmd = new SqlCommand(query, conn);

                //Encrypts password before sending
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

        public IActionResult OnPostLogin()
        {
            string connectionStringUser = "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            using (SqlConnection conn = new SqlConnection(connectionStringUser))
            {
                conn.Open();

                string query = @"SELECT userPass FROM userInfo 
                                 WHERE userName = @userName";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userName", loginUsername);

                string storedHash = cmd.ExecuteScalar() as string;

                if (storedHash != null && BCrypt.Net.BCrypt.Verify(loginPassword, storedHash))
                {
                    // ← added: store username in session so the navbar knows they're logged in
                    HttpContext.Session.SetString("UserName", loginUsername);
                    return RedirectToPage("Index");
                }
                else
                {
                    message = "Incorrect username or password";
                    return Page();
                }
            }
        }
    }
}