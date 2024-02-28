using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.SearchClasses;
using Microsoft.EntityFrameworkCore;

namespace AdvWorksAPI.RepositoryLayer;

/// <summary>
/// Asynchronous access to Product data asynchronously
/// </summary>
public partial class ProductRepository
{
    #region GetAsync Method
    /// <summary>
    /// Get all Product objects asynchronously
    /// </summary>
    /// <returns>A list of Product objects</returns>
    public async Task<List<Product>> GetAsync()
    {
        return await _DbContext.Products.OrderBy(row => row.Name).ToListAsync();
    }
    #endregion

    #region GetAsync(id) Method
    public async Task<Product?> GetAsync(int id)
    {
        return await _DbContext.Products.Where(row => row.ProductID == id).FirstOrDefaultAsync();
    }
    #endregion

    #region SearchAsync Methods
    public async Task<List<Product>> SearchAsync(ProductSearch search)
    {
        IQueryable<Product> query = _DbContext.Products;

        // Add WHERE clause(s)
        query = AddWhereClause(query, search);

        // Add ORDER BY clause(s)
        query = AddOrderByClause(query, search);

        return await query.ToListAsync();
    }
    #endregion

    #region InsertAsync Method
    public async Task<Product> InsertAsync(Product entity)
    {
        // Fill in required fields not passed by client
        entity.RowGuid = Guid.NewGuid();
        entity.ModifiedDate = DateTime.Now;

        // Add new entity to Products DbSet
        _DbContext.Products.Add(entity);

        // Save changes in database
        await _DbContext.SaveChangesAsync();

        return entity;
    }
    #endregion

    #region UpdateAsync Method
    public async Task<Product> UpdateAsync(Product entity)
    {
        // Update entity in Products DbSet
        _DbContext.Products.Update(entity);

        // Save changes in database
        await _DbContext.SaveChangesAsync();

        return entity;
    }
    #endregion

    #region DeleteAsync Method
    public async Task<bool> DeleteAsync(int id)
    {
        Product? entity = await _DbContext.Products.FindAsync(id);

        if (entity != null)
        {
            // Locate the entity to delete in the Products DbSet
            _DbContext.Products.Remove(entity);

            // Save changes in database
            await _DbContext.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}