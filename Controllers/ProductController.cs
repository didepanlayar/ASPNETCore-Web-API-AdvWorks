using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.Interfaces;
using AdvWorksAPI.BaseClasses;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBaseAPI
{
    private readonly IRepository<Product> _Repo;

    public ProductController(IRepository<Product> repo, ILogger<ProductController> logger) : base(logger)
    {
        _Repo = repo;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            InfoMessage = "Error in Product API. Please Contact the System Administrator.";
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
}