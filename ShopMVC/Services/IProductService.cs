using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public interface IProductService
{
    Task<SimpleProductViewModel[]> GetAllSimpleProducts();
    Task Create(Product newProduct);
}