using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler(FinanceTrackingContext financeTrackingContext) : ICommandScheduler
{
    public async Task EnqueueAsync(ICommand command)
    {
        await financeTrackingContext.AddAsync(CreateInternalCommandFrom(command));
        await financeTrackingContext.SaveChangesAsync();
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
