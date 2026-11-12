using Microsoft.EntityFrameworkCore;
using ShopMVC.Models.Carts;

namespace ShopMVC.Data.Repositories;

public class CartRepository : UpdatableRepository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public override async Task Create(Cart cart, bool saveChanges = true)
    {
        ClearProductReferences(cart);
        await base.Create(cart, saveChanges);
    }

    public override async Task Update(Cart cart, bool saveChanges = true)
    {
        ClearProductReferences(cart);
        await base.Update(cart, saveChanges);
    }

    public Task<Cart?> GetByUserId(string userId)
    {
        return GetAll()
            .Include(cart => cart.CartProducts)
            .ThenInclude(cartProduct => cartProduct.Product)
            .FirstOrDefaultAsync(cart => cart.UserId == userId);
    }

    public async Task RemoveProducts(Cart cart, bool saveChanges = true)
    {
        ClearProductReferences(cart);
        DbContext.Set<CartProduct>().RemoveRange(cart.CartProducts);

        if (saveChanges)
            await SaveChangesAsync();
    }

    public async Task DeleteProduct(CartProduct cartProduct, bool saveChanges = true)
    {
        cartProduct.KeepOnlyProductId();
        DbContext.Set<CartProduct>().Remove(cartProduct);

        if (saveChanges)
            await SaveChangesAsync();
    }

    private static void ClearProductReferences(Cart cart)
    {
        foreach (var cartProduct in cart.CartProducts)
            cartProduct.KeepOnlyProductId();
    }
}
