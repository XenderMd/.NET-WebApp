using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _singInManager;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> singInManager)
        {
            _logger = logger;
            _singInManager = singInManager;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _singInManager.PasswordSignInAsync(model.Username,
                                                                model.Password,
                                                                model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Shop", "App");
                }
            }

            ModelState.AddModelError("", "Failed to login");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }
    }
}
