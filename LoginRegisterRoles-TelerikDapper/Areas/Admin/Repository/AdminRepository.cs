using LoginRegisterRoles_TelerikDapper.Models;
using Dapper;
using System.Data;
using LoginRegisterRoles_TelerikDapper;
using System.Numerics;

namespace LoginRegisterRoles_TelerikDapper
{
	public class AdminRepository : IAdminRepository
	{
		private readonly IDbConnection _connection;

		public AdminRepository(IDbConnection connection)
		{
			_connection = connection;
		}
		public User Find(int id)
		{
			var sql = "SELECT * FROM Users WHERE UserId = @UserId";  // Correct parameter name
			return _connection.Query<User>(sql, new { UserId = id }).Single();  // Corrected parameter name
		}
		public IEnumerable<User> GetAllUsers()
		{
			var query = "SELECT u.FirstName, u.LastName, u.Username, u.Email, r.RoleName FROM Users u INNER JOIN Roles r ON u.RoleId = r.RoleId;";
			return  _connection.Query<User>(query).ToList();
		}


		public User Add(User user)
		{
			var sql = "INSERT INTO Users (Name, Email, Username, Password) VALUES (@Name, @Email, @Username, @Password);";
			var id = _connection.Query<int>(sql, user).Single();
			user.UserId = id;
			return user;
		}
		public User Update(User user)
		{
			var sql = "UPDATE Users SET Name = @Name, Email = @Email, Username = @Username WHERE UserId = @UserId";
			var affectedRows = _connection.Execute(sql, user);
			if (affectedRows == 0)
			{
				throw new Exception("No rows updated. The user might not exist.");
			}
			return user;
		}

		public void Remove(int id)
		{
			var sql = "DELETE FROM Users WHERE UserId = @id";
			var affectedRows = _connection.Execute(sql, new { id });
			if (affectedRows == 0)
			{
				throw new Exception("No rows deleted. The user might not exist.");
			}
		}

		public Task<int> GetByIdAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public Task SaveChangesAsync()
		{
			throw new NotImplementedException();
		}
	}
}
