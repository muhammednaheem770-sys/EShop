using EShop.Context;
using EShop.Data;
using EShop.entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using BCrypt.Net;

namespace EShop.DatabaseSeeder
{
    public static class Seeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Run pending migrations first
            await context.Database.MigrateAsync();

            // ---- SEED ROLES ----
            if (!await context.Roles.AnyAsync())
            {
                Log.Information("No roles found. Seeding default roles...");

                var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin" };
                var userRole = new Role { Id = Guid.NewGuid(), Name = "User" };

                await context.Roles.AddRangeAsync(adminRole, userRole);
                await context.SaveChangesAsync();

                Log.Information("Default roles seeded successfully: Admin, User");
            }
            else
            {
                Log.Information("Roles already exist. Skipping role seeding.");
            }

            // ---- SEED ADMIN USER ----
            if (!await context.Users.AnyAsync(u => u.Email == "admin@example.com"))
            {
                Log.Information("No admin user found. Creating default admin account...");

                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "System Administrator",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    CreatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

                // Get the admin role
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

                if (adminRole != null)
                {
                    await context.UserRoles.AddAsync(new UserRole
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id
                    });

                    await context.SaveChangesAsync();
                    Log.Information("Admin user linked to Admin role successfully.");
                }
                else
                {
                    Log.Error("Admin role not found during seeding — unable to link admin user!");
                }

                Log.Information("Default admin created successfully: Email=admin@example.com, Password=Admin123!");
            }
            else
            {
                Log.Information("Admin user already exists. Skipping admin seeding.");
            }
        }
    }
}
