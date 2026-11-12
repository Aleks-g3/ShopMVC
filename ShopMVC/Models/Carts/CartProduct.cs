using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Carts;

public class CartProduct : UpdatableEntity
{
    public long ProductId { get; private set; }
    public virtual Product Product { get; private set; } = null!;
    public int Quantity { get; private set; }

    private CartProduct()
    {
    }

    public CartProduct(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        if (quantity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        Quantity = quantity;

        var now = DateTime.UtcNow;
        
        CreatedOn = now;
        ModifiedOn = now;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity < 1)
        {
            throw new InvalidCartProductQuantityException();
        }

        Quantity += quantity;
    }

    public void DecreaseQuantity()
    {
        if (Quantity <= 1)
        {
            throw new InvalidCartProductQuantityException();
        }

        Quantity--;
    }

    public void KeepOnlyProductId()
    {
        if (Product is null)
            return;

        ProductId = Product.Id;
        Product = null!;
    }
}