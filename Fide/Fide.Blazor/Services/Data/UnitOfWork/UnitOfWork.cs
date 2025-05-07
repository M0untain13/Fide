using Fide.Blazor.Services.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Fide.Blazor.Services.Data.UnitOfWork;

public class UnitOfWork(DbContext context) : IUnitOfWork
{
    public IRepository<T> GetRepository<T>() where T : class
        => new Repository<T>(context);

    public async Task<int> CommitAsync()
        => await context.SaveChangesAsync();

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
