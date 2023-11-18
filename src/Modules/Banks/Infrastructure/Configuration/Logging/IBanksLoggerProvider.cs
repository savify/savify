using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration.Logging;

public interface IBanksLoggerProvider
{
    ILogger Provide();
}
