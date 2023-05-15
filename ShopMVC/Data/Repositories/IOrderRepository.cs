using ShopMVC.Models.Orders;

namespace ShopMVC.Data.Repositories;

public interface IOrderRepository
{
    Task Create(Order order);
    Task Update(Order order);
    Task<Order[]> GetAll();
    Task<Order?> GetById(long orderId);
    Task DeleteById(long orderId);
}