using App.Modules.FinanceTracking.Application.Contracts;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}
