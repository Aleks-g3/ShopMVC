namespace ShopMVC.Models.Shared;

public class Payment : UpdatableEntity
{
    public PaymentMethod PaymentMethod { get; private set; }

    public Payment()
    {
    }
        
    public Payment(string paymentMethod)
    {
        PaymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), paymentMethod);
    }
}

public enum PaymentMethod
{
    Cash,
    Cashless,
    Loan
}