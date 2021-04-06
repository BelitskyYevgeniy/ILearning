using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Coursach.Models
{
    public class User : IdentityUser
    {
        //public Url Image { get; set; }
        public string NickName { get; set; }
        public List<Post> Posts { get; set; }
    }
}
