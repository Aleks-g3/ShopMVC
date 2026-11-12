using Microsoft.EntityFrameworkCore;
using ShopMVC.Models;

namespace ShopMVC.Data.Repositories;

public abstract class ReadOnlyRepository<T> : IReadOnlyRepository<T>
    where T : UpdatableEntity
{
    protected DbContext DbContext { get; }

    protected ReadOnlyRepository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    protected DbSet<T> Entities => DbContext.Set<T>();

    public IQueryable<T> GetAll() => Entities.AsNoTracking();
}