using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using TrainApp.Models;

namespace TrainApp.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User newUser { get; set; }

        [BindProperty]
        public string loginUsername { get; set; }

        [BindProperty]
        public string loginPassword { get; set; }

        public string message { get; set; }

        private string connectionString = "Data Source=TrainApp.db";

        public void OnGet() { }

        public IActionResult OnPostRegister()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqliteCommand(
                    @"INSERT INTO userInfo 
                      (userName, userPass, userAge, userEmail, userPhone)
                      VALUES (@name, @pass, @age, @email, @phone)", conn);

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.userPass);

                cmd.Parameters.AddWithValue("@name", newUser.userName);
                cmd.Parameters.AddWithValue("@pass", hashedPassword);
                cmd.Parameters.AddWithValue("@age", newUser.userAge);
                cmd.Parameters.AddWithValue("@email", newUser.userEmail ?? "");
                cmd.Parameters.AddWithValue("@phone", newUser.userPhone ?? "");

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPostLogin()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqliteCommand(
                    "SELECT userPass FROM userInfo WHERE userName = @name", conn);

                cmd.Parameters.AddWithValue("@name", loginUsername);

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string storedHash = result.ToString();

                    if (BCrypt.Net.BCrypt.Verify(loginPassword, storedHash))
                    {
                        HttpContext.Session.SetString("UserName", loginUsername);
                        return RedirectToPage("Index");
                    }
                }

                message = "Invalid login";
                return Page();
            }
        }
    }
}