using Jof.Areas.Manage.ViewModels;
using Jof.DAL;
using Jof.Helpers;
using Jof.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jof.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class FruitController : Controller
    {
        AppDbContext _context;
        IWebHostEnvironment _env;
        public FruitController(AppDbContext context, IWebHostEnvironment env )
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Fruit> fruits = await _context.Fruits.Where(f=>f.IsDeleted==false).ToListAsync();
            return View(fruits);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FruitCreateVm createVm)
        {
            if (!ModelState.IsValid)
            {
                return View(createVm);
            }
            if(createVm is null)
            {
                ModelState.AddModelError(string.Empty, "Fruit Bos Ola Bilmez");
                return View();
            }

            if (!createVm.Image.CheckImage(3))
            {
                ModelState.AddModelError(string.Empty, "Yalniz Sekil Daxil ede bilersiz, Max olcu 3mb");
                return View();
            }
            Fruit fruit = new Fruit()
            {
                Name = createVm.Name,
                Description = createVm.Description,
                ImgUrl =await createVm.Image.UploadImage(_env.WebRootPath, @"\Upload\FruitImage\")
            };

            await _context.Fruits.AddAsync(fruit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            Fruit fruit =await _context.Fruits.Where(f=>f.IsDeleted==false).FirstOrDefaultAsync(f=>f.Id==id);
            FruitUpdateVm fruitU = new FruitUpdateVm()
            {
                Name = fruit.Name,
                Description = fruit.Description,
                ImgUrl = fruit.ImgUrl
            };
            return View(fruitU);
        }
        [HttpPost]
        public async Task<IActionResult> Update(FruitUpdateVm updateVm)
        {
            if (!ModelState.IsValid)
            {
                return View(updateVm);
            }
            if(updateVm == null)
            {
                ModelState.AddModelError(string.Empty, "Fruit Null ola bilmez");
                return View();
            }
            Fruit oldFruit = await _context.Fruits.Where(f => f.IsDeleted == false).FirstOrDefaultAsync(f => f.Id == updateVm.Id);
            oldFruit.Name= updateVm.Name;
            oldFruit.Description= updateVm.Description;
            if(updateVm.Image!=null)
            {
                oldFruit.ImgUrl = await updateVm.Image.UploadImage(_env.WebRootPath, @"\Upload\FruitImage\");
            }

            _context.Update(oldFruit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) throw new Exception("Id 0 ve menfi olammaz");
            Fruit fruit = await _context.Fruits.Where(f => f.Id == id).FirstOrDefaultAsync();
            fruit.IsDeleted = true;
            _context.Update(fruit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
