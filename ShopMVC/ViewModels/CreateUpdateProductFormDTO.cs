using System.ComponentModel.DataAnnotations;
using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class CreateUpdateProductFormDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required] 
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
    
    public static CreateUpdateProductFormDTO Create(Product product)
    {
        return new CreateUpdateProductFormDTO
        {
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            Price = product.Price,
            AvailableQuantity = product.AvailableQuantity,
        };
    }
}