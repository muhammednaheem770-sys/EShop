using EShop.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EShop.Data
{
    public class DatabaseSeeder
    {   
        public static async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger = null)
        {
            await dbContext.Database.MigrateAsync();

            if (!dbContext.Roles.Any(r => r.Name == "Admin"))
            {
                dbContext.Roles.Add(new Role
                {
                    Name = "Admin",
                    Description = "Administrator role"
                });

                logger?.LogInformation("Admin role created.");
            }

            if (!dbContext.Roles.Any(r => r.Name == "User"))
            {
                dbContext.Roles.Add(new Role
                {
                    Name = "User",
                    Description = "Normal user role"
                });
            }

            await dbContext.SaveChangesAsync();

            if (!dbContext.Users.Any(u => u.Email == "admin@EShop.com"))
            {
                var admin = new User
                {
                    Name = "Super",
                    LastName = "Admin",
                    ThirdName = "",
                    Email = "admin@EShop.com",
                    PhoneNumber = "00000000000",
                    Address = "Head Office",
                    Gender = Gender.Male,
                    UserName = "admin",
                    PassWord = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    CreatedAt = DateTime.UtcNow
                };


                dbContext.Users.Add(admin);
                logger?.LogInformation("Admin user created.");
                await dbContext.SaveChangesAsync();

                var adminRole = await dbContext.Roles.FirstAsync(r => r.Name == "Admin");

                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = admin.Id,
                    RoleId = adminRole.Id
                });

                await dbContext.SaveChangesAsync();
            }

        }
    }
}
