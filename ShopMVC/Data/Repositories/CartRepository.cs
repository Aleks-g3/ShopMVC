using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Carts;
using ShopMVC.Models.Shared;

namespace ShopMVC.Data.Repositories;

public class CartRepository :  UpdateableRepository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public new async Task Create(Cart item, bool saveChanges = true)
    {
        item.CartProducts[0].SetOnlyProductId();
        await base.Create(item, saveChanges);
    }

    public Task<Cart?> GetByUserId(string userId)
    {
        return GetAll()
            .Include(c => c.CartProducts)
            .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}