using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;

namespace ShopMVC.Models.Carts;

public class Cart
{
    private readonly List<CartProduct> _products = new();
    
    public string UserId { get; }
    public IReadOnlyCollection<CartProduct> Products => _products;
    
    public Cart(string userId)
    {
        UserId = userId;
    }
    
    public void AddProduct(Product product)
    {
        var cartProduct = _products.SingleOrDefault(x => x.Product.Id == product.Id);

        if (cartProduct is {Quantity: > 2})
        {
            throw new ExceededMaximumQuantityLimitException();
        }

        if (cartProduct is {Quantity: <= 2})
        {
            cartProduct.IncreaseQuantity();
            return;
        }

        _products.Add(new CartProduct(product, 1));
    }

    public void RemoveProduct(Product product)
    {
        var cartProduct = _products.SingleOrDefault(x => x.Product.Id == product.Id);

        if (cartProduct is null)
        {
            throw new CartProductNotFoundException();
        }

        cartProduct.DecreaseQuantity();
    }

    public void Clear() => _products.Clear();

    public CheckoutCart Checkout()
    {
        if (Products.Count == 0)
        {
            throw new CannotCheckoutEmptyCartException();
        }
        return new(this);
    }
}