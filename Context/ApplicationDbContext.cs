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

        var adminRoleId = Guid.NewGuid();
        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { Id = adminRoleId, Name = "Admin" },
            new RoleEntity { Id = Guid.NewGuid(), Name = "User" }
        );

        var adminUserId = Guid.NewGuid();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"); // default password
        modelBuilder.Entity<User>().HasData(
            new User { Id = adminUserId, Name = "Admin User", Email = "admin@eshop.com", PasswordHash = passwordHash }
        );

        // Assign Admin Role
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserId = adminUserId, RoleId = adminRoleId }
        );
    }

}
