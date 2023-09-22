using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.RepositoryLayer;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Product>> Get()
    {
        List<Product> list;

        // Get all data
        list = new ProductRepository().Get();

        if(list != null && list.Count > 0) {
            return StatusCode(StatusCodes.Status200OK, list);
        }
        else {
            return StatusCode(StatusCodes.Status404NotFound, "No products are available.");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product?> Get(int id)
    {
        ActionResult<Product?> ret;
        Product? entity;
        
        entity = new ProductRepository().Get(id);

        if(entity != null) {
            ret = StatusCode(StatusCodes.Status200OK, entity);
        }
        else {
            ret = StatusCode(StatusCodes.Status404NotFound, $"Can't find Product with ID '{id}'.");
        }

        return ret;
    }

    [HttpGet]
    [Route("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        List<Product> list;

        // Get all data
        list = new ProductRepository().Get();

        if(list != null && list.Count > 0) {
            return StatusCode(StatusCodes.Status200OK, list);
        }
        else {
            return StatusCode(StatusCodes.Status404NotFound, "No products are available.");
        }
    }
}