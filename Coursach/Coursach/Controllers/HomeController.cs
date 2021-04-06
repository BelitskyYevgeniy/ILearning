using Coursach.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Coursach.Services.Design;
using System;
using Coursach.Interfaces;

namespace Coursach.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> _userManager;
        private ApplicationContext _appContext;
        private IDesignChooser _designChooser;
        public HomeController(ApplicationContext appContext, UserManager<User> userManager, IDesignChooser designChooser)
        {
            _appContext = appContext;
            _userManager = userManager;
            _designChooser = designChooser;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.User = _userManager.GetUserAsync(User).Result;
            return View();
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        public IActionResult SetDesign(string design, string returnUrl)
        {
            _designChooser.Current = design;
            Response.Cookies.Append("stylePath",_designChooser.TakeTheme, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            Response.Cookies.Append("style", _designChooser.Current, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
        }
    }
}
