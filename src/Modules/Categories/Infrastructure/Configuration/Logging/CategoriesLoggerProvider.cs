using Serilog;

namespace App.Modules.Categories.Infrastructure.Configuration.Logging;

public class CategoriesLoggerProvider : ICategoriesLoggerProvider
{
    private readonly ILogger _logger;

    public CategoriesLoggerProvider(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger Provide()
    {
        return _logger.ForContext("Module", "Categories");
    }
}
