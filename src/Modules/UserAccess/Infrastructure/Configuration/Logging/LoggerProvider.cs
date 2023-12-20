using Serilog;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Logging;

public class LoggerProvider(ILogger logger) : ILoggerProvider
{
    public ILogger GetLogger()
    {
        return logger.ForContext("Module", "UserAccess");
    }
}
