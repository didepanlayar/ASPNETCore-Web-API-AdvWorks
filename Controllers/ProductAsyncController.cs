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
}
