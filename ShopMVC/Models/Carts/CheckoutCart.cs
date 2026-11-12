using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Orders;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Carts;

public class CheckoutCart
{
    private readonly List<CartProduct> _products;

    public string UserId { get; }
    public Shipment? Shipment { get; private set; }
    public Payment? Payment { get; private set; }
    public IReadOnlyCollection<CartProduct> Products => _products;

    public decimal ProductsPrice => _products.Sum(product => product.Quantity * product.Product.Price);
    public decimal ShippingCost => Shipment?.ShippingCost ?? 0m;
    public decimal TotalPrice => ProductsPrice + ShippingCost;

    internal CheckoutCart(Cart cart)
    {
        UserId = cart.UserId;
        _products = cart.CartProducts.ToList();
    }

    public void SetShipment(Shipment shipment)
        => Shipment = shipment;

    public void SetPayment(Payment payment)
    {
        if (ProductsPrice > 20_000 && payment.PaymentMethod is PaymentMethod.Cash)
        {
            throw new PaymentMethodNotAllowedException();
        }

        Payment = payment;
    }

    public Order PlaceOrder()
    {
        if (Shipment is null)
            throw new InvalidShipmentException();

        if (Payment is null)
            throw new CannotPlaceOrderException();

        return Order.CreateFromCheckout(this);
    }
}
