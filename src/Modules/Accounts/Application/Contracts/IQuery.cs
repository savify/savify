using MediatR;

namespace App.Modules.Wallets.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
