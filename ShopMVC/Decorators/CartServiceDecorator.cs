using ShopMVC.Services;

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
        _logger.LogInformation($"Upsert product with id '{productId}' to cart");
        await _cartService.Upsert(productId, quantity);
        _logger.LogInformation($"Upsert product with id '{productId}' to cart finished");
    }
}