using Jof.DAL;
using Jof.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
	opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
	opt.Lockout.MaxFailedAccessAttempts = 10;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
