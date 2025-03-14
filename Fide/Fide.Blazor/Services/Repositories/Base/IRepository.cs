namespace Fide.Blazor.Services.Repositories.Base;

public interface IRepository<T> : IRepository<T, Guid>
{
}

public interface IRepository<T, IdType> : IDisposable
{
    IEnumerable<T> GetAll();
    T Get(IdType id);
    void Create(T item);
    void Update(T item);
    void Delete(IdType id);
    void Save();

}