using System.Collections.Generic;
using System.Linq;
using Coursach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Authorization;

namespace Coursach.Controllers
{
    [Authorize]
    public class PageController : Controller
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public PageController(UserManager<User> userManger, SignInManager<User> signInManager)
        {
            _userManager = userManger;
            _signInManager = signInManager;
            
        }
        public async Task<IActionResult> Page(string id)
        {
           
            User _user =_userManager.FindByIdAsync(id).Result;
            ViewBag.User = _user;
            ViewBag.Posts = _user.Posts;
            //ViewBag.Url = Request.Path+"?id="+id;
            if (_user == null)
                return RedirectToAction("Index", "Home");
            await _userManager.UpdateAsync(_user);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPost(string id, string title, string body, string returnUrl)
        {
            User _user = _userManager.FindByIdAsync(id).Result;
            if(null==_user)
                return View("Error");
            Post post = new Post() {Title = title, User = _user, Likes = 0, Body = body};
            if (_user.Posts == null) _user.Posts = new List<Post>();
            _user.Posts.Add(post);
            await _userManager.UpdateAsync(_user);
            return LocalRedirect(returnUrl);
        }

    }

}
