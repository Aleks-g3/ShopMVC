using System.ComponentModel.DataAnnotations;
using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class CheckoutFormDTO
{
    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string StreetName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Street number must be greater than 0")]
    public int StreetNumber { get; set; }

    [Required]
    [StringLength(200)]
    public string ReceiverFullName { get; set; } = string.Empty;

    [EnumDataType(typeof(ShipmentMethod))]
    public ShipmentMethod ShipmentMethod { get; set; } = ShipmentMethod.InPost;

    [EnumDataType(typeof(PaymentMethod))]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
}
