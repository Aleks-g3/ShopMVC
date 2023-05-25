namespace ShopMVC.Data.Repositories;

public interface IReadOnlyRepository<out T>
{
    IQueryable<T> GetAll();
}