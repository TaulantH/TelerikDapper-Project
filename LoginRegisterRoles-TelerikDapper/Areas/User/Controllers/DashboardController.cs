using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterRoles_TelerikDapper.Areas.User.Controllers
{
	[Authorize(Roles = "User")]
	[Area("User")]
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}



}
