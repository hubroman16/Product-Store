using Microsoft.AspNetCore.Mvc;
using ProductStore.API.Contracts;
using ProductStore.Core.Abstractions;


namespace ProductStore.API.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ProductsResponse>>> GetAllProductsAsync()
        {
            var products = await _productsService.GetAllProductsAsync();

            var response = products.Select(product => new ProductsResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            }).ToList();

            return Ok(response);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productsService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var category = await _productsService.GetCategoryByIdAsync(request.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId");
            }

            var (createdProduct, error) = await _productsService.CreateProductAsync
                (Guid.NewGuid(), request.Name, request.Description, request.Price, request.CategoryId, category);
            
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductCreateUpdateRequest request)
        {
            if (request == null || id != request.Id)
            {
                return BadRequest("Product ID mismatch or request cannot be null");
            }

            var category = await _productsService.GetCategoryByIdAsync(request.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId");
            }

            var error = await _productsService.UpdateProductAsync
                (id, request.Name, request.Description, request.Price, request.CategoryId, category);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var error = await _productsService.DeleteProductAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                return NotFound(error);
            }

            return NoContent();
        }
    }