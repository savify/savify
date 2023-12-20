using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Application.Configuration.Data;
using App.Modules.Categories.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler(
    OutboxCommandProcessor<CategoriesContext> outboxCommandProcessor,
    ILoggerProvider loggerProvider)
    : ICommandHandler<ProcessOutboxCommand>
{
    private readonly ILogger _logger = loggerProvider.GetLogger();

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        await outboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
