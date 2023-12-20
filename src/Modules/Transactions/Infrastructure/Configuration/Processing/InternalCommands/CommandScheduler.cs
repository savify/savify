using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Transactions.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler(TransactionsContext transactionsContext) : ICommandScheduler
{
    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        await transactionsContext.AddAsync(CreateInternalCommandFrom(command));
        await transactionsContext.SaveChangesAsync();
    }

    private InternalCommand CreateInternalCommandFrom<T>(ICommand<T> command)
    {
        var internalCommand = new InternalCommand();

        internalCommand.Id = command.Id;
        internalCommand.Type = command.GetType().FullName!;
        internalCommand.Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        });
        internalCommand.EnqueueDate = DateTime.UtcNow;

        return internalCommand;
    }
}
