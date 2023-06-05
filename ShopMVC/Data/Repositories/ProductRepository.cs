using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Shared;

namespace ShopMVC.Data.Repositories;

public class ProductRepository : UpdateableRepository<Product>, IProductRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductRepository(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor) : base(applicationDbContext)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public new async Task Create(Product item, bool saveChanges = true)
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity!.Name!;
        item.SetUser(username);
        await base.Create(item, saveChanges);
    }

    public new async Task Update(Product item, bool saveChanges = true)
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity!.Name!;
        item.SetUser(username);
        await base.Update(item, saveChanges);
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