using App.BuildingBlocks.Infrastructure.Configuration.Inbox;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxCommandHandler(
    InboxCommandProcessor inboxCommandProcessor,
    ILoggerProvider loggerProvider)
    : ICommandHandler<ProcessInboxCommand>
{
    private readonly ILogger _logger = loggerProvider.GetLogger();

    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        await inboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
