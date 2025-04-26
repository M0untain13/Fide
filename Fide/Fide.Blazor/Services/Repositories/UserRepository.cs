using Fide.Blazor.Data;
using Fide.Blazor.Services.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Fide.Blazor.Services.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository<ApplicationUser>
{
    public IQueryable<ApplicationUser> Include(IQueryable<ApplicationUser> query)
    {
        return query.Include(u => u.ImageLinks);
    }

    public ApplicationUser? Get(string id)
    {
        return Include(context.Users).FirstOrDefault(u => u.Email == id);
    }

    public IEnumerable<ApplicationUser> GetAll()
    {
        return Include(context.Users);
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
        context.Entry(item).State = EntityState.Modified;
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
