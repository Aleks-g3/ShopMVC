namespace ShopMVC.Data.Repositories;

public interface IUpdateableRepository<T> : IReadOnlyRepository<T>
{
    Task DeleteById(long id);

    Task Update(T item, bool saveChanges = true);

    Task Create(T item, bool saveChanges = true);

    Task SaveChangesAsync();
}