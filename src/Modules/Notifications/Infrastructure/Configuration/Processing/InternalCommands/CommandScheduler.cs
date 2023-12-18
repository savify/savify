using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler(NotificationsContext notificationsContext) : ICommandScheduler
{
    public async Task EnqueueAsync(ICommand command)
    {
        await notificationsContext.AddAsync(CreateInternalCommandFrom(command));
        await notificationsContext.SaveChangesAsync();
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
