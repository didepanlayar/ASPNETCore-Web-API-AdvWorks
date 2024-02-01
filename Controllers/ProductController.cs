using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.BaseClasses;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBaseAPI
{
    private readonly IRepository<Product> _Repo;
    private readonly AdvWorksAPIDefaults _Settings;

    public ProductController(IRepository<Product> repo, ILogger<ProductController> logger, IOptionsMonitor<AdvWorksAPIDefaults> settings) : base(logger)
    {
        _Repo = repo;
        _Settings = settings.CurrentValue;
    }

    [HttpGet]
    [Authorize(Policy = "GetProductsClaim")]
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public ActionResult<Product> Post(Product entity)
    {
        ActionResult<Product> ret;

        // TODO: Insert Data into Data Store
        entity.ModifiedDate = DateTime.Now;

        ret = StatusCode(StatusCodes.Status201Created, entity);

        return ret;
    }
}