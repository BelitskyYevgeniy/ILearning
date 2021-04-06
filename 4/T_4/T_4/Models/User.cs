using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace T_4.Models
{
    public class User: IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
       
        public string NickName { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime LastVisit { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Registration { get; set; }

        public bool IsActive
        { 
            get
            {
                return DateTime.Now > LockoutEndDateUtc;
            }
            set
            {
                if (value)
                {
                    LockoutEndDateUtc = DateTime.Now.AddDays(-1);
                }
                else
                {

                    LockoutEndDateUtc = DateTime.Now.AddYears(300);
                }
            }
        }

    }
}