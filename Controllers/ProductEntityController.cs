using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.RepositoryLayer;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductEntityController : ControllerBase
{
    [HttpGet]
    public List<Product> Get()
    {
        List<Product> list;

        // Get all data
        list = new ProductRepository().Get();

        return list;
    }
}