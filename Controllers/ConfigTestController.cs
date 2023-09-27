using Microsoft.AspNetCore.Mvc;
using AdvWorksAPI.BaseClasses;

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
    [Route("IConfigurationTest")]
    public string IConfigurationTest()
    {
        return _Config["AdvWorksAPI:InfoMessageDefault"] ?? string.Empty;
    }
}
