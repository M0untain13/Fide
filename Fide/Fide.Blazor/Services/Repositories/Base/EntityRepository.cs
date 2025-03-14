using Fide.Blazor.Data;
using Fide.Blazor.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace Fide.Blazor.Services.Repositories.Base;

public abstract class EntityRepository<T>(ApplicationDbContext context) : IRepository<T>
    where T : Entity
{
    protected abstract DbSet<T> DbSet { get; }

    public T Get(Guid id)
    {
        return DbSet.First(s => s.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return DbSet;
    }

    public void Create(T item)
    {
        DbSet.Add(item);
    }

    public void Update(T item)
    {
        context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(Guid id)
    {
        var item = Get(id);
        DbSet.Remove(item);
    }

    public void Save()
    {
        context.SaveChanges();
    }

    private bool _disposed = false;

    public virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

