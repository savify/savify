using MediatR;

namespace App.Modules.Banks.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
