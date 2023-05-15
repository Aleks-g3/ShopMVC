using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Orders;

namespace ShopMVC.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }
    
    public async Task Create(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public Task<Order[]> GetAll()
    {
        return _context.Orders.ToArrayAsync();
    }

    public Task<Order?> GetById(long orderId)
    {
        return _context.Orders.SingleOrDefaultAsync(p => p.Id == orderId);
    }

    public async Task DeleteById(long orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order is null)
            return;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}