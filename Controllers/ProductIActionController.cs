using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.RepositoryLayer;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductIActionController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        IActionResult ret;
        List<Product> list;

        // Get all data
        list = new ProductRepository().Get();

        // list.Clear();

        if(list != null && list.Count > 0) {
            ret = StatusCode(StatusCodes.Status200OK, list);
        }
        else {
            ret = StatusCode(StatusCodes.Status404NotFound, "No products are available.");
        }

        return ret;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        IActionResult ret;
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
}