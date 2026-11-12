using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopMVC.Models.Exceptions;
using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _orderService.GetHistory());
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var cart = await _cartService.GetCart();
        if (cart.IsEmpty)
        {
            TempData["ErrorMessage"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }

        return View(new CheckoutViewModel { Cart = cart });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel checkoutViewModel)
    {
        if (!ModelState.IsValid)
            return await CheckoutWithCart(checkoutViewModel);

        try
        {
            var orderId = await _orderService.PlaceOrder(checkoutViewModel.Form);
            TempData["SuccessMessage"] = "Order has been placed.";
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
        catch (CannotCheckoutEmptyCartException)
        {
            TempData["ErrorMessage"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }
        catch (PaymentMethodNotAllowedException)
        {
            ModelState.AddModelError("Form.PaymentMethod",
                "Cash payment is not available for orders above 20 000 PLN.");
        }
        catch (InvalidShipmentException)
        {
            ModelState.AddModelError(string.Empty, "Shipment data is invalid.");
        }
        catch (InsufficientStockException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
        }
        catch (ConcurrencyConflictException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
        }

        return await CheckoutWithCart(checkoutViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(long id)
    {
        var order = await _orderService.GetById(id);
        return order is null ? NotFound() : View(order);
    }

    private async Task<IActionResult> CheckoutWithCart(CheckoutViewModel checkoutViewModel)
    {
        checkoutViewModel.Cart = await _cartService.GetCart();
        if (checkoutViewModel.Cart.IsEmpty)
        {
            TempData["ErrorMessage"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }

        return View("Checkout", checkoutViewModel);
    }
}
