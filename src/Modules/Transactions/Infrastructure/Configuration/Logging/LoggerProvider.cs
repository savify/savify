using Serilog;

namespace App.Modules.Transactions.Infrastructure.Configuration.Logging;

public class LoggerProvider : ILoggerProvider
{
    private readonly ILogger _logger;

    public LoggerProvider(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger Provide()
    {
        return _logger.ForContext("Module", "Transactions");
    }
}
