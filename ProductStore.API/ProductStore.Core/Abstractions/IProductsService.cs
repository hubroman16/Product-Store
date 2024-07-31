using ProductStore.Core.Models;

namespace ProductStore.Core.Abstractions;

public interface IProductsService
{
    Task<Product> GetProductByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<(Product Product, string Error)> CreateProductAsync(Guid id, string name, string description, decimal price, Guid categoryId, Category productCategory);
    Task<string> UpdateProductAsync(Guid id, string name, string description, decimal price, Guid categoryId, Category productCategory);
    Task<string> DeleteProductAsync(Guid id);
    Task<Category> GetCategoryByIdAsync(Guid id);
}