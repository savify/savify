using Serilog;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Logging;

public class LoggerProvider(ILogger logger) : ILoggerProvider
{
    public ILogger GetLogger()
    {
        return logger.ForContext("Module", "FinanceTracking");
    }
}
