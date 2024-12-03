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
			var users = _adminRepository.GetAllUsers();
			return Json(users.ToDataSourceResult(request));
		}

		public IActionResult ManageUsers()
		{
			var users = _adminRepository.GetAllUsers();
			return View(users); // Pass users to view
		}

		[HttpPost]
		public IActionResult Update([DataSourceRequest] DataSourceRequest request, User user)
		{
			if (user != null)
			{
				try
				{
					_adminRepository.Update(user);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", $"Error updating user: {ex.Message}");
				}
			}
			return Json(new[] { user }.ToDataSourceResult(request));
		}

		[HttpPost]
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