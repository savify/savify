using Serilog;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Logging;

public interface ILoggerProvider
{
    ILogger GetLogger();
}
