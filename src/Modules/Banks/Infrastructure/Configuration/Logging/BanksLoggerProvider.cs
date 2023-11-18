using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration.Logging;

public class BanksLoggerProvider : IBanksLoggerProvider
{
    private readonly ILogger _logger;

    public BanksLoggerProvider(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger Provide()
    {
        return _logger.ForContext("Module", "Banks");
    }
}
