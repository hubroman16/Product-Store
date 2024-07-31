using ProductStore.Core.Abstractions;
using ProductStore.Core.Models;

namespace ProductStore.App.Services;

public class ProductsService : IProductsService
{
    private readonly IProductRepository _productsRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsService(IProductRepository productsRepository, ICategoryRepository categoryRepository)
        {
            _productsRepository = productsRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productsRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productsRepository.GetAllAsync();
        }

        public async Task<(Product Product, string Error)> CreateProductAsync(Guid id, string name, string description, decimal price, Guid categoryId, Category productCategory)
        {
            var (product, error) = Product.Create(id, name, description, price, categoryId, productCategory);

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            await _productsRepository.AddAsync(product);
            return (product, string.Empty);
        }

        public async Task<string> UpdateProductAsync(Guid id, string name, string description, decimal price, Guid categoryId, Category productCategory)
        {
            var existingProduct = await _productsRepository.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return "Product not found";
            }

            var (product, error) = Product.Create(id, name, description, price, categoryId, productCategory);

            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }

            await _productsRepository.UpdateAsync(product);
            return string.Empty;
        }

        public async Task<string> DeleteProductAsync(Guid id)
        {
            var existingProduct = await _productsRepository.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return "Product not found";
            }

            await _productsRepository.DeleteAsync(id);
            return string.Empty;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }
}