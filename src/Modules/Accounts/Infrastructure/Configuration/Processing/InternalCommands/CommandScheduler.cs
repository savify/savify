using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Accounts.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler : ICommandScheduler
{
    private readonly AccountsContext _accountsContext;

    public CommandScheduler(AccountsContext accountsContext)
    {
        _accountsContext = accountsContext;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        await _accountsContext.AddAsync(CreateInternalCommandFrom(command));
        await _accountsContext.SaveChangesAsync();
    }

    private InternalCommand CreateInternalCommandFrom<T>(ICommand<T> command)
    {
        var internalCommand = new InternalCommand();
        
        internalCommand.Id = command.Id;
        internalCommand.Type = command.GetType().FullName;
        internalCommand.Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        });
        internalCommand.EnqueueDate = DateTime.UtcNow;

        return internalCommand;
    }
}
