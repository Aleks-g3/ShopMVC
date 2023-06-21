using ShopMVC.Models.Carts;
using ShopMVC.Models.Shared;

namespace ShopMVC.Services;

public interface ICartService
{
    Task Upsert(long productId, int quantity);
}