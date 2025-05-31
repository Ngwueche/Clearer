using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Data.EFCore;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.ServiceImplementations
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Apply pending migrations
            await context.Database.MigrateAsync();

            // Seed roles
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
            {
                new Role { RoleName = "Admin", Id = Guid.CreateVersion7() },
                new Role { RoleName = "User", Id = Guid.CreateVersion7() }
            };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            // Seed users
            if (!context.Users.Any())
            {
                var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
                var userRole = context.Roles.FirstOrDefault(r => r.RoleName == "User");

                context.Users.Add(new ApplicationUser
                {
                    Id = Guid.CreateVersion7(),
                    FirstName = "System",
                    LastName = "Admin",
                    OtherName = "",
                    UserName = "admin",
                    IsActive = true,
                    RoleId = adminRole.Id,
                    PasswordHash = PasswordHelper.HashPassword("Admin123")
                });
                context.Users.Add(new ApplicationUser
                {
                    Id = Guid.CreateVersion7(),
                    FirstName = "Gift",
                    LastName = "Eboigbe",
                    OtherName = "OgheneVwegba",
                    UserName = "EGO",
                    IsActive = true,
                    RoleId = userRole.Id,
                    PasswordHash = PasswordHelper.HashPassword("User123")
                });

                await context.SaveChangesAsync();
            }
        }
    }

}
