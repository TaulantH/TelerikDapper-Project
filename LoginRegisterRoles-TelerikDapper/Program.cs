using LoginRegisterRoles_TelerikDapper.Repository;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using LoginRegisterRoles_TelerikDapper;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Data.SqlClient;
using LoginRegisterRoles_TelerikDapper.Areas.Admin.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

// Add connection string for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
	new SqlConnection(builder.Configuration.GetConnectionString("LoginRegisterRole_TelerikDapperDb")));


builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); 
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/User/Login";
		options.AccessDeniedPath = "/Home/AccessDenied";
		options.ExpireTimeSpan = TimeSpan.FromMinutes(300);
	});


builder.Services.AddAuthorization(options =>
{
});

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<NewsRepository>();
builder.Services.AddScoped<FAQRepository>();


builder.Services.AddKendo();

var app = builder.Build();

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
app.UseAuthentication();  
app.UseAuthorization();  

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
