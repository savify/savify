using App.BuildingBlocks.Infrastructure.Configuration.Inbox;
using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Configuration.Data;
using App.Modules.Notifications.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.Processing.Inbox;

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
