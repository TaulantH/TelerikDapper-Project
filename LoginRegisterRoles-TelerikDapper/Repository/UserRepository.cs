using Dapper;
using LoginRegisterRoles_TelerikDapper.Models;
using LoginRegisterRoles_TelerikDapper.Repository.IRepository;
using LoginRegisterRoles_TelerikDapper.Utilities;
using System.Data;
using System.Data.SqlClient;

public class UserRepository : IUserRepository
{
	private readonly IDbConnection _connection;

	public UserRepository(IConfiguration configuration)
	{
		_connection = new SqlConnection(configuration.GetConnectionString("LoginRegisterRole_TelerikDapperDb"));
	}

	public async Task<bool> Insert(User user)
	{
		try
		{
			// Check if the email already exists
			var emailExistsQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
			var emailExists = await _connection.QuerySingleAsync<bool>(emailExistsQuery, new { Email = user.Email });

			if (emailExists)
			{
				throw new Exception("Email already exists. Please use a different email address.");
			}

			// Hash the password before inserting it into the database
			user.Password = PasswordHelper.HashPassword(user.Password); // Ensure password hashing

			// Set RoleId to 1 (default "User" role) if not specified
			if (user.RoleId == 0) user.RoleId = 2;

			var query = @"
            INSERT INTO Users (FirstName, LastName, UserName, Password, Email, DateOfBirth, RoleId)
            VALUES (@FirstName, @LastName, @UserName, @Password, @Email, @DateOfBirth, @RoleId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

			var userId = await _connection.QuerySingleAsync<int>(query, user);

			return userId > 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error inserting user: {ex.Message}");
			return false;
		}
	}
	public async Task<User> Login(string email, string password)
	{
		try
		{
			var query = @"
        SELECT u.*, r.RoleName AS Role
        FROM Users u
        INNER JOIN Roles r ON u.RoleId = r.RoleId
        WHERE u.Email = @Email"; // Use email for login

			var user = await _connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });

			if (user != null)
			{
				// Verify the hashed password using the password helper (e.g., BCrypt or SHA256)
				bool passwordValid = PasswordHelper.VerifyPassword(password, user.Password); // Compare the plain-text password with the stored hashed password

				if (passwordValid)
				{
					return user; // Credentials are valid
				}
			}

			return null; // Invalid credentials
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error during login: {ex.Message}");
			return null;
		}
	}

}
