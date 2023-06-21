using Microsoft.AspNetCore.Mvc;
using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("cart/addproduct/{productId}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> AddProduct(long productId, int quantity)
    {
        await _cartService.Upsert(productId, quantity);
        return RedirectToAction("Index", "Home");
    }
}