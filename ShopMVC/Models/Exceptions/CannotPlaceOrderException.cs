namespace ShopMVC.Models.Exceptions;

public class CannotPlaceOrderException : Exception
{
    public CannotPlaceOrderException()
        : base("Shipment and payment must be selected before placing an order.")
    {
    }
}
