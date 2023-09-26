using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AdvWorksAPI.Controllers;

public class ErrorController : ControllerBase
{
    [Route("/ProductionError")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult ProductionErrorHandler(int code)
    {
        string msg = "Unknown Error!";

        var features = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
        if(features != null) {
            msg = features.Error.Message;
        }

        return StatusCode(StatusCodes.Status500InternalServerError, msg);
    }

    [Route("/DevelopmentError")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult DevelopmentErrorHandler(int code)
    {
        string msg = "Unknown Error!";

        var features = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
        if(features != null) {
            msg = "Message: " + features.Error.Message;
            msg += Environment.NewLine + "Source: " + features.Error.Message;
            msg += Environment.NewLine + features.Error.StackTrace;
        }

        return StatusCode(StatusCodes.Status500InternalServerError, msg);
    }
}