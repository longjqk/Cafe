using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCafe.Models;
using QLCafe.ViewModels;

namespace QLCafe.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(model); 
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {

                // Lấy user hiện tại
                var user = await _userManager.FindByNameAsync(model.UserName);

                // Kiểm tra role của user
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    // Nếu là Admin, chuyển đến trang Admin
                    return RedirectToAction("Index", "Drinks");
                }
                else if (await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    // Nếu là Customer, chuyển đến trang Customer
                    return RedirectToAction("IndexDrinkCustomer", "DrinksCustomer");
                }
                else
                {
                    await _signInManager.SignOutAsync();
                    ModelState.AddModelError(string.Empty, "Tài khoản không có quyền truy cập.");
                    return View("Login", model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sai mật khẩu hoặc tên đăng nhập rồi");
            }
            return View(model);
        }


        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber, 
                    Address = model.Address
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");

                    TempData["SuccessMessage"] = "Đăng ký thành công";
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
