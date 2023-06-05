using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopMVC.Models;

namespace ShopMVC.Data.Repositories;

public abstract class UpdateableRepository<T> : ReadOnlyRepository<T>, IUpdateableRepository<T>
    where T : UpdatableEntity
{
    protected UpdateableRepository(DbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task Create(T item, bool saveChanges = true)
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
        catch (DbUpdateConcurrencyException exception) when (exception.Entries is not null)
        {
            HandleConcurrencyException(exception);
        }
        finally
        {
            foreach (var entry in DbContext.ChangeTracker.Entries())
                entry.State = EntityState.Detached;
        }
    }

    private static void HandleConcurrencyException(DbUpdateException e)
    {
        StringBuilder outputLines = new();
        foreach (var entityEntry in e.Entries)
        {
            if (entityEntry.Entity is not UpdatableEntity updatableEntity)
                continue;

            outputLines.AppendLine(
                $"DbUpdateConcurrencyException: Item \"{entityEntry.Entity.GetType().Name}\" Id= \"{updatableEntity.Id} \", ModifiedOn= \"{updatableEntity.ModifiedOn}\"");
        }

        throw new Exception(outputLines.ToString(), e);
    }

    public async Task Update(T item, bool saveChanges = true)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        item.ModifiedOn = DateTime.UtcNow;
        Entities.Update(item);
        try
        {
            DbContext.Entry(item).Property(o => o.CreatedOn).IsModified = false;
            await DbContext.SaveChangesAsync();
        }
        finally
        {
            DbContext.Entry(item).State = EntityState.Detached;
        }
    }
}