using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Infrastructure.Configuration.Logging;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
{
    private readonly IMediator _mediator;

    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private readonly IDomainNotificationsMapper<BanksContext> _domainNotificationsMapper;

    private readonly ILogger _logger;

    public ProcessOutboxCommandHandler(
        IMediator mediator,
        ISqlConnectionFactory sqlConnectionFactory,
        IDomainNotificationsMapper<BanksContext> domainNotificationsMapper,
        ILoggerProvider loggerProvider)
    {
        _mediator = mediator;
        _sqlConnectionFactory = sqlConnectionFactory;
        _domainNotificationsMapper = domainNotificationsMapper;
        _logger = loggerProvider.Provide();
    }

    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                      SELECT
                          message.id as {nameof(OutboxMessageDto.Id)},
                          message.type as {nameof(OutboxMessageDto.Type)},
                          message.data as {nameof(OutboxMessageDto.Data)}
                      FROM {DatabaseConfiguration.Schema.Name}.outbox_messages AS message
                      WHERE message.processed_date IS NULL
                      ORDER BY message.occurred_on
                      """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
        var messagesList = messages.AsList();

        var sqlUpdateProcessedDate = $"UPDATE {DatabaseConfiguration.Schema.Name}.outbox_messages SET processed_date = @Date WHERE id = @Id";

        foreach (var message in messagesList)
        {
            var type = _domainNotificationsMapper.GetType(message.Type);
            var @event = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;

            _logger.Information("Start processing outbox message {Id} of type {Type}", message.Id, type);

            try
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Outbox message {Id} of type {Type} processing failed; {Message}", message.Id, type, exception.Message);
                throw;
            }

            await connection.ExecuteAsync(sqlUpdateProcessedDate, new
            {
                Date = DateTime.UtcNow,
                message.Id
            });

            _logger.Information("Outbox message {Id} of type {Type} processed successfully", message.Id, type);
        }
    }
}
