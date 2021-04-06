using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Coursach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Coursach;

namespace Coursach.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private delegate void innerLogic(User user);
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private ApplicationContext _appContext;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext appContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _appContext = appContext;
        }
        [HttpGet]
        public IActionResult Users()
        {
            if (!CheckAccess())
                return RedirectToAction("Logout", "Login");
            ViewBag.User = _userManager.GetUserAsync(User).Result;
            
            return View();
        }
        [HttpGet]
        public IActionResult Posts(string id)
        {
            if (!CheckAccess())
                return RedirectToAction("Logout", "Login");
            if (id == null)
            {
                ViewBag.Posts = _appContext.Posts;
            }
            else
            {
                User user = _userManager.FindByIdAsync(id).Result;
                ViewBag.Posts = user.Posts;
            }
            return View();
        }
        [HttpPost]
        public IActionResult ChangeRole()
        {
            return DoMainLogic((user) =>
            {
                var role = _userManager.GetRolesAsync(user).Result;
                foreach (var r in role)
                    Console.WriteLine(r);
                Task<IdentityResult> result = null; 
                if (role.Contains("admin"))
                {
                 result= _userManager.RemoveFromRoleAsync(user, "admin");
                }
                else
                {
                  result = _userManager.AddToRoleAsync(user, "admin");
                }
                if(result.Result.Succeeded)
                    _userManager.UpdateAsync(user).Wait();
                
            }, Url.Action("Users"));
        }


        [HttpPost]
        public IActionResult DeleteUser()
        {
            return DoMainLogic((user) =>
            {
              _userManager.DeleteAsync(user).Wait();
              _userManager.UpdateAsync(user).Wait();
            }, Url.Action("Users"));
        }

        [HttpPost]
        public IActionResult BlockUser()
        {
            return DoMainLogic((user) =>
            {
                bool isActive = _userManager.GetLockoutEnabledAsync(user).Result;
                _userManager.SetLockoutEnabledAsync(user, true).Wait();
                if(_userManager.GetLockoutEndDateAsync(user).Result>DateTime.Now)
                {
                    _userManager.SetLockoutEndDateAsync(user, DateTime.Now).Wait();
                }
                else
                {
                    _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue).Wait();
                }
                _userManager.SetLockoutEnabledAsync(user, isActive).Wait();
                _userManager.UpdateAsync(user).Wait();
            }, Url.Action("Users"));
        }

        private bool CheckActivity(User user)
        {
            var time = _userManager.GetLockoutEndDateAsync(user).Result;
            return  time==null || time<= DateTime.Now;
        }
        private bool CheckAccess()
        {
            User user = _userManager.GetUserAsync(User).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            return User.Identity.IsAuthenticated && user!=null && roles.Contains("admin") && CheckActivity(user);
        }

        
        private IActionResult DoMainLogic(innerLogic logic, string path)
        {
            var json = Json(new { redirectToUrl = path });
            if (!CheckAccess())
                return json;
            var argv = Request.Query["user"].ToArray();
            if (argv == null)
                return json;
            foreach (var userLogin in argv)
            {
                var user = _userManager.FindByIdAsync(userLogin).Result;
                if (user != null)
                    logic(user);
            }
            return json;
        }
    }
}
