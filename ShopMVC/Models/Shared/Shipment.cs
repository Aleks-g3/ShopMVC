using System.ComponentModel.DataAnnotations.Schema;
using ShopMVC.Models.Exceptions;

namespace ShopMVC.Models.Shared;

public class Shipment : UpdatableEntity
{
    public string City { get; private set; } = string.Empty;
    public string StreetName { get; private set; } = string.Empty;
    public int StreetNumber { get; private set; }
    public string ReceiverFullName { get; private set; } = string.Empty;
    public ShipmentMethod ShipmentMethod { get; private set; }

    [NotMapped]
    public decimal ShippingCost => ShipmentMethod switch
    {
        ShipmentMethod.InPost => 10m,
        ShipmentMethod.Courier => 20m,
        _ => throw new ArgumentOutOfRangeException(nameof(ShipmentMethod), ShipmentMethod, "Unsupported shipment method")
    };

    private Shipment()
    {
    }

    public Shipment(string city, string streetName, int streetNumber, string receiverFullName,
        ShipmentMethod shipmentMethod)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new InvalidShipmentException();
        }

        if (string.IsNullOrWhiteSpace(streetName))
        {
            throw new InvalidShipmentException();
        }

        if (streetNumber < 1)
        {
            throw new InvalidShipmentException();
        }

        if (string.IsNullOrWhiteSpace(receiverFullName))
        {
            throw new InvalidShipmentException();
        }

        if (!Enum.IsDefined(shipmentMethod))
        {
            throw new InvalidShipmentException();
        }

        City = city;
        StreetName = streetName;
        StreetNumber = streetNumber;
        ReceiverFullName = receiverFullName;
        ShipmentMethod = shipmentMethod;
    }
}

public enum ShipmentMethod
{
    InPost = 1,
    Courier = 2
}
