using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using T_4.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using Microsoft.Owin.Security;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace T_4.Controllers
{
   
    public class HomeController : Controller
    {
        private delegate void innerLogic(User user);
        private void AfterAuthentication()
        {
            ViewBag.Users = UserManager.Users;
            User user = UserManager.FindByName(User.Identity.Name);
            if (user != null)
            {
                user.LastVisit = DateTime.Now;
                UserManager.Update(user);
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private JsonResult ChangeActive(bool b)
        {
            return DoMainLogic((user) =>
            {
                user.IsActive = b;
                UserManager.Update(user);
            });
        }
        
        private bool CheckAccess()
        {
            User user = UserManager.FindByName(User.Identity.Name);
            return user != null && user.IsActive && User.Identity.IsAuthenticated;
        }
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            if(!CheckAccess())
            {
                return RedirectToAction("Logout","Account");
            }
            AfterAuthentication();
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult Unblock()
        {

            return ChangeActive(true);
        }
        
        [Authorize]
        [HttpPost]
        public JsonResult Block()
        {
            return ChangeActive(false);
            
        }
       [Authorize]
       [HttpPost]
       public JsonResult Delete()
        {
            return DoMainLogic((user)=>
            {
                    UserManager.Delete(user);
            });
        }
        private JsonResult DoMainLogic(innerLogic logic)
        {

            if (!CheckAccess())
                return Json(new { redirectToUrl = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
            var argv = Request.QueryString.Get("user");
            if (argv == null)
                return Json(new { redirectToUrl = Url.Action("Index") }, JsonRequestBehavior.AllowGet);
            var usersLogin = argv.Split(',');
            foreach (var userLogin in usersLogin)
            {
                User user = UserManager.FindByName(userLogin);
                if (user != null)
                    logic(user);
            }
            return Json(new { redirectToUrl=Url.Action("Index")}, JsonRequestBehavior.AllowGet);
        }

    }
}