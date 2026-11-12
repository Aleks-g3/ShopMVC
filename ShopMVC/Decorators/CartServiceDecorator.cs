using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Decorators;

public class CartServiceDecorator : ICartService
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartServiceDecorator> _logger;

    public CartServiceDecorator(ICartService cartService, ILogger<CartServiceDecorator> logger)
    {
        _cartService = cartService;
        _logger = logger;
    }

    public async Task Upsert(long productId, int quantity)
    {
        _logger.LogInformation("Upserting product with id {ProductId} and quantity {Quantity} to cart", productId, quantity);
        await _cartService.Upsert(productId, quantity);
        _logger.LogInformation("Finished upserting product with id {ProductId} to cart", productId);
    }

    public Task<CartViewModel> GetCart()
    {
        _logger.LogInformation("Getting cart");
        return _cartService.GetCart();
    }

    public async Task RemoveProduct(long productId)
    {
        _logger.LogInformation("Removing product with id {ProductId} from cart", productId);
        await _cartService.RemoveProduct(productId);
        _logger.LogInformation("Finished removing product with id {ProductId} from cart", productId);
    }
}
