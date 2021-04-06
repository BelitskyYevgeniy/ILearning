using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using T_4.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace T_4.Controllers
{
    public class RegisterController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {

            if (ModelState.IsValid)
            {
                
                User user = new User { UserName = model.Login, Email  = model.Login,NickName=model.Name, Registration=DateTime.Now, IsActive=true,  };
                user.LockoutEnabled = true;
                IdentityResult result =UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    ViewData["IsSuccess"] = "Registration completed successfully!";
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

    }

    
}
