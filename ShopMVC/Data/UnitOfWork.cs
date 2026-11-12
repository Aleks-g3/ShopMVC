using Microsoft.EntityFrameworkCore;
using ShopMVC.Data.Repositories;
using ShopMVC.Models.Exceptions;

namespace ShopMVC.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ProductRepository? _productRepository;
    private OrderRepository? _orderRepository;
    private CartRepository? _cartRepository;
    

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository
    {
        get
        {
            _productRepository ??= new ProductRepository(_context);
            return _productRepository;
        }
    }

    public IOrderRepository OrderRepository
    {
        get
        {
            _orderRepository ??= new OrderRepository(_context);
            return _orderRepository;
        }
    }

    public ICartRepository CartRepository
    {
        get
        {
            _cartRepository ??= new CartRepository(_context);
            return _cartRepository;
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyConflictException(
                "The data was modified by another user. Reload the page and try again.", exception);
        }
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        await action();
        await transaction.CommitAsync();
    }
}
