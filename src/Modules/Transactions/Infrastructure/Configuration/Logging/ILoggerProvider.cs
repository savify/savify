using Serilog;

namespace App.Modules.Transactions.Infrastructure.Configuration.Logging;

public interface ILoggerProvider
{
    ILogger GetLogger();
}
