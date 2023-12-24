namespace App.Modules.Banks.Application.Contracts;

public abstract class QueryBase<TResult>(Guid id) : IQuery<TResult>
{
    public Guid Id { get; } = id;

    protected QueryBase() : this(Guid.NewGuid())
    {
    }
}
