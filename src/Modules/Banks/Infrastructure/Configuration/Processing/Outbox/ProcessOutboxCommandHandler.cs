using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler(
    OutboxCommandProcessor<BanksContext> outboxCommandProcessor,
    ILoggerProvider loggerProvider)
    : ICommandHandler<ProcessOutboxCommand>
{
    private readonly ILogger _logger = loggerProvider.GetLogger();

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        await outboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
