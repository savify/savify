using MediatR;

namespace App.Modules.Transactions.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
