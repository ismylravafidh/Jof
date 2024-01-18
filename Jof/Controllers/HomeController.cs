using Jof.DAL;
using Jof.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jof.Controllers
{
	public class HomeController : Controller
	{
		AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
		{
			List<Fruit> fruits = await _context.Fruits.Where(f=>f.IsDeleted==false).ToListAsync();
			return View(fruits);
		}
	}
}
