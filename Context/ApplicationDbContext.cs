using EShop.Data;
using EShop.entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fixed, constant GUIDs (so EF can track them)
        var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var adminUserId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var adminUserRoleId = Guid.Parse("44444444-4444-4444-4444-444444444444");

        const string adminPasswordHash = "$2a$11$HjN2Jzk4sZz9WmD8TnEj6u7xJwZ8ljD7.mK4rKMCTi/JFJH1McfWe";

        // --- Seed Roles ---
        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { Id = adminRoleId, Name = "Admin" },
            new RoleEntity { Id = userRoleId, Name = "User" }
        );

        // --- Seed Admin User ---
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = adminUserId,
                Name = "Admin User",
                Email = "admin@eshop.com",
                PasswordHash = adminPasswordHash
            }
        );

        // --- Assign Admin Role ---
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                Id = adminUserRoleId, // ✅ Important: add this
                UserId = adminUserId,
                RoleId = adminRoleId
            }
        );
    }


}
