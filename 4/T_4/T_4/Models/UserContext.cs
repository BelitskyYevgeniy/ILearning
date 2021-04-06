
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace T_4.Models
{
    
    public class UserContext : IdentityDbContext<User>
    {

        public UserContext():base("UsersDb")
        {
        }

        public static UserContext Create()
        {
            return new UserContext();
        }

    }
}