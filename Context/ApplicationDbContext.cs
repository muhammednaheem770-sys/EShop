using EShop.entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Context;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; } 
}
