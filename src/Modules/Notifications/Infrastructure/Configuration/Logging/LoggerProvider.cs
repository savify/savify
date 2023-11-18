using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.Logging;

public class LoggerProvider : ILoggerProvider
{
    private readonly ILogger _logger;

    public LoggerProvider(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger Provide()
    {
        return _logger.ForContext("Module", "Notifications");
    }
}
