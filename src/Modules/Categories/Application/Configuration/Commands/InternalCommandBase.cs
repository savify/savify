using App.Modules.Categories.Application.Contracts;

namespace App.Modules.Categories.Application.Configuration.Commands;

public abstract class InternalCommandBase(Guid id, Guid correlationId) : ICommand
{
    public Guid Id { get; } = id;

    public Guid CorrelationId { get; } = correlationId;
}
