using ProductStore.Core.Abstractions;
using ProductStore.Core.Models;

namespace ProductStore.App.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<(Category Category, string Error)> CreateCategoryAsync(Guid id, string name, string description)
    {
        var (category, error) = Category.Create(id, name, description, new List<Product>());

        if (!string.IsNullOrEmpty(error))
        {
            return (null, error);
        }

        await _categoryRepository.AddAsync(category);
        return (category, string.Empty);
    }

    public async Task<string> UpdateCategoryAsync(Guid id, string name, string description)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);

        if (existingCategory == null)
        {
            return "Category not found";
        }

        existingCategory.Name = name;
        existingCategory.Description = description;

        await _categoryRepository.UpdateAsync(existingCategory);
        return string.Empty;
    }

    public async Task<string> DeleteCategoryAsync(Guid id)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);

        if (existingCategory == null)
        {
            return "Category not found";
        }

        await _categoryRepository.DeleteAsync(id);
        return string.Empty;
    }
}