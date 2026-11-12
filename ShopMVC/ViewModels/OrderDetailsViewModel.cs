using ShopMVC.Models.Orders;
using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class OrderDetailsViewModel
{
    public long Id { get; init; }
    public DateTime CreatedOn { get; init; }
    public OrderStatus Status { get; init; }
    public string City { get; init; } = string.Empty;
    public string StreetName { get; init; } = string.Empty;
    public int StreetNumber { get; init; }
    public string ReceiverFullName { get; init; } = string.Empty;
    public ShipmentMethod ShipmentMethod { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public IReadOnlyCollection<OrderLineViewModel> Lines { get; init; } = Array.Empty<OrderLineViewModel>();
    public decimal TotalPrice { get; init; }

    public static OrderDetailsViewModel Create(Order order) => new()
    {
        Id = order.Id,
        CreatedOn = order.CreatedOn,
        Status = order.Status,
        City = order.Shipment.City,
        StreetName = order.Shipment.StreetName,
        StreetNumber = order.Shipment.StreetNumber,
        ReceiverFullName = order.Shipment.ReceiverFullName,
        ShipmentMethod = order.Shipment.ShipmentMethod,
        PaymentMethod = order.Payment.PaymentMethod,
        Lines = order.Lines.Select(OrderLineViewModel.Create).ToArray(),
        TotalPrice = order.TotalPrice
    };
}
