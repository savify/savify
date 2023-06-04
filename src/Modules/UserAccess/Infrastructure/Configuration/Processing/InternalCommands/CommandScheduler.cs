using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler : ICommandScheduler
{
    private readonly UserAccessContext _userAccessContext;

    public CommandScheduler(UserAccessContext userAccessContext)
    {
        _userAccessContext = userAccessContext;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        await _userAccessContext.AddAsync(CreateInternalCommandFrom(command));
        await _userAccessContext.SaveChangesAsync();
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
        internalCommand.EnqueueDate = DateTime.Now;

        return internalCommand;
    }
}
