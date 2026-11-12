using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Orders;

namespace ShopMVC.Data.Repositories;

public class OrderRepository : UpdatableRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public Task<Order?> GetById(long orderId)
    {
        return GetAll()
            .Include(order => order.Lines)
            .Include(order => order.Shipment)
            .FirstOrDefaultAsync(order => order.Id == orderId);
    }

    public Task<Order[]> GetByUserId(string userId)
    {
        return GetAll()
            .Include(order => order.Lines)
            .Include(order => order.Shipment)
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.CreatedOn)
            .ToArrayAsync();
    }
}
