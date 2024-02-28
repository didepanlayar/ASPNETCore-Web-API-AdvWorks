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
}