using System.ComponentModel.DataAnnotations;
using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class CheckoutFormDTO
{
    [Required]
    [Display(Name = "City")]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Street name")]
    [StringLength(200)]
    public string StreetName { get; set; } = string.Empty;

    [Required(ErrorMessage = "House number is required")]
    [Display(Name = "House number")]
    [Range(1, int.MaxValue, ErrorMessage = "House number must be greater than 0")]
    public int? StreetNumber { get; set; }

    [Required]
    [Display(Name = "Recipient full name")]
    [StringLength(200)]
    public string ReceiverFullName { get; set; } = string.Empty;

    [Display(Name = "Delivery method")]
    [EnumDataType(typeof(ShipmentMethod))]
    public ShipmentMethod ShipmentMethod { get; set; } = ShipmentMethod.InPost;

    [Display(Name = "Payment method")]
    [EnumDataType(typeof(PaymentMethod))]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
}
