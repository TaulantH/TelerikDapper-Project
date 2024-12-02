using System.Data;
using Microsoft.Data.SqlClient;

namespace LoginRegisterRoles_TelerikDapper.Data
{
	public class DapperContext
	{
		private readonly string _connectionString;

		// Constructor to initialize the connection string
		public DapperContext(string connectionString)
		{
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
		}

		// Method to create and return a new database connection
		public IDbConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}
