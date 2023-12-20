using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler(
    OutboxCommandProcessor<FinanceTrackingContext> outboxCommandProcessor,
    ILoggerProvider loggerProvider)
    : ICommandHandler<ProcessOutboxCommand>
{
    private readonly ILogger _logger = loggerProvider.GetLogger();

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        await outboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
