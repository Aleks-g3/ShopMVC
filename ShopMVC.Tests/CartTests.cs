using ShopMVC.Models.Carts;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Tests;

public class CartTests
{
    [Fact]
    public void AddProduct_NewProduct_AddsLine()
    {
        var cart = new Cart("user-1");
        var product = CreateProduct();

        cart.AddProduct(product, 2);

        var line = Assert.Single(cart.CartProducts);
        Assert.Equal(product, line.Product);
        Assert.Equal(2, line.Quantity);
    }

    [Fact]
    public void AddProduct_ExistingProduct_IncreasesQuantity()
    {
        var cart = new Cart("user-1");
        var product = CreateProduct();
        cart.AddProduct(product, 2);

        cart.AddProduct(product, 3);

        var line = Assert.Single(cart.CartProducts);
        Assert.Equal(5, line.Quantity);
    }

    [Fact]
    public void AddProduct_NewProductExceedingStock_Throws()
    {
        var cart = new Cart("user-1");
        var product = CreateProduct(availableQuantity: 2);

        Assert.Throws<MaximumQuantityLimitExceededException>(() => cart.AddProduct(product, 3));
    }

    [Fact]
    public void AddProduct_ExistingLineExceedingStock_Throws()
    {
        var cart = new Cart("user-1");
        var product = CreateProduct(availableQuantity: 3);
        cart.AddProduct(product, 2);

        Assert.Throws<MaximumQuantityLimitExceededException>(() => cart.AddProduct(product, 2));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddProduct_NonPositiveQuantity_Throws(int quantity)
    {
        var cart = new Cart("user-1");

        Assert.Throws<InvalidCartProductQuantityException>(() => cart.AddProduct(CreateProduct(), quantity));
    }

    [Fact]
    public void RemoveProduct_QuantityGreaterThanOne_DecreasesQuantity()
    {
        var cart = new Cart("user-1");
        var product = CreateProduct();
        cart.AddProduct(product, 2);

        cart.RemoveProduct(product);

        var line = Assert.Single(cart.CartProducts);
        Assert.Equal(1, line.Quantity);
    }

    [Fact]
    public void RemoveProduct_LastItem_RemovesLine()
    {
        var cart = new Cart("user-1");
        var product = CreateProduct();
        cart.AddProduct(product, 1);

        cart.RemoveProduct(product);

        Assert.Empty(cart.CartProducts);
    }

    [Fact]
    public void RemoveProduct_NotInCart_Throws()
    {
        var cart = new Cart("user-1");

        Assert.Throws<CartProductNotFoundException>(() => cart.RemoveProduct(CreateProduct()));
    }

    [Fact]
    public void Checkout_EmptyCart_Throws()
    {
        var cart = new Cart("user-1");

        Assert.Throws<CannotCheckoutEmptyCartException>(() => cart.Checkout());
    }

    private static Product CreateProduct(long id = 1, decimal price = 100m, int availableQuantity = 10)
    {
        return new Product("Test product", "Description", Category.Clothes, price, availableQuantity)
        {
            Id = id
        };
    }
}
