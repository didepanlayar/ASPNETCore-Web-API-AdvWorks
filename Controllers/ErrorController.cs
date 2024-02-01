using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AdvWorksAPI.Controllers;

public class ErrorController : ControllerBase
{
    [Route("/StatusCodeHandler/{code:int}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult StatusCodeHandler(int code)
    {
        IActionResult ret;
        string msg = $"Code is not handled: '{code}'";
        
        // Get some path information
        var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        if(feature != null) {
            msg = feature.OriginalPathBase + feature.OriginalPath + feature.OriginalQueryString;
        }
        
        switch(code) {
            case 401:
                msg = $"API Route Was Not Authorized: '{msg}'";
                ret = StatusCode(StatusCodes.Status401Unauthorized, msg);
                break;
            case 403:
                msg = $"API Route Prohibited: '{msg}'";
                ret = StatusCode(StatusCodes.Status403Forbidden, msg);
                break;
            case 404:
                msg = $"API Route Was Not Found: '{msg}'";
                ret = StatusCode(StatusCodes.Status404NotFound, msg);
                break;
            default:
                ret = StatusCode(StatusCodes.Status500InternalServerError, msg);
                break;
        }
        
        return ret;
    }
    
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