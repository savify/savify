using MediatR;

namespace App.Modules.Accounts.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
