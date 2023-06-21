using Microsoft.AspNetCore.Identity;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Carts;

public class Cart : UpdatableEntity
{
    private readonly List<CartProduct> _cartProducts = new();
    
    public string UserId { get; }
    public virtual IdentityUser User { get; private set; }
    
    public List<CartProduct> CartProducts => _cartProducts;
    
    private Cart(){}
    
    public Cart(string userId)
    {
        UserId = userId;
    }
    
    public void AddProduct(Product product, int quantity)
    {
        var cartProduct = _cartProducts.SingleOrDefault(x => x.Product.Id == product.Id);

        if (cartProduct is not null && cartProduct.Quantity + quantity > cartProduct.Product.AvailableQuantity)
        {
            throw new ExceededMaximumQuantityLimitException();
        }

        if (cartProduct is not null)
        {
            cartProduct.IncreaseQuantity(quantity);
            cartProduct.ModifiedOn =DateTime.UtcNow;
            return;
        }

        _cartProducts.Add(new CartProduct(product, quantity));
    }

    public void RemoveProduct(Product product)
    {
        var cartProduct = _cartProducts.SingleOrDefault(x => x.Product.Id == product.Id);

        if (cartProduct is null)
        {
            throw new CartProductNotFoundException();
        }

        cartProduct.DecreaseQuantity();
    }

    public void Clear() => _cartProducts.Clear();

    public CheckoutCart Checkout()
    {
        if (CartProducts.Count == 0)
        {
            throw new CannotCheckoutEmptyCartException();
        }
        return new(this);
    }
}