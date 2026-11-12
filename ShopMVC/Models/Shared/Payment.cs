namespace ShopMVC.Models.Shared;

public sealed class Payment : IEquatable<Payment>
{
    public PaymentMethod PaymentMethod { get; }

    public Payment(PaymentMethod paymentMethod)
    {
        if (!Enum.IsDefined(paymentMethod))
            throw new ArgumentException($"Payment method '{paymentMethod}' is not supported", nameof(paymentMethod));

        PaymentMethod = paymentMethod;
    }

    public Payment(string paymentMethod)
        : this(Parse(paymentMethod))
    {
    }

    public bool Equals(Payment? other) =>
        other is not null && PaymentMethod == other.PaymentMethod;

    public override bool Equals(object? obj) => Equals(obj as Payment);

    public override int GetHashCode() => PaymentMethod.GetHashCode();

    public override string ToString() => PaymentMethod.ToString();

    private static PaymentMethod Parse(string value)
    {
        if (!Enum.TryParse<PaymentMethod>(value, ignoreCase: true, out var paymentMethod) ||
            !Enum.IsDefined(paymentMethod))
        {
            throw new ArgumentException($"Payment method '{value}' is not supported", nameof(value));
        }

        return paymentMethod;
    }
}

public enum PaymentMethod
{
    Cash,
    Cashless,
    Loan
}
