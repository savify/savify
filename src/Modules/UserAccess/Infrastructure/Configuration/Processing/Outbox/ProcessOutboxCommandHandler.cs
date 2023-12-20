using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler(
    OutboxCommandProcessor<UserAccessContext> outboxCommandProcessor,
    ILoggerProvider loggerProvider)
    : ICommandHandler<ProcessOutboxCommand>
{
    private readonly ILogger _logger = loggerProvider.GetLogger();

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        await outboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
