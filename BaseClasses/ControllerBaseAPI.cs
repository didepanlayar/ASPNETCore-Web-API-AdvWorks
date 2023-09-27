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
}