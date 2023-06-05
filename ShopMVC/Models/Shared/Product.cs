using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models.Shared;

public class Product : UpdatableEntity
{
    [Required] 
    public string Name { get; private set; } = string.Empty;
    public Category Category { get; private set; }
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }
    public string CreatedBy { get; private set; }
    public string ModifiedBy { get; private set; }

    private Product()
    {
    }

    public Product(string name, string category, decimal price, int availableQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        if (price <= 0)
            throw new ArgumentException("Price cannot be equals 0");
        if (availableQuantity < 0)
            throw new ArgumentException("AvailableQuantity cannot be smaller than 0");
        
        Name = name;
        Category = Enum.Parse<Category>(category);
        Price = price;
        AvailableQuantity = availableQuantity;
    }

    public void Update(Product existingProduct)
    {
        if (string.IsNullOrWhiteSpace(existingProduct.Name))
            throw new ArgumentException("Name cannot be empty");
        if (existingProduct.Price <= 0)
            throw new ArgumentException("Price cannot be equals 0");
        if (existingProduct.AvailableQuantity < 0)
            throw new ArgumentException("AvailableQuantity cannot be smaller than 0");
        
        Name = existingProduct.Name;
        Price = existingProduct.Price;
        AvailableQuantity = existingProduct.AvailableQuantity;
    }

    public void SetUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(CreatedBy))
            CreatedBy = userId;
        
        ModifiedBy = userId;
    }
}