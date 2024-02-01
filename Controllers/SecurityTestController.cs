using AdvWorksAPI.BaseClasses;
using AdvWorksAPI.EntityLayer;
using AdvWorksAPI.SecurityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecurityTestController : ControllerBaseAPI
{
    private readonly AdvWorksAPIDefaults _Settings;

    public SecurityTestController(IOptionsMonitor<AdvWorksAPIDefaults> defaults, ILogger<SecurityTestController> logger) : base(logger)
    {
        _Settings = defaults.CurrentValue;
    }

    [HttpGet()]
    [Route("AuthenticateUser/{name}/password/{password}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<AppSecurityToken> AuthenticateUser(string name, string password)
    {
        ActionResult<AppSecurityToken> ret;
        AppSecurityToken asToken;

        asToken = new SecurityManager().AuthenticateUser(name, password, _Settings.JWTSettings);

        if (asToken.User.IsAuthenticated)
        {
            ret = StatusCode(StatusCodes.Status200OK, asToken);
        }
        else
        {
            ret = StatusCode(StatusCodes.Status401Unauthorized, "Invalid User Name/Password.");
        }

        return ret;
    }
}
