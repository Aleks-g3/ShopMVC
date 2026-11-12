namespace ShopMVC.ViewModels;

public class CheckoutViewModel
{
    public CheckoutFormDTO Form { get; set; } = new();
    public CartViewModel Cart { get; set; } = CartViewModel.Empty;
}
