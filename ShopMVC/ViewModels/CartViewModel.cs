using ShopMVC.Models.Carts;

namespace ShopMVC.ViewModels;

public class CartViewModel
{
    public IReadOnlyCollection<CartItemViewModel> Items { get; init; } = Array.Empty<CartItemViewModel>();
    public decimal TotalPrice => Items.Sum(item => item.LineTotal);
    public int TotalQuantity => Items.Sum(item => item.Quantity);
    public bool IsEmpty => Items.Count == 0;

    public static CartViewModel Empty => new();

    public static CartViewModel Create(Cart? cart) => cart is null
        ? Empty
        : new CartViewModel
        {
            Items = cart.CartProducts.Select(CartItemViewModel.Create).ToArray()
        };
}
