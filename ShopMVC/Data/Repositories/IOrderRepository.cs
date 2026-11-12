using ShopMVC.Models.Orders;

namespace ShopMVC.Data.Repositories;

public interface IOrderRepository : IUpdatableRepository<Order>
{
    Task<Order?> GetById(long orderId);
    Task<Order[]> GetByUserId(string userId);
}
