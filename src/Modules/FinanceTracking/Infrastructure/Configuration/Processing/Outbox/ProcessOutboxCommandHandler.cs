using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly OutboxCommandProcessor<FinanceTrackingContext> _outboxCommandProcessor;

    private readonly ILogger _logger;

    public ProcessOutboxCommandHandler(
        OutboxCommandProcessor<FinanceTrackingContext> outboxCommandProcessor,
        ILoggerProvider loggerProvider)
    {
        _outboxCommandProcessor = outboxCommandProcessor;
        _logger = loggerProvider.GetLogger();
    }

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        await _outboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
