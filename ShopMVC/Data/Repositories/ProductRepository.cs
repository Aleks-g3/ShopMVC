using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Shared;

namespace ShopMVC.Data.Repositories;

public class ProductRepository : UpdateableRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
    
    public Task<Product?> GetById(long productId)
    {
        return GetAll().FirstOrDefaultAsync(p => p.Id == productId);
    }

    public Task<bool> IsExist(string name)
    {
        return GetAll().AnyAsync(p => p.Name == name);
    }

    public Task<bool> IsExistWithNameForOtherProductIds(long productId, string name)
    {
        return GetAll().AnyAsync(p => p.Id != productId && p.Name == name);
    }

    public Task<Product[]> GetAllWithOrderDescByModifiedOn()
    {
        return GetAll().OrderByDescending(p => p.ModifiedOn).ToArrayAsync();
    }
}