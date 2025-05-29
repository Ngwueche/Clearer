using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data.EFCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
    }
}
