using Microsoft.AspNetCore.Identity;

namespace Fide.Blazor.Services.Repositories.Base;

public interface IUserRepository<T> : IRepository<T, string>
    where T : IdentityUser
{
}
