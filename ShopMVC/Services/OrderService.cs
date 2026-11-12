using System.Security.Claims;
using ShopMVC.Data;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Orders;
using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<long> PlaceOrder(CheckoutFormDTO form)
    {
        var cart = await _unitOfWork.CartRepository.GetByUserId(CurrentUserId);
        if (cart is null || cart.CartProducts.Count == 0)
            throw new CannotCheckoutEmptyCartException();

        var shipment = new Shipment(form.City, form.StreetName, form.StreetNumber,
            form.ReceiverFullName, form.ShipmentMethod);
        var payment = new Payment(form.PaymentMethod);

        var checkoutCart = cart.Checkout();
        checkoutCart.SetShipment(shipment);
        checkoutCart.SetPayment(payment);

        var order = checkoutCart.PlaceOrder();

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            foreach (var cartProduct in cart.CartProducts)
            {
                var product = cartProduct.Product;
                product.DecreaseAvailableQuantity(cartProduct.Quantity);
                await _unitOfWork.ProductRepository.Update(product, saveChanges: false);
            }

            await _unitOfWork.OrderRepository.Create(order, saveChanges: false);
            await _unitOfWork.CartRepository.RemoveProducts(cart, saveChanges: false);
            await _unitOfWork.SaveChangesAsync();
        });

        return order.Id;
    }

    public async Task<OrderSummaryViewModel[]> GetHistory()
    {
        var orders = await _unitOfWork.OrderRepository.GetByUserId(CurrentUserId);
        return orders.Select(OrderSummaryViewModel.Create).ToArray();
    }

    public async Task<OrderDetailsViewModel?> GetById(long orderId)
    {
        var order = await _unitOfWork.OrderRepository.GetById(orderId);
        if (order is null || !CanAccess(order))
            return null;

        return OrderDetailsViewModel.Create(order);
    }

    private bool CanAccess(Order order) =>
        order.UserId == CurrentUserId || IsAdmin;

    private bool IsAdmin =>
        _httpContextAccessor.HttpContext?.User.IsInRole("Admin") == true;

    private string CurrentUserId =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("Cannot determine the current user.");
}
