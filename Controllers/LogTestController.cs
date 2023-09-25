using Microsoft.AspNetCore.Mvc;

namespace AdvWorksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogTestController : ControllerBase
{
    private readonly ILogger<LogTestController> _Logger;

    public LogTestController(ILogger<LogTestController> logger)
    {
        _Logger = logger;
    }

    [HttpGet]
    [Route("WriteMessages")]
    public string WriteMessages()
    {
        // Write Log Messages
        WriteLogMessages();

        return "Check Console Window.";
    }

    private void WriteLogMessages()
    {
        // The following are in the Log Level order
        _Logger.LogTrace("This is a Trace log entry.");
        _Logger.LogDebug("This is a Debug log entry.");
        _Logger.LogInformation("This is an Information log entry.");
        _Logger.LogWarning("This is a Warning log entry.");
        _Logger.LogError("This is an Error log entry.");
        _Logger.LogError(new ApplicationException("This is an exception."), "Exception Object.");
        _Logger.LogCritical("This is a Critical log entry.");
    }
}