using Serilog;

namespace App.Modules.Categories.Infrastructure.Configuration.Logging;

public interface ILoggerProvider
{
    ILogger Provide();
}
