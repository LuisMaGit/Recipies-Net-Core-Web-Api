using Api.Models.Identity.DB;
using Api.Models.RecipieModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class RecipiesDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public RecipiesDbContext(DbContextOptions<RecipiesDbContext> opt) : base(opt)
        {
        }

        public DbSet<Recipie> Recipie { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        
    }
}