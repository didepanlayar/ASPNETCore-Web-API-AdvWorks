namespace AdvWorksAPI.Interfaces;

public interface IRepository<T>
{
    List<T> Get();
    T? Get(int id);
    T Insert(T entity);
    T Update(T entity);
    T SetValues(T current, T changes);
    bool Delete(int id);
}