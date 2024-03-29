using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.BaseClasses;
using AdvWorksAPI.SearchClasses;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBaseAPI
{
    private readonly IRepository<Product, ProductSearch> _Repo;
    private readonly AdvWorksAPIDefaults _Settings;

    public ProductController(IRepository<Product, ProductSearch> repo, ILogger<ProductController> logger, IOptionsMonitor<AdvWorksAPIDefaults> settings) : base(logger)
    {
        _Repo = repo;
        _Settings = settings.CurrentValue;
    }

    [HttpGet]
    // [Authorize(Policy = "GetProductsClaim")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/xml")]
    public ActionResult<IEnumerable<Product>> Get()
    {
        ActionResult<IEnumerable<Product>> ret;
        List<Product> list;
        InfoMessage = "No Products are available.";

        try {
            // Intentionally Cause an Exception
            // throw new ApplicationException("Error!");

            // Get all data
            list = _Repo.Get();

            if(list != null && list.Count > 0) {
                return StatusCode(StatusCodes.Status200OK, list);
            }
            else {
                return StatusCode(StatusCodes.Status404NotFound, InfoMessage);
            }
        }
        catch(Exception ex) {
            InfoMessage = _Settings.InfoMessageDefault.Replace("{Verb}", "GET").Replace("{ClassName}", "Product");
            ErrorLogMessage = "Error in ProductController.Get()";
            ret = HandleException<IEnumerable<Product>>(ex);
        }

        return ret;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product?> Get(int id)
    {
        ActionResult<Product?> ret;
        Product? entity;
        
        entity = _Repo.Get(id);

        if(entity != null) {
            ret = StatusCode(StatusCodes.Status200OK, entity);
        }
        else {
            ret = StatusCode(StatusCodes.Status404NotFound, $"Can't find Product with ID '{id}'.");
        }

        return ret;
    }

    #region Search Method
    [HttpGet()]
    [Route("Search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Product>> Search([FromQuery()] ProductSearch search)
    {
        ActionResult<IEnumerable<Product>> ret;
        List<Product> list;

        InfoMessage = "Can't find products matching the criteria passed in.";

        try
        {
            // Search for Data
            list = _Repo.Search(search);

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
            InfoMessage = _Settings.InfoMessageDefault
              .Replace("{Verb}", "Search").Replace("{ClassName}", "Product");

            ErrorLogMessage = "Error in ProductController.Search()";

            ret = HandleException<IEnumerable<Product>>(ex);
        }

        return ret;
    }
    #endregion

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Product> Post([FromBody] Product entity)
    {
        ActionResult<Product> ret;

        // Serialize entity
        SerializeEntity<Product>(entity);

        try
        {
            if (entity != null)
            {
                // Attempt to update the database
                entity = _Repo.Insert(entity);

                // Return a '201 Created' with the new entity
                ret = StatusCode(StatusCodes.Status201Created, entity);
            }
            else
            {
                InfoMessage = $"Product object passed to POST is empty.";
                // Return a '400 Bad Request'
                ret = StatusCode(StatusCodes.Status400BadRequest, InfoMessage);
                // Log an informational message
                _Logger.LogInformation("{InfoMessage}", InfoMessage);
            }
        }
        catch (Exception ex)
        {
            // Return generic message for the user
            InfoMessage = _Settings.InfoMessageDefault
              .Replace("{Verb}", "POST")
              .Replace("{ClassName}", "Product");

            ErrorLogMessage = $"ProductController.Post() - Exception trying to insert a new product: {EntityAsJson}";
            ret = HandleException<Product>(ex);
        }

        return ret;
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Product> Put(int id, [FromBody] Product entity)
    {
        ActionResult<Product> ret;

        // Serialize entity
        SerializeEntity<Product>(entity);

        try
        {
            if (entity != null)
            {
                // Attempt to locate the data to update
                Product? current = _Repo.Get(id);

                if (current != null)
                {
                    // Combine changes into current record
                    entity = _Repo.SetValues(current, entity);

                    // Attempt to update the database
                    current = _Repo.Update(current);

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
                InfoMessage = $"Product object passed to PUT is empty.";
                // Return a '400 Bad Request'
                ret = StatusCode(StatusCodes.Status400BadRequest, InfoMessage);
                // Log an informational message
                _Logger.LogInformation("InfoMessage}", InfoMessage);
            }
        }
        catch (Exception ex)
        {
            // Return generic message for the user
            InfoMessage = _Settings.InfoMessageDefault
              .Replace("{Verb}", "PUT")
              .Replace("{ClassName}", "Product");

            ErrorLogMessage = $"ProductController.Put() - Exception trying to update Product: {EntityAsJson}";
            ret = HandleException<Product>(ex);
        }

        return ret;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Product> Delete(int id)
    {
        ActionResult<Product> ret;

        try
        {
            // Attempt to delete from the database    
            if (_Repo.Delete(id))
            {
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
            // Return generic message for the user
            InfoMessage = _Settings.InfoMessageDefault
              .Replace("{Verb}", "DELETE")
              .Replace("{ClassName}", "Product");

            ErrorLogMessage = $"ProductController.Delete() - Exception trying to delete ProductID: '{id}'.";
            ret = HandleException<Product>(ex);
        }

        return ret;
    }
}