namespace ProductStore.API.Contracts;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ProductsResponse> Products { get; set; }
}