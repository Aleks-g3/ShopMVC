using ShopMVC.Models.Carts;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Orders;
using ShopMVC.Models.Shared;

namespace ShopMVC.Tests;

public class OrderTests
{
    [Fact]
    public void CreateFromCheckout_SetsPlacedStatusAndUser()
    {
        var order = CreateOrder();

        Assert.Equal(OrderStatus.Placed, order.Status);
        Assert.Equal("user-1", order.UserId);
        Assert.False(order.IsCompleted);
    }

    [Fact]
    public void TotalPrice_SumsAllLines()
    {
        var order = CreateOrder();

        Assert.Equal(110m, order.TotalPrice);
    }

    [Fact]
    public void Complete_ChangesStatusToCompleted()
    {
        var order = CreateOrder();

        order.Complete();

        Assert.Equal(OrderStatus.Completed, order.Status);
        Assert.True(order.IsCompleted);
    }

    [Fact]
    public void Complete_WhenCanceled_Throws()
    {
        var order = CreateOrder();
        order.Cancel();

        Assert.Throws<InvalidOrderStatusChangeException>(() => order.Complete());
    }

    [Fact]
    public void Cancel_ChangesStatusToCanceled()
    {
        var order = CreateOrder();

        order.Cancel();

        Assert.Equal(OrderStatus.Canceled, order.Status);
        Assert.True(order.IsCompleted);
    }

    [Fact]
    public void Cancel_WhenCompleted_Throws()
    {
        var order = CreateOrder();
        order.Complete();

        Assert.Throws<InvalidOrderStatusChangeException>(() => order.Cancel());
    }

    private static Order CreateOrder()
    {
        var product = new Product("Test product", "Description", Category.Clothes, 100m, 10)
        {
            Id = 1
        };
        var cart = new Cart("user-1");
        cart.AddProduct(product, 1);

        var checkoutCart = cart.Checkout();
        checkoutCart.SetShipment(new Shipment("Warsaw", "Main", 1, "Jan Kowalski", ShipmentMethod.InPost));
        checkoutCart.SetPayment(new Payment(PaymentMethod.Cashless));

        return checkoutCart.PlaceOrder();
    }
}
