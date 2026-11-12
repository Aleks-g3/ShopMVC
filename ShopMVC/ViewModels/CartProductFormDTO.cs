using System.ComponentModel.DataAnnotations;

namespace ShopMVC.ViewModels;

public class CartProductFormDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}
