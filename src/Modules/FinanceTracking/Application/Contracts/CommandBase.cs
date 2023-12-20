namespace App.Modules.FinanceTracking.Application.Contracts;

public abstract class CommandBase(Guid id) : ICommand
{
    public Guid Id { get; } = id;

    protected CommandBase() : this(Guid.NewGuid())
    {
    }
}

public abstract class CommandBase<TResult>(Guid id) : ICommand<TResult>
{
    public Guid Id { get; } = id;

    protected CommandBase() : this(Guid.NewGuid())
    {
    }
}
