using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure.Domain.PasswordResetRequests;

public class PasswordResetRequestRepository(UserAccessContext userAccessContext) : IPasswordResetRequestRepository
{
    public async Task AddAsync(PasswordResetRequest passwordResetRequest)
    {
        await userAccessContext.AddAsync(passwordResetRequest);
    }

    public async Task<PasswordResetRequest> GetByIdAsync(PasswordResetRequestId id)
    {
        var passwordResetRequest = await userAccessContext.PasswordResetRequests.SingleOrDefaultAsync(x => x.Id == id);

        if (passwordResetRequest == null)
        {
            throw new NotFoundRepositoryException<PasswordResetRequest>(id.Value);
        }

        return passwordResetRequest;
    }

    public PasswordResetRequest? GetActiveByUserIdOrNullAsync(UserId userId)
    {
        var passwordResetRequests = userAccessContext.PasswordResetRequests.Where(x => x.UserId == userId).ToList();

        return passwordResetRequests.SingleOrDefault(x => x.IsActive);
    }
}
