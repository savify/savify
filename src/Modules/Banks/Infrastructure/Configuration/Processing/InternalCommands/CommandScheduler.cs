using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler : ICommandScheduler
{
    private readonly BanksContext _banksContext;

    public CommandScheduler(BanksContext banksContext)
    {
        _banksContext = banksContext;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        await _banksContext.AddAsync(CreateInternalCommandFrom(command));
        await _banksContext.SaveChangesAsync();
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
