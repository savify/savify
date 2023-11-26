using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Configuration.Data;
using App.Modules.Wallets.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Wallets.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly OutboxCommandProcessor<WalletsContext> _outboxCommandProcessor;

    private readonly ILogger _logger;

    public ProcessOutboxCommandHandler(
        OutboxCommandProcessor<WalletsContext> outboxCommandProcessor,
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
