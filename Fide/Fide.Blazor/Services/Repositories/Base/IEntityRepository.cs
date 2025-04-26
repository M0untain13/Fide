using Fide.Blazor.Data.Base;

namespace Fide.Blazor.Services.Repositories.Base;

public interface IEntityRepository<T> : IRepository<T, Guid>
    where T : Entity
{
}
