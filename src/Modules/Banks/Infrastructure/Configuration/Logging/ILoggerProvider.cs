using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration.Logging;

public interface ILoggerProvider
{
    ILogger GetLogger();
}
