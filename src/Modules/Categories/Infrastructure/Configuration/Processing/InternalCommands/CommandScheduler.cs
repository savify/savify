using App.BuildingBlocks.Infrastructure.InternalCommands;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing.InternalCommands;

public class CommandScheduler(CategoriesContext categoriesContext) : ICommandScheduler
{
    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        await categoriesContext.AddAsync(CreateInternalCommandFrom(command));
        await categoriesContext.SaveChangesAsync();
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
