using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Tests;

public class ShipmentTests
{
    [Fact]
    public void Constructor_WithValidData_SetsProperties()
    {
        var shipment = new Shipment("Warsaw", "Main", 12, "Jan Kowalski", ShipmentMethod.Courier);

        Assert.Equal("Warsaw", shipment.City);
        Assert.Equal("Main", shipment.StreetName);
        Assert.Equal(12, shipment.StreetNumber);
        Assert.Equal("Jan Kowalski", shipment.ReceiverFullName);
        Assert.Equal(ShipmentMethod.Courier, shipment.ShipmentMethod);
    }

    [Theory]
    [InlineData("", "Main", 1, "Jan")]
    [InlineData("Warsaw", "", 1, "Jan")]
    [InlineData("Warsaw", "Main", 0, "Jan")]
    [InlineData("Warsaw", "Main", 1, "")]
    public void Constructor_WithInvalidData_Throws(string city, string street, int number, string receiver)
    {
        Assert.Throws<InvalidShipmentException>(() =>
            new Shipment(city, street, number, receiver, ShipmentMethod.InPost));
    }

    [Fact]
    public void Constructor_WithUndefinedMethod_Throws()
    {
        Assert.Throws<InvalidShipmentException>(() =>
            new Shipment("Warsaw", "Main", 1, "Jan", (ShipmentMethod)999));
    }

    [Theory]
    [InlineData(ShipmentMethod.InPost, 10)]
    [InlineData(ShipmentMethod.Courier, 20)]
    public void ShippingCost_DependsOnMethod(ShipmentMethod method, decimal expectedCost)
    {
        var shipment = new Shipment("Warsaw", "Main", 1, "Jan", method);

        Assert.Equal(expectedCost, shipment.ShippingCost);
    }
}
