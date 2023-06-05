namespace ShopMVC.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ProductRepository _productRepository;
    private OrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public IProductRepository ProductRepository
    {
        get
        {
            _productRepository ??= new ProductRepository(_context, _httpContextAccessor);
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