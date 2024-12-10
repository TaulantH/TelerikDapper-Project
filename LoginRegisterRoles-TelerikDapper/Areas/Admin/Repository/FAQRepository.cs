using Dapper;
using LoginRegisterRoles_TelerikDapper.Models;
using System.Data;

namespace LoginRegisterRoles_TelerikDapper.Areas.Admin.Repository
{
	public class FAQRepository
	{
		private readonly IDbConnection _connection;

		public FAQRepository(IDbConnection connection)
		{
			_connection = connection;
		}

		public void AddFaq(string question, string answer)
		{
			try
			{
				var query = @"INSERT INTO FAQs (Question, Answer) 
                      VALUES (@Question, @Answer)";

				var parameters = new
				{
					Question = question,
					Answer = answer,
				};

				_connection.Execute(query, parameters);

				Console.WriteLine("FAQs added successfully.");
			}
			catch (Exception ex)
			{
				// Log exception details for debugging
				Console.WriteLine($"Error adding news: {ex.Message}");
				throw; // Re-throw exception for higher-level handling
			}
		}

		public IEnumerable<FAQ> GetFaq()
		{
			string query = "SELECT * FROM FAQs";
			return _connection.Query<FAQ>(query);
		}

		public IEnumerable<FAQ> FindById(int Id)
		{
			string query = $"SELECT * FROM FAQs where Id={Id}";
			return _connection.Query<FAQ>(query);
		}
		public void UpdateFaq(FAQ faq)
		{
			var query = "UPDATE FAQs SET Question = @Question, Answer = @Answer WHERE Id = @Id";
			_connection.Execute(query, faq);
		}

		public void DeleteFaq(int id)
		{
			var query = "DELETE FROM FAQs WHERE Id = @Id";
			_connection.Execute(query, new { Id = id });
		}

	}
}
