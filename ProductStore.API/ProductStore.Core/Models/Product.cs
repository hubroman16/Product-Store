namespace ProductStore.Core.Models;

public class Product
{
    public Product(Guid id, string name, string description, decimal price, Guid categoryId, Category productCategory)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        ProductCategory = productCategory;
    }
    
    private Product() { }
        
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Category ProductCategory { get; set; }

    public static (Product Product, string Error) Create(Guid id, string name, string description, decimal price, Guid categoryId, Category productCategory)
    {
        var error = string.Empty;
            
        if (string.IsNullOrEmpty(name) || name.Length > 250)
        {
            error = "Name can't be null or longer than 250 symbols";
        }

        var product = new Product(id, name, description, price, categoryId, productCategory);

        return (product, error);
    }
}
