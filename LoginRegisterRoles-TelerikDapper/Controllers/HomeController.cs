using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Http;

public class HomeController : Controller
{
	private readonly IDbConnection _dbConnection;

	// Injecting the database connection through constructor
	public HomeController(IDbConnection dbConnection)
	{
		_dbConnection = dbConnection;
	}

	public IActionResult Index()
	{
		// Fetch the user's role dynamically
		string userRole = GetUserRole();

		// Pass the role information to the view
		ViewData["UserRole"] = userRole;

		return View();
	}
	public IActionResult About()
	{


		return View();
	}
	public IActionResult Contact()
	{


		return View();
	}


	private string GetUserRole()
	{
		try
		{
			// Get the logged-in user's ID from the session or authentication
			var userId = HttpContext.Session.GetString("UserId");

			// Check if userId exists, if not return a default or error message
			if (string.IsNullOrEmpty(userId))
			{
				return "No user logged in.";
			}

			// Modified query to get the role name directly from Users and Roles
			string query = @"
            SELECT r.RoleName
            FROM Users u
            INNER JOIN Roles r ON u.RoleId = r.RoleId
            WHERE u.UserId = @UserId";

			// Execute query to get the role name
			var role = _dbConnection.QuerySingleOrDefault<string>(query, new { UserId = userId });

			return role ?? "No role assigned."; // Return a default message if no role is found
		}
		catch (Exception ex)
		{
			// Log the exception or handle it accordingly
			return $"Error fetching role: {ex.Message}";
		}
	}
}
