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

    #region Search Methods
    public virtual List<Product> Search(ProductSearch search)
    {
        IQueryable<Product> query = _DbContext.Products;

        // Add WHERE clause(s)
        query = AddWhereClause(query, search);

        // Add ORDER BY clause(s)
        query = AddOrderByClause(query, search);

        return query.ToList();
    }

    protected virtual IQueryable<Product> AddWhereClause(IQueryable<Product> query, ProductSearch search)
    {
        // Perform Searching      
        if (!string.IsNullOrEmpty(search.Name))
        {
            query = query.Where(row => row.Name.StartsWith(search.Name));
        }
        if (search.ListPrice.HasValue)
        {
            query = query.Where(row => row.ListPrice >= search.ListPrice);
        }

        return query;
    }

    protected virtual IQueryable<Product> AddOrderByClause(IQueryable<Product> query, ProductSearch search)
    {
        switch (search.OrderBy.ToLower())
        {
            case "":
            case "name":
                query = query.OrderBy(row => row.Name);
                break;
        }

        return query;
    }
    #endregion

    #region Insert Method
    public Product Insert(Product entity)
    {
        // Fill in required fields not passed by client
        entity.RowGuid = Guid.NewGuid();
        entity.ModifiedDate = DateTime.Now;

        // Add new entity to Products DbSet
        _DbContext.Products.Add(entity);

        // Save changes in database
        _DbContext.SaveChanges();

        return entity;
    }
    #endregion

    #region Update Method
    public Product Update(Product entity)
    {
        // Set the last modified date
        entity.ModifiedDate = DateTime.Now;

        // Update entity in Products DbSet
        _DbContext.Products.Update(entity);

        // Save changes in database
        _DbContext.SaveChanges();

        return entity;
    }
    #endregion

    #region SetValues Method
    public Product SetValues(Product current, Product changes)
    {
        // Since we don't necessarily pass in all the data,
        // overwrite the changed properties in the one
        // read from the database
        // TODO: Make this a little more bullet-proof
        current.Name = string.IsNullOrWhiteSpace(changes.Name) ? current.Name : changes.Name;
        current.ProductNumber = string.IsNullOrWhiteSpace(changes.ProductNumber) ? current.ProductNumber : changes.ProductNumber;
        current.Color = string.IsNullOrWhiteSpace(changes.Color) ? current.Color : changes.Color;
        current.StandardCost = changes.StandardCost == 0 ? current.StandardCost : changes.StandardCost;
        current.ListPrice = changes.ListPrice == 0 ? current.ListPrice : changes.ListPrice;
        current.Size = string.IsNullOrWhiteSpace(changes.Size) ? current.Size : changes.Size;
        current.Weight = changes.Weight == 0 ? current.Weight : changes.Weight;
        current.SellStartDate = changes.SellStartDate == DateTime.MinValue ? current.SellStartDate : changes.SellStartDate;
        current.SellEndDate = changes.SellEndDate == DateTime.MinValue || changes.SellEndDate == null ? current.SellEndDate : changes.SellEndDate;
        current.DiscontinuedDate = changes.DiscontinuedDate == DateTime.MinValue || changes.DiscontinuedDate == null ? current.DiscontinuedDate : changes.DiscontinuedDate;
        current.RowGuid = changes.RowGuid == Guid.NewGuid() ? current.RowGuid : Guid.NewGuid();
        current.ProductCategoryID = changes.ProductCategoryID == 0 ? current.ProductCategoryID : changes.ProductCategoryID;
        current.ProductModelID = changes.ProductModelID == 0 ? current.ProductModelID : changes.ProductModelID;
        current.ModifiedDate = DateTime.Now;

        return current;
    }
    #endregion

    #region Delete Method
    public bool Delete(int id)
    {
        Product? entity = _DbContext.Products.Find(id);

        if (entity != null)
        {
            // Locate the entity to delete in the Products DbSet
            _DbContext.Products.Remove(entity);

            // Save changes in database
            _DbContext.SaveChanges();

            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}