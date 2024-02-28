namespace AdvWorksAPI.Interfaces;

public interface IRepository<TEntity, TSearch>
{
    // Asynchronous Methods
    Task<List<TEntity>> GetAsync();

    // Synchronous Methods
    List<TEntity> Get();
    TEntity? Get(int id);
    List<TEntity> Search(TSearch search);
    TEntity Insert(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity SetValues(TEntity current, TEntity changes);
    bool Delete(int id);
}