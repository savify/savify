using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Application.Configuration.Data;
using App.Modules.Transactions.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Transactions.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly OutboxCommandProcessor<TransactionsContext> _outboxCommandProcessor;

    private readonly ILogger _logger;

    public ProcessOutboxCommandHandler(
        OutboxCommandProcessor<TransactionsContext> outboxCommandProcessor,
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
