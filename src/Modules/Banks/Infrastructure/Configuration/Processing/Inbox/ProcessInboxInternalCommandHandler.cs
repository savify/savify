using App.BuildingBlocks.Application.Data;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Infrastructure.Configuration.Logging;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxInternalCommandHandler : ICommandHandler<ProcessInboxCommand>
{
    private readonly IMediator _mediator;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger _logger;

    public ProcessInboxInternalCommandHandler(
        IMediator mediator,
        ISqlConnectionFactory sqlConnectionFactory,
        ILoggerProvider loggerProvider)
    {
        _mediator = mediator;
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = loggerProvider.Provide();
    }


    public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        string sql = "SELECT " +
                     $"message.id as {nameof(InboxMessageDto.Id)}, " +
                     $"message.type as {nameof(InboxMessageDto.Type)}, " +
                     $"message.data as {nameof(InboxMessageDto.Data)} " +
                     "FROM banks.inbox_messages AS message " +
                     "WHERE message.processed_date IS NULL " +
                     "ORDER BY message.occurred_on";

        var messages = await connection.QueryAsync<InboxMessageDto>(sql);
        var messagesList = messages.AsList();

        const string sqlUpdateProcessedDate = "UPDATE banks.inbox_messages " +
                                              "SET processed_date = @Date " +
                                              "WHERE id = @Id";

        foreach (var message in messagesList)
        {
            var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name));

            Type type = messageAssembly.GetType(message.Type);
            var request = JsonConvert.DeserializeObject(message.Data, type);

            _logger.Information("Start processing inbox message of type {Type}", type);

            try
            {
                await _mediator.Publish((INotification)request, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Inbox message of type {Type} processing failed; {Message}", type, exception.Message);
                continue;
            }

            await connection.ExecuteAsync(sqlUpdateProcessedDate, new
            {
                Date = DateTime.UtcNow,
                message.Id
            });

            _logger.Information("Inbox message of type {Type} processed successfully", type);
        }
    }
}
