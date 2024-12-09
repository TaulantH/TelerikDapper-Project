using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LoginRegisterRoles_TelerikDapper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace LoginRegisterRoles_TelerikDapper
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	public class NewsController : Controller
	{
		private readonly NewsRepository _repository;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public NewsController(NewsRepository repository, IWebHostEnvironment webHostEnvironment)
		{
			_repository = repository;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetAll([DataSourceRequest] DataSourceRequest request)
		{
			var news = _repository.GetNews(); 
			return Json(news.ToDataSourceResult(request));
		}

		[HttpPost]
		public JsonResult CreateUpdateNews(News model)
		{
			Console.WriteLine(model.Images);
			if (model.Id > 0)
			{
				if (ModelState.IsValid)
				{
					model.Content = HttpUtility.HtmlDecode(model.Content ?? string.Empty);
					_repository.UpdateNews(model);
					return Json(new { success = true });
				}
				return Json(new { success = false });
			}
			else
			{
				if (ModelState.IsValid)
				{
					var images = model.Images;
					model.Content = HttpUtility.HtmlDecode(model.Content ?? string.Empty);
					_repository.AddNews(model.Title, model.Content, model.PublishDate, model.Location, images);

					return Json(new { success = true });
				}

				var errors = ModelState.Values
					.Where(v => v.Errors.Any())
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();
				return Json(new { success = false, message = "Validation failed", errors });
			}
		}


		[HttpPost]
		public async Task<ActionResult> SaveFile(IFormFile file)
		{
			if (file?.Length > 0)
			{
				var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
				if (!Directory.Exists(uploads))
				{
					Directory.CreateDirectory(uploads);
				}

				string guid = Guid.NewGuid().ToString();
				var filename = DateTime.Now.Year + "_" + guid + Path.GetExtension(file.FileName);
				var filePath = Path.Combine(uploads, filename);

				using (var stream = System.IO.File.Create(filePath))
				{
					await file.CopyToAsync(stream);
				}

				// Return the relative file path for use in the client-side script
				return Json(new { FileUrl = Path.Combine("/Images", filename).Replace("\\", "/") });
			}

			return Json(new { FileUrl = "" });
		}




		public IActionResult AddEditNews( int newsId)
		{
			if (newsId > 0)
			{
				var result =  _repository.FindById(newsId).FirstOrDefault();
			
				if (result == null)
					return Json(new { HasError = true });
				return View(result);
			}
			
			var newsModel = new News();
			return View(newsModel);
		}

		[HttpPost]
		public JsonResult Update(News model)
		{
			if (ModelState.IsValid)
			{
				_repository.UpdateNews(model);
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			_repository.DeleteNews(id);
			return Json(new { success = true });
		}
	}
}
