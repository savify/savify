using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.Logging;

public interface ILoggerProvider
{
    ILogger Provide();
}
