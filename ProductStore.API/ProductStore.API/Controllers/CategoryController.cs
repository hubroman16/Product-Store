using Microsoft.AspNetCore.Mvc;
using ProductStore.API.Contracts;
using ProductStore.Core.Abstractions;

namespace ProductStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
        [HttpGet]
        public async Task<ActionResult<List<CategoryResponse>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            var response = categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Products = category.Products.Select(product => new ProductsResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                }).ToList()
            }).ToList();

            return Ok(response);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Products = category.Products.Select(product => new ProductsResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                }).ToList()
            };

            return Ok(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var (createdCategory, error) = await _categoryService.CreateCategoryAsync
                (Guid.NewGuid(), request.Name, request.Description);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var response = new CategoryResponse
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description,
                Products = new List<ProductsResponse>()
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = response.Id }, response);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryCreateUpdateRequest request)
        {
            if (request == null || id != request.Id)
            {
                return BadRequest("Category ID mismatch or request cannot be null");
            }

            var error = await _categoryService.UpdateCategoryAsync(id, request.Name, request.Description);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var error = await _categoryService.DeleteCategoryAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                return NotFound(error);
            }

            return NoContent();
        }
}