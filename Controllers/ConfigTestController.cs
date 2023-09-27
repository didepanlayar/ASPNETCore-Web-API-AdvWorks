using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.BaseClasses;
using AdvWorksAPI.EntityLayer;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigTestController : ControllerBaseAPI
{
    private readonly IConfiguration _Config;

    public ConfigTestController(ILogger<ConfigTestController> logger, IConfiguration config) : base(logger)
    {
        _Config = config;
    }

    [HttpGet]
    [Route("AssignToClass")]
    public AdvWorksAPIDefaults AssignToClass()
    {
        AdvWorksAPIDefaults settings;

        settings = _Config.GetRequiredSection("AdvWorksAPI").Get<AdvWorksAPIDefaults>();

        return settings;
    }

    [HttpGet]
    [Route("SetProperties")]
    public AdvWorksAPIDefaults SetProperties()
    {
        AdvWorksAPIDefaults settings = new()
        {
            InfoMessageDefault = _Config["AdvWorksAPI:InfoMessageDefault"] ?? string.Empty,
            ProductCategoryID = Convert.ToInt32(_Config["AdvWorksAPI:ProductCategoryID"]),
            ProductModelID = Convert.ToInt32(_Config["AdvWorksAPI:ProductModelID"])
        };

        return settings;
    }

    [HttpGet]
    [Route("IConfigurationTest")]
    public string IConfigurationTest()
    {
        return _Config["AdvWorksAPI:InfoMessageDefault"] ?? string.Empty;
    }
}
