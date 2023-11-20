using App.BuildingBlocks.Infrastructure.Configuration.Inbox;
using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Configuration.Data;
using App.Modules.Notifications.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxCommandHandler : ICommandHandler<ProcessInboxCommand>
{
    private readonly InboxCommandProcessor _inboxCommandProcessor;

    private readonly ILogger _logger;

    public ProcessInboxCommandHandler(
        InboxCommandProcessor inboxCommandProcessor,
        ILoggerProvider loggerProvider)
    {
        _inboxCommandProcessor = inboxCommandProcessor;
        _logger = loggerProvider.Provide();
    }

    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        await _inboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
