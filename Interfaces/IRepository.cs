namespace AdvWorksAPI.Interfaces;

public interface IRepository<TEntity, TSearch>
{
    // Asynchronous Methods
    Task<List<TEntity>> GetAsync();
    Task<TEntity?> GetAsync(int id);
    Task<List<TEntity>> SearchAsync(TSearch search);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(int id);

    // Synchronous Methods
    List<TEntity> Get();
    TEntity? Get(int id);
    List<TEntity> Search(TSearch search);
    TEntity Insert(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity SetValues(TEntity current, TEntity changes);
    bool Delete(int id);
}