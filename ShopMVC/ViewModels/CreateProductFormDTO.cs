using System.ComponentModel.DataAnnotations;

namespace ShopMVC.ViewModels;

public class CreateProductFormDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required] 
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
}