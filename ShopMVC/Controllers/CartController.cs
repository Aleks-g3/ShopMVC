using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopMVC.Models.Exceptions;
using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _cartService.GetCart());
    }

    [HttpPost("cart/addproduct/{productId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct(long productId, [FromForm] CartProductFormDTO cartProductFormDto)
    {
        if (!ModelState.IsValid)
            return RedirectToProductDetails(productId, "Quantity must be greater than 0.");

        try
        {
            await _cartService.Upsert(productId, cartProductFormDto.Quantity);
        }
        catch (MaximumQuantityLimitExceededException)
        {
            return RedirectToProductDetails(productId, "Requested quantity exceeds the available stock.");
        }
        catch (InvalidCartProductQuantityException)
        {
            return RedirectToProductDetails(productId, "Quantity must be greater than 0.");
        }
        catch (ArgumentException)
        {
            return RedirectToProductDetails(productId, "Product does not exist.");
        }
        catch (ConcurrencyConflictException)
        {
            return RedirectToProductDetails(productId, "The cart was modified in another session. Try again.");
        }

        TempData["SuccessMessage"] = "Product added to cart.";
        return RedirectToProductDetails(productId);
    }

    [HttpPost("cart/removeproduct/{productId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveProduct(long productId)
    {
        try
        {
            await _cartService.RemoveProduct(productId);
            TempData["SuccessMessage"] = "Product removed from cart.";
        }
        catch (CartProductNotFoundException)
        {
            TempData["ErrorMessage"] = "Product is not in the cart.";
        }
        catch (ConcurrencyConflictException)
        {
            TempData["ErrorMessage"] = "The cart was modified in another session. Try again.";
        }

        return RedirectToAction(nameof(Index));
    }

    private IActionResult RedirectToProductDetails(long productId, string? errorMessage = null)
    {
        if (errorMessage is not null)
            TempData["ErrorMessage"] = errorMessage;

        return RedirectToAction("GetWithDetailsById", "Product", new { productId });
    }
}
