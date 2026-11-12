using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Orders;

public class OrderLine : UpdatableEntity
{
    public int OrderLineNumber { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public OrderLine()
    {
    }

    public OrderLine(int orderLineNumber, string name, decimal unitPrice, int quantity)
    {
        if (quantity < 1)
        {
            throw new InvalidOrderLineDataException();
        }

        if (unitPrice < 0)
        {
            throw new InvalidOrderLineDataException();
        }

        OrderLineNumber = orderLineNumber;
        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}