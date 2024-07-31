using Microsoft.EntityFrameworkCore;
using ProductStore.Core.Abstractions;
using ProductStore.Core.Models;

namespace ProductStore.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductStoreDbContext _context;

    public ProductRepository(ProductStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.ProductCategory)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.ProductCategory)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.ProductCategory = product.ProductCategory;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}