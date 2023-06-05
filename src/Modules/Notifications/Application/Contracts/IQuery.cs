using MediatR;

namespace App.Modules.Notifications.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
