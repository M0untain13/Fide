namespace Fide.Blazor.Services.Repositories.Base;

public interface IRepository<T> : IDisposable
{
    IEnumerable<T> GetAll();
    T Get(Guid id);
    void Create(T item);
    void Update(T item);
    void Delete(Guid id);
    void Save();

}
