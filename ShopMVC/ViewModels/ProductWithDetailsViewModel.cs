using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class ProductWithDetailsViewModel
{
    public long Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
    public int Quantity { get; set; } = 1;
    public string? ImageUrl { get; set; }

    public static ProductWithDetailsViewModel Create(Product product)
    {
        return new ProductWithDetailsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            AvailableQuantity = product.AvailableQuantity,
            ImageUrl = product.ImageUrl
        };
    }
}