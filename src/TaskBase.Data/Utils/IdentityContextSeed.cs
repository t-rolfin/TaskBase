using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Data.Identity;

namespace TaskBase.Data.Utils
{
    public class IdentityContextSeed
    {
        private readonly PasswordHasher<User> _passwordHasher
            = new PasswordHasher<User>();

        public async Task SeedAsync(
            IdentityContext context, IWebHostEnvironment environment, ILogger<IdentityContextSeed> logger)
        {
            await SeedRolesAsync(context, environment, logger);
            await SeedAdminAsync(context, environment, logger);
            await SeedUserRolesAsync(context, environment, logger);
        }


        async Task SeedRolesAsync(
        IdentityContext context, IWebHostEnvironment environment, ILogger<IdentityContextSeed> logger)
        {
            if (!context.Roles.Any())
            {
                logger.LogInformation("Seeding roles...");

                var memberRole = new IdentityRole("Member") { NormalizedName = "MEMBER" };
                var adminRole = new IdentityRole("Admin") { NormalizedName = "ADMIN" };

                context.Roles.AddRange(memberRole, adminRole);
                await context.SaveChangesAsync();
            }
        }

        async Task SeedAdminAsync(
            IdentityContext context, IWebHostEnvironment environment, ILogger<IdentityContextSeed> logger)
        {
            if (!context.Users.Any())
            {
                logger.LogInformation("Seeding admin account...");

                string userId = Guid.NewGuid().ToString();

                var user = new User()
                {
                    Id = userId,
                    Email = "admin@taskbase.com",
                    PasswordHash = "admin",
                    UserName = "admin",
                    AvatarUrl = "",
                    NormalizedEmail = "ADMIN@TASKBASE.COM",
                    NormalizedUserName = "ADMIN"
                };

                user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }

        }

        async Task SeedUserRolesAsync(
            IdentityContext context, IWebHostEnvironment environment, ILogger<IdentityContextSeed> logger)
        {
            if (!context.UserRoles.Any())
            {
                logger.LogInformation("Seeding user roles...");

                var roles = await context.Roles.ToListAsync();

                var user = await context.Users.Where(x => x.UserName.Equals("Admin"))
                    .FirstOrDefaultAsync();

                foreach (var role in roles)
                {
                    var identityUserRole = new IdentityUserRole<string>()
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    };

                    context.UserRoles.Add(identityUserRole);
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}
