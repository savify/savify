using MediatR;

namespace App.Modules.UserAccess.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
