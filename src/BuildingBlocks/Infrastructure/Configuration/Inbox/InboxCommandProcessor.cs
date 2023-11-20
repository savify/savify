using App.BuildingBlocks.Application.Data;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace App.BuildingBlocks.Infrastructure.Configuration.Inbox;

public class InboxCommandProcessor
{
    private readonly IMediator _mediator;

    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public InboxCommandProcessor(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
    {
        _mediator = mediator;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task Process(DatabaseSchema schema, ILogger logger, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                    SELECT
                        message.id as {nameof(InboxMessageDto.Id)},
                        message.type as {nameof(InboxMessageDto.Type)},
                        message.data as {nameof(InboxMessageDto.Data)}
                    FROM {schema.Name}.inbox_messages AS message
                    WHERE message.processed_date IS NULL
                    ORDER BY message.occurred_on
                    """;

        var messages = await connection.QueryAsync<InboxMessageDto>(sql);
        var messagesList = messages.AsList();

        var sqlUpdateProcessedDate = $"""
                                      UPDATE {schema.Name}.inbox_messages
                                          SET processed_date = @Date
                                          WHERE id = @Id
                                      """;

        foreach (var message in messagesList)
        {
            var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name));

            Type type = messageAssembly.GetType(message.Type);
            var request = JsonConvert.DeserializeObject(message.Data, type);

            logger.Information("Start processing inbox message of type {Type}", type);

            try
            {
                await _mediator.Publish((INotification)request, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Inbox message of type {Type} processing failed; {Message}", type, exception.Message);
                continue;
            }

            await connection.ExecuteAsync(sqlUpdateProcessedDate, new
            {
                Date = DateTime.UtcNow,
                message.Id
            });

            logger.Information("Inbox message of type {Type} processed successfully", type);
        }
    }
}
