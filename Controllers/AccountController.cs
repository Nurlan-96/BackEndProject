using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Helper;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {   if(!ModelState.IsValid) return View(registerVM);
            AppUser user = new()
            {
                FullName = registerVM.UserName,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
            };
            IdentityResult result=await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(user, nameof(RolesEnum.Member));
            return RedirectToAction("Register");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Incorrect Username or Email");
                    return View(loginVM);
                }
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "You are locked out");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect Username or Email");
                return View(loginVM);
            }
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> AddRole()
        {
            if (!await _roleManager.RoleExistsAsync("admin")) 
                await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            if (!await _roleManager.RoleExistsAsync("member")) 
                    await _roleManager.CreateAsync(new IdentityRole { Name = "member" });            
            if (!await _roleManager.RoleExistsAsync("superadmin")) 
                    await _roleManager.CreateAsync(new IdentityRole { Name = "superadmin" });
            return Content("roles added");

        }
    }
}
