using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using T_4.Models;

namespace T_4.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
     
        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model)
        { 
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = UserManager.Find(model.Email, model.Password);
            if (user == null)
            {
                    ModelState.AddModelError("", "Wrong Login or Password!");
                    return View(model);
            }
            ClaimsIdentity claim = UserManager.CreateIdentity(user,DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, claim) ;
                    
           return RedirectToAction("Index", "Home");
        }
       
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}