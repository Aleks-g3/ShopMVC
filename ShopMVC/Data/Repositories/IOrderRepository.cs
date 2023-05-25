using ShopMVC.Models.Orders;

namespace ShopMVC.Data.Repositories;

public interface IOrderRepository : IUpdateableRepository<Order>
{
    Task<Order?> GetById(long orderId);
}