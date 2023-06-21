using System.Security.Claims;
using ShopMVC.Data;
using ShopMVC.Models.Carts;

namespace ShopMVC.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task Upsert(long productId, int quantity)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value!;
        var cart = await _unitOfWork.CartRepository.GetByUserId(userId) ?? new Cart(userId);

        var product = await _unitOfWork.ProductRepository.GetById(productId);
        if (product is null) 
            throw new ArgumentException($"Product with id '{productId}' not exist");

        cart.AddProduct(product, quantity);
        if (cart.IsTransient)
        {
            await _unitOfWork.CartRepository.Create(cart);
        }
        else
        {
            await _unitOfWork.CartRepository.Update(cart);
        }
    }
}