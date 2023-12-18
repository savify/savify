using App.BuildingBlocks.Infrastructure.Configuration.InternalCommands;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Configuration.Data;
using Newtonsoft.Json;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.InternalCommands;

internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand>
{
    private readonly InternalCommandProcessor _internalCommandProcessor;

    public ProcessInternalCommandsCommandHandler(InternalCommandProcessor internalCommandProcessor)
    {
        _internalCommandProcessor = internalCommandProcessor;
    }

    public async Task Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
    {
        await _internalCommandProcessor.Process(DatabaseConfiguration.Schema, ExecuteCommand, cancellationToken);
    }

    private async Task ExecuteCommand(InternalCommandDto internalCommand)
    {
        Type type = Assemblies.Application.GetType(internalCommand.Type)!;
        dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type)!;

        await CommandExecutor.Execute(commandToProcess);
    }
}
