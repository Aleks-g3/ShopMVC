using Microsoft.EntityFrameworkCore;
using ShopMVC.Models;
using ShopMVC.Models.Exceptions;

namespace ShopMVC.Data.Repositories;

public abstract class UpdatableRepository<T> : ReadOnlyRepository<T>, IUpdatableRepository<T>
    where T : UpdatableEntity
{
    protected UpdatableRepository(DbContext dbContext)
        : base(dbContext)
    {
    }

    public virtual async Task Create(T item, bool saveChanges = true)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        
        var now = DateTime.UtcNow;

        item.CreatedOn = now;
        item.ModifiedOn = now;

        await Entities.AddAsync(item);

        if (saveChanges)
        {
            try
            {
                await SaveChangesAsync();
            }
            finally
            {
                DbContext.Entry(item).State = EntityState.Detached;
            }
        }
    }

    public async Task DeleteById(long id)
    {
        var entity = await Entities.FindAsync(id);
        if (entity is null)
            return;

        if (DbContext.Entry(entity).State == EntityState.Detached)
            DbContext.Attach(entity);

        DbContext.Remove(entity);

        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await DbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw CreateConcurrencyException(exception);
        }
        finally
        {
            foreach (var entry in DbContext.ChangeTracker.Entries())
                entry.State = EntityState.Detached;
        }
    }

    public virtual async Task Update(T item, bool saveChanges = true)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        item.ModifiedOn = DateTime.UtcNow;
        Entities.Update(item);
        DbContext.Entry(item).Property(o => o.CreatedOn).IsModified = false;

        if (!saveChanges)
            return;

        await SaveChangesAsync();
    }

    private static ConcurrencyConflictException CreateConcurrencyException(DbUpdateConcurrencyException exception)
    {
        var conflictingEntities = exception.Entries
            .Select(entry => entry.Entity is UpdatableEntity updatableEntity
                ? $"{entry.Entity.GetType().Name}#{updatableEntity.Id}"
                : entry.Entity.GetType().Name);

        return new ConcurrencyConflictException(
            $"The data was modified by another user. Conflicting entities: {string.Join(", ", conflictingEntities)}.",
            exception);
    }
}
