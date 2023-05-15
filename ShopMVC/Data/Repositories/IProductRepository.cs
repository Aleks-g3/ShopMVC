using ShopMVC.Models.Shared;

namespace ShopMVC.Data.Repositories;

public interface IProductRepository
{
    Task Create(Product product);
    Task Update(Product product);
    Task<Product[]> GetAll();
    Task<Product?> GetById(long productId);
    Task DeleteById(long productId);
}