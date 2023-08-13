using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Wallets.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler : ICommandScheduler
{
    private readonly WalletsContext _walletsContext;

    public CommandScheduler(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        await _walletsContext.AddAsync(CreateInternalCommandFrom(command));
        await _walletsContext.SaveChangesAsync();
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
