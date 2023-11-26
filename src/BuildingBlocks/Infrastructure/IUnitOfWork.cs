using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure;

public interface IUnitOfWork<TContext> where TContext : DbContext
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default, Guid? internalCommandId = null);
}
