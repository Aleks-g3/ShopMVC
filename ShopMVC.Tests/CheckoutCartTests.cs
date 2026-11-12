using ShopMVC.Models.Carts;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Tests;

public class CheckoutCartTests
{
    [Fact]
    public void SetPayment_CashAboveLimit_Throws()
    {
        var checkoutCart = CreateCheckoutCart(price: 25_000m);

        Assert.Throws<PaymentMethodNotAllowedException>(() =>
            checkoutCart.SetPayment(new Payment(PaymentMethod.Cash)));
    }

    [Fact]
    public void SetPayment_CashlessAboveLimit_Succeeds()
    {
        var checkoutCart = CreateCheckoutCart(price: 25_000m);

        checkoutCart.SetPayment(new Payment(PaymentMethod.Cashless));

        Assert.Equal(PaymentMethod.Cashless, checkoutCart.Payment?.PaymentMethod);
    }

    [Fact]
    public void PlaceOrder_WithoutShipment_Throws()
    {
        var checkoutCart = CreateCheckoutCart();
        checkoutCart.SetPayment(new Payment(PaymentMethod.Cashless));

        Assert.Throws<InvalidShipmentException>(() => checkoutCart.PlaceOrder());
    }

    [Fact]
    public void PlaceOrder_WithoutPayment_Throws()
    {
        var checkoutCart = CreateCheckoutCart();
        checkoutCart.SetShipment(CreateShipment());

        Assert.Throws<CannotPlaceOrderException>(() => checkoutCart.PlaceOrder());
    }

    [Fact]
    public void PlaceOrder_CreatesOrderWithProductAndShippingLines()
    {
        var checkoutCart = CreateCheckoutCart(quantity: 2, price: 100m);
        checkoutCart.SetShipment(CreateShipment(ShipmentMethod.Courier));
        checkoutCart.SetPayment(new Payment(PaymentMethod.Cashless));

        var order = checkoutCart.PlaceOrder();

        Assert.Equal(2, order.Lines.Count);
        Assert.Equal(220m, order.TotalPrice);
        Assert.Contains(order.Lines, line => line.Name == "Test product" && line.Quantity == 2);
        Assert.Contains(order.Lines, line => line.Name == ShopMVC.Models.Orders.Order.ShippingLineName && line.UnitPrice == 20m);
    }

    [Fact]
    public void Totals_IncludeShippingCost()
    {
        var checkoutCart = CreateCheckoutCart(quantity: 2, price: 100m);
        checkoutCart.SetShipment(CreateShipment(ShipmentMethod.Courier));

        Assert.Equal(200m, checkoutCart.ProductsPrice);
        Assert.Equal(20m, checkoutCart.ShippingCost);
        Assert.Equal(220m, checkoutCart.TotalPrice);
    }

    private static CheckoutCart CreateCheckoutCart(decimal price = 100m, int quantity = 1)
    {
        var product = new Product("Test product", "Description", Category.Clothes, price, 10)
        {
            Id = 1
        };
        var cart = new Cart("user-1");
        cart.AddProduct(product, quantity);

        return cart.Checkout();
    }

    private static Shipment CreateShipment(ShipmentMethod method = ShipmentMethod.InPost)
    {
        return new Shipment("Warsaw", "Main", 1, "Jan Kowalski", method);
    }
}
