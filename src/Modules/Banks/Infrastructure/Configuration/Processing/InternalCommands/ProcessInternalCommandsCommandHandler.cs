using App.BuildingBlocks.Infrastructure.Configuration.InternalCommands;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Configuration.Data;
using Newtonsoft.Json;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.InternalCommands;

internal class ProcessInternalCommandsCommandHandler(InternalCommandProcessor internalCommandProcessor)
    : ICommandHandler<ProcessInternalCommandsCommand>
{
    public async Task Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
    {
        await internalCommandProcessor.Process(DatabaseConfiguration.Schema, ExecuteCommand, cancellationToken);
    }

    private async Task ExecuteCommand(InternalCommandDto internalCommand)
    {
        Type type = Assemblies.Application.GetType(internalCommand.Type)!;
        dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type)!;

        await CommandExecutor.Execute(commandToProcess);
    }
}
