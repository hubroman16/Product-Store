using ProductStore.Core.Models;

namespace ProductStore.API.Contracts;

public class ProductsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}