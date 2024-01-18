using Jof.Helpers;
using Jof.Models;
using Jof.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jof.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }
            if(loginVm is null)
            {
                ModelState.AddModelError(string.Empty,"Error");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(loginVm.UsernameOrEmail) ?? await _userManager.FindByNameAsync(loginVm.UsernameOrEmail);
            if(user is null) throw new Exception("Username/Email or Password incorrect");
            var result = await _signInManager.CheckPasswordSignInAsync(user,loginVm.Password,true);
            if (!result.Succeeded) throw new Exception("Username/Email or Password incorrect");

            await _signInManager.SignInAsync(user, true);

            return RedirectToAction(nameof(Index),"Home");
        }
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVm);
            }
            if(registerVm is null)
            {
                ModelState.AddModelError(string.Empty, "Error");
                return View();
            }
            AppUser user = new AppUser()
            {
                Name=registerVm.Name,
                Surname=registerVm.Surname,
                Email=registerVm.Email,
                UserName=registerVm.UserName,
            };
            AppUser checkEmail = await _userManager.FindByEmailAsync(user.Email);
            if(checkEmail != null) throw new Exception("Bu email qeydiyyatdan kecib");
            AppUser checkUserName = await _userManager.FindByNameAsync(user.UserName);
            if (checkUserName != null) throw new Exception("Bu UserName'li Hesab var");
            var result = await _userManager.CreateAsync(user,registerVm.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                }
            }
            //await _userManager.AddToRoleAsync(user,UserRole.Admin.ToString());
            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());
            return RedirectToAction("Login","Account");
        }
        public async Task<IActionResult> LogOut()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            foreach (UserRole item in Enum.GetValues(typeof(UserRole)))
            {
                if(await _roleManager.FindByNameAsync(item.ToString()) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString(),
                    });
                }
            }
            return RedirectToAction("Index","Home");
        }

    }
}
