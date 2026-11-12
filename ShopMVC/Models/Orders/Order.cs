using Microsoft.AspNetCore.Identity;
using ShopMVC.Models.Carts;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Orders;

public class Order : UpdatableEntity
{
    public const string ShippingLineName = "Shipment";

    private List<OrderLine> _lines = new();
    
    public string UserId { get; private set; } = null!;
    public IdentityUser User { get; private set; } = null!;
    public IReadOnlyCollection<OrderLine> Lines { get => _lines; }
    public Shipment Shipment { get; private set; } = null!;
    public Payment Payment { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public bool IsCompleted => Status is OrderStatus.Canceled || Status is OrderStatus.Completed;
    public decimal TotalPrice => _lines.Sum(line => line.UnitPrice * line.Quantity);

    private Order()
    {
    }
    
    private Order(
            string userId,
            IReadOnlyCollection<OrderLine> lines,
            Shipment shipment,
            Payment payment)
        {
            Status = OrderStatus.Placed;

            UserId = userId;
            _lines = lines.ToList();
            Shipment = shipment ?? throw new ArgumentNullException(nameof(shipment));
            Payment = payment ?? throw new ArgumentNullException(nameof(payment));
        }

        public static Order CreateFromCheckout(CheckoutCart checkoutCart)
        {
            var shipment = checkoutCart.Shipment ?? throw new InvalidShipmentException();
            var payment = checkoutCart.Payment ?? throw new CannotPlaceOrderException();

            var orderLines = checkoutCart.Products
                .Select((cartProduct, index) =>
                    new OrderLine(index, cartProduct.Product.Name, cartProduct.Product.Price, cartProduct.Quantity))
                .ToList();

            var shipmentOrderLineNumber = orderLines.Max(line => line.OrderLineNumber) + 1;
            var shipmentLine = new OrderLine(shipmentOrderLineNumber, ShippingLineName, shipment.ShippingCost, 1);
            
            orderLines.Add(shipmentLine);
            
            return new Order(checkoutCart.UserId, orderLines, shipment, payment);
        }

        public void Complete()
        {
            if (Status is OrderStatus.Canceled)
            {
                throw new InvalidOrderStatusChangeException();
            }

            Status = OrderStatus.Completed;
        }

        public void Cancel()
        {
            if (Status is OrderStatus.Completed)
            {
                throw new InvalidOrderStatusChangeException();
            }

            Status = OrderStatus.Canceled;
        }
}

public enum OrderStatus
{
    Placed = 1,
    Completed = 2,
    Canceled = 3
}
