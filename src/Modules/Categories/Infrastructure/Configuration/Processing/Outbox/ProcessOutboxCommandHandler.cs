using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Application.Configuration.Data;
using App.Modules.Categories.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly OutboxCommandProcessor<CategoriesContext> _outboxCommandProcessor;

    private readonly ILogger _logger;

    public ProcessOutboxCommandHandler(
        OutboxCommandProcessor<CategoriesContext> outboxCommandProcessor,
        ILoggerProvider loggerProvider)
    {
        _outboxCommandProcessor = outboxCommandProcessor;
        _logger = loggerProvider.Provide();
    }

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        await _outboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
