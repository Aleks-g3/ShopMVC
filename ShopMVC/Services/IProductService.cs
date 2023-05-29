using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public interface IProductService
{
    Task<UpdatableProductViewModel[]> GetAll();
    Task<SimpleProductViewModel[]> GetSimpleProducts();
    Task Create(Product newProduct);
}