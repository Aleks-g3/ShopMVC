using ShopMVC.Models.Carts;

namespace ShopMVC.ViewModels;

public class CartItemViewModel
{
    public long ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal LineTotal => UnitPrice * Quantity;

    public static CartItemViewModel Create(CartProduct cartProduct) => new()
    {
        ProductId = cartProduct.ProductId != 0 ? cartProduct.ProductId : cartProduct.Product.Id,
        Name = cartProduct.Product.Name,
        UnitPrice = cartProduct.Product.Price,
        Quantity = cartProduct.Quantity
    };
}
