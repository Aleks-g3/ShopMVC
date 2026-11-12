namespace ShopMVC.Models.Exceptions;

public class InsufficientStockException : Exception
{
    public InsufficientStockException(string productName, int requestedQuantity, int availableQuantity)
        : base($"Product '{productName}' has only {availableQuantity} item(s) available, but {requestedQuantity} were requested.")
    {
    }
}
