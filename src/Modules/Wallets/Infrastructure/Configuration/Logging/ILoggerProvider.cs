using Serilog;

namespace App.Modules.Wallets.Infrastructure.Configuration.Logging;

public interface ILoggerProvider
{
    ILogger GetLogger();
}
