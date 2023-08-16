using App.BuildingBlocks.Application.Data;
using App.Modules.Wallets.Application.Configuration.Commands;
using Dapper;
using Newtonsoft.Json;
using Polly;

namespace App.Modules.Wallets.Infrastructure.Configuration.Processing.InternalCommands;

internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public ProcessInternalCommandsCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        string sql = "SELECT " +
                     $"command.id AS {nameof(InternalCommandDto.Id)}, " +
                     $"command.type AS {nameof(InternalCommandDto.Type)}, " +
                     $"command.data AS {nameof(InternalCommandDto.Data)} " +
                     "FROM wallets.internal_commands AS command " +
                     "WHERE command.processed_date IS NULL " +
                     "ORDER BY command.enqueue_date";

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
            var result = await policy.ExecuteAndCaptureAsync(() => ProcessCommand(internalCommand));

            if (result.Outcome == OutcomeType.Failure)
            {
                await connection.ExecuteScalarAsync(
                    "UPDATE wallets.internal_commands " +
                    "SET processed_date = @NowDate, " +
                    "error = @Error " +
                    "WHERE id = @Id",
                    new
                    {
                        NowDate = DateTime.UtcNow,
                        Error = result.FinalException.ToString(),
                        internalCommand.Id
                    });
            }
        }
    }

    private async Task ProcessCommand(InternalCommandDto internalCommand)
    {
        Type type = Assemblies.Application.GetType(internalCommand.Type);
        dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

        await CommandExecutor.Execute(commandToProcess);
    }
}
