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

        public void OnGet() 
        {
        }

        public IActionResult OnPostRegister()
        {
            string connectionStringUser = "Server=trainserver.database.windows.net;Initial Catalog=Users;User ID=CT855;Password=TrainPredicPass123;Encrypt=True;";

            using (SqlConnection conn = new SqlConnection(connectionStringUser))
            {
                conn.Open();

                string query = @"INSERT INTO userInfo (userName, userPass, userAge, userEmail, userPhone)
                                 VALUES (@newUsername, @newPassword, @newAge, @newEmail, @newPhone)";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Hash password once
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.userPass);

                // Add parameters using the correct property names
                cmd.Parameters.AddWithValue("@newUsername", newUser.userName);
                cmd.Parameters.AddWithValue("@newPassword", hashedPassword);
                cmd.Parameters.AddWithValue("@newAge", newUser.userAge);
                cmd.Parameters.AddWithValue("@newEmail", newUser.userEmail);
                cmd.Parameters.AddWithValue("@newPhone", newUser.userPhone);

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("Index");
        }
    }
}