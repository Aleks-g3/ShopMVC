namespace ShopMVC.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ProductRepository _productRepository;
    private OrderRepository _orderRepository;

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
}