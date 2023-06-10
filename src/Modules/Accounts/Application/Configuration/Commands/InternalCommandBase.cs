using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.Configuration.Commands;

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
