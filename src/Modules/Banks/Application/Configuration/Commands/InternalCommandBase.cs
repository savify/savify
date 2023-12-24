using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.Configuration.Commands;

public abstract class InternalCommandBase(Guid id, Guid correlationId) : ICommand
{
    public Guid Id { get; } = id;

    public Guid CorrelationId { get; } = correlationId;
}
