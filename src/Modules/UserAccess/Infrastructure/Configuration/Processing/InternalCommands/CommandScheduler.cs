using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler(UserAccessContext userAccessContext) : ICommandScheduler
{
    public async Task EnqueueAsync(ICommand command)
    {
        await userAccessContext.AddAsync(CreateInternalCommandFrom(command));
        await userAccessContext.SaveChangesAsync();
    }

    private InternalCommand CreateInternalCommandFrom(ICommand command)
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
