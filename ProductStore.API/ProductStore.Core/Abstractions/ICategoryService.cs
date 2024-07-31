using ProductStore.Core.Models;

namespace ProductStore.Core.Abstractions;

public interface ICategoryService
{
    Task<Category> GetCategoryByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<(Category Category, string Error)> CreateCategoryAsync(Guid id, string name, string description);
    Task<string> UpdateCategoryAsync(Guid id, string name, string description);
    Task<string> DeleteCategoryAsync(Guid id);
}