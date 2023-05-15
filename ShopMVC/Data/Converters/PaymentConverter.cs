using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShopMVC.Models.Shared;

namespace ShopMVC.Data.Converters;

public class PaymentConverter : ValueConverter<Payment,string>
{
    public PaymentConverter() : base(payment => payment.PaymentMethod.ToString(), value => new Payment(value))
    {
    }
}