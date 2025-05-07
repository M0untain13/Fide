﻿using System.Linq.Expressions;

namespace Fide.Blazor.Services.Data.Repository;

public interface IRepository<T>
    where T : class
{
    Task<T?> GetAsync<IdType>(IdType id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
