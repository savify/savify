using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler : ICommandScheduler
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public CommandScheduler(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task EnqueueAsync(ICommand command)
    {
        await _financeTrackingContext.AddAsync(CreateInternalCommandFrom(command));
        await _financeTrackingContext.SaveChangesAsync();
    }

    private InternalCommand CreateInternalCommandFrom(ICommand command)
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
