using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Data.Repositories;

public interface IProductRepository : IUpdateableRepository<Product>
{
    Task<Product?> GetById(long productId);
}