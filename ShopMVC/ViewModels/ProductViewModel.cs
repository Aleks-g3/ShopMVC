using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class ProductViewModel
{
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }

    public static ProductViewModel Create(Product product)
    {
        return new ProductViewModel()
        {
            Name = product.Name,
            Category = product.Category.ToString(),
            Price = product.Price,
            AvailableQuantity = product.AvailableQuantity
        };
    }
}