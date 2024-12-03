using Microsoft.AspNetCore.Mvc;
using LoginRegisterRoles_TelerikDapper.Repository;
using LoginRegisterRoles_TelerikDapper.Utilities;
using LoginRegisterRoles_TelerikDapper.Models;

namespace LoginRegisterRoles_TelerikDapper.Controllers
{
	public class FormController : Controller
	{
		private readonly UserRepository _userRepository;

		// Constructor to inject UserRepository
		public FormController(UserRepository userRepository)
		{
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
		}

		// GET: Form/Validation
		public IActionResult Validation()
		{
			return View(new User()
			{
				FirstName = "",
				LastName = "",
				Email = "",
				UserName = "",
				Password = "",
				ConfirmPassword = "",
				DateOfBirth = new DateTime(2002, 5, 8),
				Agree = true,
				RoleId = 2
			});
		}

		// POST: Form/Validation
		[HttpPost]
		public async Task<IActionResult> Validation(User user)
		{
			if (ModelState.IsValid)
			{
				// Check that password and confirm password match
				if (user.Password != user.ConfirmPassword)
				{
					ModelState.AddModelError("", "Password and Confirm Password must match.");
					return View(user);
				}

				// Hash the password before inserting
				var isInserted = await _userRepository.Insert(user);

				if (isInserted)
				{
					// Redirect to Dashboard after successful registration
					return RedirectToAction("Login", "User");
				}
				else
				{
					ModelState.AddModelError("", "Failed to register user. Please try again.");
					return View(user);
				}
			}

			// If model is invalid, return the form with validation errors
			return View(user);
		}

	}

}
