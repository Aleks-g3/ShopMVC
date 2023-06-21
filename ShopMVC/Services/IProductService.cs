using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public interface IProductService
{
    Task<UpdatableProductViewModel[]> GetAll();
    Task<SimpleProductViewModel[]> GetSimpleProducts();
    Task<ProductWithDetailsViewModel?> GetWithDetailsById(long productId);
    Task<CreateUpdateProductFormDTO?> GetFormById(long productId);
    Task Create(Product newProduct);
    Task Update(long productId, Product existingProduct);
    Task Delete(long productId);
}