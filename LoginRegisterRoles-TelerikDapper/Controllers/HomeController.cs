using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Http;
using Kendo.Mvc.UI;
using NuGet.Protocol.Core.Types;
using LoginRegisterRoles_TelerikDapper.Repository;
using Kendo.Mvc.Extensions;
using LoginRegisterRoles_TelerikDapper.Areas.Admin.Repository;
using LoginRegisterRoles_TelerikDapper.Models.ViewModel;

public class HomeController : Controller
{
	private readonly IDbConnection _dbConnection;
	private readonly NewsRepository _repository;
	private readonly FAQRepository _faqRepository;


	// Injecting the database connection through constructor
	public HomeController(IDbConnection dbConnection, NewsRepository repository, FAQRepository faQRepository)
	{

	_dbConnection = dbConnection;
		_repository = repository;
		_faqRepository = faQRepository;
	}

	public IActionResult Index()
	{
		string userRole = GetUserRole();
		var news = _repository.GetNews();
		var faqs = _faqRepository.GetFaq();

		var model = new FaqNewsModel()
		{
			News = news,
			FAQs = faqs
		};

		ViewData["UserRole"] = userRole;
		return View(model); 
	}


	public IActionResult GetAll([DataSourceRequest] DataSourceRequest request)
	{
		var news = _repository.GetNews();
				var faqs = _faqRepository.GetFaq();

		return Json(news.ToDataSourceResult(request));
	}

	public IActionResult About()
	{


		return View();
	}
	public IActionResult Contact()
	{


		return View();
	}

	public IActionResult Details(int id)
	{
		var newsItem = _repository.FindById(id).FirstOrDefault();
		if (newsItem == null)
		{
			return NotFound();
		}

		var otherNews = _repository.GetNews().Where(n => n.Id != id).Take(5);

		ViewBag.OtherNews = otherNews;

		return View(newsItem);
	}



	private string GetUserRole()
	{
		try
		{
			var userId = HttpContext.Session.GetString("UserId");

			if (string.IsNullOrEmpty(userId))
			{
				return "No user logged in.";
			}

			string query = @"
            SELECT r.RoleName
            FROM Users u
            INNER JOIN Roles r ON u.RoleId = r.RoleId
            WHERE u.UserId = @UserId";

			var role = _dbConnection.QuerySingleOrDefault<string>(query, new { UserId = userId });

			return role ?? "No role assigned.";
		}
		catch (Exception ex)
		{
			return $"Error fetching role: {ex.Message}";
		}
	}
}
