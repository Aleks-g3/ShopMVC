using ShopMVC.Models.Carts;

namespace ShopMVC.Data.Repositories;

public interface ICartRepository : IUpdateableRepository<Cart>
{
    Task<Cart?> GetByUserId(string userId);
}