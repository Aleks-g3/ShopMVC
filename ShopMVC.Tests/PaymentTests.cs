using ShopMVC.Models.Shared;

namespace ShopMVC.Tests;

public class PaymentTests
{
    [Fact]
    public void Constructor_WithEnum_SetsPaymentMethod()
    {
        var payment = new Payment(PaymentMethod.Loan);

        Assert.Equal(PaymentMethod.Loan, payment.PaymentMethod);
    }

    [Theory]
    [InlineData("Cash", PaymentMethod.Cash)]
    [InlineData("cashless", PaymentMethod.Cashless)]
    [InlineData("LOAN", PaymentMethod.Loan)]
    public void Constructor_WithString_ParsesCaseInsensitively(string value, PaymentMethod expected)
    {
        var payment = new Payment(value);

        Assert.Equal(expected, payment.PaymentMethod);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Bitcoin")]
    [InlineData("999")]
    public void Constructor_WithInvalidString_Throws(string value)
    {
        Assert.Throws<ArgumentException>(() => new Payment(value));
    }

    [Fact]
    public void Constructor_WithUndefinedEnum_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Payment((PaymentMethod)999));
    }

    [Fact]
    public void Equals_ComparesByPaymentMethod()
    {
        var first = new Payment(PaymentMethod.Cash);
        var second = new Payment("cash");
        var third = new Payment(PaymentMethod.Cashless);

        Assert.Equal(first, second);
        Assert.NotEqual(first, third);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsEnumName()
    {
        var payment = new Payment(PaymentMethod.Cashless);

        Assert.Equal("Cashless", payment.ToString());
    }
}
