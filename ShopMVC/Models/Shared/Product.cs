using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models.Shared;

public class Product : UpdatableEntity
{
    [Required] 
    public string Name { get; private set; } = string.Empty;
    [Required]
    public string Description { get; private set; } = string.Empty;
    public Category Category { get; private set; }
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string ModifiedBy { get; private set; } = string.Empty;

    private Product()
    {
    }

    public Product(string name, string description,string category, decimal price, int availableQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty");
        if (price <= 0)
            throw new ArgumentException("Price cannot be equals 0");
        if (availableQuantity < 0)
            throw new ArgumentException("AvailableQuantity cannot be smaller than 0");
        
        Name = name;
        Description = description;
        Category = Enum.Parse<Category>(category);
        Price = price;
        AvailableQuantity = availableQuantity;
    }

    public void Update(Product existingProduct)
    {
        if (string.IsNullOrWhiteSpace(existingProduct.Name))
            throw new ArgumentException("Name cannot be empty");
        if (string.IsNullOrWhiteSpace(existingProduct.Description))
            throw new ArgumentException("Description cannot be empty");
        if (existingProduct.Price <= 0)
            throw new ArgumentException("Price cannot be equals 0");
        if (existingProduct.AvailableQuantity < 0)
            throw new ArgumentException("AvailableQuantity cannot be smaller than 0");
        
        Name = existingProduct.Name;
        Description = existingProduct.Description;
        Price = existingProduct.Price;
        AvailableQuantity = existingProduct.AvailableQuantity;
    }

    public void SetUser(string username)
    {
        if (string.IsNullOrWhiteSpace(CreatedBy))
            CreatedBy = username;
        
        ModifiedBy = username;
    }
}