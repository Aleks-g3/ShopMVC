using ShopMVC.Data.Repositories;

namespace ShopMVC.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ProductRepository _productRepository;
    private OrderRepository _orderRepository;
    private CartRepository _cartRepository;
    

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
}