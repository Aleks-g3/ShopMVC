using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Orders;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Carts;

public class CheckoutCart
{
    private readonly List<CartProduct> _products;

    public string UserId { get; }
    public Shipment Shipment { get; private set; }
    public Payment Payment { get; private set; }
    public IReadOnlyCollection<CartProduct> Products => _products;

    internal CheckoutCart(Cart cart)
    {
        UserId = cart.UserId;
        _products = cart.Products.ToList();
    }

    public void SetShipment(Shipment shipment)
        => Shipment = shipment;

    public void SetPayment(Payment payment)
    {
        var cartValue = _products.Sum(x => x.Quantity * x.Product.Price);

        // does this logic belong to the Cart itself?
        if (cartValue > 20_000 && payment.PaymentMethod is PaymentMethod.Cash)
        {
            throw new PaymentMethodNotAllowedException();
        }

        Payment = payment;
    }

    public Order PlaceOrder()
    {
        return Order.CreateFromCheckout(this);
    }
}