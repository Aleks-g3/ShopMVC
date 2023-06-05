using System.ComponentModel.DataAnnotations;
using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class UpdateProductFormDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required] 
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
    
    public static UpdateProductFormDTO Create(Product product)
    {
        return new UpdateProductFormDTO
        {
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            AvailableQuantity = product.AvailableQuantity,
        };
    }
}