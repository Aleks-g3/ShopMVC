using ShopMVC.Models.Orders;

namespace ShopMVC.ViewModels;

public class OrderSummaryViewModel
{
    public long Id { get; init; }
    public DateTime CreatedOn { get; init; }
    public OrderStatus Status { get; init; }
    public int ItemsCount { get; init; }
    public decimal TotalPrice { get; init; }

    public static OrderSummaryViewModel Create(Order order) => new()
    {
        Id = order.Id,
        CreatedOn = order.CreatedOn,
        Status = order.Status,
        ItemsCount = order.Lines
            .Where(line => line.Name != Order.ShippingLineName)
            .Sum(line => line.Quantity),
        TotalPrice = order.TotalPrice
    };
}
