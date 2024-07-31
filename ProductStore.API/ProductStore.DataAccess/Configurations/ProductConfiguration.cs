using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductStore.Core.Models;

namespace ProductStore.DataAccess.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(a => a.ProductCategory)
            .WithMany(c => c.Products)
            .HasForeignKey(c=>c.CategoryId);
        
        builder.Property(b => b.Name)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(b => b.Description)
            .IsRequired();
        builder.Property(b => b.Price)
            .IsRequired();
    }
}