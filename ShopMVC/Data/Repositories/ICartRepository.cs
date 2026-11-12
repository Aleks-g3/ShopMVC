using ShopMVC.Models.Carts;

namespace ShopMVC.Data.Repositories;

public interface ICartRepository : IUpdatableRepository<Cart>
{
    Task<Cart?> GetByUserId(string userId);
    Task RemoveProducts(Cart cart, bool saveChanges = true);
    Task DeleteProduct(CartProduct cartProduct, bool saveChanges = true);
}
