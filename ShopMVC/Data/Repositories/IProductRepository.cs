using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Data.Repositories;

public interface IProductRepository : IUpdateableRepository<Product>
{
    Task<Product?> GetById(long productId);
    Task<bool> IsExist(string name);
    Task<bool> IsExistWithNameForOtherProductIds(long productId, string name);
    Task<Product[]> GetAllWithOrderDescByModifiedOn();
}