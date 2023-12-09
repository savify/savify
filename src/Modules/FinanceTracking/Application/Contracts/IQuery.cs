using MediatR;

namespace App.Modules.FinanceTracking.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
