using LoginRegisterRoles_TelerikDapper.Repository;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using LoginRegisterRoles_TelerikDapper;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

// Add connection string for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
	new SqlConnection(builder.Configuration.GetConnectionString("LoginRegisterRole_TelerikDapperDb")));


// Add session services
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/User/Login"; // Redirect if not authenticated
		options.AccessDeniedPath = "/Home/AccessDenied"; // Redirect if not authorized
		options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expiration
	});


builder.Services.AddAuthorization(options =>
{
	// Define roles or policies if needed
});

// Add the repository services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AdminRepository>();

// Add Kendo UI for Telerik
builder.Services.AddKendo();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Ensure session is before authentication
app.UseSession();
app.UseAuthentication();  // Authentication middleware
app.UseAuthorization();  // Authorization middleware

// Configure routes
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
