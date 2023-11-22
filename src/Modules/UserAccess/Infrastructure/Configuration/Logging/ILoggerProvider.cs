using Serilog;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Logging;

public interface ILoggerProvider
{
    ILogger GetLogger();
}
