using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.Models;

namespace AdvWorksAPI.RepositoryLayer;

/// <summary>
/// Class to work with Product data
/// </summary>
public class ProductRepository : IRepository<Product>
{
    private readonly AdvWorksLTDbContext _DbContext;

    public ProductRepository(AdvWorksLTDbContext context)
    {
        _DbContext = context;
    }

    #region Get Method
    /// <summary>
    /// Get all Product objects
    /// </summary>
    /// <returns>A list of Product objects</returns>
    public List<Product> Get()
    {
        return _DbContext.Products.OrderBy(row => row.Name).ToList();
    }
    #endregion

    #region Get(id) Method
    /// <summary>
    /// Get a single Product object
    /// </summary>
    /// <param name="id">The value to locate</param>
    /// <returns>A valid Product object object, or null if not found</returns>
    public Product? Get(int id)
    {
        return _DbContext.Products.Where(row => row.ProductID == id).FirstOrDefault();
    }
    #endregion
}