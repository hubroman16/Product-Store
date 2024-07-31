namespace ProductStore.Core.Models;

public class Category
{
    public Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        Products = new List<Product>();
    }
    
    private Category() 
    {
        Products = new List<Product>();
    }
        
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Product> Products { get; set; } 
        
    public static (Category Category, string Error) Create(Guid id, string name, string description, List<Product> products)
    {
        var error = string.Empty;
            
        if (string.IsNullOrEmpty(name) || name.Length > 250)
        {
            error = "Name can't be null or longer than 250 symbols";
        }

        var category = new Category(id, name, description)
        {
            Products = products
        };

        return (category, error);
    }
}