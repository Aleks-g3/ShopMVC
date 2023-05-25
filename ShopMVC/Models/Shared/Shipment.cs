using ShopMVC.Models.Exceptions;

namespace ShopMVC.Models.Shared;

public class Shipment : UpdatableEntity
{
    public string City { get; } = string.Empty;
    public string StreetName { get; } = string.Empty;
    public int StreetNumber { get; }
    public string ReceiverFullName { get; }
    public ShipmentMethod ShipmentMethod { get; }

    public Shipment()
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

        if (string.IsNullOrWhiteSpace(receiverFullName))
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