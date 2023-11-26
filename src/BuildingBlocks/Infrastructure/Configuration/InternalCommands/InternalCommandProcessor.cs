using App.BuildingBlocks.Application.Data;
using Dapper;
using Polly;

namespace App.BuildingBlocks.Infrastructure.Configuration.InternalCommands;

public class InternalCommandProcessor
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public InternalCommandProcessor(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task Process(DatabaseSchema schema, Func<InternalCommandDto, Task> executeCommandAction, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT
                       command.id AS {nameof(InternalCommandDto.Id)},
                       command.type AS {nameof(InternalCommandDto.Type)},
                       command.data AS {nameof(InternalCommandDto.Data)}
                   FROM {schema.Name}.internal_commands AS command
                   WHERE command.processed_date IS NULL
                   ORDER BY command.enqueue_date
                   """;

        var commands = await connection.QueryAsync<InternalCommandDto>(sql);
        var internalCommandsList = commands.AsList();

        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            });

        foreach (var internalCommand in internalCommandsList)
        {
            var result = await policy.ExecuteAndCaptureAsync(() => executeCommandAction(internalCommand));

            if (result.Outcome == OutcomeType.Failure)
            {
                await connection.ExecuteScalarAsync(
                    $"UPDATE {schema.Name}.internal_commands SET processed_date = @NowDate, error = @Error WHERE id = @Id",
                    new
                    {
                        NowDate = DateTime.UtcNow,
                        Error = result.FinalException.ToString(),
                        internalCommand.Id
                    });
            }
        }
    }
}
