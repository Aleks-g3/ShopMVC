using ShopMVC.Data.Repositories;

namespace ShopMVC.Data;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    IOrderRepository OrderRepository { get; }
}