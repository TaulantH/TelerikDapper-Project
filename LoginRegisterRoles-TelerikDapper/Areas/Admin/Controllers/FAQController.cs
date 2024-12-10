using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LoginRegisterRoles_TelerikDapper.Areas.Admin.Repository;
using LoginRegisterRoles_TelerikDapper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;

namespace LoginRegisterRoles_TelerikDapper
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	public class FAQController : Controller
	{
		private readonly FAQRepository _repository;

		public FAQController(FAQRepository repository)
		{
			_repository = repository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetAllFaq([DataSourceRequest] DataSourceRequest request)
		{
			var faqs = _repository.GetFaq();
			var result = faqs.ToDataSourceResult(request);
			return Json(result);
		}

		[HttpPost]
		public JsonResult CreateUpdateFaq(FAQ model)
		{
			if (ModelState.IsValid)
			{
				if (model.Id > 0) // Update existing record
				{
					_repository.UpdateFaq(model);
				}
				else // Create new record
				{
					_repository.AddFaq(model.Question, model.Answer);
				}
				return Json(new { success = true });
			}

			return Json(new
			{
				success = false,
				message = "Validation failed",
				errors = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList()
			});
		}


		public IActionResult AddEditFaq(int newsId)
		{
			if (newsId > 0)
			{
				var result = _repository.FindById(newsId).FirstOrDefault();

				if (result == null)
					return Json(new { HasError = true });
				return View(result);
			}

			var newsModel = new News();
			return View(newsModel);
		}

		[HttpPost]
		public JsonResult Update(FAQ model)
		{
			if (ModelState.IsValid)
			{
				_repository.UpdateFaq(model);
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			_repository.DeleteFaq(id);
			return Json(new { success = true });
		}
	}

}