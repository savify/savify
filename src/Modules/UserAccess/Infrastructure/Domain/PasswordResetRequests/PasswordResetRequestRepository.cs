using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure.Domain.PasswordResetRequests;

public class PasswordResetRequestRepository : IPasswordResetRequestRepository
{
    private readonly UserAccessContext _userAccessContext;

    public PasswordResetRequestRepository(UserAccessContext userAccessContext)
    {
        _userAccessContext = userAccessContext;
    }

    public async Task AddAsync(PasswordResetRequest passwordResetRequest)
    {
        await _userAccessContext.AddAsync(passwordResetRequest);
    }

    public async Task<PasswordResetRequest> GetByIdAsync(PasswordResetRequestId id)
    {
        var passwordResetRequest = await _userAccessContext.PasswordResetRequests.SingleOrDefaultAsync(x => x.Id == id);

        if (passwordResetRequest == null)
        {
            throw new NotFoundRepositoryException<PasswordResetRequest>(id.Value);
        }

        return passwordResetRequest;
    }
}
