using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TrainApp.Pages
{
    public class AccountModel : PageModel
    {
        public string userName { get; set; }
        public string userEmail { get; set; }
        public int userAge { get; set; }
        public string userPhone { get; set; }

        // favourite stations list
        public List<string> favouriteStations { get; set; } = new List<string>();

        private string connectionString = "Data Source=TrainApp.db";

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("UserName");

            if (username == null)
            {
                return RedirectToPage("/Login");
            }

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                // Get user info
                var cmd = new SqliteCommand(
                    @"SELECT userName, userEmail, userAge, userPhone 
                      FROM userInfo 
                      WHERE userName = @name", conn);

                cmd.Parameters.AddWithValue("@name", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userName = reader["userName"].ToString();
                        userEmail = reader["userEmail"].ToString();
                        userAge = reader["userAge"] != DBNull.Value ? Convert.ToInt32(reader["userAge"]) : 0;
                        userPhone = reader["userPhone"].ToString();
                    }
                }

                // Load favourite stations
                var favCmd = new SqliteCommand(
                    "SELECT stationName FROM favouriteStations WHERE userName = @name", conn);

                favCmd.Parameters.AddWithValue("@name", username);

                using (var reader = favCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        favouriteStations.Add(reader["stationName"].ToString());
                    }
                }
            }

            return Page();
        }

        public IActionResult OnPostAddFavourite(string stationName)
        {
            var username = HttpContext.Session.GetString("UserName");

            if (username == null)
                return RedirectToPage("/Login");

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqliteCommand(
                    "INSERT INTO favouriteStations (userName, stationName) VALUES (@user, @station)", conn);

                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@station", stationName);

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostRemoveFavourite(string stationName)
        {
            var username = HttpContext.Session.GetString("UserName");

            if (username == null)
                return RedirectToPage("/Login");

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqliteCommand(
                    "DELETE FROM favouriteStations WHERE userName = @user AND stationName = @station", conn);

                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@station", stationName);

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage();
        }

        // Returns favourite stations as JSON for the navbar dropdown
        public IActionResult OnGetFavourites()
        {
            var username = HttpContext.Session.GetString("UserName");

            if (username == null)
                return new JsonResult(new { loggedIn = false });

            var stations = new List<string>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SqliteCommand(
                    "SELECT stationName FROM favouriteStations WHERE userName = @name", conn);

                cmd.Parameters.AddWithValue("@name", username);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        stations.Add(reader["stationName"].ToString());
                }
            }

            return new JsonResult(new { loggedIn = true, stations });
        }
    }
}