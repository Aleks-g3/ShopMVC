using System.ComponentModel.DataAnnotations;
using ShopMVC.Models.Shared;

namespace ShopMVC.ViewModels;

public class CreateUpdateProductFormDTO
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "Please select a category")]
    [EnumDataType(typeof(Category))]
    public Category? Category { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Available quantity cannot be negative")]
    [Display(Name = "Available quantity")]
    public int AvailableQuantity { get; set; }
    [Display(Name = "Image URL")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }
    public string? RowVersion { get; set; }
    
    public static CreateUpdateProductFormDTO Create(Product product)
    {
        return new CreateUpdateProductFormDTO
        {
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            Price = product.Price,
            AvailableQuantity = product.AvailableQuantity,
            ImageUrl = product.ImageUrl,
            RowVersion = Convert.ToBase64String(product.RowVersion)
        };
    }
}
