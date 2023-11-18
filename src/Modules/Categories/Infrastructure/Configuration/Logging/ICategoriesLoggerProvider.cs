using Serilog;

namespace App.Modules.Categories.Infrastructure.Configuration.Logging;

public interface ICategoriesLoggerProvider
{
    ILogger Provide();
}
