using Microsoft.EntityFrameworkCore;
using ProductStore.Core.Models;
using ProductStore.DataAccess.Configurations;

namespace ProductStore.DataAccess;

public class ProductStoreDbContext(DbContextOptions<ProductStoreDbContext> options) 
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}