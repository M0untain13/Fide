using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Fide.Blazor.Services.Data.Repository;

public class Repository<T>(DbContext context) : IRepository<T>
    where T : class
{
    protected readonly DbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    protected readonly DbSet<T> _dbSet = context.Set<T>();
    public async Task<T?> GetAsync<IdType>(IdType id) 
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() 
        => await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Remove(T entity)
        => _dbSet.Remove(entity);

    public void Update(T entity)
        => _context.Update(entity);
}
