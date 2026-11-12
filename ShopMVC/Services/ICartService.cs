using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public interface ICartService
{
    Task Upsert(long productId, int quantity);
    Task<CartViewModel> GetCart();
    Task RemoveProduct(long productId);
}
