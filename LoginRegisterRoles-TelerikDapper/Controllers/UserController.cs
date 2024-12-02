using LoginRegisterRoles_TelerikDapper.Models;
using LoginRegisterRoles_TelerikDapper.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginRegisterRoles_TelerikDapper.Controllers
{
	public class UserController : Controller
	{
		private readonly UserRepository _userRepository;

		public UserController(UserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		// GET: Register page
		[HttpGet]
		public IActionResult Register()
		{
			return View(new User());
		}

		// POST: Register user
		[HttpPost]
		public async Task<IActionResult> Register(User user)
		{
			if (ModelState.IsValid)
			{
				// Hash the password using bcrypt before saving it to the database
				user.Password = PasswordHelper.HashPassword(user.Password);

				var isRegistered = await _userRepository.Insert(user);

				if (isRegistered)
				{
					// Store user info in session
					HttpContext.Session.SetString("UserId", user.UserId.ToString());
					HttpContext.Session.SetString("RoleId", user.RoleId.ToString());

					// Redirect to the dashboard based on role
					if (user.RoleId == 1)
					{
						return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
					}
					else
					{
						return RedirectToAction("Index", "Dashboard", new { area = "User" });
					}
				}
				else
				{
					ModelState.AddModelError("", "Failed to register user. Please try again.");
					return View(user);
				}
			}

			return View(user);
		}


		// GET: Login page
		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View(new Login()); 
		}

		// POST: Handle login logic
		[HttpPost]
		public async Task<IActionResult> Login(Login model, string returnUrl = null)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// Attempt login
			var user = await _userRepository.Login(model.Email, model.Password);
			if (user != null)
			{
				bool isPasswordCorrect = PasswordHelper.VerifyPassword(model.Password, user.Password);

				var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.RoleId == 1 ? "Admin" : "User") // Assign "Admin" or "User" based on RoleId
        };

				// Create the identity and principal
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var authProperties = new AuthenticationProperties
				{
					IsPersistent = true, 
				};

				// Sign in the user
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

				// Redirect based on returnUrl or role
				if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}
				return RedirectToAction("Index", "Dashboard", new { area = user.RoleId == 1 ? "Admin" : "User" });
			}

			// If login fails
			ModelState.AddModelError("", "Invalid login credentials.");
			return View(model);
		}


		// GET: Logout
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			// Sign out from cookie authentication
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// Clear session data
			HttpContext.Session.Clear();

			// Redirect to login page
			return RedirectToAction("Login");
		}

	}
}
