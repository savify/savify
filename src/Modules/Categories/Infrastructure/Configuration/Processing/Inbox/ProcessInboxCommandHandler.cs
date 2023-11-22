using App.BuildingBlocks.Infrastructure.Configuration.Inbox;
using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Application.Configuration.Data;
using App.Modules.Categories.Infrastructure.Configuration.Logging;
using Serilog;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxCommandHandler : ICommandHandler<ProcessInboxCommand>
{
    private readonly InboxCommandProcessor _inboxCommandProcessor;

    private readonly ILogger _logger;

    public ProcessInboxCommandHandler(
        InboxCommandProcessor inboxCommandProcessor,
        ILoggerProvider loggerProvider)
    {
        _inboxCommandProcessor = inboxCommandProcessor;
        _logger = loggerProvider.GetLogger();
    }

    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        await _inboxCommandProcessor.Process(DatabaseConfiguration.Schema, _logger, cancellationToken);
    }
}
