using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using IdentityServer4.Test;
using IdentityServer4.Services;
using Admin.Models;

namespace Admin.Controllers
{
    public class AccountController : Controller
    {
        //private readonly TestUserStore _testUserStore;

        //public AccountController(TestUserStore testUserStore)
        //{
        //    _testUserStore = testUserStore;
        //}

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IIdentityServerInteractionService _identityServer;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService identityServer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityServer = identityServer;
        }
        private IActionResult RedirectToLoacl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        private void AddErrors(IdentityResult identityResult)
        {
            if (identityResult != null && identityResult.Errors.Count() > 0)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
        }
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel register, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["returnUrl"] = returnUrl;
                var user = await _userManager.FindByEmailAsync(register.Email);// 查找用户
                if (user == null)
                {
                    ModelState.AddModelError(nameof(register.Email), "用户不存在！");
                }
                else
                {
                    if (await _userManager.CheckPasswordAsync(user, register.Password))// 验证用户和密码
                    {
                        AuthenticationProperties props = null;
                        if (register.RememBerMe)
                        {
                            props = new AuthenticationProperties()
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                            };
                        }
                        await _signInManager.SignInAsync(user, props);
                        if (_identityServer.IsValidReturnUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return Redirect("~/");
                        //return RedirectToLoacl(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(register.Password), "密码错误！");
                    }
                }
            }
            return View();
        }
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["returnUrl"] = returnUrl;
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
                    return RedirectToLoacl(returnUrl);
                }
                else
                {
                    AddErrors(identityResult);
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            // HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}