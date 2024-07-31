using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductStore.Core.Models;


namespace ProductStore.DataAccess.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(a => a.Products)
            .WithOne(c => c.ProductCategory)
            .HasForeignKey(a=>a.CategoryId);
        
        builder.Property(b => b.Name)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(b => b.Description)
            .IsRequired();
    }
}