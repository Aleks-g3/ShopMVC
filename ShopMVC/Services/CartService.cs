using System.Security.Claims;
using ShopMVC.Data;
using ShopMVC.Models.Carts;
using ShopMVC.Models.Exceptions;
using ShopMVC.ViewModels;

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
        var userId = CurrentUserId;
        var cart = await _unitOfWork.CartRepository.GetByUserId(userId) ?? new Cart(userId);

        var product = await _unitOfWork.ProductRepository.GetById(productId);
        if (product is null) 
            throw new ArgumentException($"Product with id '{productId}' does not exist", nameof(productId));

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

    public async Task<CartViewModel> GetCart()
    {
        var cart = await _unitOfWork.CartRepository.GetByUserId(CurrentUserId);
        return CartViewModel.Create(cart);
    }

    public async Task RemoveProduct(long productId)
    {
        var cart = await _unitOfWork.CartRepository.GetByUserId(CurrentUserId);
        if (cart is null)
            throw new CartProductNotFoundException();

        var cartProduct = cart.CartProducts
            .FirstOrDefault(existingCartProduct => existingCartProduct.Product.Id == productId);

        if (cartProduct is null)
            throw new CartProductNotFoundException();

        if (cartProduct.Quantity == 1)
        {
            cart.CartProducts.Remove(cartProduct);
            await _unitOfWork.CartRepository.DeleteProduct(cartProduct);
            return;
        }

        cart.RemoveProduct(cartProduct.Product);
        await _unitOfWork.CartRepository.Update(cart);
    }

    private string CurrentUserId =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("Cannot determine the current user.");
}
