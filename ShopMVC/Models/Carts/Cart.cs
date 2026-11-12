using Microsoft.AspNetCore.Identity;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Carts;

public class Cart : UpdatableEntity
{
    private readonly List<CartProduct> _cartProducts = new();
    
    public string UserId { get; } = null!;
    public virtual IdentityUser User { get; private set; } = null!;
    
    public List<CartProduct> CartProducts => _cartProducts;
    
    private Cart(){}
    
    public Cart(string userId)
    {
        UserId = userId;
    }
    
    public void AddProduct(Product product, int quantity)
    {
        if (quantity < 1)
            throw new InvalidCartProductQuantityException();

        var cartProduct = _cartProducts.SingleOrDefault(existingCartProduct => existingCartProduct.Product.Id == product.Id);

        if (cartProduct is null)
        {
            if (quantity > product.AvailableQuantity)
                throw new MaximumQuantityLimitExceededException();

            _cartProducts.Add(new CartProduct(product, quantity));
            return;
        }

        if (cartProduct.Quantity + quantity > product.AvailableQuantity)
            throw new MaximumQuantityLimitExceededException();

        cartProduct.IncreaseQuantity(quantity);
        cartProduct.ModifiedOn = DateTime.UtcNow;
    }

    public void RemoveProduct(Product product)
    {
        var cartProduct = _cartProducts.SingleOrDefault(existingCartProduct => existingCartProduct.Product.Id == product.Id);

        if (cartProduct is null)
        {
            throw new CartProductNotFoundException();
        }

        if (cartProduct.Quantity == 1)
        {
            _cartProducts.Remove(cartProduct);
            return;
        }

        cartProduct.DecreaseQuantity();
        cartProduct.ModifiedOn = DateTime.UtcNow;
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
