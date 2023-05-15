using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Shared;

namespace ShopMVC.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }
    
    public async Task Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public Task<Product[]> GetAll()
    {
        return _context.Products.ToArrayAsync();
    }

    public Task<Product?> GetById(long productId)
    {
        return _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
    }

    public async Task DeleteById(long productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product is null)
            return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}