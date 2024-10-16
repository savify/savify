using App.Modules.Notifications.Application.Contracts;

namespace App.Modules.Notifications.Application.Configuration.Commands;

public abstract class InternalCommandBase : ICommand
{
    public Guid Id { get; }

    public Guid CorrelationId { get; }

    protected InternalCommandBase(Guid id, Guid correlationId)
    {
        Id = id;
        CorrelationId = correlationId;
    }
}
