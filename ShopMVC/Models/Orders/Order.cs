using Microsoft.AspNetCore.Identity;
using ShopMVC.Models.Carts;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Orders;

public class Order : UpdatableEntity
{
    private List<OrderLine> _lines;
    
    public string UserId { get; private set; }
    public IdentityUser User { get; private set; } = null!;
    public IReadOnlyCollection<OrderLine> Lines { get => _lines; }
    public Shipment Shipment { get; private set; }
    public Payment Payment { get; private set; }
    public OrderStatus Status { get; private set; }
    public bool IsCompleted => Status is OrderStatus.Canceled || Status is OrderStatus.Completed;

    public Order()
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
            var orderLines = checkoutCart.Products
                .Select((x, i) => new OrderLine(i, x.Product.Name, x.Product.Price, x.Quantity))
                .ToList();

            var shipmentOrderLineNumber = orderLines.Max(x => x.OrderLineNumber) + 1;
            var shipmentLine = new OrderLine(shipmentOrderLineNumber, "Shipment", 10, 1);
            
            orderLines.Add(shipmentLine);
            
            return new Order(checkoutCart.UserId, orderLines, checkoutCart.Shipment, checkoutCart.Payment);
        }

        public static Order CreateFromRawData(
            string userId,
            IReadOnlyCollection<OrderLine> lines,
            Shipment shipment,
            Payment payment,
            Guid? id = null) 
            => new Order(userId, lines, shipment, payment);

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