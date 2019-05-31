using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Admin.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(RegisterViewModel register)
        {
            var user = await _userManager.FindByEmailAsync(register.Email);
            if (user == null)
            {

            }
            await _signInManager.SignInAsync(user, new AuthenticationProperties { IsPersistent = true });
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            var identityUser = new ApplicationUser
            {
                Email = register.Email,
                UserName = register.Email,
                NormalizedUserName = register.Email,
            };
            var identityResult = await _userManager.CreateAsync(identityUser, register.Password);
            if (identityResult.Succeeded)
            {
                await _signInManager.SignInAsync(identityUser, new AuthenticationProperties { IsPersistent = true });
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}