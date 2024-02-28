using AdvWorksAPI.BaseClasses;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.SearchClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdvWorksAPI.Controllers;

/// <summary>
/// Asynchronous Action Methods
/// </summary>
[Route("api/[controller]")]
[ApiController]
public partial class ProductAsyncController : ControllerBaseAPI
{
    private readonly IRepository<Product, ProductSearch> _Repo;
    private readonly AdvWorksAPIDefaults _Settings;

    public ProductAsyncController(IRepository<Product, ProductSearch> repo, ILogger<ProductController> logger, IOptionsMonitor<AdvWorksAPIDefaults> settings) : base(logger)
    {
        _Repo = repo;
        _Settings = settings.CurrentValue;
    }

    #region GetAsync Method
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
    {
        ActionResult<IEnumerable<Product>> ret;
        List<Product> list;
        InfoMessage = "No Products are available.";

        try
        {
            // Get all data
            var task = await _Repo.GetAsync();
            // Convert data to a List<T>
            list = task.ToList();

            if (list != null && list.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, list);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, InfoMessage);
            }
        }
        catch (Exception ex)
        {
            InfoMessage = _Settings.InfoMessageDefault.Replace("{Verb}", "GET").Replace("{ClassName}", "Product");

            ErrorLogMessage = "Error in ProductController.GetAsync()";

            ret = HandleException<IEnumerable<Product>>(ex);
        }

        return ret;
    }
    #endregion

    #region GetAsync(id) Method
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetAsync(int id)
    {
        ActionResult<Product> ret;
        Product? entity;

        entity = await _Repo.GetAsync(id);

        if (entity != null)
        {
            // Found data, return '200 OK'
            ret = StatusCode(StatusCodes.Status200OK, entity);
        }
        else
        {
            // Did not find data, return '404 Not Found'
            ret = StatusCode(StatusCodes.Status404NotFound, $"Can't find Product with a Product Id of '{id}'.");
        }

        return ret;
    }
    #endregion

    #region SearchAsync Method
    [HttpGet]
    [Route("Search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Product>>> SearchAsync([FromQuery()] ProductSearch search)
    {
        ActionResult<IEnumerable<Product>> ret;
        List<Product> list;

        InfoMessage = "Can't find products matching the criteria passed in.";

        try
        {
            // Search for Data
            var task = await _Repo.SearchAsync(search);
            list = task.ToList();

            if (list != null && list.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, list);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, InfoMessage);
            }
        }
        catch (Exception ex)
        {
            InfoMessage = _Settings.InfoMessageDefault.Replace("{Verb}", "Search").Replace("{ClassName}", "Product");

            ErrorLogMessage = "Error in ProductController.Search()";

            ret = HandleException<IEnumerable<Product>>(ex);
        }

        return ret;
    }
    #endregion

    #region PostAsync Method
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> PostAsync(Product entity)
    {
        ActionResult<Product> ret;

        // Serialize entity
        SerializeEntity<Product>(entity);

        try
        {
            if (entity != null)
            {
                // Attempt to update the database
                entity = await _Repo.InsertAsync(entity);

                // Return a '201 Created' with the new entity
                ret = StatusCode(StatusCodes.Status201Created, entity);
            }
            else
            {
                InfoMessage = "Product product object passed to POST method is empty.";
                // Return a '400 Bad Request'
                ret = StatusCode(StatusCodes.Status400BadRequest, InfoMessage);
                // Log an informational message
                _Logger.LogInformation("{InfoMessage}", InfoMessage);
            }
        }
        catch (Exception ex)
        {
            InfoMessage = _Settings.InfoMessageDefault.Replace("{Verb}", "POST").Replace("{ClassName}", "Product");

            ErrorLogMessage = $"ProductController.Post() - Exception trying to insert a new product: {EntityAsJson}";
            ret = HandleException<Product>(ex);
        }

        return ret;
    }
    #endregion

    #region PutAsync Method
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> PutAsync(int id, [FromBody] Product entity)
    {
        ActionResult<Product> ret;

        // Serialize entity
        SerializeEntity<Product>(entity);

        try
        {
            // Serialize entity
            SerializeEntity<Product>(entity);

            if (entity != null)
            {
                // Attempt to locate the data to update
                Product? current = await _Repo.GetAsync(id);

                if (current != null)
                {
                    // Combine changes into current record
                    entity = _Repo.SetValues(current, entity);

                    // Attempt to update the database
                    current = await _Repo.UpdateAsync(current);

                    // Pass back a '200 Ok'
                    ret = StatusCode(StatusCodes.Status200OK, current);
                }
                else
                {
                    InfoMessage = $"Can't find Product Id '{id}' to update.";
                    // Did not find data, return '404 Not Found'
                    ret = StatusCode(StatusCodes.Status404NotFound, InfoMessage);
                    // Log an informational message
                    _Logger.LogInformation("{InfoMessage}", InfoMessage);
                }
            }
            else
            {
                InfoMessage = "Product object passed to PUT method is empty.";
                // Return a '400 Bad Request'
                ret = StatusCode(StatusCodes.Status400BadRequest, InfoMessage);
                // Log an informational message
                _Logger.LogInformation("{InfoMessage}", InfoMessage);
            }
        }
        catch (Exception ex)
        {
            InfoMessage = _Settings.InfoMessageDefault.Replace("{Verb}", "PUT").Replace("{ClassName}", "Product");

            ErrorLogMessage = $"ProductController.Put() - Exception trying to update Product: {EntityAsJson}";
            ret = HandleException<Product>(ex);
        }

        return ret;
    }
    #endregion

    #region DeleteAsync Method
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> DeleteAsync(int id)
    {
        ActionResult<Product> ret;
        Product? entity;

        try
        {
            // Attempt to locate the data to delete
            entity = await _Repo.GetAsync(id);
            if (entity != null)
            {
                // Attempt to update the database
                await _Repo.DeleteAsync(id);

                // Return '204 No Content'
                ret = StatusCode(StatusCodes.Status204NoContent);
            }
            else
            {
                InfoMessage = $"Can't find Product Id '{id}' to delete.";
                // Did not find data, return '404 Not Found'
                ret = StatusCode(StatusCodes.Status404NotFound, InfoMessage);
                // Log an informational message
                _Logger.LogInformation("{InfoMessage}", InfoMessage);
            }
        }
        catch (Exception ex)
        {
            InfoMessage = _Settings.InfoMessageDefault.Replace("{Verb}", "DELETE").Replace("{ClassName}", "Product");

            ErrorLogMessage = $"ProductController.Delete() - Exception trying to delete ProductID: '{id}'.";

            ret = HandleException<Product>(ex);
        }

        return ret;
    }
    #endregion
}
