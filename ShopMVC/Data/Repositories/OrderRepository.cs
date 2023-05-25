using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Orders;

namespace ShopMVC.Data.Repositories;

public class OrderRepository : UpdateableRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public Task<Order?> GetById(long orderId)
    {
        return GetAll().FirstOrDefaultAsync(o => o.Id == orderId);
    }
}