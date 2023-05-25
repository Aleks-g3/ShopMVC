using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public interface IProductService
{
    Task<ProductViewModel[]> GetAll();
    Task<SimpleProductViewModel[]> GetAllSimpleProducts();
    Task Create(Product newProduct);
}