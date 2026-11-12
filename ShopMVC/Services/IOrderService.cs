using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public interface IOrderService
{
    Task<long> PlaceOrder(CheckoutFormDTO form);
    Task<OrderSummaryViewModel[]> GetHistory();
    Task<OrderDetailsViewModel?> GetById(long orderId);
}
