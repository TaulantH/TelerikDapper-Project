using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using LoginRegisterRoles_TelerikDapper.Models;
using Microsoft.Exchange.WebServices.Data;

namespace LoginRegisterRoles_TelerikDapper
{ 
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	public class DashboardController : Controller
	{
		private readonly AdminRepository _adminRepository;

		// Constructor to inject AdminRepository
		public DashboardController(AdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetAllUsers([DataSourceRequest] DataSourceRequest request)
		{
			// Fetch the users from the repository
			var users =  _adminRepository.GetAllUsers();
			// Return the data in the required format
			return Json(users.ToDataSourceResult(request));
		}

		public IActionResult ManageUsers()
		{
			var users = _adminRepository.GetAllUsers();
			return View(users); // Pass users to view
		}

		[HttpPost]
		public IActionResult Create([DataSourceRequest] DataSourceRequest request, User user)
		{
			if (user != null && ModelState.IsValid)
			{
				// Call the repository to insert the user
				var result = _adminRepository.Add(user);

			}

			return Json(new[] { user }.ToDataSourceResult(request, ModelState));
		}


		[HttpPost]
		public IActionResult Update([DataSourceRequest] DataSourceRequest request, User user)
		{
			if (user != null && ModelState.IsValid)
			{
				_adminRepository.Update(user);
			}
			return Json(new[] { user }.ToDataSourceResult(request, ModelState));
		}
		[HttpGet]
		public IActionResult Delete([DataSourceRequest] DataSourceRequest request, User user)
		{
			if (user != null)
			{
				_adminRepository.Remove(user.UserId);
			}

			return Json(new[] { user }.ToDataSourceResult(request, ModelState));
		}

	}
}
