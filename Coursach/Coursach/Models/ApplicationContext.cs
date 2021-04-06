using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Coursach.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }

        public ApplicationContext(DbContextOptions options):base(options)
        {
            Database.EnsureCreated();
        }

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=AppContext.md;Trusted_Connection=True;MultipleActiveResultSets=true");
        }*/



    }
}