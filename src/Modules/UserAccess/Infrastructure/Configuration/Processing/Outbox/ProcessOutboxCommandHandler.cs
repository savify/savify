using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly OutboxCommandProcessor<UserAccessContext> _outboxCommandProcessor;

    private readonly ILogger _logger;

    public ProcessOutboxCommandHandler(
        OutboxCommandProcessor<UserAccessContext> outboxCommandProcessor,
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
