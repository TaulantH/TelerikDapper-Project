using System.Data;
using Dapper;
using LoginRegisterRoles_TelerikDapper.Models;
using System.Collections.Generic;

public class NewsRepository
{
	private readonly IDbConnection _connection;

	public NewsRepository(IDbConnection connection)
	{
		_connection = connection;
	}


	public void AddNews(string title, string content, DateTime publishDate, string location, string images)
	{
		try
		{
			var query = @"INSERT INTO News (Title, Content, PublishDate, Location, Images) 
                      VALUES (@Title, @Content, @PublishDate, @Location, @Images)";

			var parameters = new
			{
				Title = title,
				Content = content,
				PublishDate = publishDate,
				Location = location,
				Images = images ?? string.Empty // Ensure Images is not null
			};

			_connection.Execute(query, parameters);

			Console.WriteLine("News added successfully.");
		}
		catch (Exception ex)
		{
			// Log exception details for debugging
			Console.WriteLine($"Error adding news: {ex.Message}");
			throw; // Re-throw exception for higher-level handling
		}
	}

	public IEnumerable<News> GetNews()
	{
		string query = "SELECT * FROM News order by Id desc";
		return _connection.Query<News>(query);
	}

	public IEnumerable<News> FindById( int Id)
	{
		string query = $"SELECT * FROM News where Id={Id}";
		return _connection.Query<News>(query);
	}
	public void UpdateNews(News news)
	{
		var query = "UPDATE News SET Title = @Title, Content = @Content, PublishDate = @PublishDate, Location = @Location, Images = @Images WHERE Id = @Id";
		_connection.Execute(query, news);
	}

	public void DeleteNews(int id)
	{
		var query = "DELETE FROM News WHERE Id = @Id";
		_connection.Execute(query, new { Id = id });
	}

}