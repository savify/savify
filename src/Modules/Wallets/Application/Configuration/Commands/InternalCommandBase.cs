using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Configuration.Commands;

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
