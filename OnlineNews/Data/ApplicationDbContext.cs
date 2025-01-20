using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Models.Database;

namespace OnlineNews.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {


        }
    }
}
