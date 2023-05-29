using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class UpdatableProductViewModel
{
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public string ModifiedBy { get; private set; } = string.Empty;
    

    public static UpdatableProductViewModel Create(Product product)
    {
        return new UpdatableProductViewModel()
        {
            Name = product.Name,
            Category = product.Category.ToString(),
            Price = product.Price,
            AvailableQuantity = product.AvailableQuantity,
            ModifiedOn = product.ModifiedOn,
        };
    }
}