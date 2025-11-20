using EShop.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Data;

public class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger = null)
    {
        await dbContext.Database.MigrateAsync();

        var adminRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole == null)
        {
            adminRole = new Role
            {
                Name = "Admin",
                Description = "Administrator role",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            dbContext.Roles.Add(adminRole);
            logger?.LogInformation("Admin role created.");
        }

        var userRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (userRole == null)
        {
            userRole = new Role
            {
                Name = "User",
                Description = "Normal user role",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            dbContext.Roles.Add(userRole);
            logger?.LogInformation("User role created.");
        }

        await dbContext.SaveChangesAsync();

        var adminUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "admin@EShop.com");

        if (adminUser == null)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin123!");

            adminUser = new User
            {
                Name = "Super",
                LastName = "Admin",
                ThirdName = "",
                Email = "admin@EShop.com",
                PhoneNumber = "00000000000",
                Address = "Head Office",
                Gender = Gender.Male,
                UserName = "admin",
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            dbContext.Users.Add(adminUser);
            await dbContext.SaveChangesAsync();
            logger?.LogInformation("Admin user created.");
        }

        bool adminHasRole = await dbContext.UserRoles
            .AnyAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);

        if (!adminHasRole)
        {
            dbContext.UserRoles.Add(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });

            await dbContext.SaveChangesAsync();
            logger?.LogInformation("Admin role assigned to admin user.");
        }
    }
}
