using Fide.Blazor.Services.Data.Repository;

namespace Fide.Blazor.Services.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>()
        where T : class;
    Task<int> CommitAsync();
}
