using App.Modules.Categories.Application.Contracts;

namespace App.Modules.Categories.Application.Configuration.Commands;

public abstract class InternalCommandBase : ICommand
{
    public Guid Id { get; }

    public Guid CorrelationId { get; }

    protected InternalCommandBase()
    {
        Id = Guid.NewGuid();
        CorrelationId = Guid.NewGuid();
    }

    protected InternalCommandBase(Guid id, Guid correlationId)
    {
        Id = id;
        CorrelationId = correlationId;
    }
}
