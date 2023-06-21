using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class SimpleProductViewModel
{
    public long Id { get; private set; }
    public string Name { get; private set; } 
    public decimal Price { get; private set; }

    public static SimpleProductViewModel Create(Product product)
    {
        return new SimpleProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}