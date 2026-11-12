using ShopMVC.Models.Orders;

namespace ShopMVC.ViewModels;

public class OrderLineViewModel
{
    public string Name { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal LineTotal => UnitPrice * Quantity;

    public static OrderLineViewModel Create(OrderLine orderLine) => new()
    {
        Name = orderLine.Name,
        UnitPrice = orderLine.UnitPrice,
        Quantity = orderLine.Quantity
    };
}
