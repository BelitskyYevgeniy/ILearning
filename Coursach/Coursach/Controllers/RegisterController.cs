using System.Linq;
using System.Threading.Tasks;
using Coursach.ViewModels;
using Coursach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Coursach.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = new User { Email = model.Email, UserName = model.Email,NickName=model.Name, Posts = new System.Collections.Generic.List<Post>() };
            var result =_userManager.CreateAsync(user, model.Password);
            if (!result.Result.Succeeded) return ShowErrors(model, result.Result);
            InitRole(user);
            return await DoAfterRegistration(model, user);
        }
        private async Task<IActionResult> DoAfterRegistration(RegisterViewModel model, User user)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        private void InitRole(User user)
        {
            if(_userManager.Users.Count()==1)
            {
               _roleManager.CreateAsync(new IdentityRole("admin")).Wait();
               _userManager.AddToRoleAsync(user, "admin").Wait();
               _roleManager.CreateAsync(new IdentityRole("user")).Wait();
            }
          _userManager.AddToRoleAsync(user, "user");
        }
        private IActionResult ShowErrors(RegisterViewModel model, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

    }
}

