namespace ShopMVC.Data.Repositories;

public interface IUpdatableRepository<T> : IReadOnlyRepository<T>
{
    Task DeleteById(long id);

    Task Update(T item, bool saveChanges = true);

    Task Create(T item, bool saveChanges = true);

    Task SaveChangesAsync();
}
