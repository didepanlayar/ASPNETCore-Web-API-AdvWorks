using Microsoft.AspNetCore.Mvc;

namespace AdvWorksAPI.BaseClasses;

public class ControllerBaseAPI : ControllerBase
{
    protected readonly ILogger _Logger;

    public ControllerBaseAPI(ILogger logger)
    {
        _Logger = logger;
        InfoMessage = string.Empty;
        ErrorLogMessage = string.Empty;
    }
    
    public string InfoMessage { get; set; }
    public string ErrorLogMessage { get; set; }

    protected ActionResult<T> HandleException<T>(Exception ex, string infoMsg, string errorMsg)
    {
        InfoMessage = infoMsg;
        ErrorLogMessage = errorMsg;

        return HandleException<T>(ex);
    }

    protected ActionResult<T> HandleException<T>(Exception ex)
    {
        ActionResult<T> ret;

        ret = StatusCode(StatusCodes.Status500InternalServerError, InfoMessage);

        ErrorLogMessage += $"{Environment.NewLine}Message: {ex.Message}";
        ErrorLogMessage += $"{Environment.NewLine}Source: {ex.Source}";
        ErrorLogMessage += $"{Environment.NewLine}Stack Trace: {ex.StackTrace}";

        _Logger.LogError(ex, "{ErrorLogMessage}", ErrorLogMessage);

        return ret;
    }
}