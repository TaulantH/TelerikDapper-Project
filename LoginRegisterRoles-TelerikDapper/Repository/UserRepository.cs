using Dapper;
using LoginRegisterRoles_TelerikDapper.Models;
using LoginRegisterRoles_TelerikDapper.Repository.IRepository;
using LoginRegisterRoles_TelerikDapper.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class UserRepository : IUserRepository
{
	private readonly IDbConnection _connection;
	private readonly ILogger<UserRepository> _logger;

	public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
	{
		_connection = new SqlConnection(configuration.GetConnectionString("LoginRegisterRole_TelerikDapperDb"));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	// Insert a new user
	public async Task<bool> Insert(User user)
	{
		try
		{
			// Hash password before inserting
			user.Password = PasswordHelper.HashPassword(user.Password);


			// Set RoleId if not specified (default to 2 for "User")
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
			// Log the exception instead of just printing to the console
			_logger.LogError($"Error inserting user: {ex.Message}");
			return false;
		}
	}

	// Login a user
	public async Task<User> Login(string email, string password)
	{
		try
		{
			var query = @"
                SELECT u.*, r.RoleName AS Role
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.RoleId
                WHERE u.Email = @Email";

			var user = await _connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });

			if (user != null)
			{
				// Verify the password
				bool passwordValid = PasswordHelper.VerifyPassword(password, user.Password);

				if (passwordValid)
				{
					return user;
				}
			}

			return null;
		}
		catch (Exception ex)
		{
			// Log the exception instead of just printing to the console
			_logger.LogError($"Error during login: {ex.Message}");
			return null;
		}
	}
}
