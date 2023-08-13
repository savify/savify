using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Configuration.Commands;

public abstract class InternalCommandBase<TResult> : ICommand<TResult>
{
    public Guid Id { get; }

    protected InternalCommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected InternalCommandBase(Guid id)
    {
        Id = id;
    }
}
