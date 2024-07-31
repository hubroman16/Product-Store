using Microsoft.EntityFrameworkCore;
using ProductStore.Core.Abstractions;
using ProductStore.Core.Models;

namespace ProductStore.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductStoreDbContext _context;

    public CategoryRepository(ProductStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Category> GetByIdAsync(Guid id)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(category.Id);
        if (existingCategory != null)
        {
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            existingCategory.Products = category.Products;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}