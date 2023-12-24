using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Infrastructure.Configuration.Logging;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;

namespace App.BuildingBlocks.Infrastructure.Configuration.Outbox;

public class OutboxCommandProcessor<TContext>(
    IMediator mediator,
    ISqlConnectionFactory sqlConnectionFactory,
    IDomainNotificationsMapper<TContext> domainNotificationsMapper)
    where TContext : DbContext
{
    public async Task Process(DatabaseSchema schema, ILogger logger, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT
                       message.id as {nameof(OutboxMessageDto.Id)},
                       message.type as {nameof(OutboxMessageDto.Type)},
                       message.data as {nameof(OutboxMessageDto.Data)}
                   FROM {schema.Name}.outbox_messages AS message
                   WHERE message.processed_date IS NULL
                   ORDER BY message.occurred_on
                   """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
        var messagesList = messages.AsList();

        var sqlUpdateProcessedDate = $"UPDATE {schema.Name}.outbox_messages SET processed_date = @Date WHERE id = @Id";

        foreach (var message in messagesList)
        {
            var type = domainNotificationsMapper.GetType(message.Type)!;
            var @event = (JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification)!;

            using (LogContext.Push(new CorrelationIdLogEnricher(@event.CorrelationId)))
            {
                logger.Information("Start processing outbox message {Id} of type {Type}", message.Id, type);

                try
                {
                    await mediator.Publish(@event, cancellationToken);
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "Outbox message {Id} of type {Type} processing failed; {Message}", message.Id, type, exception.Message);
                    throw;
                }

                await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                {
                    Date = DateTime.UtcNow,
                    message.Id
                });

                logger.Information("Outbox message {Id} of type {Type} processed successfully", message.Id, type);
            }
        }
    }
}
