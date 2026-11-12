using System.ComponentModel.DataAnnotations;
using ShopMVC.Models.Exceptions;

namespace ShopMVC.Models.Shared;

public class Product : UpdatableEntity
{
    [Required] 
    public string Name { get; private set; } = string.Empty;
    [Required]
    public string Description { get; private set; } = string.Empty;
    public Category Category { get; private set; }
    public decimal Price { get; private set; }
    public int AvailableQuantity { get; private set; }
    [MaxLength(500)]
    public string? ImageUrl { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string ModifiedBy { get; private set; } = string.Empty;

    private Product()
    {
    }

    public Product(string name, string description,Category category, decimal price, int availableQuantity,
        string? imageUrl = null)
    {
        Validate(name, description, category, price, availableQuantity, imageUrl);

        Name = name;
        Description = description;
        Category = category;
        Price = price;
        AvailableQuantity = availableQuantity;
        ImageUrl = NormalizeImageUrl(imageUrl);
    }

    public void Update(Product updatedProduct)
    {
        Validate(updatedProduct.Name, updatedProduct.Description, updatedProduct.Category,
            updatedProduct.Price, updatedProduct.AvailableQuantity, updatedProduct.ImageUrl);

        Name = updatedProduct.Name;
        Description = updatedProduct.Description;
        Category = updatedProduct.Category;
        Price = updatedProduct.Price;
        AvailableQuantity = updatedProduct.AvailableQuantity;
        ImageUrl = NormalizeImageUrl(updatedProduct.ImageUrl);
    }

    public void UpsertUser(string userName)
    {
        if (string.IsNullOrWhiteSpace(CreatedBy))
            CreatedBy = userName;
        
        ModifiedBy = userName;
    }

    public void DecreaseAvailableQuantity(int quantity)
    {
        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        if (quantity > AvailableQuantity)
            throw new InsufficientStockException(Name, quantity, AvailableQuantity);

        AvailableQuantity -= quantity;
    }

    private static void Validate(string name, string description, Category category, decimal price,
        int availableQuantity, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (!Enum.IsDefined(category))
            throw new ArgumentException($"Category '{category}' is not supported", nameof(category));
        if (price <= 0)
            throw new ArgumentException("Price must be greater than 0", nameof(price));
        if (availableQuantity < 0)
            throw new ArgumentException("Available quantity cannot be negative", nameof(availableQuantity));
        if (!string.IsNullOrWhiteSpace(imageUrl) && imageUrl.Length > 500)
            throw new ArgumentException("Image URL cannot be longer than 500 characters", nameof(imageUrl));
        if (!string.IsNullOrWhiteSpace(imageUrl) && !IsSupportedImageUrl(imageUrl))
            throw new ArgumentException("Image URL must be an absolute http(s) URL or a relative path", nameof(imageUrl));
    }

    private static bool IsSupportedImageUrl(string imageUrl)
    {
        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;

        return Uri.TryCreate(imageUrl, UriKind.Relative, out _);
    }

    private static string? NormalizeImageUrl(string? imageUrl) =>
        string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim();
}
