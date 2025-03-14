using Fide.Blazor.Data;
using Fide.Blazor.Services.Repositories.Base;

namespace Fide.Blazor.Services.Repositories;

public class UserRepository(ApplicationDbContext context) : IRepository<ApplicationUser, string>
{
    public ApplicationUser Get(string id)
    {
        return context.Users.First(u => u.Id == id);
    }

    public IEnumerable<ApplicationUser> GetAll()
    {
        return context.Users;
    }

    public void Create(ApplicationUser item)
    {
        context.Users.Add(item);
    }

    public void Delete(string id)
    {
        var user = Get(id);
        context.Users.Remove(user);
    }

    public void Save()
    {
        context.SaveChanges();
    }

    public void Update(ApplicationUser item)
    {
        context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
