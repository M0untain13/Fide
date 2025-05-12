using Fide.Blazor.Services.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Fide.Blazor.Services.Data.UnitOfWork;

public class UnitOfWork(DbContext context) : IUnitOfWork
{
    public IRepository<T> GetRepository<T>() where T : class
        => new Repository<T>(context);

    public async Task<int> CommitAsync()
        => await context.SaveChangesAsync();

    public void Rollback()
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.Reload();
                    break;
            }
        }
    }

    #region IDisposable Implementation
    private bool _isDisposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
