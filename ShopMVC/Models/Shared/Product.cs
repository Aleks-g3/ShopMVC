using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models.Shared;

public class Product : UpdatableEntity
{
    [Required]
    public string Name { get; private set; }
    public Category Category { get; private set; }
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }

    public Product()
    {
    }

    public Product(string name, string category, decimal price, int availableQuantity)
    {
        Name = name;
        Category = Enum.Parse<Category>(category);
        Price = price;
        AvailableQuantity = availableQuantity;
    }
}